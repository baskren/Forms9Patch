using System;
using UIKit;
using Foundation;

namespace Forms9Patch.iOS
{
	public static class UITextViewExtensions
	{
		public static uint NumberOfLines(this UITextView view)
		{
			var layoutManager = view.LayoutManager;
			var numberOfGlyphs = (uint)layoutManager.NumberOfGlyphs;
			var lineRange = new NSRange(0, 1);
			nuint index = 0;
			uint numberOfLines = 0;

			while (index < numberOfGlyphs)
			{
				layoutManager.LineFragmentRectForGlyphAtIndex(index, ref lineRange);
				index = (nuint)(lineRange.Location + lineRange.Length);
				numberOfLines++;
			}
			return numberOfLines;
		}
	}
}
