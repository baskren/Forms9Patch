using System;
using Forms9Patch.Interfaces;
using Xamarin.Forms;
namespace Forms9Patch
{
    public static class Audio
    {
        static Audio()
            => Settings.ConfirmInitialization();

        static IAudioService _service;
        static IAudioService Service => _service = _service ?? DependencyService.Get<IAudioService>();

        public static void PlaySoundEffect(SoundEffect sound, EffectMode mode = default)
            => Service?.PlaySoundEffect(sound, mode);

    }
}
