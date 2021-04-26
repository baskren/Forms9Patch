using System;
using Android.Content;
using Android.Media;
using Android.OS;
using Forms9Patch.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.Droid.AudioService))]
namespace Forms9Patch.Droid
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class AudioService : IAudioService
    {
        readonly static AudioManager _audio = (Android.Media.AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);
        MediaPlayer mediaPlayer;

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
                switch (effect)
                {
                    case SoundEffect.None:
                        return;
                    case SoundEffect.KeyClick:
                        _audio.PlaySoundEffect(Android.Media.SoundEffect.KeyClick);
                        break;
                    case SoundEffect.Return:
                        _audio.PlaySoundEffect(Android.Media.SoundEffect.Return);
                        break;
                    case SoundEffect.Delete:
                        _audio.PlaySoundEffect(Android.Media.SoundEffect.Delete);
                        break;
                    case SoundEffect.Message:
                        mediaPlayer = MediaPlayer.Create(P42.Utils.Droid.Settings.Context, Android.Provider.Settings.System.DefaultNotificationUri);
                        mediaPlayer.Start();
                        break;
                    case SoundEffect.Alarm:
                    case SoundEffect.Alert:
                        mediaPlayer = MediaPlayer.Create(P42.Utils.Droid.Settings.Context, Android.Provider.Settings.System.DefaultAlarmAlertUri);
                        mediaPlayer.Start();
                        break;
                    case SoundEffect.Error:
                        break;
                }

            }
        }
    }
}
