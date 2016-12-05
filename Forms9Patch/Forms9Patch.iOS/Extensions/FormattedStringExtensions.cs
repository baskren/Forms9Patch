using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Forms9Patch.iOS
{
	static class FormattedStringExtensions
	{
		static FontExtensions.MetaFont MathMetaFont;

		internal static NSAttributedString ToNSAttributedString (this Label label, UILabel control, double fontSize=-1, EllipsePlacement ellipsePlacement = EllipsePlacement.None, int secondToLastEnd = -1, int lastLineStart = 0, int lastLineEnd = -1, int startLastVisible = -1, int midLastVisible = -1, bool twice=false) {
			F9PFormattedString formattedString = label.F9PFormattedString;

			if (formattedString == null|| formattedString == null || formattedString.Text.Length < 1)
				return null;
			var text = formattedString.Text;
			var hTMLMarkupString = formattedString as HTMLMarkupString;
			if (hTMLMarkupString != null)
				text = hTMLMarkupString.UnmarkedText;
			if (twice)
				text += text;
			int ellipsesLocation = text.Length;
			NSMutableAttributedString result;
			if (ellipsePlacement == EllipsePlacement.None)
			{
				result = new NSMutableAttributedString(text);
			}
			else
			{
				string ellipsesText;
				if (secondToLastEnd > 0 && Char.IsWhiteSpace(text[secondToLastEnd - 1]))
					ellipsesText = text.Substring(0, secondToLastEnd - 1) + "\n";
				else
					ellipsesText = text.Substring(0, secondToLastEnd);
				System.Diagnostics.Debug.WriteLine("ellipsesText=[" + ellipsesText + "]");
				if (ellipsePlacement == EllipsePlacement.Start)
				{
					ellipsesText += "…" + text.Substring(lastLineStart, lastLineEnd - lastLineStart);
					ellipsesLocation = secondToLastEnd + 1;
				}
				else if (ellipsePlacement == EllipsePlacement.Mid)
				{
					ellipsesText += text.Substring(startLastVisible, midLastVisible - startLastVisible) + "…" + text.Substring(lastLineStart, lastLineEnd - lastLineStart);
					ellipsesLocation = midLastVisible + 1;
				}
				else {
					if (Char.IsWhiteSpace(text[lastLineEnd - 1]))
					{
						ellipsesText += text.Substring(lastLineStart, lastLineEnd - lastLineStart - 1) + "…";
						ellipsesLocation = lastLineEnd;
					}
					else {
						ellipsesText += text.Substring(lastLineStart, lastLineEnd - lastLineStart) + "…";
						ellipsesLocation = lastLineEnd + 1;
					}
				}
				System.Diagnostics.Debug.WriteLine("\tellipsesText=[" + ellipsesText + "]");
				text = ellipsesText;
				result = new NSMutableAttributedString(ellipsesText);
			}

			if (!Settings.IsLicenseValid && label._id > 4)
				return new NSMutableAttributedString ("UNLICENSED COPY");

			UIFont controlFont = control.Font;
			UIFont baseFont = label.ToUIFont ();

			nfloat baseFontSize = baseFont.PointSize;
			nfloat fontScale = ((nfloat)(fontSize < 0 ? 1 : fontSize / baseFontSize));
			//if (controlFont != baseFont)
			// always set baseFont so we can modify it when trying to fit the font
			result.AddAttribute (UIStringAttributeKey.Font, baseFont.WithSize(baseFontSize * fontScale), new NSRange(0, text.Length));



			UIColor controlColor = control.TextColor;
			UIColor baseColor = label.TextColor.ToUIColor (UIColor.Black);
			if (baseColor!=null && !baseColor.Equals(controlColor))
				result.AddAttribute (UIStringAttributeKey.ForegroundColor, baseColor, new NSRange (0, text.Length));

			#region Layout font-spans (MetaFonts)
			UIFont currentFont = baseFont;
			var currentSize = currentFont.PointSize * fontScale;

			var metaFonts = new List<FontExtensions.MetaFont> ();
			var baseMetaFont = new FontExtensions.MetaFont (
				baseFont.FamilyName, 
				currentSize, 
				(baseFont.FontDescriptor.Traits.SymbolicTrait & UIFontDescriptorSymbolicTraits.Bold) > 0,
				(baseFont.FontDescriptor.Traits.SymbolicTrait & UIFontDescriptorSymbolicTraits.Italic) > 0);
			
			MathMetaFont = MathMetaFont ?? new FontExtensions.MetaFont ("STIXGeneral", currentSize);


			for (int i = 0; i < text.Length; i++)
			{
				if (i + 1 < text.Length && text[i] == '\ud835' && text[i + 1] >= '\udc00' && text[i + 1] <= '\udeff')
				{
					//			for (int i = 0; i < ((string)formattedString).Length; i++) {
					//				if (i + 1 < formattedString.Text.Length && formattedString.Text [i] == '\ud835' && formattedString.Text [i + 1] >= '\udc00' && formattedString.Text [i + 1] <= '\udeff') {
					// switch to Mathematical Alphanumeric Symbols
					metaFonts.Add (new FontExtensions.MetaFont (MathMetaFont));
					metaFonts.Add (new FontExtensions.MetaFont (MathMetaFont));  // there are two because we're using a double byte unicode character
					i++;
				} else 
					metaFonts.Add (new FontExtensions.MetaFont (baseMetaFont));
			}
			#endregion

			#region Apply non-font Spans
			foreach (var span in formattedString._spans) {
				int spanStart = span.Start;
				int spanEnd = span.End;
				switch (ellipsePlacement)
				{
					case EllipsePlacement.Mid:
						// part start if between second to last line and start of last line (only really necessary if working on "last line only" width fitting)
						if (spanStart > secondToLastEnd && spanStart < startLastVisible)
							spanStart = startLastVisible;
						// part start if between ellipses text
						if (spanStart > midLastVisible && spanStart < lastLineStart)
							spanStart = lastLineStart;
						// part end if between secont to last line and start of last line
						if (spanEnd > secondToLastEnd && spanEnd < startLastVisible)
							spanEnd = secondToLastEnd;
						// part end if between ellipses text
						if (spanEnd > midLastVisible && spanEnd < lastLineStart)
							spanEnd = midLastVisible;
						if (spanEnd <= spanStart)
							continue;

						// shift up if past ellipses
						if (spanEnd >= ellipsesLocation)
							spanEnd++;
						if (spanStart >= ellipsesLocation)
							spanStart++;

						// close parts 
						if (spanStart >= lastLineStart)
							spanStart -= (lastLineStart - midLastVisible);
						if (spanStart >= startLastVisible)
							spanStart -= (startLastVisible - secondToLastEnd);

						if (spanEnd >= lastLineStart)
							spanEnd -= (lastLineStart - midLastVisible);
						if (spanEnd >= startLastVisible)
							spanEnd -= (startLastVisible - secondToLastEnd);
						break;
					case EllipsePlacement.Start:
						// part between second to last line and last line
						if (spanStart > secondToLastEnd && spanStart < lastLineStart)
							spanStart = lastLineStart;
						if (spanEnd > secondToLastEnd && spanEnd < lastLineStart)
							spanEnd = secondToLastEnd;
						if (spanEnd <= spanStart)
							continue;

						// shift up if past ellipses
						if (spanEnd >= ellipsesLocation)
							spanEnd++;
						if (spanStart >= ellipsesLocation)
							spanStart++;

						// close part
						if (spanStart >= lastLineStart)
							spanStart = (spanStart - lastLineStart) + secondToLastEnd;
						if (spanEnd >= lastLineStart)
							spanEnd = (spanEnd - lastLineStart) + secondToLastEnd;
						break;
					case EllipsePlacement.End:
						// remove if start is beyond end
						if (spanStart > lastLineEnd)
							continue;

						// shrink if end is beyond end
						if (spanEnd > lastLineEnd)
							spanEnd = lastLineEnd;

						// part between second to last line and last line
						if (spanStart > secondToLastEnd && spanStart < lastLineStart)
							spanStart = lastLineStart;
						if (spanEnd > secondToLastEnd && spanEnd < lastLineStart)
							spanEnd = secondToLastEnd;
						if (spanEnd <= spanStart)
							continue;

						// close part
						if (spanStart >= lastLineStart)
							spanStart = (spanStart - lastLineStart) + secondToLastEnd;
						if (spanEnd >= lastLineStart)
							spanEnd = (spanEnd - lastLineStart) + secondToLastEnd;
						break;
				}
				// offset spanEnd by one because Android.
				spanEnd++;
				if (spanEnd > result.Length)
					spanEnd = (int)result.Length-1;



				NSDictionary attr;
				switch (span.Key) {
				#region Spans that change UIFont attributes
					case FontFamilySpan.SpanKey:
						for (int i = spanStart; i < spanEnd; i++) {
							var metaFont = metaFonts [i];
							metaFont.Family = ((FontFamilySpan)span).FontFamilyName;
						}
						break;
					case FontSizeSpan.SpanKey:
						for (int i = spanStart; i < spanEnd; i++) {
							var metaFont = metaFonts [i];
							float size =  ((FontSizeSpan)span).Size;
							metaFont.Size = (size < 0 ?  metaFont.Size * (-size) : size * fontScale);
						}
						break;
					case BoldSpan.SpanKey:
						for (int i = spanStart; i < spanEnd; i++) {
							var metaFont = metaFonts [i];
							metaFont.Bold = true;
						}
						break;
					case ItalicsSpan.SpanKey:
						for (int i = spanStart; i < spanEnd; i++) {
							var metaFont = metaFonts [i];
							metaFont.Italic = true;
						}
						break;
					case SuperscriptSpan.SpanKey:
						for (int i = spanStart; i < spanEnd; i++) {
							var metaFont = metaFonts [i];
							metaFont.Baseline = FontBaseline.Superscript;
						}
						break;
					case SubscriptSpan.SpanKey:
						for (int i = spanStart; i < spanEnd; i++) {
							var metaFont = metaFonts [i];
							metaFont.Baseline = FontBaseline.Subscript;
						}
						break;
					case NumeratorSpan.SpanKey:
						for (int i = spanStart; i < spanEnd; i++)
						{
							var metaFont = metaFonts[i];
							metaFont.Baseline = FontBaseline.Numerator;
						}
						break;
					case DenominatorSpan.SpanKey:
						for (int i = spanStart; i < spanEnd; i++)
						{
							var metaFont = metaFonts[i];
							metaFont.Baseline = FontBaseline.Denominator;
						}
						break;
					#endregion
				#region Font Color
				case FontColorSpan.SpanKey:
					var fontColorSpan = span as FontColorSpan;
					attr = new NSMutableDictionary ();
						attr[UIStringAttributeKey.ForegroundColor] = fontColorSpan.Color.ToUIColor();
						attr[UIStringAttributeKey.UnderlineColor] = fontColorSpan.Color.ToUIColor();
						attr[UIStringAttributeKey.StrikethroughColor] = fontColorSpan.Color.ToUIColor();
						result.AddAttributes (attr, new NSRange(spanStart, spanEnd-spanStart + (twice?1:0)));
					break;
				#endregion
						/*
				#region Blink
				case BlinkSpan.SpanKey:
						var blinkSpan = span as BlinkSpan;
						attr = new NSMutableDictionary();
						attr[UIStringAttributeKey.ForegroundColor] = blinkSpan.Color.ToUIColor();
						attr[UIStringAttributeKey.UnderlineColor] = blinkSpan.Color.ToUIColor();
						attr[UIStringAttributeKey.StrikethroughColor] = blinkSpan.Color.ToUIColor();
						result.AddAttributes(attr, new NSRange(spanStart, spanEnd - spanStart + (twice ? 1 : 0)));

						break;
					#endregion
					*/
				#region Background Color
				case BackgroundColorSpan.SpanKey:
				var backgroundColorSpan = span as BackgroundColorSpan;
				attr = new NSDictionary (UIStringAttributeKey.BackgroundColor, backgroundColorSpan.Color.ToUIColor());
				result.AddAttributes (attr, new NSRange(spanStart, spanEnd - spanStart + (twice ? 1 : 0)));
				break;
				#endregion
				#region Underline
				case UnderlineSpan.SpanKey:
					NSUnderlineStyle style =  NSUnderlineStyle.Single;
					attr = new NSDictionary ( UIStringAttributeKey.UnderlineStyle, style );
					result.AddAttributes (attr, new NSRange(spanStart, spanEnd - spanStart + (twice ? 1 : 0)));
					break;
				#endregion
				#region Strikethrough
				case StrikethroughSpan.SpanKey:
					style = NSUnderlineStyle.Single;
					attr = new NSDictionary ( UIStringAttributeKey.StrikethroughStyle, style );
					result.AddAttributes (attr, new NSRange(spanStart, spanEnd - spanStart + (twice ? 1 : 0)));
					break;
				#endregion
				}
			}
			#endregion

			#region Apply MetaFonts
			// run through MetaFonts to see if we need to set new Font attributes
			var lastMetaFont = baseMetaFont;
			int startIndex = 0;
			for (int i=0; i< metaFonts.Count; i++) {
				var metaFont = metaFonts [i];
				if (lastMetaFont != metaFont) {
					// we are at the start of a new span
					if (i > 0 && lastMetaFont != baseMetaFont) {
						// and we've been inside of a metaFont span
						var font = FontExtensions.BestFont(lastMetaFont,baseFont);
						var size = lastMetaFont.Size;
						var range = new NSRange (startIndex, i - startIndex + (twice ? 1 : 0));
						switch (lastMetaFont.Baseline)
						{
							case FontBaseline.Superscript:
								result.AddAttributes(new NSDictionary(UIStringAttributeKey.BaselineOffset, size / 2.22f, UIStringAttributeKey.Font, font), range);
								break;
							case FontBaseline.Subscript:
								result.AddAttributes(new NSDictionary(UIStringAttributeKey.BaselineOffset, -size / 6f, UIStringAttributeKey.Font, font), range);
								break;
							case FontBaseline.Numerator:
								result.AddAttributes(new NSDictionary(UIStringAttributeKey.BaselineOffset, size / 4f, UIStringAttributeKey.Font, font), range);
								break;
							//case FontBaseline.Denominator:
							//	result.AddAttributes(new NSDictionary(UIStringAttributeKey.BaselineOffset, -size / 6f, UIStringAttributeKey.Font, font), range);
							//	break;
							default:
								result.AddAttribute(UIStringAttributeKey.Font, font, range);
								break;
						}
						//System.Diagnostics.Debug.WriteLine("\tRANGE["+range.Location+","+range.Length+"]");
					}
					lastMetaFont = metaFont;
					startIndex = i;
				}
			}
			if (lastMetaFont != baseMetaFont) {
				// and we've been inside of a metaFont span
				var font = FontExtensions.BestFont(lastMetaFont,baseFont);
				var size = lastMetaFont.Size;
				var range = new NSRange (startIndex, metaFonts.Count - startIndex + (twice ? 1 : 0));
				switch (lastMetaFont.Baseline)
				{
					case FontBaseline.Superscript:
						result.AddAttributes(new NSDictionary(UIStringAttributeKey.BaselineOffset, size / 2.22f, UIStringAttributeKey.Font, font), range);
						break;
					case FontBaseline.Subscript:
						result.AddAttributes(new NSDictionary(UIStringAttributeKey.BaselineOffset, -size / 6f, UIStringAttributeKey.Font, font), range);
						break;
					case FontBaseline.Numerator:
						result.AddAttributes(new NSDictionary(UIStringAttributeKey.BaselineOffset, size / 4f, UIStringAttributeKey.Font, font), range);
						break;
					//case FontBaseline.Denominator:
					//	result.AddAttributes(new NSDictionary(UIStringAttributeKey.BaselineOffset, -size / 6f, UIStringAttributeKey.Font, font), range);
					//	break;
					default:
						result.AddAttribute(UIStringAttributeKey.Font, font, range);
						break;
				}
				//System.Diagnostics.Debug.WriteLine("\tRANGE[" + range.Location + "," + range.Length + "]");
			}
			#endregion

			return result;
		}

	}

	internal enum EllipsePlacement
	{
		None,
		Start,
		Mid,
		End,
	}

}

