using CoreGraphics;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FormsGestures.iOS
{
	public class iOSPanEventArgs : PanEventArgs
	{
		public iOSPanEventArgs(UIPanGestureRecognizer gr, BaseGestureEventArgs previous)
		{
			Cancelled = (gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed);
			ElementPosition = gr.View.BoundsInFormsCoord();
			ElementTouches = iOSEventArgsHelper.GetTouches(gr, gr.View);
			WindowTouches = iOSEventArgsHelper.GetTouches(gr, null);

            CalculateDistances(previous);
			Velocity = GetVelocity(gr);
		}

		Point GetVelocity(UIPanGestureRecognizer gr)
		{
			CGPoint cGPoint = gr.VelocityInView(null);
			return cGPoint.ToPoint();
		}
	}
}
