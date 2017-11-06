using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Material Segmented Control.
    /// </summary>
    [ContentProperty("Segments")]
    public class MaterialSegmentedControl : Forms9Patch.ManualLayout, IDisposable
    {

        #region Properties
        readonly ObservableCollection<Segment> _segments;
        /// <summary>
        /// The container for the Segmented Control's buttons.
        /// </summary>
        /// <value>The buttons.</value>
        public IList<Segment> Segments
        {
            get { return _segments; }
        }

        /// <summary>
        /// Identifies the Padding bindable property.
        /// </summary>
        /// <remarks></remarks>
        public static new readonly BindableProperty PaddingProperty = BindableProperty.Create("Padding", typeof(Thickness), typeof(MaterialSegmentedControl), new Thickness(4));
        /// <summary>
        /// Gets or sets the padding for SegmentedControl's segments.
        /// </summary>
        /// <value>The Thickness values for the layout. The default value is a Thickness with all values set to 0.</value>
        public new Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }



        /// <summary>
        /// Backing store for the MaterialButton.FontColor bindable property.
        /// </summary>
        public static readonly BindableProperty FontColorProperty = BindableProperty.Create("FontColor", typeof(Color), typeof(MaterialSegmentedControl), Color.Default);
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
        /// The selected font color property.
        /// </summary>
        public static readonly BindableProperty SelectedFontColorProperty = BindableProperty.Create("SelectedFontColor", typeof(Color), typeof(MaterialSegmentedControl), Color.Default);
        /// <summary>
        /// Gets or sets the color of the selected font.
        /// </summary>
        /// <value>The color of the selected font.</value>
        public Color SelectedFontColor
        {
            get { return (Color)GetValue(SelectedFontColorProperty); }
            set { SetValue(SelectedFontColorProperty, value); }
        }


        /// <summary>
        /// Backing store for the MaterialButton.FontAttributes bindable property.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create("FontAttributes", typeof(FontAttributes), typeof(MaterialSegmentedControl), FontAttributes.None);//, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontAttributesPropertyChanged));
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
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create("FontSize", typeof(double), typeof(MaterialSegmentedControl), -1.0);//, BindingMode.OneWay), null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontSizePropertyChanged));
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
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create("FontFamily", typeof(string), typeof(MaterialSegmentedControl), null);//, BindingMode.OneWay), null, new BindableProperty.BindingPropertyChangedDelegate (ButtonState.FontFamilyPropertyChanged)); 
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
        /// Backing store for the MaterialButton.BackgroundColor bindable property.
        /// </summary>
        public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof(Color), typeof(MaterialSegmentedControl), Color.Transparent);
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
        /// Backing store for the Selected.BackgroundColor property.
        /// </summary>
        public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create("SelectedBackgroundColor", typeof(Color), typeof(MaterialSegmentedControl), Color.Transparent);
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
        public static readonly BindableProperty DarkThemeProperty = BindableProperty.Create("DarkTheme", typeof(bool), typeof(MaterialSegmentedControl), false);
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

        /// <summary>
        /// OBSOLETE: Use ToggleBehaviorProperty instead.
        /// </summary>
        [Obsolete("StickyBehavior property is obsolete, use ToggleBehavior instead")]
        public static BindableProperty StickyBehaviorProperty = null;

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
        /// The backing store for the MaterialSegmentControl's ToggleBehavior property.
        /// </summary>
        public static readonly BindableProperty GroupToggleBehaviorProperty = BindableProperty.Create("GroupToggleBehavior", typeof(GroupToggleBehavior), typeof(MaterialSegmentedControl), GroupToggleBehavior.Radio);
        /// <summary>
        /// Gets or sets the MaterialSegmentControl's Toggle behavior.
        /// </summary>
        /// <value>The Toggle behavior (None, Radio, Multiselect).</value>
        public GroupToggleBehavior GroupToggleBehavior
        {
            get { return (GroupToggleBehavior)GetValue(GroupToggleBehaviorProperty); }
            set { SetValue(GroupToggleBehaviorProperty, value); }
        }

        /// <summary>
        /// The backing store for the MaterialSegmentControl's SeparatorWidth property.
        /// </summary>
        public static readonly BindableProperty SeparatorWidthProperty = BindableProperty.Create("SeparatorWidth", typeof(float), typeof(MaterialSegmentedControl), -1f);
        /// <summary>
        /// Gets or sets the width of the separator.  Uses OutlineWidth by default (-1).
        /// </summary>
        /// <value>The width of the separator.</value>
        public float SeparatorWidth
        {
            get { return (float)GetValue(SeparatorWidthProperty); }
            set { SetValue(SeparatorWidthProperty, value); }
        }

        /// <summary>
        /// Backing store for the trailing image property.
        /// </summary>
        public static readonly BindableProperty TrailingImageProperty = BindableProperty.Create("TrailingImage", typeof(bool), typeof(MaterialSegmentedControl), default(bool));
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
        /// The haptic effect property.
        /// </summary>
        public static readonly BindableProperty HapticEffectProperty = BindableProperty.Create("HapticEffect", typeof(HapticEffect), typeof(MaterialSegmentedControl), default(HapticEffect));
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
        public static readonly BindableProperty HapticModeProperty = BindableProperty.Create("HapticMode", typeof(KeyClicks), typeof(MaterialSegmentedControl), default(KeyClicks));
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
        public static readonly BindableProperty TintImageProperty = BindableProperty.Create("TintImage", typeof(bool), typeof(MaterialSegmentedControl), true);
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
        /// Backing store for the horizontal text alignment property.
        /// </summary>
        public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create("HorizontalTextAlignment", typeof(TextAlignment), typeof(MaterialSegmentedControl), TextAlignment.Center);
        /// <summary>
        /// Gets or sets the horizontal text alignment.
        /// </summary>
        /// <value>The horizontal text alignment.</value>
        public TextAlignment HorizontalTextAlignment
        {
            get { return (TextAlignment)GetValue(HorizontalTextAlignmentProperty); }
            set { SetValue(HorizontalTextAlignmentProperty, value); }
        }

        /// <summary>
        /// Backing store for the vertical text alignment property.
        /// </summary>
        public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create("VerticalTextAlignment", typeof(TextAlignment), typeof(MaterialSegmentedControl), TextAlignment.Center);
        /// <summary>
        /// Gets or sets the vertical text alignment.
        /// </summary>
        /// <value>The vertical text alignment.</value>
        public TextAlignment VerticalTextAlignment
        {
            get { return (TextAlignment)GetValue(VerticalTextAlignmentProperty); }
            set { SetValue(VerticalTextAlignmentProperty, value); }
        }

        /// <summary>
        /// The backing store for the segments orientation property.
        /// </summary>
        public static readonly BindableProperty IntraSegmentOrientationProperty = BindableProperty.Create("IntraSegmentOrientation", typeof(StackOrientation), typeof(MaterialSegmentedControl), StackOrientation.Horizontal);
        /// <summary>
        /// Gets or sets the orientation of elements within the segments.
        /// </summary>
        /// <value>The orientation of the elements within the segments.</value>
        public StackOrientation IntraSegmentOrientation
        {
            get { return (StackOrientation)GetValue(IntraSegmentOrientationProperty); }
            set { SetValue(IntraSegmentOrientationProperty, value); }
        }

        /// <summary>
        /// The backing store for the intra segment spacing property.
        /// </summary>
        public static readonly BindableProperty IntraSegmentSpacingProperty = BindableProperty.Create("IntraSegmentSpacing", typeof(double), typeof(MaterialSegmentedControl), default(double));
        /// <summary>
        /// Gets or sets the intra segment spacing.
        /// </summary>
        /// <value>The intra segment spacing.</value>
        public double IntraSegmentSpacing
        {
            get { return (double)GetValue(IntraSegmentSpacingProperty); }
            set { SetValue(IntraSegmentSpacingProperty, value); }
        }

        public static readonly BindableProperty OrientationProperty = BindableProperty.Create("Orientation", typeof(Xamarin.Forms.StackOrientation), typeof(MaterialSegmentedControl), StackOrientation.Horizontal);
        public Xamarin.Forms.StackOrientation Orientation
        {
            get { return (Xamarin.Forms.StackOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialSegmentedControl"/> class.
        /// </summary>
        public MaterialSegmentedControl()
        {
            IgnoreChildren = true;
            //Spacing = -.5 * Display.Scale;
            base.Padding = new Thickness(0);
            //Padding = new Thickness(2);
            //Spacing = 0;
            OutlineRadius = 2;
            OutlineWidth = 1;
            Orientation = StackOrientation.Horizontal;
            _segments = new ObservableCollection<Segment>();
            _segments.CollectionChanged += OnCollectionChanged;
            //base.BackgroundColor = Color.Red;
        }

        #endregion


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

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
                    foreach (var segment in _segments)
                    {
                        var button = segment.MaterialButton;
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

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MaterialSegmentedControl() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        /// <summary>
        /// Releases all resource used by the <see cref="T:Forms9Patch.MaterialSegmentedControl"/> object.
        /// </summary>
        /// <remarks>Call <see cref="O:Forms9Patch.MaterialSegmentedControl.Dispose"/> when you are finished using the <see cref="T:Forms9Patch.MaterialSegmentedControl"/>.
        /// The <see cref="O:Forms9Patch.MaterialSegmentedControl.Dispose"/> method leaves the <see cref="T:Forms9Patch.MaterialSegmentedControl"/> in an unusable
        /// state. After calling <see cref="O:Forms9Patch.MaterialSegmentedControl.Dispose"/>, you must release all references to the
        /// <see cref="T:Forms9Patch.MaterialSegmentedControl"/> so the garbage collector can reclaim the memory that the
        /// <see cref="T:Forms9Patch.MaterialSegmentedControl"/> was occupying.</remarks>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


        #region Segment settings
        void UpdateSegment(Segment segment)
        {
            var button = segment.MaterialButton;
            button.DarkTheme = DarkTheme;
            button.BackgroundColor = BackgroundColor;
            button.SelectedBackgroundColor = SelectedBackgroundColor;
            button.OutlineColor = OutlineColor;
            button.OutlineRadius = OutlineRadius;
            button.OutlineWidth = OutlineWidth;
            button.FontFamily = FontFamily;
            button.FontSize = FontSize;
            button.HasShadow = HasShadow;
            button.ParentSegmentsOrientation = Orientation;
            button.Orientation = IntraSegmentOrientation;
            button.SeparatorWidth = SeparatorWidth;
            button.ToggleBehavior = (GroupToggleBehavior != GroupToggleBehavior.None);
            button.GroupToggleBehavior = GroupToggleBehavior;
            button.HorizontalOptions = LayoutOptions.FillAndExpand;
            button.VerticalOptions = LayoutOptions.FillAndExpand;
            if (segment.FontColor == Color.Default)
                segment.MaterialButton.FontColor = FontColor;
            button.SelectedFontColor = SelectedFontColor;
            button.TintImage = TintImage;
            button.HasTightSpacing = HasTightSpacing;
            button.HorizontalTextAlignment = HorizontalTextAlignment;
            button.VerticalTextAlignment = VerticalTextAlignment;
            if (!segment.FontAttributesSet)
                segment.MaterialButton.FontAttributes = FontAttributes;
            button.TrailingImage = TrailingImage;
            button.HapticMode = HapticMode;
            button.HapticEffect = HapticEffect;
            button.Spacing = IntraSegmentSpacing;
        }
        #endregion


        #region Collection management
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
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
                case NotifyCollectionChangedAction.Replace:
                    // not used?
                    throw new NotImplementedException();
                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();
                case NotifyCollectionChangedAction.Reset:
                    for (int i = Children.Count - 1; i >= 0; i--)
                        RemoveButton(Children[i] as MaterialButton);
                    break;
            }
            int count = Children.Count;
            if (count > 1)
            {
                ((MaterialButton)Children[0]).SegmentType = ButtonShape.SegmentStart;
                for (int i = 1; i < count - 1; i++)
                    ((MaterialButton)Children[i]).SegmentType = ButtonShape.SegmentMid;
                ((MaterialButton)Children[count - 1]).SegmentType = ButtonShape.SegmentEnd;
            }
            else if (count == 1)
            {
                ((MaterialButton)Children[0]).SegmentType = ButtonShape.Rectangle;
            }
            UpdateChildrenPadding();
        }

        void InsertSegment(int index, Segment s)
        {
            var button = s.MaterialButton;
            UpdateSegment(s);
            button.PropertyChanged += OnButtonPropertyChanged;
            button.Tapped += OnSegmentTapped;
            button.Selected += OnSegmentSelected;
            button.LongPressing += OnSegmentLongPressing;
            button.LongPressed += OnSegmentLongPressed;
            Children.Insert(index, button);
            if (button.IsSelected && GroupToggleBehavior == GroupToggleBehavior.Radio)
            {
                foreach (var segment in _segments)
                    if (segment != s)
                        segment.IsSelected = false;
            }
        }

        void RemoveSegment(Segment s)
        {
            var button = s.MaterialButton;
            RemoveButton(button);
        }

        void RemoveButton(MaterialButton button)
        {
            if (button == null)
                return;
            button.PropertyChanged -= OnButtonPropertyChanged;
            Children.Remove(button);
            button.Tapped -= OnSegmentTapped;
            button.Selected -= OnSegmentSelected;
            button.LongPressing -= OnSegmentLongPressing;
            button.LongPressed -= OnSegmentLongPressed;
        }

        void UpdateChildrenPadding()
        {
            if (Orientation == StackOrientation.Horizontal)
            {
                foreach (MaterialButton child in Children)
                    child.Padding = Padding;
            }
            else
            {

                foreach (MaterialButton child in Children)
                {
                    double plaformTweek = 0;
                    switch (Device.OS)
                    //switch (Device.RuntimePlatform)
                    {
                        //case Device.iOS:
                        case TargetPlatform.iOS:
                            plaformTweek = 0.1;
                            break;
                        //case Device.Android:
                        case TargetPlatform.Android:
                            plaformTweek = -1;
                            break;
                    }
                    switch (child.SegmentType)
                    {
                        case ButtonShape.SegmentStart:
                            child.Padding = new Thickness(Padding.Left, Padding.Top + plaformTweek, Padding.Right, Padding.Bottom - plaformTweek);
                            break;
                        case ButtonShape.SegmentMid:
                            child.Padding = new Thickness(Padding.Left, Padding.Top + plaformTweek, Padding.Right, Padding.Bottom - plaformTweek);
                            break;
                        default:
                            child.Padding = Padding;
                            break;
                    }
                }

            }
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
        /// Deselects all segments.
        /// </summary>
        public void DeselectAll()
        {
            foreach (var segment in _segments)
                segment.IsSelected = false;
        }

        /// <param name="propertyName">The name of the property that changed.</param>
        /// <summary>
        /// Call this method from a child class to notify that a change happened on a property.
        /// </summary>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == GroupToggleBehaviorProperty.PropertyName)
                foreach (MaterialButton button in Children)
                {
                    button.ToggleBehavior = (GroupToggleBehavior != GroupToggleBehavior.None);
                    button.GroupToggleBehavior = GroupToggleBehavior;
                }
            else if (propertyName == PaddingProperty.PropertyName)
                UpdateChildrenPadding();
            else if (propertyName == FontColorProperty.PropertyName)
            {
                if (Segments != null)
                    foreach (Segment segment in Segments)
                        if (segment.FontColor == Color.Default)
                            segment.MaterialButton.FontColor = FontColor;
            }
            else if (propertyName == SelectedFontColorProperty.PropertyName)
            {
                if (Segments != null)
                    foreach (Segment segment in Segments)
                        segment.MaterialButton.SelectedFontColor = SelectedFontColor;
            }
            else if (propertyName == TintImageProperty.PropertyName)
            {
                if (Segments != null)
                    foreach (Segment segment in Segments)
                        segment.MaterialButton.TintImage = TintImage;
            }
            else if (propertyName == HasTightSpacingProperty.PropertyName)
            {
                if (Segments != null)
                    foreach (Segment segment in Segments)
                        segment.MaterialButton.HasTightSpacing = HasTightSpacing;
            }
            else if (propertyName == HorizontalTextAlignmentProperty.PropertyName)
            {
                if (Segments != null)
                    foreach (Segment segment in Segments)
                        segment.MaterialButton.HorizontalTextAlignment = HorizontalTextAlignment;
            }
            else if (propertyName == VerticalTextAlignmentProperty.PropertyName)
            {
                if (Segments != null)
                    foreach (Segment segment in Segments)
                        segment.MaterialButton.VerticalTextAlignment = VerticalTextAlignment;
            }
            else if (propertyName == FontAttributesProperty.PropertyName)
            {
                if (Segments != null)
                    foreach (Segment segment in Segments)
                        if (!segment.FontAttributesSet)
                            segment.MaterialButton.FontAttributes = FontAttributes;
            }
            else if (propertyName == DarkThemeProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.DarkTheme = DarkTheme;
            else if (propertyName == BackgroundColorProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.BackgroundColor = BackgroundColor;
            else if (propertyName == SelectedBackgroundColorProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.SelectedBackgroundColor = SelectedBackgroundColor;
            else if (propertyName == OutlineColorProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.OutlineColor = OutlineColor;
            else if (propertyName == OutlineRadiusProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.OutlineRadius = OutlineRadius;
            else if (propertyName == OutlineWidthProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.OutlineWidth = OutlineWidth;
            else if (propertyName == FontFamilyProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.FontFamily = FontFamily;
            else if (propertyName == FontSizeProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.FontSize = FontSize;
            else if (propertyName == HasShadowProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.HasShadow = HasShadow;

            else if (propertyName == OrientationProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.ParentSegmentsOrientation = Orientation;
            else if (propertyName == IntraSegmentOrientationProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.Orientation = IntraSegmentOrientation;
            else if (propertyName == SeparatorWidthProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.SeparatorWidth = SeparatorWidth;
            else if (propertyName == TrailingImageProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.TrailingImage = TrailingImage;
            else if (propertyName == HapticEffectProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.HapticEffect = HapticEffect;
            else if (propertyName == HapticModeProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.HapticMode = HapticMode;
            else if (propertyName == IsEnabledProperty.PropertyName)
            {
                if (IsEnabled)
                    Opacity *= 2;
                else
                    Opacity /= 2;
            }
            else if (propertyName == IntraSegmentSpacingProperty.PropertyName)
                foreach (MaterialButton button in Children)
                    button.Spacing = IntraSegmentSpacing;
        }

        //Segment _lastSelectedSegment;

        internal void OnButtonPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var button = sender as MaterialButton;
            if (e.PropertyName == MaterialButton.IsSelectedProperty.PropertyName)
            {
                if (GroupToggleBehavior == GroupToggleBehavior.Radio)
                {
                    if (button.IsSelected)
                    {
                        foreach (var seg in _segments)
                            seg.IsSelected = seg.MaterialButton == button;
                    }
                    else if (button.IsEnabled)
                    {
                        int enabled = 0;
                        Segment eseg = null;
                        foreach (var seg in _segments)
                        {
                            if (seg.IsEnabled)
                            {
                                enabled++;
                                eseg = seg;
                            }
                        }
                        eseg.IsSelected |= enabled == 1;
                    }
                }
                /*
				else if (GroupToggleBehavior == GroupToggleBehavior.Multiselect)
				{
					foreach (var seg in _segments)
						if (seg.MaterialButton == button)
							seg.IsSelected = !seg.IsSelected;
				}
				*/
            }
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            //return base.OnMeasure(widthConstraint, heightConstraint);
            var hz = Orientation == StackOrientation.Horizontal;
            var vt = !hz;

            double requestHeight = 0;
            double requestWidth = 0;
            double minHeight = 0;
            double minWidth = 0;

            if (hz )//&& (double.IsInfinity(widthConstraint) || double.IsNaN(widthConstraint)) )
            {
                // we have all the width in the world ... but what's the height?
                foreach (var child in Children)
                {
                    var childSizeRequest = child.Measure(double.PositiveInfinity, heightConstraint, MeasureFlags.None);
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
                {
                    var childSizeRequest = child.Measure(widthConstraint, double.PositiveInfinity, MeasureFlags.None);
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
            //System.Diagnostics.Debug.WriteLine("width[" + width + "] height[" + height + "]");

            var p = new Thickness(0);
            if (HasShadow && BackgroundColor.A > 0)
                p = RoundedBoxBase.ShadowPadding(this);
            //System.Diagnostics.Debug.WriteLine("p=" + p.Description());

            var hz = Orientation == StackOrientation.Horizontal;
            var vt = !hz;
            var newWidth = width - p.HorizontalThickness;
            var newHeight = height - p.VerticalThickness;

            //System.Diagnostics.Debug.WriteLine("newWidth[" + newWidth + "] newHeight[" + newHeight + "]");

            var topPage = this.TopPage();


            int count = Children.Count;
            //System.Diagnostics.Debug.WriteLine("count=[" + count + "]");

            if (count > 0)
            {

                x = Math.Round(x);
                y = Math.Round(y);

                var outlineWidth = OutlineWidth / Display.Scale;
                double xOffset = hz ? outlineWidth + (newWidth - outlineWidth * (count + 1)) / count : 0;
                double yOffset = vt ? outlineWidth + (newHeight - outlineWidth * (count + 1)) / count : 0;
                double sWidth = hz ? xOffset : width;
                double sHeight = vt ? yOffset : height;

                //System.Diagnostics.Debug.WriteLine("sWidth=[" + sWidth + "] sHeight=[" + sHeight + "]");

                for (int i = 0; i < count; i++)
                {
                    var view = Children[i];
                    if (view.IsVisible)
                    {
                        double thisW, thisH;
                        if (i == 0)
                        {
                            thisW = sWidth + (hz ? p.Left : 0);
                            thisH = sHeight + (vt ? p.Top : 0);
                        }
                        else if (i == count - 1)
                        {
                            thisW = hz ? width - x : sWidth;
                            thisH = vt ? height - y : sHeight;
                        }
                        else
                        {
                            thisW = sWidth;
                            thisH = sHeight;
                        }

                        // Math.Round with Display.Scale fixes UWP layout gaps but not UWP SkiaRoundedBoxView gaps

                        //thisW = Math.Round(thisW);
                        //thisH = Math.Round(thisH);
                        thisW = Math.Round(thisW * Display.Scale) / Display.Scale;
                        thisH = Math.Round(thisH * Display.Scale) / Display.Scale;
                        if (x + thisW > width)
                            //thisW = width - x;
                            //thisW = Math.Floor(width - x);
                            thisW = Math.Floor((width - x) * Display.Scale) / Display.Scale;
                        if (y + thisH > height)
                            //thisH = height - y;
                            //thisH = Math.Floor(height - y);
                            thisH = Math.Floor((height - y) * Display.Scale) / Display.Scale;
                        LayoutChildIntoBoundingRegion(view, new Rectangle(x, y, thisW, thisH));
                        //if (vt)
                        //System.Diagnostics.Debug.WriteLine("MaterialSegmentedControl.LayoutChildren LayoutChildIntoBoundingRegion("+view.Id+","+x+","+y+","+thisW+","+thisH+")");
                        x = Math.Round((x + (hz ? thisW : 0)) * Display.Scale) / Display.Scale;
                        y = Math.Round((y + (vt ? thisH : 0)) * Display.Scale) / Display.Scale;
                        //x = Math.Round(x + (hz ? thisW : 0));
                        //y = Math.Round(y + (vt ? thisH : 0));
                        //x = (x + (hz ? thisW : 0));
                        //y = (y + (vt ? thisH : 0));
                    }
                }
                //var lastView = Children.Last();
                //if (lastView.IsVisible)
                //{
                //    LayoutChildIntoBoundingRegion(lastView, new Rectangle(x, y,  Math.Round(width - p.Right - x), Math.Round(height - p.Bottom - y)));
                //}
            }
        }
        #endregion


        #region Event management

        /// <summary>
        /// Occurs when one of the segments is tapped.
        /// </summary>
        public event SegmentedControlEventHandler SegmentTapped;
        void OnSegmentTapped(object sender, EventArgs e)
        {
            for (int i = 0; i < _segments.Count; i++)
            {
                var button = _segments[i].MaterialButton;
                if (button.Equals(sender))
                {
                    SegmentedControlEventHandler eventHandler = SegmentTapped;
                    if (eventHandler != null)
                        eventHandler(this, new SegmentedControlEventArgs(i, _segments[i]));
                    //Debug.WriteLine ("Tapped ["+i+"]");
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
                var button = _segments[i].MaterialButton;
                if (button.Equals(sender) && button.IsSelected)
                {
                    SegmentedControlEventHandler eventHandler = SegmentSelected;
                    if (eventHandler != null)
                        eventHandler(this, new SegmentedControlEventArgs(i, _segments[i]));
                    //Debug.WriteLine ("Selected ["+i+"]");
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
                var button = _segments[i].MaterialButton;
                if (button.Equals(sender))
                {
                    SegmentedControlEventHandler eventHandler = SegmentLongPressing;
                    if (eventHandler != null)
                        eventHandler(this, new SegmentedControlEventArgs(i, _segments[i]));
                    //Debug.WriteLine ("Tapped ["+i+"]");
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
                var button = _segments[i].MaterialButton;
                if (button.Equals(sender))
                {
                    SegmentedControlEventHandler eventHandler = SegmentLongPressed;
                    if (eventHandler != null)
                        eventHandler(this, new SegmentedControlEventArgs(i, _segments[i]));
                    //Debug.WriteLine ("Tapped ["+i+"]");
                    return;
                }
            }
        }

        #endregion

    }


}

