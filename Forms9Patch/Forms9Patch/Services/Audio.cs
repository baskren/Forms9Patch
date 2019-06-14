using System;
using Forms9Patch.Interfaces;
using Xamarin.Forms;
namespace Forms9Patch
{
    /// <summary>
    /// Audio system
    /// </summary>
    public static class Audio
    {
        static Audio()
            => Settings.ConfirmInitialization();

        static IAudioService _service;
        static IAudioService Service => _service = _service ?? DependencyService.Get<IAudioService>();

        /// <summary>
        /// Invoke system audio effect
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="mode"></param>
        public static void PlaySoundEffect(SoundEffect sound, EffectMode mode = default)
            => Service?.PlaySoundEffect(sound, mode);

    }
}
