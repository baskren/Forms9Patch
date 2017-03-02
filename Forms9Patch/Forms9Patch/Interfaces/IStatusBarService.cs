// /*******************************************************************
//  *
//  * IStatusBarService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
namespace Forms9Patch
{
	public interface IStatusBarService
	{
		double Height { get; }
		bool IsVisible { get; }
	}
}
