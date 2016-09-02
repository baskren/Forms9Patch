using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using PCL.Utils;
using UIKit;
using Foundation;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using CoreGraphics;
using System.ComponentModel;

[assembly: ResolutionGroupName("Forms9Patch")]
//[assembly: ExportEffect(typeof(Forms9Patch.iOS.BackgroundImageEffect), "BackgroundImageEffect")]
namespace Forms9Patch.iOS
{
	public class BackgroundImageEffect : PlatformEffect
	{
		protected override void OnAttached ()
		{
			// there should be called at construction: before there is a source or the layout has happened.

			Element.PropertyChanged += (object sender, PropertyChangedEventArgs e) => {
				var visualElement = Element as VisualElement;
				System.Diagnostics.Debug.WriteLine ("\t[{2}] W,H [{0},{1}]",visualElement.Width,visualElement.Height,e.PropertyName);
				System.Diagnostics.Debug.WriteLine ("\t\t\t [{0}] [{1}] [{2}] [{3}] [{4}]",Container.Frame,Container.Bounds,Container.Hidden,Container.Alpha,Container.Subviews.Length);
			};
		}

		protected override void OnDetached ()
		{
			_imageView?.RemoveFromSuperview ();
			_ninePatch?.RemoveFromSuperview ();
			if (_source != null)
				this.ReleaseStreamData (_source);
			_sourceImage = null;
			_imageView = null;
			_ninePatch = null;
			_source = null;
		}

		protected override void OnElementPropertyChanged (System.ComponentModel.PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged (args);
			var visualElement = Element as VisualElement;
			//System.Diagnostics.Debug.WriteLine ("\t\t\t [{0}] [{1}] [{2}] [{3}] [{4}]",Control?.Frame,Control?.Bounds,Control?.Hidden,Control?.Alpha,Control?.Subviews.Length);
			if (
				args.PropertyName == RoundedBoxBase.OutlineColorProperty.PropertyName
				|| args.PropertyName == RoundedBoxBase.ShadowInvertedProperty.PropertyName
				|| args.PropertyName == RoundedBoxBase.OutlineRadiusProperty.PropertyName
				|| args.PropertyName == VisualElement.BackgroundColorProperty.PropertyName
				//) {
				//_imageViewManager.LayoutImage (_oldImage);
				//} else if (
				|| args.PropertyName == VisualElement.WidthProperty.PropertyName
				|| args.PropertyName == VisualElement.HeightProperty.PropertyName
				|| args.PropertyName == VisualElement.XProperty.PropertyName
				|| args.PropertyName == VisualElement.YProperty.PropertyName
				|| args.PropertyName == RoundedBoxBase.HasShadowProperty.PropertyName
				|| args.PropertyName == RoundedBoxBase.OutlineWidthProperty.PropertyName
			) {
				if (_firstLoad && visualElement.Width > 0 && visualElement.Height > 0 && visualElement.IsVisible) {
					_oldImage = (Image)Element.GetValue (Forms9Patch.ContentView.BackgroundImageProperty);
					LayoutImage (_oldImage);
				} 
				Container.SetNeedsDisplay ();
			} else if (args.PropertyName == ContentView.BackgroundImageProperty.PropertyName) {
				if (_oldImage != null)
					_oldImage.PropertyChanged -= OnBackgroundImagePropertyChanged;
				_oldImage = (Image)Element.GetValue (Forms9Patch.ContentView.BackgroundImageProperty);
				if (_oldImage != null)
					_oldImage.PropertyChanged += OnBackgroundImagePropertyChanged;

				_firstLoad = true;
				if (visualElement.Width > 0 && visualElement.Height > 0 && visualElement.IsVisible)
					LayoutImage (_oldImage);
			}
			if (Element is IRoundedBox)
				Container.BackgroundColor = Color.Transparent.ToUIColor ();
		}

		void OnBackgroundImagePropertyChanged(object sender, PropertyChangedEventArgs e) {
			// No Wait or await here because we want RenderBackgroundImage to run in parallel
			LayoutImage(_oldImage);	
		}




		Forms9Patch.Image _oldImage;

		Xamarin.Forms.ImageSource _source;
		UIImage _sourceImage;
		UIImageView _imageView;
		NinePatch _ninePatch;

		bool _firstLoad = true;
		bool _loading;
		static int _iteration;
		internal bool Tintable;

