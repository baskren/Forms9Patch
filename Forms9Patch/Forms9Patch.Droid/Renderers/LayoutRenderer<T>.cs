using System.ComponentModel;
using Forms9Patch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;



namespace Forms9Patch.Droid
{
	/// <summary>
	/// Forms9Patch Layout renderer.
	/// </summary>
	public class LayoutRenderer<TElement> : ViewRenderer<TElement,global::Android.Widget.RelativeLayout> where TElement : View, IBackgroundImage 
	//public class LayoutRenderer<TElement> : ViewRenderer<TElement, F9PRelativeLayout> where TElement : View, IBackgroundImage
	{
		Image _oldImage;
		ImageViewManager _imageViewManager;

		string text {
			get {
				var contentView = Element as Forms9Patch.ContentView;
				if (contentView != null) {
					var label = contentView.Content as Xamarin.Forms.Label;
					if (label != null)
						return label.Text;
				}
				return "";
			}
		}


		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<TElement> e)
		{
			base.OnElementChanged(e);
			if (e.NewElement != null) {
				if (_imageViewManager == null) {
					SetNativeControl(new global::Android.Widget.RelativeLayout(Context));
					//SetNativeControl(new F9PRelativeLayout(Context));
					_imageViewManager = new ImageViewManager (Control, e.NewElement);
					_imageViewManager.LayoutComplete += OnBackgroundImageLayoutComplete;
				}
				_oldImage = e.NewElement.BackgroundImage;
#pragma warning disable 4014
				_imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
			} else {
				_imageViewManager.LayoutComplete -= OnBackgroundImageLayoutComplete;
				_imageViewManager.Dispose ();
				_imageViewManager = null;
			}
		}

		bool waitingOnLayout = false;
		//bool waitingOnInvalidate = false;
		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine ("LayoutRenderer<>.OnElementPropertyChanged txt=["+text+"] w=["+Element.Width+"] h=["+Element.Height+"] propertyName=["+e.PropertyName+"]");
			base.OnElementPropertyChanged(sender, e);
			if (
				e.PropertyName == RoundedBoxBase.OutlineColorProperty.PropertyName
				|| e.PropertyName == RoundedBoxBase.ShadowInvertedProperty.PropertyName
				|| e.PropertyName == RoundedBoxBase.OutlineRadiusProperty.PropertyName
				|| e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName) {
				var box = Element as IRoundedBox;
				if (box != null)
					Background = new RoundRectDrawable (box);
			} else if (
				e.PropertyName == VisualElement.WidthProperty.PropertyName
				|| e.PropertyName == VisualElement.HeightProperty.PropertyName
				|| e.PropertyName == VisualElement.XProperty.PropertyName
				|| e.PropertyName == VisualElement.YProperty.PropertyName
				|| e.PropertyName == RoundedBoxBase.HasShadowProperty.PropertyName
				|| e.PropertyName == RoundedBoxBase.OutlineWidthProperty.PropertyName) {
				
				LayoutCycle ();
				//Invalidate();
			} else if (e.PropertyName == ContentView.BackgroundImageProperty.PropertyName) {
				//System.Diagnostics.Debug.WriteLine ("\t["+text+"]propertyName=[" + e.PropertyName + "] ["+Element.Bounds+"]");

				if (_oldImage != null)
					_oldImage.PropertyChanged -= OnBackgroundImagePropertyChanged;
				_oldImage = Element.BackgroundImage;
				if (_oldImage != null)
					_oldImage.PropertyChanged += OnBackgroundImagePropertyChanged;
#pragma warning disable 4014
				_imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
			} else if (e.PropertyName == "\t["+text+"]Renderer") {
				/*
				if (Background != null && !waitingOnInvalidate) {
					waitingOnInvalidate = true;
					Device.StartTimer (System.TimeSpan.FromMilliseconds (50), () => {
						ViewGroup.InvalidateDrawable (Background);
						//ViewGroup.Invalidate();
						waitingOnInvalidate = false;
						return false;
					});
				}
*/
				//LayoutCycle ();
				//_imageViewManager.RelayoutImage (_oldImage);
				/*
				_imageViewManager = new ImageViewManager (Control, Element);
				_imageViewManager.LayoutComplete += OnBackgroundImageLayoutComplete;
				var s = Forms.Context.Resources.DisplayMetrics.Density;
				var b = Element.Bounds;
				Layout ((int)(b.Left * s), (int)(b.Top * s), (int)(b.Right * s), (int)(b.Bottom * s));
				System.Diagnostics.Debug.WriteLine ("\tpropertyName=[" + e.PropertyName + "] txt=["+text+"] l=[" + b.Left + "] t=[" + b.Left + "] r=[" + b.Right + "] b=[" + b.Bottom + "] w=[" + b.Width + "] h=[" + b.Height + "]");
				if (Background != null)
					ViewGroup.InvalidateDrawable (Background);
					*/
			}

		}

		void LayoutCycle() {
			/*
			System.Diagnostics.Debug.WriteLine("LayoutCycle Element=[" + Element.GetType() + "] waiting=["+waitingOnLayout+"]");
			if (!waitingOnLayout && Element != null) {
				waitingOnLayout = true;
				Device.StartTimer (System.TimeSpan.FromMilliseconds (20), () => {
					var s = Forms.Context.Resources.DisplayMetrics.Density;
					var e = Element;
					if (e!=null) {
						var b = e.Bounds;
						if (b.Left==double.NegativeInfinity || b.Width <= 0 || b.Height <= 0) {
							waitingOnLayout = false;
							System.Diagnostics.Debug.WriteLine("\tLayoutCycle A  Element=[" + Element.GetType() + "]");
							return false;
						}
						//System.Diagnostics.Debug.WriteLine ("LayoutCycle [{0}] [{1}]",b,s);
						Layout ((int)(b.Left * s), (int)(b.Top * s), (int)(b.Right * s), (int)(b.Bottom * s));
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
					System.Diagnostics.Debug.WriteLine("\tLayoutCycle B  Element=[" + Element.GetType() + "]");
					return false;
				});
			}
			*/
		}


		void LayoutImage() {

#pragma warning disable 4014
			_imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
		}



		void OnBackgroundImagePropertyChanged(object sender, PropertyChangedEventArgs e) {
			// No Wait or await here because we want RenderBackgroundImage to run in parallel
			if (e.PropertyName == Xamarin.Forms.Image.SourceProperty.PropertyName) {
#pragma warning disable 4014
				_imageViewManager.LayoutImage(null);
				_imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
			}
		}
			
		void OnBackgroundImageLayoutComplete(bool hasImage) {
			//System.Diagnostics.Debug.WriteLine("OnBackgroundImageLayoutComplete Element=["+Element.GetType()+"] hasImage=["+hasImage+"]");

			if (!hasImage) {
				var roundedBoxElement = Element as IRoundedBox;
				if (roundedBoxElement != null)
					Background = new RoundRectDrawable (roundedBoxElement);
			} else
				SetBackgroundColor (Element.BackgroundColor.ToAndroid ());

			//if (Element.GetType() == typeof(Forms9Patch.ContentView))
			//	SetBackgroundColor(Android.Graphics.Color.Orchid);
		}

		/*
		protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime) {
			System.Diagnostics.Debug.WriteLine ("");
			return base.DrawChild (canvas, child, drawingTime);
		}
		*/
	}

}
