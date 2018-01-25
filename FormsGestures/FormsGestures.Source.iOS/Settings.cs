using System;
using Xamarin.Forms;
using Foundation;


namespace FormsGestures.iOS
{
    public static class Settings
    {
        /*
		static double _minimumDeltaDistance = 2.0;

		static double _minimumDeltaScale = 0.01;

		static double _minimumDeltaAngle = 0.5;
		*/

        static double _msUntilTapped;

        static Point _swipeVelocityThreshold;

        /*
		public static double MinimumDeltaDistance {
			get { return _minimumDeltaDistance; }
			set { _minimumDeltaDistance = value; }
		}

		public static double MinimumDeltaScale {
			get { return _minimumDeltaScale; }
			set { _minimumDeltaScale = value; }
		}

		public static double MinimumDeltaAngle {
			get { return _minimumDeltaAngle; }
			set { _minimumDeltaAngle = value; }
		}
		*/

        public static TimeSpan TappedThreshold
        {
            get { return TimeSpan.FromMilliseconds(_msUntilTapped); }
            set { _msUntilTapped = value.TotalMilliseconds; }
        }

        public static Point SwipeVelocityThreshold
        {
            get { return _swipeVelocityThreshold; }
            set { _swipeVelocityThreshold = value; }
        }

#if NETSTANDARD
        [System.Runtime.InteropServices.DllImport(ObjCRuntime.Constants.FoundationLibrary)]
        public static extern IntPtr NSHomeDirectory();

        static string _appDirectory;
        public static string AppDirectory
        {
            get
            {
                _appDirectory = _appDirectory ?? ((NSString)ObjCRuntime.Runtime.GetNSObject(NSHomeDirectory())).ToString();
                return _appDirectory;
            }
        }
#endif

        public static void Init()
        {
            P42.Utils.Environment.Init();
#if NETSTANDARD
            P42.Utils.Environment.PlatformPathLoader = PlatformPathLoader;
#endif
            _msUntilTapped = 200;
            _swipeVelocityThreshold = new Point(800.0, 800.0);
            //System.Diagnostics.Debug.WriteLine ("FormsGestures.iOS.Settings.Init()");
        }

        static void PlatformPathLoader()
        {
#if NETSTANDARD
            var nsError = new NSError();
            //var documentsDir = NSFileManager.DefaultManager.GetUrl(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User, null, true, out nsError);
            //if (nsError == null && !string.IsNullOrWhiteSpace(documentsDir?.Path))
            //    P42.Utils.Environment.DocumentsPath = documentsDir.Path;
            //else
            //    throw new Exception("Could not get iOS Documents Directory");

            var appSupportDir = NSFileManager.DefaultManager.GetUrl(NSSearchPathDirectory.ApplicationSupportDirectory, NSSearchPathDomain.User, null, true, out nsError);
            if (nsError == null && !string.IsNullOrWhiteSpace(appSupportDir?.Path))
                P42.Utils.Environment.ApplicationDataPath = System.IO.Path.Combine(appSupportDir.Path, NSBundle.MainBundle.BundleIdentifier);
            else
                throw new Exception("Could not get iOS Application Support Directory");


            var cacheDir = NSFileManager.DefaultManager.GetUrl(NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User, null, true, out nsError);
            if (nsError == null && !string.IsNullOrWhiteSpace(cacheDir?.Path))
                P42.Utils.Environment.ApplicationCachePath = System.IO.Path.Combine(cacheDir.Path, NSBundle.MainBundle.BundleIdentifier);
            else
                throw new Exception("Could not get iOS Cache Directory");

            // either of the following seems to work
            //var tmpDirPath = NSFileManager.DefaultManager.GetTemporaryDirectory()?.Path;
            var tmpDirPath = System.IO.Path.GetTempPath();
            P42.Utils.Environment.TemporaryStoragePath = tmpDirPath;


            var current = NSFileManager.DefaultManager.CurrentDirectory;
#endif
        }
    }
}
