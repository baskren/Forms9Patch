// /*******************************************************************
//  *
//  * FacebookAuthCallback.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace Forms9PatchAuth.Android
{
	public class FacebookAuthCallback : Java.Lang.Object, IFacebookCallback 
	{
		public FacebookButton Element;

		public void OnSuccess(Java.Lang.Object obj)
		{
			var loginResult = obj as LoginResult;
			var accessToken = loginResult.AccessToken;
			var account = accessToken.ToAccount();
			account.Service = Element.Service;
			Element.OnCompleted(account);
		}

		public void OnCancel()
		{
			Element.OnCancelled();
		}

		public void OnError(FacebookException fbException)
		{
			var exception = fbException as FacebookAuthorizationException;
			Element.OnError(exception?.LocalizedMessage ?? exception?.Message);
		}
	}
}
