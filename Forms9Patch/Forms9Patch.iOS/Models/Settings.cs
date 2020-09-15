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
    [Preserve(AllMembers = true)]
    public class Settings : ISettings
    {
        #region Fields
        internal static UIApplicationDelegate AppDelegate;
        #endregion


        #region Properties
        internal static bool IsInitialized { get; private set; }
        #endregion


        #region events
        internal static event EventHandler OnInitialized;
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
            LinkAssemblies();

            AppDelegate = appDelegate;

            Forms9Patch.WebViewPrintEffect.ButtheadPath = $"file://{NSBundle.MainBundle.BundlePath}/";

            if (licenseKey != null)
                System.Console.WriteLine("Forms9Patch is now open source using the MIT license ... so it's free, including for commercial use.  Why?  The more people who use it, the faster bugs will be found and fixed - which helps me and you.  So, please help get the word out - tell your friends, post on social media, write about it on the bathroom walls at work!  If you have purchased a license from me, please don't get mad - you did a good deed.  They really were not that expensive and you did a great service in encouraging me keep working on Forms9Patch.");
            P42.Utils.iOS.Settings.Init();
        }

        void ISettings.LazyInit()
        {
            if (IsInitialized)
                return;
            AppDelegate = (UIKit.UIApplicationDelegate)UIApplication.SharedApplication.Delegate;
            Init();
        }

        void Init()
        {
            IsInitialized = true;
            FormsGestures.iOS.Settings.Init();
            OnInitialized?.Invoke(null, EventArgs.Empty);
        }
        #endregion


        public List<Assembly> IncludedAssemblies => throw new NotImplementedException();


        private static void LinkAssemblies()
        {
            if (false.Equals(true))
            {
#pragma warning disable IDE0067 // Dispose objects before losing scope
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                var r1 = new ColorGradientBoxRenderer();
                var r2 = new EnhancedListViewRenderer();
                var r3 = new HardwareKeyPageRenderer();
                var r4 = new LabelRenderer();
                var r5 = new PopupBaseRenderer();

                var e1 = new EmbeddedResourceFontEffect();
                var e2 = new PopupLayerEffect();
                var e3 = new SliderStepSizeEffect();
                var e4 = new EntryClearButtonEffect();

                var c1 = new ClipboardService();
                var c2 = new MimeItemCollection();
                var c3 = new NSDataItem();
                var c4 = new LazyMimeItem();
                var c5 = new SharingService();

                var s1 = new ApplicationInfoService();
                var s2 = new AudioService();
                var s3 = new DescendentBounds();
                var s4 = new FontService();
                var s5 = new HapticService();
                var s6 = new KeyboardService();
                var s7 = new OsInfoService();
                var s8 = new PrintService();
                var s9 = new ToPdfService();
                var s10 = new ToPngService();

#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning restore IDE0067 // Dispose objects before losing scope
            }
        }
    }
}
