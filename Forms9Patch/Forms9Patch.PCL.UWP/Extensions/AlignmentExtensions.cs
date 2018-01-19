using Windows.UI.Xaml;
using Xamarin.Forms;

namespace Forms9Patch.UWP
{
    internal static class AlignmentExtensions
    {
        internal static Windows.UI.Xaml.TextAlignment ToNativeTextAlignment(this Xamarin.Forms.TextAlignment alignment)
        {
            switch (alignment)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    return Windows.UI.Xaml.TextAlignment.Center;
                case Xamarin.Forms.TextAlignment.End:
                    return Windows.UI.Xaml.TextAlignment.Right;
                default:
                    return Windows.UI.Xaml.TextAlignment.Left;
            }
        }

        internal static VerticalAlignment ToNativeVerticalAlignment(this Xamarin.Forms.TextAlignment alignment)
        {
            switch (alignment)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    return VerticalAlignment.Center;
                case Xamarin.Forms.TextAlignment.End:
                    return VerticalAlignment.Bottom;
                default:
                    return VerticalAlignment.Top;
            }
        }
    }
}