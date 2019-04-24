using System;

using Xamarin.Forms;

namespace FormsGestures
{
    interface IDisplayService
    {
        //float Density { get; }

        float Scale { get; }

        float Width { get; }

        float Height { get; }

        double StatusBarOffset { get; }

        Thickness SafeAreaInset { get; }

        DisplayOrientation Orientation { get; }
    }
}

