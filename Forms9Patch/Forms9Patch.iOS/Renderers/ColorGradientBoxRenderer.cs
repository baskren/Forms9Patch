using CoreGraphics;
using Forms9Patch.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreAnimation;

[assembly: ExportRenderer(typeof(Forms9Patch.ColorGradientBox), typeof(ColorGradientBoxRenderer))]
namespace Forms9Patch.iOS
{
	class ColorGradientBoxRenderer : VisualElementRenderer<ColorGradientBox> {
	
		protected override void OnElementChanged(ElementChangedEventArgs<ColorGradientBox> e)
		{
			base.OnElementChanged(e);
			NativeView.UserInteractionEnabled = false;
		}

		CAGradientLayer _oldSublayer;

		public override void Draw (CGRect rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			//ContentMode = UIViewContentMode.

			var gradient = new CAGradientLayer ();
			if (Element.Orientation == StackOrientation.Horizontal) {
				gradient.StartPoint = new CGPoint (0.0, 0.5);
				gradient.EndPoint = new CGPoint (1.0, 0.5);
			}
			gradient.Frame = rect;
			gradient.Colors = new [] { Element.StartColor.ToCGColor (), Element.EndColor.ToCGColor () };
			if (_oldSublayer!= null)
				NativeView.Layer.ReplaceSublayer(_oldSublayer,gradient);
			else
				NativeView.Layer.InsertSublayer (gradient, 0);
			_oldSublayer = gradient;
			base.Draw (rect);
			//BackgroundColor = Color.Transparent.ToUIColor();
		}

	}
}