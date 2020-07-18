using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.Droid.RendererResolver))]
namespace Forms9Patch.Droid
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class RendererResolver : Interfaces.IRendererResolver
    {
        public object GetRenderer(VisualElement element)
           => Xamarin.Forms.Platform.Android.Platform.GetRenderer(element);

        public bool HasRenderer(VisualElement element)
           => GetRenderer(element) != null;
    }
}
