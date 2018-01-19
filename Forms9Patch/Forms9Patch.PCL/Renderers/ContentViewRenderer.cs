using Xamarin.Forms;

#if __IOS__
//[assembly: ExportRenderer(typeof(Forms9Patch.ContentView), typeof(Forms9Patch.iOS.ContentViewRenderer))]
namespace Forms9Patch.iOS
#elif __DROID__
//[assembly: ExportRenderer(typeof(Forms9Patch.ContentView), typeof(Forms9Patch.Droid.ContentViewRenderer))]
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
//[assembly: ExportRenderer(typeof(Forms9Patch.ContentView), typeof(Forms9Patch.UWP.ContentViewRenderer))]
namespace Forms9Patch.UWP
#else
namespace Forms9Patch
#endif
{
    /// <summary>
    /// Forms9Patch Content view renderer.
    /// </summary>
    //public class ContentViewRenderer : LayoutRenderer<ContentView> { }
}