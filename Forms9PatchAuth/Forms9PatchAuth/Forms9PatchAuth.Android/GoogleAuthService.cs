// /*******************************************************************
//  *
//  * GoogleAuthService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
using System.IO;
using Xamarin.Forms.Platform.Android;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api.SignIn;

namespace Forms9PatchAuth.Android
{
	public class GoogleAuthService : IGoogleAuthService
	{

		static bool PlayServicesAvailable
		{
			get
			{
				var googleApiAvailability = global::Android.Gms.Common.GoogleApiAvailability.Instance;
				var resultCode = googleApiAvailability.IsGooglePlayServicesAvailable(Forms.Context);
				return resultCode == global::Android.Gms.Common.ConnectionResult.Success;
			}
		}

		GoogleButton GoogleButton;

		GoogleSignInOptions SignInOptions
		{
			get
			{
				var clientId = GoogleButton.ClientId;
				if (clientId == null)
					throw new InvalidDataException("GoogleAuthService requires ClientId field value.  Check google-services.json for necessary value.");
				var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
								.RequestEmail()
								.RequestProfile()
								.RequestId()
								.RequestIdToken(clientId)
								.Build();
				return gso;
			}
		}

		GoogleApiClient _googleApiClient;
		GoogleApiClient GoogleApiClient
		{
			get
			{
				_googleApiClient = _googleApiClient ?? new GoogleApiClient.Builder(Forms.Context)
								                                          .EnableAutoManage(Forms.Context as global::Android.Support.V4.App.FragmentActivity, new GoogleAuthCallback())  // is this necessary?  Is callback called?
				                                                          .AddApi(global::Android.Gms.Auth.Api.Auth.GOOGLE_SIGN_IN_API, SignInOptions)
																		  .Build();
				return _googleApiClient;
			}
			set
			{
				_googleApiClient = value;
			}
		}

		/*
		bool SilentLoginAction(ButtonBase button)
		{
			GoogleApiClient = null;
			var gButton = button as GoogleButton;
			if (gButton == null)
				throw new InvalidCastException("IAuthService (GoogleAuthService) is not appropriate for " + button.GetType() + " button element.");
			GoogleButton = gButton;

			var opr = global::Android.Gms.Auth.Api.Auth.GoogleSignInApi.SilentSignIn(GoogleApiClient);
			opr.SetResultCallback((GoogleSignInResult result) =>
			{
				System.Diagnostics.Debug.WriteLine("");
			});

			if (opr.IsDone)
			{
				var result = opr.Get();
				return true;
			}
		}
		*/

		void IAuthService.LoginAction(ButtonBase button)
		{
			var gButton = button as GoogleButton;
			if (gButton == null)
				throw new InvalidCastException("IAuthService (GoogleAuthService) is not appropriate for " + button.GetType() + " button element.");
			GoogleButton = gButton;
			
			var callback = new GoogleAuthCallback();
			GoogleAuthCallback.Element = gButton;

			var activity = Forms.Context as global::Android.App.Activity;
			if (PlayServicesAvailable && gButton.UiType >= UiType.SDK)
			{
				var intent = global::Android.Gms.Auth.Api.Auth.GoogleSignInApi.GetSignInIntent(GoogleApiClient);
				activity.StartActivityForResult(intent, (int)IdentityProviderRequestCodes.GoogleSignOnAPI);
			}
			else
			{
				var auth = new Xamarin.Auth.OAuth2Authenticator(gButton.ClientId, string.Join(",", gButton.RequestedPermissions), new Uri(gButton.AuthorizeUri), new Uri(gButton.RedirectUri));
				auth.IsUsingNativeUI = gButton.UiType != UiType.WebView;
				auth.AllowCancel = true;
				var authVC = auth.GetUI(activity);
				auth.Completed += (sender, e) =>
				{
					if (e.Account != null)
					{
						var xaAccount = new Account(e.Account)
						{
							Service = gButton.Service
						};
					gButton.OnCompleted(xaAccount);
					}
					else
						gButton.OnCancelled();
				};

				auth.Error += (sender, e) =>
				{
					gButton.OnError(e.Message);
				};

				if (auth.IsUsingNativeUI == true)
				{
					//var uri_netfx = await auth.GetInitialUrlAsync();
					var uri_netfx = auth.GetInitialUrlAsync();
					var uri_droid = global::Android.Net.Uri.Parse(uri_netfx.Result.AbsoluteUri);
					System.Diagnostics.Debug.WriteLine("URI_netfx:[" + uri_droid + "]");
					var ctib = (global::Android.Support.CustomTabs.CustomTabsIntent.Builder)authVC;
					ctib.SetShowTitle(true);
					ctib.EnableUrlBarHiding();
					ctib.SetToolbarColor(Color.Blue.ToAndroid());
					ctib.SetSecondaryToolbarColor(Color.Green.ToAndroid());
					var ct_intent = ctib.Build();
					ct_intent.LaunchUrl(Forms.Context, uri_droid);
				}
				else
				{
					global::Android.Content.Intent i = null;
					i = (global::Android.Content.Intent)authVC;
					activity.StartActivity(i);
				}
			}

		}

		public void LogoutAction(ButtonBase button)
		{
			throw new NotImplementedException();
		}
	}
}
