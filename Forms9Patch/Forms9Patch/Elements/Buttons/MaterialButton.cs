using System;
using Xamarin.Forms;
using System.Windows.Input;
using PCL.Utils;

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
        public new View Content
        {
            get { throw new NotImplementedException("[Forms9Patch.MaterialButton] Content property is not supported"); }
            set { throw new NotImplementedException("[Forms9Patch.MaterialButton] Content property is not supported"); }
        }


        /// <summary>
        /// Backing store for the MaterialButton.HasShadow bindable property.
        /// </summary>
        /// <remarks></remarks>
        public static new readonly BindableProperty HasShadowProperty = BindableProperty.Create("HasShadow", typeof(bool), typeof(MaterialButton), false);
        /// <summary>
        /// Gets or sets a flag indicating if the MaterialButton has a shadow displayed. This is a bindable property.
        /// </summary>
        /// <value><c>true</c> if this instance has shadow; otherwise, <c>false</c>.</value>
        public new bool HasShadow
        {
            get { return (bool)GetValue(HasShadowProperty); }
            set { SetValue(HasShadowProperty, value); }
        }

        /// <summary>
        /// Backing store for the MaterialButton.Image bindable property.
        /// </summary>
        public static BindableProperty ImageSourceProperty = BindableProperty.Create("ImageSource", typeof(Xamarin.Forms.ImageSource), typeof(MaterialButton), null);
        /// <summary>
        /// Gets or sets the companion image - alternatively, use ImageText.
        /// </summary>
        /// <value>The image.</value>
        public Xamarin.Forms.ImageSource ImageSource
        {
            get { return (Xamarin.Forms.ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
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
        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(MaterialButton), null);
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Backing store for the formatted text property.
        /// </summary>
        public static readonly BindableProperty HtmlTextProperty = BindableProperty.Create("HtmlText", typeof(string), typeof(MaterialButton), null);
        /// <summary>
        /// Gets or sets the formatted text.
        /// </summary>
        /// <value>The formatted text.</value>
        public string HtmlText
        {
            get { return (string)GetValue(HtmlTextProperty); }
            set { SetValue(HtmlTextProperty, value); }
        }


        /// <summary>
        /// Backing store for the MaterialButton.FontColor bindable property.
        /// </summary>
        public static readonly BindableProperty FontColorProperty = BindableProperty.Create("FontColor", typeof(Color), typeof(MaterialButton), Color.Default);
        /// <summary>
        /// Gets or sets the color of the font.
        /// </summary>
        /// <value>The color of the font.</value>
        public Color FontColor
        {
            get { return (Color)GetValue(FontColorProperty); }
            set { SetValue(FontColorProperty, value); }
        }

        /// <summary>
        /// Backing store for the MaterialButton.SelectedFontColor bindable property.
        /// </summary>
        public static readonly BindableProperty SelectedFontColorProperty = BindableProperty.Create("SelectedFontColor", typeof(Color), typeof(MaterialButton), Color.Default);
        /// <summary>
        /// Gets or sets the color of the font when selected.
        /// </summary>
        /// <value>The color of the font.</value>
        public Color SelectedFontColor
        {
            get { return (Color)GetValue(SelectedFontColorProperty); }
            set { SetValue(SelectedFontColorProperty, value); }
        }

        /// <summary>
        /// Backing store for the MaterialButton.FontAttributes bindable property.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create("FontAttributes", typeof(FontAttributes), typeof(MaterialButton), FontAttributes.None);//, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontAttributesPropertyChanged));
                                                                                                                                                                                        /// <summary>
                                                                                                                                                                                        /// Gets or sets the font attributes.
                                                                                                                                                                                        /// </summary>
                                                                                                                                                                                        /// <value>The font attributes.</value>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }


        /// <summary>
        /// Backing store for the MaterialButton.FontSize bindable property.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create("FontSize", typeof(double), typeof(MaterialButton), -1.0);//, BindingMode.OneWay), null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontSizePropertyChanged));
                                                                                                                                                     /// <summary>
                                                                                                                                                     /// Gets or sets the size of the font.
                                                                                                                                                     /// </summary>
                                                                                                                                                     /// <value>The size of the font.</value>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Backing store for the MaterialButton.FontFamiily bindable property.
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create("FontFamily", typeof(string), typeof(MaterialButton), null);//, BindingMode.OneWay), null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontFamilyPropertyChanged)); 
                                                                                                                                                         /// <summary>
                                                                                                                                                         /// Gets or sets the font family.
                                                                                                                                                         /// </summary>
                                                                                                                                                         /// <value>The font family.</value>
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Backing store for the OutlineWidth bindable property.
        /// </summary>
        public static new readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("OutlineWidth", typeof(float), typeof(MaterialButton), -1.0f);
        /// <summary>
        /// Gets or sets the width of the outline.
        /// </summary>
        /// <value>The width of the outline.</value>
        public new float OutlineWidth
        {
            get { return (float)GetValue(OutlineWidthProperty); }
            set { SetValue(OutlineWidthProperty, value); }
        }


        /// <summary>
        /// Backing store for the MaterialButton.OutlineColor bindable property.
        /// </summary>
        public static new readonly BindableProperty OutlineColorProperty = BindableProperty.Create("OutlineColor", typeof(Color), typeof(MaterialButton), Color.Default);
        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        /// <value>The color of the border.</value>
        public new Color OutlineColor
        {
            get { return (Color)GetValue(OutlineColorProperty); }
            set { SetValue(OutlineColorProperty, value); }
        }

        /// <summary>
        /// Backing store for the MaterialButton.BackgroundColor bindable property.
        /// </summary>
        public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof(Color), typeof(MaterialButton), Color.Transparent);
        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        /// <summary>
        /// Backing store for the MaterialButton.SelectedBackgroundColor bindable property.
        /// </summary>
        public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create("SelectedBackgroundColor", typeof(Color), typeof(MaterialButton), Color.Transparent);
        /// <summary>
        /// Gets or sets the background color used when selected.
        /// </summary>
        /// <value>The selected background.</value>
        public Color SelectedBackgroundColor
        {
            get { return (Color)GetValue(SelectedBackgroundColorProperty); }
            set { SetValue(SelectedBackgroundColorProperty, value); }
        }

        /// <summary>
        /// Backing store for the MaterialButton.DarkTheme property.
        /// </summary>
        public static readonly BindableProperty DarkThemeProperty = BindableProperty.Create("DarkTheme", typeof(bool), typeof(MaterialButton), false);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MaterialButton"/> if for a dark theme.
        /// </summary>
        /// <value><c>true</c> if dark theme; otherwise, <c>false</c>.</value>
        public bool DarkTheme
        {
            get { return (bool)GetValue(DarkThemeProperty); }
            set { SetValue(DarkThemeProperty, value); }
        }

        /// <summary>
        /// Backing store for the MaterialButton.Command bindable property.
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(MaterialButton), null, BindingMode.OneWay, null,
            new BindableProperty.BindingPropertyChangedDelegate((bo, o, n) =>
                ((MaterialButton)bo).OnCommandChanged()),
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
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        /// <summary>
        /// Backing store for the MaterialButton.CommandParameter bindable property.
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(MaterialButton), null, BindingMode.OneWay, null,
            new BindableProperty.BindingPropertyChangedDelegate((bo, o, n) =>
                ((MaterialButton)bo).CommandCanExecuteChanged(bo, EventArgs.Empty)),
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
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Backing store for the MaterialButton.IsSelected bindable property.
        /// </summary>
        public static BindableProperty IsSelectedProperty = BindableProperty.Create("IsSelected", typeof(bool), typeof(MaterialButton), false, BindingMode.TwoWay);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Button"/> is selected.
        /// </summary>
        /// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
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
        public static BindableProperty ToggleBehaviorProperty = BindableProperty.Create("ToggleBehavior", typeof(bool), typeof(MaterialButton), false);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Button"/> will stay selected or unselected after a tap.
        /// </summary>
        /// <value><c>true</c> if togglable; otherwise, <c>false</c>.</value>
        public bool ToggleBehavior
        {
            get { return (bool)GetValue(ToggleBehaviorProperty); }
            set
            {
                SetValue(ToggleBehaviorProperty, value);
            }
        }

        /// <summary>
        /// Backing store for the MaterialButton.Alignment bindable property
        /// </summary>
        public static BindableProperty AlignmentProperty = BindableProperty.Create("Justificaiton", typeof(TextAlignment), typeof(MaterialButton), TextAlignment.Center);
        /// <summary>
        /// Gets or sets the alignment of the image and text.
        /// </summary>
        /// <value>The alignment (left, center, right).</value>
        [Obsolete("Alignment is obsolete, see HorizontalTextAlignment and VerticalTextAlignment")]
        public TextAlignment Alignment
        {
            get { return (TextAlignment)GetValue(AlignmentProperty); }
            set { SetValue(AlignmentProperty, value); }
        }

        /// <summary>
        /// Backing store for the MaterialButton.Alignment bindable property
        /// </summary>
        public static BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create("HorizontalTextAlignment", typeof(TextAlignment), typeof(MaterialButton), TextAlignment.Center);
        /// <summary>
        /// Gets or sets the alignment of the image and text.
        /// </summary>
        /// <value>The alignment (left, center, right).</value>
        public TextAlignment HorizontalTextAlignment
        {
            get { return (TextAlignment)GetValue(HorizontalTextAlignmentProperty); }
            set { SetValue(HorizontalTextAlignmentProperty, value); }
        }

        /// <summary>
        /// Backing store for the vertical alignment property.
        /// </summary>
        public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create("VerticalTextAlignment", typeof(TextAlignment), typeof(MaterialButton), TextAlignment.Center);
        /// <summary>
        /// Gets or sets the vertical alignment.
        /// </summary>
        /// <value>The vertical alignment.</value>
        public TextAlignment VerticalTextAlignment
        {
            get { return (TextAlignment)GetValue(VerticalTextAlignmentProperty); }
            set { SetValue(VerticalTextAlignmentProperty, value); }
        }



        /// <summary>
        /// Backing store for the MaterialButton's orientation property.
        /// </summary>
        public static BindableProperty OrientationProperty = BindableProperty.Create("Orientation", typeof(StackOrientation), typeof(MaterialButton), StackOrientation.Horizontal);
        /// <summary>
        /// Gets or sets the orientation of the iamge and label.
        /// </summary>
        /// <value>The image and label orientation.</value>
        public StackOrientation Orientation
        {
            get { return (StackOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
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
        public static readonly BindableProperty FitProperty = BindableProperty.Create("Fit", typeof(LabelFit), typeof(MaterialButton), LabelFit.Width);
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
        public static readonly BindableProperty HapticModeProperty = BindableProperty.Create("HapticMode", typeof(KeyClicks), typeof(MaterialButton), default(KeyClicks));
        /// <summary>
        /// Gets or sets the haptic mode.
        /// </summary>
        /// <value>The haptic mode.</value>
        public KeyClicks HapticMode
        {
            get { return (KeyClicks)GetValue(HapticModeProperty); }
            set { SetValue(HapticModeProperty, value); }
        }

        /// <summary>
        /// The tint image property backing store.
        /// </summary>
        public static readonly BindableProperty TintImageProperty = BindableProperty.Create("TintImage", typeof(bool), typeof(MaterialButton), true);
        /// <summary>
        /// Will the FontColor be applied to the ImageSource image?
        /// </summary>
        /// <value><c>true</c> tint ImageSource image with FontColor; otherwise, <c>false</c>.</value>
        public bool TintImage
        {
            get { return (bool)GetValue(TintImageProperty); }
            set { SetValue(TintImageProperty, value); }
        }

        /// <summary>
        /// The has tight spacing property.
        /// </summary>
        public static readonly BindableProperty HasTightSpacingProperty = BindableProperty.Create("HasTightSpacing", typeof(bool), typeof(MaterialButton), default(bool));
        /// <summary>
        /// Gets or sets if the Icon/Image is close (TightSpacing) to text or at edge (not TightSpacing) of button.
        /// </summary>
        /// <value><c>true</c> if has tight spacing; otherwise, <c>false</c>.</value>
        public bool HasTightSpacing
        {
            get { return (bool)GetValue(HasTightSpacingProperty); }
            set { SetValue(HasTightSpacingProperty, value); }
        }

        /// <summary>
        /// Backing store for the spacing property.
        /// </summary>
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create("Spacing", typeof(double), typeof(MaterialButton), 4.0);
        /// <summary>
        /// Gets or sets the spacing.
        /// </summary>
        /// <value>The spacing.</value>
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        #endregion


        #region SegmentedButton Properties
        /// <summary>
        /// 
        /// </summary>
        internal static BindableProperty SegmentTypeProperty = BindableProperty.Create("SegmentType", typeof(SegmentType), typeof(MaterialButton), SegmentType.Not);
        internal SegmentType SegmentType
        {
            get { return (SegmentType)GetValue(SegmentTypeProperty); }
            set { SetValue(SegmentTypeProperty, value); }
        }

        internal static BindableProperty ParentSegmentsOrientationProperty = BindableProperty.Create("ParentSegmentsOrientation", typeof(StackOrientation), typeof(MaterialButton), StackOrientation.Horizontal);
        internal StackOrientation ParentSegmentsOrientation
        {
            get { return (StackOrientation)GetValue(ParentSegmentsOrientationProperty); }
            set { SetValue(ParentSegmentsOrientationProperty, value); }
        }

        internal static BindableProperty SeparatorWidthProperty = BindableProperty.Create("SeparatorWidth", typeof(float), typeof(MaterialButton), -1f);
        internal float SeparatorWidth
        {
            get { return (float)GetValue(SeparatorWidthProperty); }
            set { SetValue(SeparatorWidthProperty, value); }
        }

        internal static BindableProperty GroupToggleBehaviorProperty = BindableProperty.Create("GroupToggleBehavior", typeof(GroupToggleBehavior), typeof(MaterialButton), GroupToggleBehavior.None);
        internal GroupToggleBehavior GroupToggleBehavior
        {
            get { return (GroupToggleBehavior)GetValue(GroupToggleBehaviorProperty); }
            set
            {
                SetValue(GroupToggleBehaviorProperty, value);
            }
        }
        #endregion


        #region Fields
        bool _noUpdate = true;
        /// <summary>
        /// The stack layout.
        /// </summary>
        internal protected Xamarin.Forms.StackLayout _stackLayout;
        /// <summary>
        /// The image.
        /// </summary>
        internal protected Image _image;
        /// <summary>
        /// The label.
        /// </summary>
        internal protected Label _label;
        Label _iconLabel;
        /// <summary>
        /// The gesture listener.
        /// </summary>
        internal protected FormsGestures.Listener _gestureListener;
        #endregion


        #region Constructor
        /// <summary>
        /// The constructing.
        /// </summary>
        internal protected bool _constructing;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialButton"/> class.
        /// </summary>
        public MaterialButton()
        {
            _constructing = true;
            Padding = new Thickness(8, 6, 8, 6);
            OutlineRadius = 2;
            _label = new Label
            {
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                //HeightRequest = 22,
                MinimizeHeight = false,
                Lines = Lines,
                Fit = Fit,
                LineBreakMode = LineBreakMode,
                MinFontSize = MinFontSize,
                FontSize = FontSize,
                FontAttributes = FontAttributes,
                FontFamily = FontFamily,
                Margin = 0,
            };
            _label.PropertyChanged += OnLabelPropertyChanged;

            _stackLayout = new Xamarin.Forms.StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = Spacing,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Padding = 0,
                Margin = 0,
            };

            base.Content = _stackLayout;
            _noUpdate = false;

            _gestureListener = FormsGestures.Listener.For(this);
            _gestureListener.Tapped += OnTapped;
            _gestureListener.LongPressed += OnLongPressed;
            _gestureListener.LongPressing += OnLongPressing;


            UpdateElements();

            _constructing = false;
        }
        #endregion


        #region IDisposable Support
        /// <summary>
        /// The disposed value.
        /// </summary>
        internal protected bool disposedValue; // To detect redundant calls

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
                _tapped = null;
                _selected = null;
                _longPressed = null;
                _longPressing = null;
            }
        }

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
            Dispose(true);
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
                KeyClicksService.Feedback(HapticEffect, HapticMode);

                //Debug.WriteLine("tapped");
                if (!(this is ImageButton))
                {
                    if (ToggleBehavior && GroupToggleBehavior == GroupToggleBehavior.None
                        || GroupToggleBehavior == GroupToggleBehavior.Multiselect
                        || GroupToggleBehavior == GroupToggleBehavior.Radio && !IsSelected)
                        IsSelected = !IsSelected;
                    else
                    {
                        Opacity = 0.5;
                        Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                        {
                            Opacity += 0.1;
                            return Opacity < 1.0;
                        });
                    }
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
        public void UpdateElements()
        {
            _noUpdate = true;
            base.OutlineWidth = OutlineWidth < 0 ? (BackgroundColor.A > 0 ? 0 : 1) : OutlineColor == Color.Transparent ? 0 : OutlineWidth;
            if (IsEnabled)
            {
                base.HasShadow = BackgroundColor.A > 0 && HasShadow;
                ShadowInverted = IsSelected && SegmentType == SegmentType.Not;

                if (IsSelected && SelectedFontColor.A > 0)
                    _label.TextColor = SelectedFontColor;
                else
                    _label.TextColor = FontColor == Color.Default ? (DarkTheme ? Color.White : Color.FromHex("#000").WithAlpha(0.5)) : FontColor;

                if (IsSelected)
                {
                    if (SelectedBackgroundColor.A > 0)
                        base.BackgroundColor = SelectedBackgroundColor;
                    else
                        base.BackgroundColor = BackgroundColor.A > 0 ? BackgroundColor.RgbBlend(Color.FromHex("#000"), 0.25) : Color.FromHex(DarkTheme ? "#FFF" : "#000").WithAlpha(0.2);
                    base.OutlineColor = BackgroundColor.A > 0 ? OutlineColor.RgbBlend(Color.FromHex("#000"), 0.25) : base.BackgroundColor.A > 0 ? base.BackgroundColor : _label.TextColor;
                }
                else
                {
                    base.BackgroundColor = BackgroundColor.A > 0 ? BackgroundColor : _label.BackgroundColor;
                    base.OutlineColor = OutlineColor;
                }
                if (OutlineColor == Color.Default)
                    //base.OutlineColor = BackgroundColor.A > 0 ? base.BackgroundColor : Color.FromHex (DarkTheme? "#FFF" : "#000").WithAlpha (0.5);
                    base.OutlineColor = _label.TextColor;//Color.FromHex (DarkTheme? "#FFF" : "#000").WithAlpha (0.5);
            }
            else
            {
                Color opaque, transp;
                if (IsSelected)
                {
                    if (DarkTheme)
                    {
                        opaque = Color.FromHex("#FFF").WithAlpha(0.2);
                        transp = Color.FromHex("#FFF").WithAlpha(0.1);
                        _label.TextColor = Color.FromHex("#FFF").WithAlpha(0.30);
                    }
                    else
                    {
                        opaque = Color.FromHex("#000").WithAlpha(0.2);
                        transp = Color.FromHex("#000").WithAlpha(0.1);
                        _label.TextColor = Color.FromHex("#000").WithAlpha(0.26);
                    }
                }
                else
                {
                    transp = Color.Transparent;
                    if (DarkTheme)
                    {
                        opaque = Color.FromHex("#FFF").WithAlpha(0.1);
                        _label.TextColor = Color.FromHex("#FFF").WithAlpha(0.30);
                    }
                    else
                    {
                        opaque = Color.FromHex("#000").WithAlpha(0.1);
                        _label.TextColor = Color.FromHex("#000").WithAlpha(0.26);
                    }
                }
                if (SegmentType == SegmentType.Not)
                {
                    base.HasShadow = false;
                    base.BackgroundColor = BackgroundColor.A > 0 ? opaque : transp;
                    //base.OutlineColor = OutlineColor.A > 0 ? opaque : base.BackgroundColor;
                    base.OutlineColor = OutlineColor == Color.Default || OutlineColor.A > 0 ? opaque : BackgroundColor;
                }
                else
                {
                    base.OutlineWidth = OutlineWidth < 0 ? (BackgroundColor.A > 0 ? 0 : 1) : OutlineColor == Color.Transparent ? 0 : OutlineWidth;
                    base.HasShadow = BackgroundColor.A > 0 && HasShadow;
                    if (IsSelected)
                    {
                        if (SelectedBackgroundColor.A > 0)
                            base.BackgroundColor = SelectedBackgroundColor.RgbBlend(Color.FromHex("#000"), 0.25);
                        else
                            base.BackgroundColor = BackgroundColor.A > 0 ? BackgroundColor.RgbBlend(Color.FromHex("#000"), 0.25) : Color.FromHex(DarkTheme ? "#FFF" : "#000").WithAlpha(0.2);
                        base.OutlineColor = BackgroundColor.A > 0 ? OutlineColor.RgbBlend(Color.FromHex("#000"), 0.25) : base.BackgroundColor.A > 0 ? base.BackgroundColor : _label.TextColor;
                    }
                    else
                    {
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
            UpdateIconTint();
            _noUpdate = false;
        }

        public void UpdateIconTint()
        {
            if (_image != null)
                _image.TintColor = TintImage ? _label.TextColor : Color.Default;
            if (_iconLabel != null)
            {
                _iconLabel.TextColor = _label.TextColor;
                _iconLabel.FontSize = _label.FontSize;
            }
        }
        #endregion


        #region CurrentProperties

        bool IsEnabledCore
        {
            set
            {
                //this.SetValueCore(VisualElement.IsEnabledProperty, (object) (bool) (value ? true : false), Xamarin.Forms.BindableObject.SetValueFlags.None);
                SetValue(IsEnabledProperty, value);
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
        public event EventHandler Tapped
        {
            add { _tapped += value; }
            remove { _tapped -= value; }
        }
        /// <summary>
        /// Invokes the tapped.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        internal protected void InvokeTapped(object sender, EventArgs args) => _tapped?.Invoke(sender, args);


        event EventHandler _selected;
        /// <summary>
        /// Occurs when transitioned to IsSelected=true.
        /// </summary>
        public event EventHandler Selected
        {
            add { _selected += value; }
            remove { _selected -= value; }
        }
        /// <summary>
        /// Invokes the selected.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        internal protected void InvokeSelected(object sender, EventArgs args) => _selected?.Invoke(sender, args);

        event EventHandler _longPressing;
        /// <summary>
        /// Occurs when button is actively being long pressed
        /// </summary>
        public event EventHandler LongPressing
        {
            add { _longPressing += value; }
            remove { _longPressing -= value; }
        }
        /// <summary>
        /// Invokes the long pressing.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        internal protected void InvokeLongPressing(object sender, EventArgs args) => _longPressing?.Invoke(sender, args);

        event EventHandler _longPressed;
        /// <summary>
        /// Occurs when long pressed has completed.
        /// </summary>
        public event EventHandler LongPressed
        {
            add { _longPressed += value; }
            remove { _longPressed -= value; }
        }
        /// <summary>
        /// Invokes the long pressed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        internal protected void InvokeLongPressed(object sender, EventArgs args) => _longPressed?.Invoke(sender, args);

        #endregion


        #region Change Handlers

        void OnLabelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            if (propertyName == Label.TextProperty.PropertyName || propertyName == Label.HtmlTextProperty.PropertyName)
            {
                if (string.IsNullOrEmpty((string)_label) && _stackLayout.Children.Contains(_label))
                    _stackLayout.Children.Remove(_label);
                else if (!string.IsNullOrEmpty((string)_label) && !_stackLayout.Children.Contains(_label))
                {
                    if (TrailingImage)
                        _stackLayout.Children.Insert(0, _label);
                    else
                        _stackLayout.Children.Add(_label);
                }
                SetOrienations();
            } 
            else if (propertyName == Label.TextColorProperty.PropertyName)
            {
                UpdateIconTint();
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


        void SetStackLayoutPadding()
        {
            var padding = OutlineWidth / Display.Scale;
            if (SegmentType == SegmentType.Not || SegmentType == SegmentType.End)
                _stackLayout.Padding = padding;
            else
            {
                if (ParentSegmentsOrientation == StackOrientation.Horizontal)
                    _stackLayout.Padding = new Thickness(padding, padding, 0, padding);
                else
                    _stackLayout.Padding = new Thickness(padding, padding, padding, 0);

            }
            //_stackLayout.BackgroundColor = Color.Pink;
        }

        void SetOrienations()
        {
            var horzOption = HorizontalTextAlignment.ToLayoutOptions();
            var vertOption = VerticalTextAlignment.ToLayoutOptions();
            if (Orientation == StackOrientation.Horizontal)
            {
                if (HasTightSpacing)
                {
                    if (_image != null)
                    {
                        _image.HorizontalOptions = horzOption;
                        _image.VerticalOptions = vertOption;
                    }
                    if (_iconLabel != null)
                    {
                        _iconLabel.HorizontalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalOptions = horzOption;
                        _iconLabel.VerticalTextAlignment = VerticalTextAlignment;
                        _iconLabel.VerticalOptions = LayoutOptions.FillAndExpand;
                    }
                    if (_label != null)
                    {
                        _label.HorizontalTextAlignment = HorizontalTextAlignment;
                        _label.HorizontalOptions = horzOption;
                        _label.VerticalTextAlignment = VerticalTextAlignment;
                        _label.VerticalOptions = LayoutOptions.FillAndExpand;
                        _label.MinimizeHeight = false;
                    }
                    _stackLayout.HorizontalOptions = horzOption;
                }
                else
                {
                    if (_image != null)
                    {
                        _image.HorizontalOptions = (TrailingImage ? LayoutOptions.End : LayoutOptions.Start);
                        _image.VerticalOptions = vertOption;
                    }
                    if (_iconLabel != null)
                    {
                        _iconLabel.HorizontalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalOptions = (TrailingImage ? LayoutOptions.End : LayoutOptions.Start);
                        _iconLabel.VerticalTextAlignment = VerticalTextAlignment;
                        _iconLabel.VerticalOptions = LayoutOptions.FillAndExpand;
                    }
                    if (_label != null)
                    {
                        _label.HorizontalTextAlignment = HorizontalTextAlignment;
                        _label.HorizontalOptions = LayoutOptions.FillAndExpand;
                        _label.VerticalTextAlignment = VerticalTextAlignment;
                        _label.VerticalOptions = LayoutOptions.FillAndExpand;
                        _label.MinimizeHeight = false;
                    }
                    _stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                }
                _stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            }
            else
            {
                if (HasTightSpacing)
                {
                    if (_image != null)
                    {
                        _image.HorizontalOptions = horzOption;// LayoutOptions.CenterAndExpand;
                        _image.VerticalOptions = LayoutOptions.Center;
                    }
                    if (_iconLabel != null)
                    {
                        _iconLabel.VerticalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalOptions = horzOption; // LayoutOptions.CenterAndExpand;
                        _iconLabel.VerticalOptions = LayoutOptions.Center;
                    }
                    if (_label != null)
                    {
                        _label.VerticalTextAlignment = VerticalTextAlignment; //TextAlignment.Center;
                        _label.HorizontalTextAlignment = TextAlignment.Center;
                        _label.HorizontalOptions = horzOption; // LayoutOptions.CenterAndExpand;
                                                               //_label.VerticalOptions = HasTightSpacing ? LayoutOptions.Center : LayoutOptions.CenterAndExpand;
                        _label.VerticalOptions = LayoutOptions.Center;
                        _label.MinimizeHeight = true;
                    }
                    //_stackLayout.Spacing = 0;// _label.FontSize< 0 ? -6 : -_label.FontSize/2.0;
                    _stackLayout.HorizontalOptions = LayoutOptions.Fill;
                    _stackLayout.VerticalOptions = VerticalTextAlignment.ToLayoutOptions(true);
                }
                else
                {
                    if (_image != null)
                    {
                        _image.HorizontalOptions = horzOption;// LayoutOptions.CenterAndExpand;
                        _image.VerticalOptions = (TrailingImage ? LayoutOptions.End : LayoutOptions.Start);
                    }
                    if (_iconLabel != null)
                    {
                        _iconLabel.VerticalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalOptions = horzOption; // LayoutOptions.CenterAndExpand;
                        _iconLabel.VerticalOptions = (TrailingImage ? LayoutOptions.End : LayoutOptions.Start);
                    }
                    if (_label != null)
                    {
                        _label.VerticalTextAlignment = HorizontalTextAlignment; //TextAlignment.Center;
                        _label.HorizontalTextAlignment = TextAlignment.Center;
                        _label.HorizontalOptions = horzOption; // LayoutOptions.CenterAndExpand;
                                                               //_label.VerticalOptions = HasTightSpacing ? LayoutOptions.Center : LayoutOptions.CenterAndExpand;
                        _label.VerticalOptions = VerticalTextAlignment.ToLayoutOptions(true);
                        _label.MinimizeHeight = true;
                    }
                    //_stackLayout.Spacing = 0;// _label.FontSize< 0 ? -6 : -_label.FontSize/2.0;
                    _stackLayout.HorizontalOptions = LayoutOptions.Fill;
                    _stackLayout.VerticalOptions = LayoutOptions.Fill;
                }
            }
            _stackLayout.Orientation = Orientation;
        }



        // WORKING ON:  NEED TO ADD VERTICAL ALIGNMENT PROPERTY!!!!

        bool _changingIconImageSource;
        bool _changingIconText;


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

            if (propertyName == OutlineWidthProperty.PropertyName
                || propertyName == ParentSegmentsOrientationProperty.PropertyName
                || propertyName == SegmentTypeProperty.PropertyName)
                SetStackLayoutPadding();
            if (propertyName == AlignmentProperty.PropertyName)
#pragma warning disable CS0618 // Type or member is obsolete
                HorizontalTextAlignment = Alignment;
#pragma warning restore CS0618 // Type or member is obsolete
            if (propertyName == HorizontalTextAlignmentProperty.PropertyName || propertyName == VerticalTextAlignmentProperty.PropertyName)
            {
                //_stackLayout.HorizontalOptions = Alignment.ToLayoutOptions();
                SetOrienations();
            }
            else if (propertyName == ImageSourceProperty.PropertyName)
            {
                if (_changingIconText)
                    return;
                _changingIconImageSource = true;
                if (_image != null)
                    _stackLayout.Children.Remove(_image);
                if (ImageSource != null)
                {
                    if (_iconLabel != null)
                    {
                        _stackLayout.Children.Remove(_iconLabel);
                        _iconLabel.HtmlText = null;
                        _iconLabel.Text = null;
                        IconText = null;
                    }
                    _image = new Image { Source = ImageSource };
                    _image.Fill = Fill.AspectFit;
                    //_image.TintColor = TintImage ? _label.TextColor : Color.Default;
                    UpdateIconTint();
                    if (_image != null)
                    {
                        if (TrailingImage)
                            _stackLayout.Children.Add(_image);
                        else
                            _stackLayout.Children.Insert(0, _image);
                        SetOrienations();
                    }
                }
                _changingIconImageSource = false;
            }
            else if (propertyName == IconTextProperty.PropertyName)
            {
                if (_changingIconImageSource)
                    return;
                _changingIconText = true;
                if (_iconLabel != null)
                    _stackLayout.Children.Remove(_iconLabel);
                if (IconText != null)
                {
                    if (_image != null)
                        _stackLayout.Children.Remove(_image);
                    _iconLabel = new Label
                    {
                        HtmlText = IconText,
                        TextColor = _label.TextColor,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        Lines = 1,
                        FontSize = FontSize,
                        Fit = LabelFit.None,
                    };
                    if (_iconLabel != null)
                    {
                        if (TrailingImage)
                            _stackLayout.Children.Add(_iconLabel);
                        else
                            _stackLayout.Children.Insert(0, _iconLabel);
                        SetOrienations();
                    }
                    UpdateIconTint();
                }
                _changingIconText = false;
            }
            else if (propertyName == BackgroundColorProperty.PropertyName
                       || propertyName == SelectedBackgroundColorProperty.PropertyName
                       || propertyName == OutlineColorProperty.PropertyName
                       || propertyName == OutlineWidthProperty.PropertyName
                       || propertyName == IsSelectedProperty.PropertyName
                       || propertyName == IsEnabledProperty.PropertyName
                       || propertyName == DarkThemeProperty.PropertyName
                       || propertyName == HasShadowProperty.PropertyName
                       || propertyName == SegmentTypeProperty.PropertyName)
            {
                UpdateElements();
            }
            else if (propertyName == OrientationProperty.PropertyName)
                SetOrienations();
            else if (propertyName == FontColorProperty.PropertyName && !IsSelected)
            {
                UpdateElements();
                UpdateIconTint();
            }
            else if (propertyName == SelectedFontColorProperty.PropertyName && IsSelected)
            {
                UpdateElements();
                UpdateIconTint();
            }
            else if (propertyName == TintImageProperty.PropertyName && _image != null)
                UpdateElements();
            else if (propertyName == HasTightSpacingProperty.PropertyName)
                SetOrienations();
            else if (propertyName == TextProperty.PropertyName)
                _label.Text = Text;
            else if (propertyName == HtmlTextProperty.PropertyName)
                _label.HtmlText = HtmlText;

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
                SetOrienations();

                // following 2 lines are required to force an update.  ForceLayout() causes label to not be sized correctly
                HasTightSpacing = !HasTightSpacing;
                HasTightSpacing = !HasTightSpacing;
            }
            else if (propertyName == SpacingProperty.PropertyName)
                _stackLayout.Spacing = Spacing;
            else if (propertyName == TintImageProperty.PropertyName)
                UpdateIconTint();
            
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

        /// <summary>
        /// Sends the tapped.
        /// </summary>
        internal protected void SendTapped()
        {
            ICommand command = Command;
            if (command != null && GroupToggleBehavior == GroupToggleBehavior.None)
                command.Execute(CommandParameter);
            _tapped?.Invoke(this, EventArgs.Empty);
        }

        #endregion


    }
}


