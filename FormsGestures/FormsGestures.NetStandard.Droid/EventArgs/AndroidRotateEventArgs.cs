using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidRotateEventArgs : RotateEventArgs
	{
		public AndroidRotateEventArgs(MotionEvent current, MotionEvent.PointerCoords[] coords, RotateEventArgs previous, View view, int[] startLocation) {
			Cancelled = (current.Action == MotionEventActions.Cancel);
			ViewPosition = AndroidEventArgsHelper.GetViewPosition(view);
			Touches = AndroidEventArgsHelper.GetTouches(coords, previous, view, startLocation);
			CalculateAngles(previous);
		}
	}
}
