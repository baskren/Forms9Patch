using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;


[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.UWP.DescendentBounds))]
namespace Forms9Patch.UWP
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    class DescendentBounds : IDescendentBounds
    {
        #region IDescendentBounds implementation

        Rectangle IDescendentBounds.PageDescendentBounds(Page page, VisualElement element)
        {
            var pageRenderer = Platform.GetRenderer(page);
            var elementRenderer = Platform.GetRenderer(element);
            if (elementRenderer?.ContainerElement is FrameworkElement elementNativeView && pageRenderer?.ContainerElement is FrameworkElement pageNativeView)
            {
                //var elementBounds = elementNativeView;
                var transform = elementNativeView.TransformToVisual(pageNativeView);

                var elementNativeBounds = new Windows.Foundation.Rect(0, 0, elementNativeView.ActualWidth, elementNativeView.ActualHeight);
                var elementInWindow = transform.TransformBounds(elementNativeBounds);
                return new Rectangle(elementInWindow.X, elementInWindow.Y, elementInWindow.Width, elementInWindow.Height);
            }
            return new Rectangle(-1, -1, -1, -1);
        }

        #endregion

    }
}
