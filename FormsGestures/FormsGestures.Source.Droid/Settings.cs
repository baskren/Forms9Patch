using System;

namespace FormsGestures.Droid
{

    public static class Settings
    {
        public static Android.App.Activity Activity { get; private set; }

        public static Android.Content.Context Context => (Android.Content.Context)Activity;

        static double _msUntilTapped = 200;
        public static TimeSpan TappedThreshold
        {
            get { return TimeSpan.FromMilliseconds(_msUntilTapped); }
            set { _msUntilTapped = value.TotalMilliseconds; }
        }

        static float _swipeVelocityThreshold = 0.1f;
        public static float SwipeVelocityThreshold
        {
            get { return Settings._swipeVelocityThreshold; }
            set { Settings._swipeVelocityThreshold = value; }
        }

        public static void Init(Android.App.Activity activity)
        {
            Activity = activity;

            P42.Utils.Environment.Init();
            P42.Utils.Environment.PlatformPathLoader = PlatformPathLoader;
            _msUntilTapped = 200;
            System.Diagnostics.Debug.WriteLine("FormsGestures.Droid.Settings.Init()");
        }

        static void PlatformPathLoader()
        {
            //P42.Utils.Environment.DocumentsPath = Context.FilesDir.Path;
            P42.Utils.Environment.ApplicationDataPath = System.IO.Path.Combine(Context.ApplicationInfo.DataDir, "AppData");
            P42.Utils.Environment.ApplicationCachePath = Context.CacheDir.Path;
            P42.Utils.Environment.TemporaryStoragePath = System.IO.Path.Combine(Context.CacheDir.Path, "tmp");
        }



    }
}

