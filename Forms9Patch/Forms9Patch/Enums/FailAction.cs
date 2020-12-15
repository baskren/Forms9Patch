using System;
namespace Forms9Patch
{
    /// <summary>
    /// Options for how Forms9Patch reacts to a failure
    /// </summary>
    public enum FailAction
    {
        /// <summary>
        /// Ignore the failure
        /// </summary>
        Ignore,
        /// <summary>
        /// Show a Alert or Toast Popup
        /// </summary>
        ShowAlert,
        /// <summary>
        /// Throw an exception
        /// </summary>
        ThrowException
    }
}
