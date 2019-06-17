using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using FormsGestures;
using Windows.UI.Xaml;

[assembly: Xamarin.Forms.Dependency (typeof(FormsGestures.UWP.CoordinateTransformService))]
namespace FormsGestures.UWP

{
	/// <summary>
	/// Descendent bounds.
	/// </summary>
	public class CoordinateTransformService : ICoordTransform
	{
		#region ILocation implementation

		public Point CoordTransform (VisualElement fromElement, Point p, VisualElement toElement) {
            var toRenderer = Platform.GetRenderer (toElement);
            var fromRenderer = Platform.GetRenderer (fromElement);
			if (toRenderer != null && fromRenderer != null) {
                if (fromRenderer.ContainerElement != null && toRenderer.ContainerElement != null)
                {
                    var transform = fromRenderer.ContainerElement.TransformToVisual(toRenderer.ContainerElement);
                    
                    var transformedPoint = transform.TransformPoint(p.ToUwpPoint());
                    return transformedPoint.ToXfPoint();
                    //return fromRenderer.ContainerElement.ConvertPointToView(new CoreGraphics.CGPoint(p.X, p.Y), toRenderer.NativeView).ToPoint();
                }
			} 
			return new Point (double.NegativeInfinity, double.NegativeInfinity);
		}

		public Rectangle CoordTransform (VisualElement fromElement, Rectangle r, VisualElement toElement) {
            if (r.Width < 0 || r.Height < 0)
                return new Rectangle(-1, -1, -1, -1);
            var toRenderer = Platform.GetRenderer(toElement);
            var fromRenderer = Platform.GetRenderer(fromElement);
            if (fromRenderer?.ContainerElement is FrameworkElement fromNativeView && toRenderer?.ContainerElement is FrameworkElement toNativeView)
            {
                var transform = fromNativeView.TransformToVisual(toNativeView);

                var transformedPoint = transform.TransformBounds(r.ToWinRect());
                return transformedPoint.ToXfRectangle();
                //return fromRenderer.ContainerElement.ConvertPointToView(new CoreGraphics.CGPoint(p.X, p.Y), toRenderer.NativeView).ToPoint();
            }
            return new Rectangle(-1, -1, -1, -1);
        }

        #endregion

    }
}

