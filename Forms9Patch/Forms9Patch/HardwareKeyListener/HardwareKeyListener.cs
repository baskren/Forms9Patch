using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Hardware key listener: Configures what key stroke to listen for and what to do when it's heard.
    /// </summary>
    public class HardwareKeyListener : BindableObject
    {
        #region properties
        /// <summary>
        /// Backing store for the HardwareKey property.
        /// </summary>
        public static readonly BindableProperty HardwareKeyProperty = BindableProperty.Create(nameof(HardwareKey), typeof(HardwareKey), typeof(HardwareKeyListener), default(HardwareKey));
        /// <summary>
        /// Gets or sets the hardware key for which to be listened.
        /// </summary>
        /// <value>The hardware key.</value>
        public HardwareKey HardwareKey
        {
            get => (HardwareKey)GetValue(HardwareKeyProperty);
            set => SetValue(HardwareKeyProperty, value);
        }

        /// <summary>
        /// Backing store for the Command property
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(HardwareKeyListener), null);
        /// <summary>
        /// Gets or sets the command to be invoked when the HardwareKey is pressed
        /// </summary>
        /// <value>The command.</value>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// The CommandParameter backing store
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(HardwareKeyListener), null);
        /// <summary>
        /// Gets or sets the command parameter  sent with the command that is invoked when the key is pressed.
        /// </summary>
        /// <value>The command parameter.</value>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// The event that is triggered when this hardware key is pressed.
        /// </summary>
        public EventHandler<HardwareKeyEventArgs> Pressed;
        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.HardwareKeyListener"/> class.
        /// </summary>
        /// <param name="hardwareKey">Hardware key.</param>
        /// <param name="onPressed">On pressed.</param>
        public HardwareKeyListener(HardwareKey hardwareKey, EventHandler<HardwareKeyEventArgs> onPressed = null)
        {
            HardwareKey = hardwareKey;
            if (onPressed != null)
                Pressed += onPressed;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Forms9Patch.HardwareKeyListener"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Forms9Patch.HardwareKeyListener"/>.</returns>
        public override string ToString()
        {
            return "{Key:[" + HardwareKey + "]}";
        }
    }
}