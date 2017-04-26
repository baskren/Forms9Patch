// /*******************************************************************
//  *
//  * OAuth2Button.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;

namespace Forms9PatchAuth
{
	public class OAuth2Button : ButtonBase
	{

		public static readonly BindableProperty AuthorizeUriProperty = BindableProperty.Create("AuthorizeUri", typeof(string), typeof(OAuth2Button), default(string));
		public string AuthorizeUri
		{
			get { return (string)GetValue(AuthorizeUriProperty); }
			set { SetValue(AuthorizeUriProperty, value); }
		}

		public static readonly BindableProperty RedirectUriProperty = BindableProperty.Create("RedirectUri", typeof(string), typeof(OAuth2Button), default(string));
		public string RedirectUri
		{
			get { return (string)GetValue(RedirectUriProperty); }
			set { SetValue(RedirectUriProperty, value); }	
		}

		public static readonly BindableProperty AllowCancelProperty = BindableProperty.Create("AllowCancel", typeof(bool), typeof(OAuth2Button), true);
		public bool AllowCancel
		{
			get { return (bool)GetValue(AllowCancelProperty); }
			set { SetValue(AllowCancelProperty, value); }	
		}



		public OAuth2Button()
		{
		}
	}
}
