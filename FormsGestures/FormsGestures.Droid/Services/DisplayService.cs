using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.Views;
using Android.Content;

[assembly: Dependency(typeof(FormsGestures.Droid.DisplayService))]
namespace FormsGestures.Droid
{

    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class DisplayService : IDisplayService
    {

        public static DisplayService Current;



        Java.Lang.Ref.WeakReference _displayMetricsReference;
        Android.Util.DisplayMetrics DisplayMetrics
        {
            get
            {
                _displayMetricsReference = _displayMetricsReference ?? new Java.Lang.Ref.WeakReference(global::Android.App.Application.Context.Resources.DisplayMetrics);
                var displayMetrics = (Android.Util.DisplayMetrics)_displayMetricsReference.Get();
                if (displayMetrics == null)
                {
                    displayMetrics = global::Android.App.Application.Context.Resources.DisplayMetrics;
                    _displayMetricsReference = new Java.Lang.Ref.WeakReference(displayMetrics);
                }
                return displayMetrics;
            }
        }

        public float Density => Scale * 160;

        public float Scale => DisplayMetrics.Density;

        public float Width => (float)(Math.Min(DisplayMetrics.WidthPixels, DisplayMetrics.HeightPixels));

        public float Height => (float)(Math.Max(DisplayMetrics.WidthPixels, DisplayMetrics.HeightPixels));

        public Thickness SafeAreaInset => default;

        public double StatusBarOffset => 0.0;

        public DisplayOrientation Orientation
        {
            get
            {
                var rotation = ((IWindowManager)Droid.Settings.Context.GetSystemService(Context.WindowService)).DefaultDisplay.Rotation;
                switch (rotation)
                {
                    case SurfaceOrientation.Rotation0:
                        return DisplayOrientation.Portrait;
                    case SurfaceOrientation.Rotation180:
                        return DisplayOrientation.PortraitUpsideDown;
                    case SurfaceOrientation.Rotation90:
                        return DisplayOrientation.LandscapeLeft;
                    case SurfaceOrientation.Rotation270:
                        return DisplayOrientation.LandscapeRight;
                }
                var droidOrientation = Droid.Settings.Context.Resources.Configuration.Orientation;
                switch (droidOrientation)
                {
                    case Android.Content.Res.Orientation.Landscape:
                        return DisplayOrientation.LandscapeLeft;
                    case Android.Content.Res.Orientation.Portrait:
                        return DisplayOrientation.Portrait;
                }
                return Width > Height ? DisplayOrientation.LandscapeLeft : DisplayOrientation.Portrait;
            }
        }

        public DisplayService()
        {
            Settings.Init();
            Current = this;
        }
    }
}
