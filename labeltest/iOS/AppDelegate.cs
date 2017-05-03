using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace labeltest.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Forms9Patch.iOS.Settings.LicenseKey = "<your license key>";

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
