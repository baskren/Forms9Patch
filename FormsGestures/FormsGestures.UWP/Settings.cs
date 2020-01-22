using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FormsGestures.UWP
{
    public class Settings
    {
        static Windows.UI.Xaml.Application _application;
        static Windows.UI.Xaml.Application Application
        {
            get
            {
                return _application ?? Windows.UI.Xaml.Application.Current;
            }
            set
            {
                _application = value;
            }
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



        public static void Init(Windows.UI.Xaml.Application applcation)
        {
            if (_application != null)
                return;
            Application = applcation;
            P42.Utils.UWP.Settings.Init(Application);
            Xamarin.Forms.DependencyService.Register<DisplayService>();
            Xamarin.Forms.DependencyService.Register<GestureService>();
            Xamarin.Forms.DependencyService.Register<CoordinateTransformService>();
        }
    }
}
