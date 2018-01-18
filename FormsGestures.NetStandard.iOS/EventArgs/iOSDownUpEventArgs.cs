using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xamarin.Forms;

namespace FormsGestures.iOS
{
	class iOSDownUpEventArgs : DownUpEventArgs
	{
		public iOSDownUpEventArgs(DownUpGestureRecognizer gr, UITouch[] triggeringTouches, CoreGraphics.CGPoint viewLocationAtStart)
		{
			Cancelled = (gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed);
			ViewPosition = iOSEventArgsHelper.GetViewPosition(gr.View.Frame);
			Touches = iOSEventArgsHelper.GetTouches(gr, viewLocationAtStart);
			TriggeringTouches = GetTriggeringTouches(gr, triggeringTouches, viewLocationAtStart);
		}

		private int[] GetTriggeringTouches(DownUpGestureRecognizer gr, UITouch[] triggeringTouches, CoreGraphics.CGPoint locationAtStart)
		{
			Point[] array = Enumerable.ToArray<Point>(Enumerable.Select<UITouch, Point>(triggeringTouches, delegate(UITouch t) {
				CGPoint cGPoint = t.LocationInView(null);
				return new Point(cGPoint.X-locationAtStart.X, cGPoint.Y-locationAtStart.Y);
			}));
			List<int> list = new List<int>();
			for (int i = 0; i < Touches.Length; i++) {
				Point t = Touches[i];
				if (Enumerable.Any<Point>(array, (Point p) => Math.Abs(p.X - t.X) < 0.1 && Math.Abs(p.Y - t.Y) < 0.1))
				{
					list.Add(i);
				}
			}
			return list.ToArray();
		}
	}
}
