using Forms9Patch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.Dependency(typeof(DescendentBounds))]
namespace Forms9Patch.Droid
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
            IVisualElementRenderer pageRenderer = Platform.GetRenderer(page);
            IVisualElementRenderer elementRenderer = Platform.GetRenderer(element);
            if (elementRenderer?.View is Android.Views.View elementNativeView && pageRenderer?.View is Android.Views.View pageNativeView)
            {
                var elementLocation = new int[2];
                var pageLocation = new int[2];

                //pageNativeView.GetLocationInWindow(pageLocation);
                //elementNativeView.GetLocationInWindow(elementLocation);
                pageNativeView.GetLocationOnScreen(pageLocation);
                elementNativeView.GetLocationOnScreen(elementLocation);
                /*
                System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": Orientation[" + ((Android.Views.IWindowManager)Droid.Settings.Context.GetSystemService(Android.Content.Context.WindowService)).DefaultDisplay.Rotation + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": pageLocation[" + string.Join(",", pageLocation) + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": elementLocation[" + string.Join(",", elementLocation) + "]");
                */
                var rect = new Rectangle(
                    (elementLocation[0] - pageLocation[0]) / Display.Scale,
                    (elementLocation[1] - pageLocation[1]) / Display.Scale,
                    (elementNativeView.Width) / Display.Scale,
                    (elementNativeView.Height) / Display.Scale);
                return rect;
            }
            return new Rectangle(-1, -1, -1, -1);
        }



        #endregion
    }
}

