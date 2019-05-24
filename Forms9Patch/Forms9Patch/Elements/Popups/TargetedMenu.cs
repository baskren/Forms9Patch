using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Linq;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch.TargetedMenu class 
    /// </summary>
    [ContentProperty(nameof(Segments))]
    public class TargetedMenu : BubblePopup
    {
        static readonly Color DefaultHorizontalBackgroundColor = Color.FromRgb(51, 51, 51);
        static readonly Color DefaultHorizontalTextColor = Color.White;
        static readonly Color DefaultHorizontalSeparatorColor = DefaultHorizontalTextColor;

        static readonly Color DefaultVerticalBackgroundColor = Color.White;
        static readonly Color DefaultVerticalTextColor = Color.FromHex("0076FF");
        static readonly Color DefaultVerticalSeparatorColor = DefaultVerticalTextColor;


        static readonly double DefaultSeparatorThickness = 1 / Forms9Patch.Display.Scale;



        #region Properties

        #region Segments property
        readonly ObservableCollection<Segment> _segments = new ObservableCollection<Segment>();
        /// <summary>
        /// The container for the Segmented Control's buttons.
        /// </summary>
        /// <value>The buttons.</value>
        public IList<Segment> Segments
        {
            get => _segments;
            set
            {
                _segments.Clear();
                if (value != null)
                {
                    foreach (var segment in value)
                        _segments.Add(segment);
                }
            }
        }
        #endregion Segments property

        #region Orientation property
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(TargetedMenu), StackOrientation.Horizontal);
        public StackOrientation Orientation
        {
            get => (StackOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        #endregion Orientation property

        #region FontSize property
        /// <summary>
        /// backing store for FontSize property
        /// </summary>
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(TargetedMenu), -1.0);
        /// <summary>
        /// Gets/Sets the FontSize property
        /// </summary>
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        #endregion FontSize property

        #region TextColor property
        /// <summary>
        /// backing store for FontColor property
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Xamarin.Forms.Color), typeof(TargetedMenu), default(Color));
        /// <summary>
        /// Gets/Sets the FontColor property
        /// </summary>
        public Xamarin.Forms.Color TextColor
        {
            get => (Xamarin.Forms.Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        #endregion TextColor property

        #region SeparatorColor property
        /// <summary>
        /// backing store for SeparatorColor property
        /// </summary>
        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(TargetedMenu), default(Color));
        /// <summary>
        /// Gets/Sets the SeparatorColor property
        /// </summary>
        public Color SeparatorColor
        {
            get => (Color)GetValue(SeparatorColorProperty);
            set => SetValue(SeparatorColorProperty, value);
        }
        #endregion SeparatorColor property

        #region SeparatorWidth property
        /// <summary>
        /// backing store for SeparatorWidth property
        /// </summary>
        public static readonly BindableProperty SeparatorThicknessProperty = BindableProperty.Create(nameof(SeparatorThickness), typeof(double), typeof(TargetedMenu), -1.0);
        /// <summary>
        /// Gets/Sets the SeparatorWidth property
        /// </summary>
        public double SeparatorThickness
        {
            get => (double)GetValue(SeparatorThicknessProperty);
            set => SetValue(SeparatorThicknessProperty, value);
        }
        #endregion SeparatorWidth property

        #region HapticEffect property
        /// <summary>
        /// backing store for HapticEffect property
        /// </summary>
        public static readonly BindableProperty HapticEffectProperty = BindableProperty.Create(nameof(HapticEffect), typeof(HapticEffect), typeof(TargetedMenu), default(HapticEffect));
        /// <summary>
        /// Gets/Sets the HapticEffect property
        /// </summary>
        public HapticEffect HapticEffect
        {
            get => (HapticEffect)GetValue(HapticEffectProperty);
            set => SetValue(HapticEffectProperty, value);
        }
        #endregion HapticEffect property

        #region HapticEffectMode property
        /// <summary>
        /// backing store for HapticEffectMode property
        /// </summary>
        public static readonly BindableProperty HapticEffectModeProperty = BindableProperty.Create(nameof(HapticEffectMode), typeof(EffectMode), typeof(TargetedMenu), default(EffectMode));
        /// <summary>
        /// Gets/Sets the HapticEffectMode property
        /// </summary>
        public EffectMode HapticEffectMode
        {
            get => (EffectMode)GetValue(HapticEffectModeProperty);
            set => SetValue(HapticEffectModeProperty, value);
        }
        #endregion HapticEffectMode property

        #region SoundEffect property
        /// <summary>
        /// The backing store for the sound effect property.
        /// </summary>
        public static readonly BindableProperty SoundEffectProperty = BindableProperty.Create(nameof(SoundEffect), typeof(SoundEffect), typeof(TargetedMenu), default(SoundEffect));
        /// <summary>
        /// Gets or sets the sound effect played when a menu item is tapped.
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
        public static readonly BindableProperty SoundEffectModeProperty = BindableProperty.Create(nameof(SoundEffectMode), typeof(EffectMode), typeof(TargetedMenu), default(EffectMode));
        /// <summary>
        /// Gets or sets if the sound effect is played when a menu item is tapped
        /// </summary>
        /// <value>The sound effect mode.</value>
        public EffectMode SoundEffectMode
        {
            get => (EffectMode)GetValue(SoundEffectModeProperty);
            set => SetValue(SoundEffectModeProperty, value);
        }
        #endregion SoundEffectMode property


        #region IcontFontFamiliy property
        public static readonly BindableProperty IconFontFamilyProperty = BindableProperty.Create(nameof(IconFontFamily), typeof(string), typeof(TargetedMenu), default(string));
        public string IconFontFamily
        {
            get => (string)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        #endregion IcontFontFamiliy property

        #endregion


        #region VisualElements
#pragma warning disable CC0033 // Dispose Fields Properly
        readonly Button _leftArrowButton = new Button
        {
            IsVisible = false,
            TextColor = DefaultHorizontalTextColor,
            FontSize = 24,
            TintIcon = true,
            IconImage = new Forms9Patch.Image("Forms9Patch.Resources.menu_left.svg") { Fill = Fill.AspectFill, WidthRequest = 24, HeightRequest = 24 },
            HasTightSpacing = true,
            Padding = Device.RuntimePlatform == Device.UWP ? new Thickness(4, 0, 4, 4) : new Thickness(10, 0),
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = DefaultHorizontalBackgroundColor.WithAlpha(0.05),
            Lines = 1,
            AutoFit = AutoFit.None,
        };
        readonly BoxView _leftArrowSeparator = new BoxView { Color = DefaultHorizontalSeparatorColor, WidthRequest = DefaultSeparatorThickness, Margin = 0 };

        readonly Button _rightArrowButton = new Button
        {
            IsVisible = false,
            TextColor = DefaultHorizontalTextColor,
            FontSize = 24,
            TintIcon = true,
            IconImage = new Forms9Patch.Image("Forms9Patch.Resources.menu_right.svg") { Fill = Fill.AspectFill, WidthRequest = 24, HeightRequest = 24 },
            HasTightSpacing = true,
            Padding = Device.RuntimePlatform == Device.UWP ? new Thickness(4, 0, 4, 4) : new Thickness(10, 0),
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = DefaultHorizontalBackgroundColor.WithAlpha(0.05),
            Lines = 1,
            AutoFit = AutoFit.None,
        };
        // this is crazy but the stack layout doesn't give the children the correct space if it isn't included.
        readonly BoxView _rightArrowSeparator = new BoxView { Color = DefaultHorizontalSeparatorColor, WidthRequest = 0.05, Margin = 0 };


        readonly Button _upArrowButton = new Button
        {
            IsVisible = false,
            TextColor = DefaultVerticalTextColor,
            FontSize = 24,
            TintIcon = true,
            IconImage = new Forms9Patch.Image("Forms9Patch.Resources.menu_up.svg") { Fill = Fill.AspectFill, WidthRequest = 24, HeightRequest = 24 },
            HasTightSpacing = true,
            //Padding = Device.RuntimePlatform == Device.UWP ? new Thickness(4, 0, 4, 4) : new Thickness(10, 0),
            Padding = new Thickness(0, 10),
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = DefaultVerticalBackgroundColor.WithAlpha(0.05),
            Lines = 1,
            AutoFit = AutoFit.None,
        };
        readonly BoxView _upArrowSeparator = new BoxView { Color = DefaultVerticalSeparatorColor, HeightRequest = DefaultSeparatorThickness, Margin = 0 };

        readonly Button _downArrowButton = new Button
        {
            IsVisible = false,
            TextColor = DefaultVerticalTextColor,
            FontSize = 24,
            TintIcon = true,
            IconImage = new Forms9Patch.Image("Forms9Patch.Resources.menu_down.svg") { Fill = Fill.AspectFill, WidthRequest = 24, HeightRequest = 24 },
            HasTightSpacing = true,
            //Padding = Device.RuntimePlatform == Device.UWP ? new Thickness(4, 0, 4, 4) : new Thickness(10, 0),
            Padding = new Thickness(0, 10),
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = DefaultVerticalBackgroundColor.WithAlpha(0.05),
            Lines = 1,
            AutoFit = AutoFit.None,
        };
        readonly BoxView _downArrowSeparator = new BoxView { Color = DefaultVerticalSeparatorColor, HeightRequest = DefaultSeparatorThickness, Margin = 0 };
#pragma warning restore CC0033 // Dispose Fields Properly


        readonly Xamarin.Forms.StackLayout _stackLayout = new Xamarin.Forms.StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 0,
            Padding = 0,
            Margin = 0,
            BackgroundColor = Color.Transparent,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

        #endregion


        #region Fields
        int _currentPage;
        #endregion


        #region Constructor / Factory
        void SegmentsFromHtmlTexts(List<string> htmlTexts)
        {
            if (htmlTexts != null)
            {
                foreach (var htmlText in htmlTexts)
                    Segments.Add(new Segment
                    {
                        HtmlText = htmlText
                    });
            }
        }

        /// <summary>
        /// Instantiates, populates and presents a TargetedMenu
        /// </summary>
        /// <param name="target">VisualElement to target</param>
        /// <param name="htmlTexts">List of text for menu items (with optional HTML markup)</param>
        /// <returns></returns>
        public static TargetedMenu Create(VisualElement target, List<string> htmlTexts = null)
        {
            var targetedMenu = new TargetedMenu(target);
            targetedMenu.SegmentsFromHtmlTexts(htmlTexts);
            targetedMenu.OutlineColor = Color.Gray;
            targetedMenu.OutlineWidth = 1;
            targetedMenu.IsVisible = true;
            return targetedMenu;
        }

        /// <summary>
        /// Instantiates, populates and presents a TargetedMenu at a Point
        /// </summary>
        /// <param name="target"></param>
        /// <param name="point"></param>
        /// <param name="htmlTexts"></param>
        /// <returns></returns>
        public static TargetedMenu Create(VisualElement target, Point point, List<string> htmlTexts = null)
        {
            var targetedMenu = new TargetedMenu(target, point);
            targetedMenu.SegmentsFromHtmlTexts(htmlTexts);
            targetedMenu.OutlineColor = Color.Gray;
            targetedMenu.OutlineWidth = 1;
            targetedMenu.IsVisible = true;
            return targetedMenu;
        }

        /// <summary>
        /// Creates a vertical Targeted Menu.
        /// </summary>
        /// <returns>The vertical.</returns>
        /// <param name="target">Target.</param>
        /// <param name="htmlTexts">Html texts.</param>
        public static TargetedMenu CreateVertical(VisualElement target, List<string> htmlTexts = null)
        {
            var targetedMenu = new TargetedMenu(target);
            targetedMenu.SegmentsFromHtmlTexts(htmlTexts);
            targetedMenu.Orientation = StackOrientation.Vertical;
            targetedMenu.IsVisible = true;
            return targetedMenu;
        }

        /// <summary>
        /// Create the specified target menu, at a point.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="target">Target.</param>
        /// <param name="point">Point.</param>
        /// <param name="htmlTexts">Html texts.</param>
        public static TargetedMenu CreateVertical(VisualElement target, Point point, List<string> htmlTexts = null)
        {
            var targetedMenu = new TargetedMenu(target, point);
            targetedMenu.SegmentsFromHtmlTexts(htmlTexts);
            targetedMenu.Orientation = StackOrientation.Vertical;
            targetedMenu.IsVisible = true;
            return targetedMenu;
        }


        void Init()
        {
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Fill;


            PointerDirection = PointerDirection.Any;
            PreferredPointerDirection = PointerDirection.Down;
            PointerLength = 10;
            BackgroundColor = default(Color);
            HasShadow = false;
            Padding = 0;
            //Margin = 0;
            OutlineRadius = 5;
            PageOverlayColor = Color.Black.WithAlpha(0.1);
            _segments.CollectionChanged += OnSegmentsCollectionChanged;
            SizeChanged += OnSizeChanged;
            Content = _stackLayout;

            _leftArrowButton.Clicked += OnBackwardArrowButtonClicked;
            _rightArrowButton.Clicked += OnForewardArrowButtonClicked;

            _upArrowButton.Clicked += OnBackwardArrowButtonClicked;
            _downArrowButton.Clicked += OnForewardArrowButtonClicked;

            if (Device.RuntimePlatform != Device.UWP)
                _stackLayout.HeightRequest = 28;

            _upArrowButton.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == Forms9Patch.Button.IsVisibleProperty.PropertyName)
                    System.Diagnostics.Debug.WriteLine(GetType() + ".");
            };
        }

        /// <summary>
        /// Constructor for TargetedMenu
        /// </summary>
        /// <param name="target"></param>
        public TargetedMenu(VisualElement target) : base(target) => Init();

        /// <summary>
        /// Constructor for TargetedMenu at a Point
        /// </summary>
        /// <param name="target"></param>
        /// <param name="point"></param>
        public TargetedMenu(VisualElement target, Point point) : base(target, point) => Init();

        #endregion


        #region Disposal
        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                foreach (var segment in Segments)
                    UnconfiguerButton(segment._button);

                _segments.CollectionChanged -= OnSegmentsCollectionChanged;
                SizeChanged -= OnSizeChanged;

                _leftArrowButton.Clicked -= OnBackwardArrowButtonClicked;
                _rightArrowButton.Clicked -= OnForewardArrowButtonClicked;
                _upArrowButton.Clicked -= OnBackwardArrowButtonClicked;
                _downArrowButton.Clicked -= OnForewardArrowButtonClicked;

                _leftArrowButton.Dispose();
                _rightArrowButton.Dispose();
                _upArrowButton.Dispose();
                _downArrowButton.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion


        #region PropertyChange handlers
        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);
            if (propertyName == IsVisibleProperty.PropertyName)
                _currentPage = 0;
        }

        /// <summary>
        /// A property changed.  Let's deal with it.
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            base.OnPropertyChanged(propertyName);

            if (propertyName == BackgroundColorProperty.PropertyName)
                UpdateLayout();
            else if (propertyName == TextColorProperty.PropertyName)
                UpdateLayout();
            else if (propertyName == FontSizeProperty.PropertyName)
                UpdateLayout();
            else if (propertyName == SeparatorColorProperty.PropertyName)
                UpdateLayout();
            else if (propertyName == SeparatorThicknessProperty.PropertyName)
                UpdateLayout();
            else if (propertyName == HapticEffectProperty.PropertyName)
            {
                foreach (VisualElement visualElement in _stackLayout.Children)
                    if (visualElement is Button button)
                        button.HapticEffect = HapticEffect;
            }
            else if (propertyName == HapticEffectModeProperty.PropertyName)
            {
                foreach (VisualElement visualElement in _stackLayout.Children)
                    if (visualElement is Button button)
                        button.HapticEffectMode = HapticEffectMode;
            }
            else if (propertyName == SoundEffectProperty.PropertyName)
            {
                foreach (VisualElement visualElement in _stackLayout.Children)
                    if (visualElement is Button button)
                        button.SoundEffect = SoundEffect;
            }
            else if (propertyName == SoundEffectModeProperty.PropertyName)
            {
                foreach (VisualElement visualElement in _stackLayout.Children)
                    if (visualElement is Button button)
                        button.SoundEffectMode = SoundEffectMode;
            }
            else if (propertyName == OrientationProperty.PropertyName)
                UpdateOrientation();
            else if (propertyName == IconFontFamilyProperty.PropertyName)
                foreach (var child in _stackLayout.Children)
                    if (child is Button button)
                        button.IconFontFamily = IconFontFamily;
        }
        #endregion


        #region ButtonFactory
        void ConfigureSegment(Segment segment)
        {
            segment._button.Text = segment.Text;
            segment._button.HtmlText = segment.HtmlText;
            segment._button.IconImage = segment.IconImage;
            segment._button.IconText = segment.IconText;
            segment._button.Spacing = 4;
            segment._button.TintIcon = true;
            segment._button.HasTightSpacing = true;
            segment._button.TextColor = TextColor;
            segment._button.VerticalTextAlignment = TextAlignment.Center;
            segment._button.HorizontalTextAlignment = TextAlignment.Center;
            segment._button.VerticalOptions = LayoutOptions.Fill;
            segment._button.HorizontalOptions = LayoutOptions.Fill;
            segment._button.BackgroundColor = BackgroundColor.WithAlpha(0.02);
            segment._button.Margin = 0;
            segment._button.IconFontFamily = IconFontFamily;

            //if (Device.RuntimePlatform != Device.UWP)
            if (FontSize < 0)
                segment._button.FontSize = 12;

            segment._button.Lines = 1;
            segment._button.AutoFit = AutoFit.None;

            segment._button.SizeChanged += OnButtonSizeChanged;
            segment._button.Tapped += OnButtonTapped;

            UpdateOrientation(segment);
        }

        void UnconfiguerButton(Button button)
        {
            button.SizeChanged -= OnButtonSizeChanged;
            button.Tapped -= OnButtonTapped;
        }

        #endregion


        #region Collection Management
        void OnSegmentsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.OldItems != null)
                foreach (var item in e.OldItems)
                    if (item is Segment segment)
                        UnconfiguerButton(segment._button);
            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    if (item is Segment segment)
                        ConfigureSegment(segment);


            _stackLayout.Children.Clear();

            _stackLayout.Children.Add(_leftArrowButton);
            _stackLayout.Children.Add(_leftArrowSeparator);
            _stackLayout.Children.Add(_upArrowButton);
            _stackLayout.Children.Add(_upArrowSeparator);

            var first = true;
            foreach (var segment in Segments)
            {
                if (!first)
                    _stackLayout.Children.Add(new BoxView { Color = SeparatorColor, WidthRequest = DefaultSeparatorThickness });
                _stackLayout.Children.Add(segment._button);
                first = false;
            }

            _stackLayout.Children.Add(_rightArrowSeparator);
            _stackLayout.Children.Add(_rightArrowButton);
            _stackLayout.Children.Add(_downArrowSeparator);
            _stackLayout.Children.Add(_downArrowButton);

            UpdateLayout();
        }
        #endregion


        #region Event Handlers
        private void OnSizeChanged(object sender, System.EventArgs e)
            => UpdateLayout();


        private void OnButtonSizeChanged(object sender, System.EventArgs e)
            => UpdateLayout();
        #endregion


        #region Layout
        void UpdateOrientation(Segment segment)
        {

            if (Orientation == StackOrientation.Horizontal)
                segment._button.Padding = Device.RuntimePlatform == Device.UWP ? new Thickness(8, 4, 8, 8) : new Thickness(8, 0);
            else
                segment._button.Padding = Device.RuntimePlatform == Device.UWP ? new Thickness(28, 4, 28, 8) : new Thickness(28, 8);
        }

        void UpdateOrientation()
        {
            _stackLayout.Orientation = Orientation;
            foreach (var segment in Segments)
                UpdateOrientation(segment);
            UpdateLayout();
        }

        double ButtonLength(Forms9Patch.Button button)
        {
            var size = button.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins);
            return Math.Ceiling(Orientation == StackOrientation.Horizontal ? size.Request.Width : size.Request.Height);
        }

        double AvailableSpace()
            => Math.Floor(Orientation == StackOrientation.Horizontal
            ? Width - _bubbleLayout.Padding.HorizontalThickness - _bubbleLayout.Margin.HorizontalThickness - Padding.HorizontalThickness - Margin.HorizontalThickness - 2 * OutlineWidth - _stackLayout.Padding.HorizontalThickness - _stackLayout.Margin.HorizontalThickness
            : Height - _upArrowButton.Padding.VerticalThickness - _upArrowButton.Margin.VerticalThickness - Padding.VerticalThickness - Margin.VerticalThickness - 2 * OutlineWidth - _stackLayout.Padding.VerticalThickness - _stackLayout.Margin.VerticalThickness);

        void SetSeparatorThickness(BoxView separator)
        {
            separator.HorizontalOptions = LayoutOptions.Fill;
            separator.VerticalOptions = LayoutOptions.Fill;
            if (Orientation == StackOrientation.Horizontal)
            {
                separator.WidthRequest = SeparatorThickness > 0 ? SeparatorThickness : DefaultSeparatorThickness;
                separator.HeightRequest = -1;
            }
            else
            {
                separator.WidthRequest = -1;
                separator.HeightRequest = SeparatorThickness > 0 ? SeparatorThickness : DefaultSeparatorThickness;
            }
        }

        void UpdateLayout()
        {
            if (P42.Utils.Environment.IsOnMainThread)
            {
                if (Width < 0 || !IsVisible || Segments.Count < 1)
                    return;

                HorizontalOptions = LayoutOptions.Center;
                VerticalOptions = LayoutOptions.Center;

                _stackLayout.HorizontalOptions = Orientation == StackOrientation.Horizontal ? LayoutOptions.Center : LayoutOptions.Fill;
                _stackLayout.VerticalOptions = LayoutOptions.Center;

                var textColor = TextColor == default
                    ? (Orientation == StackOrientation.Horizontal ? DefaultHorizontalTextColor : DefaultVerticalTextColor)
                    : TextColor;
                var separatorColor = SeparatorColor == default
                    ? (Orientation == StackOrientation.Horizontal ? DefaultHorizontalSeparatorColor : DefaultVerticalSeparatorColor)
                    : SeparatorColor;
                _bubbleLayout.BackgroundColor = BackgroundColor == default
                    ? (Orientation == StackOrientation.Horizontal ? DefaultHorizontalBackgroundColor : DefaultVerticalBackgroundColor)
                    : BackgroundColor;

                _leftArrowButton.IsVisible = false;
                _leftArrowSeparator.IsVisible = false;
                _rightArrowButton.IsVisible = false;
                _rightArrowSeparator.IsVisible = false;
                _upArrowButton.IsVisible = false;
                _upArrowSeparator.IsVisible = false;
                _downArrowButton.IsVisible = false;
                _downArrowSeparator.IsVisible = false;

                _leftArrowButton.IconImage.HeightRequest = _leftArrowButton.IconImage.WidthRequest = FontSize;
                _rightArrowButton.IconImage.HeightRequest = _rightArrowButton.IconImage.WidthRequest = FontSize;
                _upArrowButton.IconImage.HeightRequest = _upArrowButton.IconImage.WidthRequest = 2 * FontSize;
                _downArrowButton.IconImage.HeightRequest = _downArrowButton.IconImage.WidthRequest = 2 * FontSize;

                var backwardButton = Orientation == StackOrientation.Horizontal ? _leftArrowButton : _upArrowButton;
                var backwardSeparator = Orientation == StackOrientation.Horizontal ? _leftArrowSeparator : _upArrowSeparator;
                var forewardButton = Orientation == StackOrientation.Horizontal ? _rightArrowButton : _downArrowButton;
                var forewardSeparator = Orientation == StackOrientation.Horizontal ? _rightArrowSeparator : _downArrowSeparator;

                backwardButton.TextColor = textColor;
                backwardSeparator.Color = separatorColor;
                backwardButton.BackgroundColor = _bubbleLayout.BackgroundColor.MultiplyAlpha(0.02);
                SetSeparatorThickness(backwardSeparator);
                forewardButton.TextColor = textColor;
                forewardSeparator.Color = separatorColor;
                forewardButton.BackgroundColor = _bubbleLayout.BackgroundColor.MultiplyAlpha(0.02);
                SetSeparatorThickness(forewardSeparator);

                var forewardArrowShouldBeVisible = true;

                var backwardLength = Math.Ceiling(Orientation == StackOrientation.Horizontal ? _leftArrowButton.Measure(double.PositiveInfinity, double.PositiveInfinity).Request.Width : _upArrowButton.Measure(double.PositiveInfinity, double.PositiveInfinity).Request.Height) + SeparatorThickness;
                var forewardLength = Math.Ceiling(Orientation == StackOrientation.Horizontal ? _rightArrowButton.Measure(double.PositiveInfinity, double.PositiveInfinity).Request.Width : _downArrowButton.Measure(double.PositiveInfinity, double.PositiveInfinity).Request.Height);

                // calculate pages
                var pageLength = 0.0;

                backwardButton.IsVisible = _currentPage > 0;
                backwardSeparator.IsVisible = _currentPage > 0;

                var segmentIndex = 0;
                var pageIndex = 0;

                var calculatedLength = 0.0;
                for (int i = 4; i < _stackLayout.Children.Count - 4; i += 2)
                {
                    var button = _stackLayout.Children[i] as Forms9Patch.Button;
                    var separator = _stackLayout.Children[i + 1] as BoxView;
                    button.FontSize = FontSize > 0 ? FontSize : 20;

                    var thisButtonAdds = ButtonLength(button) + _stackLayout.Spacing + SeparatorThickness + 2 + _stackLayout.Spacing;
                    if (segmentIndex < Segments.Count && pageLength + thisButtonAdds + forewardLength >= AvailableSpace())
                    {
                        if (pageIndex == _currentPage)
                            calculatedLength = pageLength + forewardLength;
                        pageIndex++;
                        pageLength = backwardLength;
                    }
                    pageLength += thisButtonAdds;

                    button.TextColor = textColor;
                    separator.Color = separatorColor;
                    button.BackgroundColor = _bubbleLayout.BackgroundColor.MultiplyAlpha(0.02);
                    SetSeparatorThickness(separator);

                    button.IsVisible = pageIndex == _currentPage;
                    separator.IsVisible = pageIndex == _currentPage;

                    forewardArrowShouldBeVisible &= (pageIndex != _currentPage || segmentIndex != Segments.Count - 1);
                    segmentIndex++;

                }


                forewardSeparator.IsVisible = false;
                forewardButton.IsVisible = forewardArrowShouldBeVisible;

                if (Orientation == StackOrientation.Vertical)
                {
                    if (_stackLayout.Children.LastOrDefault((arg) => arg.IsVisible && arg is BoxView) is BoxView lastSeparator)
                        lastSeparator.IsVisible = false;
                    _stackLayout.HeightRequest = -1;
                }
            }
            else
                Device.BeginInvokeOnMainThread(UpdateLayout);


        }

        #endregion


        #region Events
        void OnBackwardArrowButtonClicked(object sender, EventArgs e)
        {
            _currentPage--;
            UpdateLayout();
        }

        void OnForewardArrowButtonClicked(object sender, EventArgs e)
        {
            _currentPage++;
            UpdateLayout();
        }

        /// <summary>
        /// Event fired with a menu item (segment) has been tapped
        /// </summary>
        public event SegmentedControlEventHandler SegmentTapped;
        async void OnButtonTapped(object sender, System.EventArgs e)
        {
            for (int i = 0; i < _segments.Count; i++)
            {
                var button = _segments[i]._button;
                if (button.Equals(sender))
                {
                    SegmentTapped?.Invoke(this, new SegmentedControlEventArgs(i, _segments[i]));
                    break;
                }
            }
            //IsVisible = false;
            await CancelAsync(sender);
        }

        #endregion
    }
}