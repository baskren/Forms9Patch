using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;
using HeightsAndAreas;
//using Android;

namespace HeightsAndAreas.Droid
{
	[Activity(Label = "H&A Calc", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			//TabLayoutResource = Resource.Layout.Tabbar;
			//ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);
			Forms9Patch.Droid.Settings.LicenseKey = "MVFQ-X27X-ANPP-QADG-FRJ3-8GSN-NP47-UHPZ-98EW-W2M4-NQXP-KXLJ-NACC";  // Key for: "H&A Calc"
			//var x = new Bc3.Forms.Droid

			LoadApplication(new App());
		}
	}
}
