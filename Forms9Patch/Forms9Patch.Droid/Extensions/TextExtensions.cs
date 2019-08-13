using System;
using Java.Lang;

namespace Forms9Patch.Droid
{
    public static class TextExtensions
    {
        public static Android.Text.StaticLayout StaticLayout(ICharSequence source, Android.Text.TextPaint paint, int width, Android.Text.Layout.Alignment align, float spacingmult, float spacingadd, bool includepad)
        {
            //P42.Utils.Debug.Message(source.ToString(), "ENTER");
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            {
                var builder = Android.Text.StaticLayout.Builder.Obtain(source, 0, source.Length(), paint, width).SetAlignment(align).SetLineSpacing(spacingadd, spacingmult).SetIncludePad(includepad);
                var layout = builder.Build();
                //P42.Utils.Debug.Message(source.ToString(), "EXIT");
                return layout;
            }
            return new Android.Text.StaticLayout(source, paint, width, align, spacingmult, spacingadd, includepad);
        }

        public static Android.Text.StaticLayout StaticLayout(string source, Android.Text.TextPaint paint, int width, Android.Text.Layout.Alignment align, float spacingmult, float spacingadd, bool includepad)
        {
            //P42.Utils.Debug.Message(source, "ENTER");
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            {
                var builder = Android.Text.StaticLayout.Builder.Obtain(source, 0, source.Length, paint, width).SetAlignment(align).SetLineSpacing(spacingadd, spacingmult).SetIncludePad(includepad);
                var layout = builder.Build();
                //P42.Utils.Debug.Message(source, "EXIT");
                return layout;
            }
            return new Android.Text.StaticLayout(source, paint, width, align, spacingmult, spacingadd, includepad);
        }


    }
}
