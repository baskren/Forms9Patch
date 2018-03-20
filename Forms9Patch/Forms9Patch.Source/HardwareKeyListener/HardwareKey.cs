using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    public class HardwareKey : BindableObject
    {
        #region properties
        /// <summary>
        /// Backing store for the KeyLabel property 
        /// </summary>
        public static readonly BindableProperty KeyLabelProperty = BindableProperty.Create("KeyLabel", typeof(string), typeof(HardwareKey), default(string));
        /// <summary>
        /// A case sensitive string representing the key pressed.  
        /// </summary>
        /// <value>The input.</value>
        public string KeyLabel
        {
            get { return (string)GetValue(KeyLabelProperty); }
            set { SetValue(KeyLabelProperty, value.ToUpper()); }
        }

        /// <summary>
        /// Backing store for the ModifierKeys property
        /// </summary>
        public static readonly BindableProperty ModifierKeysProperty = BindableProperty.Create("ModifierKeys", typeof(HardwareKeyModifierKeys), typeof(HardwareKey), default(HardwareKeyModifierKeys));
        /// <summary>
        /// Flags representing which (if any) modifier keys occompany the pressed key
        /// </summary>
        /// <value>The modifier keys.</value>
        public HardwareKeyModifierKeys ModifierKeys
        {
            get { return (HardwareKeyModifierKeys)GetValue(ModifierKeysProperty); }
            set { SetValue(ModifierKeysProperty, value); }
        }

        /// <summary>
        /// Backing store for the DiscoverableTitle property
        /// </summary>
        public static readonly BindableProperty DiscoverableTitleProperty = BindableProperty.Create("DiscoverableTitle", typeof(string), typeof(HardwareKey), default(string));
        /// <summary>
        /// The text shown, during a modifier key long press, indicating that this key is one that is available with this key press
        /// </summary>
        /// <value>The discoverable title.</value>
        public string DiscoverableTitle
        {
            get { return (string)GetValue(DiscoverableTitleProperty); }
            set { SetValue(DiscoverableTitleProperty, value); }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.HardwareKey"/> class.
        /// </summary>
        /// <param name="keyLabel">KeyLabel.</param>
        /// <param name="modifierKeys">Modifier keys.</param>
        /// <param name="discoverableTitle">Discoverable title.</param>
        public HardwareKey(string keyLabel, HardwareKeyModifierKeys modifierKeys = HardwareKeyModifierKeys.None, string discoverableTitle = null)
        {
            KeyLabel = keyLabel;
            ModifierKeys = modifierKeys;
            DiscoverableTitle = discoverableTitle;
        }


        #region Constants

        /// <summary>
        /// Up arrow key HardwareKey.KeyLabel
        /// </summary>
        public const string UpArrowKeyLabel = "↑ UP ARROW";
        /// <summary>
        /// Down arrow key HardwareKey.KeyLabel
        /// </summary>
        public const string DownArrowKeyLabel = "↓ DOWN ARROW";
        /// <summary>
        /// Left arrow key HardwareKey.KeyLabel
        /// </summary>
        public const string LeftArrowKeyLabel = "← LEFT ARROW";
        /// <summary>
        /// Right arrow key HardwareKey.KeyLabel
        /// </summary>
        public const string RightArrowKeyLabel = "→ RIGHT ARROW";
        /// <summary>
        /// Escape key HardwareKey.KeyLabel
        /// </summary>
        public const string EscapeKeyLabel = "▣ ␛ ESCAPE";

        public const string BackspaceDeleteKeyLabel = "▣ ⌫ BACKSPACE/DELETE";

        public const string ForwardDeleteKeyLabel = "▣ ⌦ FORWARED DELETE";

        public const string InsertKeyLabel = "▣ ⎀ INSERT";

        public const string TabKeyLabel = "▣ ⇥ TAB";

        public const string EnterReturnKeyLabel = "▣ ⏎ ENTER/RETURN";

        public const string PageUpKeyLabel = "▣ ⤒ PAGE UP";

        public const string PageDownKeyLabel = "▣ ⤓ PAGE DOWN";

        public const string HomeKeyLabel = "▣ ⇤ HOME";

        public const string EndKeyLabel = "▣ ⇥ END";

        /* Not supported in iOS */
        public const string F1KeyLabel = "▣ F1";

        public const string F2KeyLabel = "▣ F2";

        public const string F3KeyLabel = "▣ F3";

        public const string F4KeyLabel = "▣ F4";

        public const string F5KeyLabel = "▣ F5";

        public const string F6KeyLabel = "▣ F6";

        public const string F7KeyLabel = "▣ F7";

        public const string F8KeyLabel = "▣ F8";

        public const string F9KeyLabel = "▣ F9";

        public const string F10KeyLabel = "▣ F10";

        public const string F11KeyLabel = "▣ F11";

        public const string F12KeyLabel = "▣ F12";

        public const string Numpad0 = "▣ N0";

        public const string Numpad1 = "▣ N1";

        public const string Numpad2 = "▣ N2";

        public const string Numpad3 = "▣ N3";

        public const string Numpad4 = "▣ N4";

        public const string Numpad5 = "▣ N5";

        public const string Numpad6 = "▣ N6";

        public const string Numpad7 = "▣ N7";

        public const string Numpad8 = "▣ N8";

        public const string Numpad9 = "▣ N9";

        
        #endregion


    }
}