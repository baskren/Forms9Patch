using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace FormsGestures.Droid
{
	public class AndroidDownUpEventArgs : DownUpEventArgs
	{
		public AndroidDownUpEventArgs(MotionEvent current, Android.Views.View view, int[] startLocation, Listener listener) {
			Listener = listener;
			Cancelled = current.Action == MotionEventActions.Cancel;
			ViewPosition = listener.Element.BoundsInWindowCoord();
			Touches = AndroidEventArgsHelper.GetTouches(current, view, startLocation, listener);
			TriggeringTouches = new [] { current.ActionIndex };
		}
	}
}
