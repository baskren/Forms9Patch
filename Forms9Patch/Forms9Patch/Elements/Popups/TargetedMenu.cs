using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

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
        static readonly Color DefaultVerticalSeparatorColor = DefaultHorizontalTextColor;


        static readonly double DefaultSeparatorWidth = 1.0;



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
        public static readonly BindableProperty SeparatorWidthProperty = BindableProperty.Create(nameof(SeparatorWidth), typeof(double), typeof(TargetedMenu), -1.0);
        /// <summary>
        /// Gets/Sets the SeparatorWidth property
        /// </summary>
        public double SeparatorWidth
        {
            get => (double)GetValue(SeparatorWidthProperty);
            set => SetValue(SeparatorWidthProperty, value);
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


        #endregion


        #region VisualElements
        readonly Button _leftArrowButton = new Button
        {
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
            IsVisible = true,
        };
        readonly BoxView _leftArrowSeparator = new BoxView { Color = DefaultHorizontalSeparatorColor, WidthRequest = DefaultSeparatorWidth };

        readonly Button _rightArrowButton = new Button
        {
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
            IsVisible = true,
        };
        // this is crazy but the stack layout doesn't give the children the correct space if it isn't included.
        readonly BoxView _rightArrowSeparator = new BoxView { Color = DefaultHorizontalSeparatorColor, WidthRequest = 0.05 };


        readonly Button _upArrowButton = new Button
        {
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
            IsVisible = true,
        };
        readonly BoxView _upArrowSeparator = new BoxView { Color = DefaultVerticalSeparatorColor, HeightRequest = DefaultSeparatorWidth };

        readonly Button _downArrowButton = new Button
        {
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
            IsVisible = true,
        };
        readonly BoxView _downArrowSeparator = new BoxView { Color = DefaultVerticalSeparatorColor, HeightRequest = DefaultSeparatorWidth };


        readonly Xamarin.Forms.StackLayout _stackLayout = new Xamarin.Forms.StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 0,
            Padding = 0,
            Margin = 0,
            BackgroundColor = Color.Transparent,
            HorizontalOptions = LayoutOptions.Center,
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
            targetedMenu.BackgroundColor = DefaultHorizontalBackgroundColor;
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
            targetedMenu.BackgroundColor = DefaultHorizontalBackgroundColor;
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
            targetedMenu.BackgroundColor = DefaultVerticalBackgroundColor;
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
            targetedMenu.BackgroundColor = DefaultVerticalBackgroundColor;
            targetedMenu.IsVisible = true;
            return targetedMenu;
        }


        void Init()
        {
            PointerDirection = PointerDirection.Any;
            PreferredPointerDirection = PointerDirection.Down;
            PointerLength = 10;
            BackgroundColor = DefaultHorizontalBackgroundColor;
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
            base.Dispose(disposing);
            if (disposing && !_disposed)
            {
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

                _disposed = true;
            }
        }
        #endregion


        #region PropertyChange handlers
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
            else if (propertyName == SeparatorWidthProperty.PropertyName)
                UpdateLayout();
            //else if (propertyName == OutlineColorProperty.PropertyName)
            //    _stackLayout.OutlineColor = OutlineColor == Color.Default || OutlineColor == Color.Transparent ? BackgroundColor : OutlineColor;

            //else if (propertyName == OutlineRadiusProperty.PropertyName)
            //    _stackLayout.OutlineRadius = OutlineRadius + 1;
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
                    _stackLayout.Children.Add(new BoxView { Color = SeparatorColor, WidthRequest = DefaultSeparatorWidth });
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
                segment._button.Padding = Device.RuntimePlatform == Device.UWP ? new Thickness(8, 4, 8, 8) : new Thickness(0, 8);
        }

        void UpdateOrientation()
        {
            _stackLayout.Orientation = Orientation;
            foreach (var segment in Segments)
                UpdateOrientation(segment);
            UpdateLayout();
        }

        double ButtonLength(Forms9Patch.Button button)
            => (Orientation == StackOrientation.Horizontal ? button.UnexpandedTightSize.Width : button.UnexpandedTightSize.Height);

        double AvailableSpace()
            => (Orientation == StackOrientation.Horizontal ? Width - Padding.HorizontalThickness - Margin.HorizontalThickness : Height - Padding.VerticalThickness - Margin.VerticalThickness);

        void UpdateLayout()
        {
            if (P42.Utils.Environment.IsOnMainThread)
            {
                if (Width < 0)
                    return;

                var textColor = TextColor == default
                    ? (Orientation == StackOrientation.Horizontal ? DefaultHorizontalTextColor : DefaultVerticalTextColor)
                    : TextColor;
                var separatorColor = SeparatorColor == default
                    ? (Orientation == StackOrientation.Horizontal ? DefaultHorizontalSeparatorColor : DefaultVerticalSeparatorColor)
                    : SeparatorColor;
                base.BackgroundColor = BackgroundColor == default
                    ? (Orientation == StackOrientation.Horizontal ? DefaultHorizontalBackgroundColor : DefaultVerticalBackgroundColor)
                    : BackgroundColor;


                _leftArrowButton.IsVisible = false;
                _leftArrowSeparator.IsVisible = false;
                _rightArrowButton.IsVisible = false;
                _rightArrowSeparator.IsVisible = false;

                var backwardButton = Orientation == StackOrientation.Horizontal ? _leftArrowButton : _upArrowButton;
                var backwardSeparator = Orientation == StackOrientation.Horizontal ? _leftArrowSeparator : _upArrowSeparator;
                var forewardButton = Orientation == StackOrientation.Horizontal ? _rightArrowButton : _downArrowButton;
                var forewardSeparator = Orientation == StackOrientation.Horizontal ? _rightArrowSeparator : _downArrowSeparator;

                backwardButton.BackgroundColor = base.BackgroundColor.MultiplyAlpha(0.02);
                backwardButton.TextColor = textColor;
                backwardSeparator.Color = separatorColor;
                forewardButton.BackgroundColor = base.BackgroundColor.MultiplyAlpha(0.02);
                forewardButton.TextColor = textColor;
                forewardSeparator.Color = separatorColor;

                var forewardArrowShouldBeVisible = true;

                var backwardLength = (Orientation == StackOrientation.Horizontal ? _leftArrowButton.UnexpandedTightSize.Width : _upArrowButton.UnexpandedTightSize.Height) + SeparatorWidth;
                var forewardLength = (Orientation == StackOrientation.Horizontal ? _rightArrowButton.UnexpandedTightSize.Width : _downArrowButton.UnexpandedTightSize.Height) + SeparatorWidth;

                // calculate pages
                var pageLength = 0.0;

                backwardButton.IsVisible = _currentPage > 0;
                backwardSeparator.IsVisible = _currentPage > 0;

                var segmentIndex = 0;
                var pageIndex = 0;
                for (int i = 2; i < _stackLayout.Children.Count - 2; i += 2)
                {
                    var button = _stackLayout.Children[i] as Forms9Patch.Button;
                    var separator = _stackLayout.Children[i + 1] as BoxView;
                    button.FontSize = FontSize > 0 ? FontSize : 12;

                    pageLength += ButtonLength(button) + SeparatorWidth + _stackLayout.Spacing + 10;

                    if (segmentIndex < Segments.Count && pageLength + forewardLength >= AvailableSpace())//* 0.75)
                    {
                        pageIndex++;
                        pageLength = backwardLength;
                    }

                    button.BackgroundColor = base.BackgroundColor.MultiplyAlpha(0.02);
                    button.TextColor = textColor;
                    separator.Color = separatorColor;

                    button.IsVisible = pageIndex == _currentPage;
                    separator.IsVisible = pageIndex == _currentPage;

                    forewardArrowShouldBeVisible &= (pageIndex != _currentPage || segmentIndex != Segments.Count - 1);
                    segmentIndex++;
                }

                forewardSeparator.IsVisible = forewardArrowShouldBeVisible;
                forewardButton.IsVisible = forewardArrowShouldBeVisible;
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