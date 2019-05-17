using System;
using Android.Content;
using Android.OS;
using Forms9Patch.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.Droid.AudioService))]
namespace Forms9Patch.Droid
{
    public class AudioService : IAudioService
    {
        readonly static Android.Media.AudioManager _audio = (Android.Media.AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);

        public void PlaySoundEffect(SoundEffect effect, EffectMode mode)
        {
            if (mode == EffectMode.Off || effect == SoundEffect.None || _audio == null)
                return;
            if (mode == EffectMode.Default)
            {
                var enabled = (Forms9Patch.Settings.KeyClicks & KeyClicks.Audio) > 0;
                if (Forms9Patch.Settings.KeyClicks == KeyClicks.Default)
                {
                    enabled = Android.Provider.Settings.System.GetInt(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.System.SoundEffectsEnabled) != 0;
                    //vibeEnabled = Android.Provider.Settings.System.GetInt(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.System.HapticFeedbackEnabled) != 0;
                }
                if (!enabled)
                    return;
            }
            if (_audio != null)
            {
                Android.Media.SoundEffect droidSound = Android.Media.SoundEffect.KeyClick;
                switch (effect)
                {
                    case SoundEffect.None:
                        return;
                    case SoundEffect.KeyClick:
                        break;
                    case SoundEffect.Return:
                        droidSound = Android.Media.SoundEffect.Return;
                        break;
                    case SoundEffect.Delete:
                        droidSound = Android.Media.SoundEffect.Delete;
                        break;
                }
                _audio.PlaySoundEffect(droidSound);

            }
        }
    }
}
