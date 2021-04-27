using System;
namespace Forms9Patch
{
    /// <summary>
    /// Haptic effect used if haptics is enabled
    /// </summary>
    public enum HapticEffect
    {
        /// <summary>
        /// I'll just sit here quietly.
        /// </summary>
        None,
        /// <summary>
        /// Use this feedback generator for a selection actively changing such as picking an item from a list.
        /// </summary>
        Selection,
        /// <summary>
        /// A collision between small, light user interface elements.
        /// </summary>
        LightImpact,
        /// <summary>
        /// A collision between moderately sized user interface elements.
        /// </summary>
        MediumImpact,
        /// <summary>
        /// A collision between big, heavy user interface elements.
        /// </summary>
        HeavyImpact,
        /// <summary>
        /// A notification feedback type, indicating that a task has completed successfully.
        /// </summary>
        SuccessNotification,
        /// <summary>
        /// A notification feedback type, indicating that a task has produced a warning.
        /// </summary>
        WarningNotification,
        /// <summary>
        /// A notification feedback type, indicating that a task has failed.
        /// </summary>
        ErrorNotification,
        /// <summary>
        /// Message: a longer vibration to get your attention
        /// </summary>
        Long,
    }
}
