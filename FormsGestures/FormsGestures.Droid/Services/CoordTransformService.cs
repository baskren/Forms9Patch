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
            IVisualElementRenderer toRenderer = Platform.GetRenderer(toElement);
            IVisualElementRenderer fromRenderer = Platform.GetRenderer(fromElement);
            if (fromRenderer?.View != null && toRenderer?.View != null)
            {
                var fromElementLocation = new int[2];
                var toElementLocation = new int[2];

                //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": Orientation[" + ((IWindowManager)Droid.Settings.Context.GetSystemService(Context.WindowService)).DefaultDisplay.Rotation + "]");
                toRenderer.View.GetLocationOnScreen(toElementLocation);
                //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": toElementLocation[" + string.Join(",", toElementLocation) + "]");
                fromRenderer.View.GetLocationOnScreen(fromElementLocation);
                //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": fromElementLocation[" + string.Join(",", fromElementLocation) + "]");

                var scale = Droid.Settings.Context.Resources.DisplayMetrics.Density;
                return new Point(
                               p.X + ((fromElementLocation[0] - toElementLocation[0]) / scale),
                               p.Y + ((fromElementLocation[1] - toElementLocation[1]) / scale));
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

