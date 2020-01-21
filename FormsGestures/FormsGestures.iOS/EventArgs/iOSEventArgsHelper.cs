using CoreGraphics;
using System;
using System.Drawing;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FormsGestures.iOS
{
	public static class iOSEventArgsHelper
	{
		public static Xamarin.Forms.Point[] GetTouches(UIGestureRecognizer gestureRecognizer, CGPoint locationAtStart) {
			nint numberOfTouches = gestureRecognizer.NumberOfTouches;
			var array = new Xamarin.Forms.Point[numberOfTouches];
			for (int i = 0; i < numberOfTouches; i++)
            {
				var point = gestureRecognizer.LocationOfTouch(i, gestureRecognizer.View);
				array[i] = point.ToPoint();
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
