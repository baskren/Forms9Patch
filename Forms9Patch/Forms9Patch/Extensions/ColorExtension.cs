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
		/// Rgbs the blend.
		/// </summary>
		/// <returns>The blend.</returns>
		/// <param name="c">C.</param>
		/// <param name="c2">C2.</param>
		/// <param name="percent">Percent.</param>
		public static Color RgbBlend(this Color c, Color c2, double percent) {
			var c1 = new Color (c.R, c.G, c.B, c.A);
			if (c1 == Color.Transparent)
				c1 = new Color (1.0, 1.0, 1.0, 0.0);
			double A = c1.A;
			double R = c1.R + (c2.R - c1.R) * percent;
			double G = c1.G + (c2.G - c1.G) * percent;
			double B = c1.B + (c2.B - c1.B) * percent;
			return new Color(R,G,B,A);
		}

		/// <summary>
		/// Withs the alpha.
		/// </summary>
		/// <returns>The alpha.</returns>
		/// <param name="c">C.</param>
		/// <param name="alpha">Alpha.</param>
		public static Color WithAlpha(this Color c, double alpha) {
			return new Color (c.R, c.G, c.B, alpha);
		}
	}
}

