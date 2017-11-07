using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch RoundedBox interface
	/// </summary>
	public interface IRoundedBox
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
		/// Gets or sets a value indicating whether this <see cref="Forms9Patch.IRoundedBox"/> shadow is inverted.
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
		/// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.IRoundedBox"/> is elliptical  (rather than rectangular with rounded corners).
		/// </summary>
		/// <value><c>true</c> if elliptical; otherwise, <c>false</c>.</value>
		bool IsElliptical { get; set; }

        /// <summary>
        /// Gets or sets the shape of the button
        /// </summary>
        //ButtonShape ButtonShape { get; set; }


        /// <summary>
        /// Gets or sets the background image
        /// </summary>
        //Forms9Patch.Image BackgroundImage { get; set; }

        /// <summary>
        /// Incremental instance id (starting at zero, increasing by one for each new instance)
        /// </summary>
        int InstanceId { get; }
	}
}

