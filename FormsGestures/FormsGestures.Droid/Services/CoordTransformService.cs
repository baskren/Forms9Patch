using Android.Content;
using Android.Views;
using FormsGestures.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.Dependency(typeof(CoordTransformService))]
namespace FormsGestures.Droid
{
    /// <summary>
    /// Descendent bounds.
    /// </summary>
    public class CoordTransformService : ICoordTransform
    {
        #region ILocation implementation



        public Point CoordTransform(VisualElement fromElement, Point p, VisualElement toElement)
        {
            System.Diagnostics.Debug.WriteLine("    [CoordTransformService."
                + P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
                + P42.Utils.ReflectionExtensions.CallerLineNumber()
                + "] point["+p+"]");
            if (fromElement != null && Platform.GetRenderer(fromElement) is IVisualElementRenderer fromRenderer && fromRenderer.View != null)
            {
                var windowToPoint = fromRenderer.View.LocationInDipCoord();
                System.Diagnostics.Debug.WriteLine("    [CoordTransformService."
                    + P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
                    + P42.Utils.ReflectionExtensions.CallerLineNumber()
                    + "] windowToPoint["+windowToPoint+"] ["+fromRenderer+"]");
                if (toElement != null && Platform.GetRenderer(toElement) is IVisualElementRenderer toRenderer && toRenderer.View != null)
                {
                    var windowToDestination = toRenderer.View.LocationInDipCoord();
                    System.Diagnostics.Debug.WriteLine("    [CoordTransformService."
                        + P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
                        + P42.Utils.ReflectionExtensions.CallerLineNumber()
                        + "] windowToDestination["+windowToDestination+"] ["+toRenderer+"]");
                    var delta = new Point(p.X + windowToPoint.X - windowToDestination.X, p.Y + windowToPoint.Y - windowToDestination.Y);
                    System.Diagnostics.Debug.WriteLine("    [CoordTransformService."
                        + P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
                        + P42.Utils.ReflectionExtensions.CallerLineNumber()
                        + "] delta["+delta+"]");
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
            if (element != null && Platform.GetRenderer(element) is IVisualElementRenderer renderer && renderer.View != null)
                return point.Add(renderer.View.LocationInDipCoord());
            return new Point(double.NegativeInfinity, double.NegativeInfinity);
        }

        public Rectangle BoundsInWindowCoord(VisualElement element)
        {
            var point = new Point(double.NegativeInfinity, double.NegativeInfinity);
            if (element != null && Platform.GetRenderer(element) is IVisualElementRenderer renderer && renderer.View != null)
                point = renderer.View.LocationInDipCoord();
            return new Rectangle(point, element.Bounds.Size);
        }

        public CoordTransformService()
        {
            Settings.Init();
        }
        #endregion
    }
}

