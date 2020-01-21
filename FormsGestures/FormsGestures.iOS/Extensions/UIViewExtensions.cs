using System;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FormsGestures.iOS
{
    public static class UIViewExtensions
    {
        public static Rectangle BoundsInDipCoord(this UIView view)
            => FrameInNativeCoord(view).ToRectangle();

        public static Point LocationInDipCoord(this UIView view)
            => LocationInNativeCoord(view).ToPoint();

        public static CGRect FrameInNativeCoord(this UIView view)
            => new CGRect(LocationInNativeCoord(view), view.Frame.Size);

        public static CGPoint LocationInNativeCoord(this UIView view)
            =>  view.ConvertPointToView(view.Bounds.Location, null);

        public static CGPoint ViewPointInNativeCoord(this UIView view, CGPoint cgPoint)
        {
			var location = LocationInNativeCoord(view);
			return new CGPoint(location.X + cgPoint.X, location.Y + cgPoint.Y);
        }

        public static Point ViewPointInDipCoord(this UIView view, CGPoint cgPoint)
            => view.ViewPointInNativeCoord(cgPoint).ToPoint();
        

        public static bool IsTouchedBy(this UIView view, CGPoint cgPoint)
        {
			var targetRect = view.BoundsInDipCoord();
			return targetRect.Contains(cgPoint.ToPoint());
        }
	}
}
