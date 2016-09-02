using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Xamarin.Forms.Platform.Android;
using System;

namespace Forms9Patch.Droid
{
	static class FormattedStringExtensions
	{
		public static SpannableStringBuilder ToSpannableString(this BaseFormattedString formattedString, EllipsePlacement ellipsePlacement=EllipsePlacement.None, int secondToLastEnd=-1, int lastLineStart=0, int lastLineEnd=-1, int startLastVisible=-1, int midLastVisible=-1) {
			//BaseFormattedString formattedString = label.FormattedText;
			if (formattedString == null|| formattedString.Text == null || formattedString.Text.Length < 1)
				return null;
			var text = formattedString.Text;
			var hTMLMarkupString = formattedString as HTMLMarkupString;
			if (hTMLMarkupString != null)
				text = hTMLMarkupString.UnmarkedText;
			SpannableStringBuilder result;
			int ellipsesLocation = text.Length;
			if (ellipsePlacement == EllipsePlacement.None)
				result = new SpannableStringBuilder(text);
			else {
				string ellipsesText;
				if (secondToLastEnd > 0 && Char.IsWhiteSpace(text[secondToLastEnd-1]))
					ellipsesText = text.Substring(0, secondToLastEnd - 1) + "\n";
				else
					ellipsesText = text.Substring(0, secondToLastEnd);
				/*
				int beforeTrimLength = ellipsesText.Length;
				if (beforeTrimLength > 0)
				{
					ellipsesText = ellipsesText.TrimEnd();
					trimmedCharacters = ellipsesText.Length - beforeTrimLength;
				}
				*/
				//System.Diagnostics.Debug.WriteLine("ellipsesText=["+ellipsesText+"]");
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
				else if (lastLineEnd > lastLineStart) {
					if (Char.IsWhiteSpace(text[lastLineEnd - 1])) {
						ellipsesText += text.Substring(lastLineStart, lastLineEnd - lastLineStart - 1) + "…";
						ellipsesLocation = lastLineEnd;
					}
					else {
						ellipsesText += text.Substring(lastLineStart, lastLineEnd - lastLineStart) + "…";
						ellipsesLocation = lastLineEnd + 1;
					}
				}
				//System.Diagnostics.Debug.WriteLine("\tellipsesText=["+ellipsesText+"]");
				result = new SpannableStringBuilder(ellipsesText);
			}
				
				

			//Typeface mathFont = Typeface.CreateFromAsset(Xamarin.Forms.Forms.Context.Assets,"Fonts/STIXGeneral.otf");
			Typeface mathFont = FontManagment.TypefaceForFontFamily("Forms9Patch.Resources.Fonts.STIXGeneral.otf");
			if (mathFont == null) 
				throw new MissingMemberException ("Failed to load STIXGeneral font.");



			// process Spans
			foreach (var span in formattedString._spans) {
				//var spanEnd = span.End + 1;
				var spanEnd = span.End;
				//if (spanEnd > text.Length)
				//	spanEnd = text.Length;
				var spanStart = span.Start;


				if (ellipsePlacement == EllipsePlacement.Mid)
				{
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
				}
				else if (ellipsePlacement == EllipsePlacement.Start)
				{
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
				}
				else if (ellipsePlacement == EllipsePlacement.End)
				{
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
				}
				// offset spanEnd by one because Android.
				spanEnd++;
				if (spanEnd > result.Length())
					spanEnd = result.Length();
				switch (span.Key) {
				case FontFamilySpan.SpanKey:
					Typeface spanTypeface = FontManagment.TypefaceForFontFamily (((FontFamilySpan)span).FontFamilyName);
					if (spanTypeface == null) {
						Console.WriteLine ("Failed to find Typeface with FontFamilyName (" + ((FontFamilySpan)span).FontFamilyName + ") for FontFamilySpan.");
						Console.WriteLine ("Ignoring FontFamilySpan");
						continue;
					}
					result.SetSpan (new CustomTypefaceSpan (spanTypeface), spanStart, spanEnd, 0);
					break;
				case FontSizeSpan.SpanKey:
					var size = ((FontSizeSpan)span).Size;
					if (size < 0)
						result.SetSpan (new RelativeSizeSpan(-size), spanStart, spanEnd, 0);
					else
						result.SetSpan (new AbsoluteSizeSpan ((int)size), spanStart, spanEnd, 0);
					break;
				case FontColorSpan.SpanKey:
					result.SetSpan (new ForegroundColorSpan (((FontColorSpan)span).Color.ToAndroid ()), spanStart, spanEnd, 0);
					break;
				case BoldSpan.SpanKey:
					result.SetSpan (new StyleSpan (TypefaceStyle.Bold), spanStart, spanEnd, 0);
					break;
				case ItalicsSpan.SpanKey:
					result.SetSpan (new StyleSpan (TypefaceStyle.Italic), spanStart, spanEnd, 0);
					break;
				case SuperscriptSpan.SpanKey:
					result.SetSpan (new RelativeSizeSpan (0.575f), spanStart, spanEnd, 0);
					//result.SetSpan (new Android.Text.Style.SuperscriptSpan (), span.Start, end, 0);
					result.SetSpan (new BaselineSpan(1f/1.5f), spanStart, spanEnd, 0);
					result.SetSpan (new StyleSpan (TypefaceStyle.Bold), spanStart, spanEnd, 0);
					break;
				case SubscriptSpan.SpanKey:
					result.SetSpan (new RelativeSizeSpan (0.6f), spanStart, spanEnd, 0);
					result.SetSpan (new BaselineSpan(-1f/2.5f), spanStart, spanEnd, 0);
					//result.SetSpan (new Android.Text.Style.SubscriptSpan (), span.Start, end, 0);
					result.SetSpan (new StyleSpan (TypefaceStyle.Bold), spanStart, spanEnd, 0);
					break;
				case BackgroundColorSpan.SpanKey:
					result.SetSpan (new Android.Text.Style.BackgroundColorSpan (((BackgroundColorSpan)span).Color.ToAndroid ()), spanStart, spanEnd, 0);
					break;
				case UnderlineSpan.SpanKey:
					result.SetSpan (new Android.Text.Style.UnderlineSpan (), spanStart, spanEnd, 0);
					break;
				case StrikethroughSpan.SpanKey:
					result.SetSpan (new Android.Text.Style.StrikethroughSpan (), spanStart, spanEnd, 0);
					break;
				}
			}

			//result.SetSpan (new StyleSpan (TypefaceStyle.Italic), 0, label.FormattedText.Text.Length, 0);

			// change font for Mathamatic Alphanumeric Unicode characters
			int mathStart = -1;
			int index = 0;
			foreach (var c in formattedString.Text.GetUnicodeCodePoints()) {
				if (c.InMathAlphanumericBlock ()) {
					if (mathStart < 0)
						mathStart = index;
				} else {
					if (mathStart > -1) {
						result.SetSpan (new CustomTypefaceSpan (mathFont), mathStart, index, 0);
						System.Console.WriteLine ("MathAlphanum:[{0}]", formattedString.Text.Substring (mathStart, index - mathStart));
						mathStart = -1;
					}
				}
				index += Char.ConvertFromUtf32 (c).Length;
			}
			if (mathStart > -1) 
				result.SetSpan (new CustomTypefaceSpan (mathFont), mathStart, index - 1, 0);
			
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
