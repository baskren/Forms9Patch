using System;
using Forms9Patch.Interfaces;
using CoreFoundation;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.iOS.AudioService))]
namespace Forms9Patch.iOS
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class AudioService : IAudioService
    {
        static readonly AudioToolbox.SystemSound click = new AudioToolbox.SystemSound(1104);
        static readonly AudioToolbox.SystemSound modifier = new AudioToolbox.SystemSound(1156);
        static readonly AudioToolbox.SystemSound delete = new AudioToolbox.SystemSound(1155);
        //static readonly AudioToolbox.SystemSound message = new AudioToolbox.SystemSound(1007);
        //static readonly AudioToolbox.SystemSound alarm = new AudioToolbox.SystemSound(1005);
        //static readonly AudioToolbox.SystemSound alert = new AudioToolbox.SystemSound(1033);
        //static readonly AudioToolbox.SystemSound error = new AudioToolbox.SystemSound(1073);

        static AVFoundation.AVAudioPlayer alarmPlayer;
        static AVFoundation.AVAudioPlayer alertPlayer;
        static AVFoundation.AVAudioPlayer errorPlayer;
        static AVFoundation.AVAudioPlayer messagePlayer;
        static AVFoundation.AVAudioPlayer notificationPlayer;


        static AudioService()
        {
            var alarmPath = P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResource("Forms9Patch.Resources.Sounds.alarm.mp3", typeof(Forms9Patch.Feedback).Assembly);
            var alertPath = P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResource("Forms9Patch.Resources.Sounds.alert.mp3", typeof(Forms9Patch.Feedback).Assembly);
            var errorPath = P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResource("Forms9Patch.Resources.Sounds.error.mp3", typeof(Forms9Patch.Feedback).Assembly);
            var messagePath = P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResource("Forms9Patch.Resources.Sounds.message.mp3", typeof(Forms9Patch.Feedback).Assembly);
            var notificationPath = P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResource("Forms9Patch.Resources.Sounds.notification.mp3", typeof(Forms9Patch.Feedback).Assembly);
            alarmPlayer = new AVFoundation.AVAudioPlayer(NSUrl.FromFilename(alarmPath), "mp3", out NSError nSError0);
            alertPlayer = new AVFoundation.AVAudioPlayer(NSUrl.FromFilename(alertPath), "mp3", out NSError nSError1);
            errorPlayer = new AVFoundation.AVAudioPlayer(NSUrl.FromFilename(errorPath), "mp3", out NSError nSError2);
            messagePlayer = new AVFoundation.AVAudioPlayer(NSUrl.FromFilename(messagePath), "mp3", out NSError nSError3);
            notificationPlayer = new AVFoundation.AVAudioPlayer(NSUrl.FromFilename(notificationPath), "mp3", out NSError nSError4);
        }


        public void PlaySoundEffect(SoundEffect effect, FeedbackMode mode)
        {
            if (mode == FeedbackMode.Off
                || effect == SoundEffect.None
                || (mode == FeedbackMode.Default && Feedback.SoundMode != FeedbackMode.On))
                return;
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
                case SoundEffect.Info:
                    notificationPlayer.Play();
                    break;
                case SoundEffect.Message:
                    messagePlayer.Play();
                    break;
                case SoundEffect.Alert:
                    alertPlayer.Play();
                    break;
                case SoundEffect.Alarm:
                    alarmPlayer.Play();
                    break;
                case SoundEffect.Error:
                    errorPlayer.Play();
                    break;
            }
        }
    }
}
