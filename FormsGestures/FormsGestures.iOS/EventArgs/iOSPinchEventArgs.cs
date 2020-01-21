using System;
using UIKit;

namespace FormsGestures.iOS
{
	public class iOSPinchEventArgs : PinchEventArgs
	{
		public iOSPinchEventArgs(UIGestureRecognizer gr, PinchEventArgs previous, CoreGraphics.CGPoint locationAtStart)
		{
			Cancelled = (gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed);
			ViewPosition = gr.View.BoundsInDipCoord();
			Touches = iOSEventArgsHelper.GetTouches(gr, 2, previous, locationAtStart);
			base.CalculateScales(previous);
		}
	}
}
