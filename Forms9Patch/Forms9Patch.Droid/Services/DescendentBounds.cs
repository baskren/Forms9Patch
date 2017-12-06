using Forms9Patch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.Dependency(typeof(DescendentBounds))]
namespace Forms9Patch.Droid
{
    /// <summary>
    /// Descendent bounds.
    /// </summary>
    public class DescendentBounds : IDescendentBounds
    {

        #region IDescendentBounds implementation

        Rectangle IDescendentBounds.PageDescendentBounds(Page page, VisualElement element)
        {
            IVisualElementRenderer pageRenderer = Platform.GetRenderer(page);
            IVisualElementRenderer elementRenderer = Platform.GetRenderer(element);
            if (pageRenderer != null && elementRenderer != null)
            {
                /*
                var elementNativeView = elementRenderer.ViewGroup;
                var pageNativeView = pageRenderer.ViewGroup;
                */
                var elementNativeView = elementRenderer.View;
                var pageNativeView = pageRenderer.View;
                if (elementNativeView != null && pageNativeView != null)
                {
                    var elementLocation = new int[2];
                    var pageLocation = new int[2];

                    pageNativeView.GetLocationInWindow(pageLocation);
                    elementNativeView.GetLocationInWindow(elementLocation);

                    var rect = new Rectangle(
                        (elementLocation[0] - pageLocation[0]) / Display.Scale,
                        (elementLocation[1] - pageLocation[1]) / Display.Scale,
                        (elementNativeView.Width) / Display.Scale,
                        (elementNativeView.Height) / Display.Scale);
                    return rect;
                }
            }
            return new Rectangle(-1, -1, -1, -1);
        }



        #endregion
    }
}

