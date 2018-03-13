using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    public class HardwareKey : BindableObject
    {
        #region properties
        /// <summary>
        /// Backing store for the Input property 
        /// </summary>
        public static readonly BindableProperty InputProperty = BindableProperty.Create("Input", typeof(string), typeof(HardwareKey), default(string));
        /// <summary>
        /// A case sensitive string representing the key pressed.  
        /// </summary>
        /// <value>The input.</value>
        public string Input
        {
            get { return (string)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
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
        /// <param name="input">Input.</param>
        /// <param name="modifierKeys">Modifier keys.</param>
        /// <param name="discoverableTitle">Discoverable title.</param>
        public HardwareKey(string input, HardwareKeyModifierKeys modifierKeys = HardwareKeyModifierKeys.None, string discoverableTitle = null)
        {
            Input = input;
            ModifierKeys = modifierKeys;
            DiscoverableTitle = discoverableTitle;
        }


        #region Constants

        /// <summary>
        /// Up arrow key HardwareKey.Input
        /// </summary>
        public const string UpArrowInput = "▣ up_arrow";
        /// <summary>
        /// Down arrow key HardwareKey.Input
        /// </summary>
        public const string DownArrowInput = "▣ down_arrow";
        /// <summary>
        /// Left arrow key HardwareKey.Input
        /// </summary>
        public const string LeftArrowInput = "▣ left_arrow";
        /// <summary>
        /// Right arrow key HardwareKey.Input
        /// </summary>
        public const string RightArrowInput = "▣ right_arrow";
        /// <summary>
        /// Escape key HardwareKey.Input
        /// </summary>
        public const string EscapeInput = "▣ escape";

        public const string BackspaceDeleteInput = "▣ backspace/delete";

        public const string ForwardDeleteInput = "▣ forward delete";

        public const string InsertInput = "▣ insert";

        public const string TabInput = "▣ tab";

        public const string EnterReturnInput = "▣ enter/return";

        public const string PageUpInput = "▣ page up";

        public const string PageDownInput = "▣ page down";

        public const string HomeInput = "▣ home";

        public const string EndInput = "▣ end";

        /* Not supported in iOS
        public const string F1Input = "▣ F1";

        public const string F2Input = "▣ F2";

        public const string F3Input = "▣ F3";

        public const string F4Input = "▣ F4";

        public const string F5Input = "▣ F5";

        public const string F6Input = "▣ F6";

        public const string F7Input = "▣ F7";

        public const string F8Input = "▣ F8";

        public const string F9Input = "▣ F9";

        public const string F10Input = "▣ F10";

        public const string F11Input = "▣ F11";

        public const string F12Input = "▣ F12";
        */
        #endregion


    }
}