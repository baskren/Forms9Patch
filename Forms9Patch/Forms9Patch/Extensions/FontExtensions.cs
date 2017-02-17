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

		static IFontService _service;

		public static double LineHeight(this IFontElement element)
		{
			return LineHeight(element.FontFamily, element.FontSize, element.FontAttributes);
		}

		public static double LineHeight(this Font font)
		{
			return LineHeight(font.FontFamily, font.FontSize, font.FontAttributes);
		}

		public static double LineHeight(string fontFamily, double fontSize, FontAttributes fontAttributes)
		{
			_service = _service ?? DependencyService.Get<IFontService>();
			if (_service == null)
				throw new NotSupportedException("IFontService is not supported on this platform.");
			return _service.LineHeight(fontFamily, fontSize, fontAttributes);
		}

		public static double LineSpace(this IFontElement element)
		{
			return LineSpace(element.FontFamily, element.FontSize, element.FontAttributes);
		}

		public static double LineSpace(this Font font)
		{
			return LineSpace(font.FontFamily, font.FontSize, font.FontAttributes);
		}

		public static double LineSpace(string fontFamily, double fontSize, FontAttributes fontAttributes)
		{
			_service = _service ?? DependencyService.Get<IFontService>();
			if (_service == null)
				throw new NotSupportedException("IFontService is not supported on this platform.");
			return _service.LineSpace(fontFamily, fontSize, fontAttributes);
		}
	}
}

