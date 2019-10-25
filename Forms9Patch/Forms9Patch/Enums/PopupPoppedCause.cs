using System;

namespace Forms9Patch
{
    /// <summary>
    /// Causes for Forms9Patch popup being popped
    /// </summary>
    public enum PopupPoppedCause
    {
        /// <summary>
        /// popup's background was touched
        /// </summary>
        BackgroundTouch,
        /// <summary>
        /// Devices [back] button was pressed
        /// </summary>
        HardwareBackButtonPressed,
        /// <summary>
        /// popups IsVisible property was set to false
        /// </summary>
        IsVisiblePropertySet,
        /// <summary>
        /// A method (passed via trigger property) was called that popped the popup
        /// </summary>
        MethodCalled,
        /// <summary>
        /// The popup timed out
        /// </summary>
        Timeout,
        /// <summary>
        /// One of the popup's pre-packaged buttons was tapped
        /// </summary>
        ButtonTapped,
        /// <summary>
        /// One of the popup's pre-packaged segments was tapped
        /// </summary>
        SegmentTapped,
        /// <summary>
        /// User supplied trigger was given to the popup's CancelAsync method
        /// </summary>
        Custom,
        /// <summary>
        /// The popup was disposed
        /// </summary>
        Disposed
    }
}