using System;
using Xamarin.Forms;
namespace Forms9Patch.Extensions
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
    }
}
