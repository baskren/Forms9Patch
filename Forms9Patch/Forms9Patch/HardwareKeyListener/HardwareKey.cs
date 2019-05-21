using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Hardware key: Defines a key stroke for which to be listened
    /// </summary>
    public class HardwareKey : BindableObject, IEquatable<HardwareKey>
    {
        #region properties
        /// <summary>
        /// Backing store for the KeyInput property 
        /// </summary>
        public static readonly BindableProperty KeyInputProperty = BindableProperty.Create(nameof(KeyInput), typeof(string), typeof(HardwareKey), default(string));
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
        public static readonly BindableProperty ModifierKeysProperty = BindableProperty.Create(nameof(ModifierKeys), typeof(HardwareKeyModifierKeys), typeof(HardwareKey), default(HardwareKeyModifierKeys));
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
        public static readonly BindableProperty DiscoverableTitleProperty = BindableProperty.Create(nameof(DiscoverableTitle), typeof(string), typeof(HardwareKey), default(string));
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

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Forms9Patch.HardwareKey"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Forms9Patch.HardwareKey"/>.</returns>
        public override string ToString()
            => "{KeyInput[" + KeyInput + "] ModifierKeys[" + ModifierKeys + "] DiscoverableTitle[" + DiscoverableTitle + "]}";


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
        /// The clear key HardwareKey.KeyInput.
        /// </summary>
        public const string ClearKeyInput = "⌧";

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


        #region Equality Operators
        /// <summary>
        /// Determines whether a specified instance of <see cref="Forms9Patch.HardwareKey"/> is equal to another
        /// specified <see cref="Forms9Patch.HardwareKey"/>.
        /// </summary>
        /// <param name="a">The first <see cref="Forms9Patch.HardwareKey"/> to compare.</param>
        /// <param name="b">The second <see cref="Forms9Patch.HardwareKey"/> to compare.</param>
        /// <returns><c>true</c> if <c>a</c> and <c>b</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(HardwareKey a, HardwareKey b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if ((a is null) != (b is null))
                return false;
            if (a.KeyInput != b.KeyInput)
                return false;
            if (a.ModifierKeys != b.ModifierKeys)
                return false;
            return true;
        }

        /// <summary>
        /// Determines whether a specified instance of <see cref="Forms9Patch.HardwareKey"/> is not equal to another
        /// specified <see cref="Forms9Patch.HardwareKey"/>.
        /// </summary>
        /// <param name="a">The first <see cref="Forms9Patch.HardwareKey"/> to compare.</param>
        /// <param name="b">The second <see cref="Forms9Patch.HardwareKey"/> to compare.</param>
        /// <returns><c>true</c> if <c>a</c> and <c>b</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(HardwareKey a, HardwareKey b)
            => !(a == b);

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:Forms9Patch.HardwareKey"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:Forms9Patch.HardwareKey"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="T:Forms9Patch.HardwareKey"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
            => Equals(obj as HardwareKey);

        /// <summary>
        /// Determines whether the specified <see cref="Forms9Patch.HardwareKey"/> is equal to the current <see cref="T:Forms9Patch.HardwareKey"/>.
        /// </summary>
        /// <param name="other">The <see cref="Forms9Patch.HardwareKey"/> to compare with the current <see cref="T:Forms9Patch.HardwareKey"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Forms9Patch.HardwareKey"/> is equal to the current
        /// <see cref="T:Forms9Patch.HardwareKey"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(HardwareKey other)
        {
            return other != null &&
                   KeyInput == other.KeyInput &&
                   ModifierKeys == other.ModifierKeys;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="T:Forms9Patch.HardwareKey"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1819452472;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(KeyInput);
            hashCode = hashCode * -1521134295 + ModifierKeys.GetHashCode();
            return hashCode;
        }
        #endregion
    }
}