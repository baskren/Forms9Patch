using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using PCL.Utils;

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
            Settings.ConfirmInitialization();
            return DependencyService.Get<IFontFamilies>().FontFamilies();
		}

		static IFontService _service;
        static IFontService Service
        {
            get
            {
                Settings.ConfirmInitialization();
                _service = _service ?? DependencyService.Get<IFontService>();
                if (_service == null)
                    throw new NotSupportedException("IFontService is not supported on this platform.");
                return _service;
            }
        }

		/// <summary>
		/// Height of a line of text for the given font.
		/// </summary>
		/// <returns>The height.</returns>
		/// <param name="element">Element.</param>
		public static double LineHeight(this IFontElement element)
		{
			return LineHeight(element.FontFamily, element.FontSize, element.FontAttributes);
		}

        /// <summary>
        /// Height of a line of text for the given font.
        /// </summary>
        /// <returns>The height.</returns>
        /// <param name="font">Font.</param>
        public static double LineHeight(this Font font)
		{
			return LineHeight(font.FontFamily, font.FontSize, font.FontAttributes);
		}

        /// <summary>
        /// Height of a line of text for the given font.
        /// </summary>
        /// <returns>The height.</returns>
        /// <param name="fontFamily">Font family.</param>
        /// <param name="fontSize">Font size.</param>
        /// <param name="fontAttributes">Font attributes.</param>
        public static double LineHeight(string fontFamily, double fontSize, FontAttributes fontAttributes)
		{
			return Service.LineHeight(fontFamily, fontSize, fontAttributes);
		}

		/// <summary>
		/// Lines the space.
		/// </summary>
		/// <returns>The space.</returns>
		/// <param name="element">Element.</param>
		public static double LineSpace(this IFontElement element)
		{
			return LineSpace(element.FontFamily, element.FontSize, element.FontAttributes);
		}

		/// <summary>
		/// Lines the space.
		/// </summary>
		/// <returns>The space.</returns>
		/// <param name="font">Font.</param>
		public static double LineSpace(this Font font)
		{
			return LineSpace(font.FontFamily, font.FontSize, font.FontAttributes);
		}

		/// <summary>
		/// Lines the space.
		/// </summary>
		/// <returns>The space.</returns>
		/// <param name="fontFamily">Font family.</param>
		/// <param name="fontSize">Font size.</param>
		/// <param name="fontAttributes">Font attributes.</param>
		public static double LineSpace(string fontFamily, double fontSize, FontAttributes fontAttributes)
		{
			return Service.LineSpace(fontFamily, fontSize, fontAttributes);
		}
	}
}

