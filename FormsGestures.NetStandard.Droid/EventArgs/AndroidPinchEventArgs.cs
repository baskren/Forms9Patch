using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidPinchEventArgs : PinchEventArgs
	{
		public AndroidPinchEventArgs(MotionEvent current, MotionEvent.PointerCoords[] coords, PinchEventArgs previous, View view, int[] startLocation) {
			Cancelled = (current.Action == MotionEventActions.Cancel);
			ViewPosition = AndroidEventArgsHelper.GetViewPosition(view);
			Touches = AndroidEventArgsHelper.GetTouches(coords, previous, view, startLocation);
			CalculateScales(previous);
		}
	}

}
