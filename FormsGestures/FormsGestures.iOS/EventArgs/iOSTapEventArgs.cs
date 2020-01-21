using UIKit;

namespace FormsGestures.iOS
{
	public class iOSTapEventArgs : TapEventArgs
	{
		public iOSTapEventArgs(UITapGestureRecognizer gr, int numberOfTaps, CoreGraphics.CGPoint locationAtStart)
		{
			Cancelled = (gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed);
			ViewPosition = gr.View.BoundsInDipCoord();
			Touches = iOSEventArgsHelper.GetTouches(gr, locationAtStart);
			NumberOfTaps = numberOfTaps;
		}
	}
}
