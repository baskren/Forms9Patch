using Xamarin.Forms;

#if __IOS__
[assembly: ExportRenderer(typeof(Forms9Patch.StackLayout), typeof(Forms9Patch.iOS.StackLayoutRenderer))]
namespace Forms9Patch.iOS
#elif __DROID__
[assembly: ExportRenderer(typeof(Forms9Patch.StackLayout), typeof(Forms9Patch.Droid.StackLayoutRenderer))]
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
[assembly: ExportRenderer(typeof(Forms9Patch.StackLayout), typeof(Forms9Patch.UWP.StackLayoutRenderer))]
namespace Forms9Patch.UWP
#else
namespace Forms9Patch
#endif
{
    /// <summary>
    /// Forms9Patch Stack layout renderer.
    /// </summary>
    internal class StackLayoutRenderer : F9pLayoutRenderer<StackLayout>
    {
#if __DROID3__
        public StackLayoutRenderer(Android.Content.Context context) : base(context) { }
#endif
    }
}