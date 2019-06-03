using System;
using Java.Lang;

namespace Forms9Patch.Droid
{
    public static class TextExtensions
    {
        public static Android.Text.StaticLayout StaticLayout(ICharSequence source, Android.Text.TextPaint paint, int width, Android.Text.Layout.Alignment align, float spacingmult, float spacingadd, bool includepad)
        {
            var builder = Android.Text.StaticLayout.Builder.Obtain(source, 0, source.Length(), paint, width).SetAlignment(align).SetLineSpacing(spacingadd, spacingmult).SetIncludePad(includepad);
            var layout = builder.Build();
            return layout;
        }

        public static Android.Text.StaticLayout StaticLayout(string source, Android.Text.TextPaint paint, int width, Android.Text.Layout.Alignment align, float spacingmult, float spacingadd, bool includepad)
        {
            var builder = Android.Text.StaticLayout.Builder.Obtain(source, 0, source.Length, paint, width).SetAlignment(align).SetLineSpacing(spacingadd, spacingmult).SetIncludePad(includepad);
            var layout = builder.Build();
            return layout;
        }


    }
}
