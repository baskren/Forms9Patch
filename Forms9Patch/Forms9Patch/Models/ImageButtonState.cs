using Xamarin.Forms;

namespace Forms9Patch
{

	/// <summary>
	/// Describes the properties of a <see cref="ImageButton"/> for a given state.
	/// </summary>
	[ContentProperty("HtmlText")]
	public class ImageButtonState : BindableObject {
		/// <summary>
		/// Backing store for the Image bindable property.
		/// </summary>
		public static BindableProperty ImageProperty = BindableProperty.Create ("Image", typeof(Xamarin.Forms.Image), typeof(ImageButtonState), null);
		/// <summary>
		/// Gets or sets the companion image.
		/// </summary>
		/// <value>The image.</value>
		public Xamarin.Forms.Image Image {
			get { return (Xamarin.Forms.Image)GetValue (ImageProperty);}
			set { SetValue (ImageProperty, value); }
		}

		/// <summary>
		/// Backing store for the Text bindable property.
		/// </summary>
		public static readonly BindableProperty TextProperty =  BindableProperty.Create ("Text", typeof(string), typeof(ImageButtonState), null);
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text {
			get { return (string) GetValue(TextProperty); }
			set { 
				SetValue(TextProperty, value); 
			}
		}

		/// <summary>
		/// The formatted text property backing store.
		/// </summary>
		public static readonly BindableProperty HtmlTextProperty = BindableProperty.Create ("HtmlText", typeof(string), typeof(ImageButtonState), null);
		/// <summary>
		/// Gets or sets the formatted text.
		/// </summary>
		/// <value>The formatted text.</value>
		public string HtmlText {
			get { return (string)GetValue (HtmlTextProperty); }
			set { 
				SetValue (HtmlTextProperty, value); 
			}
		}



		internal bool FontColorSet;
		/// <summary>
		/// Backing store for the FontColor bindable property.
		/// </summary>
		public static readonly BindableProperty FontColorProperty = BindableProperty.Create ("FontColor", typeof(Color), typeof(ImageButtonState), Color.Default, 
			propertyChanged: ((bindable, oldValue, newValue) => {
				((ImageButtonState)bindable).FontColorSet = (((Color)newValue) != Color.Default);
			})
		);
		/// <summary>
		/// Gets or sets the color of the font.
		/// </summary>
		/// <value>The color of the font.</value>
		public Color FontColor {
			get { return (Color)GetValue (FontColorProperty); }
			set { 
				SetValue (FontColorProperty, value); 
				//FontColorSet = true;
			}
		}


		internal bool FontAttributesSet;
		/// <summary>
		/// Backing store for the FontAttributes bindable property.
		/// </summary>
		public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create ( "FontAttributes", typeof(FontAttributes), typeof(ImageButtonState), Xamarin.Forms.FontAttributes.None,
			propertyChanged: (bindable, oldValue, newValue) => {
				((ImageButtonState)bindable).FontAttributesSet = true;
			}
		);//, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontAttributesPropertyChanged));
		/// <summary>
		/// Gets or sets the font attributes.
		/// </summary>
		/// <value>The font attributes.</value>
		public FontAttributes FontAttributes {
			get { return (FontAttributes)GetValue (FontAttributesProperty); }
			set { 
				SetValue (FontAttributesProperty, value); 
				//FontAttributesSet = true;
			}
		}

		internal bool FontSizeSet;
		/// <summary>
		/// Backing store for the FontSize bindable property.
		/// </summary>
		public static readonly BindableProperty FontSizeProperty = BindableProperty.Create ( "FontSize", typeof(double), typeof(ImageButtonState), -1.0,
			propertyChanged: (bindable, oldValue, newValue) => {
				((ImageButtonState)bindable).FontSizeSet = ((double)newValue) >= 0;
			}
		);//, BindingMode.OneWay), null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontSizePropertyChanged));
		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		/// <value>The size of the font.</value>
		public double FontSize {
			get { return (double)GetValue (FontSizeProperty); }
			set { SetValue (FontSizeProperty, value); }
		}

		internal bool FontFamilySet;
		/// <summary>
		/// Backing store for the FontFamiily bindable property.
		/// </summary>
		public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create ( "FontFamily", typeof(string), typeof(ImageButtonState), null,
			propertyChanged: (bindable, oldValue, newValue) => {
				((ImageButtonState)bindable).FontFamilySet = newValue != null;
			}
		);//, BindingMode.OneWay), null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontFamilyPropertyChanged)); 
		/// <summary>
		/// Gets or sets the font family.
		/// </summary>
		/// <value>The font family.</value>
		public string FontFamily {
			get { return (string)GetValue (FontFamilyProperty); }
			set { 
				SetValue (FontFamilyProperty, value); 
				//FontFamilySet = true;
			}
		}

