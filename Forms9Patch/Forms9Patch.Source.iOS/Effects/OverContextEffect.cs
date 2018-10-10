using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using System.Threading.Tasks;

[assembly: ExportEffect(typeof(Forms9Patch.iOS.OverContextEffect), "OverContextEffect")]
namespace Forms9Patch.iOS
{
    public class OverContextEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Element is Forms9Patch.PopupBase popup && popup.IsVisible && Container != null)
                // we are not going to get a "IsVisible call and, in release builds, the render cycle is going to overwrite the above
                DelayedBringToFront();
        }

        async Task DelayedBringToFront()
        {
            while (Container == null || UIApplication.SharedApplication?.KeyWindow == null)
            {
                await Task.Delay(50);
            }
            UIApplication.SharedApplication.KeyWindow.Add(Container);
            UIApplication.SharedApplication.KeyWindow.BringSubviewToFront(Container);
        }

        /*
        void ShowResponderTree(UIResponder responder, int iter = 0, UIView subview = null)
        {
            if (responder == null)
                return;
            if (subview == null)
                subview = responder as UIView;
            for (int i = 0; i < iter; i++)
                Console.Write("\t");
            if (responder.NextResponder is UIView view)
            {
                view.BringSubviewToFront(subview);
                Console.Write("~");
            }
            Console.WriteLine(responder.ToString());
            ShowResponderTree(responder.NextResponder, iter + 1, subview);
        }
        */

        protected override void OnDetached() { }

    }
}