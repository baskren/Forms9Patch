using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(Forms9Patch.iOS.WebViewFixEffect), "WebViewFixEffect")]
namespace Forms9Patch.iOS
{
    public class WebViewFixEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control != null)
            {
                //Control.Opaque = false;
                //Control.BackgroundColor = UIColor.Clear;
            }
        }

        protected override void OnDetached()
        {
            //throw new NotImplementedException();
        }
    }
}
