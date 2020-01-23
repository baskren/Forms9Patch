using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidTapEventArgs : TapEventArgs
	{
		public AndroidTapEventArgs(MotionEvent current, View view, int numberOfTaps, Listener listener) {
			Listener = listener;
			Cancelled = (current.Action == MotionEventActions.Cancel);
			ElementPosition = VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			ElementTouches = AndroidEventArgsHelper.GetTouches(current, view, listener);
			//WindowTouches = AndroidEventArgsHelper.GetTouches(current, view, null);
			WindowTouches = AndroidEventArgsHelper.GetRawTouches(current);
			NumberOfTaps = numberOfTaps;
		}
	}
}
