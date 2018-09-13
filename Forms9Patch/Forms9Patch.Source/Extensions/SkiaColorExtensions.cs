using System;
namespace Forms9Patch
{
    public static class SkiaColorExtensions
    {
        public static SkiaSharp.SKColor ToSKColor(this Xamarin.Forms.Color color)
        {
            return new SkiaSharp.SKColor((byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255), (byte)(color.A * 255));
        }
    }
}
