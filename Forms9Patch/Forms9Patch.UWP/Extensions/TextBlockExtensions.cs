using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace Forms9Patch.UWP
{
    static class TextBlockExtensions
    {
        static bool _textHighlighterPresent;
        static bool _textHighlighterPresentSet;
        public static bool TextHeghLighterPresent
        {
            get
            {
                if (!_textHighlighterPresentSet)
                {
                    _textHighlighterPresent = Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Documents.TextHighlighter");
                    _textHighlighterPresentSet = true;
                }
                return _textHighlighterPresent;
            }
        }

        public static void UpdateLineBreakMode(this TextBlock textBlock, Forms9Patch.Label label)
        {
            //P42.Utils.Debug.Message(Element,"ENTER");
            //_perfectSizeValid = false;

            if (textBlock != null && label != null)
            {
                switch (label.LineBreakMode)
                {
                    case Xamarin.Forms.LineBreakMode.NoWrap:
                        textBlock.TextTrimming = TextTrimming.None;
                        textBlock.TextWrapping = TextWrapping.NoWrap;
                        break;
                    case Xamarin.Forms.LineBreakMode.WordWrap:
                        textBlock.TextTrimming = TextTrimming.None;
                        textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                        break;
                    case Xamarin.Forms.LineBreakMode.CharacterWrap:
                        textBlock.TextTrimming = TextTrimming.None;
                        textBlock.TextWrapping = TextWrapping.Wrap;
                        break;
                    case Xamarin.Forms.LineBreakMode.HeadTruncation:
                        // TODO: This truncates at the end.
                        textBlock.TextTrimming = TextTrimming.WordEllipsis;
                        textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                        break;
                    case Xamarin.Forms.LineBreakMode.TailTruncation:
                        textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                        textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                        break;
                    case Xamarin.Forms.LineBreakMode.MiddleTruncation:
                        // TODO: This truncates at the end.
                        textBlock.TextTrimming = TextTrimming.WordEllipsis;
                        textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            //P42.Utils.Debug.Message(Element,"EXIT");
        }


        /// <summary>
        /// WARNING!  Calling this will cause a crash IF target version of APP is not set to Windows10 FallCreatorsUpdate (10.0.16299.0) or greater
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="color"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public static void ApplyBackgroundColor(this Windows.UI.Xaml.Controls.TextBlock textBlock, Xamarin.Forms.Color color, int startIndex=0, int length=-1)
        {
            if (TextHeghLighterPresent)
            {

                if (length < 0)
                {
                    if (startIndex != 0)
                        return;
                    length = textBlock.Text.Length;
                    length += textBlock.Inlines.Count;
                }

                var highlighter = new TextHighlighter
                {
                    Background = new Windows.UI.Xaml.Media.SolidColorBrush(color.ToWindowsColor()),
                    //Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(metaFont.TextColor.ToWindowsColor()),
                };
                highlighter.Ranges.Add(new Windows.UI.Xaml.Documents.TextRange
                {
                    StartIndex = startIndex,
                    Length = length
                });
                textBlock.TextHighlighters.Add(highlighter);
            }
        }



        public static TextBlock Copy(this Windows.UI.Xaml.Controls.TextBlock textBlock)
        {
            if (textBlock is TextBlock source)
            {
                var result = new TextBlock();
                result.FontSize = source.FontSize;
                result.LineStackingStrategy = source.LineStackingStrategy;
                result.LineHeight = source.LineHeight;
                result.CharacterSpacing = source.CharacterSpacing;
                result.IsTextSelectionEnabled = source.IsTextSelectionEnabled;
                result.FontWeight = source.FontWeight;
                result.Padding = source.Padding;
                result.Foreground = source.Foreground;
                result.FontStyle = source.FontStyle;
                result.FontStretch = source.FontStretch;
                result.FontFamily = source.FontFamily;
                result.TextWrapping = source.TextWrapping;
                result.TextTrimming = source.TextTrimming;
                result.TextAlignment = source.TextAlignment;
                result.Text = source.Text;
                result.OpticalMarginAlignment = source.OpticalMarginAlignment;
                result.TextReadingOrder = source.TextReadingOrder;
                result.TextLineBounds = source.TextLineBounds;
                result.SelectionHighlightColor = source.SelectionHighlightColor;
                result.MaxLines = source.MaxLines;
                result.IsColorFontEnabled = source.IsColorFontEnabled;
                result.IsTextScaleFactorEnabled = source.IsTextScaleFactorEnabled;
                result.TextDecorations = source.TextDecorations;
                result.HorizontalTextAlignment = source.HorizontalTextAlignment;
                result.FlowDirection = source.FlowDirection;
                result.DataContext = source.DataContext;
                result.Name = source.Name+".Copy";
                result.MinWidth = source.MinWidth;
                result.MinHeight = source.MinHeight;
                result.MaxWidth = source.MaxWidth;
                result.MaxHeight = source.MaxHeight;
                result.Margin = source.Margin;
                result.Language = source.Language;
                result.HorizontalAlignment = source.HorizontalAlignment;
                result.VerticalAlignment = source.VerticalAlignment;
                result.Width = source.Width;
                result.Height = source.Height;
                result.Style = source.Style;
                result.RequestedTheme = source.RequestedTheme;
                result.FocusVisualSecondaryThickness = source.FocusVisualSecondaryThickness;
                result.FocusVisualSecondaryBrush = source.FocusVisualSecondaryBrush;
                result.FocusVisualPrimaryThickness = source.FocusVisualPrimaryThickness;
                result.FocusVisualPrimaryBrush = source.FocusVisualPrimaryBrush;
                result.FocusVisualMargin = source.FocusVisualMargin;
                result.AllowFocusWhenDisabled = source.AllowFocusWhenDisabled;
                result.AllowFocusOnInteraction = source.AllowFocusOnInteraction;
                result.Clip = source.Clip;
                return result;
            }
            return null;
        }
    }


}