		/*
		/// <summary>
		/// Backing store for the BorderWidth bindable property.
		/// </summary>
		public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create("BorderWidth", typeof (double), typeof (ImageButtonState), (object) -1.0);
		/// <summary>
		/// Gets or sets the width of the border.
		/// </summary>
		/// <value>The width of the border.</value>
		public double BorderWidth {
			get { return (double) GetValue (BorderWidthProperty); }
			set { SetValue (BorderWidthProperty, value); }
		}

		internal bool BorderColorSet;
		/// <summary>
		/// Backing store for the BorderColor bindable property.
		/// </summary>
		public static readonly BindableProperty BorderColorProperty = BindableProperty.Create("BorderColor", typeof (Color), typeof (ImageButtonState), (object) Color.Default);
		/// <summary>
		/// Gets or sets the color of the border.
		/// </summary>
		/// <value>The color of the border.</value>
		public Color BorderColor {
			get { return (Color)GetValue (BorderColorProperty);}
			set { 
				SetValue (BorderColorProperty, value); 
				BorderColorSet = true;
			}
		}

		/// <summary>
		/// Backing store for the BorderRadius bindable property.
		/// </summary>
		public static readonly BindableProperty BorderRadiusProperty = BindableProperty.Create("BorderRadius", typeof (int), typeof (ImageButtonState), (object) -1);
		/// <summary>
		/// Gets or sets the border radius.
		/// </summary>
		/// <value>The border radius.</value>
		public int BorderRadius {
			get { return (int) GetValue (BorderRadiusProperty); }
			set { SetValue (BorderRadiusProperty, value); }
		}
		*/

		internal bool BackgroundColorSet;
		/// <summary>
		/// Backing store for the BackgroundColor bindable property.
		/// </summary>
		public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof (Color), typeof (ImageButtonState), (object) Color.Transparent,
			propertyChanged: (bindable, oldValue, newValue) => {
				((ImageButtonState)bindable).BackgroundColorSet = true;
			}
		);
		/// <summary>
		/// Gets or sets the color of the background.
		/// </summary>
		/// <value>The color of the background.</value>
		public Color BackgroundColor {
			get { return (Color)GetValue (BackgroundColorProperty);}
			set { 
				SetValue (BackgroundColorProperty, value); 
				//BackgroundColorSet = true;
			}
		}


		/// <summary>
		/// Backing store for the BackgroundImage bindable property.
		/// </summary>
		public static BindableProperty BackgroundImageProperty = BindableProperty.Create ("BackgroundImage", typeof(Image), typeof(ImageButtonState), null);
		/// <summary>
		/// Gets or sets the background image.
		/// </summary>
		/// <value>The background image.</value>
		public Image BackgroundImage {
			get { return (Image)GetValue (BackgroundImageProperty); }
			set { 
				SetValue (BackgroundImageProperty, value); 
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageButtonState"/> class.
		/// </summary>
		public ImageButtonState () {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageButtonState"/> class to the same properties of Source instance.
		/// </summary>
		/// <param name="source">Source instance.</param>
		public ImageButtonState (ImageButtonState source) {
			//cancelEvents = true;
			Image = source.Image;
			Text = source.Text;
			HtmlText = source.HtmlText;

			FontColor = source.FontColor;
			FontColorSet = source.FontColorSet;
			FontAttributes = source.FontAttributes;
			FontAttributesSet = source.FontAttributesSet;
			FontSize = source.FontSize;
			FontSizeSet = source.FontSizeSet;
			FontFamily = source.FontFamily;
			FontFamilySet = source.FontFamilySet;

			BackgroundColor = source.BackgroundColor;
			BackgroundColorSet = source.BackgroundColorSet;
			BackgroundImage = source.BackgroundImage;
		}

		/// <param name="propertyName">The name of the property that changed.</param>
		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		protected override void OnPropertyChanged (string propertyName = null)
		{
			base.OnPropertyChanged (propertyName);
			if (propertyName == HtmlTextProperty.PropertyName && HtmlText != null) {
				Text = null;
			} else if (propertyName == TextProperty.PropertyName && Text != null) {
				HtmlText = null;
			}

		}

		#region Operators

		/// <param name="state">State.</param>
		public static explicit operator string (ImageButtonState state) {
			return state.HtmlText ?? state.Text;
		}

		#endregion
	}
}

