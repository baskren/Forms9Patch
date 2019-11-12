using System;
using CoreGraphics;
using UIKit;
using Foundation;
using Forms9Patch.Elements.Popups.Core;
using Xamarin.Forms.Platform.iOS;

namespace Forms9Patch.iOS
{
    [Preserve(AllMembers = true)]
    [Register("F9PPopupWindow")]
    internal class PopupWindow : UIWindow
    {
        public PopupWindow(IntPtr handle) : base(handle) { }

        public PopupWindow() { }

        public override UIView HitTest(CGPoint point, UIEvent uievent)
        {
            if (RootViewController is PopupPlatformRenderer platformRenderer
                && platformRenderer.Renderer is IVisualElementRenderer renderer
                && renderer.Element is PopupPage formsElement)
            {
                if (formsElement.InputTransparent)
                    return null;
                var hitTestResult = base.HitTest(point, uievent);
                if (formsElement.BackgroundInputTransparent && renderer.NativeView == hitTestResult)
                {
                    formsElement.SendBackgroundClick();
                    return null;
                }
                return hitTestResult;
            }
            return null;
        }
    }
}
