using System;
using Xamarin.Forms;
using Foundation;


namespace FormsGestures.iOS
{
    public static class Settings
    {
        static double _msUntilTapped = 200;

        static Point _swipeVelocityThreshold = new Point(800.0, 800.0);

        /// <summary>
        /// Gets or sets the time between taps to determine if a tap is two single taps or one double tap
        /// </summary>
        /// <value>The tapped threshold.</value>
        public static TimeSpan TappedThreshold
        {
            get { return TimeSpan.FromMilliseconds(_msUntilTapped); }
            set { _msUntilTapped = value.TotalMilliseconds; }
        }

        /// <summary>
        /// Gets or sets the swipe velocity threshold.
        /// </summary>
        /// <value>The swipe velocity threshold.</value>
        public static Point SwipeVelocityThreshold
        {
            get { return _swipeVelocityThreshold; }
            set { _swipeVelocityThreshold = value; }
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        public static void Init()
        {
            P42.Utils.iOS.Settings.Init();
            DisplayService.Init();
        }

    }
}
