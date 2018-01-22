using System;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Forms9Patch.RootPage), typeof(Forms9Patch.Droid.RootPageRenderer))]
namespace Forms9Patch.Droid
{
    public class RootPageRenderer : Xamarin.Forms.Platform.Android.PageRenderer
    {
    }
}
