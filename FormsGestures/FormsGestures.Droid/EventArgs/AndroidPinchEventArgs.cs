using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidPinchEventArgs : PinchEventArgs
	{
		public AndroidPinchEventArgs(MotionEvent current, MotionEvent.PointerCoords[] coords, PinchEventArgs previous, View view, int[] startLocation, Listener listener) {
			Listener = listener;
			Cancelled = (current.Action == MotionEventActions.Cancel);
			ViewPosition = FormsGestures.VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			Touches = AndroidEventArgsHelper.GetTouches(coords, previous, view, startLocation, listener);
			CalculateScales(previous);
		}
	}

}
