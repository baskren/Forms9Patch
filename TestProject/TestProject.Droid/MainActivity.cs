using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using TestProject;


namespace TestProject.Droid
{
	[Activity (Label = "TestProject", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Xamarin.Forms.Forms.Init has to be here (for Android) otherwise Xamarin.Forms.Image() { Source = "local source file" } doesnt work!
			global::Xamarin.Forms.Forms.Init (this, bundle);
			Forms9Patch.Droid.Settings.LicenseKey = "NJHS-HGTK-EWPL-789K-H4A3-9LHZ-67FA-P8AQ-ZJ8R-X83P-UNWE-QNYT-LGYA";
			//FormsGestures.Droid.Settings.Init ();
			//FormsPopups.Droid.Framework.Init();


			LoadApplication (new App ());
		}
	}
}

