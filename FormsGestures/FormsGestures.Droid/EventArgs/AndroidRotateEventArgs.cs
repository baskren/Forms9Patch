using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidRotateEventArgs : RotateEventArgs
	{
		public AndroidRotateEventArgs(MotionEvent current, MotionEvent.PointerCoords[] coords, RotateEventArgs previous, View view, int[] startLocation, Listener listener) {
			Listener = listener;
			Cancelled = (current.Action == MotionEventActions.Cancel);
			ViewPosition = FormsGestures.VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			Touches = AndroidEventArgsHelper.GetTouches(coords, previous, view, startLocation, listener);
			CalculateAngles(previous);
		}
	}
}
