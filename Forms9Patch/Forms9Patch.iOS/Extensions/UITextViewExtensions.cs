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


        internal static NSMutableAttributedString AddCharacterSpacing(this NSAttributedString attributedString, string text, double characterSpacing)
        {
            if (attributedString == null && characterSpacing == 0)
                return null;

            NSMutableAttributedString mutableAttributedString = attributedString as NSMutableAttributedString;
            if (attributedString == null || attributedString.Length == 0)
            {
                mutableAttributedString = text == null ? new NSMutableAttributedString() : new NSMutableAttributedString(text);
            }
            else
            {
                mutableAttributedString = new NSMutableAttributedString(attributedString);
            }

            AddKerningAdjustment(mutableAttributedString, text, characterSpacing);

            return mutableAttributedString;
        }

        internal static void AddKerningAdjustment(NSMutableAttributedString mutableAttributedString, string text, double characterSpacing)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (characterSpacing == 0 && !mutableAttributedString.HasCharacterAdjustment())
                    return;

                mutableAttributedString.AddAttribute
                (
                    UIStringAttributeKey.KerningAdjustment,
                    NSObject.FromObject(characterSpacing), new NSRange(0, text.Length - 1)
                );
            }
        }

        internal static bool HasCharacterAdjustment(this NSMutableAttributedString mutableAttributedString)
        {
            if (mutableAttributedString == null)
                return false;

            NSRange removalRange;
            var attributes = mutableAttributedString.GetAttributes(0, out removalRange);

            for (uint i = 0; i < attributes.Count; i++)
                if (attributes.Keys[i] is NSString nSString && nSString == UIStringAttributeKey.KerningAdjustment)
                    return true;

            return false;
        }

    }
}
