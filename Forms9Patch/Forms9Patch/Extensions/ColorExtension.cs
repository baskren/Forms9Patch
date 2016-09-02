using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	internal static class ColorExtension
	{
		internal static Color RgbBlend(this Color c, Color c2, double percent) {
			var c1 = new Color (c.R, c.G, c.B, c.A);
			if (c1 == Color.Transparent)
				c1 = new Color (1.0, 1.0, 1.0, 0.0);
			double A = c1.A;
			double R = c1.R + (c2.R - c1.R) * percent;
			double G = c1.G + (c2.G - c1.G) * percent;
			double B = c1.B + (c2.B - c1.B) * percent;
			return new Color(R,G,B,A);
		}

		internal static Color WithAlpha(this Color c, double alpha) {
			return new Color (c.R, c.G, c.B, alpha);
		}
	}
}

