// /*******************************************************************
//  *
//  * IFontFamilies.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Collections.Generic;
namespace Forms9Patch
{
	/// <summary>
	/// Font families interface used for backing service.
	/// </summary>
	public interface IFontFamilies
	{
		/// <summary>
		/// primary method in FontFamilies interface.
		/// </summary>
		/// <returns>The families.</returns>
		List<string> FontFamilies();
	}
}

