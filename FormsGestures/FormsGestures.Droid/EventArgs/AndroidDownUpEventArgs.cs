using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace FormsGestures.Droid
{
	public class AndroidDownUpEventArgs : DownUpEventArgs
	{
		public AndroidDownUpEventArgs(MotionEvent current, Android.Views.View view, Listener listener) {
			Listener = listener;
			Cancelled = current.Action == MotionEventActions.Cancel;
			ElementPosition = listener.Element.BoundsInWindowCoord();
			ElementTouches = AndroidEventArgsHelper.GetTouches(current, view, listener);
			//WindowTouches = AndroidEventArgsHelper.GetTouches(current, view, null);
			WindowTouches = AndroidEventArgsHelper.GetRawTouches(current);
			TriggeringTouches = new [] { current.ActionIndex };
		}
	}
}
