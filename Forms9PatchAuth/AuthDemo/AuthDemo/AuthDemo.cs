// /*******************************************************************
//  *
//  * AuthDemo.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;
using Forms9Patch;
using Forms9PatchAuth;

namespace AuthDemo
{
	public class App : Application
	{
		public App()
		{
			var content = new ContentPage
			{
				Title = "XamAuthExp",
				Content = new Xamarin.Forms.StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Children = {
								new Xamarin.Forms.Label {
									HorizontalTextAlignment = TextAlignment.Center,
									Text = "Welcome to Xamarin Forms!"
								}
							}
				}
			};

			var navPage = new NavigationPage(content);
			MainPage = RootPage.Create(navPage);


			var facebookLoginButton = new FacebookButton
			{
				ClientId = "1482485135117630",
				AppName = "XamAuthExp"
			};


			var googleLoginButton = new GoogleButton
			{
				//UiType = UiType.NativeBrowser
				ClientId = "424089952055-j7pjt3sjav1l69b3hvhuh5euci3d6j63.apps.googleusercontent.com" 
			};

			facebookLoginButton.Cancelled += OnCancelled;
			facebookLoginButton.Completed += OnCompleted;
			facebookLoginButton.Error += OnError;
			facebookLoginButton.LoggedOut += OnLoggedOut;


			googleLoginButton.Cancelled += OnCancelled;
			googleLoginButton.Completed += OnCompleted;
			googleLoginButton.Error += OnError;
			googleLoginButton.LoggedOut += OnLoggedOut;


			var selector = new MaterialSegmentedControl
			{
				Segments =
						{
							new Segment { Text = "Best" },
							new Segment { Text = "SDK" },
							new Segment { Text = "Browser" },
							new Segment { Text = "WebView" }
						},
				SelectedIndexes = { 1 }
			};

			selector.SegmentSelected += (sender, e) =>
			{
				switch (e.Segment.Text)
				{
					case "Best":
						facebookLoginButton.UiType = UiType.BestAvailable;
						googleLoginButton.UiType = UiType.BestAvailable;
						break;
					case "SDK":
						facebookLoginButton.UiType = UiType.SDK;
						googleLoginButton.UiType = UiType.SDK;
						googleLoginButton.ClientId = "689959927708-1tbu4is1slkthnqflcvo8jd609skp64s.apps.googleusercontent.com";
						break;
					case "Browser":
						facebookLoginButton.UiType = UiType.NativeBrowser;
						googleLoginButton.UiType = UiType.NativeBrowser;


						// web client
						//googleLoginButton.ClientId = "689959927708-1tbu4is1slkthnqflcvo8jd609skp64s.apps.googleusercontent.com"; 
						// default redirect:  400: Error: invalid request.  Custom scheme URIs are not allowed for 'WEB' client type;
						// firebase redirect: 400: Error: invalid scope. Some requested scopes are invalid { invalid = [public_profile, email, profile]}

						// unknown client.  Found in google-services.json 
						//googleLoginButton.ClientId = "689959927708-ndc4kjnkoif3i17k3cdhdk6mjsofbn5c.apps.googleusercontent.com"; 
						// both redirects: 401: Error: deleted client.   The OAuth client was deleted.

						// iOS client
						//googleLoginButton.ClientId = "689959927708-adfiqmhp2jbc4cekmt9qe4ja15slgb3g.apps.googleusercontent.com"; 
						// both redirects: 400: Error: redirect_uri_mismatch.

						// android client
						googleLoginButton.ClientId = "689959927708-1q9nl9t6l6100h1qpjcsqtg0gptkr25i.apps.googleusercontent.com";
						// both redirects: 400: Error: redirect_uri_mismatch.


						googleLoginButton.RedirectUri = "https://xamauthexp-3c638.firebaseapp.com/__/auth/handler";
						break;
					default:
						facebookLoginButton.UiType = UiType.WebView;
						googleLoginButton.UiType = UiType.WebView;
						break;
				}
			};

			var loginPopup = new ModalPopup
			{
				OutlineRadius = 4,
				Content = new Xamarin.Forms.StackLayout
				{
					WidthRequest = 200,
					HeightRequest = 200,
					Children = {
								selector,
								facebookLoginButton,
								googleLoginButton
							}
				},
				CancelOnPageOverlayTouch = false
			};

			loginPopup.IsVisible = true;

		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		void OnCancelled(object sender, EventArgs args)
		{
			Toast.Create("Cancelled", null);
		}

		void OnCompleted(object sender, Account account)
		{
			Device.BeginInvokeOnMainThread(() => Toast.Create("Success", account.ToString()));
		}

		void OnError(object sender, string message)
		{
			Device.BeginInvokeOnMainThread(() => Toast.Create("Error", message));
		}

		void OnLoggedOut(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() => Toast.Create("Logged Out", null));
		}	
	}
}
