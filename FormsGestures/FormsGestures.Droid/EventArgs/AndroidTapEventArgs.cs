using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidTapEventArgs : TapEventArgs
	{
		public AndroidTapEventArgs(MotionEvent tap, View view, int numberOfTaps, Listener listener) {
			Listener = listener;
			Cancelled = (tap.Action == MotionEventActions.Cancel);
			ElementPosition = VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			ElementTouches = AndroidEventArgsHelper.GetTouches(tap, view, listener);
			NumberOfTaps = numberOfTaps;
		}
	}
}
