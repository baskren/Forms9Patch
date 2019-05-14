using System;
namespace Forms9Patch
{
    /// <summary>
    /// The different haptic modes
    /// </summary>
    [Flags]
    public enum KeyClicks
    {
        /// <summary>
        /// No haptic response
        /// </summary>
        Off = 0,
        /// <summary>
        /// The system default haptic, if detectable (which it is not on iOS)
        /// </summary>
        Default = 1,
        /// <summary>
        /// GIVE ME TOUCH SOUNDS!
        /// </summary>
        Audio = 2,
        /// <summary>
        /// GIVE ME TOUCH Haptic Feedback!
        /// </summary>
        Haptic = 4,
        /// <summary>
        /// Both audio and haptic
        /// </summary>
        AudioHaptic = 6,
    }
}
