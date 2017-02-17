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
	public class FontService : IFontService
	{
		public double LineHeight(string fontFamily, double fontSize, FontAttributes fontAttributes)
		{
			var font = FontExtensions.BestFont(fontFamily, (nfloat)fontSize, (fontAttributes & FontAttributes.Bold) != 0, (fontAttributes & FontAttributes.Italic) != 0);
			return font.LineHeight;
		}

		public double LineSpace(string fontFamily, double fontSize, FontAttributes fontAttributes)
		{
			var font = FontExtensions.BestFont(fontFamily, (nfloat)fontSize, (fontAttributes & FontAttributes.Bold) != 0, (fontAttributes & FontAttributes.Italic) != 0);
			return font.Leading;
		}
	}
}

