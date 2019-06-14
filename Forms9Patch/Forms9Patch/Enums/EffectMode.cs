using System;
namespace Forms9Patch
{
    /// <summary>
    /// Will a sound effect play?
    /// </summary>
    public enum EffectMode
    {
        /// <summary>
        /// Will not play
        /// </summary>
        Off,
        /// <summary>
        /// Will play depending on the Forms9Patch setting
        /// </summary>
        Default,
        /// <summary>
        /// Will play
        /// </summary>
        On,
    }
}
