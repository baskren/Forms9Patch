// /*******************************************************************
//  *
//  * XaAccountExt.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Facebook;

namespace  Forms9PatchAuth.Android
{
	public static class AccountExt
	{
		public static Account ToAccount(this AccessToken accessToken)
		{
			var account = new Account();
			account.AppId = accessToken.ApplicationId;
			var declinedPermissions = accessToken.DeclinedPermissions.ToList();
			account.DeclinedPermissions = declinedPermissions;
			account.Type = accessToken.GetType();
			account.ExpirationDate = accessToken.Expires.ToDateTime(); 
			var permissions = accessToken.Permissions.ToList();
			var grantedPermissions = new List<string>();
			foreach (var permission in permissions)
				if (declinedPermissions == null || !declinedPermissions.Contains(permission))
					grantedPermissions.Add(permission);
			account.GrantedPermissions = grantedPermissions;
			account.RefreshDate = accessToken.LastRefresh.ToDateTime();
			account.AccessToken = accessToken.Token;
			account.UserId = accessToken.UserId;
			account.TokenSource = accessToken.Source.ToString();

			return account;
		}

		public static AccessToken ToFacebookAccessToken(this Account account)
		{
			if (account == null)
				return null;
			var tokenSource = AccessTokenSource.ValueOf(account.TokenSource);
			return new AccessToken(account.AccessToken, account.AppId, account.UserId, account.GrantedPermissions, account.DeclinedPermissions, tokenSource, account.ExpirationDate.Value.ToJavaDate(), account.RefreshDate.Value.ToJavaDate());
		}

		static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static Java.Util.Date ToJavaDate(this DateTime dateTime)
		{
			var timeSinceEpoch = dateTime - Epoch;
			var milliseconds = timeSinceEpoch.TotalMilliseconds;
			var result = new Java.Util.Date { Time = (long)milliseconds };
			return result;
		}

		public static DateTime ToDateTime(this Java.Util.Date date)
		{
			var milliseconds = date.Time;
			return FromUnixTime(milliseconds);
		}

		public static DateTime FromUnixTime(long unixTimeMillis)
		{
			return Epoch.AddMilliseconds(unixTimeMillis);
		}
	}
}
