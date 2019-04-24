using Android.Views;
using Xamarin.Forms;

namespace FormsGestures.Droid
{
	public class AndroidPanEventArgs : PanEventArgs
	{
		public AndroidPanEventArgs(MotionEvent previous, MotionEvent current, PanEventArgs prevArgs, global::Android.Views.View view, int[] startLocation) {
			Cancelled = (current.Action == MotionEventActions.Cancel);
			ViewPosition = AndroidEventArgsHelper.GetViewPosition(view);
			Touches = AndroidEventArgsHelper.GetTouches(current,view, startLocation);
			CalculateDistances(prevArgs);
			Velocity = GetVelocity(previous, current);
		}

		Point GetVelocity(InputEvent previous, InputEvent current) {
			if (previous == null)
				return new Point(0.0, 0.0);
			Point deltaDistance = DeltaDistance;
#if __DROIDv15__
#else
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {
                double num = (double)(current.EventTime - previous.EventTime);
                return new Point(deltaDistance.X * 1000.0 / num, deltaDistance.Y * 1000.0 / num);
            }
            else
#endif
                return new Point(0, 0);
		}
	}
}
