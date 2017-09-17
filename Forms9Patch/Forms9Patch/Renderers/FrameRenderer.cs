using Xamarin.Forms;

#if __IOS__
[assembly: ExportRenderer(typeof(Forms9Patch.Frame), typeof(Forms9Patch.iOS.FrameRenderer))]
namespace Forms9Patch.iOS
#elif __DROID__
[assembly: ExportRenderer(typeof(Forms9Patch.Frame), typeof(Forms9Patch.Droid.FrameRenderer))]
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
[assembly: ExportRenderer(typeof(Forms9Patch.Frame), typeof(Forms9Patch.UWP.FrameRenderer))]
namespace Forms9Patch.UWP
#else
namespace Forms9Patch
#endif
{
    /// <summary>
    /// Forms9Patch Frame renderer.
    /// </summary>
    public class FrameRenderer : LayoutRenderer<Frame> {}
}