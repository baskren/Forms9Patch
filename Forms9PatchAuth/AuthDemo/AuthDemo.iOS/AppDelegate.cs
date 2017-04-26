// /*******************************************************************
//  *
//  * AppDelegate.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace AuthDemo.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			Forms9Patch.iOS.Settings.LicenseKey = "L89Z-J4AV-D449-G3P6-TZ29-F45P-M9UN-XLWE-LAXM-LB8C-NTMA-4W2W-QA4D";

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
