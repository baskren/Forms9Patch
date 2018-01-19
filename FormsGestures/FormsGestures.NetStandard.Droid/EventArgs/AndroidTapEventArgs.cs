using Android.Views;

namespace FormsGestures.Droid
{
	public class AndroidTapEventArgs : TapEventArgs
	{
		public AndroidTapEventArgs(MotionEvent tap, View view, int numberOfTaps, int[] startLocation) {
			//System.Diagnostics.Debug.WriteLine("tap0=["+tap.GetX()+","+tap.GetY()+"]");
			Cancelled = (tap.Action == MotionEventActions.Cancel);
			//System.Diagnostics.Debug.WriteLine("tap1=["+tap.GetX()+","+tap.GetY()+"]");
			ViewPosition = AndroidEventArgsHelper.GetViewPosition(view);
			//System.Diagnostics.Debug.WriteLine("tap2=["+tap.GetX()+","+tap.GetY()+"]");
			Touches = AndroidEventArgsHelper.GetTouches(tap, view, startLocation);
			//System.Diagnostics.Debug.WriteLine("tap3=["+tap.GetX()+","+tap.GetY()+"]");
			NumberOfTaps = numberOfTaps;
		}
	}
}
