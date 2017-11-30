using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace Forms9Patch.UWP
{
    static class TextBlockExtensions
    {
        /// <summary>
        /// WARNING!  Calling this will cause a crash IF target version of APP is not set to Windows10 FallCreatorsUpdate (10.0.16299.0) or greater
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="color"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public static void ApplyBackgroundColor(this Windows.UI.Xaml.Controls.TextBlock textBlock, Xamarin.Forms.Color color, int startIndex=0, int length=-1)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Documents.TextHighlighter"))
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

    }
}
