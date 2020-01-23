using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidPinchEventArgs : PinchEventArgs
	{
		public AndroidPinchEventArgs(MotionEvent current, MotionEvent.PointerCoords[] coords, PinchEventArgs previous, View view, Listener listener) {
			Listener = listener;
			Cancelled = (current.Action == MotionEventActions.Cancel);
			ElementPosition = VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			ElementTouches = AndroidEventArgsHelper.GetTouches(coords, previous, view, listener);
			WindowTouches = AndroidEventArgsHelper.GetTouches(coords, previous, view, null);
			//WindowTouches = AndroidEventArgsHelper.GetRawTouches(current);
			CalculateScales(previous);
		}
	}

}
