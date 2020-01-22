using CoreGraphics;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FormsGestures.iOS
{
	public class iOSPanEventArgs : PanEventArgs
	{
		public iOSPanEventArgs(UIPanGestureRecognizer gr, PanEventArgs previous, CGPoint locationAtStart)
		{
			Cancelled = (gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed);
			ViewPosition = gr.View.BoundsInDipCoord();
			Touches = iOSEventArgsHelper.GetTouches(gr, locationAtStart);

			var viewLocationAtStart = new Point(locationAtStart.X, locationAtStart.Y);
			System.Diagnostics.Debug.WriteLine("[iOSPanEventArgs."
            + P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
            + P42.Utils.ReflectionExtensions.CallerLineNumber()
            + "] ViewPosition["+ViewPosition.Location+"] viewLocationAtStart["+viewLocationAtStart+"]  delta["+(ViewPosition.Location.Subtract(viewLocationAtStart))+"]");

            CalculateDistances(previous, viewLocationAtStart);
			Velocity = GetVelocity(gr);
		}

		Point GetVelocity(UIPanGestureRecognizer gr)
		{
			CGPoint cGPoint = gr.VelocityInView(null);
			return cGPoint.ToPoint();
		}
	}
}
