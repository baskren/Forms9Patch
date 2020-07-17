// /*******************************************************************
//  *
//  * FontService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.FontService))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Font service.
	/// </summary>
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	public class FontService : IFontService
	{
		/// <summary>
		/// Lines the height.
		/// </summary>
		/// <returns>The height.</returns>
		/// <param name="fontFamily">Font family.</param>
		/// <param name="fontSize">Font size.</param>
		/// <param name="fontAttributes">Font attributes.</param>
		public double LineHeight(string fontFamily, double fontSize, FontAttributes fontAttributes)
		{
			var font = FontExtensions.BestFont(fontFamily, (nfloat)fontSize, (fontAttributes & FontAttributes.Bold) != 0, (fontAttributes & FontAttributes.Italic) != 0);
			return font.LineHeight;
		}

		/// <summary>
		/// Lines the space.
		/// </summary>
		/// <returns>The space.</returns>
		/// <param name="fontFamily">Font family.</param>
		/// <param name="fontSize">Font size.</param>
		/// <param name="fontAttributes">Font attributes.</param>
		public double LineSpace(string fontFamily, double fontSize, FontAttributes fontAttributes)
		{
			var font = FontExtensions.BestFont(fontFamily, (nfloat)fontSize, (fontAttributes & FontAttributes.Bold) != 0, (fontAttributes & FontAttributes.Italic) != 0);
			return font.Leading;
		}
	}
}

