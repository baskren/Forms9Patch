using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch.TargetedMenu class 
    /// </summary>
    [ContentProperty("Segments")]
    public class TargetedMenu : BubblePopup
    {
        static Color DefaultBackgroundColor = Color.FromRgb(51, 51, 51);
        static Color DefaultTextColor = Color.White;
        static Color DefaultSeparatorColor = DefaultTextColor;
        static double DefaultSeparatorWidth = 1.0;

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

        #region FontSize property
        /// <summary>
        /// backing store for FontSize property
        /// </summary>
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create("FontSize", typeof(double), typeof(TargetedMenu), -1.0);
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
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Xamarin.Forms.Color), typeof(TargetedMenu), DefaultTextColor);
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
        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create("SeparatorColor", typeof(Color), typeof(TargetedMenu), DefaultSeparatorColor);
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
        public static readonly BindableProperty SeparatorWidthProperty = BindableProperty.Create("SeparatorWidth", typeof(double), typeof(TargetedMenu), DefaultSeparatorWidth);
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
        public static readonly BindableProperty HapticEffectProperty = BindableProperty.Create("HapticEffect", typeof(HapticEffect), typeof(TargetedMenu), default(HapticEffect));
        /// <summary>
        /// Gets/Sets the HapticEffect property
        /// </summary>
        public HapticEffect HapticEffect
        {
            get => (HapticEffect)GetValue(HapticEffectProperty);
            set => SetValue(HapticEffectProperty, value);
        }
        #endregion HapticEffect property

        #region HapticMode property
        /// <summary>
        /// backing store for HapticMode property
        /// </summary>
        public static readonly BindableProperty HapticModeProperty = BindableProperty.Create("HapticMode", typeof(KeyClicks), typeof(TargetedMenu), default(KeyClicks));
        /// <summary>
        /// Gets/Sets the HapticMode property
        /// </summary>
        public KeyClicks HapticMode
        {
            get => (KeyClicks)GetValue(HapticModeProperty);
            set => SetValue(HapticModeProperty, value);
        }
        #endregion HapticMode property

        #endregion


        #region Fields


        //readonly List<Button> _buttons;
        //readonly List<Size> _buttonSizes;
        int _currentPage;
        readonly Button _leftArrowButton = new Button
        {
            TextColor = DefaultTextColor,
            FontSize = 24,
            TintIcon = true,
            IconImage = new Forms9Patch.Image("Forms9Patch.Resources.ic_navigate_before_white_24px.svg") { Fill = Fill.AspectFill, WidthRequest = 24, HeightRequest = 16 },
            HasTightSpacing = true,
            Padding = Device.RuntimePlatform == Device.UWP ? new Thickness(4, 0, 4, 4) : new Thickness(10, 0),
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Start,
            BackgroundColor = DefaultBackgroundColor.WithAlpha(0.05),
            Lines = 1,
            AutoFit = AutoFit.None,
            IsVisible = true,
            //WidthRequest = 30,
            //HeightRequest = 30,
        };
        readonly BoxView _leftArrowSeparator = new BoxView { Color = DefaultSeparatorColor, WidthRequest = DefaultSeparatorWidth };
        readonly Button _rightArrowButton = new Button
        {
            TextColor = DefaultTextColor,
            FontSize = 24,
            TintIcon = true,
            IconImage = new Forms9Patch.Image("Forms9Patch.Resources.ic_navigate_next_white_24px.svg") { Fill = Fill.AspectFill, WidthRequest = 24, HeightRequest = 16 },
            HasTightSpacing = true,
            Padding = Device.RuntimePlatform == Device.UWP ? new Thickness(4, 0, 4, 4) : new Thickness(10, 0),
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Center,
            BackgroundColor = DefaultBackgroundColor.WithAlpha(0.05),
            Lines = 1,
            AutoFit = AutoFit.None,
            IsVisible = true,
            //WidthRequest = 30,
            //HeightRequest = 30,
        };

        // this is crazy but the stack layout doesn't give the children the correct space if it isn't included.
        readonly BoxView _rightArrowSeparator = new BoxView { Color = DefaultSeparatorColor, WidthRequest = 0.05 };

        Xamarin.Forms.StackLayout _stackLayout = new Xamarin.Forms.StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 0,
            Padding = 0,
            Margin = 0,
            BackgroundColor = Color.Transparent,
            HorizontalOptions = LayoutOptions.Center,
            //BackgroundColor = Color.White,
            //OutlineColor = DefaultBackgroundColor,
            //OutlineWidth = 1,
            //OutlineRadius = 4,
        };
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
            targetedMenu.IsVisible = true;
            return targetedMenu;
        }

        void Init()
        {
            PointerDirection = PointerDirection.Any;
            PreferredPointerDirection = PointerDirection.Down;
            PointerLength = 10;
            BackgroundColor = DefaultBackgroundColor;
            HasShadow = false;
            Padding = 0;
            //Margin = 0;
            OutlineRadius = 5;
            PageOverlayColor = Color.Black.WithAlpha(0.1);
            _segments.CollectionChanged += OnSegmentsCollectionChanged;
            SizeChanged += OnSizeChanged;
            Content = _stackLayout;

            _leftArrowButton.Clicked += OnLeftArrowButtonClicked;
            _rightArrowButton.Clicked += OnRightArrowButtonClicked;


            _leftArrowButton.AutomationId = "Forms9Patch.TargetedMenu.LeftButton";
            _rightArrowButton.AutomationId = "Forms9Patch.TargetedMenu.RightButton";

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
            {
                foreach (VisualElement visualElement in _stackLayout.Children)
                {
                    if (visualElement is Button button)
                        button.BackgroundColor = BackgroundColor;
                }
            }
            else if (propertyName == TextColorProperty.PropertyName)
            {
                foreach (VisualElement visualElement in _stackLayout.Children)
                    if (visualElement is Button button)
                        button.TextColor = TextColor;
            }
            else if (propertyName == FontSizeProperty.PropertyName)
            {
                foreach (VisualElement visualElement in _stackLayout.Children)
                    if (visualElement is Button button)
                        button.FontSize = FontSize;
            }
            //else if (propertyName == SeparatorColorProperty.PropertyName)
            //     _stackLayout.BackgroundColor = SeparatorColor;
            //else if (propertyName == OutlineColorProperty.PropertyName)
            //    _stackLayout.OutlineColor = OutlineColor == Color.Default || OutlineColor == Color.Transparent ? BackgroundColor : OutlineColor;

            //else if (propertyName == OutlineRadiusProperty.PropertyName)
            //    _stackLayout.OutlineRadius = OutlineRadius + 1;
            //else if (propertyName == SeparatorWidthProperty.PropertyName)
            //    _stackLayout.Spacing = SeparatorWidth;
            else if (propertyName == HapticEffectProperty.PropertyName)
            {
                foreach (VisualElement visualElement in _stackLayout.Children)
                    if (visualElement is Button button)
                        button.HapticEffect = HapticEffect;
            }
            else if (propertyName == HapticModeProperty.PropertyName)
            {
                foreach (VisualElement visualElement in _stackLayout.Children)
                    if (visualElement is Button button)
                        button.HapticMode = HapticMode;
            }
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
            segment._button.Padding = Device.RuntimePlatform == Device.UWP ? new Thickness(8, 4, 8, 8) : new Thickness(8, 0);
            segment._button.TextColor = TextColor;
            segment._button.VerticalTextAlignment = TextAlignment.Center;
            segment._button.HorizontalTextAlignment = TextAlignment.Center;
            segment._button.VerticalOptions = LayoutOptions.Center;
            segment._button.HorizontalOptions = LayoutOptions.Center;
            segment._button.BackgroundColor = BackgroundColor.WithAlpha(0.02);
            if (Device.RuntimePlatform != Device.UWP)
                segment._button.FontSize = 12;
            //segment.
            segment._button.Lines = 1;
            segment._button.AutoFit = AutoFit.None;

            segment._button.SizeChanged += OnButtonSizeChanged;
            segment._button.Tapped += OnButtonTapped;
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

            bool first = true;
            foreach (var segment in Segments)
            {
                if (!first)
                    _stackLayout.Children.Add(new BoxView { Color = SeparatorColor, WidthRequest = DefaultSeparatorWidth });
                _stackLayout.Children.Add(segment._button);
                first = false;
            }

            _stackLayout.Children.Add(_rightArrowSeparator);
            _stackLayout.Children.Add(_rightArrowButton);

            UpdateButtonVisibilities();
        }
        #endregion


        #region Layout

        private void OnSizeChanged(object sender, System.EventArgs e)
        {
            UpdateButtonVisibilities();
        }

        private void OnButtonSizeChanged(object sender, System.EventArgs e)
        {
            UpdateButtonVisibilities();
        }

        void UpdateButtonVisibilities()
        {
            if (P42.Utils.Environment.IsOnMainThread)
            {
                if (Width < 0)
                    return;

                bool rightArrowShouldBeVisible = true;

                var leftWidth = _leftArrowButton.UnexpandedTightSize.Width + SeparatorWidth;
                var rightWidth = _rightArrowButton.UnexpandedTightSize.Width + SeparatorWidth;

                // calculate pages
                var pageWidth = 0.0;

                _leftArrowButton.IsVisible = _currentPage > 0;
                _leftArrowSeparator.IsVisible = _currentPage > 0;

                int segmentIndex = 0;
                var pageIndex = 0;
                for (int i = 2; i < _stackLayout.Children.Count - 2; i += 2)
                {
                    var button = _stackLayout.Children[i] as Forms9Patch.Button;
                    var separator = _stackLayout.Children[i + 1] as BoxView;

                    pageWidth += button.UnexpandedTightSize.Width + (SeparatorWidth) + (_stackLayout.Spacing) + 10;

                    if (segmentIndex < Segments.Count && pageWidth + rightWidth >= (Width - Padding.HorizontalThickness - Margin.HorizontalThickness))//* 0.75)
                    {
                        pageIndex++;
                        pageWidth = leftWidth;
                    }

                    button.IsVisible = pageIndex == _currentPage;
                    separator.IsVisible = pageIndex == _currentPage;

                    rightArrowShouldBeVisible &= (pageIndex != _currentPage || segmentIndex != Segments.Count - 1);
                    segmentIndex++;
                }

                _rightArrowSeparator.IsVisible = rightArrowShouldBeVisible;
                _rightArrowButton.IsVisible = rightArrowShouldBeVisible;
                _rightArrowButton.TextColor = TextColor;
            }
            else
                Device.BeginInvokeOnMainThread(UpdateButtonVisibilities);
        }

        #endregion


        #region Events
        void OnLeftArrowButtonClicked(object sender, EventArgs e)
        {
            _currentPage--;
            UpdateButtonVisibilities();
        }

        void OnRightArrowButtonClicked(object sender, EventArgs e)
        {
            _currentPage++;
            UpdateButtonVisibilities();
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