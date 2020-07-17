// /*******************************************************************
//  *
//  * StatusBarService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.StatusBarService))]
namespace Forms9Patch.iOS
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	internal class StatusBarService : IStatusBarService
	{
		public double Height
		{
			get
			{
				var result = UIApplication.SharedApplication.StatusBarFrame;
				//var orientation = UIApplication.SharedApplication.StatusBarOrientation;
				return result.Height;
			}
		}

		public bool IsVisible
		{
			get
			{
				return !UIApplication.SharedApplication.StatusBarHidden;			
			}
		}
	}
}
