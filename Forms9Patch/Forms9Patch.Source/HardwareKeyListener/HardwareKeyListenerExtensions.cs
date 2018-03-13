using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Forms9Patch
{
    public static class HardwareKeyListenerExtensions
    {
        /// <summary>
        /// Gets the collection of hardware key listeners for this VisualElement
        /// </summary>
        /// <returns>The hardware key listeners.</returns>
        /// <param name="visualElement">Visual element.</param>
        public static ObservableCollection<HardwareKeyListener> GetHardwareKeyListeners(this VisualElement visualElement)
        {
            var behavior = HardwareKeyListenerBehavior.GetFor(visualElement);
            return behavior.HardwareKeyListeners;
        }

        /// <summary>
        /// Instantiates and adds a hardware key listener to this element.
        /// </summary>
        /// <returns>The hardware key listener.</returns>
        /// <param name="visualElement">Visual element.</param>
        /// <param name="input">Input.</param>
        /// <param name="hardwareKeyModifierKeys">Hardware key modifier keys.</param>
        /// <param name="discoverableTitle">Discoverable title.</param>
        public static HardwareKeyListener AddHardwareKeyListener(this VisualElement visualElement, string input, HardwareKeyModifierKeys hardwareKeyModifierKeys = HardwareKeyModifierKeys.None, string discoverableTitle = null)
        {
            var hardwareKeyListener = new HardwareKeyListener
            {
                HardwareKey = new HardwareKey(input, hardwareKeyModifierKeys, discoverableTitle)
            };
            var listeners = visualElement.GetHardwareKeyListeners();
            listeners.Add(hardwareKeyListener);
            return hardwareKeyListener;
        }

        /// <summary>
        /// Adds a hardware key listener to this element.
        /// </summary>
        /// <returns>The hardware key listener.</returns>
        /// <param name="visualElement">Visual element.</param>
        /// <param name="hardwareKeyListener">Hardware key listener.</param>
        public static HardwareKeyListener AddHardwareKeyListener(this VisualElement visualElement, HardwareKeyListener hardwareKeyListener)
        {
            visualElement.GetHardwareKeyListeners().Add(hardwareKeyListener);
            return hardwareKeyListener;
        }

        /// <summary>
        /// Matches a hardware key listener and, if found, removes it from this element.
        /// </summary>
        /// <param name="visualElement">Visual element.</param>
        /// <param name="input">Input.</param>
        /// <param name="hardwareKeyModifierKeys">Hardware key modifier keys.</param>
        public static void RemoveHardwareKeyListener(this VisualElement visualElement, string input, HardwareKeyModifierKeys hardwareKeyModifierKeys = HardwareKeyModifierKeys.None)
        {
            //foreach (var listener in visualElement.GetHardwareKeyListeners())
            var listeners = visualElement.GetHardwareKeyListeners();
            for (int i = 0; i < listeners.Count; i++)
            {
                var key = listeners[i].HardwareKey;
                if (key.Input == input && key.ModifierKeys == hardwareKeyModifierKeys)
                {
                    listeners.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Removes a hardware key listener from this element.
        /// </summary>
        /// <param name="visualElement">Visual element.</param>
        /// <param name="listener">Listener.</param>
        public static void RemoveHardwareKeyListener(this VisualElement visualElement, HardwareKeyListener listener)
        {
            var listeners = visualElement.GetHardwareKeyListeners();
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }

        /*
        /// <summary>
        /// Sets the Xamairin.Forms.VisualElement.Focus and Forms9Patch.HardwareKeyFocus to this VisualElement
        /// </summary>
        /// <returns><c>true</c>, if key focus was hardwared, <c>false</c> otherwise.</returns>
        /// <param name="visualElement">Visual element.</param>
        public static bool HardwareKeyFocus(this VisualElement visualElement)
        {
            HardwareKeyFocus.Element = visualElement;
            return visualElement.Focus();
        }
        */

        /*
        private static void OnUnfocused(object sender, FocusEventArgs e)
        {
            if (e.VisualElement != null)
            {
                if (RootPage.HardwareKeyFocusElement == e.VisualElement)
                    RootPage.HardwareKeyFocusElement = null;
                e.VisualElement.Unfocused -= OnUnfocused;
            }
        }
        */

        /*
        /// <summary>
        /// Removes the Xamairin.Forms.VisualElement.Focus and Forms9Patch.HardwareKeyFocus from this VisualElement
        /// </summary>
        /// <param name="visualElement">Visual element.</param>
        public static void HardwareKeyUnfocus(this VisualElement visualElement)
        {
            HardwareKeyFocus.HardwareKeyFocusElement = null;
            visualElement.Unfocus();
        }

        public static void DefaultHardwareKeyFocus(this VisualElement visualElement)
        {

        }
        */
    }
}