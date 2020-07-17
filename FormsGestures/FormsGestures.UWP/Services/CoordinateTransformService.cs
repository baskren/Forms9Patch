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
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class CoordinateTransformService : ICoordTransform
	{
        #region ILocation implementation

        public Point CoordTransform (VisualElement fromElement, Point p, VisualElement toElement) 
        {
            if (Platform.GetRenderer(toElement) is IVisualElementRenderer toRenderer && toRenderer.ContainerElement != null)
            {
                if (Platform.GetRenderer(fromElement) is IVisualElementRenderer fromRenderer && fromRenderer.ContainerElement != null)
                {
                    var transform = fromRenderer.ContainerElement.TransformToVisual(toRenderer.ContainerElement);
                    var transformedPoint = transform.TransformPoint(p.ToUwpPoint());
                    return transformedPoint.ToXfPoint();
                }
            }
            return new Point(double.NegativeInfinity, double.NegativeInfinity);
        }

        public Rectangle CoordTransform (VisualElement fromElement, Rectangle r, VisualElement toElement) {
            if (r.Width < 0 || r.Height < 0)
                return new Rectangle(-1, -1, -1, -1);

            if (Platform.GetRenderer(toElement) is IVisualElementRenderer toRenderer && toRenderer.ContainerElement != null)
            {
                if (Platform.GetRenderer(fromElement) is IVisualElementRenderer fromRenderer && fromRenderer.ContainerElement != null)
                {
                    var transform = fromRenderer.ContainerElement.TransformToVisual(toRenderer.ContainerElement);
                    var transformedPoint = transform.TransformBounds(r.ToWinRect());
                    return transformedPoint.ToXfRectangle();
                }
            }
            return new Rectangle(double.NegativeInfinity, double.NegativeInfinity, fromElement.Width, fromElement.Height);
        }

        public bool HasRenderer(VisualElement element)
        {
            return Platform.GetRenderer(element) != null;
        }

        public Point PointInWindowCoord(VisualElement element, Point point)
        {
            if (Platform.GetRenderer(element) is IVisualElementRenderer renderer && renderer.ContainerElement != null)
            {
                var result = renderer.ContainerElement.PointInNativeAppWindowCoord(point.ToUwpPoint());
                return result.ToXfPoint();
            }
            return new Point(double.NegativeInfinity, double.NegativeInfinity);
        }
        public Rectangle BoundsInWindowCoord(VisualElement element)
        {
            if (Platform.GetRenderer(element) is IVisualElementRenderer renderer && renderer.ContainerElement != null)
            {
                var transform = renderer.ContainerElement.TransformToVisual(Window.Current.Content);
                var transformedRectangle = transform.TransformBounds(new Windows.Foundation.Rect(0,0, renderer.ContainerElement.Width, renderer.ContainerElement.Height));
                return transformedRectangle.ToXfRectangle();
            }
            return new Rectangle(double.NegativeInfinity, double.NegativeInfinity, element.Width, element.Height);
        }
        #endregion

    }
}

