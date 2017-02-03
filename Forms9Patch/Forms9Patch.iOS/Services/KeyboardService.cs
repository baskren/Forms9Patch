using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.KeyboardService))]
namespace Forms9Patch.iOS
{
	public class KeyboardService : IKeyboardService
	{
		public void Hide()
		{
			bool dismissalResult = UIApplication.SharedApplication.KeyWindow.EndEditing(true);
		}
	}
}
