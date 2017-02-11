using System;
using CoreFoundation;
using AudioToolbox;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.HapticService))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Haptic service.
	/// </summary>
	public class HapticService : IHapticService
	{
		static SystemSound click = new SystemSound(1104);
		static SystemSound modifier = new SystemSound(1156);
		static SystemSound delete = new SystemSound(1155);

		/// <summary>
		/// Feedback the specified effect and mode.
		/// </summary>
		/// <param name="effect">Effect.</param>
		/// <param name="mode">Mode.</param>
		public void Feedback(HapticEffect effect, HapticMode mode = HapticMode.ApplicationDefault)
		{
			//var type = CFPreferences.CurrentApplication;
			//CFPreferences.AppSynchronize();
			//var keyboardPref = CFPreferences.GetAppValue("keyboard", "com.apple.Preferences");
			//var keyboardSoundOn = CFPreferences.GetAppBooleanValue("keyboard", "/var/mobile/Library/Preferences/com.apple.preferences.sounds");

			// It appears that there is not a way to detect if keyboard clicks system setting is enabled (on iOS).  
			// So, if we got this far, play the sound.


			if (Forms9Patch.Settings.Haptics)
			{
				switch (effect)
				{
					case HapticEffect.None:
						return;
					case HapticEffect.KeyClick:
						click.PlaySystemSoundAsync();
						return;
					case HapticEffect.Return:
						modifier.PlaySystemSoundAsync();
						return;
					case HapticEffect.Delete:
						delete.PlaySystemSoundAsync();
						return;
				}
			}
		}

	}
}
