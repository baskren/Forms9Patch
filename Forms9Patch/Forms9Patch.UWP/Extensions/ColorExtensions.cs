
namespace Forms9Patch.UWP
{
    static class ColorExtensions
    {
        public static Windows.UI.Color ToWindowsColor(this Xamarin.Forms.Color color)
        {
            var winColor = new Windows.UI.Color
            {
                R = (byte)(color.R * 255),
                G = (byte)(color.G * 255),
                B = (byte)(color.B * 255),
                A = (byte)(color.A * 255)
            };
            return winColor;
        }

        public static Xamarin.Forms.Color ToXfColor(this Windows.UI.Color color)
        {
            return Xamarin.Forms.Color.FromRgba(color.R, color.G, color.B, color.A);
        }

        public static int ToArgb(this Windows.UI.Color color)
        {
            int a = color.A;
            int r = color.R;
            int g = color.G;
            int b = color.B;

            return a << 24 | r << 16 | g << 8 | b;
        }

        const int MarkedPixel = 255 << 24;

        public static bool IsNinePatchMark(this int argb)
        {
            return argb == MarkedPixel || (argb & 0xFF000000) == 0;
        }

        public static bool IsNotNinePatchMark(this int argb)
        {
            return (argb & 0xFF000000) != 0 && argb != MarkedPixel;
        }

        public static Windows.UI.Xaml.Media.Brush ToBrush(this Xamarin.Forms.Color color)
        {
            return new Windows.UI.Xaml.Media.SolidColorBrush(color.ToWindowsColor());
        }

    }
}
