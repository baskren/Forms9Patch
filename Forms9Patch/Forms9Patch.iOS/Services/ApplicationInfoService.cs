// /*******************************************************************
//  *
//  * ApplicationInfoService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.iOS.ApplicationInfoService))]
namespace Forms9Patch.iOS
{
	public class ApplicationInfoService : IApplicationInfoService
	{
		public int Build
		{
			get
			{
				var resultAsString = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
				int result = -1;
				int.TryParse(resultAsString, out result);
				return result;
			}
		}

		public string Identifier
		{
			get
			{
				return NSBundle.MainBundle.InfoDictionary["CFBundleIdentifier"].ToString();
			}
		}

		public string Name
		{
			get
			{
				return NSBundle.MainBundle.InfoDictionary["CFBundleDisplayName"]?.ToString() ?? NSBundle.MainBundle.InfoDictionary["CFBundleName"]?.ToString();
			}
		}

		public string Version
		{
			get
			{
				return NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
			}
		}
	}
}
