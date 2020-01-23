using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidLongPressEventArgs : LongPressEventArgs
	{
		public AndroidLongPressEventArgs(MotionEvent start, MotionEvent end, View view, Listener listener) {
			MotionEvent current = end ?? start;
			Listener = listener;
			Cancelled = current.Action == MotionEventActions.Cancel;
			ElementPosition = VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			ElementTouches = AndroidEventArgsHelper.GetTouches(current,view, listener);
			//WindowTouches = AndroidEventArgsHelper.GetTouches(current, view, null);
			WindowTouches = AndroidEventArgsHelper.GetRawTouches(current);
			if (start != null && end != null)
				Duration = end.EventTime - start.EventTime;
			else 
				Duration = 500;
		}
	}
}
