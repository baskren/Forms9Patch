using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidDownUpEventArgs : DownUpEventArgs
	{
		public AndroidDownUpEventArgs(MotionEvent current, View view, int[] startLocation) {
			Cancelled = (current.Action == MotionEventActions.Cancel);
            ViewPosition = AndroidEventArgsHelper.GetViewPosition(view);
			Touches = AndroidEventArgsHelper.GetTouches(current, view, startLocation);
			TriggeringTouches = new [] { current.ActionIndex };
		}
	}
}
