using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Forms9Patch.Image), typeof(Forms9Patch.UWP.ImageRenderer))]
namespace Forms9Patch.UWP
{
    public class ImageRenderer : ViewRenderer<Image, Windows.UI.Xaml.Controls.Image>
    {
        bool _measured;
        bool _disposed;

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (Control?.Source is WriteableBitmap bitmap)
            {
                if (bitmap.PixelWidth < 1 || bitmap.PixelHeight < 1)
                    return new SizeRequest();
                _measured = true;
                Size result = new Size { Width = bitmap.PixelWidth, Height = bitmap.PixelHeight };
                if (Element.Fill != Fill.None)
                {
                    if (!Double.IsInfinity(widthConstraint))
                        result.Width = Math.Max(bitmap.PixelWidth, widthConstraint);
                    if (!Double.IsInfinity(heightConstraint))
                        result.Height = Math.Max(bitmap.PixelHeight, heightConstraint);
                }
                return new SizeRequest(result);
            }
            else
                return new SizeRequest();
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (disposing)
            {
                if (Control != null)
                {
                    Control.ImageOpened -= OnImageOpened;
                    Control.ImageFailed -= OnImageFailed;
                }
            }

            base.Dispose(disposing);
        }

        protected override async void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var image = new Windows.UI.Xaml.Controls.Image();
                    image.ImageOpened += OnImageOpened;
                    image.ImageFailed += OnImageFailed;
                    SetNativeControl(image);
                }

                await TryUpdateSource();
                UpdateAspect();
            }
        }

        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Image.SourceProperty.PropertyName)
                await TryUpdateSource();
            else if (e.PropertyName == Image.AspectProperty.PropertyName)
                UpdateAspect();
            else if (e.PropertyName == Image.FillProperty.PropertyName)
                UpdateAspect();
        }

        /*
        static Stretch GetStretch(Aspect aspect)
        {
            switch (aspect)
            {
                case Aspect.Fill:
                    return Stretch.Fill;
                case Aspect.AspectFill:
                    return Stretch.UniformToFill;
                default:
                case Aspect.AspectFit:
                    return Stretch.Uniform;
            }
        }
        */

        void OnImageOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_measured)
                RefreshImage();
            ((IElementController)Element).SetValueFromRenderer(Xamarin.Forms.Image.IsLoadingProperty, false);
        }

        protected virtual void OnImageFailed(object sender, ExceptionRoutedEventArgs exceptionRoutedEventArgs)
        {
            System.Diagnostics.Debug.WriteLine ("Image Loading", $"Image failed to load: {exceptionRoutedEventArgs.ErrorMessage}");
            ((IElementController)Element).SetValueFromRenderer(Xamarin.Forms.Image.IsLoadingProperty, false);
        }

        void RefreshImage()
        {
            System.Diagnostics.Debug.WriteLine("RefreshImage");
            ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.RendererReady);
        }

        void UpdateAspect()
        {
            if (_disposed || Element == null || Control == null)
            {
                return;
            }

            if (Element.Fill == Fill.Tile || Element.Fill == Fill.None)
            {
                Control.HorizontalAlignment = HorizontalAlignment.Left;
                Control.VerticalAlignment = VerticalAlignment.Top;
            }
            else
            {
                Control.HorizontalAlignment = HorizontalAlignment.Center;
                Control.VerticalAlignment = VerticalAlignment.Center;
            }

            if (Control.NineGrid.IsANineGrid())
                Control.Stretch = Stretch.Fill;
            else
                Control.Stretch = Element.Fill.ToStretch();

        }

        protected virtual async Task TryUpdateSource()
        {
            // By default we'll just catch and log any exceptions thrown by UpdateSource so we don't bring down
            // the application; a custom renderer can override this method and handle exceptions from
            // UpdateSource differently if it wants to

            try
            {
                await UpdateSource().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(nameof(ImageRenderer), "Error loading image: {0}", ex);
            }
            finally
            {
                ((IImageController)Element)?.SetIsLoading(false);
            }
        }

        protected async Task UpdateSource()
        {
            if (_disposed || Element == null || Control == null)
            {
                return;
            }

            //Element.SetIsLoading(true);
            ((IElementController)Element).SetValueFromRenderer(Xamarin.Forms.Image.IsLoadingProperty, true);

            Xamarin.Forms.ImageSource source = Element.Source;

            Xamarin.Forms.Platform.UWP.IImageSourceHandler handler = null;
            if (source != null)
            {
                if (source is FileImageSource)
                    handler = new FileImageSourceHandler();
                else if (source is UriImageSource)
                    handler = new UriImageSourceHandler();
                else if (source is StreamImageSource)
                    handler = new StreamImageSourceHandler();
            }
            if (handler!=null)
            {
                Windows.UI.Xaml.Media.ImageSource imagesource;
                try
                {
                    imagesource = await handler.LoadImageAsync(source);
                }
                catch (OperationCanceledException)
                {
                    imagesource = null;
                }
                // In the time it takes to await the imagesource, some zippy little app
                // might have disposed of this Image already.
                var thickness = new Windows.UI.Xaml.Thickness(-1, -1, -1, -1);
                if (imagesource is WriteableBitmap bitmap)
                {
                    Element.BaseImageSize = new Size(bitmap.PixelWidth, bitmap.PixelHeight);
                    thickness = bitmap.NineGridThickness();
                    if (thickness.IsANineGrid())
                    {
                        // remove the 1 pixel border
                        bitmap = bitmap.Crop(1, 1, bitmap.PixelWidth - 2, bitmap.PixelHeight - 2);
                        imagesource = bitmap;
                    }
                }
                
                Control.Source = imagesource;
                    RefreshImage();

                if (thickness.IsANineGrid())
                    Control.NineGrid = thickness;
                UpdateAspect();
                RefreshImage();

            }
            else
                Control.Source = null;
            //Element.SetIsLoading(false);
            ((IElementController)Element).SetValueFromRenderer(Xamarin.Forms.Image.IsLoadingProperty, false);

        }
    }
}
