using System;
using Xamarin.Forms;
using Android;
using Android.Content.Res;
using System.Runtime.Remoting.Contexts;
using Android.Views;
using Android.Runtime;

[assembly: Dependency(typeof(FormsGestures.Droid.DisplayService))]
namespace FormsGestures.Droid
{
    public class DisplayService : IDisplayService
    {
        public float Scale => global::Android.App.Application.Context.Resources.DisplayMetrics.Density;

        public float Width => Display.IsPortrait ? Min : Max;

        public float Height => Display.IsPortrait ? Max : Min;

        public Thickness SafeAreaInset => new Xamarin.Forms.Thickness();

        public DisplayOrientation Orientation
        {
            get
            {
                var orientation = Resources.System.Configuration.Orientation;
                var rotation = Xamarin.Forms.Forms.Context.GetSystemService(Android.Content.Context.WindowService).JavaCast<IWindowManager>().DefaultDisplay.Rotation;


                switch (rotation)
                {
                    case SurfaceOrientation.Rotation0:
                        return orientation == Android.Content.Res.Orientation.Portrait ? DisplayOrientation.Portrait : DisplayOrientation.LandscapeLeft;
                    case SurfaceOrientation.Rotation90:
                        return orientation == Android.Content.Res.Orientation.Portrait ? DisplayOrientation.PortraitUpsideDown : DisplayOrientation.LandscapeLeft;
                    case SurfaceOrientation.Rotation180:
                        return orientation == Android.Content.Res.Orientation.Portrait ? DisplayOrientation.PortraitUpsideDown : DisplayOrientation.LandscapeRight;
                    default:
                        return orientation == Android.Content.Res.Orientation.Portrait ? DisplayOrientation.Portrait : DisplayOrientation.LandscapeRight;
                }

            }
        }

        float Max => Math.Max(global::Android.App.Application.Context.Resources.DisplayMetrics.WidthPixels, global::Android.App.Application.Context.Resources.DisplayMetrics.HeightPixels);

        float Min => Math.Min(global::Android.App.Application.Context.Resources.DisplayMetrics.WidthPixels, global::Android.App.Application.Context.Resources.DisplayMetrics.HeightPixels);

    }
}
