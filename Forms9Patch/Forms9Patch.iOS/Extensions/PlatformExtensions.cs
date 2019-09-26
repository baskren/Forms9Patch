using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFPlatform = Xamarin.Forms.Platform.iOS.Platform;

namespace Forms9Patch.iOS
{
    internal static class PlatformExtensions
    {
        public static IVisualElementRenderer GetOrCreateRenderer(this VisualElement bindable)
        {
            if (XFPlatform.GetRenderer(bindable) is IVisualElementRenderer renderer)
                return renderer;
            
            renderer = XFPlatform.CreateRenderer(bindable);
            XFPlatform.SetRenderer(bindable, renderer);
            
            return renderer;
        }
    }
}
