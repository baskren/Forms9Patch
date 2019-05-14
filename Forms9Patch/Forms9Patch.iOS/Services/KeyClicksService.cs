using System;
using CoreFoundation;
using AudioToolbox;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.KeyClicksService))]
namespace Forms9Patch.iOS
{
    /// <summary>
    /// Haptic service.
    /// </summary>
    public class KeyClicksService : IKeyClicksService
    {
        static SystemSound click = new SystemSound(1104);
        static SystemSound modifier = new SystemSound(1156);
        static SystemSound delete = new SystemSound(1155);

        /// <summary>
        /// Feedback the specified effect and mode.
        /// </summary>
        /// <param name="effect">Effect.</param>
        /// <param name="mode">Mode.</param>
        public void Feedback(HapticEffect effect, KeyClicks mode = KeyClicks.Default)
        {
            var soundEnabled = (mode & KeyClicks.Audio) > 0;
            if (mode == KeyClicks.Default)
            {
                soundEnabled = (Forms9Patch.Settings.KeyClicks & KeyClicks.Audio) > 0;
                if (Forms9Patch.Settings.KeyClicks == KeyClicks.Default)
                {
                    // this no longer works and there doesn't appear to be a way to detect if keyclicks is on
                    //var type = CFPreferences.CurrentApplication;
                    CFPreferences.AppSynchronize("/var/mobile/Library/Preferences/com.apple.preferences.sounds");
                    soundEnabled = CFPreferences.GetAppBooleanValue("keyboard", "/var/mobile/Library/Preferences/com.apple.preferences.sounds");
                }
            }
            if (soundEnabled)
            {
                switch (effect)
                {
                    case HapticEffect.None:
                        break;
                    case HapticEffect.KeyClick:
                        click.PlaySystemSound();
                        break;
                    case HapticEffect.Return:
                        modifier.PlaySystemSound();
                        break;
                    case HapticEffect.Delete:
                        delete.PlaySystemSound();
                        break;
                }
            }

            var hapticEnabled = (mode & KeyClicks.Haptic) > 0;
            if (mode == KeyClicks.Default)
                hapticEnabled = (Forms9Patch.Settings.KeyClicks & (KeyClicks.Default | KeyClicks.Haptic)) > 0;
            if (hapticEnabled && UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                var selection = new UISelectionFeedbackGenerator();
                selection.Prepare();

                // Trigger feedback
                selection.SelectionChanged();
            }
        }

    }
}
