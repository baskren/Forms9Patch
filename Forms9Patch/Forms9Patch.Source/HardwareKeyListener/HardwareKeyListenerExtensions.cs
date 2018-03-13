using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Forms9Patch
{
    public static class HardwareKeyListenerExtensions
    {
        internal static ObservableCollection<HardwareKeyListener> GetHardwareKeyListeners(this VisualElement visualElement)
        {
            var behavior = HardwareKeyListenerBehavior.GetFor(visualElement);
            return behavior.HardwareKeyListeners;
        }

        /// <summary>
        /// Gets the collection of hardware key listeners for this Xamarin.Forms.View
        /// </summary>
        /// <returns>The hardware key listeners.</returns>
        /// <param name="view">Xamarin.Forms.View.</param>
        public static ObservableCollection<HardwareKeyListener> GetHardwareKeyListeners(this View view) => GetHardwareKeyListeners(view as VisualElement);

        /// <summary>
        /// Gets the collection of hardware key listeners for this ContentPage
        /// </summary>
        /// <returns>The hardware key listeners.</returns>
        /// <param name="page">Forms9Patch.HardwareKeyPage.</param>
        public static ObservableCollection<HardwareKeyListener> GetHardwareKeyListeners(this Forms9Patch.HardwareKeyPage page) => GetHardwareKeyListeners(page as VisualElement);



        internal static HardwareKeyListener AddHardwareKeyListener(this VisualElement visualElement, string input, HardwareKeyModifierKeys hardwareKeyModifierKeys = HardwareKeyModifierKeys.None, string discoverableTitle = null)
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
        /// Instantiates and adds a hardware key listener to this Xamarin.Forms.View.
        /// </summary>
        /// <returns>The hardware key listener.</returns>
        /// <param name="view">Xamarin.Forms.View.</param>
        /// <param name="input">Input.</param>
        /// <param name="hardwareKeyModifierKeys">Hardware key modifier keys.</param>
        /// <param name="discoverableTitle">Discoverable title.</param>
        public static HardwareKeyListener AddHardwareKeyListener(this View view, string input, HardwareKeyModifierKeys hardwareKeyModifierKeys = HardwareKeyModifierKeys.None, string discoverableTitle = null) => AddHardwareKeyListener(view as VisualElement, input, hardwareKeyModifierKeys, discoverableTitle);

        /// <summary>
        /// Instantiates and adds a hardware key listener to this ContentPage.
        /// </summary>
        /// <returns>The hardware key listener.</returns>
        /// <param name="page">Forms9Patch.HardwareKeyPage.</param>
        /// <param name="input">Input.</param>
        /// <param name="hardwareKeyModifierKeys">Hardware key modifier keys.</param>
        /// <param name="discoverableTitle">Discoverable title.</param>
        public static HardwareKeyListener AddHardwareKeyListener(this Forms9Patch.HardwareKeyPage page, string input, HardwareKeyModifierKeys hardwareKeyModifierKeys = HardwareKeyModifierKeys.None, string discoverableTitle = null) => AddHardwareKeyListener(page as VisualElement, input, hardwareKeyModifierKeys, discoverableTitle);



        internal static HardwareKeyListener AddHardwareKeyListener(this VisualElement visualElement, HardwareKeyListener hardwareKeyListener)
        {
            visualElement.GetHardwareKeyListeners().Add(hardwareKeyListener);
            return hardwareKeyListener;
        }

        /// <summary>
        /// Adds a hardware key listener to this Xamarin.Forms.View.
        /// </summary>
        /// <returns>The hardware key listener.</returns>
        /// <param name="view">Xamarin.Forms.View.</param>
        /// <param name="hardwareKeyListener">Hardware key listener.</param>
        public static HardwareKeyListener AddHardwareKeyListener(this View view, HardwareKeyListener hardwareKeyListener) => AddHardwareKeyListener(view as VisualElement, hardwareKeyListener);

        /// <summary>
        /// Adds a hardware key listener to this ContentPage.
        /// </summary>
        /// <returns>The hardware key listener.</returns>
        /// <param name="page">Forms9Patch.HardwareKeyPage.</param>
        /// <param name="hardwareKeyListener">Hardware key listener.</param>
        public static HardwareKeyListener AddHardwareKeyListener(this Forms9Patch.HardwareKeyPage page, HardwareKeyListener hardwareKeyListener) => AddHardwareKeyListener(page as VisualElement, hardwareKeyListener);



        internal static void RemoveHardwareKeyListener(this VisualElement visualElement, string input, HardwareKeyModifierKeys hardwareKeyModifierKeys = HardwareKeyModifierKeys.None)
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
        /// Matches a hardware key listener and, if found, removes it from this Xamarin.Forms.View.
        /// </summary>
        /// <param name="view">Xamarin.Forms.View.</param>
        /// <param name="input">Input.</param>
        /// <param name="hardwareKeyModifierKeys">Hardware key modifier keys.</param>
        public static void RemoveHardwareKeyListener(this View view, string input, HardwareKeyModifierKeys hardwareKeyModifierKeys = HardwareKeyModifierKeys.None) => RemoveHardwareKeyListener(view as VisualElement, input, hardwareKeyModifierKeys);

        /// <summary>
        /// Matches a hardware key listener and, if found, removes it from this ContentPage.
        /// </summary>
        /// <param name="page">Forms9Patch.HardwareKeyPage.</param>
        /// <param name="input">Input.</param>
        /// <param name="hardwareKeyModifierKeys">Hardware key modifier keys.</param>
        public static void RemoveHardwareKeyListener(this Forms9Patch.HardwareKeyPage page, string input, HardwareKeyModifierKeys hardwareKeyModifierKeys = HardwareKeyModifierKeys.None) => RemoveHardwareKeyListener(page as VisualElement, input, hardwareKeyModifierKeys);



        internal static void RemoveHardwareKeyListener(this VisualElement visualElement, HardwareKeyListener listener)
        {
            var listeners = visualElement.GetHardwareKeyListeners();
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }

        /// <summary>
        /// Removes a hardware key listener from this ContentPage.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="listener">Listener.</param>
        public static void RemoveHardwareKeyListener(this View view, HardwareKeyListener listener) => RemoveHardwareKeyListener(view as VisualElement, listener);

        /// <summary>
        /// Removes a hardware key listener from this Xamarin.Forms.View.
        /// </summary>
        /// <param name="page">Forms9Patch.HardwareKeyPage.</param>
        /// <param name="listener">Listener.</param>
        public static void RemoveHardwareKeyListener(this Forms9Patch.HardwareKeyPage page, HardwareKeyListener listener) => RemoveHardwareKeyListener(page as VisualElement, listener);



        internal static void HardwareKeyFocus(this VisualElement visualElement) => HardwareKeyPage.FocusedElement = visualElement;

        /// <summary>
        /// Sets the HardwareKeyFocus to this view.
        /// </summary>
        /// <param name="view">View.</param>
        public static void HardwareKeyFocus(this View view) => HardwareKeyFocus(view as VisualElement);

        /// <summary>
        /// Sets the HardwareKeyFocus to this page.
        /// </summary>
        /// <param name="page">Page.</param>
        public static void HardwareKeyFocus(this Forms9Patch.HardwareKeyPage page) => HardwareKeyFocus(page as VisualElement);


        /// <summary>
        /// Removes the hardware key focus from this VisualElement
        /// </summary>
        /// <param name="visualElement">Visual element.</param>
        public static void HardwareKeyUnfocus(this VisualElement visualElement) => HardwareKeyPage.FocusedElement = null;

    }
}