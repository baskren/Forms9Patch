using System;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Aspect mode for Image
	/// </summary>
	public enum Fill 
	{
        /*
        /// <summary>
        /// Do not adjust the image size to fill the available space
        /// </summary>
        None,
        */

		/// <summary>
		/// Scale the image to fit the view. Some parts may be left empty (letter boxing).
		/// </summary>
		AspectFit,// = Xamarin.Forms.Aspect.AspectFit,

        /// <summary>
        /// Scale the image to fill the view. Some parts may be clipped in order to fill the view.
        /// </summary>
        AspectFill,// = Xamarin.Forms.Aspect.AspectFill,

		/// <summary>
		/// 	Scale the image so it exactly fill the view. Scaling may not be uniform in X and Y.
		/// </summary>
		Fill,// = Xamarin.Forms.Aspect.Fill,

		/// <summary>
		/// Tile the unscaled image within the view's boundaries.  
		/// </summary>
		Tile,

	}
}

