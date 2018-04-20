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
            IVisualElementRenderer toRenderer = Platform.GetRenderer(toElement);
            IVisualElementRenderer fromRenderer = Platform.GetRenderer(fromElement);
            if (toRenderer != null && fromRenderer != null)
            {
                if (fromRenderer.NativeView != null && toRenderer.NativeView != null)
                    return fromRenderer.NativeView.ConvertPointToView(new CoreGraphics.CGPoint(p.X, p.Y), toRenderer.NativeView).ToPoint();
            }
            return new Point(double.NegativeInfinity, double.NegativeInfinity);
        }

        public Rectangle CoordTransform(VisualElement fromElement, Rectangle r, VisualElement toElement)
        {
            var point = CoordTransform(fromElement, r.Location, toElement);
            return new Rectangle(point, r.Size);
        }

        public LocationService()
        {
            Settings.Init();
        }
        #endregion

    }
}

