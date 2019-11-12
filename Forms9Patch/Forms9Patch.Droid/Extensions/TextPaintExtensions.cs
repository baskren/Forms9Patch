using System;
using Android.Text;
using Java.Lang;
using Xamarin.Forms;

namespace Forms9Patch.Droid
{
    public static class TextPaintExtensions
    {
        #region System Values
        static float FontScale => Settings.Context.Resources.Configuration.FontScale;
        static float Scale => Settings.Context.Resources.DisplayMetrics.Density;
        #endregion


        #region Truncation
#pragma warning disable IDE0060 // Remove unused parameter
        internal static StaticLayout Truncate(string text, Forms9Patch.F9PFormattedString baseFormattedString, TextPaint paint, int availWidth, int availHeight, AutoFit fit, LineBreakMode lineBreakMode, ref int lines, ref ICharSequence textFormatted)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            StaticLayout layout=null;
            var fontMetrics = paint.GetFontMetrics();
            var fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
            var fontLeading = System.Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);
            textFormatted = ((ICharSequence)baseFormattedString?.ToSpannableString()) ?? new Java.Lang.String(text);
            if (lines > 0)
            {
                if (baseFormattedString != null)
                {
                    layout = TextExtensions.StaticLayout(textFormatted, paint, availWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);

                    if (layout.Height > availHeight)
                    {
                        var visibleLines = (int)((fontLeading + availHeight) / (fontLineHeight + fontLeading));
                        if (visibleLines < lines)
                            lines = visibleLines;
                    }
                    if (layout.LineCount > lines && lines > 0)
                    {

                        var secondToLastEnd = lines > 1 ? layout.GetLineEnd(lines - 2) : 0;
                        var start = lines > 1 ? layout.GetLineStart(layout.LineCount - 2) : 0;
                        textFormatted?.Dispose();
                        switch (lineBreakMode)
                        {
                            case LineBreakMode.HeadTruncation:
                                textFormatted = StartTruncatedFormatted(baseFormattedString, paint, secondToLastEnd, start, layout.GetLineEnd(layout.LineCount - 1), availWidth);
                                break;
                            case LineBreakMode.MiddleTruncation:
                                textFormatted = MidTruncatedFormatted(baseFormattedString, paint, secondToLastEnd, layout.GetLineStart(lines - 1), (layout.GetLineEnd(lines - 1) + layout.GetLineStart(lines - 1)) / 2 - 1, start, layout.GetLineEnd(layout.LineCount - 1), availWidth);
                                break;
                            case LineBreakMode.TailTruncation:
                                textFormatted = EndTruncatedFormatted(baseFormattedString, paint, secondToLastEnd, layout.GetLineStart(lines - 1), layout.GetLineEnd(layout.LineCount - 1), availWidth);
                                break;
                            default:
                                textFormatted = TruncatedFormatted(baseFormattedString, paint, secondToLastEnd, layout.GetLineStart(lines - 1), layout.GetLineEnd(layout.LineCount - 1), availWidth);
                                break;
                        }
                    }
                }
                else
                {
                    layout = TextExtensions.StaticLayout(text, paint, availWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
                    if (layout.Height > availHeight)
                    {
                        var visibleLines = (int)((fontLeading + availHeight) / (fontLineHeight + fontLeading));
                        if (visibleLines < lines)
                            lines = visibleLines;
                    }
                    if (layout.LineCount > lines && lines > 0)
                    {
                        var secondToLastEnd = lines > 1 ? layout.GetLineEnd(lines - 2) : 0;
                        var start = lines > 1 ? layout.GetLineStart(layout.LineCount - 2) : 0;
                        switch (lineBreakMode)
                        {
                            case LineBreakMode.HeadTruncation:
                                text = StartTruncatedLastLine(text, paint, secondToLastEnd, start, layout.GetLineEnd(layout.LineCount - 1), availWidth);
                                break;
                            case LineBreakMode.MiddleTruncation:
                                text = MidTruncatedLastLine(text, paint, secondToLastEnd, layout.GetLineStart(lines - 1), (layout.GetLineEnd(lines - 1) + layout.GetLineStart(lines - 1)) / 2 - 1, start, layout.GetLineEnd(layout.LineCount - 1), availWidth);
                                break;
                            case LineBreakMode.TailTruncation:
                                text = EndTruncatedLastLine(text, paint, secondToLastEnd, layout.GetLineStart(lines - 1), layout.GetLineEnd(layout.LineCount - 1), availWidth);
                                break;
                            default:
                                text = text.Substring(0, layout.GetLineEnd(lines - 1));
                                break;
                        }
                        textFormatted?.Dispose();
                        textFormatted = new Java.Lang.String(text);
                    }
                }
            }
            layout?.Dispose();
            var result = TextExtensions.StaticLayout(textFormatted, paint, availWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
            textFormatted?.Dispose();
            return result;
        }

        static ICharSequence TruncatedFormatted(F9PFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int start, int end, float availWidth)
        {
            return TruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, start, start, end, availWidth);
        }

