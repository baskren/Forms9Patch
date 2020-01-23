using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidSwipeEventArgs : SwipeEventArgs
	{
		public AndroidSwipeEventArgs(MotionEvent end, View view, Direction direction, Listener listener) {
			Listener = listener;
			Cancelled = (end.Action == MotionEventActions.Cancel);
			ElementPosition = VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			ElementTouches = AndroidEventArgsHelper.GetTouches(end, view, listener);
			Direction = direction;
		}
	}
}
