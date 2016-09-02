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
	[Activity (Label = "BC.FormsExt DemoApp", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Xamarin.Forms.Forms.Init has to be here (for Android) otherwise Xamarin.Forms.Image() { Source = "local source file" } doesnt work!
			global::Xamarin.Forms.Forms.Init (this, bundle);
			Forms9Patch.Droid.Settings.LicenseKey = "SG6T-B2W5-9ELF-GRGH-Z8NK-VURP-5TRG-F2L7-Q88H-W8SH-HXE3-VT5X-9WZ9";
			//FormsGestures.Droid.Settings.Init ();
			//FormsPopups.Droid.Framework.Init();


			LoadApplication (new App ());
		}
	}
}

