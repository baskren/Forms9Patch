using System;
using Xamarin.Forms;
using System.Windows.Input;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch MaterialButton.
	/// </summary>
	public class MaterialButton : Frame, IDisposable
{
		
		#region Properties
		/// <summary>
		/// UNSUPPORTED INHERITED PROPERTY.
		/// </summary>
		/// <value>The content.</value>
		public new View Content {
			get { throw new NotImplementedException ("[Forms9Patch.MaterialButton] Content property is not supported"); }
			set { throw new NotImplementedException ("[Forms9Patch.MaterialButton] Content property is not supported"); }
		}


		/// <summary>
		/// Backing store for the MaterialButton.HasShadow bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static new readonly BindableProperty HasShadowProperty = BindableProperty.Create ("HasShadow", typeof(bool), typeof(MaterialButton), false);
		/// <summary>
		/// Gets or sets a flag indicating if the MaterialButton has a shadow displayed. This is a bindable property.
		/// </summary>
		/// <value><c>true</c> if this instance has shadow; otherwise, <c>false</c>.</value>
		public new bool HasShadow {
			get { return (bool)GetValue (HasShadowProperty); }
			set { SetValue (HasShadowProperty, value); }
		}
			
		/// <summary>
		/// Backing store for the MaterialButton.Image bindable property.
		/// </summary>
		public static BindableProperty ImageSourceProperty = BindableProperty.Create ("ImageSource", typeof(Xamarin.Forms.ImageSource), typeof(MaterialButton), null);
		/// <summary>
		/// Gets or sets the companion image - alternatively, use ImageText.
		/// </summary>
		/// <value>The image.</value>
		public Xamarin.Forms.ImageSource ImageSource {
			get { return (Xamarin.Forms.ImageSource)GetValue (ImageSourceProperty);}
			set { SetValue (ImageSourceProperty, value); }
		}

		/// <summary>
		/// The image text property backing store
		/// </summary>
		public static readonly BindableProperty IconTextProperty = BindableProperty.Create("IconText", typeof(string), typeof(MaterialButton), default(string));
		/// <summary>
		/// Gets or sets the image text - use this to specify the image as an HTML markup string.
		/// </summary>
		/// <value>The image text.</value>
		public string IconText
		{
			get { return (string)GetValue(IconTextProperty); }
			set { SetValue(IconTextProperty, value); }
		}



