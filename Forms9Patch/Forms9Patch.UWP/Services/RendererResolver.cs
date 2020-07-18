using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.UWP.RendererResolver))]
namespace Forms9Patch.UWP
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class RendererResolver : Interfaces.IRendererResolver
    {
        public object GetRenderer(VisualElement element)
           => Xamarin.Forms.Platform.UWP.Platform.GetRenderer(element);

        public bool HasRenderer(VisualElement element)
           => GetRenderer(element) != null;
    }
}
