using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using global::Xamarin.Forms.Platform.iOS;
using Foundation;
using UIKit;

namespace TestProject.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			Forms9Patch.iOS.Settings.LicenseKey ="NJHS-HGTK-EWPL-789K-H4A3-9LHZ-67FA-P8AQ-ZJ8R-X83P-UNWE-QNYT-LGYA";
			//FormsGestures.iOS.Settings.Init ();

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			//Xamarin.Calabash.Start();
			#endif

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

