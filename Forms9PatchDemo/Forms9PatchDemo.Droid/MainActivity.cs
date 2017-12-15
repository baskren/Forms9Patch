using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.OS;
using Xamarin.Forms.Platform.Android;

namespace Forms9PatchDemo.Droid
{
    //[Activity(Label = "Forms9Patch Demo", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //public class MainActivity : FormsAppCompatActivity
    [Activity(Label = "Forms9Patch Demo", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            Window.RequestFeature(WindowFeatures.ActionBar);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Forms9Patch.Droid.Settings.Initialize(this, "NZPK-RMP4-PJVV-Z7LP-78JF-GNXB-CDJZ-SRYA-BLR2-WBZC-G64K-QJZW-65DB");
            LoadApplication(new App());

            ActionBar.Hide();
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetHomeButtonEnabled(false);
        }
    }
}

