using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidRotateEventArgs : RotateEventArgs
	{
		public AndroidRotateEventArgs(MotionEvent current, MotionEvent.PointerCoords[] coords, RotateEventArgs previous, View view, Listener listener) {
			Listener = listener;
			Cancelled = (current.Action == MotionEventActions.Cancel);
			ElementPosition = VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			ElementTouches = AndroidEventArgsHelper.GetTouches(coords, previous, view, listener);
			WindowTouches = AndroidEventArgsHelper.GetTouches(coords, previous, view, null);
			//WindowTouches = AndroidEventArgsHelper.GetRawTouches(current);

			CalculateAngles(previous);
		}
	}
}
