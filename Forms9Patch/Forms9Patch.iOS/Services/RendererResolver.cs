using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.iOS.RendererResolver))]
namespace Forms9Patch.iOS
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class RendererResolver : Interfaces.IRendererResolver
    {
        public object GetRenderer(VisualElement element)
           => Xamarin.Forms.Platform.iOS.Platform.GetRenderer(element);

        public bool HasRenderer(VisualElement element)
           => GetRenderer(element) != null;
    }
}
