using System;

using Xamarin.Forms;

namespace Forms9Patch
{
    interface IDisplayService
    {
        //float Density { get; }

        float Scale { get; }

        float Width { get; }

        float Height { get; }

        Thickness SafeAreaInset { get; }

        DisplayOrientation Orientation { get; }
    }
}

