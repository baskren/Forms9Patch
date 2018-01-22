using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch ContentView.  Same as Frame but with different property presets.
    /// </summary>
    public class ContentView : Forms9Patch.Frame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ContentView"/> class.  Forms9Patch.ContentView is same as Forms9Patch.Frame - but with different default values.
        /// </summary>
        public ContentView() : base()
        {
            BackgroundColor = Color.Transparent;
            OutlineWidth = 0;
            OutlineRadius = 0;
            HasShadow = false;
            Margin = 0;
            Padding = 0;
        }
    }
}
