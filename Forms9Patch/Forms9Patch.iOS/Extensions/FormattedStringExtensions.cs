using System.Collections.Generic;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Forms9Patch.iOS
{
	static class FormattedStringExtensions
	{
		static FontExtensions.MetaFont MathMetaFont;

		internal static NSAttributedString ToNSAttributedString (this F9PFormattedString formattedString, UIFont baseFont, UIColor baseColor) //, EllipsePlacement ellipsePlacement = EllipsePlacement.None, int secondToLastEnd = -1, int lastLineStart = 0, int lastLineEnd = -1, int startLastVisible = -1, int midLastVisible = -1, bool twice=false) {
		{	
			var text = formattedString?.Text;
			if (string.IsNullOrEmpty(text))
				return null;
			var hTMLMarkupString = formattedString as HTMLMarkupString;
			if (hTMLMarkupString != null)
				text = hTMLMarkupString.UnmarkedText;
			
			var result = new NSMutableAttributedString(text);

			result.AddAttribute (UIStringAttributeKey.Font, baseFont, new NSRange(0, text.Length));
			result.AddAttribute (UIStringAttributeKey.ForegroundColor, baseColor, new NSRange (0, text.Length));

			#region Layout font-spans (MetaFonts)

			var metaFonts = new List<FontExtensions.MetaFont> ();
			var baseMetaFont = new FontExtensions.MetaFont (
				baseFont.FamilyName, 
				baseFont.PointSize, 
				(baseFont.FontDescriptor.Traits.SymbolicTrait & UIFontDescriptorSymbolicTraits.Bold) > 0,
				(baseFont.FontDescriptor.Traits.SymbolicTrait & UIFontDescriptorSymbolicTraits.Italic) > 0);
			
			MathMetaFont = MathMetaFont ?? new FontExtensions.MetaFont ("STIXGeneral", baseFont.PointSize);


			for (int i = 0; i < text.Length; i++)
			{
				if (i + 1 < text.Length && text[i] == '\ud835' && text[i + 1] >= '\udc00' && text[i + 1] <= '\udeff')
				{
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
							metaFont.Size = (size < 0 ? metaFont.Size * (-size) : size );
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
						result.AddAttributes (attr, new NSRange(spanStart, spanEnd-spanStart ));
					break;
				#endregion
				#region Background Color
				case BackgroundColorSpan.SpanKey:
				var backgroundColorSpan = span as BackgroundColorSpan;
				attr = new NSDictionary (UIStringAttributeKey.BackgroundColor, backgroundColorSpan.Color.ToUIColor());
				result.AddAttributes (attr, new NSRange(spanStart, spanEnd - spanStart ));
				break;
				#endregion
				#region Underline
				case UnderlineSpan.SpanKey:
					NSUnderlineStyle style =  NSUnderlineStyle.Single;
					attr = new NSDictionary ( UIStringAttributeKey.UnderlineStyle, style );
					result.AddAttributes (attr, new NSRange(spanStart, spanEnd - spanStart ));
					break;
				#endregion
				#region Strikethrough
				case StrikethroughSpan.SpanKey:
					style = NSUnderlineStyle.Single;
					attr = new NSDictionary ( UIStringAttributeKey.StrikethroughStyle, style );
					result.AddAttributes (attr, new NSRange(spanStart, spanEnd - spanStart ));
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
						var range = new NSRange (startIndex, i - startIndex );
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
				var range = new NSRange (startIndex, metaFonts.Count - startIndex );
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

