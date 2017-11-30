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
        /*
        internal static async Task<FontSource> GetEmbeddedResourceFontSourceAsync(string embeddedResourceId, Assembly assembly=null)
        {
            if (assembly == null)
                assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);
            return await Service.GetEmbeddedResourceFontSource(embeddedResourceId, assembly);
        }

        internal static async Task<FontSource> GetFileFontSourceAsync(PCLStorage.IFile file)
        {
            return await Service.GetFileFontSource(file);
        }

        internal static async Task<FontSource> GetUrlFontSource(string url)
        {
            return await Service.GetUriFontSource(url);
        }
        */
        /*
        public static async Task SetFontSource(this Xamarin.Forms.VisualElement element, string embeddedResourceId, Assembly assembly = null)
        {
            if (element.HasProperty("FontFamily"))
            {
                if (assembly == null)
                    assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);

                // load font and set element's FontFamily
                var fontFamily = await RegisterFontSource(embeddedResourceId, assembly);
                element.SetPropertyValue("FontFamily", fontFamily);

                // attach effect
                bool containsEmbeddedResourceFontEffect = false;
                foreach (var effect in element.Effects)
                    if (effect is EmbeddedResourceFontEffect)
                    {
                        containsEmbeddedResourceFontEffect = true;
                        break;
                    }
                if (!containsEmbeddedResourceFontEffect)
                    element.Effects.Add(new EmbeddedResourceFontEffect());
            }
        }

        // key = fontFamily, value = Tuple<embeddedResource, assembly>
        internal static Dictionary<string, FontSource> FontSourceRegistry = new Dictionary<string, FontSource>();


        /// <summary>
        /// Makes a .ttf or .otf font file, stored as an EmbeddedResource, available to Text elements (EmbeddedResourceFontEffect needs to be added to Xamarin.Forms elements)
        /// </summary>
        /// <param name="embeddedResourceId"></param>
        /// <param name="assembly"></param>
        /// <returns>the FamilyName of the font or null if font fails to load</returns>
        public static async Task<string> RegisterFontSource(string embeddedResourceId, Assembly assembly=null)
        {
            if (assembly==null)
                assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);
            _service = _service ?? DependencyService.Get<IFontService>();
            if (_service == null)
                throw new NotSupportedException("IFontService is not supported on this platform.");

            foreach (var fontSource in FontSourceRegistry.Values)
            {
                if (fontSource is EmbeddedResourceFontSource embeddedResourceFontSource)
                {
                    if (embeddedResourceFontSource.EmbeddedResourceId == embeddedResourceId)
                        return embeddedResourceFontSource.FamilyName;
                }
            }
            var source = await _service.GetEmbeddedResourceFontSource(embeddedResourceId, assembly);
            if (source != null)
                FontSourceRegistry.Add(source.FamilyName, source );

            return source.FamilyName;
        }
        */
        


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
        static IFontService Service
        {
            get
            {
                _service = _service ?? DependencyService.Get<IFontService>();
                if (_service == null)
                    throw new NotSupportedException("IFontService is not supported on this platform.");
                return _service;
            }
        }

		/// <summary>
		/// Lines the height.
		/// </summary>
		/// <returns>The height.</returns>
		/// <param name="element">Element.</param>
		public static double LineHeight(this IFontElement element)
		{
			return LineHeight(element.FontFamily, element.FontSize, element.FontAttributes);
		}

		/// <summary>
		/// Lines the height.
		/// </summary>
		/// <returns>The height.</returns>
		/// <param name="font">Font.</param>
		public static double LineHeight(this Font font)
		{
			return LineHeight(font.FontFamily, font.FontSize, font.FontAttributes);
		}

		/// <summary>
		/// Lines the height.
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

