// /*******************************************************************
//  *
//  * GoogleButton.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;

namespace Forms9PatchAuth
{
	public class GoogleButton : OAuth2Button
	{
		static IAuthService AuthService = DependencyService.Get<IGoogleAuthService>();

		public static readonly BindableProperty IsDarkThemeProperty = BindableProperty.Create("IsDarkTheme", typeof(bool), typeof(GoogleButton), default(bool));
		public bool IsDarkTheme
		{
			get { return (bool)GetValue(IsDarkThemeProperty); }
			set { SetValue(IsDarkThemeProperty, value); }
		}



		public GoogleButton()
		{
			Service = "Google";
			RequestedPermissions.Add("profile");
			AuthorizeUri = "https://accounts.google.com/o/oauth2/auth";
			//AccessTokenUri = "https://www.googleapis.com/oauth2/v4/token";
			RedirectUri = "com.google.codelabs.appauth:/oauth2callback";

			ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchAuth.Resources.Google.icon");
			UpdateTheme();
			Tapped += OnTapped;
		}

		void OnTapped(object sender, EventArgs e)
		{
			if (IsAuthenticated)
				AuthService.LogoutAction(this);
			else
				AuthService.LoginAction(this);
		}

		void UpdateTheme()
		{
			if (IsDarkTheme)
			{
				BackgroundColor = Color.FromRgb(0x3E, 0x82, 0xF7);
				FontColor = Color.White;
			}
			else
			{
				BackgroundColor = Color.White;
				FontColor = Color.FromRgb(0x75, 0x75, 0x75);
			}
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == IsDarkThemeProperty.PropertyName)
				UpdateTheme();
		}
	}
}
