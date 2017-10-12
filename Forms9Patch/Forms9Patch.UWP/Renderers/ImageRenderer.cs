using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Forms9Patch.Image), typeof(Forms9Patch.UWP.ImageRenderer))]
namespace Forms9Patch.UWP
{
    public class ImageRenderer : ViewRenderer<Image, ImageView>
    {
        #region Fields
        bool _disposed;

        bool _debugMessages=true;

        static int _instances;
        int _instance;
        #endregion

        #region Constructor / disposal
        public ImageRenderer()
        {
            _instance = _instances++;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Control?.Dispose();
                SetNativeControl(null);
                _disposed = true;
            }

            base.Dispose(disposing);
        }
        #endregion


        #region Change managements
        protected override async void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer["+_instance+"].OnElementChanged(["+e.OldElement+", "+e.NewElement+"]");
            base.OnElementChanged(e);

            /*
            if (e.OldElement != null && Control != null)
                Control.ImageElement = null;

            if (e.NewElement != null)
            {
                //e.NewElement.SizeChanged += OnSizeChanged;
                if (Control == null)
                    SetNativeControl(new ImageView(_instance));

                await TryUpdateSource();
                UpdateAspect();
                UpdateCapInsets();
            }
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer[" + _instance + "].OnElementChanged([" + e.OldElement + ", " + e.NewElement + "] RETURN");
            */
            if (e.NewElement != null)
            {
                if (Control == null)
                    SetNativeControl(new ImageView(_instance));
                Control.ImageElement = e.NewElement;
            }
        }

        /*
        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer[" + _instance + "].OnElementPropertyChanged([" + e.PropertyName+"]");
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Image.SourceProperty.PropertyName)
                await TryUpdateSource();

            //if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer[" + _instance + "].OnElementPropertyChanged([" + e.PropertyName + "] RETURN");
        }
        */
        
        void RefreshImage()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer[" + _instance + "].RefreshImage()");
            ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.RendererReady);
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer[" + _instance + "].RefreshImage() RETURN ");
        }

        /*
        protected virtual async Task TryUpdateSource()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer[" + _instance + "].TryUpdateSource()");
            // By default we'll just catch and log any exceptions thrown by UpdateSource so we don't bring down
            // the application; a custom renderer can override this method and handle exceptions from
            // UpdateSource differently if it wants to

            try
            {
                await UpdateSource().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(nameof(ImageRenderer), "ImageRenderer[" + _instance + "].TryUpdateSource() Error loading image: {0}", ex);
            }
            finally
            {
                ((IImageController)Element)?.SetIsLoading(false);
            }
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer[" + _instance + "].TryUpdateSource() RETURN");
        }

        /*
        protected async Task UpdateSource()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer[" + _instance + "].UpdateSource()");
            if (_disposed || Element == null || Control == null)
            {
                System.Diagnostics.Debug.WriteLine("ImageRenderer[" + _instance + "].UpdateSource() RETURN");
                return;
            }
           
            Stopwatch stopwatch = Stopwatch.StartNew();

            //Element.SetIsLoading(true);
            ((IElementController)Element).SetValueFromRenderer(Xamarin.Forms.Image.IsLoadingProperty, true);

            Xamarin.Forms.ImageSource source = Element.Source;

            await Control.SetSourceAsync(source);
            
            RefreshImage();



            //Element.SetIsLoading(false);
            ((IElementController)Element).SetValueFromRenderer(Xamarin.Forms.Image.IsLoadingProperty, false);

            stopwatch.Stop();
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer[" + _instance + "].UpdateSource() RETURN elapsedTime=["+stopwatch.ElapsedMilliseconds+"]");

        }

        
        Windows.Foundation.Size _lastFinalSize = Windows.Foundation.Size.Empty;
        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer[" + _instance + "].ArrangeOverride(" + finalSize+") ENTER/RETURN");

            if (_lastFinalSize!=finalSize && Element.Fill == Fill.Tile && finalSize.Width > 0 && finalSize.Height > 0 && !Double.IsInfinity(finalSize.Width) && !Double.IsInfinity(finalSize.Height) && !Double.IsNaN(finalSize.Width) && !Double.IsNaN(finalSize.Height))
                Control.GenerateImageLayout(finalSize);
            _lastFinalSize = finalSize;
            return base.ArrangeOverride(finalSize);
        }
        

        /*
        Xamarin.Forms.Size _lastXfSize = Xamarin.Forms.Size.Zero;
        private void OnSizeChanged(object sender, EventArgs e)
        {
            var size = Element.Bounds.Size;
            if (_lastXfSize != size && Element.Fill == Fill.Tile && size.Width > 0 && size.Height > 0 && !Double.IsInfinity(size.Width) && !Double.IsInfinity(size.Height) && !Double.IsNaN(size.Width) && !Double.IsNaN(size.Height))
                Control.GenerateLayout(new Windows.Foundation.Size(size.Width, size.Height));
            _lastXfSize = size;
        }
        */


        #endregion
    }
}
