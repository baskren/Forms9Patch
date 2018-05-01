using System;
using Android.App;

namespace FormsGestures.Droid
{
    public static class Settings
    {
        static Android.Content.Context _context;
        /// <summary>
        /// An activity is a Context because ???  Android!
        /// </summary>
        /// <value>The context.</value>
        public static Android.Content.Context Context
        {
            get => _context ?? Xamarin.Forms.Forms.Context;
            private set => _context = value;

        }

        static double _msUntilTapped = 300;
        /// <summary>
        /// Gets or sets the tapped threshold.
        /// </summary>
        /// <value>The tapped threshold.</value>
        public static TimeSpan TappedThreshold
        {
            get => TimeSpan.FromMilliseconds(_msUntilTapped);
            set => _msUntilTapped = value.TotalMilliseconds;
        }

        static double _msUntilLongPressed = 1100;
        public static TimeSpan LongPressedThreshold
        {
            get => TimeSpan.FromMilliseconds(_msUntilLongPressed);
            set => _msUntilLongPressed = value.TotalMilliseconds;
        }

        static float _swipeVelocityThreshold = 0.1f;
        /// <summary>
        /// Gets or sets the swipe velocity threshold.
        /// </summary>
        /// <value>The swipe velocity threshold.</value>
        public static float SwipeVelocityThreshold
        {
            get => _swipeVelocityThreshold;
            set => _swipeVelocityThreshold = value;
        }


        /// <summary>
        /// Init the specified context.
        /// </summary>
        /// <returns>The init.</returns>
        /// <param name="context">Context.</param>
        public static void Init(Android.Content.Context context = null)
        {
            if (_context != null)
                return;
            Context = context;
            P42.Utils.Droid.Settings.Init(Context);
        }

    }
}

