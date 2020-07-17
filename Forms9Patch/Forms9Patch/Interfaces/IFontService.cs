// /*******************************************************************
//  *
//  * IFontService.cs copyright 2017 Ben Askren, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
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
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
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


    }
}

