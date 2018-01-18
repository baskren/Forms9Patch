using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidSwipeEventArgs : SwipeEventArgs
	{
		public AndroidSwipeEventArgs(MotionEvent end, View view, Direction direction, int[] startLocation) {
			Cancelled = (end.Action == MotionEventActions.Cancel);
			ViewPosition = AndroidEventArgsHelper.GetViewPosition(view);
			Touches = AndroidEventArgsHelper.GetTouches(end,view,startLocation);
			Direction = direction;
		}
	}
}
