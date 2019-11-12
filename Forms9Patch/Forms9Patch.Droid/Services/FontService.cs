// /*******************************************************************
//  *
//  * FontService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.Droid.FontService))]
namespace Forms9Patch.Droid
{
    public class FontService : IFontService
    {
        public double LineHeight(string fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            var fontMetrics = FontMetrics(fontFamily, fontSize, fontAttributes);
            var fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
            return fontLineHeight;
        }

        public double LineSpace(string fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            var fontMetrics = FontMetrics(fontFamily, fontSize, fontAttributes);
            var fontLeading = Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);
            return fontLeading;
        }

#pragma warning disable IDE0060 // Remove unused parameter
        Android.Graphics.Paint.FontMetrics FontMetrics(string fontFamily, double fontSize, FontAttributes fontAttributes)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            using (var typeface = FontManagment.TypefaceForFontFamily(fontFamily))
            using (var label = new Android.Widget.TextView(Settings.Context))
            {
                Android.Graphics.TypefaceStyle style = Android.Graphics.TypefaceStyle.Normal;
                if (fontAttributes == FontAttributes.Bold)
                    style = Android.Graphics.TypefaceStyle.Bold;
                else if (fontAttributes == FontAttributes.Italic)
                    style = Android.Graphics.TypefaceStyle.Italic;
                else if (fontAttributes != FontAttributes.None)
                    style = Android.Graphics.TypefaceStyle.BoldItalic;
                label.SetTypeface(typeface, style);
                var fontMetrics = label.Paint.GetFontMetrics();
                return fontMetrics;
            }
        }
    }
}
