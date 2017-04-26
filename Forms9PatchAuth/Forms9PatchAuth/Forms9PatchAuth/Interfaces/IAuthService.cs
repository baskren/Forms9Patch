// /*******************************************************************
//  *
//  * IAuthPlatformSdk.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
namespace Forms9PatchAuth
{
	public interface IAuthService
	{
		void LoginAction(ButtonBase button);
		void LogoutAction(ButtonBase button);
	}
}
