using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Forms9PatchDemo;


namespace Forms9PatchDemo.Droid
{
	[Activity (Label = "Forms9Patch Demo", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Xamarin.Forms.Forms.Init has to be here (for Android) otherwise Xamarin.Forms.Image() { Source = "local source file" } doesnt work!
			global::Xamarin.Forms.Forms.Init (this, bundle);
			Forms9Patch.Droid.Settings.LicenseKey = "NZPK-RMP4-PJVV-Z7LP-78JF-GNXB-CDJZ-SRYA-BLR2-WBZC-G64K-QJZW-65DB";
			//FormsGestures.Droid.Settings.Init ();
			//FormsPopups.Droid.Framework.Init();


			LoadApplication (new App ());
		}
	}
}

