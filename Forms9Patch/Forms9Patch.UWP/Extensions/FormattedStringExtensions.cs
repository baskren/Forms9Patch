using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Controls;

namespace Forms9Patch.UWP
{
    static class FormattedStringExtensions
    {

        internal static void SetF9pFormattedText(this TextBlock textBlock, Forms9Patch.Label label)
        {
            textBlock.Inlines.Clear();
            if (label.Text != null)
            {
                textBlock.Text = label.Text;
                return;
            }
            var formattedString = label.F9PFormattedString;
            var text = formattedString?.Text;
            if (string.IsNullOrWhiteSpace(text) || label.HtmlText == text)
            {
                textBlock.Text = text;
                return;
            }

            if (formattedString is HTMLMarkupString htmlMarkupString)
                text = htmlMarkupString.UnmarkedText;

            #region Layout font-spans (MetaFonts)
            var metaFonts = new List<MetaFont>();
            var baseMetaFont = new MetaFont(
                textBlock.FontFamily.ToString(),
                textBlock.FontSize,
                textBlock.FontWeight.Weight >= Windows.UI.Text.FontWeights.Bold.Weight,
                (textBlock.FontStyle & Windows.UI.Text.FontStyle.Italic) > 0);

            var MathMetaFont = new MetaFont("STIXGeneral", textBlock.FontSize);


            for (int i = 0; i < text.Length; i++)
            {
                if (i + 1 < text.Length && text[i] == '\ud835' && text[i + 1] >= '\udc00' && text[i + 1] <= '\udeff')
                {
                    metaFonts.Add(new MetaFont(MathMetaFont));
                    metaFonts.Add(new MetaFont(MathMetaFont));  // there are two because we're using a double byte unicode character
                    i++;
                }
                else
                    metaFonts.Add(new MetaFont(baseMetaFont));
            }
            #endregion


            #region Apply non-font Spans
            foreach (var span in formattedString._spans)
            {
                int spanStart = span.Start;
                int spanEnd = span.End;

                spanEnd++;
                if (spanEnd >= text.Length)
                    spanEnd = text.Length - 1;

                for (int i = spanStart; i < spanEnd; i++)
                {
                    switch (span.Key)
                    {
                        case FontFamilySpan.SpanKey: // TextElement.FontFamily
                                metaFonts[i].Family = ((FontFamilySpan)span).FontFamilyName;
                            break;
                        case FontSizeSpan.SpanKey:  // TextElement.FontSize
                                float size = ((FontSizeSpan)span).Size;
                                metaFonts[i].Size = (size < 0 ? metaFonts[i].Size * (-size) : size);
                            break;
                        case BoldSpan.SpanKey: // Bold span // TextElement.FontWeight (Thin, ExtraLight, Light, SemiLight, Normal, Medium, SemiBold, Bold, ExtraBold, Black, ExtraBlack)
                                metaFonts[i].Bold = true;
                            break;
                        case ItalicsSpan.SpanKey: // Italic span // TextElement.FontStyle (Normal, Italic, Oblique)
                                metaFonts[i].Italic = true;
                            break;
                        case FontColorSpan.SpanKey: // TextElement.Foreground
                            metaFonts[i].TextColor = ((FontColorSpan)span).Color;
                            break;
                        case UnderlineSpan.SpanKey: // Underline span  // TextElement.TextDecorations = None, Strikethought, Underline
                            metaFonts[i].Underline = true;
                        break;
                        case StrikethroughSpan.SpanKey: // TextElement.TextDecorations = None, Strikethought, Underline
                            metaFonts[i].Strikethrough = true;
                            break;
                        case SuperscriptSpan.SpanKey: // Run with Typographic.Variants=FontVariants.Superscript while using Cambria
                                metaFonts[i].Baseline = FontBaseline.Superscript;
                            break;
                        case SubscriptSpan.SpanKey: // Run with Typographic.Varients=FontVariants.Subscript while using Cambria
                                metaFonts[i].Baseline = FontBaseline.Subscript;
                            break;
                        case NumeratorSpan.SpanKey: // no UWP solution - need to use SuperScript
                                metaFonts[i].Baseline = FontBaseline.Numerator;
                            break;
                        case DenominatorSpan.SpanKey: // no UWP solution - need to use Subscript
                                metaFonts[i].Baseline = FontBaseline.Denominator;
                            break;
                        case ActionSpan.SpanKey:  // Hyperlink span ??
                            metaFonts[i].Action = new MetaFontAction((ActionSpan)span);
                            break;
                        case BackgroundColorSpan.SpanKey: // if Win10 fall creator's update, there is a solution: create TextHighlighter, set its BackgroundColor and add the Range (Start/End) to it's Ranges, and add to TextBlock.Highlighters
                            metaFonts[i].BackgroundColor = ((BackgroundColorSpan)span).Color;
                            break;
                    }
                }
            }
            #endregion

            #region Convert MetaFonts to InLines
            var inlineColection = new List<Inline>();

            // run through MetaFonts to see if we need to set new Font attributes
            var lastMetaFont = baseMetaFont;
            int startIndex = 0;
            for (int i = 0; i < metaFonts.Count; i++)
            {
                var metaFont = metaFonts[i];
                if (lastMetaFont != metaFont)
                {
                    // we are at the start of a new span
                    if (i > 0 && lastMetaFont != baseMetaFont)
                        AddInline(textBlock, label, lastMetaFont, text, startIndex, i - startIndex - 1);
                    lastMetaFont = metaFont;
                    startIndex = i;
                }
            }
            if (lastMetaFont != baseMetaFont)
                AddInline(textBlock, label, lastMetaFont, text, startIndex, text.Length - startIndex - 1);
            #endregion

        }


