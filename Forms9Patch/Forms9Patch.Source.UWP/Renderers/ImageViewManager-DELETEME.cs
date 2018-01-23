using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Windows.UI.Xaml.Controls;
using System.IO;
using System.Threading;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;

namespace Forms9Patch.UWP
{
    class ImageViewManager : IDisposable
    {
        static int _instances = 0;
        int _instance;
        bool _loading;
        bool _firstLoad;

        Xamarin.Forms.ImageSource _source;
        Forms9Patch.Image _image;

        Canvas _canvas;
        Windows.UI.Xaml.Controls.Image _uwpImage;

        //Xamarin.Forms.Size _baseImageSize = Size.Zero;
       

        //Windows.UI.Xaml.Media.ImageSource _imageSource;
        BitmapImage _imageSource;

        public VisualElement Element
        {
            get;
            set;
        }

        internal bool Tintable;

        internal ImageViewManager(Canvas canvas, VisualElement element)
        {
            _instance = _instances++;
            _canvas = canvas;
            Element = element;
        }


        internal bool HasBackgroundImage
        {
            get
            {
                return _uwpImage != null;
            }
        }

        internal delegate void BackgroundImageRenderedHandler(bool hasImage);
        internal event BackgroundImageRenderedHandler LayoutComplete;

        async internal void RelayoutImage(Image image)
        {
            await LayoutImage(null);
            await LayoutImage(image);
        }


