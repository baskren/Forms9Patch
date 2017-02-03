using System;
using Android.App;
using Android.Content;
using Android.Views.InputMethods;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.Droid.KeyboardService))]
namespace Forms9Patch.Droid
{
	public class KeyboardService : IKeyboardService
	{
		public void Hide()
		{
			var inputMethodManager = Application.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
			if (inputMethodManager != null && Application.Context is Activity)
			{
				var activity = Application.Context as Activity;
				var token = activity.CurrentFocus == null ? null : activity.CurrentFocus.WindowToken;
				inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.ImplicitOnly);
			}		
		}
	}
}
