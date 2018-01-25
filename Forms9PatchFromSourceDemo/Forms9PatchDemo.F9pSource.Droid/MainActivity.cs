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
            LoadApplication(new Forms9PatchDemo.App());

            ActionBar.Hide();
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetHomeButtonEnabled(false);
        }
    }
}



/*
using Android.App;
using Android.Widget;
using Android.OS;

namespace Forms9PatchDemo.Droid
{
    [Activity(Label = "Forms9PatchDemo.Droid", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }
    }
}
*/