        async void ImagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Forms9Patch.Image.SourceProperty.PropertyName)
                await LayoutImage(_image);
            else if (e.PropertyName == Forms9Patch.Image.OpacityProperty.PropertyName)
            {
                //if (_ninePatch != null)
                //    _ninePatch.Alpha = _image.Opacity;
                if (_canvas != null)
                    _canvas.Opacity = _image.Opacity;
            }
        }


        static int _iterations;
        async internal Task LayoutImage(Image image)
        {
            int iteration = _iterations++;
            if (_loading)
                return;

            if (image != _image)
            {
                if (_image != null)
                    _image.PropertyChanged -= ImagePropertyChanged;
                _image = image;
                if (_image != null)
                    _image.PropertyChanged += ImagePropertyChanged;
            }

            Xamarin.Forms.ImageSource newSource;
            if (Settings.IsLicenseValid || _instance < 4)
                newSource = image?.Source;
            else
                newSource = Forms9Patch.ImageSource.FromMultiResource("Forms9Patch.Resources.unlicensedcopy");

            if (_source.SameAs(newSource))
            {
                LayoutComplete?.Invoke(/*_ninePatch != null || */ _uwpImage != null);
                return;
            }
            else
            {
                _firstLoad = true;

                if (_uwpImage!=null)
                {
                    _uwpImage.ImageFailed -= OnImageFailed;
                    _uwpImage.ImageOpened -= OnImageOpened;
                }
                if (_source!=null)
                {
                    Windows.UI.Xaml.FrameworkElement toDispose = null;
                    //if (_ninePatch != null)
                    //    toDispose = _ninePatch;
                    if (_uwpImage != null)
                        toDispose = _uwpImage;
                    if (toDispose != null)
                    {
                        Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
                        {
                            toDispose.Opacity -= 0.25f;
                            if (toDispose.Opacity > 0)
                                return true;
                            _canvas.Children.Remove(toDispose);
                            toDispose.Opacity = 1;
                            return false;
                        });
                    }
                    this.ReleaseStreamData(_source);
                    _source = null;

                }
                _loading = true;
                //_ninePath = null;
                _uwpImage = null;
                Xamarin.Forms.Platform.UWP.IImageSourceHandler handler = null;
                if (newSource != null)
                {
                    if (newSource is FileImageSource)
                        handler = new Xamarin.Forms.Platform.UWP.FileImageSourceHandler();
                    else if (newSource is UriImageSource)
                        handler = new Xamarin.Forms.Platform.UWP.UriImageSourceHandler();
                    else if (newSource is StreamImageSource)
                        handler = new Xamarin.Forms.Platform.UWP.StreamImageSourceHandler();
                }
                if (handler!=null)
                {
                    ((IElementController)Element).SetValueFromRenderer(Xamarin.Forms.Image.IsLoadingProperty, true);
                    if (newSource is StreamImageSource streamSource && streamSource.Stream != null)
                    {
                        BitmapImage bitmap = null;
                        try
                        {
                            bitmap = await this.FetchResourceData(streamSource, iteration);
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine("{0} Failed to FetchDataResource.", iteration);
                            System.Diagnostics.Debug.WriteLine("msg: " + e.Message);
                        }
                        /*
                        if (bitmap == null)
                        {
                            try
                            {
                                // not a Forms9Patch stream!
                                //var start34 = DateTime.Now;
                                Windows.Storage.Streams.InMemoryRandomAccessStream memoryStream = new Windows.Storage.Streams.InMemoryRandomAccessStream();
                                Stream stream = await streamSource.Stream(default(CancellationToken));
                                await stream.CopyToAsync(memoryStream.AsStreamForWrite());  //stream.CopyTo(data); // data = NSData.FromStream(stream);
                                //delta = DateTime.Now - start34;
                                //System.Diagnostics.Debug.WriteLine ("\t{1}\tD3.4:\t{0}", delta.TotalSeconds, _i);
                                stream.Dispose();
                                bitmap = new BitmapImage();
                                bitmap.SetSource(memoryStream);
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine("{0} Failed to NSData.FromStream.", iteration);
                                System.Diagnostics.Debug.WriteLine("msg: " + e.Message);
                                bitmap = null;
                            }
                        }
                        */
                        //if (bitmap != null)
                        _imageSource = bitmap;
                    }
                    else
                    {
                        try
                        {
                            var startLoadImageAsync = DateTime.Now;
                            var imageSource =  await handler.LoadImageAsync(newSource, default(CancellationToken));
                            _imageSource = imageSource as BitmapImage;
                            if (imageSource != null && _imageSource == null)
                                throw new Exception("_imageSource should be BitmapImage 1");
                        }
                        catch (OperationCanceledException e)
                        {
                            System.Diagnostics.Debug.WriteLine("Cancelled LoadImageAsync. ");
                            System.Diagnostics.Debug.WriteLine("msg: " + e.Message);
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }
                    }
                    if (Element != null)
                        ((IElementController)Element).SetValueFromRenderer(Xamarin.Forms.Image.IsLoadingProperty, false);


                }
            }

            if (_imageSource != null)
            {
                _uwpImage = new Windows.UI.Xaml.Controls.Image();

                switch (_image.Fill)
                {
                    case Fill.Tile:
                        _uwpImage.HorizontalAlignment = HorizontalAlignment.Left;
                        _uwpImage.VerticalAlignment = VerticalAlignment.Top;
                        break;
                    default:
                        _uwpImage.HorizontalAlignment = HorizontalAlignment.Center;
                        _uwpImage.VerticalAlignment = VerticalAlignment.Center;
                        break;
                }

                _uwpImage.Stretch = _image.Fill.ToStretch();

                _uwpImage.ImageFailed += OnImageFailed;
                _uwpImage.ImageOpened += OnImageOpened;
                _uwpImage.Source = _imageSource;
                _source = newSource;
                _canvas.Children.Add(_uwpImage);
            }
            
            /*
            if (newSource == null)
            {
                _imageSource = null;
                _loading = false;
                //_ninePatch = null;
                _source = null;
                if (_uwpImage!=null)
                {
                    _uwpImage.ImageOpened -= OnImageOpened;
                    _uwpImage.ImageFailed -= OnImageFailed;
                    _uwpImage = null;
                }
            }
            //_control.BackgroundColor = Color.Transparent.ToUIColor();
            _canvas.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Transparent);

            if (_imageSource != null)
            {
                _source = newSource;

                
                //var start4 = DateTime.Now;
                if (image.TintColor != Xamarin.Forms.Color.Default)
                {
                    Tintable = true;

                    var writableBitmap = await _imageSource.ToWritableBitmap();
                    writableBitmap.ForEach((int x, int y, Windows.UI.Color color) =>
                    {
                        if (color.A>0)
                        {
                            color.R = (byte)(image.TintColor.R * 255);
                            color.G = (byte)(image.TintColor.G * 255);
                            color.B = (byte)(image.TintColor.B * 255);
                        }
                        return color;
                    });
                    Windows.Storage.Streams.InMemoryRandomAccessStream memoryStream = null;
                    await writableBitmap.ToStream(memoryStream, BitmapEncoder.BmpEncoderId);
                    var bitmap = new BitmapImage();
                    await bitmap.SetSourceAsync(memoryStream);
                    _imageSource = bitmap;
                }
                //else
                //{
                //    Tintable = false;
                //    _sourceImage = _sourceImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                //    _control.TintColor = null;
                //}
                //delta = DateTime.Now-start4;
                //System.Diagnostics.Debug.WriteLine("\t{1}\tD4:\t{0}",delta.TotalSeconds,_i);

                //var start5 = DateTime.Now;
                bool normalized = (image.CapInsets.Left <= 1 && image.CapInsets.Right <= 1);
                var scale = (float)_source.GetValue(ImageSource.ImageScaleProperty);
                var capsX = new List<Range> {new Range {
                        Start = image.CapInsets.Left * (normalized ?  _imageSource.PixelWidth : scale),
                        End = _imageSource.PixelWidth - image.CapInsets.Right * (normalized ?_imageSource.PixelWidth :  scale)
                    }};
                normalized = (image.CapInsets.Top <= 1 && image.CapInsets.Bottom <= 1);
                var capsY = new List<Range> {new Range {
                        Start = image.CapInsets.Top * (normalized ? _imageSource.PixelHeight :  scale),
                        End = _imageSource.PixelHeight - image.CapInsets.Bottom * (normalized ? _imageSource.PixelHeight :  scale),
                    }};


                if (Element is Forms9Patch.Image imageElement)
                    imageElement.BaseImageSize = new Size(_imageSource.PixelWidth / scale, _imageSource.PixelHeight / scale);

                
                
                _ninePatch = new NinePatch(_sourceImage, (image.CapInsets.Left >= 0 ? capsX : null), (image.CapInsets.Top >= 0 ? capsY : null));

                if (_ninePatch.Ranges == null || _ninePatch.Ranges.PatchesX == null || _ninePatch.Ranges.PatchesY == null || _ninePatch.Ranges.PatchesY.Count + _ninePatch.Ranges.PatchesX.Count == 0)
                {
                    if (image.Fill == Fill.Tile)
                    {
                        try
                        {
                            var tiledImage = _sourceImage.CreateResizableImage(UIEdgeInsets.Zero, UIImageResizingMode.Tile);
                            _imageView = new UIImageView(tiledImage);
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }
                        _control.ContentMode = UIViewContentMode.ScaleToFill;
                    }
                    else
                    {
                        _imageView = new UIImageView(_sourceImage);
                        _imageView.ContentMode = image.Fill.ToUIViewContentMode();
                    }

                    _imageView.Frame = new CGRect(0.0, 0.0, _control.Frame.Width, _control.Frame.Height);
                    _imageView.ClipsToBounds = true;
                    _imageView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                    _imageView.Alpha = (System.nfloat)image.Opacity;
                    _control.Add(_imageView);
                    _control.SendSubviewToBack(_imageView);
                }
                else if (image.ContentPadding.Left < 0)
                {
                    double left = -1, right = -1, top = -1, bottom = -1;
                    var marginX = _ninePatch.Ranges.MarginX;
                    if (marginX != null)
                    {
                        left = marginX.Start;
                        right = marginX.End;
                    }
                    var marginY = _ninePatch.Ranges.MarginY;
                    if (marginY != null)
                    {
                        top = marginY.Start;
                        bottom = marginY.End;
                    }
                    if (left + top >= 0)
                    {
                        ((IElementController)Element).SetValueFromRenderer(Image.ContentPaddingProperty, new Thickness(left, top, _ninePatch.SourceWidth - right, _ninePatch.SourceHeight - bottom));
                        //System.Diagnostics.Debug.WriteLine ("ImageRendere ThicknessSet to " + image.ContentPadding.Description ());
                    }
                    //_ninePatch.Frame = new CGRect (_control.Frame.X, _control.Frame.Y, _control.Frame.Width, _control.Frame.Height);
                    _ninePatch.Frame = new CGRect(0, 0, 0, 0);
                    //_ninePatch.Frame = new CGRect(
                    //_control.Frame = _ninePatch.Frame;
                    _ninePatch.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                    _ninePatch.Alpha = (nfloat)image.Opacity;
                    _control.Add(_ninePatch);
                    _control.SendSubviewToBack(_ninePatch);
                }
                

                //if (!Settings.IsLicenseValid || _instance >= 4)
                //    _control.ContentMode = UIViewContentMode.ScaleAspectFit;

                //delta = DateTime.Now-start6;
                //System.Diagnostics.Debug.WriteLine("\t{1}\tD6:\t{0}",delta.TotalSeconds,_i);

                //_control.SetNeedsDisplay ();
                //var start7 = DateTime.Now;
                if (_firstLoad)
                {
                    if (Element.Height < 0 || Element.Width < 0)
                        await Task.Delay(TimeSpan.FromMilliseconds(20));
                    double hzThickness = 0, vtThickness = 0;
                    if (Element is Layout layout)
                    {
                        hzThickness = layout.Padding.HorizontalThickness;
                        vtThickness = layout.Padding.VerticalThickness;
                    }
                    if (Element.HeightRequest > 0)
                    {
                        if (Element.Height > 0)
                            Element.HeightRequest -= vtThickness;
                    }
                    else
                    {
                        if (Element.Height <= 0)
                            Element.HeightRequest = _imageSource.PixelHeight / FormsGestures.Display.Scale - vtThickness;
                    }
                    if (Element.WidthRequest > 0)
                    {
                        if (Element.Width > 0)
                            Element.WidthRequest -= hzThickness;
                    }
                    else
                    {
                        if (Element.Width <= 0)
                            Element.WidthRequest = _imageSource.PixelWidth / FormsGestures.Display.Scale - hzThickness;
                    }
                }
                //_control.ClipsToBounds = true;
                _firstLoad = false;
            }

            LayoutComplete?.Invoke(_ninePatch != null || _uwpImage != null);
            _loading = false;
            */
        }

        private void OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void OnImageOpened(object sender, RoutedEventArgs e)
        {
            //_baseImageSize = new Size(_imageSource.PixelWidth, _imageSource.PixelHeight);

            if (_image.TintColor != Xamarin.Forms.Color.Default)
            {
                Tintable = true;
                /*
                var writableBitmap = await _imageSource.ToWritableBitmap();

                if (writableBitmap != null)
                {

                    writableBitmap.ForEach((int x, int y, Windows.UI.Color color) =>
                    {
                        if (color.A > 0)
                        {
                            color.R = (byte)(_image.TintColor.R * 255);
                            color.G = (byte)(_image.TintColor.G * 255);
                            color.B = (byte)(_image.TintColor.B * 255);
                        }
                        return color;
                    });
                    Windows.Storage.Streams.InMemoryRandomAccessStream memoryStream = null;
                    await writableBitmap.ToStream(memoryStream, BitmapEncoder.BmpEncoderId);
                    var bitmap = new BitmapImage();
                    await bitmap.SetSourceAsync(memoryStream);
                    _imageSource = bitmap;
                }
                */
            }

            bool normalized = (_image.CapInsets.Left <= 1 && _image.CapInsets.Right <= 1);
            var scale = (float)_source.GetValue(ImageSource.ImageScaleProperty);
            var capsX = new List<Range> {new Range {
                        Start = _image.CapInsets.Left * (normalized ?  _imageSource.PixelWidth : scale),
                        End = _imageSource.PixelWidth - _image.CapInsets.Right * (normalized ?_imageSource.PixelWidth :  scale)
                    }};
            normalized = (_image.CapInsets.Top <= 1 && _image.CapInsets.Bottom <= 1);
            var capsY = new List<Range> {new Range {
                        Start = _image.CapInsets.Top * (normalized ? _imageSource.PixelHeight :  scale),
                        End = _imageSource.PixelHeight - _image.CapInsets.Bottom * (normalized ? _imageSource.PixelHeight :  scale),
                    }};


            if (Element is Forms9Patch.Image imageElement)
                imageElement.BaseImageSize = new Size(_imageSource.PixelWidth / scale, _imageSource.PixelHeight / scale);

            if (_firstLoad)
            {
                if (Element.Height < 0 || Element.Width < 0)
                    await Task.Delay(TimeSpan.FromMilliseconds(20));
                double hzThickness = 0, vtThickness = 0;
                if (Element is Layout layout)
                {
                    hzThickness = layout.Padding.HorizontalThickness;
                    vtThickness = layout.Padding.VerticalThickness;
                }
                if (Element.HeightRequest > 0)
                {
                    if (Element.Height > 0)
                        Element.HeightRequest -= vtThickness;
                }
                else
                {
                    if (Element.Height <= 0)
                        Element.HeightRequest = _imageSource.PixelHeight / FormsGestures.Display.Scale - vtThickness;
                }
                if (Element.WidthRequest > 0)
                {
                    if (Element.Width > 0)
                        Element.WidthRequest -= hzThickness;
                }
                else
                {
                    if (Element.Width <= 0)
                        Element.WidthRequest = _imageSource.PixelWidth / FormsGestures.Display.Scale - hzThickness;
                }
            }
            //_control.ClipsToBounds = true;
            _firstLoad = false;

            LayoutComplete?.Invoke(true);
            _loading = false;

            _canvas.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Bisque);

        }





        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ImageViewManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
