using System;
namespace Forms9Patch
{
    /// <summary>
    /// Making using Skia just a little easier
    /// </summary>
    public static class SkiaColorExtensions
    {
        /// <summary>
        /// Convert Xamarin.Forms.Color to Skai SKColor
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static SkiaSharp.SKColor ToSKColor(this Xamarin.Forms.Color color)
        {
            return new SkiaSharp.SKColor((byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255), (byte)(color.A * 255));
        }
    }
}
