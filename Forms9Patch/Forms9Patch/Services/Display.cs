using System;
namespace Forms9Patch
{
    public static class Display
    {
        #region Extension Static Properties
        /*
        /// <summary>
        /// The density (resolution) of the screen (dpi)
        /// </summary>
        /// <value>screen density (dpi)</value>
        public static float Density { get; set; } = 160;
        */

        /// <summary>
        /// The scale (relative to 160 dpi) of the screen
        /// </summary>
        /// <value>screen scale (1x=160dpi)</value>
        public static float Scale => FormsGestures.Display.Scale;

        /// <summary>
        /// The width (pixels) of the screen
        /// </summary>
        /// <value>screen width (pixels)</value>
        public static float Width => FormsGestures.Display.Width;

        /// <summary>
        /// The hieght (pixels) of the screen
        /// </summary>
        /// <value>screen height (pixels)</value>
        public static float Height => FormsGestures.Display.Height;

        /// <summary>
        /// Gets or sets the safe area inset (I'm looking at you, iPhone X).
        /// </summary>
        /// <value>The safe area inset.</value>
        public static Xamarin.Forms.Thickness SafeAreaInset => FormsGestures.Display.SafeAreaInset;

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public static FormsGestures.DisplayOrientation Orientation => FormsGestures.Display.Orientation;

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Forms9Patch.Display"/> is portrait.
        /// </summary>
        /// <value><c>true</c> if is portrait; otherwise, <c>false</c>.</value>
        public static bool IsPortrait => FormsGestures.Display.IsPortrait;

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Forms9Patch.Display"/> is landscape.
        /// </summary>
        /// <value><c>true</c> if is landscape; otherwise, <c>false</c>.</value>
        public static bool IsLandscape => FormsGestures.Display.IsLandscape;
        #endregion

    }
}
