using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Box view with a color gradient fill.  Don't count on this element sticking around for too long.
	/// </summary>
	internal class ColorGradientBox : Xamarin.Forms.View {

		/// <summary>
		/// The start color property.
		/// </summary>
		public static readonly BindableProperty StartColorProperty = BindableProperty.Create ("StartColor", typeof(Color), typeof(ColorGradientBox), default(Color));
		/// <summary>
		/// Gets or sets the start color.
		/// </summary>
		/// <value>The start color.</value>
		public Color StartColor {
			get { return (Color)GetValue (StartColorProperty); }
			set { SetValue (StartColorProperty, value); }
		}

		/// <summary>
		/// The end color property.
		/// </summary>
		public static readonly BindableProperty EndColorProperty = BindableProperty.Create ("EndColor", typeof(Color), typeof(ColorGradientBox), default(Color));
		/// <summary>
		/// Gets or sets the end color.
		/// </summary>
		/// <value>The end color.</value>
		public Color EndColor {
			get { return (Color)GetValue (EndColorProperty); }
			set { SetValue (EndColorProperty, value); }
		}

		/// <summary>
		/// The orientation property.
		/// </summary>
		public static readonly BindableProperty OrientationProperty = BindableProperty.Create ("Orientation", typeof(StackOrientation), typeof(ColorGradientBox), default(StackOrientation));
		/// <summary>
		/// Gets or sets the orientation.
		/// </summary>
		/// <value>The orientation.</value>
		public StackOrientation Orientation {
			get { return (StackOrientation)GetValue (OrientationProperty); }
			set { SetValue (OrientationProperty, value); }
		}
			
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.ColorGradientBox"/> class.
		/// </summary>
		public ColorGradientBox() {
			//BackgroundColor = Color.Transparent;
		}

	}
}

