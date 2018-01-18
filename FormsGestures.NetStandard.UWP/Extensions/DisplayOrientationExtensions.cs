using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsGestures.UWP
{
    static class DisplayOrientationExtensions
    {
        public static DisplayOrientation ToF9pDisplayOrientation(this Windows.Graphics.Display.DisplayOrientations winOrientation)
        {
            switch(winOrientation)
            {
                case Windows.Graphics.Display.DisplayOrientations.Landscape:
                    return DisplayOrientation.LandscapeLeft;
                case Windows.Graphics.Display.DisplayOrientations.LandscapeFlipped:
                    return DisplayOrientation.LandscapeRight;
                case Windows.Graphics.Display.DisplayOrientations.Portrait:
                    return DisplayOrientation.Portrait;
                case Windows.Graphics.Display.DisplayOrientations.PortraitFlipped:
                    return DisplayOrientation.PortraitUpsideDown;
                default:
                    return DisplayOrientation.Portrait;
            }
        }
    }
}
