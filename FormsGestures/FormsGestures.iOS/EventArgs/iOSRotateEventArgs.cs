using System;
using UIKit;

namespace FormsGestures.iOS
{
	public class iOSRotateEventArgs : RotateEventArgs
	{
		public iOSRotateEventArgs(UIGestureRecognizer gr, RotateEventArgs previous, CoreGraphics.CGPoint locationAtStart)
		{
			Cancelled = (gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed);
			ViewPosition = gr.View.BoundsInDipCoord();
			Touches = iOSEventArgsHelper.GetTouches(gr, 2, previous, locationAtStart);
			base.CalculateAngles(previous);
		}
	}
}