		/// <summary>
		/// Backing store for the MaterialButton.Text bindable property.
		/// </summary>
		public static readonly BindableProperty TextProperty =  BindableProperty.Create ("Text", typeof(string), typeof(MaterialButton), null);
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text {
			get { return (string) GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		/// Backing store for the formatted text property.
		/// </summary>
		public static readonly BindableProperty HtmlTextProperty = BindableProperty.Create ("HtmlText", typeof(string), typeof(MaterialButton), null);
		/// <summary>
		/// Gets or sets the formatted text.
		/// </summary>
		/// <value>The formatted text.</value>
		public string HtmlText {
			get { return (string)GetValue (HtmlTextProperty); }
			set { SetValue (HtmlTextProperty, value); }
		}


		/// <summary>
		/// Backing store for the MaterialButton.FontColor bindable property.
		/// </summary>
		public static readonly BindableProperty FontColorProperty = BindableProperty.Create ("FontColor", typeof(Color), typeof(MaterialButton), Color.Default);
		/// <summary>
		/// Gets or sets the color of the font.
		/// </summary>
		/// <value>The color of the font.</value>
		public Color FontColor {
			get { return (Color)GetValue (FontColorProperty); }
			set { SetValue (FontColorProperty, value); }
		}


		/// <summary>
		/// Backing store for the MaterialButton.FontAttributes bindable property.
		/// </summary>
		public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create ( "FontAttributes", typeof(FontAttributes), typeof(MaterialButton), FontAttributes.None);//, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontAttributesPropertyChanged));
		/// <summary>
		/// Gets or sets the font attributes.
		/// </summary>
		/// <value>The font attributes.</value>
		public FontAttributes FontAttributes {
			get { return (FontAttributes)GetValue (FontAttributesProperty); }
			set { SetValue (FontAttributesProperty, value); }
		}


		/// <summary>
		/// Backing store for the MaterialButton.FontSize bindable property.
		/// </summary>
		public static readonly BindableProperty FontSizeProperty = BindableProperty.Create ( "FontSize", typeof(double), typeof(MaterialButton), 12.0);//, BindingMode.OneWay), null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontSizePropertyChanged));
		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		/// <value>The size of the font.</value>
		public double FontSize {
			get { return (double)GetValue (FontSizeProperty); }
			set { SetValue (FontSizeProperty, value); }
		}

		/// <summary>
		/// Backing store for the MaterialButton.FontFamiily bindable property.
		/// </summary>
		public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create ( "FontFamily", typeof(string), typeof(MaterialButton), null);//, BindingMode.OneWay), null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontFamilyPropertyChanged)); 
		/// <summary>
		/// Gets or sets the font family.
		/// </summary>
		/// <value>The font family.</value>
		public string FontFamily {
			get { return (string)GetValue (FontFamilyProperty); }
			set { SetValue (FontFamilyProperty, value); }
		}

		/// <summary>
		/// Backing store for the OutlineWidth bindable property.
		/// </summary>
		public static new readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("OutlineWidth", typeof (float), typeof (MaterialButton), -1.0f);
		/// <summary>
		/// Gets or sets the width of the outline.
		/// </summary>
		/// <value>The width of the outline.</value>
		public new float OutlineWidth {
			get { return (float) GetValue (OutlineWidthProperty); }
			set { SetValue (OutlineWidthProperty, value); }
		}


		/// <summary>
		/// Backing store for the MaterialButton.OutlineColor bindable property.
		/// </summary>
		public static new readonly BindableProperty OutlineColorProperty = BindableProperty.Create("OutlineColor", typeof (Color), typeof (MaterialButton), Color.Default);
		/// <summary>
		/// Gets or sets the color of the border.
		/// </summary>
		/// <value>The color of the border.</value>
		public new Color OutlineColor {
			get { return (Color)GetValue (OutlineColorProperty);}
			set { SetValue (OutlineColorProperty, value); }
		}

		/// <summary>
		/// Backing store for the MaterialButton.BackgroundColor bindable property.
		/// </summary>
		public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof (Color), typeof (MaterialButton), Color.Transparent);
		/// <summary>
		/// Gets or sets the color of the background.
		/// </summary>
		/// <value>The color of the background.</value>
		public new Color BackgroundColor {
			get { return (Color)GetValue (BackgroundColorProperty);}
			set { SetValue (BackgroundColorProperty, value); } }

		/// <summary>
		/// Backing store for the MaterialButton.DarkTheme property.
		/// </summary>
		public static readonly BindableProperty DarkThemeProperty = BindableProperty.Create ("DarkTheme", typeof(bool), typeof(MaterialButton), false);
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MaterialButton"/> if for a dark theme.
		/// </summary>
		/// <value><c>true</c> if dark theme; otherwise, <c>false</c>.</value>
		public bool DarkTheme {
			get { return (bool)GetValue (DarkThemeProperty);}
			set { SetValue (DarkThemeProperty, value); }
		}

		/// <summary>
		/// Backing store for the MaterialButton.Command bindable property.
		/// </summary>
		public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof (ICommand), typeof (MaterialButton), null, BindingMode.OneWay, null, 
			new BindableProperty.BindingPropertyChangedDelegate((bo, o, n) =>
				((MaterialButton)bo).OnCommandChanged ()),
			 null, null, null);
		
		/// <summary>
		/// Gets or sets the command to invoke when the button is activated. This is a bindable property.
		/// </summary>
		/// 
		/// <value>
		/// A command to invoke when the button is activated. The default value is <see langword="null"/>.
		/// </value>
		/// 
		/// <remarks>
		/// This property is used to associate a command with an instance of a button. This property is most often set in the MVVM pattern to bind callbacks back into the ViewModel. <see cref="P:Xamarin.Forms.VisualElement.IsEnabled"/> is controlled by the Command if set.
		/// </remarks>
		public ICommand Command
		{
			get { return (ICommand)GetValue (CommandProperty); }
			set { SetValue (CommandProperty, value); }
		}


