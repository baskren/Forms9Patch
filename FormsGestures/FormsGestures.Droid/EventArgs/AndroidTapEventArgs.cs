using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidTapEventArgs : TapEventArgs
	{
		public AndroidTapEventArgs(MotionEvent tap, View view, int numberOfTaps, int[] startLocation, Listener listener) {
			Listener = listener;
			Cancelled = (tap.Action == MotionEventActions.Cancel);
			ViewPosition = FormsGestures.VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			Touches = AndroidEventArgsHelper.GetTouches(tap, view, startLocation, listener);
			NumberOfTaps = numberOfTaps;
		}
	}
}
