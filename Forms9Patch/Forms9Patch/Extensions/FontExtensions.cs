using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Font extensions.
	/// </summary>
	public static class FontExtensions
	{
		/// <summary>
		/// Is Int32 char in the Unicode math alphanumeric block.
		/// </summary>
		/// <returns><c>true</c>, if math alphanumeric block was ined, <c>false</c> otherwise.</returns>
		/// <param name="utf32Char">Utf32 char.</param>
		public static bool InMathAlphanumericBlock(this Int32 utf32Char) {
			var result = (utf32Char >= 0x1D400 && utf32Char <= 0x1D7FF);
			return result;
		}

		/// <summary>
		/// Provides a list of the currently loaded font families (included embedded resources) that are available.
		/// </summary>
		/// <returns>The font families.</returns>
		public static List<string> LoadedFontFamilies()
		{
			return DependencyService.Get<IFontFamilies>().FontFamilies();
		}
	}
}

