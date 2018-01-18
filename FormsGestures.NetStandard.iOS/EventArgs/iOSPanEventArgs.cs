using CoreGraphics;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FormsGestures.iOS
{
	public class iOSPanEventArgs : PanEventArgs
	{
		public iOSPanEventArgs(UIPanGestureRecognizer gr, PanEventArgs previous, CoreGraphics.CGPoint locationAtStart)
		{
			Cancelled = (gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed);
			ViewPosition = iOSEventArgsHelper.GetViewPosition(gr.View.Frame);
			Touches = iOSEventArgsHelper.GetTouches(gr, locationAtStart);
			base.CalculateDistances(previous);
			Velocity = GetVelocity(gr);
		}

		Point GetVelocity(UIPanGestureRecognizer gr)
		{
			CGPoint cGPoint = gr.VelocityInView(null);
			return cGPoint.ToPoint();
		}
	}
}
