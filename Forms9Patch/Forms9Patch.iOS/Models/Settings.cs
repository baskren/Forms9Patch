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


        /// <summary>
        /// Initialize the specified licenseKey.
        /// </summary>
        /// <returns>The initialize.</returns>
        /// <param name="licenseKey">License key.</param>
        public static void Initialize(string licenseKey = null)
        {
            LicenseKey = licenseKey ?? "freebee";
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
                Device.BeginInvokeOnMainThread(() => DetectDisplay());
            }
            get
            {
                return _licenseKey;
            }
        }


        #region Display Property Detection

        static void DetectDisplay()
        {
            // screen size in pixels
            var nativeSize = UIScreen.MainScreen.NativeBounds;
            Display.Width = (float)Math.Min(nativeSize.Width, nativeSize.Height);
            Display.Height = (float)Math.Max(nativeSize.Height, nativeSize.Width);
            //var name = UIDevice.CurrentDevice.Name;
            //var model = UIDevice.CurrentDevice.Model;

            // iOS point (~160 dpi) to pixel conversion.  Std displays: 1  Retina displays: 2
            Display.Scale = (float)UIScreen.MainScreen.Scale;
            //var hardwareVersion = GetSystemProperty("hw.machine");
            //Console.WriteLine ("[Forms9Patch] DD 6");
            /*
			if (nativeSize.Width == 340 && nativeSize.Height == 272)
				// Apple Watch, 38mm
				Display.Density = 290;
			else if (nativeSize.Width == 390 && nativeSize.Height == 312)
				// Apple Watch, 42mm
				Display.Density = 303;
			else if (nativeSize.Width == 320 && nativeSize.Height == 480)
				// iPhone 2/3/3S
				Display.Density = 163;
			else if (nativeSize.Width == 640 && nativeSize.Height == 960)
				// iPhone 4/4S
				Display.Density = 326;
			else if (nativeSize.Width == 640 && nativeSize.Height == 1136)
				// iPhone 5/5S/5C
				Display.Density = 326;
			else if (nativeSize.Width == 750 && nativeSize.Height == 1334)
				// iPhone 6/6S
				Display.Density = 326;
			else if (nativeSize.Width == 1080 && nativeSize.Height == 1920)
				// iPhone 6 Plus / 6S Plus
				Display.Density = 401;
			else if (nativeSize.Width == 768 && nativeSize.Height == 1024) {
				// iPad 1/2, iPad mini 1
				Display.Density = (float)(AppleScreenDesities.ContainsKey (hardwareVersion) ? AppleScreenDesities [hardwareVersion] : 163);
			}
			else if (nativeSize.Width == 1536 && nativeSize.Height == 2048)
				// iPad Mini 2/3/4, iPad 3/4, iPad Air 1/2
				Display.Density = (float)(AppleScreenDesities.ContainsKey (hardwareVersion) ? AppleScreenDesities [hardwareVersion] : 326);
			else if (nativeSize.Width == 2048 && nativeSize.Height == 2732)
				// iPad Pro
				Display.Density = 264;
			//else
			//	Display.Density = 
			*/
            Console.WriteLine(/*"hardware="+hardwareVersion+*/"[Forms9Patch]\t DD native=" + nativeSize + " scale=" + Display.Scale + " density=" + Display.Density);
        }

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
