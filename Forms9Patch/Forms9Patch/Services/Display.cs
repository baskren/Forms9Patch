//using System;
using Xamarin.Forms;


namespace Forms9Patch
{
    /// <summary>
    /// Display metrics.
    /// </summary>
    public static class Display
    {
        static IDisplayService _service;
        static IDisplayService Service
        {
            get
            {
                _service = _service ?? DependencyService.Get<IDisplayService>();
                return _service;
            }
        }

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
        public static float Scale => Service.Scale;

        /// <summary>
        /// The width (pixels) of the screen
        /// </summary>
        /// <value>screen width (pixels)</value>
        public static float Width => Service.Width;

        /// <summary>
        /// The hieght (pixels) of the screen
        /// </summary>
        /// <value>screen height (pixels)</value>
        public static float Height => Service.Height;

        /// <summary>
        /// Gets or sets the safe area inset (I'm looking at you, iPhone X).
        /// </summary>
        /// <value>The safe area inset.</value>
        public static Thickness SafeAreaInset => Service.SafeAreaInset;

        public static DisplayOrientation Orientation => Service.Orientation;

        public static bool IsPortrait => (Orientation == DisplayOrientation.Portrait || Orientation == DisplayOrientation.PortraitUpsideDown);

        public static bool IsLandscape => (Orientation == DisplayOrientation.LandscapeLeft || Orientation == DisplayOrientation.LandscapeRight);
        #endregion

    }
}

