using System;
using UIKit;
using Foundation;

namespace Forms9Patch.iOS
{
    /// <summary>
    /// UIT ext view extensions.
    /// </summary>
    public static class UITextViewExtensions
    {
        /// <summary>
        /// Numbers the of lines.
        /// </summary>
        /// <returns>The of lines.</returns>
        /// <param name="view">View.</param>
        public static uint NumberOfLines(this UITextView view)
        {
            var layoutManager = view.LayoutManager;
            var numberOfGlyphs = (uint)layoutManager.NumberOfGlyphs;
            //var lineRange = new NSRange(0, 1);
            nuint index = 0;
            uint numberOfLines = 0;

            while (index < numberOfGlyphs)
            {
                layoutManager.GetLineFragmentRect(index, out NSRange range);
                index = (nuint)(range.Location + range.Length);
                numberOfLines++;
            }
            return numberOfLines;
        }


        public static void PropertiesFromControlState(this UILabel label, TextControlState state)
        {
            if (state.AttributedString != null)
                label.AttributedText = state.AttributedString;
            else
                label.Text = state.Text;

            label.Font = state.Font;
            label.TextAlignment = state.HorizontalTextAlignment;
            label.LineBreakMode = state.LineBreakMode;
        }


    }
}
