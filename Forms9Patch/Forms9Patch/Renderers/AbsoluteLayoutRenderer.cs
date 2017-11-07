using Xamarin.Forms;


#if __IOS__
[assembly: ExportRenderer(typeof(Forms9Patch.AbsoluteLayout), typeof(Forms9Patch.iOS.AbsoluteLayoutRenderer))]
namespace Forms9Patch.iOS
#elif __DROID__
[assembly: ExportRenderer(typeof(Forms9Patch.AbsoluteLayout), typeof(Forms9Patch.Droid.AbsoluteLayoutRenderer))]
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
[assembly: ExportRenderer(typeof(Forms9Patch.AbsoluteLayout), typeof(Forms9Patch.UWP.AbsoluteLayoutRenderer))]
namespace Forms9Patch.UWP
#else
namespace Forms9Patch
#endif
{
    /// <summary>
    /// Forms9Patch Absolute layout renderer.
    /// </summary>
    internal class AbsoluteLayoutRenderer : LayoutRenderer<AbsoluteLayout> { }
}

