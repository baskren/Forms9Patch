using Xamarin.Forms;

#if __IOS__
[assembly: ExportRenderer(typeof(Forms9Patch.Grid), typeof(Forms9Patch.iOS.GridRenderer))]
namespace Forms9Patch.iOS
#elif __DROID__
[assembly: ExportRenderer(typeof(Forms9Patch.Grid), typeof(Forms9Patch.Droid.GridRenderer))]
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
[assembly: ExportRenderer(typeof(Forms9Patch.Grid), typeof(Forms9Patch.UWP.GridRenderer))]
namespace Forms9Patch.UWP
#else
namespace Forms9Patch
#endif
{
    /// <summary>
    /// Forms9Patch Grid renderer.
    /// </summary>
    internal class GridRenderer : LayoutRenderer<Grid> {}
}