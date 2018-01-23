using Xamarin.Forms;

#if __IOS__
[assembly: ExportRenderer(typeof(Forms9Patch.RelativeLayout), typeof(Forms9Patch.iOS.RelativeLayoutRenderer))]
namespace Forms9Patch.iOS
#elif __DROID__
[assembly: ExportRenderer(typeof(Forms9Patch.RelativeLayout), typeof(Forms9Patch.Droid.RelativeLayoutRenderer))]
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
[assembly: ExportRenderer(typeof(Forms9Patch.RelativeLayout), typeof(Forms9Patch.UWP.RelativeLayoutRenderer))]
namespace Forms9Patch.UWP
#else
namespace Forms9Patch
#endif
{
    /// <summary>
    /// Forms9Patch Relative layout renderer.
    /// </summary>
    internal class RelativeLayoutRenderer : F9pLayoutRenderer<RelativeLayout> { }
}