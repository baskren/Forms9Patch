using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Touch;
using Foundation;
using UIKit;

namespace TestLabel.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			CachedImageRenderer.Init();
			Forms9Patch.iOS.Settings.LicenseKey = "TQUL-WWD9-PSEK-YR32-M36E-63QJ-K27G-BCUP-UXYH-KAAP-F38R-TKVW-U43L";
			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
