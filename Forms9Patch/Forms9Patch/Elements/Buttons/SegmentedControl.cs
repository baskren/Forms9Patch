using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// DEPRICATED: USE SegmentedControl
    /// </summary>
    [Obsolete("DEPRICATED: Use SegmentedControl")]
    public class MaterialSegmentedControl : SegmentedControl
    {
    }

    /// <summary>
    /// Forms9Patch Material Segmented Control.
    /// </summary>
    [ContentProperty(nameof(Segments))]
    public class SegmentedControl : Forms9Patch.ManualLayout, ILabelStyle, IDisposable
    {
        #region Obsolete Properties
        /// <summary>
        /// Use TextColorProperty
        /// </summary>
        [Obsolete("Use TextColorProperty")]
        public static readonly BindableProperty FontColorProperty = BindableProperty.Create(nameof(FontColor), typeof(Color), typeof(SegmentedControl), Color.Default, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is SegmentedControl control && newValue is Color color)
                control.TextColor = color;
        });
        /// <summary>
        /// OBSOLETE: Use TextColor
        /// </summary>
        /// <value>The color of the font.</value>
        [Obsolete("Use TextColor")]
        public Color FontColor
        {
            get { return (Color)GetValue(FontColorProperty); }
            set { SetValue(FontColorProperty, value); }
        }

        /// <summary>
        /// OBSOLETE: Use SelectedTextColorProperty
        /// </summary>
        [Obsolete("Use SelectedTextColorProperty")]
        public static readonly BindableProperty SelectedFontColorProperty = BindableProperty.Create(nameof(SelectedFontColor), typeof(Color), typeof(SegmentedControl), Color.Default, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is SegmentedControl control && newValue is Color color)
                control.SelectedTextColor = color;
        });
        /// <summary>
        /// OBSOLETE: Use SelectedTextColor property
        /// </summary>
        /// <value>The color of the selected font.</value>
        [Obsolete("Use SelectedTextColor")]
        public Color SelectedFontColor
        {
            get { return (Color)GetValue(SelectedFontColorProperty); }
            set { SetValue(SelectedFontColorProperty, value); }
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
        /// OBSOLETE: Use TrailingIconProperty
        /// </summary>
        [Obsolete("Use TrailingIconProperty")]
        public static readonly BindableProperty TrailingImageProperty = BindableProperty.Create(nameof(TrailingIcon), typeof(bool), typeof(MaterialSegmentedControl), false, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is MaterialSegmentedControl control && newValue is bool value)
                control.TrailingIcon = value;
        });
        /// <summary>
        /// OBSOLETE: Use TrailingIcon
        /// </summary>
        [Obsolete("Use TrailingIcon")]
        public bool TrailingImage
        {
            get { return (bool)GetValue(TrailingImageProperty); }
            set { SetValue(TrailingImageProperty, value); }
        }


        /// <summary>
        /// OBSOLETE: Use TintIconProperty
        /// </summary>
        [Obsolete("Use TintIconProperty", true)]
        public static readonly BindableProperty TintImageProperty = BindableProperty.Create(nameof(TintImage), typeof(bool), typeof(MaterialSegmentedControl), false, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is MaterialSegmentedControl control && newValue is bool value)
                control.TintIcon = value;
        });
        /// <summary>
        /// OBSOLETE: Use TintIcon property
        /// </summary>
        [Obsolete("Use TintIcon", true)]
        public bool TintImage
        {
            get { return (bool)GetValue(TintImageProperty); }
            set { SetValue(TintImageProperty, value); }
        }

        /// <summary>
        /// Do not sue!
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use Segments", true)]
        public VisualElement Content
        {
            get => throw new NotSupportedException("Forms9Patch.SegmentControl: Content is not a supported property.  Use Segments property instead");
            set => throw new NotSupportedException("Forms9Patch.SegmentControl: Content is not a supported property.  Use Segments property instead");
        }
        #endregion


        #region Properties


        #region IsClipped property
        internal static readonly BindablePropertyKey IsClippedPropertyKey = BindableProperty.CreateReadOnly(nameof(IsClipped), typeof(bool), typeof(SegmentedControl), default(bool));
        /// <summary>
        /// Backing store for the IsClipped property
        /// </summary>
        public static readonly BindableProperty IsClippedProperty = IsClippedPropertyKey.BindableProperty;
        /// <summary>
        /// Gets or sets a value indicating whether any of the contents of this <see cref="T:Forms9Patch.SegmentedControl"/> is clipped.
        /// </summary>
        /// <value><c>true</c> if is clipped; otherwise, <c>false</c>.</value>
        public bool IsClipped
        {
            get => (bool)GetValue(IsClippedProperty);
            internal set => SetValue(IsClippedPropertyKey, value);
        }
        #endregion IsClipped property

        #region FontScaling property
        /// <summary>
        /// The size segment fonts equally property.
        /// </summary>
        public static readonly BindableProperty SyncSegmentFontSizesProperty = BindableProperty.Create(nameof(SyncSegmentFontSizes), typeof(bool), typeof(SegmentedControl), true);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.SegmentedControl"/> size segment fonts equally.
        /// </summary>
        /// <value><c>true</c> if size segment fonts equally; otherwise, <c>false</c>.</value>
        public bool SyncSegmentFontSizes
        {
            get => (bool)GetValue(SyncSegmentFontSizesProperty);
            set => SetValue(SyncSegmentFontSizesProperty, value);
        }
        #endregion FontScaling property

        #region Segments
        internal readonly ObservableCollection<Segment> _segments;
        /// <summary>
        /// The container for the Segmented Control's buttons.
        /// </summary>
        /// <value>The buttons.</value>
        public IList<Segment> Segments
        {
            get { return _segments; }
            set
            {
                UpdatingSegments = true;
                _segments.Clear();
                if (value != null)
                {
                    foreach (var segment in value)
                        _segments.Add(segment);
                }
                UpdatingSegments = false;
            }
        }

        int _updatingCount;
        bool UpdatingSegments
        {
            get => _updatingCount > 0;
            set
            {
                _updatingCount += value ? 1 : -1;
                if (_updatingCount <= 0)
                {
                    _updatingCount = 0;
                    UpdateChildrenPadding();
                    InvalidateLayout();
                }
            }
        }
        #endregion

        #region Padding
        /// <summary>
        /// Identifies the Padding bindable property.
        /// </summary>
        /// <remarks></remarks>
        public static new readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(SegmentedControl), new Thickness(2));
        /// <summary>
        /// Gets or sets the padding for SegmentedControl's segments.
        /// </summary>
        /// <value>The Thickness values for the layout. The default value is a Thickness with all values set to 0.</value>
        public new Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
        #endregion

        #region SelectedTextColor
        /// <summary>
        /// The selected text color property.
        /// </summary>
        public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create(nameof(SelectedTextColor), typeof(Color), typeof(SegmentedControl), Color.Default);
        /// <summary>
        /// Gets or sets the color of the selected font.
        /// </summary>
        /// <value>The color of the selected font.</value>
        public Color SelectedTextColor
        {
            get => (Color)GetValue(SelectedTextColorProperty);
            set => SetValue(SelectedTextColorProperty, value);
        }
        #endregion

        #region FontAttributes
        /// <summary>
        /// Backing store for the Button.FontAttributes bindable property.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(SegmentedControl), FontAttributes.None);

        /// <summary>
        /// Gets or sets the font attributes.
        /// </summary>
        /// <value>The font attributes.</value>
        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }
        #endregion

        #region FontSize
        /// <summary>
        /// Backing store for the Button.FontSize bindable property.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(SegmentedControl), -1.0);

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        #endregion

        #region FontFamily
        /// <summary>
        /// Backing store for the Button.FontFamiily bindable property.
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(SegmentedControl), null);

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>The font family.</value>
        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }
        #endregion

        #region IconFontFamiliy property
        /// <summary>
        /// Backing store for IconFontFamily property
        /// </summary>
        public static readonly BindableProperty IconFontFamilyProperty = BindableProperty.Create(nameof(IconFontFamily), typeof(string), typeof(SegmentedControl), default(string));
        /// <summary>
        /// Sets the font used for rendering the IconText 
        /// </summary>
        public string IconFontFamily
        {
            get => (string)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        #endregion IconFontFamiliy property

        #region BackgroundColor
        /// <summary>
        /// Backing store for the Button.BackgroundColor bindable property.
        /// </summary>
        public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SegmentedControl), Color.Transparent);
        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        #endregion

        #region SelectedBackgroundColor
        /// <summary>
        /// Backing store for the Selected.BackgroundColor property.
        /// </summary>
        public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create(nameof(SelectedBackgroundColor), typeof(Color), typeof(SegmentedControl), Color.Transparent);
        /// <summary>
        /// Gets or sets the background color used when selected.
        /// </summary>
        /// <value>The selected background.</value>
        public Color SelectedBackgroundColor
        {
            get => (Color)GetValue(SelectedBackgroundColorProperty);
            set => SetValue(SelectedBackgroundColorProperty, value);
        }
        #endregion

        #region DarkThemem
        /// <summary>
        /// Backing store for the Button.DarkTheme property.
        /// </summary>
        public static readonly BindableProperty DarkThemeProperty = BindableProperty.Create(nameof(DarkTheme), typeof(bool), typeof(SegmentedControl), false);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Button"/> if for a dark theme.
        /// </summary>
        /// <value><c>true</c> if dark theme; otherwise, <c>false</c>.</value>
        public bool DarkTheme
        {
            get => (bool)GetValue(DarkThemeProperty);
            set => SetValue(DarkThemeProperty, value);
        }
        #endregion

        #region SelectedSegments
        /// <summary>
        /// Gets the selected segments(s).
        /// </summary>
        /// <value>The selected segment(s).</value>
        public List<Segment> SelectedSegments
        {
            get
            {
                var results = new List<Segment>();
                foreach (var segment in _segments)
                    if (segment.IsSelected)
                        results.Add(segment);
                return results;
            }
        }
        #endregion

        #region SelectedIndexes
        /// <summary>
        /// Gets the selected index(es).
        /// </summary>
        /// <value>The selected index(es).</value>
        public List<int> SelectedIndexes
        {
            get
            {
                var results = new List<int>();
                for (int i = 0; i < _segments.Count; i++)
                    if (_segments[i].IsSelected)
                        results.Add(i);
                return results;
            }
        }
        #endregion


        /// <summary>
        /// Answers the question: Is an index selected?
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsIndexSelected(int index)
        {
            return index >= 0 && index < _segments.Count && _segments[index].IsSelected;
        }

        #region IsLongPressEnabled property
        /// <summary>
        /// The is long press enabled property.
        /// </summary>
        public static readonly BindableProperty IsLongPressEnabledProperty = BindableProperty.Create(nameof(IsLongPressEnabled), typeof(bool), typeof(SegmentedControl), true);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.SegmentedControl"/> has long press enabled.
        /// </summary>
        /// <value><c>true</c> if is long press enabled; otherwise, <c>false</c>.</value>
        public bool IsLongPressEnabled
        {
            get => (bool)GetValue(IsLongPressEnabledProperty);
            set => SetValue(IsLongPressEnabledProperty, value);
        }
        #endregion IsLongPressEnabled property

        #region GroupToggleBehavior
        /// <summary>
        /// The backing store for the MaterialSegmentControl's ToggleBehavior property.
        /// </summary>
        public static readonly BindableProperty GroupToggleBehaviorProperty = BindableProperty.Create(nameof(GroupToggleBehavior), typeof(GroupToggleBehavior), typeof(SegmentedControl), GroupToggleBehavior.Radio);
        /// <summary>
        /// Gets or sets the MaterialSegmentControl's Toggle behavior.
        /// </summary>
        /// <value>The Toggle behavior (None, Radio, Multiselect).</value>
        public GroupToggleBehavior GroupToggleBehavior
        {
            get => (GroupToggleBehavior)GetValue(GroupToggleBehaviorProperty);
            set => SetValue(GroupToggleBehaviorProperty, value);
        }
        #endregion

        #region SeparatorWidth
        /// <summary>
        /// The backing store for the MaterialSegmentControl's SeparatorWidth property.
        /// </summary>
        public static readonly BindableProperty SeparatorWidthProperty = BindableProperty.Create(nameof(SeparatorWidth), typeof(float), typeof(SegmentedControl), -1f);
        /// <summary>
        /// Gets or sets the width of the separator.  Uses OutlineWidth by default (-1).
        /// </summary>
        /// <value>The width of the separator.</value>
        public float SeparatorWidth
        {
            get => (float)GetValue(SeparatorWidthProperty);
            set => SetValue(SeparatorWidthProperty, value);
        }
        #endregion

        #region TrailingIcon
        /// <summary>
        /// Backing store for the trailing image property.
        /// </summary>
        public static readonly BindableProperty TrailingIconProperty = BindableProperty.Create(nameof(TrailingIcon), typeof(bool), typeof(SegmentedControl), default(bool));
        /// <summary>
        /// Gets or sets if the image is to be rendered after the text.
        /// </summary>
        /// <value>default=false</value>
        public bool TrailingIcon
        {
            get => (bool)GetValue(TrailingIconProperty);
            set => SetValue(TrailingIconProperty, value);
        }
        #endregion

        #region HapticEffect
        /// <summary>
        /// The haptic effect property.
        /// </summary>
        public static readonly BindableProperty HapticEffectProperty = BindableProperty.Create(nameof(HapticEffect), typeof(HapticEffect), typeof(SegmentedControl), default(HapticEffect));
        /// <summary>
        /// Gets or sets the haptic effect.
        /// </summary>
        /// <value>The haptic effect.</value>
        public HapticEffect HapticEffect
        {
            get => (HapticEffect)GetValue(HapticEffectProperty);
            set => SetValue(HapticEffectProperty, value);
        }
        #endregion

        #region HapticEffectMode
        /// <summary>
        /// The haptic mode property.
        /// </summary>
        public static readonly BindableProperty HapticEffectModeProperty = BindableProperty.Create(nameof(HapticEffectMode), typeof(EffectMode), typeof(SegmentedControl), default(EffectMode));
        /// <summary>
        /// Gets or sets the haptic mode.
        /// </summary>
        /// <value>The haptic mode.</value>
        public EffectMode HapticEffectMode
        {
            get => (EffectMode)GetValue(HapticEffectModeProperty);
            set => SetValue(HapticEffectModeProperty, value);
        }
        #endregion

        #region SoundEffect property
        /// <summary>
        /// The backing store for the sound effect property.
        /// </summary>
        public static readonly BindableProperty SoundEffectProperty = BindableProperty.Create(nameof(SoundEffect), typeof(SoundEffect), typeof(SegmentedControl), default(SoundEffect));
        /// <summary>
        /// Gets or sets the sound effect played when a segment is tapped
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
        /// Backing store for the sound effect mode property.
        /// </summary>
        public static readonly BindableProperty SoundEffectModeProperty = BindableProperty.Create(nameof(SoundEffectMode), typeof(EffectMode), typeof(SegmentedControl), default(EffectMode));
        /// <summary>
        /// Gets or sets the sound effect is perfomed when a segment is tapped
        /// </summary>
        /// <value>The sound effect mode.</value>
        public EffectMode SoundEffectMode
        {
            get => (EffectMode)GetValue(SoundEffectModeProperty);
            set => SetValue(SoundEffectModeProperty, value);
        }
        #endregion SoundEffectMode property

        #region TintIcon
        /// <summary>
        /// The tint image property backing store.
        /// </summary>
        public static readonly BindableProperty TintIconProperty = BindableProperty.Create(nameof(TintIcon), typeof(bool), typeof(SegmentedControl), true);
        /// <summary>
        /// Will the TextColor be applied to the IconImage image?
        /// </summary>
        /// <value><c>true</c> tint IconImage image with TextColor; otherwise, <c>false</c>.</value>
        public bool TintIcon
        {
            get => (bool)GetValue(TintIconProperty);
            set => SetValue(TintIconProperty, value);
        }
        #endregion

        #region HasTightSpacing
        /// <summary>
        /// The has tight spacing property.
        /// </summary>
        public static readonly BindableProperty HasTightSpacingProperty = BindableProperty.Create(nameof(HasTightSpacing), typeof(bool), typeof(SegmentedControl), default(bool));
        /// <summary>
        /// Gets or sets if the Icon/Image is close (TightSpacing) to text or at edge (not TightSpacing) of button.
        /// </summary>
        /// <value><c>true</c> if has tight spacing; otherwise, <c>false</c>.</value>
        public bool HasTightSpacing
        {
            get => (bool)GetValue(HasTightSpacingProperty);
            set => SetValue(HasTightSpacingProperty, value);
        }
        #endregion

        #region IntraSegmentOrientation
        /// <summary>
        /// The backing store for the segments orientation property.
        /// </summary>
        public static readonly BindableProperty IntraSegmentOrientationProperty = BindableProperty.Create(nameof(IntraSegmentOrientation), typeof(StackOrientation), typeof(SegmentedControl), StackOrientation.Horizontal);
        /// <summary>
        /// Gets or sets the orientation of elements within the segments.
        /// </summary>
        /// <value>The orientation of the elements within the segments.</value>
        public StackOrientation IntraSegmentOrientation
        {
            get => (StackOrientation)GetValue(IntraSegmentOrientationProperty);
            set => SetValue(IntraSegmentOrientationProperty, value);
        }
        #endregion

        #region IntraSegmentSpacing
        /// <summary>
        /// The backing store for the intra segment spacing property.
        /// </summary>
        public static readonly BindableProperty IntraSegmentSpacingProperty = BindableProperty.Create(nameof(IntraSegmentSpacing), typeof(double), typeof(SegmentedControl), default(double));
        /// <summary>
        /// Gets or sets the intra segment spacing.
        /// </summary>
        /// <value>The intra segment spacing.</value>
        public double IntraSegmentSpacing
        {
            get => (double)GetValue(IntraSegmentSpacingProperty);
            set => SetValue(IntraSegmentSpacingProperty, value);
        }
        #endregion

        #region Orientation
        /// <summary>
        /// Backing store for the Orienation property
        /// </summary>
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(Xamarin.Forms.StackOrientation), typeof(SegmentedControl), StackOrientation.Horizontal);
        /// <summary>
        /// controls the orientation of the segments relative to eachother
        /// </summary>
        public Xamarin.Forms.StackOrientation Orientation
        {
            get => (Xamarin.Forms.StackOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        #endregion

        #region ILabelStyle

        #region TextColor
        /// <summary>
        /// Backing store for the TextColor bindable property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SegmentedControl), Color.Default);
        /// <summary>
        /// Gets or sets the color of the font.
        /// </summary>
        /// <value>The color of the font.</value>
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        #endregion

        #region HorizontalTextAlignment
        /// <summary>
        /// Backing store for the horizontal text alignment property.
        /// </summary>
        public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(SegmentedControl), TextAlignment.Center);
        /// <summary>
        /// Gets or sets the horizontal text alignment.
        /// </summary>
        /// <value>The horizontal text alignment.</value>
        public TextAlignment HorizontalTextAlignment
        {
            get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
            set => SetValue(HorizontalTextAlignmentProperty, value);
        }
        #endregion

        #region VerticalTextAlignment
        /// <summary>
        /// Backing store for the vertical text alignment property.
        /// </summary>
        public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create(nameof(VerticalTextAlignment), typeof(TextAlignment), typeof(SegmentedControl), TextAlignment.Center);
        /// <summary>
        /// Gets or sets the vertical text alignment.
        /// </summary>
        /// <value>The vertical text alignment.</value>
        public TextAlignment VerticalTextAlignment
        {
            get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
            set => SetValue(VerticalTextAlignmentProperty, value);
        }
        #endregion

        #region LineBreakMode property
        /// <summary>
        /// backing store for LineBreakMode property
        /// </summary>
        public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create(nameof(LineBreakMode), typeof(LineBreakMode), typeof(SegmentedControl), LineBreakMode.WordWrap);
        /// <summary>
        /// Gets/Sets the LineBreakMode property
        /// </summary>
        public LineBreakMode LineBreakMode
        {
            get => (LineBreakMode)GetValue(LineBreakModeProperty);
            set => SetValue(LineBreakModeProperty, value);
        }
        #endregion LineBreakMode property

        #region AutoFit property
        /// <summary>
        /// backing store for AutoFit property
        /// </summary>
        public static readonly BindableProperty AutoFitProperty = BindableProperty.Create(nameof(AutoFit), typeof(AutoFit), typeof(SegmentedControl), AutoFit.Width);
        /// <summary>
        /// Gets/Sets the AutoFit property
        /// </summary>
        public AutoFit AutoFit
        {
            get => (AutoFit)GetValue(AutoFitProperty);
            set => SetValue(AutoFitProperty, value);
        }
        #endregion AutoFit property

        #region Lines property
        /// <summary>
        /// backing store for Lines property
        /// </summary>
        public static readonly BindableProperty LinesProperty = BindableProperty.Create(nameof(Lines), typeof(int), typeof(SegmentedControl), 1);
        /// <summary>
        /// Gets/Sets the Lines property
        /// </summary>
        public int Lines
        {
            get => (int)GetValue(LinesProperty);
            set => SetValue(LinesProperty, value);
        }
        #endregion Lines property

        #region MinFontSize property
        /// <summary>
        /// backing store for MinFontSize property
        /// </summary>
        public static readonly BindableProperty MinFontSizeProperty = BindableProperty.Create(nameof(MinFontSize), typeof(double), typeof(SegmentedControl), -1.0);
        /// <summary>
        /// Gets/Sets the MinFontSize property
        /// </summary>
        public double MinFontSize
        {
            get => (double)GetValue(MinFontSizeProperty);
            set => SetValue(MinFontSizeProperty, value);
        }
        #endregion MinFontSize property

        #endregion

        #endregion


        #region Fields
        readonly SegmentedControlBackground _background = new SegmentedControlBackground();
        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentedControl"/> class.
        /// </summary>
        public SegmentedControl()
        {
            IgnoreChildren = true;
            base.Padding = new Thickness(0);
            OutlineRadius = 2;
            OutlineWidth = 1;
            Orientation = StackOrientation.Horizontal;
            _segments = new ObservableCollection<Segment>();
            _segments.CollectionChanged += OnCollectionChanged;
            SizeChanged += (sender, e) =>
            {
                // This should not be necessary because the segments will have already been layed out again, driving any IsClipped setting;
                //System.Diagnostics.Debug.WriteLine("SegmentedControl.SizeChanged ENTER");
                //IsClipped = CheckIsClipped();
                //System.Diagnostics.Debug.WriteLine("SegmentedControl.SizeChanged EXIT");
                // rather ...
            };
            Children.Add(_background);
        }

        #endregion


        #region IDisposable Support
        bool _disposed; // To detect redundant calls

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                Children.Remove(_background);
                foreach (var segment in _segments)
                {
                    var button = segment._button;
                    button.PropertyChanged -= OnButtonPropertyChanged;
                    Children.Remove(button);
                    button.Tapped -= OnSegmentTapped;
                    button.Selected -= OnSegmentSelected;
                    button.LongPressing -= OnSegmentLongPressing;
                    button.LongPressed -= OnSegmentLongPressed;
                }
                _segments.CollectionChanged -= OnCollectionChanged;
                _segments.Clear();
            }
        }

        /// <summary>
        /// Dispose of Forms9Patch.SegmentedControl element.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        #region Segment settings
        void UpdateSegment(Segment segment)
        {
            var button = segment._button;
            button.DarkTheme = DarkTheme;
            button.BackgroundColor = BackgroundColor;
            button.SelectedBackgroundColor = SelectedBackgroundColor;
            button.OutlineColor = OutlineColor;
            button.OutlineRadius = OutlineRadius;
            button.OutlineWidth = OutlineWidth;
            button.FontFamily = FontFamily;
            button.IconFontFamily = IconFontFamily;
            button.FontSize = FontSize;
            button.HasShadow = HasShadow;
            button.ExtendedElementShapeOrientation = Orientation;
            button.Orientation = IntraSegmentOrientation;
            button.ExtendedElementSeparatorWidth = SeparatorWidth;
            button.ToggleBehavior = (GroupToggleBehavior != GroupToggleBehavior.None);
            button.GroupToggleBehavior = GroupToggleBehavior;
            button.HorizontalOptions = LayoutOptions.FillAndExpand;
            button.VerticalOptions = LayoutOptions.FillAndExpand;
            if (segment.TextColor == Color.Default)
                segment._button.TextColor = TextColor;
            button.SelectedTextColor = SelectedTextColor;
            button.TintIcon = TintIcon;
            button.HasTightSpacing = HasTightSpacing;
            button.HorizontalTextAlignment = HorizontalTextAlignment;
            button.VerticalTextAlignment = VerticalTextAlignment;
            button.LineBreakMode = LineBreakMode;
            button.AutoFit = AutoFit;
            button.Lines = Lines;
            button.MinFontSize = MinFontSize;
            if (!segment.FontAttributesSet)
                segment._button.FontAttributes = FontAttributes;
            button.TrailingIcon = TrailingIcon;
            button.HapticEffectMode = HapticEffectMode;
            button.HapticEffect = HapticEffect;
            button.SoundEffect = SoundEffect;
            button.SoundEffectMode = SoundEffectMode;
            button.Spacing = IntraSegmentSpacing;
            button.IsLongPressEnabled = IsLongPressEnabled;
            segment.Parent = this;
            segment.BindingContext = BindingContext;
        }
        #endregion


        #region Collection management
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnCollectionChanged(sender, e));
                return;
            }

            int index;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    index = e.NewStartingIndex;
                    if (e.NewItems != null)
                        foreach (Segment newItem in e.NewItems)
                            InsertSegment(index++, newItem);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                        foreach (Segment oldItem in e.OldItems)
                            RemoveSegment(oldItem);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    for (int i = Children.Count - 1; i >= 0; i--)
                    {
                        if (Children[i] is SegmentButton button)
                            RemoveButton(button);
                    }
                    break;
            }
            var count = Children.Count;
            if (count > 2)
            {
                ((IExtendedShape)Children[1]).ExtendedElementShape = ExtendedElementShape.SegmentStart;
                for (int i = 2; i < count - 1; i++)
                    ((IExtendedShape)Children[i]).ExtendedElementShape = ExtendedElementShape.SegmentMid;
                ((IExtendedShape)Children[count - 1]).ExtendedElementShape = ExtendedElementShape.SegmentEnd;
            }
            else if (count == 2)
            {
                ((IExtendedShape)Children[1]).ExtendedElementShape = ExtendedElementShape.Rectangle;
            }
            if (!UpdatingSegments)
            {
                UpdateChildrenPadding();
                InvalidateLayout();
            }
        }

        void InsertSegment(int index, Segment s)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => InsertSegment(index, s));
                return;
            }

            var button = s._button;
            UpdateSegment(s);
            button.PropertyChanged += OnButtonPropertyChanged;
            button.Tapped += OnSegmentTapped;
            button.Selected += OnSegmentSelected;
            button.LongPressing += OnSegmentLongPressing;
            button.LongPressed += OnSegmentLongPressed;
            button.FittedFontSizeChanged += OnButtonFittedFontSizeChanged;
            Children.Insert(index + 1, button);
            if (button.IsSelected && GroupToggleBehavior == GroupToggleBehavior.Radio)
            {
                foreach (var segment in _segments)
                    if (segment != s)
                        segment.IsSelected = false;
            }
        }

        void RemoveSegment(Segment s)
        {
            var button = s._button;
            RemoveButton(button);
        }

        void RemoveButton(SegmentButton button)
        {
            if (button == null)
                return;
            button.PropertyChanged -= OnButtonPropertyChanged;
            Children.Remove(button);
            button.Tapped -= OnSegmentTapped;
            button.Selected -= OnSegmentSelected;
            button.LongPressing -= OnSegmentLongPressing;
            button.LongPressed -= OnSegmentLongPressed;
            button.FittedFontSizeChanged -= OnButtonFittedFontSizeChanged;
        }


        void UpdateChildrenPadding()
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(UpdateChildrenPadding);
                return;
            }

            foreach (var child in Children)
                if (child is SegmentButton button)
                    button.Padding = Padding;
        }
        #endregion


        #region Change management
        /// <summary>
        /// Taps the index.
        /// </summary>
        /// <param name="index">Index.</param>
        public void TapIndex(int index)
        {
            if (index >= 0 && index < _segments.Count)
                _segments[index].Tap();
        }


        /// <summary>
        /// Selects the segment at index.
        /// </summary>
        /// <param name="index">segment index.</param>
        public void SelectIndex(int index)
        {
            if (index >= 0 && index < _segments.Count)
                _segments[index].IsSelected = true;
        }

        /// <summary>
        /// Deselected the segment at index
        /// </summary>
        /// <param name="index"></param>
        public void DeselectIndex(int index)
        {
            if (index >= 0 && index < _segments.Count)
                _segments[index].IsSelected = false;
        }

        /// <summary>
        /// Selects all segments
        /// </summary>
        public void SelectAll()
        {
            foreach (var segment in _segments)
                segment.IsSelected = true;
        }

        /// <summary>
        /// Deselects all segments.
        /// </summary>
        public void DeselectAll()
        {
            foreach (var segment in _segments)
                segment.IsSelected = false;
        }

        /// <summary>
        /// Called when BindingContext is changed
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            foreach (var segment in _segments)
                segment.BindingContext = BindingContext;
        }

        /// <param name="propertyName">The name of the property that changed.</param>
        /// <summary>
        /// Call this method from a child class to notify that a change happened on a property.
        /// </summary>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            base.OnPropertyChanged(propertyName);

            if (Segments != null)
            {
                if (propertyName == GroupToggleBehaviorProperty.PropertyName)
                    foreach (Segment segment in Segments)
                    {
                        segment._button.ToggleBehavior = (GroupToggleBehavior != GroupToggleBehavior.None);
                        segment._button.GroupToggleBehavior = GroupToggleBehavior;
                    }
                else if (propertyName == PaddingProperty.PropertyName)
                    UpdateChildrenPadding();
                else if (propertyName == TextColorProperty.PropertyName)
                {
                    foreach (Segment segment in Segments)
                        if (segment.TextColor == Color.Default)
                            segment._button.TextColor = TextColor;
                }
                else if (propertyName == SelectedTextColorProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.SelectedTextColor = SelectedTextColor;
                else if (propertyName == TintIconProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.TintIcon = TintIcon;
                else if (propertyName == HasTightSpacingProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.HasTightSpacing = HasTightSpacing;
                else if (propertyName == HorizontalTextAlignmentProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.HorizontalTextAlignment = HorizontalTextAlignment;
                else if (propertyName == VerticalTextAlignmentProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.VerticalTextAlignment = VerticalTextAlignment;
                else if (propertyName == LineBreakModeProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.LineBreakMode = LineBreakMode;
                else if (propertyName == AutoFitProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.AutoFit = AutoFit;
                else if (propertyName == LinesProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.Lines = Lines;
                else if (propertyName == MinFontSizeProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.MinFontSize = MinFontSize;
                else if (propertyName == FontAttributesProperty.PropertyName)
                {
                    foreach (Segment segment in Segments)
                        if (!segment.FontAttributesSet)
                            segment._button.FontAttributes = FontAttributes;

                }
                else if (propertyName == DarkThemeProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.DarkTheme = DarkTheme;
                else if (propertyName == BackgroundColorProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.BackgroundColor = BackgroundColor;
                else if (propertyName == SelectedBackgroundColorProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.SelectedBackgroundColor = SelectedBackgroundColor;
                else if (propertyName == OutlineColorProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.OutlineColor = OutlineColor;
                else if (propertyName == OutlineRadiusProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.OutlineRadius = OutlineRadius;
                else if (propertyName == OutlineWidthProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.OutlineWidth = OutlineWidth;
                else if (propertyName == FontFamilyProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.FontFamily = FontFamily;
                else if (propertyName == IconFontFamilyProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.IconFontFamily = FontFamily;
                else if (propertyName == FontSizeProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.FontSize = FontSize;
                else if (propertyName == HasShadowProperty.PropertyName || propertyName == Xamarin.Forms.Frame.HasShadowProperty.PropertyName)
                {
                    foreach (Segment segment in Segments)
                        segment._button.HasShadow = HasShadow;
                    InvalidateLayout();
                }
                else if (propertyName == OrientationProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.ExtendedElementShapeOrientation = Orientation;
                else if (propertyName == IntraSegmentOrientationProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.Orientation = IntraSegmentOrientation;
                else if (propertyName == SeparatorWidthProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.ExtendedElementSeparatorWidth = SeparatorWidth;
                else if (propertyName == TrailingIconProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.TrailingIcon = TrailingIcon;
                else if (propertyName == HapticEffectProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.HapticEffect = HapticEffect;
                else if (propertyName == HapticEffectModeProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.HapticEffectMode = HapticEffectMode;
                else if (propertyName == SoundEffectProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.SoundEffect = SoundEffect;
                else if (propertyName == SoundEffectModeProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.SoundEffectMode = SoundEffectMode;
                else if (propertyName == IsEnabledProperty.PropertyName)
                {
                    if (IsEnabled)
                        Opacity *= 2;
                    else
                        Opacity /= 2;
                }
                else if (propertyName == IntraSegmentSpacingProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.Spacing = IntraSegmentSpacing;
                else if (propertyName == IsLongPressEnabledProperty.PropertyName)
                    foreach (Segment segment in Segments)
                        segment._button.IsLongPressEnabled = IsLongPressEnabled;
                else if (propertyName == WidthProperty.PropertyName || propertyName == HeightProperty.PropertyName)
                    CheckIsClipped();
            }
        }

        //Segment _lastSelectedSegment;

        internal void OnButtonPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is SegmentButton button)
            {
                if (e.PropertyName == Forms9Patch.Button.IsSelectedProperty.PropertyName)
                {
                    if (GroupToggleBehavior == GroupToggleBehavior.Radio)
                    {
                        if (button.IsSelected)
                        {
                            foreach (var seg in _segments)
                                seg.IsSelected = seg._button == button;
                        }
                        else if (button.IsEnabled)
                        {
                            var enabled = 0;
                            Segment eseg = null;
                            foreach (var seg in _segments)
                            {
                                if (seg.IsEnabled)
                                {
                                    enabled++;
                                    eseg = seg;
                                }
                            }
                            eseg.IsSelected = eseg.IsSelected || enabled == 1;
                        }
                    }
                    /*
                    else if (GroupToggleBehavior == GroupToggleBehavior.Multiselect)
                    {
                        foreach (var seg in _segments)
                            if (seg.Button == button)
                                seg.IsSelected = !seg.IsSelected;
                    }
                    */
                }
                else if (e.PropertyName == Forms9Patch.Button.IsClippedProperty.PropertyName)
                {
                    if (button.IsClipped)
                        IsClipped = true;
                    else
                        IsClipped = CheckIsClipped();
                    return;
                }
            }
        }
        #endregion


        #region FontSize Synchronization
        DateTime _lastFontSizeResetTime = DateTime.MinValue;
        static int _iterations;
        bool _waitingForThingsToCalmDown;
        private void OnButtonFittedFontSizeChanged(object sender, double e)
        {
            if (!SyncSegmentFontSizes)
                return;
            _lastFontSizeResetTime = DateTime.Now;
            if (!_waitingForThingsToCalmDown)
            {
                _waitingForThingsToCalmDown = true;
                Device.StartTimer(TimeSpan.FromMilliseconds(30), () =>
                 {
                     if (DateTime.Now - _lastFontSizeResetTime > TimeSpan.FromMilliseconds(100))
                     {
                         var iteration = _iterations++;
                         var maxFittedFontSize = -1.0;
                         var minFittedFontSize = double.MaxValue;
                         var maxSyncFontSize = double.MinValue;
                         var minSyncFontSize = double.MaxValue;
                         //bool debug = false; // (_segments[0]._button.Text == "BACKGROUND" || _segments[0]._button.Text == "H1");
                         foreach (var segment in _segments)
                         {
                             var segmentFittedFontSize = segment._button.FittedFontSize;
                             //if (debug)
                             //    System.Diagnostics.Debug.WriteLine("\t[" + iteration + "][" + InstanceId + "][" + segment._button.LabelInstanceId + "] segmentFittedFontSize=[" + segmentFittedFontSize + "] segmentSyncFontSize=[" + segment._button.SynchronizedFontSize + "] txt=[" + (segment.Text ?? segment.HtmlText) + "]");

                             if (segmentFittedFontSize < minFittedFontSize && segmentFittedFontSize > 0)
                                 minFittedFontSize = segment._button.FittedFontSize;
                             if (segmentFittedFontSize > maxFittedFontSize)
                                 maxFittedFontSize = segmentFittedFontSize;

                             var segmentSyncFontSize = segment._button.SynchronizedFontSize;
                             if (minSyncFontSize - segmentSyncFontSize > 1)
                                 minSyncFontSize = segmentSyncFontSize;
                             if (segmentSyncFontSize - maxSyncFontSize > 1)
                                 maxSyncFontSize = segmentSyncFontSize;
                         }
                         //if (debug)
                         //    System.Diagnostics.Debug.WriteLine("\t[" + iteration + "][" + InstanceId + "] maxSync=[" + maxSyncFontSize + "] minSync=[" + minSyncFontSize + "] maxOpt=[" + maxFittedFontSize + "] minOpt=[" + minFittedFontSize + "]");

                         if (minFittedFontSize >= double.MaxValue / 3)
                             minFittedFontSize = -1;

                         foreach (var segment in _segments)
                         {
                             ((ILabel)segment._button).SynchronizedFontSize = minFittedFontSize;
                             // if (debug)
                             //     System.Diagnostics.Debug.WriteLine("\t[" + iteration + "][" + InstanceId + "][" + segment._button.LabelInstanceId + "] SynchronizedFontSize=[" + minFittedFontSize + "] txt=[" + (segment.Text ?? segment.HtmlText) + "]");
                         }
                         _waitingForThingsToCalmDown = false;
                         return false;
                     }
                     return true;
                 });
            }
        }
        #endregion


        #region Xamarin Layout
        /// <summary>
        /// Processes a measurement request
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            //return base.OnMeasure(widthConstraint, heightConstraint);
            var hz = Orientation == StackOrientation.Horizontal;

            var shadowPadding = new Thickness(0);
            if (HasShadow && BackgroundColor.A > 0 && Children.Count > 1)
                shadowPadding = ShapeBase.ShadowPadding(this);

            var requestHeight = shadowPadding.VerticalThickness;
            var requestWidth = shadowPadding.HorizontalThickness;
            var minHeight = shadowPadding.VerticalThickness;
            var minWidth = shadowPadding.HorizontalThickness;

            if (hz)//&& (double.IsInfinity(widthConstraint) || double.IsNaN(widthConstraint)) )
            {
                // we have all the width in the world ... but what's the height?
                foreach (var child in Children)
                    if (child is SegmentButton button)
                    {
                        var childSizeRequest = button.Measure(double.PositiveInfinity, heightConstraint, MeasureFlags.None);
                        //System.Diagnostics.Debug.WriteLine("\tOnMeasure childSizeRequest["+child.Id+"]=["+childSizeRequest+"]");
                        if (childSizeRequest.Request.Height > requestHeight)
                            requestHeight = childSizeRequest.Request.Height;
                        if (childSizeRequest.Minimum.Height > minHeight)
                            minHeight = childSizeRequest.Minimum.Height;
                        requestWidth += childSizeRequest.Request.Width;
                        minWidth += childSizeRequest.Minimum.Width;
                    }
            }
            else // if (vt)// && (double.IsInfinity(heightConstraint) || double.IsNaN(heightConstraint)))
            {
                // we have all the height in the world ... but what's the width?
                foreach (var child in Children)
                    if (child is SegmentButton button)
                    {
                        var childSizeRequest = button.Measure(widthConstraint, double.PositiveInfinity, MeasureFlags.None);
                        //System.Diagnostics.Debug.WriteLine("\tOnMeasure childSizeRequest[" + child.Id + "]=[" + childSizeRequest + "]");
                        if (childSizeRequest.Request.Width > requestWidth)
                            requestWidth = childSizeRequest.Request.Width;
                        if (childSizeRequest.Minimum.Width > minWidth)
                            minWidth = childSizeRequest.Minimum.Width;
                        requestHeight += childSizeRequest.Request.Height;
                        minHeight += childSizeRequest.Minimum.Height;
                    }
            }
            return new SizeRequest(new Size(requestWidth, requestHeight), new Size(minWidth, minHeight));
        }

        /// <param name="x">A value representing the x coordinate of the child region bounding box.</param>
        /// <param name="y">A value representing the y coordinate of the child region bounding box.</param>
        /// <param name="width">A value representing the width of the child region bounding box.</param>
        /// <param name="height">A value representing the height of the child region bounding box.</param>
        /// <summary>
        /// Positions and sizes the children of a Layout.
        /// </summary>
        /// <remarks>Implementors wishing to change the default behavior of a Layout should override this method. It is suggested to
        /// still call the base method and modify its calculated results.</remarks>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            //if (Segments.Any(s => s.HtmlText.StartsWith("Lateral")))
            //    System.Diagnostics.Debug.WriteLine(GetType() + ".");
            if (!UpdatingSegments)
            {
                LayoutChildIntoBoundingRegion(_background, new Rectangle(x, y, width, height));
                LayoutFunction(x, y, width, height, LayoutSegment);
            }
        }

        private static bool LayoutSegment(View view, Rectangle segmentRect, object arg3)
        {
            //if (view is Button button && (button.HtmlText?.StartsWith("Lateral") ?? false))
            //    System.Diagnostics.Debug.WriteLine(".");
            LayoutChildIntoBoundingRegion(view, segmentRect);
            return false;
        }

        internal bool LayoutFunction(double x, double y, double width, double height, Func<View, Rectangle, object, bool> layoutAction, object parameter = null)
        {
            var shadowPadding = new Thickness(0);
            if (HasShadow && BackgroundColor.A > 0)
                shadowPadding = ShapeBase.ShadowPadding(this);

            var hz = Orientation == StackOrientation.Horizontal;
            var vt = !hz;
            var newWidth = width - shadowPadding.HorizontalThickness;
            var newHeight = height - shadowPadding.VerticalThickness;


            var count = Children.Count - 1;

            if (count > 0)
            {

                //x = Math.Round(x);
                //y = Math.Round(y);

                var outlineWidth = OutlineWidth;// / Display.Scale;
                var xOffset = hz ? outlineWidth + (newWidth - outlineWidth * (count + 1)) / count : 0;
                var yOffset = vt ? outlineWidth + (newHeight - outlineWidth * (count + 1)) / count : 0;
                var segmentWidth = hz ? xOffset : width;
                var segmentHeight = vt ? yOffset : height;

                for (int i = 0; i <= count; i++)
                {
                    if (Children[i] is SegmentButton button && button.IsVisible)
                    {
                        double thisW, thisH;
                        if (i == 1)
                        {
                            thisW = segmentWidth + (hz ? shadowPadding.Left : 0);
                            thisH = segmentHeight + (vt ? shadowPadding.Top : 0);
                        }
                        else if (i == count)
                        {
                            thisW = hz ? width - x : segmentWidth;
                            thisH = vt ? height - y : segmentHeight;
                        }
                        else
                        {
                            thisW = segmentWidth;
                            thisH = segmentHeight;
                        }

                        if (x + thisW > width)
                            thisW = width - x;
                        if (y + thisH > height)
                            thisH = height - y;
                        var segmentRect = new Rectangle(x, y, thisW, thisH);
                        //LayoutChildIntoBoundingRegion(view, segmentRect);
                        //layoutAction.Invoke(segmentRect);

                        //if (!P42.Utils.Environment.IsOnMainThread)
                        //    Device.BeginInvokeOnMainThread(() => layoutAction.Invoke(view, segmentRect));
                        //else
                        if (layoutAction.Invoke(button, segmentRect, parameter))
                            return true;

                        x = (x + (hz ? thisW : 0));
                        y = (y + (vt ? thisH : 0));
                    }
                }
            }
            return false;
        }
        #endregion


        #region Event management
        /// <summary>
        /// tests if the contents of a segment is clipped
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

            var result = LayoutFunction(0, 0, width, height, CheckSegmentIsClipped);
            return result;
        }

        private static bool CheckSegmentIsClipped(View view, Rectangle segmentRect, object arg3)
        {
            if (view is Button button)
                return button.CheckIsClipped(segmentRect.Width, segmentRect.Height);
            return false;
        }




        /// <summary>
        /// Occurs when one of the segments is tapped.
        /// </summary>
        public event SegmentedControlEventHandler SegmentTapped;
        void OnSegmentTapped(object sender, EventArgs e)
        {
            for (int i = 0; i < _segments.Count; i++)
            {
                var button = _segments[i]._button;
                if (button.Equals(sender))
                {
                    SegmentTapped?.Invoke(this, new SegmentedControlEventArgs(i, _segments[i]));
                    return;
                }
            }
        }

        /// <summary>
        /// Occurs when one of the segments is selected.
        /// </summary>
        public event SegmentedControlEventHandler SegmentSelected;
        void OnSegmentSelected(object sender, EventArgs e)
        {
            for (int i = 0; i < _segments.Count; i++)
            {
                var button = _segments[i]._button;
                if (button.Equals(sender) && button.IsSelected)
                {
                    SegmentSelected?.Invoke(this, new SegmentedControlEventArgs(i, _segments[i]));
                    return;
                }
            }
        }

        /// <summary>
        /// Occurs when segment long pressing.
        /// </summary>
        public event SegmentedControlEventHandler SegmentLongPressing;
        void OnSegmentLongPressing(object sender, EventArgs e)
        {
            for (int i = 0; i < _segments.Count; i++)
            {
                var button = _segments[i]._button;
                if (button.Equals(sender))
                {
                    SegmentLongPressing?.Invoke(this, new SegmentedControlEventArgs(i, _segments[i]));
                    return;
                }
            }
        }

        /// <summary>
        /// Occurs when segment long pressed.
        /// </summary>
        public event SegmentedControlEventHandler SegmentLongPressed;
        void OnSegmentLongPressed(object sender, EventArgs e)
        {
            for (int i = 0; i < _segments.Count; i++)
            {
                var button = _segments[i]._button;
                if (button.Equals(sender))
                {
                    SegmentLongPressed?.Invoke(this, new SegmentedControlEventArgs(i, _segments[i]));
                    return;
                }
            }
        }

        #endregion

        /*
        public void HardForceLayout()
        {
            LayoutChildren(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
        }
        */

    }


}

