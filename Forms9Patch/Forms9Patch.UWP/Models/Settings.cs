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
            get {
                if (Xamarin.Forms.Application.Current == null)
                    return true;
                return _valid;// && Forms9Patch.UWP.ApplicationInfoService.AppDisplayName().Result==Windows.ApplicationModel.Package.Current.DisplayName;
            }
        }
        internal static bool IsLicenseValid
        {
            get {
                if (Xamarin.Forms.Application.Current == null)
                    return true;
                return _valid;// && Forms9Patch.UWP.ApplicationInfoService.AppDisplayName().Result == Windows.ApplicationModel.Package.Current.DisplayName;
            }
        }

        public static void Initialize(Windows.UI.Xaml.Application app, string licenseKey = null)
        {
            Xamarin.Forms.DependencyService.Register<FormsGestures.UWP.DisplayService>();
            Xamarin.Forms.DependencyService.Register<FormsGestures.UWP.GestureService>();
            Xamarin.Forms.DependencyService.Register<ApplicationInfoService>();



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
                    //System.Diagnostics.Debug.WriteLine("[" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] 1");
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
                    //System.Diagnostics.Debug.WriteLine("[" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] 2");
                    //FormsGestures.Droid.Settings.Init();
                }
            }
            get
            {
                return _licenseKey;
            }
        }

        static List<Assembly> _assembliesToInclude = new List<Assembly>
        {
            typeof(Forms9Patch.Settings).GetTypeInfo().Assembly,
            typeof(Settings).GetTypeInfo().Assembly,
            typeof(FormsGestures.Listener).GetTypeInfo().Assembly,
            typeof(FormsGestures.UWP.UwpRotateEventArgs).GetTypeInfo().Assembly,
            typeof(NumericalMethods.Search1D).GetTypeInfo().Assembly,
            typeof(PCL.Utils.DownloadCache).GetTypeInfo().Assembly,
            typeof(Windows.ApplicationModel.Core.AppListEntry).GetTypeInfo().Assembly,
            typeof(PCLStorage.FileSystem).GetTypeInfo().Assembly,
            typeof(Newtonsoft.Json.JsonConvert).GetTypeInfo().Assembly,
            typeof(Windows.UI.Xaml.Media.Imaging.BitmapFactory).GetTypeInfo().Assembly,
        };
        public static List<Assembly> AssembliesToInclude => _assembliesToInclude;
    }
}
