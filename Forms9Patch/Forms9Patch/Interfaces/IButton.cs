using System.Windows.Input;

namespace Forms9Patch
{
    /// <summary>
    /// Interface for Forms9Patch button elements
    /// </summary>
    public interface IButton : Xamarin.Forms.IButtonController, IButtonState
    {
        /// <summary>
        /// Gets/Sets color of text when button is selected
        /// </summary>
        Xamarin.Forms.Color SelectedTextColor { get; set; }

        /// <summary>
        /// Gets/Sets background color used when button is selected
        /// </summary>
        Xamarin.Forms.Color SelectedBackgroundColor { get; set; }

        /// <summary>
        /// Gets/Sets ICommand invoked when button is clicked
        /// </summary>
        ICommand Command { get; set; }

        /// <summary>
        /// Gets/Sets command parameter used when Command is invoked
        /// </summary>
        object CommandParameter { get; set; }

        /// <summary>
        /// Gets/Sets if button stays in selected or unselected state with one click and requires a second click to return
        /// </summary>
        bool ToggleBehavior { get; set; }

        /// <summary>
        /// Gets/Sets if the button is enabled
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets/Sets if the button is selected
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Get/Sets the haptic effect to be performed when button is clicked
        /// </summary>
        HapticEffect HapticEffect { get; set; }

        /// <summary>
        /// Gets/Sets the haptic mode used to determine if a HapticEffect will occur when button is clicked
        /// </summary>
        FeedbackMode HapticEffectMode { get; set; }


        /// <summary>
        /// Get/Sets the sound effect to be performed when button is clicked
        /// </summary>
        SoundEffect SoundEffect { get; set; }

        /// <summary>
        /// Gets/Sets the sound mode used to determine if a SoundEffect  will occur when button is clicked
        /// </summary>
        FeedbackMode SoundEffectMode { get; set; }

        /*
        /// <summary>
        /// Enables the detection of long press events
        /// </summary>
        bool IsLongPressEnabled { get; set; }
        */

    }
}
