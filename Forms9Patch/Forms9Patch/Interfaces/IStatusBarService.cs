// /*******************************************************************
//  *
//  * IStatusBarService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
namespace Forms9Patch
{
	/// <summary>
	/// Status bar service.
	/// </summary>
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	internal interface IStatusBarService
	{
		/// <summary>
		/// Gets the height of the status bar.
		/// </summary>
		/// <value>The height.</value>
		double Height { get; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Forms9Patch.IStatusBarService"/> is visible.
		/// </summary>
		/// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
		bool IsVisible { get; }
	}
}
