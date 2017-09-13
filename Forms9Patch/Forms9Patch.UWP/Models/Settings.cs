using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.UWP.Settings))]
namespace Forms9Patch.UWP
{
    public class Settings : INativeSettings
    {
        static bool _valid = false;

        public bool IsLicensed
        {
            get { return _valid || Xamarin.Forms.Application.Current == null; }
        }
        internal static bool IsLicenseValid
        {
            get { return _valid || Xamarin.Forms.Application.Current == null; }
        }

        public static void Initialize(Windows.UI.Xaml.Application app, string licenseKey = null)
        {
            _app = app;
            LicenseKey = licenseKey ?? "demo key";
        }

        static string _licenseKey;
        static Windows.UI.Xaml.Application _app;

        internal static Assembly ApplicationAssembly;
        /// <summary>
        /// Sets the Forms9Patch license key.
        /// </summary>
        /// <value>The license key.</value>
        public static string LicenseKey
        {
            private set
            {
                //ApplicationAssembly = System.Reflection.Assembly.GetCallingAssembly();
                ApplicationAssembly = _app.GetType().GetTypeInfo().Assembly;
                if (!string.IsNullOrEmpty(value))
                {
                    _licenseKey = value;
                    Forms9Patch.Settings.LicenseKey = _licenseKey;
                    var licenseChecker = new LicenseChecker();
                    _valid = licenseChecker.CheckLicenseKey(Settings._licenseKey, Forms9Patch.ApplicationInfoService.Name);
                    if (!_valid)
                    {
                        var errorDialog = new Windows.UI.Xaml.Controls.ContentDialog
                        {
                            Title = "Forms9Patch licensing failure",
                            Content = "The LicenseKey [" + Settings._licenseKey + "] is not for the app [" + Forms9Patch.ApplicationInfoService.Name + "].  You are in trial mode and will be able to render 1 scaleable image and 5 formatted strings.",
                            CloseButtonText = "Whatever"
                        };
                        errorDialog.ShowAsync();
                    }
                    //FormsGestures.Droid.Settings.Init();
                }
            }
            get
            {
                return _licenseKey;
            }
        }

    }
}
