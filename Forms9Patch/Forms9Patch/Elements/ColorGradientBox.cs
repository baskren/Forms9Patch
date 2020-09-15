using Xamarin.Forms;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Box view with a color gradient fill.  Don't count on this element sticking around for too long.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class ColorGradientBox : Xamarin.Forms.View
    {

        /// <summary>
        /// The start color property.
        /// </summary>
        public static readonly BindableProperty StartColorProperty = BindableProperty.Create(nameof(StartColor), typeof(Color), typeof(ColorGradientBox), default(Color));
        /// <summary>
        /// Gets or sets the start color.
        /// </summary>
        /// <value>The start color.</value>
        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        /// <summary>
        /// The end color property.
        /// </summary>
        public static readonly BindableProperty EndColorProperty = BindableProperty.Create(nameof(EndColor), typeof(Color), typeof(ColorGradientBox), default(Color));
        /// <summary>
        /// Gets or sets the end color.
        /// </summary>
        /// <value>The end color.</value>
        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        /// <summary>
        /// The orientation property.
        /// </summary>
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(ColorGradientBox), default(StackOrientation));
        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public StackOrientation Orientation
        {
            get { return (StackOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        static ColorGradientBox() => Settings.ConfirmInitialization();


    }
}