		/// <summary>
		/// Backing store for the MaterialButton.CommandParameter bindable property.
		/// </summary>
		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof (object), typeof (MaterialButton), null, BindingMode.OneWay, null, 
			new BindableProperty.BindingPropertyChangedDelegate((bo, o, n) =>
				((MaterialButton)bo).CommandCanExecuteChanged (bo, EventArgs.Empty)),
			 null, null, null);
		/// <summary>
		/// Gets or sets the parameter to pass to the Command property. This is a bindable property.
		/// </summary>
		/// 
		/// <value>
		/// A object to pass to the command property. The default value is <see langword="null"/>.
		/// </value>
		/// 
		/// <remarks/>
		public object CommandParameter
		{
			get { return GetValue (CommandParameterProperty); }
			set { SetValue (CommandParameterProperty, value); }
		}

		/// <summary>
		/// Backing store for the MaterialButton.IsSelected bindable property.
		/// </summary>
		public static BindableProperty IsSelectedProperty = BindableProperty.Create ("IsSelected", typeof(bool), typeof(MaterialButton), false, BindingMode.TwoWay);
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Button"/> is selected.
		/// </summary>
		/// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
		public bool IsSelected {
			get { return (bool)GetValue (IsSelectedProperty); }
			set { SetValue (IsSelectedProperty, value); }
		}

		/// <summary>
		/// OBSOLETE: Use ToggleBehaviorProperty instead.
		/// </summary>
		[Obsolete("StickyBehavior property is obsolete, use ToggleBehavior instead")]
		public static BindableProperty StickyBehaviorProperty;

		/// <summary>
		/// OBSOLETE: Use ToggleBehavior instead.
		/// </summary>
		[Obsolete("StickyBehavior property is obsolete, use ToggleBehavior instead")]
		public bool StickyBehavior
		{
			get { throw new NotSupportedException("StickyBehavior property is obsolete, use ToggleBehavior instead"); }
			set { throw new NotSupportedException("StickyBehavior property is obsolete, use ToggleBehavior instead"); }
		}

		/// <summary>
		/// Backing store for the MaterialButton.ToggleBehavior bindable property.
		/// </summary>
		public static BindableProperty ToggleBehaviorProperty = BindableProperty.Create ("ToggleBehavior", typeof(bool), typeof(MaterialButton), false);
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Button"/> will stay selected or unselected after a tap.
		/// </summary>
		/// <value><c>true</c> if togglable; otherwise, <c>false</c>.</value>
		public bool ToggleBehavior {
			get { return (bool)GetValue (ToggleBehaviorProperty); }
			set { 
				SetValue (ToggleBehaviorProperty, value); 
			}
		}

		/// <summary>
		/// Backing store for the MaterialButton.Alignment bindable property
		/// </summary>
		public static BindableProperty AlignmentProperty = BindableProperty.Create ("Justificaiton", typeof(TextAlignment), typeof(MaterialButton), TextAlignment.Center);
		/// <summary>
		/// Gets or sets the alignment of the image and text.
		/// </summary>
		/// <value>The alignment (left, center, right).</value>
		public TextAlignment Alignment {
			get { return (TextAlignment)GetValue (AlignmentProperty); }
			set { SetValue (AlignmentProperty, value); }
		}

		/// <summary>
		/// Backing store for the MaterialButton's orientation property.
		/// </summary>
		public static BindableProperty OrientationProperty = BindableProperty.Create("Orientation", typeof(StackOrientation), typeof(MaterialButton), StackOrientation.Horizontal);
		/// <summary>
		/// Gets or sets the orientation of the iamge and label.
		/// </summary>
		/// <value>The image and label orientation.</value>
		public StackOrientation Orientation {
			get { return (StackOrientation)GetValue (OrientationProperty); }
			set { SetValue (OrientationProperty, value); }
		}

		/// <summary>
		/// Backing store for the trailing image property.
		/// </summary>
		public static readonly BindableProperty TrailingImageProperty = BindableProperty.Create("TrailingImage", typeof(bool), typeof(MaterialButton), default(bool));
		/// <summary>
		/// Gets or sets if the image is to be rendered after the text.
		/// </summary>
		/// <value>default=false</value>
		public bool TrailingImage
		{
			get { return (bool)GetValue(TrailingImageProperty); }
			set { SetValue(TrailingImageProperty, value); }
		}

		/// <summary>
		/// The lines property.
		/// </summary>
		public static readonly BindableProperty LinesProperty = BindableProperty.Create("Lines", typeof(int), typeof(MaterialButton), 1);
		/// <summary>
		/// Gets or sets the lines.
		/// </summary>
		/// <value>The lines.</value>
		public int Lines
		{
			get { return (int)GetValue(LinesProperty); }
			set { SetValue(LinesProperty, value); }
		}

		/// <summary>
		/// The fit property.
		/// </summary>
		public static readonly BindableProperty FitProperty = BindableProperty.Create("Fit", typeof(LabelFit), typeof(MaterialButton), LabelFit.None);
		/// <summary>
		/// Gets or sets the fit.
		/// </summary>
		/// <value>The fit.</value>
		public LabelFit Fit
		{
			get { return (LabelFit)GetValue(FitProperty); }
			set { SetValue(FitProperty, value); }
		}

		/// <summary>
		/// The line break mode property.
		/// </summary>
		public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create("LineBreakMode", typeof(LineBreakMode), typeof(MaterialButton), LineBreakMode.TailTruncation);
		/// <summary>
		/// Gets or sets the line break mode.
		/// </summary>
		/// <value>The line break mode.</value>
		public LineBreakMode LineBreakMode
		{
			get { return (LineBreakMode)GetValue(LineBreakModeProperty); }
			set { SetValue(LineBreakModeProperty, value); }
		}

		/// <summary>
		/// The backing store for the minimum font size property.
		/// </summary>
		public static readonly BindableProperty MinFontSizeProperty = BindableProperty.Create("MinFontSize", typeof(double), typeof(Label), -1.0);
		/// <summary>
		/// Gets or sets the minimum size of the font allowed during an autofit. 
		/// </summary>
		/// <value>The minimum size of the font.  Default=4</value>
		public double MinFontSize
		{
			get { return (double)GetValue(MinFontSizeProperty); }
			set { SetValue(MinFontSizeProperty, value); }
		}

		/// <summary>
		/// The haptic effect property.
		/// </summary>
		public static readonly BindableProperty HapticEffectProperty = BindableProperty.Create("HapticEffect", typeof(HapticEffect), typeof(MaterialButton), HapticEffect.KeyClick);
		/// <summary>
		/// Gets or sets the haptic effect.
		/// </summary>
		/// <value>The haptic effect.</value>
		public HapticEffect HapticEffect
		{
			get { return (HapticEffect)GetValue(HapticEffectProperty); }
			set { SetValue(HapticEffectProperty, value); }
		}

		/// <summary>
		/// The haptic mode property.
		/// </summary>
		public static readonly BindableProperty HapticModeProperty = BindableProperty.Create("HapticMode", typeof(HapticMode), typeof(MaterialButton), default(HapticMode));
		/// <summary>
		/// Gets or sets the haptic mode.
		/// </summary>
		/// <value>The haptic mode.</value>
		public HapticMode HapticMode
		{
			get { return (HapticMode)GetValue(HapticModeProperty); }
			set { SetValue(HapticModeProperty, value); }
		}


		#endregion


		#region SegmentedButton Properties
		internal static BindableProperty SegmentTypeProperty = BindableProperty.Create("SegmentType", typeof(SegmentType), typeof(MaterialButton), SegmentType.Not);
		internal SegmentType SegmentType {
			get { return (SegmentType)GetValue (SegmentTypeProperty); }
			set { SetValue (SegmentTypeProperty, value); }
		}

		internal static BindableProperty SegmentOrientationProperty = BindableProperty.Create("SegmentOrientation", typeof(StackOrientation), typeof(MaterialButton), StackOrientation.Horizontal);
		internal StackOrientation SegmentOrientation {
			get { return (StackOrientation)GetValue (SegmentOrientationProperty); }
			set { SetValue (SegmentOrientationProperty, value); }
		}

		internal static BindableProperty SeparatorWidthProperty = BindableProperty.Create("SeparatorWidth", typeof(float), typeof(MaterialButton), -1f);
		internal float SeparatorWidth {
			get { return (float)GetValue (SeparatorWidthProperty); }
			set { SetValue (SeparatorWidthProperty, value); }
		}

		internal static BindableProperty GroupToggleBehaviorProperty = BindableProperty.Create("GroupToggleBehavior", typeof(GroupToggleBehavior), typeof(MaterialButton), GroupToggleBehavior.None);
		internal GroupToggleBehavior GroupToggleBehavior {
			get { return (GroupToggleBehavior)GetValue (GroupToggleBehaviorProperty); }
			set { 
				SetValue (GroupToggleBehaviorProperty, value); 
			}
		}
		#endregion


		#region Fields
		bool _noUpdate = true;
		Xamarin.Forms.StackLayout _stackLayout;
		Image _image;
		Label _label;
		Label _iconLabel;
		FormsGestures.Listener _gestureListener;
		#endregion


		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="MaterialButton"/> class.
		/// </summary>
		public MaterialButton ()
		{
			Padding = new Thickness (8, 6, 8, 6);
			OutlineRadius = 2;
			_label = new Label {
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				//HeightRequest = 22,
				MinimizeHeight = true,
				Lines = Lines,
				Fit = Fit,
				LineBreakMode = LineBreakMode,
				MinFontSize = MinFontSize,
				FontSize = FontSize,
				FontAttributes = FontAttributes,
				FontFamily = FontFamily
			};

			_stackLayout = new Xamarin.Forms.StackLayout {
				Orientation = StackOrientation.Horizontal,
				Spacing = 4,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = Alignment.ToLayoutOptions ()
			};

			base.Content = _stackLayout;
			_noUpdate = false;

			_gestureListener = new FormsGestures.Listener (this);
			_gestureListener.Tapped += OnTapped;
			_gestureListener.LongPressed += OnLongPressed;
			_gestureListener.LongPressing += OnLongPressing;

			UpdateState ();

			_label.PropertyChanged +=  OnLabelPropertyChanged;
		}
		#endregion


		#region IDisposable Support
		bool disposedValue; // To detect redundant calls

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">Disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_gestureListener.Tapped -= OnTapped;
					_gestureListener.LongPressed -= OnLongPressed;
					_gestureListener.LongPressing -= OnLongPressing;
					_gestureListener.Dispose();
					_gestureListener = null;
					_label.PropertyChanged -= OnLabelPropertyChanged;
					_label = null;
					if (Command != null)
						Command.CanExecuteChanged -= CommandCanExecuteChanged;
					disposedValue = true;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~MaterialButton() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		/// <summary>
		/// Releases all resource used by the <see cref="T:Forms9Patch.MaterialButton"/> object.
		/// </summary>
		/// <remarks>Call <see cref="O:Forms9Patch.MaterialButton.Dispose"/> when you are finished using the <see cref="T:Forms9Patch.MaterialButton"/>. The
		/// <see cref="O:Forms9Patch.MaterialButton.Dispose"/> method leaves the <see cref="T:Forms9Patch.MaterialButton"/> in an unusable state. After
		/// calling <see cref="O:Forms9Patch.MaterialButton.Dispose"/>, you must release all references to the <see cref="T:Forms9Patch.MaterialButton"/>
		/// so the garbage collector can reclaim the memory that the <see cref="T:Forms9Patch.MaterialButton"/> was occupying.</remarks>
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion


		#region Gesture event responders
		/// <summary>
		/// Tap this instance.
		/// </summary>
		public void Tap()
		{
			OnTapped(this, new FormsGestures.TapEventArgs(null, null));
		}

		void OnTapped(object sender, FormsGestures.TapEventArgs e)
		{
			if (IsEnabled)
			{
				HapticsService.Feedback(HapticEffect, HapticMode);

				//Debug.WriteLine("tapped");
				if (ToggleBehavior && GroupToggleBehavior == GroupToggleBehavior.None
					|| GroupToggleBehavior == GroupToggleBehavior.Multiselect
					|| GroupToggleBehavior == GroupToggleBehavior.Radio && !IsSelected)
					IsSelected = !IsSelected;
				else {
					Opacity = 0.5;
					Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
					{
						Opacity += 0.1;
						return Opacity < 1.0;
					});
				}
				SendTapped();
				e.Handled = true;
			}
		}

		void OnLongPressed(object sender, FormsGestures.LongPressEventArgs e)
		{
			if (IsEnabled)
				_longPressed?.Invoke(this, EventArgs.Empty);
		}

		void OnLongPressing(object sender, FormsGestures.LongPressEventArgs e)
		{
			if (IsEnabled)
				_longPressing?.Invoke(this, EventArgs.Empty);
		}

		#endregion


		#region State management

		/// <summary>
		/// Redraws the button to the current state: Default, Selected, Disabled or DisabledAndSelected.
		/// </summary>
		public void UpdateState() {
			_noUpdate = true;
			base.OutlineWidth = OutlineWidth < 0  ? (BackgroundColor.A > 0 ? 0 : 1) : OutlineColor == Color.Transparent ? 0 : OutlineWidth;
			if (IsEnabled) {
				base.HasShadow = BackgroundColor.A > 0 && HasShadow;
				ShadowInverted = IsSelected && SegmentType==SegmentType.Not;
				_label.TextColor = FontColor == Color.Default ? (DarkTheme ? Color.White : Color.FromHex("#000").WithAlpha(0.5)) : FontColor;
				if (IsSelected) {
					base.BackgroundColor = BackgroundColor.A > 0 ? BackgroundColor.RgbBlend (Color.FromHex ("#000"), 0.25) : Color.FromHex (DarkTheme?"#FFF":"#000").WithAlpha (0.2);
					base.OutlineColor = BackgroundColor.A > 0 ? OutlineColor.RgbBlend (Color.FromHex ("#000"), 0.25) : base.BackgroundColor.A > 0 ? base.BackgroundColor : _label.TextColor;
				} else {
					base.BackgroundColor = BackgroundColor.A > 0 ? BackgroundColor : _label.BackgroundColor;
					base.OutlineColor = OutlineColor;
				}
				if (OutlineColor == Color.Default) 
					//base.OutlineColor = BackgroundColor.A > 0 ? base.BackgroundColor : Color.FromHex (DarkTheme? "#FFF" : "#000").WithAlpha (0.5);
					base.OutlineColor = _label.TextColor;//Color.FromHex (DarkTheme? "#FFF" : "#000").WithAlpha (0.5);
			} else {
				Color opaque, transp;
				if (IsSelected) {
					if (DarkTheme) {
						opaque = Color.FromHex ("#FFF").WithAlpha (0.2);
						transp = Color.FromHex ("#FFF").WithAlpha (0.1);
						_label.TextColor = Color.FromHex ("#FFF").WithAlpha (0.30);
					} else {
						opaque = Color.FromHex ("#000").WithAlpha (0.2);
						transp = Color.FromHex ("#000").WithAlpha (0.1);
						_label.TextColor = Color.FromHex ("#000").WithAlpha (0.26);
					}
				} else {
					transp = Color.Transparent;
					if (DarkTheme) {
						opaque = Color.FromHex ("#FFF").WithAlpha (0.1);
						_label.TextColor = Color.FromHex ("#FFF").WithAlpha (0.30);
					} else {
						opaque = Color.FromHex ("#000").WithAlpha (0.1);
						_label.TextColor = Color.FromHex ("#000").WithAlpha (0.26);
					}
				}
				if (SegmentType == SegmentType.Not) {
					base.HasShadow = false;
					base.BackgroundColor = BackgroundColor.A > 0 ? opaque : transp;
					//base.OutlineColor = OutlineColor.A > 0 ? opaque : base.BackgroundColor;
					base.OutlineColor = OutlineColor==Color.Default || OutlineColor.A > 0 ? opaque : BackgroundColor;
				} else {
					base.OutlineWidth = OutlineWidth < 0  ? (BackgroundColor.A > 0 ? 0 : 1) : OutlineColor==Color.Transparent ? 0 : OutlineWidth;
					base.HasShadow = BackgroundColor.A > 0 && HasShadow;
					if (IsSelected) {
						base.BackgroundColor = BackgroundColor.A > 0 ? BackgroundColor.RgbBlend (Color.FromHex ("#000"), 0.25) : Color.FromHex (DarkTheme?"#FFF":"#000").WithAlpha (0.2);
						base.OutlineColor = BackgroundColor.A > 0 ? OutlineColor.RgbBlend (Color.FromHex ("#000"), 0.25) : base.BackgroundColor.A > 0 ? base.BackgroundColor : _label.TextColor;
					} else {
						base.BackgroundColor = BackgroundColor.A > 0 ? BackgroundColor : _label.BackgroundColor;
						base.OutlineColor = OutlineColor;
					}
					//base.OutlineColor = FontColor == Color.Default ? (DarkTheme ? Color.White : Color.FromHex("#000").WithAlpha(0.5)) : FontColor;
					if (OutlineColor == Color.Default) 
						//base.OutlineColor = BackgroundColor.A > 0 ? base.BackgroundColor : Color.FromHex (DarkTheme? "#FFF" : "#000").WithAlpha (0.5);
						base.OutlineColor = _label.TextColor;//Color.FromHex (DarkTheme? "#FFF" : "#000").WithAlpha (0.5);
				}
				//base.OutlineColor = _label.TextColor;
				//base.OutlineColor = OutlineColor.A > 0 ? opaque : _label.TextColor;//base.BackgroundColor;
				//base.OutlineWidth = base.OutlineWidth > 0
			}
			if (_image != null)
				_image.TintColor = _label.TextColor;
			if (_iconLabel != null)
				_iconLabel.TextColor = _label.TextColor;
			_noUpdate = false;
		}
		#endregion


		#region CurrentProperties

		bool IsEnabledCore
		{
			set
			{
				//this.SetValueCore(VisualElement.IsEnabledProperty, (object) (bool) (value ? true : false), Xamarin.Forms.BindableObject.SetValueFlags.None);
				SetValue(IsEnabledProperty, value );
			}
		}

		event EventHandler _tapped;
		/// <summary>
		/// Occurs when the Button is clicked.
		/// </summary>
		/// 
		/// <remarks>
		/// The user may be able to raise the clicked event using accessibility or keyboard controls when the Button has focus.
		/// </remarks>
		public event EventHandler Tapped {
			add { _tapped += value; }
			remove { _tapped -= value;}
		}

		event EventHandler _selected;
		/// <summary>
		/// Occurs when transitioned to IsSelected=true.
		/// </summary>
		public event EventHandler Selected {
			add { _selected += value; }
			remove { _selected -= value; }
		}

		event EventHandler _longPressing;
		/// <summary>
		/// Occurs when button is actively being long pressed
		/// </summary>
		public event EventHandler LongPressing {
			add { _longPressing += value;}
			remove { _longPressing -= value; }
		}

		event EventHandler _longPressed;
		/// <summary>
		/// Occurs when long pressed has completed.
		/// </summary>
		public event EventHandler LongPressed {
			add { _longPressed += value; }
			remove { _longPressed -= value; }
		}

		#endregion


		#region Change Handlers

		void OnLabelPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			var propertyName = e.PropertyName;
			if (propertyName == Label.TextProperty.PropertyName || propertyName == Label.HtmlTextProperty.PropertyName) {
				if (string.IsNullOrEmpty((string)_label) && _stackLayout.Children.Contains(_label))
					_stackLayout.Children.Remove(_label);
				else if (!string.IsNullOrEmpty((string)_label) && !_stackLayout.Children.Contains(_label))
				{
					if (TrailingImage)
						_stackLayout.Children.Insert(0, _label);
					else
						_stackLayout.Children.Add(_label);
				}
				SetOrienations ();
			}
		}

		/// <param name="propertyName">The name of the changed property.</param>
		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		/// 
		/// <remarks>
		/// A Button triggers this by itself. An inheritor only need to call this for properties without BindableProperty as backend store.
		/// </remarks>
		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);
			if (_noUpdate)
				return;
			if (propertyName == CommandProperty.PropertyName)
			{
				ICommand command = Command;
				if (command != null)
				{
					command.CanExecuteChanged -= CommandCanExecuteChanged;
					//} else if (propertyName == Button.PressingStateProperty.PropertyName && PressingState != null) {
					//	PressingState.PropertyChanged -= OnStatePropertyChanged;
				}
			}
		}


		void SetOrienations() {
			if (Orientation == StackOrientation.Horizontal) {
				if (_image != null) {
					_image.VerticalOptions = LayoutOptions.Center;
					_image.HorizontalOptions = LayoutOptions.Center;
				}
				if (_iconLabel != null)
				{
					_iconLabel.VerticalTextAlignment = TextAlignment.Center;
					_iconLabel.HorizontalTextAlignment = TextAlignment.Center;
				}
				if (_label != null) {
					_label.VerticalTextAlignment = TextAlignment.Center;
					_label.HorizontalTextAlignment = TextAlignment.Center;
					_label.MinimizeHeight = false;
				}
				_stackLayout.Spacing = 4;
			} else {
				if (_image != null) {
					_image.VerticalOptions = LayoutOptions.Center;
					_image.HorizontalOptions = LayoutOptions.CenterAndExpand;
				}
				if (_iconLabel != null)
				{
					_iconLabel.VerticalTextAlignment = TextAlignment.Center;
					_iconLabel.HorizontalTextAlignment = TextAlignment.Center;
				}
				if (_label != null) {
					_label.VerticalTextAlignment = TextAlignment.Center;
					_label.HorizontalTextAlignment = TextAlignment.Center;
					_label.MinimizeHeight = true;
				}
				_stackLayout.Spacing = 0;// _label.FontSize< 0 ? -6 : -_label.FontSize/2.0;
			}
			_stackLayout.Orientation = Orientation;
		}



		/// <param name="propertyName">The name of the changed property.</param>
		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		/// 
		/// <remarks>
		/// A Button triggers this by itself. An inheritor only need to call this for properties without BindableProperty as backend store.
		/// </remarks>
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (_noUpdate)
				return;

			if (propertyName == AlignmentProperty.PropertyName)
			{
				_stackLayout.HorizontalOptions = Alignment.ToLayoutOptions();
			}
			else if (propertyName == ImageSourceProperty.PropertyName)
			{
				if (_image != null)
					_stackLayout.Children.Remove(_image);
				if (ImageSource != null)
				{
					if (_iconLabel != null)
						_stackLayout.Children.Remove(_iconLabel);
					_image = new Image { Source = ImageSource };
					_image.Fill = Fill.AspectFit;
					_image.TintColor = _label.TextColor;
					if (_image != null)
					{
						if (TrailingImage)
							_stackLayout.Children.Add(_image);
						else
							_stackLayout.Children.Insert(0, _image);
						SetOrienations();
					}
				}
			}
			else if (propertyName == IconTextProperty.PropertyName)
			{
				if (_iconLabel != null)
					_stackLayout.Children.Remove(_iconLabel);
				if (IconText != null)
				{
					if (_image != null)
						_stackLayout.Children.Remove(_image);
					_iconLabel = new Label { 
						HtmlText = IconText, 
						TextColor = _label.TextColor, 
						HorizontalTextAlignment = TextAlignment.Center, 
						VerticalTextAlignment = TextAlignment.Center, 
						Lines=0,
					};
					if (_iconLabel != null)
					{
						if (TrailingImage)
							_stackLayout.Children.Add(_iconLabel);
						else
							_stackLayout.Children.Insert(0, _iconLabel);
						SetOrienations();
					}
				}
			}
			else if (propertyName == BackgroundColorProperty.PropertyName
			           || propertyName == OutlineColorProperty.PropertyName
			           || propertyName == OutlineWidthProperty.PropertyName
			           || propertyName == IsSelectedProperty.PropertyName
			           || propertyName == IsEnabledProperty.PropertyName
			           || propertyName == DarkThemeProperty.PropertyName
			           || propertyName == HasShadowProperty.PropertyName
			           || propertyName == SegmentTypeProperty.PropertyName) 
			{
				UpdateState ();
			} 
			else if (propertyName == OrientationProperty.PropertyName) 
			{
				SetOrienations ();
				/*
			} else if (propertyName == FontFamilyProperty.PropertyName) {
				_label.FontFamily = FontFamily;
			} else if (propertyName == FontSizeProperty.PropertyName) {
				_label.FontSize = FontSize;
			} else if (propertyName == FontAttributesProperty.PropertyName) {
				_label.FontAttributes = FontAttributes;
				*/
			} 
			else if (propertyName == FontColorProperty.PropertyName) 
			{
				_label.TextColor = FontColor;
				if (_iconLabel != null)
					_iconLabel.TextColor = FontColor;
				if (_image != null)
					_image.TintColor = FontColor;
			} 
			else if (propertyName == TextProperty.PropertyName) 
			{
				_label.Text = Text;
			} else if (propertyName == HtmlTextProperty.PropertyName) {
				_label.HtmlText = HtmlText;
			}
			if (propertyName == IsSelectedProperty.PropertyName)
			{
				if (IsSelected)
				{
					ICommand command = Command;
					if (command != null && GroupToggleBehavior != GroupToggleBehavior.None)
						command.Execute(CommandParameter);

					EventHandler eventHandler = _selected;
					if (eventHandler != null)
						eventHandler(this, EventArgs.Empty);
				}
			}
			else if (propertyName == LinesProperty.PropertyName)
				_label.Lines = Lines;
			else if (propertyName == FitProperty.PropertyName)
				_label.Fit = Fit;
			else if (propertyName == LineBreakModeProperty.PropertyName)
				_label.LineBreakMode = LineBreakMode;
			else if (propertyName == MinFontSizeProperty.PropertyName)
				_label.MinFontSize = MinFontSize;
			else if (propertyName == FontSizeProperty.PropertyName)
				_label.FontSize = FontSize;
			else if (propertyName == FontAttributesProperty.PropertyName)
				_label.FontAttributes = FontAttributes;
			else if (propertyName == FontFamilyProperty.PropertyName)
				_label.FontFamily = FontFamily;
			else if (propertyName == TrailingImageProperty.PropertyName && _stackLayout.Children.Contains(_label))
			{
				if (TrailingImage)
					_stackLayout.LowerChild(_label);
				else
					_stackLayout.RaiseChild(_label);
			}
		}

		void OnCommandChanged()
		{
			if (Command != null)
			{
				Command.CanExecuteChanged += CommandCanExecuteChanged;
				CommandCanExecuteChanged(this, EventArgs.Empty);
			}
			else
				IsEnabledCore = true;
		}

		void CommandCanExecuteChanged(object sender, EventArgs eventArgs)
		{
			ICommand command = Command;
			if (command == null)
				return;
			IsEnabledCore = command.CanExecute(CommandParameter);
		}

		void SendTapped()
		{
			ICommand command = Command;
			if (command != null && GroupToggleBehavior==GroupToggleBehavior.None)
				command.Execute(CommandParameter);
			_tapped?.Invoke(this, EventArgs.Empty);
		}

		#endregion


	}
}


