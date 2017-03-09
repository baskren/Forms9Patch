// /*******************************************************************
//  *
//  * StatusBarService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	internal static class StatusBarService
	{
		static IStatusBarService _service;

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public static double Height
		{
			get
			{
				_service = _service ?? DependencyService.Get<IStatusBarService>();
				if (_service == null)
				{
					if (Device.OS == TargetPlatform.iOS)
						return 20;
					return 0;
				}
				//if (!IsVisible)
				//	return 0;
				return _service.Height;
			}
		}

		public static bool IsVisible
		{
			get
			{
				_service = _service ?? DependencyService.Get<IStatusBarService>();
				if (_service == null)
					return true;
				return _service.IsVisible;
			}
		}
	}
}
