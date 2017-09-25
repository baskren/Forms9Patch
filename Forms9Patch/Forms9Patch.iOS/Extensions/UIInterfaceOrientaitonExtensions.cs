using System;
using UIKit;
namespace Forms9Patch.iOS
{
    public static class UIInterfaceOrientaitonExtensions
    {
        public static DisplayOrientation ToDisplayOrientation(this UIInterfaceOrientation orientation)
        {
            switch (orientation)
            {
                case UIInterfaceOrientation.Portrait:
                    return DisplayOrientation.Portrait;

                case UIInterfaceOrientation.PortraitUpsideDown:
                    return DisplayOrientation.PortraitUpsideDown;

                case UIInterfaceOrientation.LandscapeLeft:
                    return DisplayOrientation.LandscapeLeft;

                case UIInterfaceOrientation.LandscapeRight:
                    return DisplayOrientation.LandscapeRight;

                default:
                    return DisplayOrientation.Portrait;
            }
        }
    }
}
