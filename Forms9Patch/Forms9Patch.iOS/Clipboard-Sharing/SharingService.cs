using System;
using Xamarin.Forms;
using UIKit;
using Foundation;
using MobileCoreServices;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Platform.iOS;
using System.IO;
using System.Diagnostics;
using ObjCRuntime;
using System.Threading.Tasks;
using CoreGraphics;

[assembly: Dependency(typeof(Forms9Patch.iOS.SharingService))]
namespace Forms9Patch.iOS
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class SharingService : Forms9Patch.ISharingService
    {

        public void Share(Forms9Patch.MimeItemCollection mimeItemCollection, VisualElement target)
        {
            var nsItemProviders = mimeItemCollection.AsNSItemProviders();

            var activityController = new UIActivityViewController(nsItemProviders.ToArray(), null);

            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;

            if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
            {
                var targetUIView = Xamarin.Forms.Platform.iOS.Platform.GetRenderer(target).NativeView;
                activityController.PopoverPresentationController.SourceView = targetUIView;
                activityController.PopoverPresentationController.SourceRect = new CGRect(new CGPoint(0, 0), targetUIView.Frame.Size); // targetUIView.Frame;
                //activityController.PopoverPresentationController.BackgroundColor = UIColor.DarkGray;  // sets the popup's background color
                activityController.PopoverPresentationController.PageOverlayView().BackgroundColor = UIColor.Black.ColorWithAlpha(0.4f);
            }

            vc.PresentViewController(activityController, true, null);

        }
    }

    public static class UIPopoverPresentationControllerExtensions
    {
        public static UIView PageOverlayView(this UIPopoverPresentationController controller)
        {
            return controller.ValueForKey((Foundation.NSString)"_dimmingView") as UIView;
        }
    }
}