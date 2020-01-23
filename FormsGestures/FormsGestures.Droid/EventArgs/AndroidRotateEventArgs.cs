using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidRotateEventArgs : RotateEventArgs
	{
		public AndroidRotateEventArgs(MotionEvent current, MotionEvent.PointerCoords[] coords, RotateEventArgs previous, View view, int[] startLocation, Listener listener) {
			Listener = listener;
			Cancelled = (current.Action == MotionEventActions.Cancel);
			ElementPosition = VisualElementExtensions.BoundsInWindowCoord(listener.Element);
			ElementTouches = AndroidEventArgsHelper.GetTouches(coords, previous, view, listener);


			System.Diagnostics.Debug.WriteLine("[AndroidRotateEventArgs."
				+ P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
				+ P42.Utils.ReflectionExtensions.CallerLineNumber()
				+ "] T0[" + ElementTouches[0].ToString("N3") + "] P1[" + ElementTouches[1].ToString("N3") + "]");


			CalculateAngles(previous);
		}
	}
}
