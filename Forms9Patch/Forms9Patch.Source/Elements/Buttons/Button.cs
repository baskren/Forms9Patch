using System;
using Xamarin.Forms;
using System.Windows.Input;
using P42.Utils;
using System.ComponentModel;
using System.Reflection;

namespace Forms9Patch
{
    /// <summary>
    /// DEPRICATED: Use Button
    /// </summary>
    [Obsolete("Depricated: Use Forms9Patch.Button")]
    public class MaterialButton : Forms9Patch.Button { }

    /// <summary>
    /// Forms9Patch Button.
    /// </summary>
    public class Button : Frame, IDisposable, IButton, IExtendedShape
    {
        #region Xamarin.Forms emulation properties
        /*
        /// <summary>
        /// The border radius property.
        /// </summary>
        public static readonly BindableProperty BorderRadiusProperty = Xamarin.Forms.Button.BorderRadiusProperty;
        /// <summary>
        /// Gets or sets the border radius (Xamarin.Forms.Button compatibility property).
        /// </summary>
        /// <value>The border radius.</value>
        public int BorderRadius
        {
            get { return (int)GetValue(BorderRadiusProperty); }
            set { SetValue(BorderRadiusProperty, value); }
        }
        */

        /// <summary>
        /// The border width property.
        /// </summary>
        public static readonly BindableProperty BorderWidthProperty = Xamarin.Forms.Button.BorderWidthProperty;
        /// <summary>
        /// Gets or sets the width of the border (Xamarin.Forms.Button compatibility property).
        /// </summary>
        /// <value>The width of the border.</value>
        public double BorderWidth
        {
            get { return (double)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }

        /*
        /// <summary>
        /// The border color property.
        /// </summary>
        public static readonly BindableProperty BorderColorProperty = Xamarin.Forms.Button.BorderColorProperty;
        /// <summary>
        /// Gets or sets the color of the border (Xamarin.Forms.Button compatibility property).
        /// </summary>
        /// <value>The color of the border.</value>
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }
        */

        #endregion


        #region Obsolete Properties
        /// <summary>
        /// OBSOLETE: Use SelectedTextColorProperty
        /// </summary>
        [Obsolete("Use SelectedTextColorProperty")]
        public static readonly BindableProperty SelectedFontColorProperty = BindableProperty.Create("SelectedFontColor", typeof(Color), typeof(Button), Color.Default, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Button button && newValue is Xamarin.Forms.Color color)
                button.SelectedTextColor = color;
        });
        /// <summary>
        /// OBSOLETE: Use SelectedTextColor
        /// </summary>
        /// <value>The color of the font.</value>
        [Obsolete("Use SelectedTextColor")]
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
        public static BindableProperty ImageSourceProperty = BindableProperty.Create("ImageSource", typeof(Xamarin.Forms.ImageSource), typeof(Button), null, propertyChanged: (bindable, oldValue, newValue) =>
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
        [Obsolete("Use TrailingIconProperty")]
        public static readonly BindableProperty TrailingImageProperty = BindableProperty.Create("TrailingImage", typeof(bool), typeof(Button), default(bool), propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Button button && newValue is bool value)
                button.TrailingIcon = value;
        });
        /// <summary>
        /// OBSOLETE: Use TrailingIcon
        /// </summary>
        [Obsolete("Use TrailingIcon")]
        public bool TrailingImage
        {
            get => (bool)GetValue(TrailingImageProperty);
            set => SetValue(TrailingImageProperty, value);
        }

        /// <summary>
        /// OBSOLETE: Used TintIconProperty
        /// </summary>
        [Obsolete("Use TintIconProperty")]
        public static readonly BindableProperty TintImageProperty = BindableProperty.Create("TintImage", typeof(bool), typeof(Button), true, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Button button && newValue is bool value)
                button.TintIcon = value;
        });
        /// <summary>
        /// OBSOLETE: Use TintIcon property
        /// </summary>
        [Obsolete("Use TintIcon")]
        public bool TintImage
        {
            get => (bool)GetValue(TintImageProperty);
            set => SetValue(TintImageProperty, value);
        }

        #region IsElliptical property
        /// <summary>
        /// backing store for IsEliptical property
        /// </summary>
        [Obsolete("Use ElementShapeProperty instead.")]
        public static readonly BindableProperty IsEllipticalProperty = BindableProperty.Create("IsElliptical", typeof(bool), typeof(Button), default(bool), propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Button button && newValue is bool value)
                button.ElementShape = value ? ElementShape.Elliptical : ElementShape.Rectangle;
        });
        /// <summary>
        /// Gets/Sets the IsEliptical property
        /// </summary>
        [Obsolete("Use ElementShape property instead.")]
        public bool IsElliptical
        {
            get { return (bool)GetValue(IsEllipticalProperty); }
            set { SetValue(IsEllipticalProperty, value); }
        }
        #endregion IsElliptical property

        /// <summary>
        /// OBSOLETE: Use TextColorProperty
        /// </summary>
        [Obsolete("Use TextColorProperty")]
        public static readonly BindableProperty FontColorProperty = BindableProperty.Create("FontColor", typeof(Color), typeof(Button), Color.Default, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Button button && newValue is Xamarin.Forms.Color color)
                button.TextColor = color;
        });
        /// <summary>
        /// OBSOLETE: Use TextColor
        /// </summary>
        /// <value>The color of the font.</value>
        [Obsolete("Use TextColor")]
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
        [Obsolete("Alignment is obsolete, see HorizontalTextAlignment and VerticalTextAlignment")]
        public TextAlignment Alignment
        {
            get => (TextAlignment)GetValue(AlignmentProperty);
            set => SetValue(AlignmentProperty, value);
        }

        /// <summary>
        /// backing store for Fit property
        /// </summary>
        [Obsolete("FitProperty is obsolete.  Use AutoFitProperty instead.")]
        public static readonly BindableProperty FitProperty = BindableProperty.Create("Fit", typeof(LabelFit), typeof(Button), LabelFit.None, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is Label label && newValue is LabelFit fit)
            {
                switch (fit)
                {
                    case LabelFit.None: label.AutoFit = AutoFit.None; break;
                    case LabelFit.Width: label.AutoFit = AutoFit.Width; break;
                    case LabelFit.Lines: label.AutoFit = AutoFit.Lines; break;
                }
            }
        });
        /// <summary>
        /// Gets/Sets the Fit property
        /// </summary>
        [Obsolete("Fit property is obsolete.  Use AutoFit property instead.")]
        public LabelFit Fit
        {
            get => (LabelFit)GetValue(FitProperty);
            set => SetValue(FitProperty, value);
        }
        #endregion


        #region Properties
        /// <summary>
        /// UNSUPPORTED INHERITED PROPERTY.
        /// </summary>
        /// <value>The content.</value>
        [Obsolete("Unsupported Property", true)]
        public new View Content
        {
            get { throw new NotSupportedException("[Forms9Patch.Button] Content property is not supported"); }
            set { throw new NotSupportedException("[Forms9Patch.Button] Content property is not supported"); }
        }


        #region IButton

        #region SelectedTextColor
        /// <summary>
        /// Backing store for the SelectedTextColor property
        /// </summary>
        public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create("SelectedTextColor", typeof(Color), typeof(Button), Color.Default);
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
        public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create("SelectedBackgroundColor", typeof(Color), typeof(Button), Color.Transparent);
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
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(Button), null, BindingMode.OneWay, null,
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
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(Button), null, BindingMode.OneWay, null,
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
        public static BindableProperty ToggleBehaviorProperty = BindableProperty.Create("ToggleBehavior", typeof(bool), typeof(Button), false);
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
        public static BindableProperty IsSelectedProperty = BindableProperty.Create("IsSelected", typeof(bool), typeof(Button), false, BindingMode.TwoWay);
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
        public static readonly BindableProperty HapticEffectProperty = BindableProperty.Create("HapticEffect", typeof(HapticEffect), typeof(Button), HapticEffect.KeyClick);
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

        #region HapticMode
        /// <summary>
        /// The haptic mode property.
        /// </summary>
        public static readonly BindableProperty HapticModeProperty = BindableProperty.Create("HapticMode", typeof(KeyClicks), typeof(Button), default(KeyClicks));
        /// <summary>
        /// Gets or sets the haptic mode.
        /// </summary>
        /// <value>The haptic mode.</value>
        public KeyClicks HapticMode
        {
            get => (KeyClicks)GetValue(HapticModeProperty);
            set => SetValue(HapticModeProperty, value);
        }
        #endregion HapticMode

        #region IButtonState

        #region IconImage
        /// <summary>
        /// Backing store for the IconImage property
        /// </summary>
        public static BindableProperty IconImageProperty = BindableProperty.Create("IconImage", typeof(Forms9Patch.Image), typeof(Button), null);
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
        public static readonly BindableProperty IconTextProperty = BindableProperty.Create("IconText", typeof(string), typeof(Button), default(string));
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

        #region TrailingIcon
        /// <summary>
        /// Backing store for the trailing image property.
        /// </summary>
        public static readonly BindableProperty TrailingIconProperty = BindableProperty.Create("TrailingIcon", typeof(bool), typeof(Button), default(bool));
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
        public static readonly BindableProperty TintIconProperty = BindableProperty.Create("TintIcon", typeof(bool), typeof(Button), true);
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
        public static readonly BindableProperty HasTightSpacingProperty = BindableProperty.Create("HasTightSpacing", typeof(bool), typeof(Button), default(bool));
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
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create("Spacing", typeof(double), typeof(Button), 4.0);
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
        public static BindableProperty OrientationProperty = BindableProperty.Create("Orientation", typeof(StackOrientation), typeof(Button), StackOrientation.Horizontal);
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

        #region ExtendedElementShapeOrientation property
        /// <summary>
        /// Backing store for the extended element shape orientation property.
        /// </summary>
        public static readonly BindableProperty ExtendedElementShapeOrientationProperty = ShapeBase.ExtendedElementShapeOrientationProperty;
        /// <summary>
        /// Gets or sets the orientation of the shape if it's an extended element shape
        /// </summary>
        /// <value>The forms9 patch. IS hape. extended element shape orientation.</value>
        ExtendedElementShapeOrientation IExtendedShape.ExtendedElementShapeOrientation
        {
            get => (ExtendedElementShapeOrientation)GetValue(ExtendedElementShapeOrientationProperty);
            set => SetValue(ExtendedElementShapeOrientationProperty, value);
        }
        #endregion

        #region ExtendedElementShape property
        /// <summary>
        /// backing store for ExtendedElementShape property
        /// </summary>
        public static readonly BindableProperty ExtendedElementShapeProperty = ShapeBase.ExtendedElementShapeProperty;// = BindableProperty.Create("ExtendedElementShape", typeof(ExtendedElementShape), typeof(ShapeAndImageView), default(ExtendedElementShape));
        /// <summary>
        /// Gets/Sets the ExtendedElementShape property
        /// </summary>
        ExtendedElementShape IExtendedShape.ExtendedElementShape
        {
            get => (ExtendedElementShape)GetValue(ExtendedElementShapeProperty);
            set => SetValue(ExtendedElementShapeProperty, value);
        }
        #endregion ExtendedElementShape property

        #region ExtendedElementSeparatorWidth
        public static readonly BindableProperty ExtendedElementSeparatorWidthProperty = ShapeBase.ExtendedElementSeparatorWidthProperty;
        float IExtendedShape.ExtendedElementSeparatorWidth
        {
            get => (float)GetValue(ExtendedElementSeparatorWidthProperty);
            set => SetValue(ExtendedElementSeparatorWidthProperty, value);
        }
        #endregion ExtendedElementSeparatorWidth

        #region ParentSegmentsOrientation
        public static readonly BindableProperty ParentSegmentsOrientationProperty = ShapeBase.ParentSegmentsOrientationProperty;
        public StackOrientation ParentSegmentsOrientation
        {
            get => (StackOrientation)GetValue(ParentSegmentsOrientationProperty);
            set => SetValue(ParentSegmentsOrientationProperty, value);
        }
        #endregion ParentSegmentsOrientation



        #region IShape


        #region BackgroundColor
        /// <summary>
        /// Backing store for the BackgroundColor property
        /// </summary>
        public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof(Color), typeof(Button), Color.Default);
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
        public static new readonly BindableProperty HasShadowProperty = BindableProperty.Create("HasShadow", typeof(bool), typeof(Button), false);
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
        public static new readonly BindableProperty OutlineColorProperty = BindableProperty.Create("OutlineColor", typeof(Color), typeof(Button), Color.Default);
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
        public static new readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("OutlineWidth", typeof(float), typeof(Button), -1f);
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
        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(Button), null);
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
        public static readonly BindableProperty HtmlTextProperty = BindableProperty.Create("HtmlText", typeof(string), typeof(Button), null);
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
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(Button), Color.Default);
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
        public static BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create("HorizontalTextAlignment", typeof(TextAlignment), typeof(Button), TextAlignment.Center);
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
        public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create("VerticalTextAlignment", typeof(TextAlignment), typeof(Button), TextAlignment.Center);
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
        public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create("LineBreakMode", typeof(LineBreakMode), typeof(Button), LineBreakMode.WordWrap);
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
        public static readonly BindableProperty AutoFitProperty = BindableProperty.Create("AutoFit", typeof(AutoFit), typeof(Button), AutoFit.Width);
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
        public static readonly BindableProperty LinesProperty = BindableProperty.Create("Lines", typeof(int), typeof(Button), 1);
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
        public static readonly BindableProperty MinFontSizeProperty = BindableProperty.Create("MinFontSize", typeof(double), typeof(Button), -1.0);
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
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create("FontSize", typeof(double), typeof(Button), -1.0);//, BindingMode.OneWay), null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontSizePropertyChanged));
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
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create("FontFamily", typeof(string), typeof(Button), null);//, BindingMode.OneWay), null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontFamilyPropertyChanged)); 
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
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create("FontAttributes", typeof(FontAttributes), typeof(Button), FontAttributes.None);//, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontAttributesPropertyChanged));
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
        public static readonly BindableProperty DarkThemeProperty = BindableProperty.Create("DarkTheme", typeof(bool), typeof(Button), false);
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
                int childCount = 0;
                foreach (var child in Children)
                {
                    if (child is VisualElement element && element.IsVisible)
                    {
                        childCount++;
                        if (Orientation == StackOrientation.Horizontal)
                        {

                            var measure = element.Measure(double.PositiveInfinity, double.PositiveInfinity);
                            //width += element.Width;
                            //height = Math.Max(height, element.Height);
                            width += measure.Request.Width;
                            height += measure.Request.Height;

                        }
                        else
                        {
                            var measure = element.Measure(double.PositiveInfinity, double.PositiveInfinity);
                            //width = Math.Max(width, element.Width);
                            //height += element.Height;
                            width += measure.Request.Width;
                            height += measure.Request.Height;
                        }
                    }
                }
                if (childCount > 1)
                {
                    width += Orientation == StackOrientation.Horizontal ? Spacing : 0;
                    height += Orientation == StackOrientation.Vertical ? Spacing : 0;

                }
                width += Padding.HorizontalThickness + Margin.HorizontalThickness;
                height += Padding.VerticalThickness + Margin.VerticalThickness;

                return new Size(width, height);
            }
        }
        #endregion


        #region Internal, SegmentedButton Properties
        /*
        /// <summary>
        /// 
        /// </summary>
        internal static BindableProperty SegmentTypeProperty = BindableProperty.Create("SegmentType", typeof(ExtendedElementShape), typeof(Button), ExtendedElementShape.Rectangle);
        internal ExtendedElementShape SegmentType
        {
            get { return (ExtendedElementShape)GetValue(SegmentTypeProperty); }
            set { SetValue(SegmentTypeProperty, value); }
        }
        */
        /*
        internal static BindableProperty ParentSegmentsOrientationProperty = BindableProperty.Create("ParentSegmentsOrientation", typeof(StackOrientation), typeof(Button), StackOrientation.Horizontal);
        internal StackOrientation ParentSegmentsOrientation
        {
            get => (StackOrientation)GetValue(ParentSegmentsOrientationProperty);
            set => SetValue(ParentSegmentsOrientationProperty, value);
        }
                */

        internal static BindableProperty SeparatorWidthProperty = BindableProperty.Create("SeparatorWidth", typeof(float), typeof(Button), -1f);
        internal float SeparatorWidth
        {
            get => (float)GetValue(SeparatorWidthProperty);
            set => SetValue(SeparatorWidthProperty, value);
        }

        internal static BindableProperty GroupToggleBehaviorProperty = BindableProperty.Create("GroupToggleBehavior", typeof(GroupToggleBehavior), typeof(Button), GroupToggleBehavior.None);
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
        internal protected Xamarin.Forms.StackLayout _stackLayout;
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
            Padding = new Thickness(8, 6, 8, 6);
            OutlineRadius = 2;
            _label = new Label
            {
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                //HeightRequest = 22,
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
            _gestureListener.LongPressed += OnLongPressed;
            _gestureListener.LongPressing += OnLongPressing;


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
            bool isIconText = false;

            assembly = assembly ?? AssemblyExtensions.AssemblyFromResourceId(icon);
            if (assembly == null && Device.RuntimePlatform != Device.UWP)
                assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);
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

        /// <summary>
        /// Dispose of Forms9Patch.Button object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
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
            if (this is StateButton)
                return;
            _noUpdate = true;

            var enabledLabelColor = TextColor == Color.Default ? (DarkTheme ? Color.White : Color.FromHex("#000").WithAlpha(0.5)) : TextColor;
            if (IsSelected && SelectedTextColor.A > 0)
                enabledLabelColor = SelectedTextColor;

            base.OutlineWidth = OutlineWidth < 0 ? (BackgroundImage?.Source != null ? 0 : (BackgroundColor.A > 0 ? 0 : 1)) : (OutlineColor == Color.Transparent ? 0 : OutlineWidth);

            if (IsEnabled)
            {
                //System.Diagnostics.Debug.WriteLine("Caller: [" + callerName + "." + lineNumber + "] _label.TextColor=[" + enabledLabelColor + "]");

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
                    //base.OutlineColor = BackgroundColor.A > 0 ? base.BackgroundColor : Color.FromHex (DarkTheme? "#FFF" : "#000").WithAlpha (0.5);
                    base.OutlineColor = enabledLabelColor; // _label.TextColor;//Color.FromHex (DarkTheme? "#FFF" : "#000").WithAlpha (0.5);

                base.HasShadow = (base.BackgroundColor.A > 0 || BackgroundImage?.Source != null) && HasShadow;
                //var hasShadow = base.BackgroundColor.A > 0 && HasShadow;
                //SetValue(ShapeBase.HasShadowProperty, hasShadow);
                ShadowInverted = IsSelected && !this.IsSegment();
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
                //if (ElementShape == ExtendedElementShape.Rectangle)
                if (!this.IsSegment())
                {
                    base.HasShadow = false;
                    base.BackgroundColor = BackgroundColor.A > 0 ? opaque : transp;
                    //base.OutlineColor = OutlineColor.A > 0 ? opaque : base.BackgroundColor;
                    base.OutlineColor = OutlineColor == Color.Default || OutlineColor.A > 0 ? opaque : BackgroundColor;
                }
                else
                {
                    // the next line is already covered above
                    //base.OutlineWidth = OutlineWidth < 0 ? (BackgroundColor.A > 0 ? 0 : 1) : OutlineColor == Color.Transparent ? 0 : OutlineWidth;
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
                    //base.OutlineColor = TextColor == Color.Default ? (DarkTheme ? Color.White : Color.FromHex("#000").WithAlpha(0.5)) : TextColor;
                    if (OutlineColor == Color.Default)
                        //base.OutlineColor = BackgroundColor.A > 0 ? base.BackgroundColor : Color.FromHex (DarkTheme? "#FFF" : "#000").WithAlpha (0.5);
                        base.OutlineColor = enabledLabelColor; //_label.TextColor;//Color.FromHex (DarkTheme? "#FFF" : "#000").WithAlpha (0.5);
                }
                //base.OutlineColor = _label.TextColor;
                //base.OutlineColor = OutlineColor.A > 0 ? opaque : _label.TextColor;//base.BackgroundColor;
                //base.OutlineWidth = base.OutlineWidth > 0
            }
            UpdateIconTint();
            _noUpdate = false;
        }

        internal void UpdateIconTint()
        {
            if (_iconImage != null)
                _iconImage.TintColor = TintIcon ? _label.TextColor : Color.Default;
            if (_iconLabel != null)
            {
                _iconLabel.TextColor = _label.TextColor;
                _iconLabel.FontSize = _label.FontSize;
            }
        }
        #endregion


        #region Events

        bool IsEnabledCore
        {
            set
            {
                //this.SetValueCore(VisualElement.IsEnabledProperty, (object) (bool) (value ? true : false), Xamarin.Forms.BindableObject.SetValueFlags.None);
                SetValue(IsEnabledProperty, value);
            }
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

        void OnLabelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            if (propertyName == Label.TextProperty.PropertyName || propertyName == Label.HtmlTextProperty.PropertyName)
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
            else if (propertyName == Label.TextColorProperty.PropertyName)
            {
                UpdateIconTint();
            }

            //else if (propertyName == Label.FittedFontSizeProperty.PropertyName)
            //    _actualFontSizeChanged?.Invoke(this, EventArgs.Empty);
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
            // this causes the text to shift ... it would have worked if the size of the frame increased proportionally.
            /*
            var padding = OutlineWidth / Display.Scale;
            if (!this.IsSegment() || ((IShape)this).ExtendedElementShape == ExtendedElementShape.SegmentEnd)
                _stackLayout.Padding = padding;
            else
            {
                if (ParentSegmentsOrientation == StackOrientation.Horizontal)
                    _stackLayout.Padding = new Thickness(padding, padding, 0, padding);
                else
                    _stackLayout.Padding = new Thickness(padding, padding, padding, 0);

            }
            //_stackLayout.BackgroundColor = Color.Pink;
            */
        }

        void SetOrienations()
        {
            var horzOption = HorizontalTextAlignment.ToLayoutOptions();
            var vertOption = VerticalTextAlignment.ToLayoutOptions();
            if (Orientation == StackOrientation.Horizontal)
            {
                if (HasTightSpacing)
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
                        _iconLabel.VerticalOptions = LayoutOptions.Fill; // vertOption; // LayoutOptions.Fill;// LayoutOptions.FillAndExpand;
                    }
                    if (_label != null)
                    {
                        _label.HorizontalTextAlignment = HorizontalTextAlignment;
                        _label.VerticalTextAlignment = VerticalTextAlignment;
                        _label.HorizontalOptions = LayoutOptions.Start;
                        _label.VerticalOptions = LayoutOptions.Fill; // vertOption; // LayoutOptions.Fill; // LayoutOptions.FillAndExpand;
                        _label.MinimizeHeight = false;
                    }
                    _stackLayout.HorizontalOptions = horzOption;
                    //_stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                }
                else
                {
                    if (_iconImage != null)
                    {
                        _iconImage.HorizontalOptions = LayoutOptions.Center; // (TrailingIcon ? LayoutOptions.End : LayoutOptions.Start);
                        _iconImage.VerticalOptions = vertOption;
                    }
                    if (_iconLabel != null)
                    {
                        /*
                        _iconLabel.HorizontalTextAlignment = TextAlignment.Center;
                        _iconLabel.VerticalTextAlignment = VerticalTextAlignment;
                        _iconLabel.HorizontalOptions = (TrailingIcon ? LayoutOptions.End : LayoutOptions.Start);
                        _iconLabel.VerticalOptions = vertOption; // LayoutOptions.FillAndExpand;*/
                        _iconLabel.HorizontalTextAlignment = TextAlignment.Center;
                        _iconLabel.VerticalTextAlignment = VerticalTextAlignment;
                        _iconLabel.HorizontalOptions = LayoutOptions.Center;
                        _iconLabel.VerticalOptions = LayoutOptions.Fill; // vertOption; // LayoutOptions.Fill;// LayoutOptions.FillAndExpand;
                    }
                    if (_label != null)
                    {
                        /*
                        _label.HorizontalTextAlignment = HorizontalTextAlignment;
                        _label.HorizontalOptions = LayoutOptions.FillAndExpand;
                        _label.VerticalTextAlignment = VerticalTextAlignment;
                        _label.VerticalOptions = vertOption; // LayoutOptions.FillAndExpand;
                        _label.MinimizeHeight = false;
                        //_label.BackgroundColor = Color.Green;
                        */
                        _label.HorizontalTextAlignment = HorizontalTextAlignment;
                        _label.VerticalTextAlignment = VerticalTextAlignment;
                        _label.HorizontalOptions = LayoutOptions.FillAndExpand;
                        _label.VerticalOptions = LayoutOptions.Fill; // vertOption; // LayoutOptions.Fill; // LayoutOptions.FillAndExpand;
                        _label.MinimizeHeight = false;
                    }
                    _stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                }
                _stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
                //_label.BackgroundColor = Color.Orange.WithAlpha(0.5);
                //_stackLayout.BackgroundColor = Color.Green;
            }
            else
            {
                if (HasTightSpacing)
                {
                    if (_iconImage != null)
                    {
                        _iconImage.HorizontalOptions = horzOption;// LayoutOptions.CenterAndExpand;
                        _iconImage.VerticalOptions = LayoutOptions.Center;
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
                    if (_iconImage != null)
                    {
                        _iconImage.HorizontalOptions = horzOption;// LayoutOptions.CenterAndExpand;
                        _iconImage.VerticalOptions = (TrailingIcon ? LayoutOptions.End : LayoutOptions.Start);
                    }
                    if (_iconLabel != null)
                    {
                        _iconLabel.VerticalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalTextAlignment = TextAlignment.Center;
                        _iconLabel.HorizontalOptions = horzOption; // LayoutOptions.CenterAndExpand;
                        _iconLabel.VerticalOptions = (TrailingIcon ? LayoutOptions.End : LayoutOptions.Start);
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
            base.OnPropertyChanged(propertyName);

            if (propertyName == BorderWidthProperty.PropertyName)
                OutlineWidth = (float)BorderWidth;
            //else if (propertyName == BorderRadiusProperty.PropertyName)
            //    OutlineRadius = BorderRadius;
            //else if (propertyName == BorderColorProperty.PropertyName)
            //    OutlineColor = BorderColor;

            if (_noUpdate)
                return;

            if (propertyName == OutlineWidthProperty.PropertyName
                || propertyName == ParentSegmentsOrientationProperty.PropertyName
                || propertyName == ExtendedElementShapeProperty.PropertyName
               )
                SetStackLayoutPadding();
            if (propertyName == HorizontalTextAlignmentProperty.PropertyName || propertyName == VerticalTextAlignmentProperty.PropertyName)
            {
                //_stackLayout.HorizontalOptions = Alignment.ToLayoutOptions();
                SetOrienations();
            }
            else if (propertyName == IconImageProperty.PropertyName)
            {
                if (_changingIconText)
                    return;
                _changingIconImage = true;
                if (_iconImage != null)
                    _stackLayout.Children.Remove(_iconImage);
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
                        _stackLayout.Children.Remove(_iconLabel);
                        _iconLabel.HtmlText = null;
                        _iconLabel.Text = null;
                        IconText = null;
                    }
                    _iconImage = IconImage;
                    //_iconImage.Fill = Fill.AspectFit;
                    //_image.TintColor = TintIcon ? _label.TextColor : Color.Default;
                    UpdateIconTint();
                    if (_iconImage != null)
                    {
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
                    _stackLayout.Children.Remove(_iconLabel);
                if (IconText != null)
                {
                    if (_iconImage != null)
                        _stackLayout.Children.Remove(_iconImage);
                    _iconLabel = new Label
                    {
                        HtmlText = IconText,
                        TextColor = _label.TextColor,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        Lines = 1,
                        FontSize = FontSize,
                        AutoFit = AutoFit.None,
                    };
                    if (_iconLabel != null)
                    {
                        if (TrailingIcon)
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
                || propertyName == BackgroundImageProperty.PropertyName
                || propertyName == HasShadowProperty.PropertyName
                || propertyName == ShadowInvertedProperty.PropertyName
                || propertyName == OutlineColorProperty.PropertyName
                || propertyName == OutlineWidthProperty.PropertyName
                || (propertyName == SelectedBackgroundColorProperty.PropertyName && IsSelected)
                || propertyName == IsSelectedProperty.PropertyName
                || propertyName == IsEnabledProperty.PropertyName
                || propertyName == DarkThemeProperty.PropertyName
                || propertyName == ExtendedElementShapeProperty.PropertyName
                    )
            {
                //System.Diagnostics.Debug.WriteLine("PropertyName: " + propertyName);
                UpdateElements();
            }
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


