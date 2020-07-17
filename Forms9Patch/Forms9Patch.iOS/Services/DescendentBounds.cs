using Forms9Patch.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(DescendentBounds))]
namespace Forms9Patch.iOS
{
    /// <summary>
    /// Descendent bounds.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class DescendentBounds : IDescendentBounds
    {

        #region IDescendentBounds implementation

        Rectangle IDescendentBounds.PageDescendentBounds(Page page, VisualElement element)
        {
            if (page != null && Platform.GetRenderer(page) is IVisualElementRenderer pageRenderer)
            {
                if (element != null && Platform.GetRenderer(element) is IVisualElementRenderer elementRenderer)
                {
                    var nativeView = elementRenderer.NativeView;
                    if (nativeView != null && pageRenderer.NativeView != null)
                    {
                        var elementBounds = nativeView.Bounds;
                        var windowBounds = nativeView.ConvertRectToView(elementBounds, pageRenderer.NativeView).ToRectangle();
                        return windowBounds;
                    }
                }
            }
            return new Rectangle(-1, -1, -1, -1);
        }

        #endregion
    }
}

