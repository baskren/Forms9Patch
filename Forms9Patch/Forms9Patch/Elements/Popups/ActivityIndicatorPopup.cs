using System;
using System.ComponentModel;
using Xamarin.Forms;
namespace Forms9Patch
{
    /// <summary>
    /// Activity indicator full page overlay.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class ActivityIndicatorPopup : ModalPopup
    {
        #region Properties
        /// <summary>
        /// The color property.
        /// </summary>
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(ActivityIndicatorPopup), Color.Blue, propertyChanged: (b, o, n) =>
        {
            if (b is ActivityIndicatorPopup indicatorPopup)
                indicatorPopup._indicator.Color = indicatorPopup.Color;
        });
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        #endregion


        #region Fields
        readonly ActivityIndicator _indicator = new ActivityIndicator
        {
            BackgroundColor = Color.Transparent,
            Color = Color.Blue

        };


        /*
        internal readonly Xamarin.Forms.Grid Grid = new Xamarin.Forms.Grid
        {

        };

        internal readonly Xamarin.Forms.WebView WebView = new Xamarin.Forms.WebView()
        {
            Opacity = 0.01
        };
        */
        #endregion


        #region Factory
        /// <summary>
        /// Presents an Activity Indicator Page Overlay
        /// </summary>
        /// <param name="popAfter"></param>
        /// <returns></returns>
        public static ActivityIndicatorPopup Create(TimeSpan popAfter = default)
            => new ActivityIndicatorPopup(popAfter) { IsVisible = true };

        /// <summary>
        /// Presents an Activity Indicator Page Overlay
        /// </summary>
        /// <param name="color"></param>
        /// <param name="popAfter"></param>
        /// <returns></returns>
        public static ActivityIndicatorPopup Create(Color color, TimeSpan popAfter = default)
            => new ActivityIndicatorPopup(color, popAfter) { IsVisible = true };
        #endregion


        #region Constructor
        /// <summary>
        /// Constructs an ActivityIndicator Popup
        /// </summary>
        /// <param name="color"></param>
        /// <param name="popAfter"></param>
        public ActivityIndicatorPopup(Color color, TimeSpan popAfter = default) : this(popAfter)
        {
            Color = color;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ActivityIndicatorPopup"/> class.
        /// </summary>
        /// <param name="popAfter"></param>
        public ActivityIndicatorPopup(TimeSpan popAfter = default) : base(popAfter)
        {
            /*
            if (Device.RuntimePlatform == Device.UWP)
                Grid.WidthRequest = 300;

            Grid.Children.Add(WebView);
            Grid.Children.Add(_indicator);

            Content = Grid;
            */

            if (Device.RuntimePlatform == Device.UWP)
                _indicator.WidthRequest = 300;
            Content = _indicator;

            CancelOnPageOverlayTouch = false;
            BackgroundColor = Color.FromRgba(0, 0, 0, 1);
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;
            _indicator.IsVisible = true;
        }
        #endregion


        #region Property Change Management
        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                base.OnPropertyChanged(propertyName);

                if (propertyName == IsVisibleProperty.PropertyName)
                    Activate();
                else if (propertyName == ScaleProperty.PropertyName)
                    _indicator.Scale = Scale;
            });
        }

        void Activate()
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                _indicator.IsRunning = IsVisible;
            });
        }
        #endregion
    }
}
