using Xamarin.Forms;


#if __IOS__
[assembly: ExportRenderer(typeof(Forms9Patch.ManualLayout), typeof(Forms9Patch.iOS.ManualLayoutRenderer))]
namespace Forms9Patch.iOS
#elif __DROID__
[assembly: ExportRenderer(typeof(Forms9Patch.ManualLayout), typeof(Forms9Patch.Droid.ManualLayoutRenderer))]
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
[assembly: ExportRenderer(typeof(Forms9Patch.ManualLayout), typeof(Forms9Patch.UWP.ManualLayoutRenderer))]
namespace Forms9Patch.UWP
#else
namespace Forms9Patch
#endif
{
    /// <summary>
    /// Forms9Patch Manual layout renderer.
    /// </summary>
    internal class ManualLayoutRenderer : F9pLayoutRenderer<ManualLayout> { }
}

