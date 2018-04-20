using Foundation;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ObjCRuntime;
using UIKit;
using System.Reflection;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.Settings))]
namespace Forms9Patch.iOS
{
    /// <summary>
    /// Forms9Patch Settings.
    /// </summary>
    public class Settings : ISettings
    {
        #region Fields
        internal static UIApplicationDelegate AppDelegate;
        #endregion


        #region Initialization
        /// <summary>
        /// Initialize the specified appDelegate 
        /// </summary>
        /// <returns>The initialize.</returns>
        /// <param name="appDelegate">App delegate.</param>
        /// <param name="licenseKey">License key.</param>
        public static void Initialize(UIKit.UIApplicationDelegate appDelegate, string licenseKey = null)
        {
            _initizalized = true;
            AppDelegate = appDelegate;
            if (licenseKey != null)
                System.Console.WriteLine("Forms9Patch is now open source using the MIT license ... so it's free, including for commercial use.  Why?  The more people who use it, the faster bugs will be found and fixed - which helps me and you.  So, please help get the word out - tell your friends, post on social media, write about it on the bathroom walls at work!  If you have purchased a license from me, please don't get mad - you did a good deed.  They really were not that expensive and you did a great service in encouraging me keep working on Forms9Patch.");
            FormsGestures.iOS.Settings.Init();
        }

        static bool _initizalized;
        void ISettings.LazyInit()
        {
            if (_initizalized)
                return;
            _initizalized = true;
            FormsGestures.iOS.Settings.Init();
        }
        #endregion


        public List<Assembly> IncludedAssemblies => throw new NotImplementedException();

    }
}
