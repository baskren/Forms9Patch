using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Color extension.
    /// </summary>
    public static class ColorExtension
    {
        /// <summary>
        /// Interpolates between two colors - keeping the Alpha of the first (unless it's transparent ... then its white with alpha 0);
        /// </summary>
        /// <returns>The blend.</returns>
        /// <param name="c">C.</param>
        /// <param name="c2">C2.</param>
        /// <param name="percent">Percent.</param>
        public static Color RgbHybridBlend(this Color c, Color c2, double percent)
        {
            var c1 = new Color(c.R, c.G, c.B, c.A);
            if (c1 == Color.Transparent)
                c1 = new Color(1.0, 1.0, 1.0, 0.0);
            var A = c1.A;
            var R = c1.R + (c2.R - c1.R) * percent;
            var G = c1.G + (c2.G - c1.G) * percent;
            var B = c1.B + (c2.B - c1.B) * percent;
            return new Color(R, G, B, A);
        }

        /// <summary>
        /// Interpolates between two colors
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static Color RgbaBlend(this Color c1, Color c2, double percent)
        {
            var A = c1.A + (c2.A - c1.A) * percent;
            var R = c1.R + (c2.R - c1.R) * percent;
            var G = c1.G + (c2.G - c1.G) * percent;
            var B = c1.B + (c2.B - c1.B) * percent;
            return new Color(R, G, B, A);
        }

        /// <summary>
        /// Withs the alpha.
        /// </summary>
        /// <returns>The alpha.</returns>
        /// <param name="c">C.</param>
        /// <param name="alpha">Alpha.</param>
        public static Color WithAlpha(this Color c, double alpha)
            => new Color(c.R, c.G, c.B, alpha);
        

        /// <summary>
        /// Tests if the color is one of the default values
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsDefault(this Color c)
            =>  c == default || c == Color.Default;
        

        /// <summary>
        /// Tests if the color is a default or is transparent
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsDefaultOrTransparent(this Color c)
            =>  IsDefault(c) || c.A == 0;
        

        /// <summary>
        /// Returns a Xamarin.Forms.Color's red value in byte form
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static byte ByteR(this Color color) => (byte)(color.R * 255);

        /// <summary>
        /// Returns a Xamarin.Forms.Color's green value in byte form
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static byte ByteG(this Color color) => (byte)(color.G * 255);

        /// <summary>
        /// Returns a Xamarin.Forms.Color's blue value in byte form
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static byte ByteB(this Color color) => (byte)(color.B * 255);

        /// <summary>
        /// Returns a Xamarin.Forms.Color's alpha (opacity) value in byte form
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static byte ByteA(this Color color) => (byte)(color.A * 255);

        /// <summary>
        /// Returns a string with comma separated, 0-255, integer values for color's RGB
        /// </summary>
        /// <returns>The int rgb color string.</returns>
        /// <param name="color">Color.</param>
        public static string ToIntRgbColorString(this Color color)
            => color.ByteR() + "," + color.ByteG() + "," + color.ByteB();
        
        /// <summary>
        /// Returns a sring with comma separated, 0-255, integer values for color's RGBA
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ToIntRgbaColorString(this Color color)
            => color.ByteR() + "," + color.ByteG() + "," + color.ByteB() + "," + color.ByteA();

        /// <summary>
        /// Returns a string with comma separated, 0-255, integer values for color's RGBA
        /// </summary>
        /// <returns>The int rgb color string.</returns>
        /// <param name="color">Color.</param>
        public static string ToRgbaColorString(this Color color)
        {
            return color.ToIntRgbColorString() + "," + color.ByteA();
        }

        /// <summary>
        /// Returns a 3 character hexadecimal string of a color's RGB value
        /// </summary>
        /// <returns>The hex rgb color string.</returns>
        /// <param name="color">Color.</param>
        public static string ToHexRgbColorString(this Color color)
        {
            var r = color.ByteR() >> 4;
            var g = color.ByteG() >> 4;
            var b = color.ByteB() >> 4;
            var R = r.ToString("x1");
            var G = g.ToString("x1");
            var B = b.ToString("x1");
            return R + G + B;
        }

        /// <summary>
        /// Returns a 4 character hexadecimal string of a color's ARGB value
        /// </summary>
        /// <returns>The hex rgb color string.</returns>
        /// <param name="color">Color.</param>
        public static string ToHexArgbColorString(this Color color)
        {
            var a = color.ByteA() >> 4;
            return a.ToString("x1") + color.ToHexRgbColorString();
        }

        /// <summary>
        /// Returns a 6 character hexadecimal string of a color's RRGGBB value
        /// </summary>
        /// <returns>The hex rgb color string.</returns>
        /// <param name="color">Color.</param>
        public static string ToHexRrggbbColorString(this Color color)
        {
            return color.ByteR().ToString("x2") + color.ByteG().ToString("x2") + color.ByteB().ToString("x2");
        }

        /// <summary>
        /// Returns a 8 character hexadecimal string of a color's AARRGGBB value
        /// </summary>
        /// <returns>The hex rgb color string.</returns>
        /// <param name="color">Color.</param>
        public static string ToHextAarrggbbColorString(this Color color)
        {
            return color.ByteA().ToString("x2") + color.ToHexRrggbbColorString();
        }

        /// <summary>
        /// Takes a color string, typical of an HTML tag's color attribute, and coverts it to a Xamarin.Forms color.
        /// </summary>
        /// <returns>The color.</returns>
        /// <param name="s">the color string</param>
        public static Color ToColor(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return Color.Default;
            s = s.Trim();
            if (s.ToLower().StartsWith("rgb(", StringComparison.OrdinalIgnoreCase))
            {
                //var values = s.Substring(4, s.Length - 5).Split(',').Select(int.Parse).ToArray();
                var components = s.Substring(4, s.Length - 5).Split(',');
                if (components.Length != 3)
                    throw new FormatException("Could not parse [" + s + "] into RGB integer components");
                var r = int.Parse(components[0]);
                var g = int.Parse(components[1]);
                var b = int.Parse(components[2]);
                return Color.FromRgb(r, g, b);
            }
            if (s.ToLower().StartsWith("rgba(", StringComparison.OrdinalIgnoreCase))
            {
                //var values = s.Substring(5, s.Length - 6).Split(',').Select(int.Parse).ToArray();
                var components = s.Substring(5, s.Length - 6).Split(',');
                if (components.Length != 4)
                    throw new FormatException("Could not parse [" + s + "] into RGBA integer components");
                var r = int.Parse(components[0]);
                var g = int.Parse(components[1]);
                var b = int.Parse(components[2]);
                var a = int.Parse(components[3]);
                return Color.FromRgba(r, g, b, a);
            }
            if (s.StartsWith("#", StringComparison.OrdinalIgnoreCase))
            {
                var color = Color.FromHex(s);
                return color;
            }
            var colorName = s.ToLower();
            switch (colorName)
            {
                case "aliceblue": return Color.FromHex("F0F8FF");
                case "antiquewhite": return Color.FromHex("FAEBD7");
                case "aqua": return Color.FromHex("00FFFF");
                case "aquamarine": return Color.FromHex("7FFFD4");
                case "azure": return Color.FromHex("F0FFFF");
                case "beige": return Color.FromHex("F5F5DC");
                case "bisque": return Color.FromHex("FFE4C4");
                case "black": return Color.FromHex("000000");
                case "blanchedalmond": return Color.FromHex("FFEBCD");
                case "blue": return Color.FromHex("0000FF");
                case "blueviolet": return Color.FromHex("8A2BE2");
                case "brown": return Color.FromHex("A52A2A");
                case "burlywood": return Color.FromHex("DEB887");
                case "cadetblue": return Color.FromHex("5F9EA0");
                case "chartreuse": return Color.FromHex("7FFF00");
                case "chocolate": return Color.FromHex("D2691E");
                case "coral": return Color.FromHex("FF7F50");
                case "cornflowerblue": return Color.FromHex("6495ED");
                case "cornsilk": return Color.FromHex("FFF8DC");
                case "crimson": return Color.FromHex("DC143C");
                case "cyan": return Color.FromHex("00FFFF");
                case "darkblue": return Color.FromHex("00008B");
                case "darkcyan": return Color.FromHex("008B8B");
                case "darkgoldenrod": return Color.FromHex("B8860B");
                case "darkgray": return Color.FromHex("A9A9A9");
                case "darkgrey": return Color.FromHex("A9A9A9");
                case "darkgreen": return Color.FromHex("006400");
                case "darkkhaki": return Color.FromHex("BDB76B");
                case "darkmagenta": return Color.FromHex("8B008B");
                case "darkolivegreen": return Color.FromHex("556B2F");
                case "darkorange": return Color.FromHex("FF8C00");
                case "darkorchid": return Color.FromHex("9932CC");
                case "darkred": return Color.FromHex("8B0000");
                case "darksalmon": return Color.FromHex("E9967A");
                case "darkseagreen": return Color.FromHex("8FBC8F");
                case "darkslateblue": return Color.FromHex("483D8B");
                case "darkslategray": return Color.FromHex("2F4F4F");
                case "darkslategrey": return Color.FromHex("2F4F4F");
                case "darkturquoise": return Color.FromHex("00CED1");
                case "darkviolet": return Color.FromHex("9400D3");
                case "deeppink": return Color.FromHex("FF1493");
                case "deepskyblue": return Color.FromHex("00BFFF");
                case "dimgray": return Color.FromHex("696969");
                case "dimgrey": return Color.FromHex("696969");
                case "dodgerblue": return Color.FromHex("1E90FF");
                case "firebrick": return Color.FromHex("B22222");
                case "floralwhite": return Color.FromHex("FFFAF0");
                case "forestgreen": return Color.FromHex("228B22");
                case "fuchsia": return Color.FromHex("FF00FF");
                case "gainsboro": return Color.FromHex("DCDCDC");
                case "ghostwhite": return Color.FromHex("F8F8FF");
                case "gold": return Color.FromHex("FFD700");
                case "goldenrod": return Color.FromHex("DAA520");
                case "gray": return Color.FromHex("808080");
                case "grey": return Color.FromHex("808080");
                case "green": return Color.FromHex("008000");
                case "greenyellow": return Color.FromHex("ADFF2F");
                case "honeydew": return Color.FromHex("F0FFF0");
                case "hotpink": return Color.FromHex("FF69B4");
                case "indianred ": return Color.FromHex("CD5C5C");
                case "indigo ": return Color.FromHex("4B0082");
                case "ivory": return Color.FromHex("FFFFF0");
                case "khaki": return Color.FromHex("F0E68C");
                case "lavender": return Color.FromHex("E6E6FA");
                case "lavenderblush": return Color.FromHex("FFF0F5");
                case "lawngreen": return Color.FromHex("7CFC00");
                case "lemonchiffon": return Color.FromHex("FFFACD");
                case "lightblue": return Color.FromHex("ADD8E6");
                case "lightcoral": return Color.FromHex("F08080");
                case "lightcyan": return Color.FromHex("E0FFFF");
                case "lightgoldenrodyellow": return Color.FromHex("FAFAD2");
                case "lightgray": return Color.FromHex("D3D3D3");
                case "lightgrey": return Color.FromHex("D3D3D3");
                case "lightgreen": return Color.FromHex("90EE90");
                case "lightpink": return Color.FromHex("FFB6C1");
                case "lightsalmon": return Color.FromHex("FFA07A");
                case "lightseagreen": return Color.FromHex("20B2AA");
                case "lightskyblue": return Color.FromHex("87CEFA");
                case "lightslategray": return Color.FromHex("778899");
                case "lightslategrey": return Color.FromHex("778899");
                case "lightsteelblue": return Color.FromHex("B0C4DE");
                case "lightyellow": return Color.FromHex("FFFFE0");
                case "lime": return Color.FromHex("00FF00");
                case "limegreen": return Color.FromHex("32CD32");
                case "linen": return Color.FromHex("FAF0E6");
                case "magenta": return Color.FromHex("FF00FF");
                case "maroon": return Color.FromHex("800000");
                case "mediumaquamarine": return Color.FromHex("66CDAA");
                case "mediumblue": return Color.FromHex("0000CD");
                case "mediumorchid": return Color.FromHex("BA55D3");
                case "mediumpurple": return Color.FromHex("9370DB");
                case "mediumseagreen": return Color.FromHex("3CB371");
                case "mediumslateblue": return Color.FromHex("7B68EE");
                case "mediumspringgreen": return Color.FromHex("00FA9A");
                case "mediumturquoise": return Color.FromHex("48D1CC");
                case "mediumvioletred": return Color.FromHex("C71585");
                case "midnightblue": return Color.FromHex("191970");
                case "mintcream": return Color.FromHex("F5FFFA");
                case "mistyrose": return Color.FromHex("FFE4E1");
                case "moccasin": return Color.FromHex("FFE4B5");
                case "navajowhite": return Color.FromHex("FFDEAD");
                case "navy": return Color.FromHex("000080");
                case "oldlace": return Color.FromHex("FDF5E6");
                case "olive": return Color.FromHex("808000");
                case "olivedrab": return Color.FromHex("6B8E23");
                case "orange": return Color.FromHex("FFA500");
                case "orangered": return Color.FromHex("FF4500");
                case "orchid": return Color.FromHex("DA70D6");
                case "palegoldenrod": return Color.FromHex("EEE8AA");
                case "palegreen": return Color.FromHex("98FB98");
                case "paleturquoise": return Color.FromHex("AFEEEE");
                case "palevioletred": return Color.FromHex("DB7093");
                case "papayawhip": return Color.FromHex("FFEFD5");
                case "peachpuff": return Color.FromHex("FFDAB9");
                case "peru": return Color.FromHex("CD853F");
                case "pink": return Color.FromHex("FFC0CB");
                case "plum": return Color.FromHex("DDA0DD");
                case "powderblue": return Color.FromHex("B0E0E6");
                case "purple": return Color.FromHex("800080");
                case "rebeccapurple": return Color.FromHex("663399");
                case "red": return Color.FromHex("FF0000");
                case "rosybrown": return Color.FromHex("BC8F8F");
                case "royalblue": return Color.FromHex("4169E1");
                case "saddlebrown": return Color.FromHex("8B4513");
                case "salmon": return Color.FromHex("FA8072");
                case "sandybrown": return Color.FromHex("F4A460");
                case "seagreen": return Color.FromHex("2E8B57");
                case "seashell": return Color.FromHex("FFF5EE");
                case "sienna": return Color.FromHex("A0522D");
                case "silver": return Color.FromHex("C0C0C0");
                case "skyblue": return Color.FromHex("87CEEB");
                case "slateblue": return Color.FromHex("6A5ACD");
                case "slategray": return Color.FromHex("708090");
                case "slategrey": return Color.FromHex("708090");
                case "snow": return Color.FromHex("FFFAFA");
                case "springgreen": return Color.FromHex("00FF7F");
                case "steelblue": return Color.FromHex("4682B4");
                case "tan": return Color.FromHex("D2B48C");
                case "teal": return Color.FromHex("008080");
                case "thistle": return Color.FromHex("D8BFD8");
                case "tomato": return Color.FromHex("FF6347");
                case "turquoise": return Color.FromHex("40E0D0");
                case "violet": return Color.FromHex("EE82EE");
                case "wheat": return Color.FromHex("F5DEB3");
                case "white": return Color.FromHex("FFFFFF");
                case "whitesmoke": return Color.FromHex("F5F5F5");
                case "yellow": return Color.FromHex("FFFF00");
                case "yellowgreen": return Color.FromHex("9ACD32");
            }
            return Color.Default;
        }

    }
}

