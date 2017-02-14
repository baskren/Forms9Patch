// /*******************************************************************
//  *
//  * BackgroundImageEffect.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(Forms9Patch.Droid.BackgroundImageEffect), "BackgroundImageEffect")]
namespace Forms9Patch.Droid
{
	public class BackgroundImageEffect : PlatformEffect
	{
		//IVisualElementRenderer _puRenderer;
		Image _oldImage;
		ImageViewManager _imageViewManager;

		protected override void OnAttached()
		{
			System.Diagnostics.Debug.WriteLine("BackgroundImageEffect ATTACHED!!!!");

			if (_imageViewManager == null)
			{
				//SetNativeControl(new global::Android.Widget.RelativeLayout(Context));
				//SetNativeControl(new F9PRelativeLayout(Context));
				_imageViewManager = new ImageViewManager(Container, (VisualElement)Element);
				_imageViewManager.LayoutComplete += OnBackgroundImageLayoutComplete;
			}
			_oldImage = ((IBackgroundImage)Element)?.BackgroundImage;
			_imageViewManager.LayoutImage(_oldImage);
		}

		protected override void OnDetached()
		{
			if (_oldImage != null)
				_oldImage.PropertyChanged -= OnBackgroundImagePropertyChanged;
			if (_imageViewManager != null)
				_imageViewManager.LayoutComplete -= OnBackgroundImageLayoutComplete;
			_oldImage = null;
			_imageViewManager = null;
		}

		protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(e);
			if (
				e.PropertyName == RoundedBoxBase.OutlineColorProperty.PropertyName
				|| e.PropertyName == RoundedBoxBase.ShadowInvertedProperty.PropertyName
				|| e.PropertyName == RoundedBoxBase.OutlineRadiusProperty.PropertyName
				|| e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
			{
				var box = Element as IRoundedBox;
				if (box != null)
				{
					if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean)
						Container.SetBackgroundDrawable(new RoundRectDrawable(box));
					else
						Container.Background = new RoundRectDrawable(box);
				}
			}
			else if (
			  e.PropertyName == VisualElement.WidthProperty.PropertyName
			  || e.PropertyName == VisualElement.HeightProperty.PropertyName
			  || e.PropertyName == VisualElement.XProperty.PropertyName
			  || e.PropertyName == VisualElement.YProperty.PropertyName
			  || e.PropertyName == RoundedBoxBase.HasShadowProperty.PropertyName
			  || e.PropertyName == RoundedBoxBase.OutlineWidthProperty.PropertyName)
			{

				LayoutCycle();
				//Invalidate();
			}
			else if (e.PropertyName == ContentView.BackgroundImageProperty.PropertyName)
			{
				//System.Diagnostics.Debug.WriteLine ("\t["+text+"]propertyName=[" + e.PropertyName + "] ["+Element.Bounds+"]");

				if (_oldImage != null)
					_oldImage.PropertyChanged -= OnBackgroundImagePropertyChanged;
				_oldImage = ((IBackgroundImage)Element)?.BackgroundImage;
				if (_oldImage != null)
					_oldImage.PropertyChanged += OnBackgroundImagePropertyChanged;
#pragma warning disable 4014
				_imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
			}		
		}


		//bool waitingOnLayout = false;
		void LayoutCycle()
		{
			/*
			if (!waitingOnLayout && Element != null)
			{
				waitingOnLayout = true;
				Device.StartTimer(System.TimeSpan.FromMilliseconds(20), () =>
				{
					var s = Forms.Context.Resources.DisplayMetrics.Density;
					var e = (VisualElement)Element;
					if (e != null)
					{
						var b = e.Bounds;
						if (b.Left == double.NegativeInfinity || b.Width <= 0 || b.Height <= 0)
						{
							waitingOnLayout = false;
							return false;
						}
						//System.Diagnostics.Debug.WriteLine ("LayoutCycle [{0}] [{1}]",b,s);
						Container.Layout((int)(b.Left * s), (int)(b.Top * s), (int)(b.Right * s), (int)(b.Bottom * s));
						//System.Diagnostics.Debug.WriteLine ("\t["+text+"]propertyName=[" + e.PropertyName + "] l=[" + b.Left + "] t=[" + b.Left + "] r=[" + b.Right + "] b=[" + b.Bottom + "] w=[" + b.Width + "] h=[" + b.Height + "]");
						//ViewGroup.InvalidateDrawable (Background);
						//Invalidate();
						//ViewGroup.ForceLayout();
						//ViewGroup.Layout((int)(b.Left * s), (int)(b.Top * s), (int)(b.Right * s), (int)(b.Bottom * s));
						//var box = Element as IRoundedBox;
						//if (box != null)
						//	Background = new RoundRectDrawable(box);
						//var box = Element as IRoundedBox;
						//if (box != null)
						//	Background = new RoundRectDrawable(box);
					}
					waitingOnLayout = false;
					return false;
				});
			}
			*/
		}


		void LayoutImage()
		{

#pragma warning disable 4014
			_imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
		}



		void OnBackgroundImagePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// No Wait or await here because we want RenderBackgroundImage to run in parallel
			if (e.PropertyName == Xamarin.Forms.Image.SourceProperty.PropertyName)
			{
#pragma warning disable 4014
				_imageViewManager.LayoutImage(null);
				_imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
			}
		}

		void OnBackgroundImageLayoutComplete(bool hasImage)
		{
			System.Diagnostics.Debug.WriteLine("Element=[" + Element.GetType() + "] hasImage=[" + hasImage + "]");

			if (!hasImage)
			{
				var roundedBoxElement = Element as IRoundedBox;
				if (roundedBoxElement != null)
				{
					if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean)
						Container.SetBackgroundDrawable(new RoundRectDrawable(roundedBoxElement));
					else
						Container.Background = new RoundRectDrawable(roundedBoxElement);
				}
			}
			else
				Container.SetBackgroundColor(((VisualElement)Element).BackgroundColor.ToAndroid());

			if (Element.GetType() == typeof(Forms9Patch.ContentView))
				Container.SetBackgroundColor(Android.Graphics.Color.Orchid);
		}

	}
}