        static ICharSequence TruncatedFormattedIter(F9PFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int start, int endLow, int endHigh, float availWidth)
        {
            if (endHigh - endLow <= 1)
            {
                return baseFormattedString.ToSpannableString(EllipsePlacement.Char, secondToLastEnd, start, endLow);
            }
            int mid = (endLow + endHigh) / 2;
            var formattedText = baseFormattedString.ToSpannableString(EllipsePlacement.Char, 0, start, mid);
            var layout = TextExtensions.StaticLayout(formattedText, paint, int.MaxValue - 10000, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
            var layoutWidth = layout.GetLineWidth(0);
            layout.Dispose();
            if (layoutWidth > availWidth)
                return TruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, start, endLow, mid, availWidth);
            return TruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, start, mid, endHigh, availWidth);
        }

        static ICharSequence StartTruncatedFormatted(F9PFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int start, int end, float availWidth)
        {
            return StartTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, start, end, end, availWidth);
        }

        static ICharSequence StartTruncatedFormattedIter(F9PFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int startLow, int startHigh, int end, float availWidth)
        {
            if (startHigh - startLow <= 1)
                return baseFormattedString.ToSpannableString(EllipsePlacement.Start, secondToLastEnd, startHigh, end);
            int mid = (startLow + startHigh) / 2;
            var formattedText = baseFormattedString.ToSpannableString(EllipsePlacement.Start, 0, mid, end);
            var layout = TextExtensions.StaticLayout(formattedText, paint, int.MaxValue - 10000, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
            var layoutWidth = layout.GetLineWidth(0);
            layout.Dispose();
            if (layoutWidth > availWidth)
                return StartTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, mid, startHigh, end, availWidth);
            return StartTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, startLow, mid, end, availWidth);
        }

        static ICharSequence MidTruncatedFormatted(F9PFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int startLastVisible, int midLastVisible, int start, int end, float availWidth)
        {
            return MidTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, startLastVisible, midLastVisible, start, end, end, availWidth);
        }

        static ICharSequence MidTruncatedFormattedIter(F9PFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int startLastVisible, int midLastVisible, int startLow, int startHigh, int end, float availWidth)
        {
            if (startHigh - startLow <= 1)
                return baseFormattedString.ToSpannableString(EllipsePlacement.Mid, secondToLastEnd, startHigh, end, startLastVisible, midLastVisible);
            int mid = (startLow + startHigh) / 2;
            var formattedText = baseFormattedString.ToSpannableString(EllipsePlacement.Mid, 0, mid, end, startLastVisible, midLastVisible);
            var layout = TextExtensions.StaticLayout(formattedText, paint, int.MaxValue - 10000, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
            var layoutWidth = layout.GetLineWidth(0);
            layout.Dispose();
            if (layoutWidth > availWidth)
                return MidTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, startLastVisible, midLastVisible, mid, startHigh, end, availWidth);
            return MidTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, startLastVisible, midLastVisible, startLow, mid, end, availWidth);
        }

        static ICharSequence EndTruncatedFormatted(F9PFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int start, int end, float availWidth)
        {
            return EndTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, start, start, end, availWidth);
        }

        static ICharSequence EndTruncatedFormattedIter(F9PFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int start, int endLow, int endHigh, float availWidth)
        {
            if (endHigh - endLow <= 1)
                return baseFormattedString.ToSpannableString(EllipsePlacement.End, secondToLastEnd, start, endLow);
            int mid = (endLow + endHigh) / 2;
            var formattedText = baseFormattedString.ToSpannableString(EllipsePlacement.End, 0, start, mid);
            var layout = TextExtensions.StaticLayout(formattedText, paint, int.MaxValue - 10000, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
            var layoutWidth = layout.GetLineWidth(0);
            layout.Dispose();
            if (layoutWidth > availWidth)
                return EndTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, start, endLow, mid, availWidth);
            return EndTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, start, mid, endHigh, availWidth);
        }

        static string StartTruncatedLastLine(string text, TextPaint paint, int secondToLastEnd, int start, int end, float availWidth)
        {
            return StartTruncatedIter(text, paint, secondToLastEnd, start, end, end, availWidth);
        }

        static string StartTruncatedIter(string text, TextPaint paint, int secondToLastEnd, int startLow, int startHigh, int end, float availWidth)
        {
            if (startHigh - startLow <= 1)
                return (secondToLastEnd > 0 ? text.Substring(0, secondToLastEnd).TrimEnd() + "\n" : "") + "…" + text.Substring(startHigh, end - startHigh).TrimStart();
            int mid = (startLow + startHigh) / 2;
            var lastLineText = new Java.Lang.String("…" + text.Substring(mid, end - mid).TrimStart());
            var layout = TextExtensions.StaticLayout(lastLineText, paint, int.MaxValue - 10000, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
            var layoutWidth = layout.GetLineWidth(0);
            layout.Dispose();
            if (layoutWidth > availWidth)
                return StartTruncatedIter(text, paint, secondToLastEnd, mid, startHigh, end, availWidth);
            return StartTruncatedIter(text, paint, secondToLastEnd, startLow, mid, end, availWidth);
        }

        static string MidTruncatedLastLine(string text, TextPaint paint, int secondToLastEnd, int startLastVisible, int midLastVisible, int start, int end, float availWidth)
        {
            return MidTruncatedIter(text, paint, secondToLastEnd, startLastVisible, midLastVisible, start, end, end, availWidth);
        }

        static string MidTruncatedIter(string text, TextPaint paint, int secondToLastEnd, int startLastVisible, int midLastVisible, int startLow, int startHigh, int end, float availWidth)
        {
            if (startHigh - startLow <= 1)
                return (secondToLastEnd > 0 ? text.Substring(0, secondToLastEnd).TrimEnd() + "\n" : "") + text.Substring(startLastVisible, midLastVisible - startLastVisible).TrimEnd() + "…" + text.Substring(startHigh, end - startHigh).TrimStart();
            int mid = (startLow + startHigh) / 2;
            var lastLineText = new Java.Lang.String(text.Substring(startLastVisible, midLastVisible - startLastVisible).TrimEnd() + "…" + text.Substring(mid, end - mid).TrimStart());
            var layout = TextExtensions.StaticLayout(lastLineText, paint, int.MaxValue - 10000, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
            var layoutWidth = layout.GetLineWidth(0);
            layout.Dispose();
            if (layoutWidth > availWidth)
                return MidTruncatedIter(text, paint, secondToLastEnd, startLastVisible, midLastVisible, mid, startHigh, end, availWidth);
            return MidTruncatedIter(text, paint, secondToLastEnd, startLastVisible, midLastVisible, startLow, mid, end, availWidth);
        }

        static string EndTruncatedLastLine(string text, TextPaint paint, int secondToLastEnd, int start, int end, float availWidth)
        {
            return EndTruncatedIter(text, paint, secondToLastEnd, start, start, end, availWidth);
        }

        static string EndTruncatedIter(string text, TextPaint paint, int secondToLastEnd, int start, int endLow, int endHigh, float availWidth)
        {
            if (endHigh - endLow <= 1)
                return (secondToLastEnd > 0 ? text.Substring(0, secondToLastEnd).TrimEnd() + "\n" : "") + text.Substring(start, endLow - start) + "…";
            int mid = (endLow + endHigh) / 2;
            var lastLineText = new Java.Lang.String(text.Substring(start, mid - start) + "…");
            var layout = TextExtensions.StaticLayout(lastLineText, paint, int.MaxValue - 10000, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
            var layoutWidth = layout.GetLineWidth(0);
            layout.Dispose();
            if (layoutWidth > availWidth)
                return EndTruncatedIter(text, paint, secondToLastEnd, start, endLow, mid, availWidth);
            return EndTruncatedIter(text, paint, secondToLastEnd, start, mid, endHigh, availWidth);
        }

        #endregion


        #region Fitting
        internal static float WidthFit(ICharSequence text, TextPaint paint, int lines, float min, float max, int availWidth, int availHeight)
        {
            if (availWidth > int.MaxValue / 3)
            {
                if (availHeight > int.MaxValue / 3)
                    return max;
                var fontMetrics = paint.GetFontMetrics();
                var fontLineHeight = fontMetrics.Descent * FontScale - fontMetrics.Ascent * FontScale;
                var fontLeading = System.Math.Abs(fontMetrics.Bottom * FontScale - fontMetrics.Descent * FontScale);
                var fontPixelSize = paint.TextSize * FontScale;
                var lineHeightRatio = fontLineHeight / fontPixelSize;
                var leadingRatio = fontLeading / fontPixelSize;
                float ans = ((availHeight / (lines + leadingRatio * (lines - 1))) / lineHeightRatio - 0.1f);
                return System.Math.Min(ans, max);
            }
            var result = ZeroLinesFit(text, paint, min, max, availWidth, availHeight);

            float step = (result - min) / 5;
            if (step > 0.05f)
            {
                result = DescendingWidthFit(text, paint, lines, min, result, availWidth, availHeight, step);
                while (step > 0.25f)
                {
                    step /= 5;
                    result = DescendingWidthFit(text, paint, lines, result, result + step * 5, availWidth, availHeight, step);
                }
            }
            return result;
        }

#pragma warning disable IDE0060 // Remove unused parameter
        static float DescendingWidthFit(ICharSequence text, TextPaint paint, int lines, float min, float max, int availWidth, int availHeight, float step)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            float result;
            for (result = max; result > min; result -= step)
            {
                paint.TextSize = result * Scale * FontScale;
                var layout = TextExtensions.StaticLayout(text, paint, availWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
                var layoutLineCount = layout.LineCount;
                layout.Dispose();
                if (layoutLineCount <= lines)
                    return result;
            }
            return result;
        }

        const float Precision = 0.05f;
        internal static float ZeroLinesFit(ICharSequence text, TextPaint paint, float min, float max, int availWidth, int availHeight)
        {

            if (availHeight > int.MaxValue / 3)
                return max;
            if (availWidth > int.MaxValue / 3)
            {
                var fontMetrics = paint.GetFontMetrics();
                var fontLineHeight = fontMetrics.Descent * FontScale - fontMetrics.Ascent * FontScale;
                var fontPixelSize = paint.TextSize * FontScale;
                var lineHeightRatio = fontLineHeight / fontPixelSize;

                var result = (availHeight / lineHeightRatio - 0.1f);
                return System.Math.Min(result, max);
            }

            if (max - min < Precision)
                return min;

            float mid = (max + min) / 2.0f;
            paint.TextSize = mid * Scale;
            var layout = TextExtensions.StaticLayout(text, paint, availWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
            var lineCount = layout.LineCount;
            var height = layout.Height - layout.BottomPadding + layout.TopPadding;
            if (height > availHeight)
            {
                layout.Dispose();
                return ZeroLinesFit(text, paint, min, mid, availWidth, availHeight);
            }
            if (height < availHeight)
            {
                layout.Dispose();
                return ZeroLinesFit(text, paint, mid, max, availWidth, availHeight);
            }
            float maxLineWidth = 0;
            for (int i = 0; i < lineCount; i++)
                if (layout.GetLineWidth(i) > maxLineWidth)
                    maxLineWidth = layout.GetLineWidth(i);
            layout.Dispose();
            if (maxLineWidth > availWidth)
                return ZeroLinesFit(text, paint, min, mid, availWidth, availHeight);
            if (maxLineWidth < availWidth)
                return ZeroLinesFit(text, paint, mid, max, availWidth, availHeight);
            return mid;
        }
        #endregion


    }
}
