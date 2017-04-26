// /*******************************************************************
//  *
//  * ButtonBase.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Xamarin.Auth;
using Xamarin.Forms;

namespace Forms9PatchAuth
{
	public class ButtonBase : Forms9Patch.MaterialButton
	{

		#region Static Properties
		static AccountStore _accountStore;
		static AccountStore AccountStore
		{
			get
			{
				_accountStore = _accountStore ?? AccountStore.Create();
				return _accountStore;
			}
		}

		public Account Account
		{
			get
			{
				var accounts = AccountStore.FindAccountsForService(Service);
				if (accounts != null && accounts.Count() > 0)
				{
					if (accounts.Count() > 1)
						throw new InvalidDataContractException("Should only have 1 active account at a time");
					var account = accounts.First();
					return new Account(account);
				}
				return null;
			}
			set
			{
				if (value != Account)
				{
					if (value != null)
						AccountStore.SaveAsync(value, "Forms9PatchAuth");
					else if (Account != null)
						AccountStore.Delete(Account, "Forms9PatchAuth");
					OnPropertyChanged("IsAuthenticated");
				}
			}
		}

		#endregion


		#region Properties
		public static readonly BindableProperty ServiceProperty = BindableProperty.Create("IdProviderName", typeof(string), typeof(ButtonBase), default(string));
		public string Service
		{
			get { return (string)GetValue(ServiceProperty); }
			set { SetValue(ServiceProperty, value); }
		}

		public static readonly BindablePropertyKey RequestedPermissionsPropertyKey = BindableProperty.CreateReadOnly("RequestedPermissions", typeof(HashSet<string>), typeof(ButtonBase), new HashSet<string>());
		public HashSet<string> RequestedPermissions
		{
			get { return (HashSet<string>)GetValue(RequestedPermissionsPropertyKey.BindableProperty); }
			private set { SetValue(RequestedPermissionsPropertyKey, value); }
		}

		public static readonly BindablePropertyKey GrantedPermissionsKeyProperty = BindableProperty.CreateReadOnly("GrantedPermissionsKey", typeof(HashSet<string>), typeof(ButtonBase), null);
		public HashSet<string> GrantedPermissions
		{
			get { return (HashSet<string>)GetValue(GrantedPermissionsKeyProperty.BindableProperty); }
			private set { SetValue(GrantedPermissionsKeyProperty, value); }
		}

		public static readonly BindableProperty AuthUiTypeProperty = BindableProperty.Create("AuthUiType", typeof(UiType), typeof(ButtonBase), UiType.BestAvailable);
		public UiType UiType
		{
			get { return (UiType)GetValue(AuthUiTypeProperty); }
			set { SetValue(AuthUiTypeProperty, value); }
		}

		public bool IsAuthenticated
		{
			get {
				if (Account != null)
				{
					if (Account.ExpirationDate > DateTime.Now)
						return true;
				}
				return false;
			}
		}

		public static readonly BindableProperty LoginTextProperty = BindableProperty.Create("LoginText", typeof(string), typeof(ButtonBase), default(string));
		public string LoginText
		{
			get { return (string)GetValue(LoginTextProperty); }
			set { SetValue(LoginTextProperty, value); }
		}

		public static readonly BindableProperty LogoutTextProperty = BindableProperty.Create("LogoutText", typeof(string), typeof(ButtonBase), default(string));
		public string LogoutText
		{
			get { return (string)GetValue(LogoutTextProperty); }
			set { SetValue(LogoutTextProperty, value); }
		}

		public static readonly BindableProperty ClientIdProperty = BindableProperty.Create("ClientId", typeof(string), typeof(ButtonBase), default(string));
		public string ClientId
		{
			get { return (string)GetValue(ClientIdProperty); }
			set { SetValue(ClientIdProperty, value); }	
		}


		#endregion



		internal Action LoginAction;
		internal Action LogoutAction;
		public event EventHandler<Account> Completed;
		public event EventHandler<string> Error;
		public event EventHandler Cancelled;
		public event EventHandler LoggedOut;

		public ButtonBase()
		{
			OutlineWidth = 0;
			OutlineRadius = 4;
			HasShadow = true;
			RequestedPermissions = new HashSet<string>();
			GrantedPermissions = new HashSet<string>();
			UpdateText();
		}

		void UpdateText()
		{
			if (IsAuthenticated)
				HtmlText = LogoutText ?? (Service != null ? "Sign in with " + Service : "Sign in");
			else
				HtmlText = LoginText ?? "Sign out";	
		}


		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == LoginTextProperty.PropertyName || propertyName == LogoutTextProperty.PropertyName || propertyName == "IsAuthenticated" || propertyName == ServiceProperty.PropertyName)
                UpdateText();
		}



		internal virtual void OnCompleted(Xamarin.Auth.Account account)
		{
			if (account != null)
			{
				var f9pAccount = new Account(account);
				Completed?.Invoke(this, f9pAccount);
			}
			else
				Cancelled?.Invoke(this, EventArgs.Empty);
		}

		internal virtual void OnCompleted(Account account)
		{
			if (account != null)
				Completed?.Invoke(this, account);
			else
				Cancelled?.Invoke(this, EventArgs.Empty);
		}

		internal virtual void OnCancelled()
		{
			Cancelled?.Invoke(this, EventArgs.Empty);
		}

		internal virtual void OnError(string message)
		{
			Error?.Invoke(this, message);
		}

		internal virtual void OnLoggedOut()
		{
			Account = null;
			LoggedOut?.Invoke(this, EventArgs.Empty);
		}



	}
}
