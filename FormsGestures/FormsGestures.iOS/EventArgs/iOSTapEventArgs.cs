using UIKit;

namespace FormsGestures.iOS
{
	public class iOSTapEventArgs : TapEventArgs
	{
		public iOSTapEventArgs(UITapGestureRecognizer gr, int numberOfTaps)
		{
			Cancelled = (gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed);
			ElementPosition = gr.View.BoundsInFormsCoord();
			ElementTouches = iOSEventArgsHelper.GetTouches(gr, gr.View);
			WindowTouches = iOSEventArgsHelper.GetTouches(gr, null);
			NumberOfTaps = numberOfTaps;
		}
	}
}
