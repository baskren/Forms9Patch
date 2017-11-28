// /*******************************************************************
//  *
//  * IFontService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Font service.
	/// </summary>
	interface IFontService 
	{
		/// <summary>
		/// Lines the height.
		/// </summary>
		/// <returns>The height.</returns>
		/// <param name="fontFamily">Font family.</param>
		/// <param name="fontSize">Font size.</param>
		/// <param name="fontAttributes">Font attributes.</param>
		double LineHeight(string fontFamily, double fontSize, FontAttributes fontAttributes);

		/// <summary>
		/// Lines the space.
		/// </summary>
		/// <returns>The space.</returns>
		/// <param name="fontFamily">Font family.</param>
		/// <param name="fontSize">Font size.</param>
		/// <param name="fontAttributes">Font attributes.</param>
		double LineSpace(string fontFamily, double fontSize, FontAttributes fontAttributes);

        /// <summary>
        /// Loads a font stored as an EmbeddedResource
        /// </summary>
        /// <param name="embeddedResourceId"></param>
        /// <param name="assembly"></param>
        /// <returns>string for the font's FamilyName or null if font is not loaded</returns>
        Task<FontSource> GetEmbeddedResourceFontSource(string embeddedResourceId, Assembly assembly);

        /// <summary>
        /// Loads a font stored in the file system
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<FontSource> GetFileFontSource(IFile file);

        /// <summary>
        /// Loads a font from the internets
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<FontSource> GetUriFontSource(string url);


    }
}

