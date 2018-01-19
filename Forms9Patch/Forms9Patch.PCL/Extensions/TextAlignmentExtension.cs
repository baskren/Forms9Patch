using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    internal static class TextAlignmentExtension
    {
        internal static LayoutOptions ToLayoutOptions(this TextAlignment alignment, bool expand = false)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return expand ? LayoutOptions.CenterAndExpand : LayoutOptions.Center;
                case TextAlignment.End:
                    return expand ? LayoutOptions.EndAndExpand : LayoutOptions.End;
                default:
                    return expand ? LayoutOptions.StartAndExpand : LayoutOptions.Start;
            }
        }
    }
}

