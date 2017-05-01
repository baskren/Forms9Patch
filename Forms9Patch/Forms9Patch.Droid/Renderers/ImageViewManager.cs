using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Graphics.Drawables;
using Forms9Patch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Threading;


namespace Forms9Patch.Droid
{
	class ImageViewManager : IDisposable
	{
		static int instances = 0;

		//int _instance;
		bool _fail;
		//bool _loading;
		readonly Android.Views.View _control;
		readonly VisualElement _element;
		Xamarin.Forms.ImageSource _source;
		Android.Widget.ImageView _imageView;
		bool _firstLoad = true;
		Forms9Patch.Image _image;

		string text
		{
			get
			{
				var contentView = _element as Forms9Patch.ContentView;
				if (contentView != null)
				{
					var label = contentView.Content as Xamarin.Forms.Label;
					if (label != null)
						return label.Text;
				}
				return "";
			}
		}

		internal ImageViewManager(Android.Views.View control, VisualElement element)
		{
			//_instance = instances++;
			_control = control;
			_element = element;
		}

		~ImageViewManager()
		{
			System.Diagnostics.Debug.WriteLine("{0}[{1}] ", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
		}

		bool _disposed;
		public void Dispose()
		{
			//System.Diagnostics.Debug.WriteLine("{0}[{1}] ", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
			Dispose(true);
			//GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			//System.Diagnostics.Debug.WriteLine("{0}[{1}] disposing=["+disposing+"]", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
			if (!_disposed && disposing)
			{
				//if (_ninePatch != null)
				//	_ninePatch.RemoveFromSuperview();
				this.ReleaseStreamBitmap(_source);
				var droidViewGroup = _control as global::Android.Views.ViewGroup;
				if (droidViewGroup != null && _imageView != null)
				{
					droidViewGroup.RemoveView(_imageView);
					_imageView = null;
				}
				_image = null;
				_disposed = true;
			}
		}

		internal bool HasBackgroundImage
		{
			get { return /*_ninePatch != null || */_imageView != null; }
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

		async internal void RelayoutImage(Image image)
		{
			await LayoutImage(null);
			await LayoutImage(image);
		}

		void ImagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Xamarin.Forms.Image.SourceProperty.PropertyName)
				LayoutImage(_image);
			else if (e.PropertyName == VisualElement.OpacityProperty.PropertyName)
			{
				if (_imageView != null)
					_imageView.Alpha = (float)_image.Opacity;
			}
		}

		bool working;
		bool waiting;
		int call = 0;
		async internal Task LayoutImage(Image image)
		{
			if (_fail)
				return;
			if (working)
			{
				waiting = true;
				int instance = ++call;
				while (working || instance < call)
				{
					if (instance < call)
						return;
					await Task.Delay(TimeSpan.FromMilliseconds(200));
				}
				waiting = false;
			}
			working = true;


			if (image != _image)
			{
				if (_image != null)
					_image.PropertyChanged -= ImagePropertyChanged;
				_image = image;
				if (_image != null)
					_image.PropertyChanged += ImagePropertyChanged;
			}

			Xamarin.Forms.ImageSource newSource;
			newSource = image?.Source;
			if (image != null && !Settings.IsLicenseValid)
			{
				newSource = Forms9Patch.ImageSource.FromMultiResource("Forms9Patch.Resources.unlicensedcopy");
				//image.Fill = Forms9Patch.Fill.AspectFit;
				_fail = true;
			}

			Drawable drawable = null;

			if (newSource == null || !Equals(_source, newSource))
			{
				var droidViewGroup = _control as global::Android.Views.ViewGroup;
				var droidImageView = _control as global::Android.Widget.ImageView;

				// clean up old image 
				if (droidViewGroup != null && _imageView != null)
				{
					var toDispose = _imageView;
					Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
					{
						toDispose.Alpha -= 0.25f;
						if (toDispose.Alpha > 0 || _disposed)
							return true;
						droidViewGroup.RemoveView(toDispose);
						toDispose.Alpha = 1;
						return false;
					});
				}
				else if (droidImageView != null)
					droidImageView.SetImageBitmap(null);
				this.ReleaseStreamBitmap(_source);
				_source = null;
				_imageView = null;

				if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean)
					_control.SetBackgroundDrawable(null);
				else
					_control.Background = null;

				if (waiting)
				{
					working = false;
					return;
				}

