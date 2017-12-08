using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch RoundedBox interface
    /// </summary>
    public interface IShape : IElement
    {

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has shadow.
        /// </summary>
        /// <value><c>true</c> if this instance has shadow; otherwise, <c>false</c>.</value>
        bool HasShadow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Forms9Patch.IShape"/> shadow is inverted.
        /// </summary>
        /// <value><c>true</c> if shadow inverted; otherwise, <c>false</c>.</value>
        bool ShadowInverted { get; set; }

        /// <summary>
        /// Gets or sets the color of the outline.
        /// </summary>
        /// <value>The color of the outline.</value>
        Color OutlineColor { get; set; }

        /// <summary>
        /// Gets or sets the outline radius.
        /// </summary>
        /// <value>The outline radius.</value>
        float OutlineRadius { get; set; }

        /// <summary>
        /// Gets or sets the width of the outline.
        /// </summary>
        /// <value>The width of the outline.</value>
        float OutlineWidth { get; set; }

        /// <summary>
        /// For internal use only.  Goes beyond ElementShape to allow setting of segment shapes
        /// </summary>
        ExtendedElementShape ExtendedElementShape { get; set; }

        /// <summary>
        /// Gets or sets the geometry of the shape
        /// </summary>
        ElementShape ElementShape { get; set; }

        /// <summary>
        /// What is the shadow and pointer padding?
        /// </summary>
        /// <returns></returns>
        Xamarin.Forms.Thickness ShadowPadding();

    }
}