		async internal Task LayoutImage(Image image) {

			int _i = _iteration++;
			if (_loading)
				return;
			System.Diagnostics.Debug.WriteLine ("\t[A][{0}] [{2}] Subviews=[{1}]",_i,Container.Subviews.Length, Element.BindingContext.GetPropertyValue("Name"));

			//var startLayoutImage = DateTime.Now;
			//TimeSpan delta;

			//var start1 = DateTime.Now;
			Xamarin.Forms.ImageSource newSource = image?.Source;

			//UIImage uiImage = null;

			//if (_source != newSource) {
			if (ImageViewManagerExtension.Same (_source, newSource)) {
				System.Diagnostics.Debug.WriteLine ("\t\t[SAME][{0}] [{2}] Subviews=[{1}]",_i,Container.Subviews.Length, Element.BindingContext.GetPropertyValue("Name"));
				System.Diagnostics.Debug.WriteLine ("\t\t_control [{0}] [{1}] [{2}] [{3}]",Container.Frame,Container.Bounds,Container.Hidden,Container.Alpha);
				if (Container.Subviews.Length > 0)
					System.Diagnostics.Debug.WriteLine ("\t\tsubview0 [{0}] [{1}] [{2}] [{3}]",Container.Subviews[0].Frame,Container.Subviews[0].Bounds,Container.Subviews[0].Hidden,Container.Subviews[0].Alpha);
				//Container.Subviews [0].SetNeedsDisplay ();
				//Container.Subviews [0].BackgroundColor = Color.Pink.ToUIColor ();	
				//Container.BackgroundColor = Color.Teal.ToUIColor();
//				LayoutComplete?.Invoke (_ninePatch != null || _imageView != null);
				//Container.SetNeedsDisplay();
				return;
			} else {
				if (_source != null) {
					//if (object.Equals (_source, newSource))
					//TODO: look for CapInset changes before returning
					//return;
					if (_source is FileImageSource && newSource is FileImageSource && ((FileImageSource)_source).File == ((FileImageSource)newSource).File)
						//TODO: look for CapInset changes before returning
						return;
					UIView toDispose = null;
					if (_ninePatch != null)
						toDispose = _ninePatch;
					if (_imageView != null)
						toDispose = _imageView;
					if (toDispose != null) {
						Device.StartTimer (TimeSpan.FromMilliseconds (10), () => {
							toDispose.Alpha -= 0.25f;
							if (toDispose.Alpha > 0)
								return true;
							toDispose.RemoveFromSuperview ();
							toDispose.Alpha = 1;
							return false;
						});
					}
					this.ReleaseStreamData (_source);
					_source = null;
				}
				//delta = DateTime.Now-start1;
				//System.Diagnostics.Debug.WriteLine("\t{1}\tD1:\t{0}",delta.TotalSeconds,_i);

				//var start2 = DateTime.Now;
				_loading = true;
				_ninePatch = null;
				_imageView = null;
				IImageSourceHandler handler = null;
				if (newSource != null) {
					if (newSource is FileImageSource)
						handler = new FileImageSourceHandler ();
					else if (newSource is UriImageSource)
						handler = new ImageLoaderSourceHandler ();
					else if (newSource is StreamImageSource)
						handler = new StreamImagesourceHandler ();
				} 
				//delta = DateTime.Now-start2;
				//System.Diagnostics.Debug.WriteLine("\t{1}\tD2:\t{0}",delta.TotalSeconds,_i);
				System.Diagnostics.Debug.WriteLine ("\t[B][{0}] [{2}] Subviews=[{1}]",_i,Container.Subviews.Length, Element.BindingContext.GetPropertyValue("Name"));


				if (handler != null) {
					//var start3 = DateTime.Now;
					((IElementController)Element).SetValueFromRenderer (Xamarin.Forms.Image.IsLoadingProperty, true);

					//var start32 = DateTime.Now;
					var streamSource = newSource as StreamImageSource;
					//delta = DateTime.Now - start32;
					//System.Diagnostics.Debug.WriteLine ("\t{1}\tD3.2:\t{0}", delta.TotalSeconds, _i);


					if (streamSource != null && streamSource.Stream != null) {
						NSData data=null;
						//Stream stream = await streamSource.GetStreamAsync(cancelationToken).ConfigureAwait(false);
						try {
							//var start33 = DateTime.Now;
							//Stream stream = await streamSource.Stream (default(CancellationToken));
							data = await this.FetchResourceData  (streamSource,_i);
							//delta = DateTime.Now - start33;
							//System.Diagnostics.Debug.WriteLine ("\t{1}\tD3.3:\t{0}", delta.TotalSeconds, _i);
						} catch (Exception e) {
							System.Diagnostics.Debug.WriteLine ("{0} Failed to FetchDataResource.",_i);
							System.Diagnostics.Debug.WriteLine ("msg: " + e.Message);
						}

						if (data == null) {
							try {
								// not a Forms9Patch stream!
								//var start34 = DateTime.Now;
								Stream stream = await streamSource.Stream (default(CancellationToken));
								data = NSData.FromStream (stream);
								//delta = DateTime.Now - start34;
								//System.Diagnostics.Debug.WriteLine ("\t{1}\tD3.4:\t{0}", delta.TotalSeconds, _i);
							} catch (Exception e) {
								System.Diagnostics.Debug.WriteLine ("{0} Failed to NSData.FromStream.", _i);
								System.Diagnostics.Debug.WriteLine ("msg: " + e.Message);
							}
						}
						//var startLoadFromData = DateTime.Now;
						if (data != null)
							_sourceImage = UIImage.LoadFromData (data, UIScreen.MainScreen.Scale);
						//delta = DateTime.Now - startLoadFromData;
						//System.Diagnostics.Debug.WriteLine ("\t{1}\tLoadFromData:\t{0}", delta.TotalSeconds, _i);
					} else {
						try { 
							var startLoadImageAsync = DateTime.Now;
							_sourceImage = await handler.LoadImageAsync (newSource, default(CancellationToken), (float)UIScreen.MainScreen.Scale); 
							//delta = DateTime.Now - startLoadImageAsync;
							//System.Diagnostics.Debug.WriteLine ("\t{1}\tLoad-Async:\t{0}", delta.TotalSeconds, _i);
						} catch (OperationCanceledException e) {
							System.Diagnostics.Debug.WriteLine ("Cancelled LoadImageAsync. ");
							System.Diagnostics.Debug.WriteLine ("msg: " + e.Message);
						} catch (Exception e) {
							System.Diagnostics.Debug.WriteLine (e.Message);
						}
					}
					//delta = DateTime.Now - start3;
					//System.Diagnostics.Debug.WriteLine ("\t{1}\tD3:\t{0}", delta.TotalSeconds, _i);

					//var start9 = DateTime.Now;
					//if (Element != null)
					//	((IVisualElementController)_element).NativeSizeChanged();
					if (Element != null)
						((IElementController)Element).SetValueFromRenderer (Xamarin.Forms.Image.IsLoadingProperty, false);
					//delta = DateTime.Now-start9;
					//System.Diagnostics.Debug.WriteLine("\t{1}\tD9:\t{0}",delta.TotalSeconds,_i);

				}
			}
			if (newSource == null) {
				_sourceImage = null;
				_loading = false;
				_ninePatch = null;
				_source = null;
				_imageView = null;
			}
			Container.BackgroundColor = Color.Transparent.ToUIColor ();
			System.Diagnostics.Debug.WriteLine ("\t[C][{0}] [{2}] Subviews=[{1}]",_i,Container.Subviews.Length, Element.BindingContext.GetPropertyValue("Name"));


			if (_sourceImage != null) {
				_source = newSource;
				//var start4 = DateTime.Now;
				if (image.TintColor != Color.Default) {
					Tintable = true;
					//var startImageWithRenderingMode = DateTime.Now;
					_sourceImage = _sourceImage.ImageWithRenderingMode (UIImageRenderingMode.AlwaysTemplate);
					//delta = DateTime.Now - startImageWithRenderingMode;
					//System.Diagnostics.Debug.WriteLine ("\t{1}\tI-R-Mode:\t{0}", delta.TotalSeconds,_i);
					Container.TintColor = image.TintColor.ToUIColor ();
				} else {
					Tintable = false;
					_sourceImage = _sourceImage.ImageWithRenderingMode (UIImageRenderingMode.AlwaysOriginal);
					Container.TintColor = null;
				}
				//delta = DateTime.Now-start4;
				//System.Diagnostics.Debug.WriteLine("\t{1}\tD4:\t{0}",delta.TotalSeconds,_i);

				//var start5 = DateTime.Now;
				bool normalized = (image.CapInsets.Left <= 1 && image.CapInsets.Right <= 1);
				var scale = (float)_source.GetValue (ImageSource.ImageScaleProperty);
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
				_ninePatch = new NinePatch (_sourceImage, (image.CapInsets.Left>=0?capsX:null), (image.CapInsets.Top>=0?capsY:null));
				//delta = DateTime.Now - startNinePatch;
				//System.Diagnostics.Debug.WriteLine ("\t{1}\tNinePatch:\t{0}", delta.TotalSeconds,_i);

				//var start6 = DateTime.Now;
				if (_ninePatch.Ranges == null || _ninePatch.Ranges.PatchesX == null || _ninePatch.Ranges.PatchesY == null || _ninePatch.Ranges.PatchesY.Count + _ninePatch.Ranges.PatchesX.Count == 0) {
					if (image.Fill == Fill.Tile) {
						try {
							var tiledImage = _sourceImage.CreateResizableImage (UIEdgeInsets.Zero, UIImageResizingMode.Tile);
							_imageView = new UIImageView (tiledImage);
						} catch (Exception e) {
							System.Diagnostics.Debug.WriteLine(e.Message);
						}
						Container.ContentMode = UIViewContentMode.ScaleToFill;
					} else {
						_imageView = new UIImageView (_sourceImage);
						_imageView.ContentMode = image.Fill.ToUIViewContentMode ();
					}
					_imageView.Frame = new CGRect (0.0, 0.0, Container.Frame.Width, Container.Frame.Height);
					_imageView.ClipsToBounds = true;
					_imageView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
					Container.Add (_imageView);
					Container.SendSubviewToBack (_imageView);
				} else if (image.ContentPadding.Left < 0 )  {
					double left = -1, right = -1, top = -1, bottom = -1;
					var marginX = _ninePatch.Ranges.MarginX;
					if (marginX != null) {
						left = marginX.Start;
						right = marginX.End;
					}								
					var marginY = _ninePatch.Ranges.MarginY;
					if (marginY != null) {
						top = marginY.Start;
						bottom = marginY.End;
					}
					if (left + top >= 0) {
						((IElementController)Element).SetValueFromRenderer (Image.ContentPaddingProperty, new Thickness (left, top, _ninePatch.SourceWidth - right, _ninePatch.SourceHeight - bottom));
						//System.Diagnostics.Debug.WriteLine ("ImageRendere ThicknessSet to " + image.ContentPadding.Description ());
					}
					_ninePatch.Frame = new CGRect (0, 0, Container.Frame.Width, Container.Frame.Height);
					_ninePatch.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
					Container.Add (_ninePatch);
					Container.SendSubviewToBack (_ninePatch);
				}
				//delta = DateTime.Now-start6;
				//System.Diagnostics.Debug.WriteLine("\t{1}\tD6:\t{0}",delta.TotalSeconds,_i);

				//Container.SetNeedsDisplay ();
				//var start7 = DateTime.Now;
				if (_firstLoad) {

					var visualElement = Element as VisualElement;

					if (visualElement.Height < 0 || visualElement.Width < 0)
						await Task.Delay (TimeSpan.FromMilliseconds (20));
					double hzThickness = 0, vtThickness = 0;
					var layout = Element as Layout;
					if (layout != null) {
						hzThickness = layout.Padding.HorizontalThickness;
						vtThickness = layout.Padding.VerticalThickness;
					}
					if (visualElement.HeightRequest > 0) { 
						if (visualElement.Height > 0)
							visualElement.HeightRequest -= vtThickness;
					} else {
						if (visualElement.Height <= 0)
							visualElement.HeightRequest = _sourceImage.CGImage.Height / Display.Scale - vtThickness;
					}
					if (visualElement.WidthRequest > 0) {
						if (visualElement.Width > 0)
							visualElement.WidthRequest -= hzThickness;
					} else {
						if (visualElement.Width <= 0)
							visualElement.WidthRequest = _sourceImage.CGImage.Width / Display.Scale - hzThickness; 
					}
				}
				//delta = DateTime.Now-start7;
				//System.Diagnostics.Debug.WriteLine("\t{1}\tD7:\t{0}",delta.TotalSeconds,_i);

				//var start8 = DateTime.Now;
				Container.ClipsToBounds = true;
				_firstLoad = false;
				//_updatingCaps = false;
				//delta = DateTime.Now-start8;
				//System.Diagnostics.Debug.WriteLine("\t{1}\tD8:\t{0}",delta.TotalSeconds,_i);


			}

			//var start10 = DateTime.Now;
//			LayoutComplete?.Invoke (_ninePatch != null || _imageView != null);
			_loading = false;
			//delta = DateTime.Now-start10;
			//System.Diagnostics.Debug.WriteLine("\t{1}\tD10:\t{0}",delta.TotalSeconds,_i);

			//delta = DateTime.Now - startLayoutImage;
			//System.Diagnostics.Debug.WriteLine ("\t{1}\tTOTAL:\t{0}", delta.TotalSeconds,_i);

			System.Diagnostics.Debug.WriteLine ("\t[D][{0}] [{2}] Subviews=[{1}]",_i,Container.Subviews.Length, Element.BindingContext.GetPropertyValue("Name"));

			//Container.SetNeedsDisplay ();

		}

	}
}

