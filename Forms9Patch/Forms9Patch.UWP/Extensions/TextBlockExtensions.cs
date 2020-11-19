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
            //P42.Utils.DebugExtensions.Message(Element,"ENTER");
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
            //P42.Utils.DebugExtensions.Message(Element,"EXIT");
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
                var result = new TextBlock
                {
                    FontSize = source.FontSize,
                    LineStackingStrategy = source.LineStackingStrategy,
                    LineHeight = source.LineHeight,
                    CharacterSpacing = source.CharacterSpacing,
                    IsTextSelectionEnabled = source.IsTextSelectionEnabled,
                    FontWeight = source.FontWeight,
                    Padding = source.Padding,
                    Foreground = source.Foreground,
                    FontStyle = source.FontStyle,
                    FontStretch = source.FontStretch,
                    FontFamily = source.FontFamily,
                    TextWrapping = source.TextWrapping,
                    TextTrimming = source.TextTrimming,
                    TextAlignment = source.TextAlignment,
                    Text = source.Text,
                    OpticalMarginAlignment = source.OpticalMarginAlignment,
                    TextReadingOrder = source.TextReadingOrder,
                    TextLineBounds = source.TextLineBounds,
                    SelectionHighlightColor = source.SelectionHighlightColor,
                    MaxLines = source.MaxLines,
                    IsColorFontEnabled = source.IsColorFontEnabled,
                    IsTextScaleFactorEnabled = source.IsTextScaleFactorEnabled,
                    TextDecorations = source.TextDecorations,
                    HorizontalTextAlignment = source.HorizontalTextAlignment,
                    FlowDirection = source.FlowDirection,
                    DataContext = source.DataContext,
                    Name = source.Name + ".Copy",
                    MinWidth = source.MinWidth,
                    MinHeight = source.MinHeight,
                    MaxWidth = source.MaxWidth,
                    MaxHeight = source.MaxHeight,
                    Margin = source.Margin,
                    Language = source.Language,
                    HorizontalAlignment = source.HorizontalAlignment,
                    VerticalAlignment = source.VerticalAlignment,
                    Width = source.Width,
                    Height = source.Height,
                    Style = source.Style,
                    RequestedTheme = source.RequestedTheme,
                    FocusVisualSecondaryThickness = source.FocusVisualSecondaryThickness,
                    FocusVisualSecondaryBrush = source.FocusVisualSecondaryBrush,
                    FocusVisualPrimaryThickness = source.FocusVisualPrimaryThickness,
                    FocusVisualPrimaryBrush = source.FocusVisualPrimaryBrush,
                    FocusVisualMargin = source.FocusVisualMargin,
                    AllowFocusWhenDisabled = source.AllowFocusWhenDisabled,
                    AllowFocusOnInteraction = source.AllowFocusOnInteraction,
                    Clip = source.Clip
                };
                return result;
            }
            return null;
        }
    }


}