        static void AddInline(TextBlock textBlock, Forms9Patch.Label label, MetaFont metaFont, string text, int startIndex, int length)
        {
            var run = new Run();

            run.Text = text.Substring(startIndex, length);
            run.FontSize = metaFont.Size;
            run.FontWeight = metaFont.Bold ? Windows.UI.Text.FontWeights.Bold : Windows.UI.Text.FontWeights.Normal;
            run.FontStyle = metaFont.Italic ? Windows.UI.Text.FontStyle.Italic : Windows.UI.Text.FontStyle.Normal;
            run.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(metaFont.TextColor.ToWindowsColor());
            run.TextDecorations = Windows.UI.Text.TextDecorations.Underline;
            run.TextDecorations |= Windows.UI.Text.TextDecorations.Strikethrough;

            switch (metaFont.Baseline)
            {
                case FontBaseline.Numerator:
                case FontBaseline.Superscript:
                    run.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Cambria");
                    Typography.SetVariants(run, Windows.UI.Xaml.FontVariants.Superscript);
                    break;

                case FontBaseline.Denominator:
                case FontBaseline.Subscript:
                    run.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Cambria");
                    Typography.SetVariants(run, Windows.UI.Xaml.FontVariants.Subscript);
                    break;
                default:
                    run.FontFamily = new Windows.UI.Xaml.Media.FontFamily(metaFont.Family);
                    break;
            }

            if (Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 5))
            {
                // Window's fall creator's update
                var highlighter = new TextHighlighter
                {
                    Background = new Windows.UI.Xaml.Media.SolidColorBrush(metaFont.BackgroundColor.ToWindowsColor()),
                    Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(metaFont.TextColor.ToWindowsColor()),
                };
                highlighter.Ranges.Add(new TextRange
                {
                    StartIndex = startIndex,
                    Length = length
                });
                textBlock.TextHighlighters.Add(highlighter);
            }

            if (metaFont.Action.IsEmpty())
                textBlock.Inlines.Add(run);
            else
            {
                var hyperlink = new Hyperlink();
                hyperlink.Inlines.Add(run);
                hyperlink.Click += (Hyperlink sender, HyperlinkClickEventArgs args) => label.Tap(metaFont.Action.Id, metaFont.Action.Href);
                textBlock.Inlines.Add(hyperlink);
            }
        }
    }
}
