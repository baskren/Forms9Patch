using FormsGestures.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(LocationService))]
namespace FormsGestures.iOS
{
    /// <summary>
    /// Descendent bounds.
    /// </summary>
    public class LocationService : ICoordTransform
    {
        #region ILocation implementation

        public Point CoordTransform(VisualElement fromElement, Point p, VisualElement toElement)
        {
            if (fromElement != null && Platform.GetRenderer(fromElement) is IVisualElementRenderer fromRenderer && fromRenderer.NativeView != null)
            {
                var windowToPoint = fromRenderer.NativeView.LocationInDipCoord();
                if (toElement != null && Platform.GetRenderer(toElement) is IVisualElementRenderer toRenderer && toRenderer.NativeView != null)
                {
                    var windowToDestination = toRenderer.NativeView.LocationInDipCoord();
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
                var origin = renderer.NativeView.LocationInDipCoord();
                return origin.Add(point);
            }
            return new Point(double.NegativeInfinity, double.NegativeInfinity);
        }

        public Rectangle BoundsInWindowCoord(VisualElement element)
        {
            if (element != null && Platform.GetRenderer(element) is IVisualElementRenderer renderer && renderer.NativeView != null)
                return renderer.NativeView.BoundsInDipCoord();
            return new Rectangle(new Point(double.NegativeInfinity, double.NegativeInfinity), element.Bounds.Size);
        }

        public LocationService()
        {
            Settings.Init();
        }
        #endregion

    }
}

