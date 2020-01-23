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
		public static Xamarin.Forms.Point[] GetTouches(this UIGestureRecognizer gestureRecognizer, UIView view) {
			nint numberOfTouches = gestureRecognizer.NumberOfTouches;
			var array = new Xamarin.Forms.Point[numberOfTouches];
			for (int i = 0; i < numberOfTouches; i++)
            {
                /*
                if (view == null)
                {
					var page = Xamarin.Forms.Application.Current.MainPage;
					if (page is Xamarin.Forms.NavigationPage navPage)
						page = navPage.CurrentPage;
					if (Xamarin.Forms.Platform.iOS.Platform.GetRenderer(page) is Xamarin.Forms.Platform.iOS.IVisualElementRenderer renderer)
						view = renderer.NativeView;
                }
                */
				var point = gestureRecognizer.LocationOfTouch(i, view);
				array[i] = point.ToPoint();
			}
			return array;
		}

		public static Xamarin.Forms.Point[] GetTouches(this UIGestureRecognizer gestureRecognizer, UIView view, int requiredTouches, BaseGestureEventArgs previous) {
			nint numberOfTouches = gestureRecognizer.NumberOfTouches;
			if (numberOfTouches < requiredTouches && previous != null)
				return view==null
                    ? previous.WindowTouches
                    : previous.ElementTouches;
			return iOSEventArgsHelper.GetTouches(gestureRecognizer, view);
		}
	}
}
