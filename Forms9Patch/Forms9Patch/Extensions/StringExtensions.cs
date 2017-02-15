using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// String extensions.
	/// </summary>
	static class StringExtensions
	{
		internal static IEnumerable<int> GetUnicodeCodePoints(this string s) {
			for (int i = 0; i < s.Length; i++) {
				int unicodeCodePoint = char.ConvertToUtf32(s, i);
				if (unicodeCodePoint > 0xffff)
					i++;
				yield return unicodeCodePoint;
			}
		}

		internal static double ToFontSize(this string sizeString) {
			var s = sizeString;
			double size;
			var element = new Xamarin.Forms.Label ();
			if (s.EndsWith ("px",StringComparison.Ordinal)) {
				var subString = s.Substring(0, s.Length - 2);
				if (double.TryParse (subString, NumberStyles.Float, CultureInfo.InvariantCulture, out size)) 
					return size;
				throw new FormatException ("Cannot parse ["+s+"]["+subString+"] ["+size+"] px font size");
			}
			if (s.EndsWith ("em",StringComparison.Ordinal)) {
				var subString = s.Substring(0, s.Length - 2);
				if (double.TryParse(subString, NumberStyles.Float, CultureInfo.InvariantCulture, out size))
					return -size;
				throw new FormatException("Cannot parse [" + s + "][" + subString + "] [" + size + "] em font size");
			}
			if (s.EndsWith("%", StringComparison.Ordinal))
			{
				var subString = s.Substring(0, s.Length - 1);
				if (double.TryParse(subString, NumberStyles.Float, CultureInfo.InvariantCulture, out size))
					return -size / 100.0;
				throw new FormatException("Cannot parse [" + s + "][" + subString + "] [" + size + "] % font size");
			}
			switch (s.ToLower ()) {
			case "xx-small":
				//return Device.GetNamedSize (NamedSize.Micro, element) - (Device.GetNamedSize (NamedSize.Small, element) - Device.GetNamedSize (NamedSize.Micro, element)) ;
				return -10.0/17.0;
			case "x-small": case "1": case "-2":
				//return Device.GetNamedSize (NamedSize.Micro, element);
				return -12.0/17.0;
			case "small": case "2": case "-1":
				//return Device.GetNamedSize (NamedSize.Small, element);
				return -14.0/17.0;
			case "medium": case "3":
				//return Device.GetNamedSize (NamedSize.Medium, element);
				return -1.0;
			case "large": case "4": case "+1":
				//return Device.GetNamedSize (NamedSize.Large, element);
				return -22.0/17.0;
			case "x-large":
			case "5": case "+2":
				//return Device.GetNamedSize (NamedSize.Large, element) + (Device.GetNamedSize (NamedSize.Large, element) - Device.GetNamedSize (NamedSize.Medium, element));
				return -27.0/17.0;
			case "xx-large": case "6": case "+3":
				//return Device.GetNamedSize (NamedSize.Large, element) + (Device.GetNamedSize (NamedSize.Large, element) - Device.GetNamedSize (NamedSize.Medium, element)) * 2.0;
				return -32.0/17.0;
			case "7": case "+4":
				//return Device.GetNamedSize (NamedSize.Large, element) + (Device.GetNamedSize (NamedSize.Large, element) - Device.GetNamedSize (NamedSize.Medium, element)) * 3.0;
				return -37.0/17.0;
			case "initial":
				return -1;
			}
			if (double.TryParse (s, NumberStyles.Float, CultureInfo.InvariantCulture, out size)) {
				if (size < 1)
					return Device.GetNamedSize (NamedSize.Micro, element);
				if (size > 7)
					return Device.GetNamedSize (NamedSize.Large, element) + (Device.GetNamedSize (NamedSize.Large, element) - Device.GetNamedSize (NamedSize.Medium, element)) * 3.0;
			}
			return 0;
		}

		public static Color ToColor(this string s) {
			if (s.ToLower ().StartsWith ("rgb(", StringComparison.Ordinal)) {
				var values = s.Substring (4, s.Length - 5).Split (',').Select (int.Parse).ToArray ();
				if (values.Length != 3)
					throw new FormatException ("Could not parse ["+s+"] into RGB integer components");
				return Color.FromRgb (values [0], values [1], values [2]);
			} else if (s.ToLower().StartsWith("rgba(", StringComparison.Ordinal)) {
				var values = s.Substring (5, s.Length - 6).Split (',').Select (int.Parse).ToArray ();
				if (values.Length != 4)
					throw new FormatException ("Could not parse ["+s+"] into RGBA integer components");
				return Color.FromRgba (values [0], values [1], values [2], values[3]);
			} else if (s.StartsWith ("#", StringComparison.Ordinal)) {
				return Color.FromHex (s.Substring (1));
			} else {
				var colorName = s.ToLower ();
				switch (colorName) {
				case "aliceblue": return Color.FromHex ("F0F8FF");
				case "antiquewhite": return Color.FromHex ("FAEBD7");
				case "aqua": return Color.FromHex ("00FFFF");
				case "aquamarine": return Color.FromHex ("7FFFD4");
				case "azure": return Color.FromHex ("F0FFFF");
				case "beige": return Color.FromHex ("F5F5DC");
				case "bisque": return Color.FromHex ("FFE4C4");
				case "black": return Color.FromHex ("000000");
				case "blanchedalmond": return Color.FromHex ("FFEBCD");
				case "blue": return Color.FromHex ("0000FF");
				case "blueviolet": return Color.FromHex ("8A2BE2");
				case "brown": return Color.FromHex ("A52A2A");
				case "burlywood": return Color.FromHex ("DEB887");
				case "cadetblue": return Color.FromHex ("5F9EA0");
				case "chartreuse": return Color.FromHex ("7FFF00");
				case "chocolate": return Color.FromHex ("D2691E");
				case "coral": return Color.FromHex ("FF7F50");
				case "cornflowerblue": return Color.FromHex ("6495ED");
				case "cornsilk": return Color.FromHex ("FFF8DC");
				case "crimson": return Color.FromHex ("DC143C");
				case "cyan": return Color.FromHex ("00FFFF");
				case "darkblue": return Color.FromHex ("00008B");
				case "darkcyan": return Color.FromHex ("008B8B");
				case "darkgoldenrod": return Color.FromHex ("B8860B");
				case "darkgray": return Color.FromHex ("A9A9A9");
				case "darkgrey": return Color.FromHex ("A9A9A9");
				case "darkgreen": return Color.FromHex ("006400");
				case "darkkhaki": return Color.FromHex ("BDB76B");
				case "darkmagenta": return Color.FromHex ("8B008B");
				case "darkolivegreen": return Color.FromHex ("556B2F");
				case "darkorange": return Color.FromHex ("FF8C00");
				case "darkorchid": return Color.FromHex ("9932CC");
				case "darkred": return Color.FromHex ("8B0000");
				case "darksalmon": return Color.FromHex ("E9967A");
				case "darkseagreen": return Color.FromHex ("8FBC8F");
				case "darkslateblue": return Color.FromHex ("483D8B");
				case "darkslategray": return Color.FromHex ("2F4F4F");
				case "darkslategrey": return Color.FromHex ("2F4F4F");
				case "darkturquoise": return Color.FromHex ("00CED1");
				case "darkviolet": return Color.FromHex ("9400D3");
				case "deeppink": return Color.FromHex ("FF1493");
				case "deepskyblue": return Color.FromHex ("00BFFF");
				case "dimgray": return Color.FromHex ("696969");
				case "dimgrey": return Color.FromHex ("696969");
				case "dodgerblue": return Color.FromHex ("1E90FF");
				case "firebrick": return Color.FromHex ("B22222");
				case "floralwhite": return Color.FromHex ("FFFAF0");
				case "forestgreen": return Color.FromHex ("228B22");
				case "fuchsia": return Color.FromHex ("FF00FF");
				case "gainsboro": return Color.FromHex ("DCDCDC");
				case "ghostwhite": return Color.FromHex ("F8F8FF");
				case "gold": return Color.FromHex ("FFD700");
				case "goldenrod": return Color.FromHex ("DAA520");
				case "gray": return Color.FromHex ("808080");
				case "grey": return Color.FromHex ("808080");
				case "green": return Color.FromHex ("008000");
				case "greenyellow": return Color.FromHex ("ADFF2F");
				case "honeydew": return Color.FromHex ("F0FFF0");
				case "hotpink": return Color.FromHex ("FF69B4");
				case "indianred ": return Color.FromHex ("CD5C5C");
				case "indigo ": return Color.FromHex ("4B0082");
				case "ivory": return Color.FromHex ("FFFFF0");
				case "khaki": return Color.FromHex ("F0E68C");
				case "lavender": return Color.FromHex ("E6E6FA");
				case "lavenderblush": return Color.FromHex ("FFF0F5");
				case "lawngreen": return Color.FromHex ("7CFC00");
				case "lemonchiffon": return Color.FromHex ("FFFACD");
				case "lightblue": return Color.FromHex ("ADD8E6");
				case "lightcoral": return Color.FromHex ("F08080");
				case "lightcyan": return Color.FromHex ("E0FFFF");
				case "lightgoldenrodyellow": return Color.FromHex ("FAFAD2");
				case "lightgray": return Color.FromHex ("D3D3D3");
				case "lightgrey": return Color.FromHex ("D3D3D3");
				case "lightgreen": return Color.FromHex ("90EE90");
				case "lightpink": return Color.FromHex ("FFB6C1");
				case "lightsalmon": return Color.FromHex ("FFA07A");
				case "lightseagreen": return Color.FromHex ("20B2AA");
				case "lightskyblue": return Color.FromHex ("87CEFA");
				case "lightslategray": return Color.FromHex ("778899");
				case "lightslategrey": return Color.FromHex ("778899");
				case "lightsteelblue": return Color.FromHex ("B0C4DE");
				case "lightyellow": return Color.FromHex ("FFFFE0");
				case "lime": return Color.FromHex ("00FF00");
				case "limegreen": return Color.FromHex ("32CD32");
				case "linen": return Color.FromHex ("FAF0E6");
				case "magenta": return Color.FromHex ("FF00FF");
				case "maroon": return Color.FromHex ("800000");
				case "mediumaquamarine": return Color.FromHex ("66CDAA");
				case "mediumblue": return Color.FromHex ("0000CD");
				case "mediumorchid": return Color.FromHex ("BA55D3");
				case "mediumpurple": return Color.FromHex ("9370DB");
				case "mediumseagreen": return Color.FromHex ("3CB371");
				case "mediumslateblue": return Color.FromHex ("7B68EE");
				case "mediumspringgreen": return Color.FromHex ("00FA9A");
				case "mediumturquoise": return Color.FromHex ("48D1CC");
				case "mediumvioletred": return Color.FromHex ("C71585");
				case "midnightblue": return Color.FromHex ("191970");
				case "mintcream": return Color.FromHex ("F5FFFA");
				case "mistyrose": return Color.FromHex ("FFE4E1");
				case "moccasin": return Color.FromHex ("FFE4B5");
				case "navajowhite": return Color.FromHex ("FFDEAD");
				case "navy": return Color.FromHex ("000080");
				case "oldlace": return Color.FromHex ("FDF5E6");
				case "olive": return Color.FromHex ("808000");
				case "olivedrab": return Color.FromHex ("6B8E23");
				case "orange": return Color.FromHex ("FFA500");
				case "orangered": return Color.FromHex ("FF4500");
				case "orchid": return Color.FromHex ("DA70D6");
				case "palegoldenrod": return Color.FromHex ("EEE8AA");
				case "palegreen": return Color.FromHex ("98FB98");
				case "paleturquoise": return Color.FromHex ("AFEEEE");
				case "palevioletred": return Color.FromHex ("DB7093");
				case "papayawhip": return Color.FromHex ("FFEFD5");
				case "peachpuff": return Color.FromHex ("FFDAB9");
				case "peru": return Color.FromHex ("CD853F");
				case "pink": return Color.FromHex ("FFC0CB");
				case "plum": return Color.FromHex ("DDA0DD");
				case "powderblue": return Color.FromHex ("B0E0E6");
				case "purple": return Color.FromHex ("800080");
				case "rebeccapurple": return Color.FromHex ("663399");
				case "red": return Color.FromHex ("FF0000");
				case "rosybrown": return Color.FromHex ("BC8F8F");
				case "royalblue": return Color.FromHex ("4169E1");
				case "saddlebrown": return Color.FromHex ("8B4513");
				case "salmon": return Color.FromHex ("FA8072");
				case "sandybrown": return Color.FromHex ("F4A460");
				case "seagreen": return Color.FromHex ("2E8B57");
				case "seashell": return Color.FromHex ("FFF5EE");
				case "sienna": return Color.FromHex ("A0522D");
				case "silver": return Color.FromHex ("C0C0C0");
				case "skyblue": return Color.FromHex ("87CEEB");
				case "slateblue": return Color.FromHex ("6A5ACD");
				case "slategray": return Color.FromHex ("708090");
				case "slategrey": return Color.FromHex ("708090");
				case "snow": return Color.FromHex ("FFFAFA");
				case "springgreen": return Color.FromHex ("00FF7F");
				case "steelblue": return Color.FromHex ("4682B4");
				case "tan": return Color.FromHex ("D2B48C");
				case "teal": return Color.FromHex ("008080");
				case "thistle": return Color.FromHex ("D8BFD8");
				case "tomato": return Color.FromHex ("FF6347");
				case "turquoise": return Color.FromHex ("40E0D0");
				case "violet": return Color.FromHex ("EE82EE");
				case "wheat": return Color.FromHex ("F5DEB3");
				case "white": return Color.FromHex ("FFFFFF");
				case "whitesmoke": return Color.FromHex ("F5F5F5");
				case "yellow": return Color.FromHex ("FFFF00");
				case "yellowgreen": return Color.FromHex ("9ACD32");
				}
			}
			return Color.Default;
		}

	}
}

