using System.ComponentModel;
using Forms9Patch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;



namespace Forms9Patch.Droid
{
	/// <summary>
	/// Forms9Patch Layout renderer.
	/// </summary>
	internal class BubbleLayoutRenderer : ViewRenderer<BubbleLayout,global::Android.Widget.RelativeLayout> 
	{
		//Image _oldImage;
		//ImageViewManager _imageViewManager;

		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<BubbleLayout> e)
		{
			base.OnElementChanged(e);
			if (e.NewElement != null) {
				Background = new BubbleDrawable (e.NewElement);
			}
		}

		bool waitingOnLayout = false;
		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (
				e.PropertyName == RoundedBoxBase.OutlineColorProperty.PropertyName
			    || e.PropertyName == RoundedBoxBase.ShadowInvertedProperty.PropertyName
			    || e.PropertyName == BubbleLayout.PointerAxialPositionProperty.PropertyName
			    || e.PropertyName == RoundedBoxBase.OutlineRadiusProperty.PropertyName
				|| e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName
			    || e.PropertyName == BubbleLayout.PointerTipRadiusProperty.PropertyName
				|| e.PropertyName == BubbleLayout.PointerCornerRadiusProperty.PropertyName 
			    || e.PropertyName == BubbleLayout.PointerAxialPositionProperty.PropertyName) {
				Background = new BubbleDrawable (Element);
			} else if (
				e.PropertyName == BubbleLayout.PointerDirectionProperty.PropertyName
				|| e.PropertyName == VisualElement.WidthProperty.PropertyName
				|| e.PropertyName == VisualElement.HeightProperty.PropertyName
				|| e.PropertyName == VisualElement.XProperty.PropertyName
				|| e.PropertyName == VisualElement.YProperty.PropertyName
				|| e.PropertyName == BubbleLayout.HasShadowProperty.PropertyName
				|| e.PropertyName == RoundedBoxBase.OutlineWidthProperty.PropertyName
				|| e.PropertyName == BubbleLayout.PointerLengthProperty.PropertyName ) {

				/*
				var s = Forms.Context.Resources.DisplayMetrics.Density;
				var b = Element.Bounds;
				Layout ((int)(b.Left * s), (int)(b.Top * s), (int)(b.Right * s), (int)(b.Bottom * s));
				Background = new BubbleDrawable (Element);
				*/

				if (!waitingOnLayout) {
					waitingOnLayout = true;
					Device.StartTimer (System.TimeSpan.FromMilliseconds (25), () => {
						ViewGroup.InvalidateDrawable(Background);
						var s = Forms.Context.Resources.DisplayMetrics.Density;
						var b = Element.Bounds;
						Layout ((int)(b.Left * s), (int)(b.Top * s), (int)(b.Right * s), (int)(b.Bottom * s));
						waitingOnLayout = false;
						return false;
					});
				}

				//ViewGroup.RequestLayout();
				//ViewGroup.ForceLayout ();

			}
		}


	}

}
