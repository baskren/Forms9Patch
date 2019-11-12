using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.ComponentModel;
using System.Reflection;

namespace Forms9Patch
{
    /// <summary>
    /// DEPRICATED: Use Button
    /// </summary>
    [Obsolete("Depricated: Use Forms9Patch.Button", true)]
    public class MaterialButton : Forms9Patch.Button { }

    /// <summary>
    /// Forms9Patch Button.
    /// </summary>
    [DesignTimeVisible(true)]
    public class Button : Frame, IButton
    {


        #region Obsolete Properties
        /// <summary>
        /// OBSOLETE: Use SelectedTextColorProperty
        /// </summary>
        [Obsolete("Use SelectedTextColorProperty", true)]
        public static readonly BindableProperty SelectedFontColorProperty = BindableProperty.Create(nameof(SelectedFontColor), typeof(Color), typeof(Button), Color.Default, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Button button && newValue is Xamarin.Forms.Color color)
                button.SelectedTextColor = color;
        });
        /// <summary>
        /// OBSOLETE: Use SelectedTextColor
        /// </summary>
        /// <value>The color of the font.</value>
        [Obsolete("Use SelectedTextColor", true)]
        public Color SelectedFontColor
        {
            get => (Color)GetValue(SelectedFontColorProperty);
            set => SetValue(SelectedFontColorProperty, value);
        }

        /// <summary>
        /// OBSOLETE: Use ToggleBehaviorProperty instead.
        /// </summary>
        [Obsolete("StickyBehavior property is obsolete, use ToggleBehavior instead", true)]
        public static BindableProperty StickyBehaviorProperty;

        /// <summary>
        /// OBSOLETE: Use ToggleBehavior instead.
        /// </summary>
        [Obsolete("StickyBehavior property is obsolete, use ToggleBehavior instead", true)]
        public bool StickyBehavior
        {
            get { throw new NotSupportedException("StickyBehavior property is obsolete, use ToggleBehavior instead"); }
            set { throw new NotSupportedException("StickyBehavior property is obsolete, use ToggleBehavior instead"); }
        }

        /// <summary>
        /// OBSOLETE: Use IconImageProperty
        /// </summary>
        [Obsolete("Use IconImageProperty", true)]
        public static BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(Xamarin.Forms.ImageSource), typeof(Button), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Button button && newValue is Xamarin.Forms.ImageSource source)
                button.IconImage = new Forms9Patch.Image(source);
        });
        /// <summary>
        /// OBSOLETE: Use IconImage.
        /// </summary>
        /// <value>The image.</value>
        [Obsolete("Use IconImage", true)]
        public Xamarin.Forms.ImageSource ImageSource
        {
            get => (Xamarin.Forms.ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        /// <summary>
        /// OBSOLETE: Use TrailingIconProperty
        /// </summary>
        [Obsolete("Use TrailingIconProperty", true)]
        public static readonly BindableProperty TrailingImageProperty = BindableProperty.Create(nameof(TrailingImage), typeof(bool), typeof(Button), default(bool), propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Button button && newValue is bool value)
                button.TrailingIcon = value;
        });
        /// <summary>
        /// OBSOLETE: Use TrailingIcon
        /// </summary>
        [Obsolete("Use TrailingIcon", true)]
        public bool TrailingImage
        {
            get => (bool)GetValue(TrailingImageProperty);
            set => SetValue(TrailingImageProperty, value);
        }

        /// <summary>
        /// OBSOLETE: Used TintIconProperty
        /// </summary>
        [Obsolete("Use TintIconProperty", true)]
        public static readonly BindableProperty TintImageProperty = BindableProperty.Create(nameof(TintImage), typeof(bool), typeof(Button), true, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Button button && newValue is bool value)
                button.TintIcon = value;
        });
        /// <summary>
        /// OBSOLETE: Use TintIcon property
        /// </summary>
        [Obsolete("Use TintIcon", true)]
        public bool TintImage
        {
            get => (bool)GetValue(TintImageProperty);
            set => SetValue(TintImageProperty, value);
        }

        #region IsElliptical property
        /// <summary>
        /// backing store for IsEliptical property
        /// </summary>
        [Obsolete("Use ElementShapeProperty instead.", true)]
        public static readonly BindableProperty IsEllipticalProperty = BindableProperty.Create(nameof(IsElliptical), typeof(bool), typeof(Button), default(bool), propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Button button && newValue is bool value)
                button.ElementShape = value ? ElementShape.Elliptical : ElementShape.Rectangle;
        });
        /// <summary>
        /// Gets/Sets the IsEliptical property
        /// </summary>
        [Obsolete("Use ElementShape property instead.", true)]
        public bool IsElliptical
        {
            get { return (bool)GetValue(IsEllipticalProperty); }
            set { SetValue(IsEllipticalProperty, value); }
        }
        #endregion IsElliptical property

        /// <summary>
        /// OBSOLETE: Use TextColorProperty
        /// </summary>
        [Obsolete("Use TextColorProperty", true)]
        public static readonly BindableProperty FontColorProperty = BindableProperty.Create(nameof(FontColor), typeof(Color), typeof(Button), Color.Default, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Button button && newValue is Xamarin.Forms.Color color)
                button.TextColor = color;
        });
        /// <summary>
        /// OBSOLETE: Use TextColor
        /// </summary>
        /// <value>The color of the font.</value>
        [Obsolete("Use TextColor", true)]
        public Color FontColor
        {
            get => (Color)GetValue(FontColorProperty);
            set => SetValue(FontColorProperty, value);
        }

        /// <summary>
        /// Backing store for the Button.Alignment bindable property
        /// </summary>
        public static BindableProperty AlignmentProperty = BindableProperty.Create("Justificaiton", typeof(TextAlignment), typeof(Button), TextAlignment.Center, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Button button && newValue is TextAlignment alignment)
                button.HorizontalTextAlignment = alignment;
        });
        /// <summary>
        /// Gets or sets the alignment of the image and text.
        /// </summary>
        /// <value>The alignment (left, center, right).</value>
        [Obsolete("Alignment is obsolete, see HorizontalTextAlignment and VerticalTextAlignment", true)]
        public TextAlignment Alignment
        {
            get => (TextAlignment)GetValue(AlignmentProperty);
            set => SetValue(AlignmentProperty, value);
        }

        /// <summary>
        /// backing store for Fit property
        /// </summary>
        [Obsolete("FitProperty is obsolete.  Use AutoFitProperty instead.", true)]
        public static readonly BindableProperty FitProperty = BindableProperty.Create(nameof(Fit), typeof(LabelFit), typeof(Button), LabelFit.None, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Label label && newValue is LabelFit fit)
            {
                switch (fit)
                {
                    case LabelFit.None: label.AutoFit = AutoFit.None; break;
                    case LabelFit.Width: label.AutoFit = AutoFit.Width; break;
                    case LabelFit.Lines: label.AutoFit = AutoFit.Lines; break;
                    default:
                        break;
                }
            }
        });
        /// <summary>
        /// Gets/Sets the Fit property
        /// </summary>
        [Obsolete("Fit property is obsolete.  Use AutoFit property instead.", true)]
        public LabelFit Fit
        {
            get => (LabelFit)GetValue(FitProperty);
            set => SetValue(FitProperty, value);
        }

        /// <summary>
        /// Do not use
        /// </summary>
        [Obsolete("Content property is not supported", true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1061:Do not hide base class methods", Justification = "We really don't want devs to have access to base class Content_set!")]
        public new VisualElement Content
        {
            get => throw new NotSupportedException("Forms9Patch.Button: Content is not a supported property.");
            set => throw new NotSupportedException("Forms9Patch.Button: Content is not a supported property.");
        }

        #endregion


        #region Properties

        #region IsLongPressEnabled property
        /// <summary>
        /// The is long press enabled property.
        /// </summary>
        public static readonly BindableProperty IsLongPressEnabledProperty = BindableProperty.Create(nameof(IsLongPressEnabled), typeof(bool), typeof(Button), true);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.Button"/> has long press enabled.
        /// </summary>
        /// <value><c>true</c> if is long press enabled; otherwise, <c>false</c>.</value>
        public bool IsLongPressEnabled
        {
            get => (bool)GetValue(IsLongPressEnabledProperty);
            set => SetValue(IsLongPressEnabledProperty, value);
        }
        #endregion IsLongPressEnabled property

        #region IsClipped property
        internal static readonly BindablePropertyKey IsClippedPropertyKey = BindableProperty.CreateReadOnly("Forms9Patch.Button.IsClipped", typeof(bool), typeof(Button), default(bool));
        /// <summary>
        /// Backing store for the IsClipped property.
        /// </summary>
        public static readonly BindableProperty IsClippedProperty = IsClippedPropertyKey.BindableProperty;
        /// <summary>
        /// Gets or sets a value indicating whether the contents of this <see cref="T:Forms9Patch.Button"/> is clipped.
        /// </summary>
        /// <value><c>true</c> if is clipped; otherwise, <c>false</c>.</value>
        public bool IsClipped
        {
            get => (bool)GetValue(IsClippedProperty);
            internal set => SetValue(IsClippedPropertyKey, value);
        }
        #endregion IsClipped property

        #region IButton

        #region SelectedTextColor
        /// <summary>
        /// Backing store for the SelectedTextColor property
        /// </summary>
        public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create(nameof(SelectedTextColor), typeof(Color), typeof(Button), Color.Default);
        /// <summary>
        /// Gets or sets the font color of the Text or the base font color of the HtmlText for when the button is selected
        /// </summary>
        public Color SelectedTextColor
        {
            get => (Color)GetValue(SelectedTextColorProperty);
            set => SetValue(SelectedTextColorProperty, value);
        }
        #endregion SelectedTextColor

        #region SelectedBackgroundColor
        /// <summary>
        /// Backing store for the Button.SelectedBackgroundColor bindable property.
        /// </summary>
        public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create(nameof(SelectedBackgroundColor), typeof(Color), typeof(Button), Color.Transparent);
        /// <summary>
        /// Gets or sets the background color used when selected.
        /// </summary>
        /// <value>The selected background.</value>
        public Color SelectedBackgroundColor
        {
            get => (Color)GetValue(SelectedBackgroundColorProperty);
            set => SetValue(SelectedBackgroundColorProperty, value);
        }

        #endregion SelectedBackgroundColor

        #region Command
        /// <summary>
        /// Backing store for the Button.Command bindable property.
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Button), null, BindingMode.OneWay, null,
            new BindableProperty.BindingPropertyChangedDelegate((bo, o, n) =>
                ((Button)bo).OnCommandChanged()),
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
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        #endregion Command

        #region CommandParameter
        /// <summary>
        /// Backing store for the Button.CommandParameter bindable property.
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(Button), null, BindingMode.OneWay, null,
            new BindableProperty.BindingPropertyChangedDelegate((bo, o, n) =>
                ((Button)bo).CommandCanExecuteChanged(bo, EventArgs.Empty)),
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
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        #endregion CommandParameter

        #region ToggleBehavior
        /// <summary>
        /// Backing store for the Button.ToggleBehavior bindable property.
        /// </summary>
        public static BindableProperty ToggleBehaviorProperty = BindableProperty.Create(nameof(ToggleBehavior), typeof(bool), typeof(Button), false);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Button"/> will stay selected or unselected after a tap.
        /// </summary>
        /// <value><c>true</c> if togglable; otherwise, <c>false</c>.</value>
        public bool ToggleBehavior
        {
            get => (bool)GetValue(ToggleBehaviorProperty);
            set => SetValue(ToggleBehaviorProperty, value);
        }
        #endregion ToggleBehavior

        // IsEnabled inherited

        #region IsSelected
        /// <summary>
        /// Backing store for the Button.IsSelected bindable property.
        /// </summary>
        public static BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(Button), false, BindingMode.TwoWay);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Button"/> is selected.
        /// </summary>
        /// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        #endregion IsSelected

        #region HapticEffect
        /// <summary>
        /// The haptic effect property.
        /// </summary>
        public static readonly BindableProperty HapticEffectProperty = BindableProperty.Create(nameof(HapticEffect), typeof(HapticEffect), typeof(Button), default(HapticEffect));
        /// <summary>
        /// Gets or sets the haptic effect.
        /// </summary>
        /// <value>The haptic effect.</value>
        public HapticEffect HapticEffect
        {
            get => (HapticEffect)GetValue(HapticEffectProperty);
            set => SetValue(HapticEffectProperty, value);
        }
        #endregion HapticEffect

        #region HapticEffectMode
        /// <summary>
        /// The haptic mode property.
        /// </summary>
        public static readonly BindableProperty HapticEffectModeProperty = BindableProperty.Create(nameof(HapticEffectMode), typeof(EffectMode), typeof(Button), default(EffectMode));
        /// <summary>
        /// Gets or sets the haptic mode.
        /// </summary>
        /// <value>The haptic mode.</value>
        public EffectMode HapticEffectMode
        {
            get => (EffectMode)GetValue(HapticEffectModeProperty);
            set => SetValue(HapticEffectModeProperty, value);
        }
        #endregion HapticEffectMode

        #region SoundEffect property
        /// <summary>
        /// The sound effect property backing store
        /// </summary>
        public static readonly BindableProperty SoundEffectProperty = BindableProperty.Create(nameof(SoundEffect), typeof(SoundEffect), typeof(Button), default(SoundEffect));
        /// <summary>
        /// Gets or sets the sound effect.
        /// </summary>
        /// <value>The sound effect.</value>
        public SoundEffect SoundEffect
        {
            get => (SoundEffect)GetValue(SoundEffectProperty);
            set => SetValue(SoundEffectProperty, value);
        }
        #endregion SoundEffect property

        #region SoundEffectMode property
        /// <summary>
        /// The backing store for the sound effect mode property.
        /// </summary>
        public static readonly BindableProperty SoundEffectModeProperty = BindableProperty.Create(nameof(SoundEffectMode), typeof(EffectMode), typeof(Button), default(EffectMode));
        /// <summary>
        /// Gets or sets the sound effect mode.
        /// </summary>
        /// <value>The sound effect mode.</value>
        public EffectMode SoundEffectMode
        {
            get => (EffectMode)GetValue(SoundEffectModeProperty);
            set => SetValue(SoundEffectModeProperty, value);
        }
        #endregion SoundEffectMode property

        #region IButtonState

        #region IconImage
        /// <summary>
        /// Backing store for the IconImage property
        /// </summary>
        public static BindableProperty IconImageProperty = BindableProperty.Create(nameof(IconImage), typeof(Forms9Patch.Image), typeof(Button), null);
        /// <summary>
        /// Gets or sets the icon image.  Alternatively, use IconText
        /// </summary>
        public Forms9Patch.Image IconImage
        {
            get => (Forms9Patch.Image)GetValue(IconImageProperty);
            set => SetValue(IconImageProperty, value);
        }
        #endregion IconImage

        #region IconText
        /// <summary>
        /// The image text property backing store
        /// </summary>
        public static readonly BindableProperty IconTextProperty = BindableProperty.Create(nameof(IconText), typeof(string), typeof(Button), default(string));
        /// <summary>
        /// Gets or sets the image text - use this to specify the image as an HTML markup string.
        /// </summary>
        /// <value>The image text.</value>
        public string IconText
        {
            get => (string)GetValue(IconTextProperty);
            set => SetValue(IconTextProperty, value);
        }
        #endregion IconText

        #region IconFontFamily property
        /// <summary>
        /// Backing store for the icon font family property.
        /// </summary>
        public static readonly BindableProperty IconFontFamilyProperty = BindableProperty.Create(nameof(IconFontFamily), typeof(string), typeof(Button), default(string));
        /// <summary>
        /// Gets or sets the font family for the IconText
        /// </summary>
        /// <value>The icon font family.</value>
        public string IconFontFamily
        {
            get => (string)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        #endregion IconFontFamily property

        #region TrailingIcon
        /// <summary>
        /// Backing store for the trailing image property.
        /// </summary>
        public static readonly BindableProperty TrailingIconProperty = BindableProperty.Create(nameof(TrailingIcon), typeof(bool), typeof(Button), default(bool));
        /// <summary>
        /// Gets or sets if the image is to be rendered after the text.
        /// </summary>
        /// <value>default=false</value>
        public bool TrailingIcon
        {
            get => (bool)GetValue(TrailingIconProperty);
            set => SetValue(TrailingIconProperty, value);
        }
        #endregion TrailingIcon

        #region TintIcon
        /// <summary>
        /// The tint image property backing store.
        /// </summary>
        public static readonly BindableProperty TintIconProperty = BindableProperty.Create(nameof(TintIcon), typeof(bool), typeof(Button), true);
        /// <summary>
        /// Will the TextColor be used to tint the IconImage?
        /// </summary>
        /// <value><c>true</c> tint IconImage with TextColor; otherwise, <c>false</c>.</value>
        public bool TintIcon
        {
            get => (bool)GetValue(TintIconProperty);
            set => SetValue(TintIconProperty, value);
        }

        #endregion TintIcon

        #region HasTightSpacing
        /// <summary>
        /// The has tight spacing property.
        /// </summary>
        public static readonly BindableProperty HasTightSpacingProperty = BindableProperty.Create(nameof(HasTightSpacing), typeof(bool), typeof(Button), default(bool));
        /// <summary>
        /// Gets or sets if the Icon/Image is close (TightSpacing) to text or at edge (not TightSpacing) of button.
        /// </summary>
        /// <value><c>true</c> if has tight spacing; otherwise, <c>false</c>.</value>
        public bool HasTightSpacing
        {
            get => (bool)GetValue(HasTightSpacingProperty);
            set => SetValue(HasTightSpacingProperty, value);
        }
        #endregion HasTightSpacing

        #region Spacing
        /// <summary>
        /// Backing store for the spacing property.
        /// </summary>
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(Button), 4.0);
        /// <summary>
        /// Gets or sets the spacing.
        /// </summary>
        /// <value>The spacing.</value>
        public double Spacing
        {
            get => (double)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }
        #endregion Spacing

        #region Orientation
        /// <summary>
        /// Backing store for the Button's orientation property.
        /// </summary>
        public static BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(Button), StackOrientation.Horizontal);
        /// <summary>
        /// Gets or sets the orientation of the iamge and label.
        /// </summary>
        /// <value>The image and label orientation.</value>
        public StackOrientation Orientation
        {
            get => (StackOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        #endregion Orientation

        #region IBackground 

        // BackgroundImage inherited

        #region IExtendedShape

        #region IShape


        #region BackgroundColor
        /// <summary>
        /// Backing store for the BackgroundColor property
        /// </summary>
        public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(Button), Color.Default);
        /// <summary>
        /// Gets or sets the button's background color
        /// </summary>
        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        #endregion


        #region HasShadow
        /// <summary>
        /// Backing store for the HasShadow property
        /// </summary>
        public static new readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(Button), false);
        /// <summary>
        /// controls if the button has a shadow
        /// </summary>
        public new bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }
        #endregion

        // ShadowInverted inherited

        #region OutlineColor
        /// <summary>
        /// Backing store for the OutlineColor property
        /// </summary>
        public static new readonly BindableProperty OutlineColorProperty = BindableProperty.Create(nameof(OutlineColor), typeof(Color), typeof(Button), Color.Default);
        /// <summary>
        /// controls the outline color of the button
        /// </summary>
        public new Color OutlineColor
        {
            get => (Color)GetValue(OutlineColorProperty);
            set => SetValue(OutlineColorProperty, value);
        }
        #endregion OutlineColor


        // OutlineRadius inherited

        #region OutlineWidth
        /// <summary>
        /// Backing store for the button's OutlineWidth property
        /// </summary>
        public static new readonly BindableProperty OutlineWidthProperty = BindableProperty.Create(nameof(OutlineWidth), typeof(float), typeof(Button), -1f);
        /// <summary>
        /// controls the width of the button's outline
        /// </summary>
        public new float OutlineWidth
        {
            get => (float)GetValue(OutlineWidthProperty);
            set => SetValue(OutlineWidthProperty, value);
        }
        #endregion OutlineWidth




        // ElementShape inherited from Forms9Patch.Frame

        // ExtendedElementShape inherited from Forms9Patch.Frame


        #endregion IShape

        #endregion IExtendedShape

        #endregion Superceding IBackground

        #region ILabel

        #region Text
        /// <summary>
        /// Backing store for the Button.Text bindable property.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(Button), null);
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        #endregion Text

        #region HtmlText
        /// <summary>
        /// Backing store for the formatted text property.
        /// </summary>
        public static readonly BindableProperty HtmlTextProperty = BindableProperty.Create(nameof(HtmlText), typeof(string), typeof(Button), null);
        /// <summary>
        /// Gets or sets the formatted text.
        /// </summary>
        /// <value>The formatted text.</value>
        public string HtmlText
        {
            get => (string)GetValue(HtmlTextProperty);
            set => SetValue(HtmlTextProperty, value);
        }
        #endregion HtmlText

        #region TextColor
        /// <summary>
        /// Backing store for the TextColor property
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Button), Color.Default);
        /// <summary>
        /// Gets or sets the font color of the Text or the base font color of the HtmlText
        /// </summary>
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        #endregion TextColor

        #region HorizontalTextAlignment
        /// <summary>
        /// Backing store for the Button.Alignment bindable property
        /// </summary>
        public static BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(Button), TextAlignment.Center);
        /// <summary>
        /// Gets or sets the alignment of the image and text.
        /// </summary>
        /// <value>The alignment (left, center, right).</value>
        public TextAlignment HorizontalTextAlignment
        {
            get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
            set => SetValue(HorizontalTextAlignmentProperty, value);
        }
        #endregion HorizontalTextAlignment

        #region VerticalTextAlignment
        /// <summary>
        /// Backing store for the vertical alignment property.
        /// </summary>
        public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create(nameof(VerticalTextAlignment), typeof(TextAlignment), typeof(Button), TextAlignment.Center);
        /// <summary>
        /// Gets or sets the vertical alignment.
        /// </summary>
        /// <value>The vertical alignment.</value>
        public TextAlignment VerticalTextAlignment
        {
            get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
            set => SetValue(VerticalTextAlignmentProperty, value);
        }
        #endregion VerticalTextAlignment

        #region LineBreakMode
        /// <summary>
        /// The line break mode property.
        /// </summary>
        public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create(nameof(LineBreakMode), typeof(LineBreakMode), typeof(Button), LineBreakMode.WordWrap);
        /// <summary>
        /// Gets or sets the line break mode.
        /// </summary>
        /// <value>The line break mode.</value>
        public LineBreakMode LineBreakMode
        {
            get => (LineBreakMode)GetValue(LineBreakModeProperty);
            set => SetValue(LineBreakModeProperty, value);
        }
        #endregion LineBreakMode

        #region AutoFit

        /// <summary>
        /// The fit property.
        /// </summary>
        public static readonly BindableProperty AutoFitProperty = BindableProperty.Create(nameof(AutoFit), typeof(AutoFit), typeof(Button), AutoFit.Width);
        /// <summary>
        /// Gets or sets the fit.
        /// </summary>
        /// <value>The fit.</value>
        public AutoFit AutoFit
        {
            get => (AutoFit)GetValue(AutoFitProperty);
            set => SetValue(AutoFitProperty, value);
        }
        #endregion AutoFit

        #region Lines
        /// <summary>
        /// The lines property.
        /// </summary>
        public static readonly BindableProperty LinesProperty = BindableProperty.Create(nameof(Lines), typeof(int), typeof(Button), 0);
        /// <summary>
        /// Gets or sets the lines.
        /// </summary>
        /// <value>The lines.</value>
        public int Lines
        {
            get => (int)GetValue(LinesProperty);
            set => SetValue(LinesProperty, value);
        }
        #endregion Lines

        #region MinFontSize
        /// <summary>
        /// The backing store for the minimum font size property.
        /// </summary>
        public static readonly BindableProperty MinFontSizeProperty = BindableProperty.Create(nameof(MinFontSize), typeof(double), typeof(Button), -1.0);
        /// <summary>
        /// Gets or sets the minimum size of the font allowed during an autofit. 
        /// </summary>
        /// <value>The minimum size of the font.  Default=4</value>
        public double MinFontSize
        {
            get => (double)GetValue(MinFontSizeProperty);
            set => SetValue(MinFontSizeProperty, value);
        }
        #endregion MinFontSize

        #region IFontElement

        #region FontSize
        /// <summary>
        /// Backing store for the Button.FontSize bindable property.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(Button), -1.0);//, BindingMode.OneWay), null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontSizePropertyChanged));
        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        #endregion FontSize

        #region FontFamily
        /// <summary>
        /// Backing store for the Button.FontFamiily bindable property.
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(Button), null);//, BindingMode.OneWay), null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontFamilyPropertyChanged)); 
        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>The font family.</value>
        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }
        #endregion FontFamily

        #region FontAttributes
        /// <summary>
        /// Backing store for the Button.FontAttributes bindable property.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(Button), FontAttributes.None);//, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontAttributesPropertyChanged));
        /// <summary>
        /// Gets or sets the font attributes.
        /// </summary>
        /// <value>The font attributes.</value>
        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }
        #endregion FontAttributes

        #endregion IFontElement

        #endregion ILabel

        #endregion IButtonState

        #endregion IButton



        /// <summary>
        /// Backing store for the Button.DarkTheme property.
        /// </summary>
        public static readonly BindableProperty DarkThemeProperty = BindableProperty.Create(nameof(DarkTheme), typeof(bool), typeof(Button), false);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Button"/> if for a dark theme.
        /// </summary>
        /// <value><c>true</c> if dark theme; otherwise, <c>false</c>.</value>
        public bool DarkTheme
        {
            get => (bool)GetValue(DarkThemeProperty);
            set => SetValue(DarkThemeProperty, value);
        }


        internal Size UnexpandedTightSize
        {
            get
            {
                var width = 0.0;
                var height = 0.0;
                var childCount = 0;
                foreach (var child in _stackLayout.Children)
                {
                    if (child is VisualElement element && element.IsVisible)
                    {
                        childCount++;
                        var measure = element.Measure(double.PositiveInfinity, double.PositiveInfinity);
                        width += Math.Ceiling(measure.Request.Width);
                        height += Math.Ceiling(measure.Request.Height);
                    }
                }
                if (childCount > 1)
                {
                    width += Orientation == StackOrientation.Horizontal ? Math.Ceiling(Spacing) : 0;
                    height += Orientation == StackOrientation.Vertical ? Math.Ceiling(Spacing) : 0;

                }
                width += Math.Ceiling(Padding.HorizontalThickness) + Math.Ceiling(Margin.HorizontalThickness);
                height += Math.Ceiling(Padding.VerticalThickness) + Math.Ceiling(Margin.VerticalThickness);

                return new Size(width, height);
            }
        }
        #endregion


        #region Internal, SegmentedButton Properties
        internal static BindableProperty GroupToggleBehaviorProperty = BindableProperty.Create(nameof(GroupToggleBehavior), typeof(GroupToggleBehavior), typeof(Button), GroupToggleBehavior.None);
        internal GroupToggleBehavior GroupToggleBehavior
        {
            get => (GroupToggleBehavior)GetValue(GroupToggleBehaviorProperty);
            set => SetValue(GroupToggleBehaviorProperty, value);
        }

        /// <summary>
        /// Gets the FittedFontSize of the button's text (assuming it hasn't been overridden by SychronizedFontSize
        /// </summary>
        public double FittedFontSize => _label.FittedFontSize;

        /// <summary>
        /// Overrides the button's label's FontSize for the purpose of synchronization between 
        /// </summary>
        public double SynchronizedFontSize
        {
            get => _label.SynchronizedFontSize;
            set => _label.SynchronizedFontSize = value;
        }

        internal int LabelInstanceId => _label.InstanceId;
        #endregion


        #region Fields
        bool _noUpdate = true;
        /// <summary>
        /// The stack layout.
        /// </summary>
        readonly Xamarin.Forms.StackLayout _stackLayout;
        /// <summary>
        /// The image.
        /// </summary>
        internal protected Forms9Patch.Image _iconImage;
        /// <summary>
        /// The label.
        /// </summary>
        internal protected Label _label;
        /// <summary>
        /// The icon label.
        /// </summary>
        internal protected Label _iconLabel;
        /// <summary>
        /// The gesture listener.
        /// </summary>
        internal protected FormsGestures.Listener _gestureListener;

        #endregion


        #region Constructor
        static Button()
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// The constructing.
        /// </summary>
        internal protected bool _constructing;

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        public Button()
        {
            _constructing = true;
            Padding = new Thickness(8, 0, 8, 0);
            OutlineRadius = 2;
            _label = new Label
            {
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                MinimizeHeight = false,
                Lines = Lines,
                AutoFit = AutoFit,
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
            _gestureListener.Up += OnUp;
            _gestureListener.Down += OnDown;
            _gestureListener.LongPressed += OnLongPressed;
            _gestureListener.LongPressing += OnLongPressing;

            SizeChanged += OnSizeChanged;

            UpdateElements();

            _constructing = false;
        }

        /// <summary>
        /// Instantiates an new Button and sets its Text and imageSource properties
        /// </summary>
        /// <param name="text"></param>
        /// <param name="image"></param>
        public Button(string text, Forms9Patch.Image image) : this()
        {
            Text = text;
            if (image != null)
                IconImage = image;
        }

        /// <summary>
        /// Instantiates an new Button and sets its Text property and sets the IconImage to a new image, created using the provided ImageSource
        /// </summary>
        /// <param name="text"></param>
        /// <param name="imageSource"></param>
        public Button(string text, Xamarin.Forms.ImageSource imageSource = null) : this()
        {
            Text = text;
            if (imageSource != null)
                IconImage = new Forms9Patch.Image(imageSource);
        }

        /// <summary>
        /// Instantiates a new Button and sets its Text and either its IconText or its IconImage, created using the provided embedded resource id 
        /// </summary>
        /// <param name="text">Button's text</param>
        /// <param name="icon">Segments's icon (either EmbeddedResourceId or HtmlText)</param>
        /// <param name="assembly">Assembly that has EmbeddedResource used for Icon</param>
        public Button(string text, string icon, Assembly assembly = null) : this()
        {
            Text = text;
            var isIconText = false;

            assembly = assembly ?? AssemblyExtensions.AssemblyFromResourceId(icon);
            //if (assembly == null && Device.RuntimePlatform != Device.UWP)
            //    assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);
            var match = Forms9Patch.ImageSource.BestEmbeddedMultiResourceMatch(icon, assembly);

            //if (icon.Contains("<") && icon.Contains("/>"))
            if (match == null)
            {
                var sansStarts = icon.Replace("<", "");
                var starts = icon.Length - sansStarts.Length;
                var sansEnds = icon.Replace(">", "");
                var ends = icon.Length - sansEnds.Length;
                isIconText = starts == ends;
            }
            if (isIconText)
                IconText = icon;
            else
            {
                IconImage = new Forms9Patch.Image(icon, assembly);
            }
        }
        #endregion


        #region IDisposable Support
        /// <summary>
        /// The disposed value.
        /// </summary>
        bool _disposed; // To detect redundant calls

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">Disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;

                _tapped = null;
                _pressed = null;
                _released = null;
                _selected = null;
                _longPressing = null;
                _longPressed = null;

                _gestureListener.Tapped -= OnTapped;
                _gestureListener.Up -= OnUp;
                _gestureListener.Down -= OnDown;
                _gestureListener.LongPressed -= OnLongPressed;
                _gestureListener.LongPressing -= OnLongPressing;
                _gestureListener.Dispose();
                _gestureListener = null;

                SizeChanged -= OnSizeChanged;

                _label.PropertyChanged -= OnLabelPropertyChanged;

                if (_iconImage != null)
                    _iconImage.SizeChanged -= OnIconImageSizeChanged;

                if (_iconLabel != null)
                    _iconLabel.SizeChanged -= OnIconLabelSizeChanged;

                if (BackgroundImage is IDisposable backgroundImage)
                    backgroundImage.Dispose();

                if (IconImage is IDisposable iconImage)
                    iconImage.Dispose();

                if (Command != null)
                    Command.CanExecuteChanged -= CommandCanExecuteChanged;

            }
            base.Dispose(disposing);
        }

        #endregion


        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Forms9Patch.Button"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Forms9Patch.Button"/>.</returns>
        public override string ToString()
        {
            var result = base.ToString();
            result += "[" + (Text ?? HtmlText) + "]";
            return result;
        }


        #region Gesture event responders
        bool HandleTap()
        {
            if (IsEnabled && IsVisible)
            {
                if (!(this is StateButton))
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
                Haptics.Feedback(HapticEffect, HapticEffectMode);
                Audio.PlaySoundEffect(SoundEffect, SoundEffectMode);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tap this instance.
        /// </summary>
        public void Tap()
            => HandleTap();

        /// <summary>
        /// Called when button is pressed down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnDown(object sender, FormsGestures.DownUpEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "OnDown");
            e.Handled = IsEnabled && IsVisible;
        }

        /// <summary>
        /// Called when button is released
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnUp(object sender, FormsGestures.DownUpEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "OnUp");
            if (!IsLongPressEnabled)
                e.Handled = HandleTap();
        }

        /// <summary>
        /// Called when button is tapped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnTapped(object sender, FormsGestures.TapEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "OnTapped");
            if (IsLongPressEnabled)
                e.Handled = HandleTap();
            else
                e.Handled |= (IsEnabled && IsVisible);
        }

        /// <summary>
        /// Called whe button is released after long press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnLongPressed(object sender, FormsGestures.LongPressEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "OnLongPressed");
            if (IsEnabled && IsVisible && IsLongPressEnabled)
            {
                _longPressed?.Invoke(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called whe button has been long pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnLongPressing(object sender, FormsGestures.LongPressEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "OnLongPressing");
            if (IsEnabled && IsVisible && IsLongPressEnabled)
            {
                _longPressing?.Invoke(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        #endregion


        #region State management
        /// <summary>
        /// Updates the elements in the button
        /// </summary>
        /// <param name="isSegment"></param>
        protected virtual void UpdateElements(bool isSegment = false)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => UpdateElements(isSegment));
                return;
            }

            if (this is StateButton)
                return;


            P42.Utils.Recursion.Enter(GetType().ToString(), InstanceId.ToString());
            _noUpdate = true;

            var enabledLabelColor = TextColor == Color.Default ? (DarkTheme ? Color.White : Color.FromHex("#000").WithAlpha(0.5)) : TextColor;
            if (IsSelected && SelectedTextColor.A > 0)
                enabledLabelColor = SelectedTextColor;

            base.OutlineWidth = OutlineWidth < 0 ? (BackgroundImage?.Source != null ? 0 : (BackgroundColor.A > 0 ? 0 : 1)) : (OutlineColor == Color.Transparent ? 0 : OutlineWidth);

            if (IsEnabled)
            {
                _label.TextColor = enabledLabelColor;

                if (IsSelected)
                {
                    if (SelectedBackgroundColor.A > 0)
                        base.BackgroundColor = SelectedBackgroundColor;
                    else
                        base.BackgroundColor = BackgroundColor.A > 0 ? BackgroundColor.RgbBlend(Color.FromHex("#000"), 0.25) : Color.FromHex(DarkTheme ? "#FFF" : "#000").WithAlpha(0.2);
                    base.OutlineColor = BackgroundColor.A > 0 ? OutlineColor.RgbBlend(Color.FromHex("#000"), 0.25) : base.BackgroundColor.A > 0 ? base.BackgroundColor : enabledLabelColor;
                }
                else
                {
                    base.BackgroundColor = BackgroundColor.A > 0 ? BackgroundColor : _label.BackgroundColor;
                    base.OutlineColor = OutlineColor;
                }
                if (OutlineColor == Color.Default)
                    base.OutlineColor = enabledLabelColor;

                base.HasShadow = (base.BackgroundColor.A > 0 || BackgroundImage?.Source != null) && HasShadow;
                ShadowInverted = IsSelected && !isSegment;
            }
            else
            {
                _label.TextColor = DarkTheme ? Color.FromHex("#FFF").WithAlpha(0.30) : Color.FromHex("#000").WithAlpha(0.26);
                if (!isSegment)
                {
                    base.HasShadow = false;
                    base.BackgroundColor = BackgroundColor.A > 0 ? OpaqueColor : TransparentColor;
                    base.OutlineColor = OutlineColor == Color.Default || OutlineColor.A > 0 ? OpaqueColor : BackgroundColor;
                }
                else
                {
                    // the next line is already covered above
                    base.HasShadow = BackgroundColor.A > 0 && HasShadow;
                    if (IsSelected)
                    {
                        if (SelectedBackgroundColor.A > 0)
                            base.BackgroundColor = SelectedBackgroundColor.RgbBlend(Color.FromHex("#000"), 0.25);
                        else
                            base.BackgroundColor = BackgroundColor.A > 0 ? BackgroundColor.RgbBlend(Color.FromHex("#000"), 0.25) : Color.FromHex(DarkTheme ? "#FFF" : "#000").WithAlpha(0.2);
                        base.OutlineColor = BackgroundColor.A > 0 ? OutlineColor.RgbBlend(Color.FromHex("#000"), 0.25) : base.BackgroundColor.A > 0 ? base.BackgroundColor : enabledLabelColor; // _label.TextColor;
                    }
                    else
                    {
                        base.BackgroundColor = BackgroundColor.A > 0 ? BackgroundColor : _label.BackgroundColor;
                        base.OutlineColor = OutlineColor;
                    }
                    if (OutlineColor == Color.Default)
                        base.OutlineColor = enabledLabelColor;
                }
            }
            UpdateIconTint();
            _noUpdate = false;
            P42.Utils.Recursion.Exit(GetType().ToString(), InstanceId.ToString());
        }

        Color OpaqueColor
        {
            get
            {
                return IsSelected
                    ? DarkTheme
                        ? Color.FromHex("#FFF").WithAlpha(0.2)
                        : Color.FromHex("#000").WithAlpha(0.2)
                    : DarkTheme
                       ? Color.FromHex("#FFF").WithAlpha(0.1)
                       : Color.FromHex("#000").WithAlpha(0.1);
            }
        }

        Color TransparentColor
        {
            get
            {
                return IsSelected
                    ? DarkTheme
                        ? Color.FromHex("#FFF").WithAlpha(0.1)
                        : Color.FromHex("#000").WithAlpha(0.1)
                    : Color.Transparent;
            }
        }


        internal void UpdateIconTint()
        {
            if (_iconImage != null)
                _iconImage.TintColor = TintIcon ? _label.TextColor : Color.Default;
            if (_iconLabel != null)
            {
                _iconLabel.TextColor = _label.TextColor;
                _iconLabel.FontSize = _label.FontSize;
                _iconLabel.FontFamily = IconFontFamily;
            }
        }
        #endregion


        #region Events

        bool IsEnabledCore
        {
            set => SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Xamarin.Forms.IButton Clicked event
        /// </summary>
        public event EventHandler Clicked
        {
            add { _tapped += value; }
            remove { _tapped -= value; }
        }

        event EventHandler _pressed;
        /// <summary>
        /// Xamarin.Forms.IButton Pressed event
        /// </summary>
        public event EventHandler Pressed
        {
            add { _pressed += value; }
            remove { _pressed -= value; }
        }

        event EventHandler _released;
        /// <summary>
        /// Xamarin.Forms.IButton Released event
        /// </summary>
        public event EventHandler Released
        {
            add { _released += value; }
            remove { _released -= value; }
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


        //event EventHandler _actualFontSizeChanged;
        internal event EventHandler<double> FittedFontSizeChanged
        {
            add { _label.FittedFontSizeChanged += value; }
            remove { _label.FittedFontSizeChanged -= value; }
        }

        #endregion


        #region Change Handlers
        private void OnSizeChanged(object sender, EventArgs e)
            => IsClipped = CheckIsClipped();

        private void OnIconLabelSizeChanged(object sender, EventArgs e)
            => IsClipped = CheckIsClipped();

        private void OnIconImageSizeChanged(object sender, EventArgs e)
            => IsClipped = CheckIsClipped();

        private void OnLabelSizeChanged(object sender, EventArgs e)
            => IsClipped = CheckIsClipped();

        /// <summary>
        /// Test if the segment contents are clipped
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public bool CheckIsClipped(double width = -1, double height = -1)
        {
            var isClipped = false;

            if (width < 1) width = Width;
            if (height < 1) height = Height;

            if (width < 1 || height < 1)
                return isClipped;

            P42.Utils.Recursion.Enter(GetType().ToString(), InstanceId.ToString());

            var elementWidths = Padding.HorizontalThickness + Margin.HorizontalThickness;
            var elementHeights = Padding.VerticalThickness + Margin.VerticalThickness;
            var notFirst = false;
            foreach (var child in _stackLayout.Children)
            {
                var hzFree = width - elementWidths;
                var vtFree = height - elementHeights;

                isClipped = Orientation == StackOrientation.Horizontal
                    ? hzFree < 0.01
                    : vtFree < 0.01;

                if (!isClipped)
                {
                    if (child.IsVisible)
                    {
                        if (child is Forms9Patch.Label label)
                        {
                            var fontSize = label.SynchronizedFontSize;
                            if (fontSize < 0)
                                fontSize = label.FittedFontSize < 0
                                ? label.FontSize < 0
                                    ? Label.DefaultFontSize
                                    : label.FontSize
                                : label.FittedFontSize;
                            var hzSize = label.SizeForWidthAndFontSize(double.MaxValue, fontSize);
                            var vtSize = label.SizeForWidthAndFontSize(width, fontSize);
                            elementWidths += hzSize.Width;
                            elementHeights += vtSize.Height;
                        }
                        else
                        {
                            elementWidths += child.Width;
                            elementHeights += child.Height;
                        }
                        if (notFirst)
                        {
                            if (Orientation == StackOrientation.Horizontal)
                                elementWidths += Spacing;
                            else
                                elementHeights += Spacing;
                        }
                        notFirst = true;
                    }

                    hzFree = width - elementWidths;
                    vtFree = height - elementHeights;

                    isClipped = Orientation == StackOrientation.Horizontal
                        ? hzFree < 0.01
                        : vtFree < 0.01;
                }
            }
            P42.Utils.Recursion.Exit(GetType().ToString(), InstanceId.ToString());
            return isClipped;
        }


        void OnLabelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnLabelPropertyChanged(sender, e));
                return;
            }

            var propertyName = e.PropertyName;
            if (propertyName == Xamarin.Forms.Label.TextProperty.PropertyName || propertyName == Label.HtmlTextProperty.PropertyName)
            {
                if (string.IsNullOrEmpty((string)_label) && _stackLayout.Children.Contains(_label))
                    _stackLayout.Children.Remove(_label);
                else if (!string.IsNullOrEmpty((string)_label) && !_stackLayout.Children.Contains(_label))
                {
                    if (TrailingIcon)
                        _stackLayout.Children.Insert(0, _label);
                    else
                        _stackLayout.Children.Add(_label);
                }
                SetOrienations();
            }
            else if (propertyName == Xamarin.Forms.Label.TextColorProperty.PropertyName)
                UpdateIconTint();
            else if (propertyName == Forms9Patch.Label.FittedFontSizeProperty.PropertyName || propertyName == Forms9Patch.Label.SynchronizedFontSizeProperty.PropertyName)
                CheckIsClipped();
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
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanging(propertyName));
                return;
            }

            base.OnPropertyChanging(propertyName);

            if (_noUpdate)
                return;
            if (propertyName == CommandProperty.PropertyName)
            {
                var command = Command;
                if (command != null)
                    command.CanExecuteChanged -= CommandCanExecuteChanged;
            }
        }

        void SetOrienations()
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(SetOrienations);
                return;
            }

            P42.Utils.Recursion.Enter(GetType().ToString(), InstanceId.ToString());

            var horzOption = HorizontalTextAlignment.ToLayoutOptions();
            var vertOption = VerticalTextAlignment.ToLayoutOptions();
            if (Orientation == StackOrientation.Horizontal)
            {
                if (string.IsNullOrEmpty(_label.Text ?? _label.HtmlText))
                {
                    if (_iconImage != null)
                    {
                        _iconImage.HorizontalOptions = horzOption;
                        _iconImage.VerticalOptions = vertOption;
                    }
                    if (_iconLabel != null)
                    {
                        _iconLabel.HorizontalTextAlignment = HorizontalTextAlignment;
                        _iconLabel.VerticalTextAlignment = VerticalTextAlignment;
                        _iconLabel.HorizontalOptions = HorizontalTextAlignment.ToLayoutOptions(true);
                        _iconLabel.VerticalOptions = LayoutOptions.Fill;
                        _iconLabel.FontFamily = IconFontFamily;
                    }
                    _stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                }
                else if (HasTightSpacing)
                {
                    if (_iconImage != null)
                    {
                        _iconImage.HorizontalOptions = LayoutOptions.Center;
                        _iconImage.VerticalOptions = vertOption;
                    }
                    if (_iconLabel != null)
                    {
                        _iconLabel.HorizontalTextAlignment = TextAlignment.Center;
                        _iconLabel.VerticalTextAlignment = VerticalTextAlignment;
                        _iconLabel.HorizontalOptions = LayoutOptions.Center;
                        _iconLabel.VerticalOptions = LayoutOptions.Fill;
                        _iconLabel.FontFamily = IconFontFamily;
                    }
                    if (_label != null)
                    {
                        _label.HorizontalTextAlignment = HorizontalTextAlignment;
                        _label.VerticalTextAlignment = VerticalTextAlignment;
                        _label.HorizontalOptions = LayoutOptions.Start;
                        _label.VerticalOptions = LayoutOptions.Fill;
                        _label.MinimizeHeight = false;
                    }
                    _stackLayout.HorizontalOptions = horzOption;
                }
                else
                {
                    if (_iconImage != null)
                    {
                        _iconImage.HorizontalOptions = LayoutOptions.Center;
                        _iconImage.VerticalOptions = vertOption;
                    }
                    if (_iconLabel != null)
                    {
                        _iconLabel.HorizontalTextAlignment = TextAlignment.Center;
                        _iconLabel.VerticalTextAlignment = VerticalTextAlignment;
                        _iconLabel.HorizontalOptions = LayoutOptions.Center;
                        _iconLabel.VerticalOptions = LayoutOptions.Fill;
                        _iconLabel.FontFamily = IconFontFamily;
                    }
                    if (_label != null)
                    {
                        _label.HorizontalTextAlignment = HorizontalTextAlignment;
                        _label.VerticalTextAlignment = VerticalTextAlignment;
                        _label.HorizontalOptions = LayoutOptions.FillAndExpand;
                        _label.VerticalOptions = LayoutOptions.Fill;
                        _label.MinimizeHeight = false;
                    }
                    _stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                    _label?.HardForceLayout();
                }
                _stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            }
            else
            {
                if (string.IsNullOrEmpty(_label.Text ?? _label.HtmlText))
                {
                    if (_iconImage != null)
                    {
                        _iconImage.HorizontalOptions = horzOption;
                        _iconImage.VerticalOptions = vertOption;
                    }
                    if (_iconLabel != null)
                    {
                        _iconLabel.VerticalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalOptions = horzOption;
                        _iconLabel.VerticalOptions = vertOption;
                        _iconLabel.FontFamily = IconFontFamily;
                    }
                    _stackLayout.HorizontalOptions = LayoutOptions.Fill;
                    _stackLayout.VerticalOptions = VerticalTextAlignment.ToLayoutOptions(true);
                }
                else if (HasTightSpacing)
                {
                    if (_iconImage != null)
                    {
                        _iconImage.HorizontalOptions = horzOption;
                        _iconImage.VerticalOptions = LayoutOptions.Center;
                    }
                    if (_iconLabel != null)
                    {
                        _iconLabel.VerticalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalOptions = horzOption;
                        _iconLabel.VerticalOptions = LayoutOptions.Center;
                        _iconLabel.FontFamily = IconFontFamily;
                    }
                    if (_label != null)
                    {
                        _label.VerticalTextAlignment = VerticalTextAlignment;
                        _label.HorizontalTextAlignment = TextAlignment.Center;
                        _label.HorizontalOptions = horzOption;
                        _label.VerticalOptions = LayoutOptions.Center;
                        _label.MinimizeHeight = true;
                    }
                    _stackLayout.HorizontalOptions = LayoutOptions.Fill;
                    _stackLayout.VerticalOptions = VerticalTextAlignment.ToLayoutOptions(true);
                }
                else
                {
                    if (_iconImage != null)
                    {
                        _iconImage.HorizontalOptions = horzOption;
                        _iconImage.VerticalOptions = (TrailingIcon ? LayoutOptions.End : LayoutOptions.Start);
                    }
                    if (_iconLabel != null)
                    {
                        _iconLabel.VerticalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalOptions = horzOption;
                        _iconLabel.VerticalOptions = (TrailingIcon ? LayoutOptions.End : LayoutOptions.Start);
                        _iconLabel.FontFamily = IconFontFamily;
                    }
                    if (_label != null)
                    {
                        _label.VerticalTextAlignment = HorizontalTextAlignment;
                        _label.HorizontalTextAlignment = TextAlignment.Center;
                        _label.HorizontalOptions = horzOption;
                        _label.VerticalOptions = VerticalTextAlignment.ToLayoutOptions(true);
                        _label.MinimizeHeight = true;
                    }
                    _stackLayout.HorizontalOptions = LayoutOptions.Fill;
                    _stackLayout.VerticalOptions = LayoutOptions.Fill;
                }
            }
            _stackLayout.Orientation = Orientation;
            P42.Utils.Recursion.Exit(GetType().ToString(), InstanceId.ToString());
        }



        bool _changingIconImage;
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
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            if (propertyName == SelectedBackgroundColorProperty.PropertyName && IsSelected)
                UpdateElements();

            base.OnPropertyChanged(propertyName);

            if (_noUpdate)
                return;

            if (propertyName == HorizontalTextAlignmentProperty.PropertyName || propertyName == VerticalTextAlignmentProperty.PropertyName)
                SetOrienations();
            else if (propertyName == IconImageProperty.PropertyName)
            {
                if (_changingIconText)
                    return;
                _changingIconImage = true;
                if (_iconImage != null)
                {
                    _iconImage.SizeChanged -= OnIconImageSizeChanged;
                    _stackLayout.Children.Remove(_iconImage);
                }
                if (IconImage != null)
                {
                    if (!IconImage.FillOrLayoutSet)
                    {
                        IconImage.Fill = Fill.AspectFit;
                        IconImage.HorizontalOptions = LayoutOptions.Center;
                        IconImage.VerticalOptions = LayoutOptions.Center;
                    }
                    if (_iconLabel != null)
                    {
                        _iconLabel.SizeChanged -= OnIconLabelSizeChanged;
                        _stackLayout.Children.Remove(_iconLabel);
                        _iconLabel.HtmlText = null;
                        _iconLabel.Text = null;
                        IconText = null;
                    }
                    _iconImage = IconImage;
                    UpdateIconTint();
                    if (_iconImage != null)
                    {
                        _iconImage.SizeChanged += OnIconImageSizeChanged;
                        if (TrailingIcon)
                            _stackLayout.Children.Add(_iconImage);
                        else
                            _stackLayout.Children.Insert(0, _iconImage);
                        SetOrienations();
                    }
                }
                _changingIconImage = false;
            }
            else if (propertyName == IconTextProperty.PropertyName)
            {
                if (_changingIconImage)
                    return;
                _changingIconText = true;
                if (_iconLabel != null)
                {
                    _iconLabel.SizeChanged -= OnIconLabelSizeChanged;
                    _stackLayout.Children.Remove(_iconLabel);
                    _iconLabel.FontFamily = IconFontFamily;
                }
                if (IconText != null)
                {
                    if (_iconImage != null)
                    {
                        _iconImage.SizeChanged -= OnIconImageSizeChanged;
                        _stackLayout.Children.Remove(_iconImage);
                    }
                    _iconLabel = new Label
                    {
                        HtmlText = IconText,
                        TextColor = _label.TextColor,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        Lines = 1,
                        FontSize = FontSize,
                        AutoFit = AutoFit.None,
                        FontFamily = IconFontFamily
                    };
                    if (_iconLabel != null)
                    {
                        _iconLabel.SizeChanged += OnIconLabelSizeChanged;
                        if (TrailingIcon)
                            _stackLayout.Children.Add(_iconLabel);
                        else
                            _stackLayout.Children.Insert(0, _iconLabel);
                        SetOrienations();
                        _iconLabel.FontFamily = IconFontFamily;
                    }
                    UpdateIconTint();
                }
                _changingIconText = false;
            }
            else if (propertyName == BackgroundColorProperty.PropertyName
                || propertyName == BackgroundImageProperty.PropertyName
                || propertyName == HasShadowProperty.PropertyName
                || propertyName == ShadowInvertedProperty.PropertyName
                || propertyName == OutlineColorProperty.PropertyName
                || propertyName == OutlineWidthProperty.PropertyName
                // below line moved before base.OnPropertyChanged to fix rendering issue on UWP.
                //|| (propertyName == SelectedBackgroundColorProperty.PropertyName && IsSelected)
                || propertyName == IsSelectedProperty.PropertyName
                || propertyName == IsEnabledProperty.PropertyName
                || propertyName == DarkThemeProperty.PropertyName
                    )
                UpdateElements();
            else if (propertyName == OrientationProperty.PropertyName)
                SetOrienations();
            else if (propertyName == TextColorProperty.PropertyName && !IsSelected)
            {
                UpdateElements();
                UpdateIconTint();
            }
            else if (propertyName == SelectedTextColorProperty.PropertyName && IsSelected)
            {
                UpdateElements();
                UpdateIconTint();
            }
            else if (propertyName == TintIconProperty.PropertyName && _iconImage != null)
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
                    if (GroupToggleBehavior != GroupToggleBehavior.None)
                        Command?.Execute(CommandParameter);
                    _selected?.Invoke(this, EventArgs.Empty);
                }
            }
            else if (propertyName == LinesProperty.PropertyName)
                _label.Lines = Lines;
            else if (propertyName == AutoFitProperty.PropertyName)
                _label.AutoFit = AutoFit;
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
            else if (propertyName == IconFontFamilyProperty.PropertyName && _iconLabel != null)
                _iconLabel.FontFamily = IconFontFamily;
            else if (propertyName == TrailingIconProperty.PropertyName && _stackLayout.Children.Contains(_label))
            {
                if (_label != null && _stackLayout.Children.Contains(_label))
                {
                    _stackLayout.Children.Remove(_label);
                    if (TrailingIcon)
                        _stackLayout.Children.Insert(0, _label);
                    else
                        _stackLayout.Children.Add(_label);
                }
                SetOrienations();
            }
            else if (propertyName == SpacingProperty.PropertyName)
                _stackLayout.Spacing = Spacing;
            else if (propertyName == TintIconProperty.PropertyName)
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
            => IsEnabledCore = (Command?.CanExecute(CommandParameter) ?? IsEnabled);


        /// <summary>
        /// Sends the tapped.
        /// </summary>
        internal protected void SendTapped()
        {
            P42.Utils.BreadCrumbs.Add(GetType(), Text ?? HtmlText);
            if (GroupToggleBehavior == GroupToggleBehavior.None || IsSelected)
                Command?.Execute(CommandParameter);
            _tapped?.Invoke(this, EventArgs.Empty);
        }

        #endregion


        #region IButtonController
        /// <summary>
        /// Xamarin.Forms.IButtonController SendClicked method
        /// </summary>
        public void SendClicked()
        {
            SendTapped();
        }

        /// <summary>
        /// Xamarin.Forms.IButtonController SendPressed method
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SendPressed()
        {
            _pressed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Xamarin.Forms.IButtonController SendReleased method
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SendReleased()
        {
            _released?.Invoke(this, EventArgs.Empty);
        }


        #endregion


    }

}


