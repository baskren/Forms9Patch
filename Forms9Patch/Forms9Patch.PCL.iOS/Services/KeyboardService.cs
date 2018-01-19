using UIKit;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.KeyboardService))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Keyboard service.
	/// </summary>
	public class KeyboardService : IKeyboardService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.iOS.KeyboardService"/> class.
		/// </summary>
		public KeyboardService()
		{
			UIKeyboard.Notifications.ObserveWillHide(OnHidden);
			UIKeyboard.Notifications.ObserveWillShow(OnShown);
		}

		/// <summary>
		/// Ons the hidden.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void OnHidden(object sender, UIKeyboardEventArgs e)
		{
			Forms9Patch.KeyboardService.OnVisiblityChange(KeyboardVisibilityChange.Hidden);
		}

		/// <summary>
		/// Ons the shown.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void OnShown(object sender, UIKeyboardEventArgs e)
		{
			Forms9Patch.KeyboardService.OnVisiblityChange(KeyboardVisibilityChange.Shown);
		}


		/// <summary>
		/// Hide this instance.
		/// </summary>
		public void Hide()
		{
			UIApplication.SharedApplication.KeyWindow.EndEditing(true);
		}


	}
}
