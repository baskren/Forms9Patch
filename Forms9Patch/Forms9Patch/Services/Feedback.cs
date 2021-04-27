using System;
using Forms9Patch.Interfaces;
using Xamarin.Forms;

namespace Forms9Patch
{
    public static class Feedback
    {
        static Feedback() => Settings.ConfirmInitialization();

        /// <summary>
        /// Haptic effect to use if HapticEffect = Default;
        /// </summary>
        public static FeedbackMode HapticMode = FeedbackMode.Default;

        /// <summary>
        /// Sound effect to use if SoundEffect = Default;
        /// </summary>
        public static FeedbackMode SoundMode = FeedbackMode.Default;


        static IAudioService _audioService;
        static IAudioService AudioService => _audioService = _audioService ?? DependencyService.Get<IAudioService>();

        /// <summary>
        /// Invoke system audio effect
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="mode"></param>
        public static void Play(SoundEffect sound, FeedbackMode mode = default)
            => AudioService?.PlaySoundEffect(sound, mode);



        static IHapticsService _hapticService;
        static IHapticsService HapticService => _hapticService = _hapticService ?? DependencyService.Get<IHapticsService>();

        /// <summary>
        /// Invoke haptic feedback
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="mode"></param>
        public static void Play(HapticEffect effect, FeedbackMode mode = default)
            => HapticService?.Feedback(effect, mode);


        public static void Play(FeedbackEffect effect, FeedbackMode mode = default)
        {
            if (effect == FeedbackEffect.None
                || mode == FeedbackMode.Off)
                return;

            switch (effect)
            {
                case FeedbackEffect.Select:
                    Play(HapticEffect.Selection, mode);
                    Play(SoundEffect.Return, mode);
                    break;
                case FeedbackEffect.Delete:
                    Play(HapticEffect.HeavyImpact, mode);
                    Play(SoundEffect.Delete, mode);
                    break;
                case FeedbackEffect.Info:
                    Play(HapticEffect.MediumImpact, mode);
                    Play(SoundEffect.Info, mode);
                    break;
                case FeedbackEffect.Inquiry:
                case FeedbackEffect.Message:
                    Play(HapticEffect.Long, mode);
                    Play(SoundEffect.Message, mode);
                    break;
                case FeedbackEffect.Alert:
                    Play(HapticEffect.WarningNotification, mode);
                    Play(SoundEffect.Alert, mode);
                    break;
                case FeedbackEffect.Alarm:
                    Play(HapticEffect.WarningNotification, mode);
                    Play(SoundEffect.Alarm, mode);
                    break;
                case FeedbackEffect.Error:
                    Play(HapticEffect.ErrorNotification, mode);
                    Play(SoundEffect.Error, mode);
                    break;
            }
        }
    }
}
