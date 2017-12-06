using Android.App;
using System;
//using Xamarin.Forms;
using Dalvik.SystemInterop;
using System.Reflection;
using System.Collections.Generic;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.Droid.Settings))]
namespace Forms9Patch.Droid
{
    /// <summary>
    /// Forms9Patch Settings.
    /// </summary>
    public class Settings : INativeSettings
    {
        public static Android.App.Activity Activity { get; private set; }

        static bool _valid = false;
        /// <summary>
        /// Gets a value indicating whether Forms9Patch is licensed.
        /// </summary>
        /// <value>true</value>
        /// <c>false</c>
        public bool IsLicensed
        {
            get { return _valid || Xamarin.Forms.Application.Current == null; }
        }
        internal static bool IsLicenseValid
        {
            get { return _valid || Xamarin.Forms.Application.Current == null; }
        }

        public static void Initialize(Android.App.Activity activity, string licenseKey = null)
        {
            Activity = activity;
            LicenseKey = licenseKey ?? "freebee";
        }



        static string _licenseKey;

        internal static Assembly ApplicationAssembly;
        /// <summary>
        /// Sets the Forms9Patch license key.
        /// </summary>
        /// <value>The license key.</value>
        public static string LicenseKey
        {
            private set
            {
                ApplicationAssembly = System.Reflection.Assembly.GetCallingAssembly();
                if (!string.IsNullOrEmpty(value))
                {
                    _licenseKey = value;
                    Forms9Patch.Settings.LicenseKey = _licenseKey;
                    var licenseChecker = new LicenseChecker();
                    _valid = licenseChecker.CheckLicenseKey(Settings._licenseKey, ((Activity)Xamarin.Forms.Forms.Context).Title);
                    if (!_valid)
                    {
                        Console.WriteLine(string.Format("[Forms9Patch] The LicenseKey '{0}' is not for the app '{1}'.", Settings._licenseKey, ((Activity)Xamarin.Forms.Forms.Context).Title));
                        Console.WriteLine("[Forms9Patch] You are in trial mode and will be able to render 1 scaleable image and 5 formatted strings");
                    }
                    FormsGestures.Droid.Settings.Init();
                }
            }
            get
            {
                return _licenseKey;
            }
        }

        public List<Assembly> IncludedAssemblies => throw new NotImplementedException();
    }
}
