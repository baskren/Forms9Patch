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

namespace AsafTest.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			Forms9Patch.iOS.Settings.LicenseKey = "6PVV-4ASX-6GTM-Y9CW-Y4FB-XZ6B-AADB-LTYF-HT8A-XKF3-99GE-LQBT-5TKQ";

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
