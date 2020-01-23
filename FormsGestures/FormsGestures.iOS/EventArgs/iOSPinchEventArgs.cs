using System;
using UIKit;

namespace FormsGestures.iOS
{
	public class iOSPinchEventArgs : PinchEventArgs
	{
		public iOSPinchEventArgs(UIGestureRecognizer gr, PinchEventArgs previous)
		{
			Cancelled = (gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed);
			ElementPosition = gr.View.BoundsInFormsCoord();
			ElementTouches = iOSEventArgsHelper.GetTouches(gr, gr.View, 2, previous);
			WindowTouches = iOSEventArgsHelper.GetTouches(gr, null, 2, previous);
			CalculateScales(previous);
		}
	}
}
