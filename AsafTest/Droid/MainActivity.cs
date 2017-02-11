using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AsafTest.Droid
{
	[Activity(Label = "AsafTest", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			Forms9Patch.Droid.Settings.LicenseKey = "6PVV-4ASX-6GTM-Y9CW-Y4FB-XZ6B-AADB-LTYF-HT8A-XKF3-99GE-LQBT-5TKQ";

			LoadApplication(new App());
		}
	}
}
