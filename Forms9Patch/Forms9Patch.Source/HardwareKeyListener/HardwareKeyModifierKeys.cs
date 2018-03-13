using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Hardware key modifier keys.
    /// </summary>
    [Flags]
    public enum HardwareKeyModifierKeys
    {
        /// <summary>
        /// No modifier keys (default)
        /// </summary>
        None = 0,
        /// <summary>
        /// The caps lock [Caps] is on when the hardware key was pressed
        /// </summary>
        CapsLock = 1,
        /// <summary>
        /// The shift key was pressed at the same time the hardware key was pressed
        /// </summary>
        Shift = 2,
        /// <summary>
        /// The control [Ctrl] key was pressed at the same time the hardware key was pressed
        /// </summary>
        Control = 4,
        /// <summary>
        /// The alternate (traditional [Alt] or Apple [option]) key was pressed at the same time the hardware key was pressed
        /// </summary>
        Alternate = 8,
        /// <summary>
        /// The platform key (Windows menu [⊞] or Apple [⌘ command])
        /// </summary>
        PlatformKey = 16,
        /// <summary>
        /// Did the key press come from a key on the numberic key pad?
        /// </summary>
        NumericPadKey = 32,
        /// <summary>
        /// Was the key press occompanied by the function [fn] key?
        /// </summary>
        FunctionKey = 64,
    }
}