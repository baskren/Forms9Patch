using System;
using UIKit;
namespace Forms9Patch.iOS
{
    public static class UIViewExtensions
    {
        public static UIViewController FindUIViewController(this UIView view)
        {
            UIResponder responder = view;
            while (responder != null && !(responder is UIViewController))
                responder = responder.NextResponder;
            return (UIViewController)responder;
        }

        public static UIResponder FindCurrentResponder(this UIView view)
        {
            UIResponder responder = view;
            while (responder != null && !(responder.IsFirstResponder))
                responder = responder.NextResponder;
            return (UIViewController)responder;
        }
    }
}
