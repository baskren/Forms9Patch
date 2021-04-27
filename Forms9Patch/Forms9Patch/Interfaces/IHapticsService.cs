using System;

namespace Forms9Patch.Interfaces
{
    /// <summary>
    /// Internal use only
    /// </summary>
    public interface IHapticsService
    {
        /// <summary>
        /// Internal use only
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="mode"></param>
        void Feedback(HapticEffect effect, FeedbackMode mode = FeedbackMode.Default);
    }
}
