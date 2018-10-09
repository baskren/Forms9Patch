using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;

[assembly: ExportEffect(typeof(Forms9Patch.iOS.OverContextEffect), "OverContextEffect")]
namespace Forms9Patch.iOS
{
    public class OverContextEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            UIApplication.SharedApplication.KeyWindow.BringSubviewToFront(Container);
        }

        protected override void OnDetached() { }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == PopupBase.IsVisibleProperty.PropertyName)
                UIApplication.SharedApplication.KeyWindow.BringSubviewToFront(Container);
        }
    }
}