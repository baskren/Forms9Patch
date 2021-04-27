using System;
using Xamarin.Forms;
namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Popup Interface.
    /// </summary>
    public interface IPopup : IBackground
    {
        /// <summary>
        /// Padding between popup frame and its content
        /// </summary>
        Thickness Padding { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.IPopup"/> is visible.
        /// </summary>
        /// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the margin.
        /// </summary>
        /// <value>The margin.</value>
        Thickness Margin { get; set; }

        /// <summary>
        /// Gets or sets the horizontal layout options.
        /// </summary>
        /// <value>The horizontal options.</value>
        LayoutOptions HorizontalOptions { get; set; }

        /// <summary>
        /// Gets or sets the vertical layout options.
        /// </summary>
        /// <value>The vertical options.</value>
        LayoutOptions VerticalOptions { get; set; }

        /// <summary>
        /// Gets or sets the target of the popup pointer (if applicable).
        /// </summary>
        /// <value>The target.</value>
        VisualElement Target { get; set; }

        /// <summary>
        /// Gets or sets the color of the background page overlay.
        /// </summary>
        /// <value>The color of the page overlay.</value>
        Color PageOverlayColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.IPopup"/> will cancel on page overlay touch.
        /// </summary>
        /// <value><c>true</c> if cancel on page overlay touch; otherwise, <c>false</c>.</value>
        bool CancelOnPageOverlayTouch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.IPopup"/> will cancel on a back button touch.
        /// </summary>
        bool CancelOnBackButtonClick { get; set; }

        /// <summary>
        /// Gets or sets the TimeSpan before 
        /// </summary>
        /// <value>The fade at.</value>
        TimeSpan PopAfter { get; set; }


        /// <summary>
        /// Object you can set for processing, typically after popup has been acted upon.
        /// </summary>
        object Parameter { get; set; }


    }
}
