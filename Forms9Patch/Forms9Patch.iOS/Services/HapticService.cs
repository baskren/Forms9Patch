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
		public void Feedback(HapticEffect effect, HapticMode mode = HapticMode.Default)
		{
			var soundEnabled = (mode & HapticMode.Sound) > 0;
			var vibeEnabled = (mode & HapticMode.Vibrate) > 0;
			if (mode == HapticMode.Default)
			{
				soundEnabled = (Forms9Patch.Settings.HapticMode & HapticMode.Sound) > 0;
				vibeEnabled = (Forms9Patch.Settings.HapticMode & HapticMode.Vibrate) > 0;
				if (Forms9Patch.Settings.HapticMode == HapticMode.Default)
				{
					// this no longer works and there doesn't appear to be a way to detect if keyclicks is on
					var type = CFPreferences.CurrentApplication;
					CFPreferences.AppSynchronize("/var/mobile/Library/Preferences/com.apple.preferences.sounds");
					soundEnabled = CFPreferences.GetAppBooleanValue("keyboard", "/var/mobile/Library/Preferences/com.apple.preferences.sounds");
				}
			}

			if (soundEnabled)
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

			if (vibeEnabled)
				SystemSound.Vibrate.PlaySystemSoundAsync();	
		}

	}
}
