using CoreGraphics;
using System;
using System.Drawing;
using UIKit;
using Xamarin.Forms;

namespace FormsGestures.iOS
{
	public static class iOSEventArgsHelper
	{
        /* 
        //Some how, someone reported that System.Drawing.RectangeF was not found during build/link.  Huh?
		public static Xamarin.Forms.Rectangle GetViewPosition(RectangleF frame) {
			return new Xamarin.Forms.Rectangle((double)frame.X, (double)frame.Y, (double)frame.Width, (double)frame.Height);
		}
        */

		public static Xamarin.Forms.Rectangle GetViewPosition(CGRect frame) {
			return new Xamarin.Forms.Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
		}

		public static Xamarin.Forms.Point[] GetTouches(UIGestureRecognizer gestureRecognizer, CGPoint locationAtStart) {
			nint numberOfTouches = gestureRecognizer.NumberOfTouches;
			var array = new Xamarin.Forms.Point[numberOfTouches];
			for (int i = 0; i < numberOfTouches; i++) {
				//var viewPoint = gestureRecognizer.LocationOfTouch(i, gestureRecognizer.View);
				//var windowPoint = gestureRecognizer.View.ConvertPointToView (viewPoint, null);
				var windowPoint = gestureRecognizer.LocationOfTouch(i, null);

				array[i] = new Xamarin.Forms.Point(windowPoint.X-locationAtStart.X, windowPoint.Y-locationAtStart.Y);
			}
			return array;
		}

		public static Xamarin.Forms.Point[] GetTouches(UIGestureRecognizer gestureRecognizer, int requiredTouches, BaseGestureEventArgs previous, CGPoint locationAtStart) {
			nint numberOfTouches = gestureRecognizer.NumberOfTouches;
			if (numberOfTouches < requiredTouches && previous != null)
				return previous.Touches;
			return iOSEventArgsHelper.GetTouches(gestureRecognizer, locationAtStart);
		}
	}
}
