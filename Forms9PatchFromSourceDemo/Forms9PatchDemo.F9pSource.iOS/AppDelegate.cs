using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using global::Xamarin.Forms.Platform.iOS;
using Foundation;
using UIKit;

namespace Forms9PatchDemo.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Forms9Patch.iOS.Settings.Initialize(this);
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
    }
}

