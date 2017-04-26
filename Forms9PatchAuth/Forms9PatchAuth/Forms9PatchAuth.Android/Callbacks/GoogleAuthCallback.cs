// /*******************************************************************
//  *
//  * GoogleAuthCallback.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Collections.Generic;
using Android.Gms.Common;
using Android.OS;

namespace Forms9PatchAuth.Android
{
	public class GoogleAuthCallback : Java.Lang.Object, global::Android.Gms.Common.Apis.GoogleApiClient.IConnectionCallbacks, global::Android.Gms.Common.Apis.GoogleApiClient.IOnConnectionFailedListener 
	{
		public static GoogleButton Element;

		public static void OnActivityResult(int requestCode, global::Android.App.Result resultCode, global::Android.Content.Intent data)
		{
			if (requestCode == (int)IdentityProviderRequestCodes.GoogleSignOnAPI)
			{
				

				var result = global::Android.Gms.Auth.Api.Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
				//if (result.Status.StatusCode == Android.Gms.Common.Apis.CommonStatusCodes.Success)
				if (result.IsSuccess)
				{
					/*
					var status = result.Status;
					var acct = result.SignInAccount;
					var androidAccount = acct.Account;
					var accountName = androidAccount.Name;
					var accountType = androidAccount.Type;
					var name = acct.DisplayName;
					var email = acct.Email;
					var familyName = acct.FamilyName;
					var givenName = acct.GivenName;
					var grantedScopes = acct.GrantedScopes;
					var id = acct.Id;
					var idToken = acct.IdToken;
					var photo = acct.PhotoUrl;
					var serverAuthCode = acct.ServerAuthCode;
					*/


					var account = new Account();
					// Type
					account.Type = result.SignInAccount.GetType();
					// Service
					account.Service = "Google";
					// AppId
					//account.AppId = accessToken.ApplicationId;

					// AccessToken
					account.AccessToken = result.SignInAccount.IdToken;  // or is this serverAuthCode???

					// Declined Permissions
					var grantedPermissions = new List<string>();
					foreach (var scope in result.SignInAccount.GrantedScopes)
						grantedPermissions.Add(scope.ToString());
					var declinedPermissions = new List<string>();
					foreach (var permission in Element.RequestedPermissions)
						if (!grantedPermissions.Contains(permission))
							declinedPermissions.Add(permission);
					account.DeclinedPermissions = declinedPermissions;

					// ExpirationDate
					account.ExpirationDate = DateTime.MinValue;

					// GrantedPermissions
					account.GrantedPermissions = grantedPermissions;

					// RefreshDate
					account.RefreshDate = DateTime.MinValue;

					// UserId
					account.UserId = result.SignInAccount.Id;

					// TokenSource
					account.TokenSource = null;

					// UserName
					account.UserName = result.SignInAccount.DisplayName;

					// UserEmail
					account.UserEmail = result.SignInAccount.Email;

					Element?.OnCompleted(account);
					// TODO: how to use silent sign-in (https://developers.google.com/android/reference/com/google/android/gms/auth/api/signin/GoogleSignInApi.html#silentSignIn(com.google.android.gms.common.api.GoogleApiClient))
				}
				else if (result.Status.StatusCode == global::Android.Gms.Common.Apis.CommonStatusCodes.Canceled)
				{
					Element?.OnCancelled();
				}
				else
				{
					Element?.OnError(result.Status.StatusMessage);
				}
			}
		}

		public void OnConnected(Bundle connectionHint)
		{
			throw new NotImplementedException();
		}

		public void OnConnectionFailed(ConnectionResult result)
		{
			throw new NotImplementedException();
		}

		public void OnConnectionSuspended(int cause)
		{
			throw new NotImplementedException();
		}

	}
}
