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
    public class Settings : ISettings
    {
        #region Properties
        public static Android.App.Activity Activity { get; private set; }

        static Android.Content.Context _context;
        /// <summary>
        /// An activity is a Context because ???  Android!
        /// </summary>
        /// <value>The context.</value>
        public static Android.Content.Context Context
        {
            get
            {
                return _context ?? Xamarin.Forms.Forms.Context;
            }
            private set
            {
                _context = value;
            }
        }

        public List<Assembly> IncludedAssemblies => throw new NotImplementedException();
        #endregion


        #region Initialization
        public static void Initialize(Android.App.Activity activity, string licenseKey = null)
        {
            Activity = activity;
            Context = activity as Android.Content.Context;
            FormsGestures.Droid.Settings.Init(Context);

            if (licenseKey != null)
                System.Console.WriteLine("Forms9Patch is now open source using the MIT license ... so it's free, including for commercial use.  Why?  The more people who use it, the faster bugs will be found and fixed - which helps me and you.  So, please help get the word out - tell your friends, post on social media, write about it on the bathroom walls at work!  If you have purchased a license from me, please don't get mad - you did a good deed.  They really were not that expensive and you did a great service in encouraging me keep working on Forms9Patch.");
        }

        bool _lastInitialized;
        void ISettings.LazyInit()
        {
            if (_lastInitialized)
                return;
            _lastInitialized = true;
            Activity = Context as Android.App.Activity;
            FormsGestures.Droid.Settings.Init(Context);
        }
        #endregion
    }
}
