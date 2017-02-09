using UIKit;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.KeyboardService))]
namespace Forms9Patch.iOS
{
	public class KeyboardService : IKeyboardService
	{
		public KeyboardService()
		{
			UIKeyboard.Notifications.ObserveDidHide(OnHidden);
			UIKeyboard.Notifications.ObserveDidShow(OnShown);
		}

		void OnHidden(object sender, UIKeyboardEventArgs e)
		{
			Forms9Patch.KeyboardService.OnVisiblityChange(KeyboardVisibilityChange.Hidden);
		}

		void OnShown(object sender, UIKeyboardEventArgs e)
		{
			Forms9Patch.KeyboardService.OnVisiblityChange(KeyboardVisibilityChange.Shown);
		}



		public void Hide()
		{
			bool dismissalResult = UIApplication.SharedApplication.KeyWindow.EndEditing(true);
		}


	}
}
