using System;
using Java.Lang;

namespace Forms9Patch.Droid
{
    /// <summary>
    /// Android Text Extensions
    /// </summary>
    public static class TextExtensions
    {
        /// <summary>
        /// Generate StaticLayout
        /// </summary>
        /// <param name="charSequence"></param>
        /// <param name="paint"></param>
        /// <param name="width"></param>
        /// <param name="align"></param>
        /// <param name="spacingmult"></param>
        /// <param name="spacingadd"></param>
        /// <param name="includepad"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Notifications", "XA0001:Find issues with Android API usage", Justification = "<Pending>")]
        public static Android.Text.StaticLayout StaticLayout(ICharSequence charSequence, Android.Text.TextPaint paint, int width, Android.Text.Layout.Alignment align, float spacingmult, float spacingadd, bool includepad)
        {
            var source = charSequence ?? new Java.Lang.String("");
            //P42.Utils.DebugExtensions.Message(source.ToString(), "ENTER");
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            {
                var builder = Android.Text.StaticLayout.Builder.Obtain(source, 0, source.Length(), paint, width).SetAlignment(align).SetLineSpacing(spacingadd, spacingmult).SetIncludePad(includepad);
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.P)
                {
                    builder.SetUseLineSpacingFromFallbacks(true);
                }
                var layout = builder.Build();
                //P42.Utils.DebugExtensions.Message(source.ToString(), "EXIT");
                return layout;
            }
#pragma warning disable CS0618 // Type or member is obsolete
            return new Android.Text.StaticLayout(source, paint, width, align, spacingmult, spacingadd, includepad);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <summary>
        /// Generate StaticLayout
        /// </summary>
        /// <param name="charSequence"></param>
        /// <param name="paint"></param>
        /// <param name="width"></param>
        /// <param name="align"></param>
        /// <param name="spacingmult"></param>
        /// <param name="spacingadd"></param>
        /// <param name="includepad"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Notifications", "XA0001:Find issues with Android API usage", Justification = "<Pending>")]
        public static Android.Text.StaticLayout StaticLayout(string charSequence, Android.Text.TextPaint paint, int width, Android.Text.Layout.Alignment align, float spacingmult, float spacingadd, bool includepad)
        {
            var source = charSequence ?? "";
            //P42.Utils.DebugExtensions.Message(source, "ENTER");
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            {
                var builder = Android.Text.StaticLayout.Builder.Obtain(source, 0, source.Length, paint, width).SetAlignment(align).SetLineSpacing(spacingadd, spacingmult).SetIncludePad(includepad);
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.P)
                {
                    builder.SetUseLineSpacingFromFallbacks(true);
                }
                var layout = builder.Build();
                //P42.Utils.DebugExtensions.Message(source, "EXIT");
                return layout;
            }
            return new Android.Text.StaticLayout(source, paint, width, align, spacingmult, spacingadd, includepad);
        }


    }
}
