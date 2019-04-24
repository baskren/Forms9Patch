using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidLongPressEventArgs : LongPressEventArgs
	{
		public AndroidLongPressEventArgs(MotionEvent start, MotionEvent end, View view, int[] startLocation) {
			MotionEvent e = end ?? start;
			Cancelled = e.Action == MotionEventActions.Cancel;
			ViewPosition = AndroidEventArgsHelper.GetViewPosition(view);
			Touches = AndroidEventArgsHelper.GetTouches(e,view,startLocation);
			if (start != null && end != null)
				Duration = end.EventTime - start.EventTime;
			else 
				Duration = 500;
		}
	}
}
