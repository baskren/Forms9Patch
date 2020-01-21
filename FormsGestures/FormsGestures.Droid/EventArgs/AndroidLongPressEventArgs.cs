using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidLongPressEventArgs : LongPressEventArgs
	{
		public AndroidLongPressEventArgs(MotionEvent start, MotionEvent end, View view, int[] startLocation, Listener listener) {
			MotionEvent e = end ?? start;
			Listener = listener;
			Cancelled = e.Action == MotionEventActions.Cancel;
			ViewPosition = FormsGestures.VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			Touches = AndroidEventArgsHelper.GetTouches(e,view,startLocation, listener);
			if (start != null && end != null)
				Duration = end.EventTime - start.EventTime;
			else 
				Duration = 500;
		}
	}
}
