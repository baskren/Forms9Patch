using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch ContentView.  Same as Frame but with different property presets.
    /// </summary>
    public class ContentView : Forms9Patch.Frame
    {
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
