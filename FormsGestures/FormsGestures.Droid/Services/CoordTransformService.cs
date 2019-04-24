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
            IVisualElementRenderer toRenderer = Platform.GetRenderer(toElement);
            IVisualElementRenderer fromRenderer = Platform.GetRenderer(fromElement);
            if (toRenderer != null && fromRenderer != null)
            {
                //if (fromRenderer.ViewGroup != null && toRenderer.ViewGroup != null) {
                if (fromRenderer.View != null && toRenderer.View != null)
                {
                    var fromElementLocation = new int[2];
                    var toElementLocation = new int[2];

                    //toRenderer.ViewGroup.GetLocationOnScreen (toElementLocation);
                    //fromRenderer.ViewGroup.GetLocationOnScreen (fromElementLocation);
                    toRenderer.View.GetLocationOnScreen(toElementLocation);
                    fromRenderer.View.GetLocationOnScreen(fromElementLocation);

                    var scale = Droid.Settings.Context.Resources.DisplayMetrics.Density;
                    return new Point(
                                   p.X + ((fromElementLocation[0] - toElementLocation[0]) / scale),
                                   p.Y + ((fromElementLocation[1] - toElementLocation[1]) / scale));
                }
            }
            return new Point(double.NegativeInfinity, double.NegativeInfinity);
        }

        public Rectangle CoordTransform(VisualElement fromElement, Rectangle r, VisualElement toElement)
        {
            var point = CoordTransform(fromElement, r.Location, toElement);
            return new Rectangle(point, r.Size);
        }

        public CoordTransformService()
        {
            Settings.Init();
        }
        #endregion
    }
}

