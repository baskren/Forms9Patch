// /*******************************************************************
//  *
//  * XaAccount.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Forms9PatchAuth
{
	public class Account : Xamarin.Auth.Account
	{
		internal static string TypeKey = "$type";
		internal static string ServiceKey = "service";
		internal static string AppIdKey = "app_id";
		internal static string AccessTokenKey = "access_token";
		internal static string DeclinedPermissionsKey = "declined_permissions";
		internal static string ExpirationDateKey = "expiration_date";
		internal static string GrantedPermissionsKey = "granted_permissions";
		internal static string RefreshDateKey = "refresh_date";
		internal static string UserIdKey = "user_id";
		internal static string TokenSourceKey = "token_source";


		internal static string UserNameKey = "user_name";
		internal static string UserEmailKey = "user_email";

		public Type Type
		{
			get
			{
				if (Properties.ContainsKey(TypeKey))
					return Type.GetType(Properties[TypeKey]);
				return null;
			}
			internal set
			{
				if (value == null)
				{
					if (Properties.ContainsKey(TypeKey))
						Properties.Remove(TypeKey);
					return;
				}
				Properties[TypeKey] = value.ToString();
			}
		}

		public string Service
		{
			get
			{
				if (Properties.ContainsKey(ServiceKey))
					return Properties[ServiceKey];
				return null;
			}
			internal set
			{
				if (value == null)
				{
					if (Properties.ContainsKey(ServiceKey))
						Properties.Remove(ServiceKey);
					return;
				}
				Properties[ServiceKey] = value;
			}
		}

		public string AppId
		{
			get {
				if (Properties.ContainsKey(AppIdKey))
					return Properties[AppIdKey];
				return null;
			}
			internal set
			{
				if (value == null)
				{
					if (Properties.ContainsKey(AppIdKey))
						Properties.Remove(AppIdKey);
					return;
				}
				Properties[AppIdKey] = value;
			}
		}

		public string AccessToken
		{
			get
			{
				if (Properties.ContainsKey(AccessTokenKey))
					return Properties[AccessTokenKey];
				return null;
			}
			internal set
			{
				if (value == null)
				{
					if (Properties.ContainsKey(AccessTokenKey))
						Properties.Remove(AccessTokenKey);
					return;
				}
				Properties[AccessTokenKey] = value;
			}
		}

		public List<string> DeclinedPermissions
		{
			get
			{
				if (Properties.ContainsKey(DeclinedPermissionsKey))
					return Properties[DeclinedPermissionsKey].Split(',').ToList();
				return null;
			}
			internal set
			{
				if (value == null || value.Count() < 1)
				{
					if (Properties.ContainsKey(DeclinedPermissionsKey))
						Properties.Remove(DeclinedPermissionsKey);
					return;
				}
				Properties[DeclinedPermissionsKey] = string.Join(",",value);
			}
		}

		public DateTime? ExpirationDate
		{
			get
			{
				if (Properties.ContainsKey(ExpirationDateKey))
				{
					var dateTimeString = Properties[ExpirationDateKey];
					long dateTimeLong;
					if (long.TryParse(dateTimeString, out dateTimeLong))
						return new DateTime(dateTimeLong);
				}
				return null;
			}
			internal set
			{
				if (value == null)
				{
					if (Properties.ContainsKey(ExpirationDateKey))
						Properties.Remove(ExpirationDateKey);
					return;
				}
				Properties[ExpirationDateKey] = value.Value.Ticks.ToString();
			}
		}

		public List<string> GrantedPermissions
		{
			get
			{
				if (Properties.ContainsKey(GrantedPermissionsKey))
					return Properties[GrantedPermissionsKey].Split(',').ToList();
				return null;
			}
			internal set
			{
				if (value == null || value.Count() < 1)
				{
					if (Properties.ContainsKey(GrantedPermissionsKey))
						Properties.Remove(GrantedPermissionsKey);
					return;
				}
				Properties[GrantedPermissionsKey] = string.Join(",", value);
			}
		}

		public DateTime? RefreshDate
		{
			get
			{
				if (Properties.ContainsKey(RefreshDateKey))
				{
					var dateTimeString = Properties[RefreshDateKey];
					long dateTimeLong;
					if (long.TryParse(dateTimeString, out dateTimeLong))
						return new DateTime(dateTimeLong);
				}
				return null;
			}
			internal set
			{
				if (value == null)
				{
					if (Properties.ContainsKey(RefreshDateKey))
						Properties.Remove(RefreshDateKey);
					return;
				}
				Properties[RefreshDateKey] = value.Value.Ticks.ToString();
			}
		}

		public string UserId
		{
			get
			{
				if (Properties.ContainsKey(UserIdKey))
					return Properties[UserIdKey];
				return null;
			}
			internal set
			{
				if (value == null)
				{
					if (Properties.ContainsKey(UserIdKey))
						Properties.Remove(UserIdKey);
					return;
				}
				Properties[UserIdKey] = value;
			}
		}

		public string TokenSource
		{
			get
			{
				if (Properties.ContainsKey(TokenSourceKey))
					return Properties[TokenSourceKey];
				return null;
			}
			internal set
			{
				if (value == null)
				{
					if (Properties.ContainsKey(TokenSourceKey))
						Properties.Remove(TokenSourceKey);
					return;
				}
				Properties[TokenSourceKey] = value;
			}
		}


		public string UserName
		{
			get
			{
				if (Properties.ContainsKey(UserNameKey))
					return Properties[UserNameKey];
				return null;
			}
			internal set
			{
				if (value == null)
				{
					if (Properties.ContainsKey(UserNameKey))
						Properties.Remove(UserNameKey);
					return;
				}
				Properties[UserNameKey] = value;
			}
		}

		public string UserEmail
		{
			get
			{
				if (Properties.ContainsKey(UserEmailKey))
					return Properties[UserEmailKey];
				return null;
			}
			internal set
			{
				if (value == null)
				{
					if (Properties.ContainsKey(UserEmailKey))
						Properties.Remove(UserEmailKey);
					return;
				}
				Properties[UserEmailKey] = value;
			}
		}

		public new string Username
		{
			get { throw new InvalidDataContractException("use UserName instead");}
			set { throw new InvalidDataContractException("use UserName instead"); }
		}

		//
		// Constructors
		//
		public Account() : base()
		{
			base.Username = "";
		}

		public Account(Xamarin.Auth.Account account) : base(account.Username, account.Properties, account.Cookies) 
		{
			base.Username = "";
		}

		public override string ToString()
		{
			return string.Format("[XaAccount: \nType=[{0}], \nService=[{1}], \nAppId=[{2}], \nAccessToken=[{3}], \nDeclinedPermissions=[{4}], \nExpirationDate=[{5}], \nGrantedPermissions=[{6}], \nRefreshDate=[{7}], \nUserId=[{8}], \nTokenSource=[{9}], \nUserName=[{10}], \nUserEmail=[{11}]]", Type, Service, AppId, AccessToken, DeclinedPermissions.Description(), ExpirationDate, GrantedPermissions.Description(), RefreshDate, UserId, TokenSource, UserName, UserEmail);
		}
	}
}
