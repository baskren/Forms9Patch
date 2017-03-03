// /*******************************************************************
//  *
//  * ApplicationInfoService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Android.App;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.Droid.ApplicationInfoService))]
namespace Forms9Patch.Droid
{
	public class ApplicationInfoService : IApplicationInfoService
	{
		public int Build
		{
			get
			{
				return Forms.Context.PackageManager.GetPackageInfo(Identifier, 0).VersionCode;
			}
		}

		public string Identifier
		{
			get
			{
				return Forms.Context.PackageName;
			}
		}

		public string Name
		{
			get
			{
				return ((Activity)Forms.Context).Title;
			}
		}

		public string Version
		{
			get
			{
				return Forms.Context.PackageManager.GetPackageInfo(Identifier, 0).VersionName;
			}
		}
	}
}
