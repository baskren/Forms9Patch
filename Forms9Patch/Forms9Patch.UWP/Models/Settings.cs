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

        public List<Assembly> IncludedAssemblies => AssembliesToInclude;


        static List<Assembly> _forms9PatchAssemblies = null;
        internal static List<Assembly> Forms9PatchAssemblies
        {
            get
            {
                if (_forms9PatchAssemblies==null)
                {
                    _forms9PatchAssemblies = new List<Assembly>();
                    try  { _forms9PatchAssemblies.Add(typeof(Forms9Patch.Settings).GetTypeInfo().Assembly);  }
                    catch(Exception e) { throw new Exception("Cannot load Forms9Patch assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(Settings).GetTypeInfo().Assembly); }
                    catch (Exception e) { throw new Exception("Cannot load Forms9Patch.UWP assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(FormsGestures.Listener).GetTypeInfo().Assembly); }
                    catch (Exception e) { throw new Exception("Cannot load FormsGestures assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(NumericalMethods.Search1D).GetTypeInfo().Assembly); }
                    catch (Exception e) { throw new Exception("Cannot load NumericalMethods assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(PCL.Utils.DownloadCache).GetTypeInfo().Assembly); }
                    catch (Exception e) { throw new Exception("Cannot load PCL.Utils assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(Windows.ApplicationModel.Core.AppListEntry).GetTypeInfo().Assembly); }
                    catch (Exception e) { throw new Exception("Cannot load Windows.ApplicationModel.Core assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(PCLStorage.FileSystem).GetTypeInfo().Assembly); }
                    catch (Exception e) { throw new Exception("Cannot load PCLStorage assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(Newtonsoft.Json.JsonConvert).GetTypeInfo().Assembly); }
                    catch (Exception e) { throw new Exception("Cannot load Newtonsoft.Json assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(SkiaSharp.SKBitmap).GetTypeInfo().Assembly); }
                    catch (Exception e) { throw new Exception("Cannot load SkiaSharp assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(SkiaSharp.Views.UWP.SKXamlCanvas).GetTypeInfo().Assembly); }
                    catch (Exception e) { throw new Exception("Cannot load SkiaSharp.Views.UWP assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(SharpDX.Direct2D1.Factory).GetTypeInfo().Assembly); }
                    catch (Exception e) { throw new Exception("Cannot load SharpDX.Direct2D1 assembly"); }

                }
                return _forms9PatchAssemblies;
            }
        }
        /*= new List<Assembly>
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
            typeof(SkiaSharp.SKBitmap).GetTypeInfo().Assembly,
            typeof(SkiaSharp.Views.UWP.SKPaintGLSurfaceEventArgs).GetTypeInfo().Assembly,
            typeof(SharpDX.Direct2D1.Factory).GetTypeInfo().Assembly,
        };
        */

        static List<Assembly> _assembliesToInclude;
        public static List<Assembly> AssembliesToInclude
        {
            get
            {
                if (_assembliesToInclude==null)
                {
                    _assembliesToInclude = new List<Assembly>();
                    _assembliesToInclude.AddRange(Forms9PatchAssemblies);
                }
                return _assembliesToInclude;
            }
        }

    }
}
