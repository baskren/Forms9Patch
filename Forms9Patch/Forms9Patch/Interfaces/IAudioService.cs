using System;
namespace Forms9Patch.Interfaces
{
    /// <summary>
    /// Internal use only
    /// </summary>
    public interface IAudioService
    {
        /// <summary>
        /// Internal use only
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="mode"></param>
        void PlaySoundEffect(SoundEffect sound, FeedbackMode mode);
    }
}