				if (newSource != null)
				{
					// get handler
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

					if (waiting)
					{
						working = false;
						return;
					}

					if (handler != null)
					{
						// load bitmap
						Bitmap bitmap = null;
						((IElementController)_element).SetValueFromRenderer(Xamarin.Forms.Image.IsLoadingProperty, true);
						var streamSource = newSource as StreamImageSource;
						if (streamSource != null && streamSource.Stream != null)
						{

							try
							{
								byte[] data = await this.FetchStreamData(streamSource);
								if (data != null)
									bitmap = BitmapFactory.DecodeByteArray(data, 0, data.Length);
							}
#pragma warning disable 0168
							catch (Exception e)
#pragma warning restore 0168
							{
								//System.Diagnostics.Debug.WriteLine ("{0} Failed to FetchDataResource.");
								//System.Diagnostics.Debug.WriteLine ("msg: " + e.Message);
							}

							if (bitmap == null)
							{
								// not a Forms9Patch stream!
								try
								{
									Stream stream = await streamSource.Stream(default(CancellationToken));
									if (stream != null)
										bitmap = await BitmapFactory.DecodeStreamAsync(stream);//.ConfigureAwait (false);
								}
#pragma warning disable 0168
								catch (Exception e)
#pragma warning restore 0168
								{
									//System.Diagnostics.Debug.WriteLine ("Failed to load StreamImageResource. ");
									//System.Diagnostics.Debug.WriteLine ("msg: " + e.Message);
									working = false;
									return;
								}
							}

						}
						else
						{
							try
							{
								bitmap = await handler.LoadImageAsync(newSource, _control.Context);
							}
#pragma warning disable 0168
							catch (OperationCanceledException e)
#pragma warning restore 0168
							{
								//System.Diagnostics.Debug.WriteLine ("Cancelled LoadImageAsync. ");
								//System.Diagnostics.Debug.WriteLine ("msg: " + e.Message);
								working = false;
								return;
							}
							catch (Exception e)
							{
								System.Diagnostics.Debug.WriteLine(e.Message);
							}
						}

						if (waiting)
						{
							working = false;
							return;
						}

						if (bitmap != null)
						{
							// tint bitmap
							if (image.TintColor != Xamarin.Forms.Color.Default)
							{
								// The matrix is stored in a single array, and its treated as follows: [ a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t ]
								// When applied to a color [r, g, b, a], the resulting color is computed as (after clamping) ;
								//   R' = a*R + b*G + c*B + d*A + e; 
								//   G' = f*R + g*G + h*B + i*A + j; 
								//   B' = k*R + l*G + m*B + n*A + o; 
								//   A' = p*R + q*G + r*B + s*A + t; 
								var filter = new ColorMatrixColorFilter(new ColorMatrix(new float[] {
									0, 0, 0, 0, (float)image.TintColor.R * 255,
									0, 0, 0, 0, (float)image.TintColor.G * 255,
									0, 0, 0, 0, (float)image.TintColor.B * 255,
									0, 0, 0, (float)image.TintColor.A, 0
								}
											));
								var tinted = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);
								var canvas = new Canvas(tinted);
								var paint = new Paint();
								paint.SetColorFilter(filter);
								canvas.DrawBitmap(bitmap, 0, 0, paint);
								bitmap = tinted;
							}

							if (waiting)
							{
								working = false;
								return;
							}

							// get CapInsets
							bool normalized = (image.CapInsets.Left <= 1 && image.CapInsets.Right <= 1);
							var scale = (float)newSource.GetValue(ImageSource.ImageScaleProperty);
							var capsX = new List<Range> {new Range {
									Start = image.CapInsets.Left * (normalized ? bitmap.Width : scale),
									End = bitmap.Width - image.CapInsets.Right * (normalized ? bitmap.Width : scale),
								}
							};
							normalized = (image.CapInsets.Top <= 1 && image.CapInsets.Bottom <= 1);
							var capsY = new List<Range> {new Range {
									Start = image.CapInsets.Top * (normalized ? bitmap.Height : scale),
									End = bitmap.Height - image.CapInsets.Bottom * (normalized ? bitmap.Height : scale),
								}
							};

							if (waiting)
							{
								working = false;
								return;
							}

							// get NinePatch drawable
							NinePatch ninePatch;
							if (image.CapInsets.Left != -1 || image.CapInsets.Right != -1 || image.CapInsets.Top != -1 || image.CapInsets.Bottom != -1)
							{
								//System.Diagnostics.Debug.WriteLine ("\t[" + text + "] ~~~~ A ~~~~");
								drawable = NinePatch.CreateDrawableWithCapInsets(bitmap, (image.CapInsets.Left >= 0 ? capsX : null), (image.CapInsets.Top >= 0 ? capsY : null));
								image.Fill = Fill.Fill;
							}
							else
							{
								//System.Diagnostics.Debug.WriteLine ("\t[" + text + "] ~~~~ B ~~~~");
								ninePatch = bitmap.NinePatch();
								if (ninePatch != null)
								{
									drawable = ninePatch.Drawable();
									image.Fill = Fill.Fill;
									/*
								if (drawable != null) {
									if (image.ContentPadding.Left < 0) {
										double left = -1, right = -1, top = -1, bottom = -1;
										var marginX = ninePatch.Ranges.MarginX;
										if (marginX != null) {
											left = marginX.Start;
											right = marginX.End;
										}								
										var marginY = ninePatch.Ranges.MarginY;
										if (marginY != null) {
											top = marginY.Start;
											bottom = marginY.End;
										}
										if (left + top >= 0)
											((IElementController)_element).SetValueFromRenderer (Image.ContentPaddingProperty, new Thickness (left, top, ninePatch.SourceWidth - right, ninePatch.SourceHeight - bottom));
									}
								}
								*/
								}
							}

							if (waiting)
							{
								working = false;
								return;
							}

							if (droidViewGroup != null)
								_imageView = new Android.Widget.ImageView(global::Android.App.Application.Context);
							else if (droidImageView != null)
								_imageView = droidImageView;

							_imageView.Alpha = (float)image.Opacity;

							if (image.Fill != Fill.Tile)
							{
								if (_imageView != null)
								{
									if (drawable != null)
									{
										_imageView.SetImageDrawable(drawable);
										_imageView.SetScaleType(Android.Widget.ImageView.ScaleType.FitXy);
									}
									else
									{
										_imageView.SetImageBitmap(bitmap);
										switch (image.Fill)
										{
											case Fill.AspectFill:
												_imageView.SetScaleType(Android.Widget.ImageView.ScaleType.CenterCrop);
												break;
											case Fill.AspectFit:
												_imageView.SetScaleType(Android.Widget.ImageView.ScaleType.CenterInside);
												_imageView.SetAdjustViewBounds(true);
												break;
											case Fill.Fill:
												_imageView.SetScaleType(Android.Widget.ImageView.ScaleType.FitXy);
												break;
											case Fill.Tile:
												break;
										}
									}
								}
								else
								{
									working = false;
									LayoutComplete?.Invoke(drawable != null || _imageView != null);
									//_loading = false;
									_firstLoad = false;
									return;
								}
							}
							else
							{
								var resources = global::Android.App.Application.Context.Resources;
								drawable = new BitmapDrawable(resources, bitmap);
								((BitmapDrawable)drawable).SetTileModeXY(Shader.TileMode.Repeat, Shader.TileMode.Repeat);
								_imageView.SetImageDrawable(drawable);
								_imageView.SetScaleType(Android.Widget.ImageView.ScaleType.FitXy);
							}
							var layoutParams = new global::Android.Widget.RelativeLayout.LayoutParams(global::Android.Views.ViewGroup.LayoutParams.MatchParent, global::Android.Views.ViewGroup.LayoutParams.MatchParent);
							if (droidViewGroup != null)
								droidViewGroup.AddView(_imageView, layoutParams);

							if (waiting)
							{
								working = false;
								return;
							}



							if (_firstLoad && image.Fill == Fill.Fill && (_element.HeightRequest > 0 || _element.WidthRequest > 0 || droidImageView != null))
							{
								//System.Diagnostics.Debug.WriteLine ("FIRSTLOAD");
								while (_element.Height < 0 || _element.Width < 0)
									await Task.Delay(TimeSpan.FromMilliseconds(100));
								var layout = _element as Layout;
								double hzThickness = 0, vtThickness = 0;
								if (layout != null)
								{
									hzThickness = layout.Padding.HorizontalThickness;
									vtThickness = layout.Padding.VerticalThickness;
								}
								//System.Diagnostics.Debug.WriteLine ("WR=["+_element.WidthRequest+"] W=["+_element.Width+"] HR=["+_element.HeightRequest+"] H=["+_element.Height+"]");
								if (_element.HeightRequest > 0)
								{
									if (_element.Height > 0)
									{
										_element.HeightRequest -= vtThickness;
										//										System.Diagnostics.Debug.WriteLine ("~~~~ W ~~~~");
									}
								}
								else if (_element.Height <= bitmap.Height / Display.Scale - vtThickness)
								{// || droidImageView != null) {
									_element.HeightRequest = bitmap.Height / Display.Scale - vtThickness;
									//									System.Diagnostics.Debug.WriteLine ("~~~~ X ~~~~");
								}
								if (_element.WidthRequest > 0)
								{
									if (_element.Width > 0)
									{
										_element.WidthRequest -= hzThickness;
										//										System.Diagnostics.Debug.WriteLine ("~~~~ Y ~~~~");
									}
								}
								else if (_element.Width <= bitmap.Width / Display.Scale - hzThickness)
								{ // || droidImageView != null) {
									_element.WidthRequest = bitmap.Width / Display.Scale - hzThickness;
									//									System.Diagnostics.Debug.WriteLine ("~~~~ Z ~~~~");
								}
								//System.Diagnostics.Debug.WriteLine ("WR=["+_element.WidthRequest+"] W=["+_element.Width+"] HR=["+_element.HeightRequest+"] H=["+_element.Height+"]");
							}


							_control.Layout(0, 0, (int)_element.Width, (int)_element.Height);
							_imageView.SetBackgroundColor(Android.Graphics.Color.AliceBlue);
							_imageView.SetBackgroundColor(Android.Graphics.Color.Transparent);
							bitmap.Dispose();
						}

						if (waiting)
						{
							working = false;
							_firstLoad = false;
							return;
						}

						if (_element != null)
						{
							((IElementController)_element).SetValueFromRenderer(Xamarin.Forms.Image.IsLoadingProperty, false);
							((IVisualElementController)_element).NativeSizeChanged();
						}
					}
				}
			}

			working = false;
			LayoutComplete?.Invoke(drawable != null || _imageView != null);
			//_loading = false;
			_firstLoad = false;

		}
	}
}

