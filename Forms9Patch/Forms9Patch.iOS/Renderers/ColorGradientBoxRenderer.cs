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

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == ColorGradientBox.StartColorProperty.PropertyName ||
			    e.PropertyName == ColorGradientBox.EndColorProperty.PropertyName ||
			    e.PropertyName == ColorGradientBox.OrientationProperty.PropertyName ||
			    e.PropertyName == VisualElement.IsVisibleProperty.PropertyName && base.Element.IsVisible
			   )
				this.SetNeedsDisplay();
		}

		private CGSize _previousSize;
		public override void LayoutSubviews()
		{
			if (this._previousSize != this.Bounds.Size)
				this.SetNeedsDisplay();
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
			this._previousSize = this.Bounds.Size;
			//BackgroundColor = Color.Transparent.ToUIColor();
		}

        /*
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			System.Diagnostics.Debug.WriteLine("e.PropertyName="+e.PropertyName);
			if (e.PropertyName == VisualElement.WidthProperty.PropertyName)
				System.Diagnostics.Debug.WriteLine("\tWidth=["+Element.Width+"] ["+NativeView.Bounds.Width+"]" );
			if (e.PropertyName == VisualElement.HeightProperty.PropertyName)
				System.Diagnostics.Debug.WriteLine("\tHeight=["+Element.Height+"] ["+NativeView.Bounds.Height+"]");
		}
		*/

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                _oldSublayer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}