using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidSwipeEventArgs : SwipeEventArgs
	{
		public AndroidSwipeEventArgs(MotionEvent end, View view, Direction direction, int[] startLocation, Listener listener) {
			Listener = listener;
			Cancelled = (end.Action == MotionEventActions.Cancel);
			ViewPosition = FormsGestures.VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			Touches = AndroidEventArgsHelper.GetTouches(end, view, startLocation, listener);
			Direction = direction;
		}
	}
}
