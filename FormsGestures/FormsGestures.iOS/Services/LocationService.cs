using FormsGestures.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(LocationService))]
namespace FormsGestures.iOS
{
    /// <summary>
    /// Descendent bounds.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class LocationService : ICoordTransform
    {
        #region ILocation implementation

        public Point CoordTransform(VisualElement fromElement, Point p, VisualElement toElement)
        {
            if (fromElement != null && Platform.GetRenderer(fromElement) is IVisualElementRenderer fromRenderer && fromRenderer.NativeView != null)
            {
                var windowToPoint = fromRenderer.NativeView.LocationInFormsCoord();
                if (toElement != null && Platform.GetRenderer(toElement) is IVisualElementRenderer toRenderer && toRenderer.NativeView != null)
                {
                    var windowToDestination = toRenderer.NativeView.LocationInFormsCoord();
                    var delta = new Point(p.X + windowToPoint.X - windowToDestination.X, p.Y + windowToPoint.Y - windowToDestination.Y);
                    return delta;
                }
            }
            return new Point(double.NegativeInfinity, double.NegativeInfinity);
        }

        public Rectangle CoordTransform(VisualElement fromElement, Rectangle r, VisualElement toElement)
        {
            var point = CoordTransform(fromElement, r.Location, toElement);
            return new Rectangle(point, r.Size);
        }

        public Point PointInWindowCoord(VisualElement element, Point point)
        {
            if (element != null && Platform.GetRenderer(element) is IVisualElementRenderer renderer && renderer.NativeView != null)
            {
                var origin = renderer.NativeView.LocationInFormsCoord();
                return origin.Add(point);
            }
            return new Point(double.NegativeInfinity, double.NegativeInfinity);
        }

        public Rectangle BoundsInWindowCoord(VisualElement element)
        {
            if (element != null && Platform.GetRenderer(element) is IVisualElementRenderer renderer && renderer.NativeView != null)
                return renderer.NativeView.BoundsInFormsCoord();
            return new Rectangle(new Point(double.NegativeInfinity, double.NegativeInfinity), element.Bounds.Size);
        }

        public LocationService()
        {
            Settings.Init();
        }
        #endregion

    }
}

