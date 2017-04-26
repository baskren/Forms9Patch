// /*******************************************************************
//  *
//  * FacebookButton.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
namespace Forms9PatchAuth
{
	public class FacebookButton : ButtonBase
	{
		static IAuthService AuthService = DependencyService.Get<IFacebookAuthService>();

		public static readonly BindableProperty IsDarkThemeProperty = BindableProperty.Create("IsDarkTheme", typeof(bool), typeof(FacebookButton), default(bool));
		public bool IsDarkTheme
		{
			get { return (bool)GetValue(IsDarkThemeProperty); }
			set { SetValue(IsDarkThemeProperty, value); }	
		}

		public static readonly BindableProperty AppNameProperty = BindableProperty.Create("AppName", typeof(string), typeof(FacebookButton), default(string));
		public string AppName
		{
			get { return (string)GetValue(AppNameProperty); }
			set { SetValue(AppNameProperty, value); }
		}


		public FacebookButton()
		{
			Service = "Facebook";
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
				BackgroundColor = Color.FromRgb(59, 90, 154);
				FontColor = Color.White;
				ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchAuth.Resources.Facebook.Dark.icon");
			}
			else
			{
				BackgroundColor = Color.White;
				FontColor = Color.FromRgb(234, 235, 237);
				ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchAuth.Resources.Facebook.Light.icon");
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
