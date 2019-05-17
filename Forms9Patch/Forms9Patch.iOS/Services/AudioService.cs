using System;
using Forms9Patch.Interfaces;
using CoreFoundation;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.AudioService))]
namespace Forms9Patch.iOS
{
    public class AudioService : IAudioService
    {
        static readonly AudioToolbox.SystemSound click = new AudioToolbox.SystemSound(1104);
        static readonly AudioToolbox.SystemSound modifier = new AudioToolbox.SystemSound(1156);
        static readonly AudioToolbox.SystemSound delete = new AudioToolbox.SystemSound(1155);


        public void PlaySoundEffect(SoundEffect effect, EffectMode mode)
        {
            if (mode == EffectMode.Off || effect == SoundEffect.None)
                return;
            if (mode == EffectMode.Default && Forms9Patch.Settings.SoundEffectMode != EffectMode.On)
            {
                if (Forms9Patch.Settings.SoundEffectMode == EffectMode.Default)
                {
                    // this no longer works and there doesn't appear to be a way to detect if keyclicks is on
                    //var type = CFPreferences.CurrentApplication;
                    CFPreferences.AppSynchronize("/var/mobile/Library/Preferences/com.apple.preferences.sounds");
                    if (!CFPreferences.GetAppBooleanValue("keyboard", "/var/mobile/Library/Preferences/com.apple.preferences.sounds"))
                        return;
                }
                else // Forms9Patch.Settings.SoundEffectMode == EffectMode.Off
                    return;
            }
            switch (effect)
            {
                case SoundEffect.KeyClick:
                    click.PlaySystemSound();
                    break;
                case SoundEffect.Return:
                    modifier.PlaySystemSound();
                    break;
                case SoundEffect.Delete:
                    delete.PlaySystemSound();
                    break;
            }
        }
    }
}
