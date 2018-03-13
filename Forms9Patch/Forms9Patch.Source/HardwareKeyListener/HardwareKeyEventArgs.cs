using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    public class HardwareKeyEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the hardware key that was pressed
        /// </summary>
        /// <value>The hardware key.</value>
        public HardwareKey HardwareKey { get; private set; }

        /// <summary>
        /// Gets the visual element that is listening for this hardware key press event
        /// </summary>
        /// <value>The visual element.</value>
        public VisualElement VisualElement { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.HardwareKeyEventArgs"/> class.
        /// </summary>
        /// <param name="hardwareKey">Hardware key.</param>
        /// <param name="visualElement">Visual element.</param>
        public HardwareKeyEventArgs(HardwareKey hardwareKey, VisualElement visualElement)
        {
            HardwareKey = hardwareKey;
            VisualElement = visualElement;
        }
    }
}