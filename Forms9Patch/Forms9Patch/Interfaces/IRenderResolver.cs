using System;
using Xamarin.Forms;

namespace Forms9Patch.Interfaces
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    interface IRendererResolver
    {
        object GetRenderer(VisualElement element);

        bool HasRenderer(VisualElement element);
    }
}
