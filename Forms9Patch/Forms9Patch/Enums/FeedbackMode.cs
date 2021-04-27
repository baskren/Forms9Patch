using System;
namespace Forms9Patch
{
    /// <summary>
    /// Will a sound effect play?
    /// </summary>
    public enum FeedbackMode
    {
        /// <summary>
        /// Will play depending on the Forms9Patch setting
        /// </summary>
        Default,
        /// <summary>
        /// Will not play
        /// </summary>
        Off,
        /// <summary>
        /// Will play
        /// </summary>
        On,
    }
}
