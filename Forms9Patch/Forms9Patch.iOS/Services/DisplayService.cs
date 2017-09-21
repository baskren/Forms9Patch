using System;
using UIKit;
using Xamarin.Forms;
using System.Linq;

[assembly: Dependency(typeof(Forms9Patch.iOS.DisplayService))]
namespace Forms9Patch.iOS
{
    public class DisplayService : IDisplayService
    {
        //public float Density => 163 * Scale;

        public float Scale => (float)UIScreen.MainScreen.Scale;

        public float Width => Display.IsPortrait ? Min : Max;

        public float Height => Display.IsPortrait ? Max : Min;

        public Thickness SafeAreaInset
        {
            get
            {
                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone && UIScreen.MainScreen.NativeBounds.Size.Height == 2436)
                {
                    int top = 34;
                    int bottom = 12;
                    switch (Orientation)
                    {
                        case DisplayOrientation.Portrait:
                            return new Thickness(0, top, 0, bottom);
                        case DisplayOrientation.PortraitUpsideDown:
                            return new Thickness(0, bottom, 0, top);
                        case DisplayOrientation.LandscapeLeft:
                            return new Thickness(top, 0, bottom, 0);
                        case DisplayOrientation.LandscapeRight:
                            return new Thickness(bottom, 0, top, 0);
                    }
                }
                if (UIApplication.SharedApplication.StatusBarHidden)
                    return new Thickness(0, 0, 0, 0);
                return new Thickness(0, 20, 0, 0);
            }
        }

        public DisplayOrientation Orientation
        {
            get
            {
                var currentOrientation = UIApplication.SharedApplication.StatusBarOrientation;
                return currentOrientation.ToDisplayOrientation();
            }
        }

        float Min => (float)Math.Min(UIScreen.MainScreen.NativeBounds.Width, UIScreen.MainScreen.NativeBounds.Height);

        float Max => (float)Math.Max(UIScreen.MainScreen.NativeBounds.Width, UIScreen.MainScreen.NativeBounds.Height);

        // iPhoneX  34, 44


    }
}

