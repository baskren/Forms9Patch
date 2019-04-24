using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Hardware key: Defines a key stroke for which to be listened
    /// </summary>
    public class HardwareKey : BindableObject
    {
        #region properties
        /// <summary>
        /// Backing store for the KeyInput property 
        /// </summary>
        public static readonly BindableProperty KeyInputProperty = BindableProperty.Create("KeyInput", typeof(string), typeof(HardwareKey), default(string));
        /// <summary>
        /// A case sensitive string representing the key pressed.  
        /// </summary>
        /// <value>The input.</value>
        public string KeyInput
        {
            get { return (string)GetValue(KeyInputProperty); }
            set { SetValue(KeyInputProperty, value?.ToUpper()); }
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
        /// <param name="keyInput">KeyInput.</param>
        /// <param name="modifierKeys">Modifier keys.</param>
        /// <param name="discoverableTitle">Discoverable title.</param>
        public HardwareKey(string keyInput, HardwareKeyModifierKeys modifierKeys = HardwareKeyModifierKeys.None, string discoverableTitle = null)
        {
            KeyInput = keyInput;
            ModifierKeys = modifierKeys;
            DiscoverableTitle = discoverableTitle;
        }


        #region Constants

        /// <summary>
        /// Up arrow key HardwareKey.KeyInput
        /// </summary>
        public const string UpArrowKeyInput = "↑";
        /// <summary>
        /// Down arrow key HardwareKey.KeyInput
        /// </summary>
        public const string DownArrowKeyInput = "↓";
        /// <summary>
        /// Left arrow key HardwareKey.KeyInput
        /// </summary>
        public const string LeftArrowKeyInput = "←";
        /// <summary>
        /// Right arrow key HardwareKey.KeyInput
        /// </summary>
        public const string RightArrowKeyInput = "→";
        /// <summary>
        /// Escape key HardwareKey.KeyInput
        /// </summary>
        public const string EscapeKeyInput = "␛";

        /// <summary>
        /// The backspace delete key HardwareKey.KeyInput.
        /// </summary>
        public const string BackspaceDeleteKeyInput = "⌫";

        /// <summary>
        /// The forward delete key HardwareKey.KeyInput.
        /// </summary>
        public const string ForwardDeleteKeyInput = "⌦";

        /// <summary>
        /// The insert key HardwareKey.KeyInput.
        /// </summary>
        public const string InsertKeyInput = "⤹";

        /// <summary>
        /// The tab key HardwareKey.KeyInput.
        /// </summary>
        public const string TabKeyInput = "⇥";

        /// <summary>
        /// The enter return key HardwareKey.KeyInput.
        /// </summary>
        public const string EnterReturnKeyInput = "⏎";

        /// <summary>
        /// The page up key HardwareKey.KeyInput.
        /// </summary>
        public const string PageUpKeyInput = "⇧";

        /// <summary>
        /// The page down key HardwareKey.KeyInput.
        /// </summary>
        public const string PageDownKeyInput = "⇩";

        /// <summary>
        /// The home key HardwareKey.KeyInput.
        /// </summary>
        public const string HomeKeyInput = "⇦";

        /// <summary>
        /// The end key HardwareKey.KeyInput.
        /// </summary>
        public const string EndKeyInput = "⇨";

        /* Function keys are not supported in iOS */

        /// <summary>
        /// The f1 key HardwareKey.KeyInput.
        /// </summary>
        public const string F1KeyInput = "①";
        /// <summary>
        /// The f2 key HardwareKey.KeyInput.
        /// </summary>
        public const string F2KeyInput = "②";
        /// <summary>
        /// The f3 key HardwareKey.KeyInput.
        /// </summary>
        public const string F3KeyInput = "③";
        /// <summary>
        /// The f4 key HardwareKey.KeyInput.
        /// </summary>
        public const string F4KeyInput = "④";
        /// <summary>
        /// The f5 key HardwareKey.KeyInput.
        /// </summary>
        public const string F5KeyInput = "⑤";
        /// <summary>
        /// The f6 key HardwareKey.KeyInput.
        /// </summary>
        public const string F6KeyInput = "⑥";
        /// <summary>
        /// The f7 key HardwareKey.KeyInput.
        /// </summary>
        public const string F7KeyInput = "⑦";
        /// <summary>
        /// The f8 key HardwareKey.KeyInput.
        /// </summary>
        public const string F8KeyInput = "⑧";
        /// <summary>
        /// The f9 key HardwareKey.KeyInput.
        /// </summary>
        public const string F9KeyInput = "⑨";
        /// <summary>
        /// The f10 key HardwareKey.KeyInput.
        /// </summary>
        public const string F10KeyInput = "⑩";
        /// <summary>
        /// The f11 key HardwareKey.KeyInput.
        /// </summary>
        public const string F11KeyInput = "⑪";
        /// <summary>
        /// The f12 key HardwareKey.KeyInput.
        /// </summary>
        public const string F12KeyInput = "⑫";

        #endregion


    }
}