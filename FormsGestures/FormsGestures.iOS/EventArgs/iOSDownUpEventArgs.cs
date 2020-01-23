using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FormsGestures.iOS
{
	class iOSDownUpEventArgs : DownUpEventArgs
	{
		public iOSDownUpEventArgs(DownUpGestureRecognizer gr, UITouch[] triggeringTouches)
		{
			Cancelled = (gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed);
            ElementPosition = gr.View.BoundsInFormsCoord();
			ElementTouches = iOSEventArgsHelper.GetTouches(gr, gr.View);
			WindowTouches = iOSEventArgsHelper.GetTouches(gr, null);
			TriggeringTouches = GetTriggeringTouches(gr, triggeringTouches);
		}

#pragma warning disable IDE0060 // Remove unused parameter
        private int[] GetTriggeringTouches(DownUpGestureRecognizer gr, UITouch[] triggeringTouches)
#pragma warning restore IDE0060 // Remove unused parameter
        {
			/*  remember this notation next time I trip over using blocks for expressions
            Point[] array = triggeringTouches.Select(delegate(UITouch t) {
				CGPoint cGPoint = t.LocationInView(null);
				return cGPoint.ToPoint();
			}).ToArray();
            */
			var array = triggeringTouches.Select(t => t.LocationInView(null).ToPoint() ).ToArray();

			List<int> list = new List<int>();
			for (int i = 0; i < WindowTouches.Length; i++) {
				Point t = WindowTouches[i];
				if (array.Any(p => Math.Abs(p.X - t.X) < 0.1 && Math.Abs(p.Y - t.Y) < 0.1) )
					list.Add(i);				
			}
			return list.ToArray();
		}
	}
}
