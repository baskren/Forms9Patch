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
        static int notificationId;
        static int messageId;
        static int alertId;
        static int alarmId;
        static int errorId;

        static SoundPool _soundPool;
        static Vibrator Vibrator;

        static AudioService()
        {
            var alarmPath = P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResource("Forms9Patch.Resources.Sounds.alarm.ogg", typeof(Forms9Patch.Feedback).Assembly);
            var alertPath = P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResource("Forms9Patch.Resources.Sounds.alert.ogg", typeof(Forms9Patch.Feedback).Assembly);
            var errorPath = P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResource("Forms9Patch.Resources.Sounds.error.ogg", typeof(Forms9Patch.Feedback).Assembly);
            var messagePath = P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResource("Forms9Patch.Resources.Sounds.message.ogg", typeof(Forms9Patch.Feedback).Assembly);
            var notificationPath = P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResource("Forms9Patch.Resources.Sounds.notification.ogg", typeof(Forms9Patch.Feedback).Assembly);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var audioAttributes = new AudioAttributes.Builder()
                    .SetUsage(AudioUsageKind.NotificationEvent)
                    .SetContentType(AudioContentType.Sonification)
                    .Build();
                _soundPool = new SoundPool.Builder()
                    .SetMaxStreams(5)
                    .SetAudioAttributes(audioAttributes)
                    .Build();
            }
            else
            {
                _soundPool = new SoundPool(5, Stream.Alarm, 0);
            }

            alarmId = _soundPool.Load(alarmPath, 1);
            alertId = _soundPool.Load(alertPath, 1);
            errorId = _soundPool.Load(errorPath, 1);
            messageId = _soundPool.Load(messagePath, 1);
            notificationId = _soundPool.Load(notificationPath, 1);

        }

        readonly static AudioManager _audio = (Android.Media.AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);
        MediaPlayer mediaPlayer;

        public void PlaySoundEffect(SoundEffect effect, FeedbackMode mode)
        {
            if (mode == FeedbackMode.Off
                || effect == SoundEffect.None
                || (mode == FeedbackMode.Default && Feedback.SoundMode == FeedbackMode.Off
                || _audio is null
                || _soundPool is null))
                return;
            if (mode == FeedbackMode.Default)
            {
                if (Feedback.HapticMode != FeedbackMode.On)
                {
                    var enabled = Android.Provider.Settings.System.GetInt(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.System.SoundEffectsEnabled) != 0;
                    if (!enabled)
                        return;
                }
            }
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
                case SoundEffect.Info:
                    _soundPool.Play(notificationId, 1, 1, 0, 0, 1);
                    break;
                case SoundEffect.Message:
                    _soundPool.Play(messageId, 1, 1, 0, 0, 1);
                    break;
                case SoundEffect.Alert:
                    _soundPool.Play(alertId, 1, 1, 0, 0, 1);
                    break;
                case SoundEffect.Alarm:
                    _soundPool.Play(alarmId, 1, 1, 0, 0, 1);
                    break;
                case SoundEffect.Error:
                    _soundPool.Play(errorId, 1, 1, 0, 0, 1);
                    break;
            }

        }
    }
}
