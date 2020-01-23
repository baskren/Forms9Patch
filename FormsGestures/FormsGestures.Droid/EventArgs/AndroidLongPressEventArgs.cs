using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidLongPressEventArgs : LongPressEventArgs
	{
		public AndroidLongPressEventArgs(MotionEvent start, MotionEvent end, View view, Listener listener) {
			MotionEvent e = end ?? start;
			Listener = listener;
			Cancelled = e.Action == MotionEventActions.Cancel;
			ElementPosition = VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			ElementTouches = AndroidEventArgsHelper.GetTouches(e,view, listener);
			if (start != null && end != null)
				Duration = end.EventTime - start.EventTime;
			else 
				Duration = 500;
		}
	}
}
