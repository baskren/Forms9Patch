// /*******************************************************************
//  *
//  * IFontService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;

namespace Forms9Patch
{
	public interface IFontService 
	{
		double LineHeight(string fontFamily, double fontSize, FontAttributes fontAttributes);

		double LineSpace(string fontFamily, double fontSize, FontAttributes fontAttributes);
	}
}

