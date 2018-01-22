using Foundation;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ObjCRuntime;
using UIKit;
using System.Reflection;
//using Xamarin.Forms;
//using System.Linq;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.Settings))]
namespace Forms9Patch.iOS
{
    /// <summary>
    /// Forms9Patch Settings.
    /// </summary>
    public class Settings : INativeSettings
    {
        static bool _valid;
        /// <summary>
        /// Gets a value indicating whether this instance is licensed.
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

        internal static UIApplicationDelegate AppDelegate;


        /// <summary>
        /// Initialize the specified licenseKey.
        /// </summary>
        /// <returns>The initialize.</returns>
        /// <param name="licenseKey">License key.</param>
        public static void Initialize(UIKit.UIApplicationDelegate appDelegate, string licenseKey = null)
        {
            AppDelegate = appDelegate;
            LicenseKey = licenseKey ?? "freebee";
            //Display.SafeAreaInset = new Thickness(appDelegate.Window.SafeAreaInsets.Left, appDelegate.Window.SafeAreaInsets.Top, appDelegate.Window.SafeAreaInsets.Right, appDelegate.Window.SafeAreaInsets.Bottom);
        }

        static string _licenseKey;
        /// <summary>
        /// Sets the Forms9Patch license key.
        /// </summary>
        /// <value>The license key.</value>
        public static string LicenseKey
        {
            private set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _licenseKey = value;
                    Forms9Patch.Settings.LicenseKey = _licenseKey;
                    var licenseChecker = new LicenseChecker();
                    var keys = NSBundle.MainBundle.InfoDictionary.Keys;//.Where ((s) => ((NSString)s).Contains ((NSString)"CFBundleDisplayName"));
                    bool found = false;
                    foreach (var key in keys)
                    {
                        string keyNSString = key as NSString;
                        string keyString = keyNSString.ToString();
                        if (keyString.Contains("CFBundleName") || keyString.Contains("CFBundleDisplayName"))
                        {
                            _valid = licenseChecker.CheckLicenseKey(_licenseKey, NSBundle.MainBundle.ObjectForInfoDictionary(keyString).ToString());
                            found = true;
                        }
                        if (_valid)
                            break;
                    }
                    //var bundleNameKeys = NSBundle.MainBundle.InfoDictionary.Keys;//.Where ((s) => ((NSString)s).Contains ((NSString)"CFBundleName"));
                    //var appName = (NSBundle.MainBundle.ObjectForInfoDictionary ("CFBundleName") ?? NSBundle.MainBundle.ObjectForInfoDictionary ("CFBundleDisplayName")).ToString ();
                    //var appName = NSBundle.MainBundle.ObjectForInfoDictionary ("CFBundleDisplayName").ToString ();
                    //if (appName == null) {
                    if (!found)
                    {
                        throw new KeyNotFoundException("CFBundleDisplayName or CFBundleName key was not found.  Is it missing from Info.plist?  Are there whitespace character(s) before or after this keyname in Info.plist?  It should be <key>CFBundleDisplayName</key><string>YOUR APP NAME HERE</string>.");
                    }
                    //_valid = licenseChecker.CheckLicenseKey(_licenseKey, NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleDisplayName").ToString());

                    if (!_valid)
                    {
                        Console.WriteLine(string.Format("The LicenseKey '{0}' is not for the app '{1}' '{2}'.", _licenseKey, NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleDisplayName"), NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleName")));
                        Console.WriteLine("[Forms9Patch] You are in trial mode and will be able to render 1 scaleable image and 5 formatted strings");
                    }
                    FormsGestures.iOS.Settings.Init();
                }
                //Device.BeginInvokeOnMainThread(() => DetectDisplay());
                //DetectDisplay();
            }
            get
            {
                return _licenseKey;
            }
        }

        public List<Assembly> IncludedAssemblies => throw new NotImplementedException();


        #region Display Property Detection

        /*
        static string GetSystemProperty(string property)
        {
            var pLen = Marshal.AllocHGlobal(sizeof(int));
            sysctlbyname(property, IntPtr.Zero, pLen, IntPtr.Zero, 0);
            var length = Marshal.ReadInt32(pLen);
            var pStr = Marshal.AllocHGlobal(length);
            sysctlbyname(property, pStr, pLen, IntPtr.Zero, 0);
            return Marshal.PtrToStringAnsi(pStr);
        }

        [DllImport(Constants.SystemLibrary)]
        static extern int sysctlbyname(
            [MarshalAs(UnmanagedType.LPStr)] string property,
            IntPtr output,
            IntPtr oldLen,
            IntPtr newp,
            uint newlen);

        [DllImport(Constants.SystemLibrary)]
        static extern int sysctl(
            [MarshalAs(UnmanagedType.LPArray)] int[] name,
            uint namelen,
            out uint oldp,
            ref int oldlenp,
            IntPtr newp,
            uint newlen);

        /*
		static Dictionary<string,double> AppleScreenDesities = new Dictionary<string,double> {
			{ "iPod1,1", 163 }, // iPod touch, orginal
			{ "iPod2,1", 163 }, // iPod touch 2
			{ "iPod3,1", 163 }, // iPod touch 3
			{ "iPod4,1", 326 }, // iPod touch 4
			{ "iPod5,1", 326 }, // iPod touch 5
			{ "iPod7,1", 326 }, // iPod touch 6


			{ "iPad1,1", 132 }, // iPad, original
			{ "iPad1,2", 132 }, // iPad, original 3G

			{ "iPad2,1", 132 }, // iPad 2 wifi
			{ "iPad2,2", 132 }, // iPad 2
			{ "iPad2,3", 132 }, // iPad 2 CDMA
			{ "iPad2,4", 132 }, // iPad 2

			{ "iPad2,5", 163 }, // iPad Mini wifi, original
			{ "iPad2,6", 163 }, // iPad Mini, original
			{ "iPad2,7", 163 }, // iPad Mini wifi CDMA, original

			{ "iPad3,1", 264 }, // iPad 3 wifi
			{ "iPad3,2", 264 }, // iPad 3 wifi CDMA
			{ "iPad3,3", 264 }, // iPad 3 wifi

			{ "iPad3,4", 264 }, // iPad 4 wifi
			{ "iPad3,5", 264 }, // iPad 4
			{ "iPad3,6", 264 }, // iPad 4 GSM CDMA

			{ "iPad4,1", 264 }, // iPad Air wifi
			{ "iPad4,2", 264 }, // iPad Air wifi CSM
			{ "iPad4,3", 264 }, // iPad Air wifi CDMA

			{ "iPad4,4", 326 },	// iPad Mini (2) Retina wifi
			{ "iPad4,5", 326 },	// iPad Mini (2) Retina wifi cell
			{ "iPad4,6", 326 },	// iPad Mini (2) Retina wifi cell, china

			{ "iPad4,7", 326 }, // iPad mini 3 wifi
			{ "iPad4,8", 326 }, // iPad mini 3 wifi cell
			{ "iPad4,9", 326 },	// iPad mini 3 wifi cell, china

			{ "iPad5,1", 326 }, // iPad mini 4 wifi
			{ "iPad5,2", 326 }, // iPad mini 4 wifi cell

			{ "iPad5,3", 264 }, // iPad Air 2 wifi
			{ "iPad5,4", 264 }, // iPad Air 2 wifi cell

			{ "iPad6,7", 264 }, // iPad Pro wifi
			{ "iPad6,8", 264 }, // iPad Pro wifi cell


			{ "iPhone1,1", 163 },         // iPhone, original
			{ "iPhone1,2", 163 },         // iPhone 3 
			{ "iPhone2,1", 163 },         // iPhone 3S

			{ "iPhone3,1", 326 },			// iPhone 4
			{ "iPhone3,2", 326 },			// iPhone 4  
			{ "iPhone3,3", 326 },         // iPhone 4

			{ "iPhone4,1", 326 },         // iPhone 4S

			{ "iPhone5,1", 326 },         // iPhone 5
			{ "iPhone5,2", 326 },         // iPhone 5
			{ "iPhone5,3", 326 },         // iPhone 5C
			{ "iPhone5,4", 326 },         // iPhone 5C

			{ "iPhone6,1", 326 },         // iPhone 5S
			{ "iPhone6,2", 326 },         // iPhone 5S

			{ "iPhone7,1", 401 },         // iPhone 6 plus
			{ "iPhone7,2", 326 },         // iPhone 6

			{ "iPhone8,2", 401 },         // iPhone 6S plus
			{ "iPhone8,1", 326 },         // iPhone 6S

		};
	*/
        #endregion

    }
}
