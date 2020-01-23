using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidSwipeEventArgs : SwipeEventArgs
	{
		public AndroidSwipeEventArgs(MotionEvent current, View view, Direction direction, Listener listener) {
			Listener = listener;
			Cancelled = current.Action == MotionEventActions.Cancel;
			ElementPosition = VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			ElementTouches = AndroidEventArgsHelper.GetTouches(current, view, listener);
			//WindowTouches = AndroidEventArgsHelper.GetTouches(current, view, null);
			WindowTouches = AndroidEventArgsHelper.GetRawTouches(current);
			Direction = direction;
		}
	}
}
