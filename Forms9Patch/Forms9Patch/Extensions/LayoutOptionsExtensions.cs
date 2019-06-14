using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Layout options extensions.
    /// </summary>
    public static class LayoutOptionsExtensions
    {
        /// <summary>
        /// Tos the text alignment.
        /// </summary>
        /// <returns>The text alignment.</returns>
        /// <param name="options">Options.</param>
        public static TextAlignment ToTextAlignment(this LayoutOptions options)
        {
            switch (options.Alignment)
            {
                case LayoutAlignment.Start:
                    return TextAlignment.Start;
                case LayoutAlignment.Center:
                    return TextAlignment.Center;
                case LayoutAlignment.End:
                    return TextAlignment.End;
            }
            return TextAlignment.Start;
        }

        /// <summary>
        /// Determines if two LayoutOptinos are the same
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool IsEqualTo(this LayoutOptions o1, LayoutOptions o2)
        {
            return o1.Alignment == o2.Alignment && o1.Expands == o2.Expands;
        }

        /// <summary>
        /// Because this should have been done by Xamarin
        /// </summary>
        /// <param name="layoutOptions"></param>
        /// <returns></returns>
        public static string ToString(this LayoutOptions layoutOptions)
        {
            return layoutOptions.Alignment + (layoutOptions.Expands ? "AndExpands" : null);
        }
    }
}
