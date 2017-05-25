using System;
using UIKit;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Collections.Generic;
using CoreGraphics;
using System.IO;
using System.Threading;
using Foundation;

namespace Forms9Patch.iOS
{
    class ImageViewManager : IDisposable
    {

        static int instances = 0;

        int _instance;
        bool _loading;
        readonly UIView _control;
        //readonly VisualElement _element;
        Xamarin.Forms.ImageSource _source;
        UIImageView _imageView;
        NinePatch _ninePatch;
        bool _firstLoad = true;
        UIImage _sourceImage;
        Forms9Patch.Image _image;

        public VisualElement Element
        {
            get;
            set;
        }

        internal bool Tintable;

        internal ImageViewManager(UIView control, VisualElement element)
        {
            _instance = instances++;
            _control = control;
            Element = element;
        }

        bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                this.ReleaseStreamData(_source);
                if (_ninePatch != null)
                    _ninePatch.RemoveFromSuperview();
                if (_imageView != null)
                    _imageView.RemoveFromSuperview();
                _disposed = true;
            }
        }


        internal bool HasBackgroundImage
        {
            get { return _ninePatch != null || _imageView != null; }
        }

        internal void Draw(CGRect rect)
        {
            try
            {
                if (_ninePatch != null)
                    _ninePatch.Draw(rect);
                if (_imageView != null)
                    _imageView.Frame = rect;
                /*
            if (_imageView != null) {
                if (rect.Equals(_imageView.Frame)) {
                    //System.Diagnostics.Debug.WriteLine("\t\tSame rect");
                    //_imageView.SetNeedsDisplay();
                    _imageView.Draw(rect);
                } else {
                    //System.Diagnostics.Debug.WriteLine("\t\tNew rect");
                    _imageView.Frame = rect;
                }
            }
                */
            }
            catch (ObjectDisposedException e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to Draw child");
                System.Diagnostics.Debug.WriteLine("msg: " + e.Message);
            }
        }

        /// <summary>
        /// Background image rendered handler.
        /// </summary>
        /// <value>
        /// hasImage: was there an image to render?
        /// </value>
        internal delegate void BackgroundImageRenderedHandler(bool hasImage);

        /// <summary>
        /// Attempt to render a background image completed.
        /// </summary>
        internal event BackgroundImageRenderedHandler LayoutComplete;

        /*
		async internal Task<Stream> GetStreamAsync (StreamImageSource source, CancellationToken userToken = default(CancellationToken))
		{
			Stream result;
			if (source.Stream == null) {
				result = null;
			}
			else {
				source.OnLoadingStarted ();
				userToken.Register (new Action (source.CancellationTokenSource.Cancel));
				Stream stream = null;
				try {
					Stream stream2 = await source.Stream (this.CancellationTokenSource.Token);
					stream = stream2;
					this.OnLoadingCompleted (false);
				}
				catch (OperationCanceledException) {
					this.OnLoadingCompleted (true);
					throw;
				}
				result = stream;
			}
			return result;
		}
		*/

        async internal void RelayoutImage(Image image)
        {
            await LayoutImage(null);
            await LayoutImage(image);
        }

        /// <summary>
        /// Images the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void ImagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Forms9Patch.Image.SourceProperty.PropertyName)
                await LayoutImage(_image);
            else if (e.PropertyName == Forms9Patch.Image.OpacityProperty.PropertyName)
            {
                if (_ninePatch != null)
                    _ninePatch.Alpha = (nfloat)_image.Opacity;
                if (_imageView != null)
                    _imageView.Alpha = (nfloat)_image.Opacity;
            }
        }

        static int _iteration;
        async internal Task LayoutImage(Image image)
        {
            int _i = _iteration++;
            if (_loading)
                return;

            //var startLayoutImage = DateTime.Now;
            //TimeSpan delta;

            //var start1 = DateTime.Now;
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

            //UIImage uiImage = null;

            if (ImageViewManagerExtension.Same(_source, newSource))
            {
                LayoutComplete?.Invoke(_ninePatch != null || _imageView != null);
                return;
            }
            else
            {
                if (_source != null)
                {

                    /*
                if (_source != newSource) {
                    if (_source != null) {
                        if (object.Equals (_source, newSource))
                        //TODO: look for CapInset changes before returning
                        return;
                        if (_source is FileImageSource && newSource is FileImageSource && ((FileImageSource)_source).File == ((FileImageSource)newSource).File)
                        //TODO: look for CapInset changes before returning
                        return;
                        */
                    UIView toDispose = null;
                    if (_ninePatch != null)
                        toDispose = _ninePatch;
                    if (_imageView != null)
                        toDispose = _imageView;
                    if (toDispose != null)
                    {
                        Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
                        {
                            toDispose.Alpha -= 0.25f;
                            if (toDispose.Alpha > 0)
                                return true;
                            toDispose.RemoveFromSuperview();
                            toDispose.Alpha = 1;
                            return false;
                        });
                    }
                    this.ReleaseStreamData(_source);
                    _source = null;
                }
                //delta = DateTime.Now-start1;
                //System.Diagnostics.Debug.WriteLine("\t{1}\tD1:\t{0}",delta.TotalSeconds,_i);

                //var start2 = DateTime.Now;
                _loading = true;
                _ninePatch = null;
                _imageView = null;
                IImageSourceHandler handler = null;
                if (newSource != null)
                {
                    if (newSource is FileImageSource)
                        handler = new FileImageSourceHandler();
                    else if (newSource is UriImageSource)
                        handler = new ImageLoaderSourceHandler();
                    else if (newSource is StreamImageSource)
                        handler = new StreamImagesourceHandler();
                }
                //delta = DateTime.Now-start2;
                //System.Diagnostics.Debug.WriteLine("\t{1}\tD2:\t{0}",delta.TotalSeconds,_i);
                //System.Diagnostics.Debug.WriteLine ("\t[B][{0}] [{2}] Subviews=[{1}]",_i,_control.Subviews.Length, Element.BindingContext.GetPropertyValue("Name"));


                if (handler != null)
                {
                    //var start3 = DateTime.Now;
                    ((IElementController)Element).SetValueFromRenderer(Xamarin.Forms.Image.IsLoadingProperty, true);

                    //var start32 = DateTime.Now;
                    var streamSource = newSource as StreamImageSource;
                    //delta = DateTime.Now - start32;
                    //System.Diagnostics.Debug.WriteLine ("\t{1}\tD3.2:\t{0}", delta.TotalSeconds, _i);


                    if (streamSource != null && streamSource.Stream != null)
                    {
                        NSData data = null;
                        //Stream stream = await streamSource.GetStreamAsync(cancelationToken).ConfigureAwait(false);
                        try
                        {
                            //var start33 = DateTime.Now;
                            //Stream stream = await streamSource.Stream (default(CancellationToken));
                            data = await this.FetchResourceData(streamSource, _i);
                            //delta = DateTime.Now - start33;
                            //System.Diagnostics.Debug.WriteLine ("\t{1}\tD3.3:\t{0}", delta.TotalSeconds, _i);
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine("{0} Failed to FetchDataResource.", _i);
                            System.Diagnostics.Debug.WriteLine("msg: " + e.Message);
                        }

                        if (data == null)
                        {
                            try
                            {
                                // not a Forms9Patch stream!
                                //var start34 = DateTime.Now;
                                Stream stream = await streamSource.Stream(default(CancellationToken));
                                data = NSData.FromStream(stream);
                                //delta = DateTime.Now - start34;
                                //System.Diagnostics.Debug.WriteLine ("\t{1}\tD3.4:\t{0}", delta.TotalSeconds, _i);
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine("{0} Failed to NSData.FromStream.", _i);
                                System.Diagnostics.Debug.WriteLine("msg: " + e.Message);
                            }
                        }
                        //var startLoadFromData = DateTime.Now;
                        if (data != null)
                            _sourceImage = UIImage.LoadFromData(data, UIScreen.MainScreen.Scale);
                        //delta = DateTime.Now - startLoadFromData;
                        //System.Diagnostics.Debug.WriteLine ("\t{1}\tLoadFromData:\t{0}", delta.TotalSeconds, _i);
                    }
                    else
                    {
                        try
                        {
                            var startLoadImageAsync = DateTime.Now;
                            _sourceImage = await handler.LoadImageAsync(newSource, default(CancellationToken), (float)UIScreen.MainScreen.Scale);
                            //delta = DateTime.Now - startLoadImageAsync;
                            //System.Diagnostics.Debug.WriteLine ("\t{1}\tLoad-Async:\t{0}", delta.TotalSeconds, _i);
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
                    //delta = DateTime.Now - start3;
                    //System.Diagnostics.Debug.WriteLine ("\t{1}\tD3:\t{0}", delta.TotalSeconds, _i);

                    //var start9 = DateTime.Now;
                    //if (Element != null)
                    //	((IVisualElementController)_element).NativeSizeChanged();
                    if (Element != null)
                        ((IElementController)Element).SetValueFromRenderer(Xamarin.Forms.Image.IsLoadingProperty, false);
                    //delta = DateTime.Now-start9;
                    //System.Diagnostics.Debug.WriteLine("\t{1}\tD9:\t{0}",delta.TotalSeconds,_i);

                }
            }
            if (newSource == null)
            {
                _sourceImage = null;
                _loading = false;
                _ninePatch = null;
                _source = null;
                _imageView = null;
            }
            _control.BackgroundColor = Color.Transparent.ToUIColor();
            //System.Diagnostics.Debug.WriteLine ("\t[C][{0}] [{2}] Subviews=[{1}]",_i,_control.Subviews.Length, Element.BindingContext.GetPropertyValue("Name"));


            if (_sourceImage != null)
            {
                _source = newSource;
                //var start4 = DateTime.Now;
                if (image.TintColor != Color.Default)
                {
                    Tintable = true;
                    //var startImageWithRenderingMode = DateTime.Now;
                    _sourceImage = _sourceImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                    //delta = DateTime.Now - startImageWithRenderingMode;
                    //System.Diagnostics.Debug.WriteLine ("\t{1}\tI-R-Mode:\t{0}", delta.TotalSeconds,_i);
                    _control.TintColor = image.TintColor.ToUIColor();
                }
                else
                {
                    Tintable = false;
                    _sourceImage = _sourceImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                    _control.TintColor = null;
                }
                //delta = DateTime.Now-start4;
                //System.Diagnostics.Debug.WriteLine("\t{1}\tD4:\t{0}",delta.TotalSeconds,_i);

                //var start5 = DateTime.Now;
                bool normalized = (image.CapInsets.Left <= 1 && image.CapInsets.Right <= 1);
                var scale = (float)_source.GetValue(ImageSource.ImageScaleProperty);
                var capsX = new List<Range> {new Range {
                        Start = image.CapInsets.Left * (normalized ?  _sourceImage.CGImage.Width : scale),
                        End = _sourceImage.CGImage.Width - image.CapInsets.Right * (normalized ? _sourceImage.CGImage.Width :  scale)
                    }};
                normalized = (image.CapInsets.Top <= 1 && image.CapInsets.Bottom <= 1);
                var capsY = new List<Range> {new Range {
                        Start = image.CapInsets.Top * (normalized ? _sourceImage.CGImage.Height :  scale),
                        End = _sourceImage.CGImage.Height - image.CapInsets.Bottom * (normalized ? _sourceImage.CGImage.Height :  scale),
                    }};
                //delta = DateTime.Now-start5;
                //Diagnostics.Debug.WriteLine("\t{1}\tD5:\t{0}",delta.TotalSeconds,_i);


                //var startNinePatch = DateTime.Now;

                var imageElement = Element as Forms9Patch.Image;
                if (imageElement != null)
                {
                    imageElement.BaseImageSize = new Size(_sourceImage.CGImage.Width / scale, _sourceImage.CGImage.Height / scale);
                }

                _ninePatch = new NinePatch(_sourceImage, (image.CapInsets.Left >= 0 ? capsX : null), (image.CapInsets.Top >= 0 ? capsY : null));
                //delta = DateTime.Now - startNinePatch;
                //System.Diagnostics.Debug.WriteLine ("\t{1}\tNinePatch:\t{0}", delta.TotalSeconds,_i);

                //var start6 = DateTime.Now;
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

                if (!Settings.IsLicenseValid || _instance >= 4)
                    _control.ContentMode = UIViewContentMode.ScaleAspectFit;

                //delta = DateTime.Now-start6;
                //System.Diagnostics.Debug.WriteLine("\t{1}\tD6:\t{0}",delta.TotalSeconds,_i);

                //_control.SetNeedsDisplay ();
                //var start7 = DateTime.Now;
                if (_firstLoad)
                {
                    if (Element.Height < 0 || Element.Width < 0)
                        await Task.Delay(TimeSpan.FromMilliseconds(20));
                    double hzThickness = 0, vtThickness = 0;
                    var layout = Element as Layout;
                    if (layout != null)
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
                            Element.HeightRequest = _sourceImage.CGImage.Height / Display.Scale - vtThickness;
                    }
                    if (Element.WidthRequest > 0)
                    {
                        if (Element.Width > 0)
                            Element.WidthRequest -= hzThickness;
                    }
                    else
                    {
                        if (Element.Width <= 0)
                            Element.WidthRequest = _sourceImage.CGImage.Width / Display.Scale - hzThickness;
                    }
                }
                //delta = DateTime.Now-start7;
                //System.Diagnostics.Debug.WriteLine("\t{1}\tD7:\t{0}",delta.TotalSeconds,_i);

                //var start8 = DateTime.Now;
                _control.ClipsToBounds = true;
                _firstLoad = false;
                //_updatingCaps = false;
                //delta = DateTime.Now-start8;
                //System.Diagnostics.Debug.WriteLine("\t{1}\tD8:\t{0}",delta.TotalSeconds,_i);


            }

            //var start10 = DateTime.Now;
            LayoutComplete?.Invoke(_ninePatch != null || _imageView != null);
            _loading = false;
            //delta = DateTime.Now-start10;
            //System.Diagnostics.Debug.WriteLine("\t{1}\tD10:\t{0}",delta.TotalSeconds,_i);

            //delta = DateTime.Now - startLayoutImage;
            //System.Diagnostics.Debug.WriteLine ("\t{1}\tTOTAL:\t{0}", delta.TotalSeconds,_i);

            //System.Diagnostics.Debug.WriteLine ("\t[D][{0}] [{2}] Subviews=[{1}]",_i,_control.Subviews.Length, Element.BindingContext.GetPropertyValue("Name"));
        }


    }
}

