using System;
using Xamarin.Forms;
using Forms9Patch;
using System.Linq;
using System.Threading.Tasks;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class VariableWidthButtonMasterDetailPageA : MasterDetailPage
    {
        ContentPage switchPage = new ContentPage
        {
            Title = "Swap Content Page",
            Content = new Xamarin.Forms.Label { Text = "CONTENT" }
        };

        VariableWidthButtonPage contentPage = new VariableWidthButtonPage();

        public VariableWidthButtonMasterDetailPageA()
        {
            Detail = contentPage;
            Master = switchPage;
        }
    }

    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class VariableWidthButtonMasterDetailPageB : MasterDetailPage
    {
        ContentPage switchPage = new ContentPage
        {
            Title = "Swap Content Page",
            Content = new Xamarin.Forms.Label { Text = "CONTENT" }
        };

        VariableWidthButtonPage contentPage = new VariableWidthButtonPage();

        public VariableWidthButtonMasterDetailPageB()
        {
            Detail = switchPage;
            Master = contentPage;
        }
    }

    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class VariableWidthButtonPage : ContentPage
    {
        #region VisualElements
        readonly Forms9Patch.Button _f9pButton = new Forms9Patch.Button
        {
            BackgroundColor = Color.DarkGray,
            Text = "F9P Button",
            OutlineWidth = 0,
            HorizontalOptions = LayoutOptions.Center
        };

        readonly Xamarin.Forms.Button _xfButton = new Xamarin.Forms.Button
        {
            BackgroundColor = Color.DarkGray,
            Text = "XF Button",
            HorizontalOptions = LayoutOptions.Center,
        };

        readonly Slider _slider = new Xamarin.Forms.Slider
        {
            Value = 100,
            Maximum = 800,
            Minimum = 100,
        };

        readonly BoxView _boxView = new BoxView
        {
            Color = Color.Transparent,
        };

        readonly Switch _longPressEnabledSwitch = new Switch() { IsToggled = true };

        readonly Switch _darkBackgroundSwitch = new Switch() { IsToggled = true };

        readonly SegmentedControl _layoutTypeControl = new SegmentedControl
        {
            GroupToggleBehavior = GroupToggleBehavior.Radio,
            Segments =
            {
                new Segment("StackLayout"),
                new Segment("Grid")
            }
        };

        readonly SegmentedControl _touchType1 = new SegmentedControl
        {
            GroupToggleBehavior = GroupToggleBehavior.Multiselect,
            Segments =
            {
                new Segment(nameof(FormsGestures.Listener.Down)),
                new Segment(nameof(FormsGestures.Listener.Up)),
                new Segment(nameof(FormsGestures.Listener.Tapping)),
                new Segment(nameof(FormsGestures.Listener.Tapped)),
                new Segment(nameof(FormsGestures.Listener.DoubleTapped)),
                new Segment(nameof(FormsGestures.Listener.LongPressing)),
                new Segment(nameof(FormsGestures.Listener.LongPressed)),
            }
        };
        readonly SegmentedControl _touchType2 = new SegmentedControl
        {
            GroupToggleBehavior = GroupToggleBehavior.Multiselect,
            Segments =
            {
                new Segment(nameof(FormsGestures.Listener.Pinching)),
                new Segment(nameof(FormsGestures.Listener.Pinched)),
                new Segment(nameof(FormsGestures.Listener.Panning)),
                new Segment(nameof(FormsGestures.Listener.Panned)),
                new Segment(nameof(FormsGestures.Listener.Swiped)),
                new Segment(nameof(FormsGestures.Listener.Rotating)),
                new Segment(nameof(FormsGestures.Listener.Rotated))
            }
        };

        readonly Switch _pagePadding = new Switch() { IsToggled = true };

        readonly ScrollView scrollView = new ScrollView();

        readonly Switch _inScrollViewSwitch = new Switch() { IsToggled = false };
        #endregion

        public VariableWidthButtonPage()
        {
            Title = "Variable Width Button Page";

            _layoutTypeControl.SelectIndex(0);
            _touchType1.SelectIndex(0);

            _slider.ValueChanged += (s, e) =>
            {
                _xfButton.WidthRequest = _slider.Value;
                _f9pButton.WidthRequest = _slider.Value;
            };
            _xfButton.WidthRequest = _slider.Value;
            _f9pButton.WidthRequest = _slider.Value;


            _xfButton.Clicked += (s, e) => FlashBox(Color.Green);
            _f9pButton.Clicked += (s, e) => FlashBox(Color.Green);
            _f9pButton.LongPressing += (s, e) => FlashBox(Color.Yellow);
            _f9pButton.LongPressed += (s, e) => FlashBox(Color.Red);

            _f9pButton.IsLongPressEnabled = _longPressEnabledSwitch.IsToggled;
            _longPressEnabledSwitch.Toggled += (s, e) => _f9pButton.IsLongPressEnabled = _longPressEnabledSwitch.IsToggled;

            UpdateButtonsBackgroundColor();
            _darkBackgroundSwitch.Toggled += (s, e) => UpdateButtonsBackgroundColor();

            SetContent();
            _layoutTypeControl.SegmentSelected += (s, e) => SetContent();

            _inScrollViewSwitch.Toggled += (s, e) =>
            {
                Task.Run(() => SetScrollViewEmbed());
            };

            SetPagePadding();
            _pagePadding.Toggled += (s, e) => SetPagePadding();

        }

        Xamarin.Forms.StackLayout _stack;
        Xamarin.Forms.Grid _grid;
        void SetContent()
        {
            View content = null;
            if (_layoutTypeControl.SelectedSegments[0].Text == "StackLayout")
            {
                _grid?.Children.Clear();
                if (_stack == null)
                {
                    _stack = new Xamarin.Forms.StackLayout
                    {
                        BackgroundColor = Color.LightBlue,
                        Padding = 0,
                        Margin = 0,
                        Spacing = 10,
                    };
                    var stackListener = FormsGestures.Listener.For(_stack);
                    SetupTouchEvents(stackListener);
                }
                _stack.Children.Add(_boxView);
                _stack.Children.Add(_slider);
                _stack.Children.Add(new Xamarin.Forms.StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                            {
                                new Xamarin.Forms.Label { Text = "Content in ScrollView"},
                                _inScrollViewSwitch,
                            }
                });
                _stack.Children.Add(new Xamarin.Forms.StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                            {
                                new Xamarin.Forms.Label { Text = "Long Press Enabled"},
                                _longPressEnabledSwitch,
                            }
                });
                _stack.Children.Add(new Xamarin.Forms.StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                            {
                                new Xamarin.Forms.Label { Text = "Dark Background"},
                                _darkBackgroundSwitch,
                            }
                });
                _stack.Children.Add(new Xamarin.Forms.StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                            {
                                new Xamarin.Forms.Label { Text = "Page Padding"},
                                _pagePadding,
                            }
                });
                _stack.Children.Add(_f9pButton);
                _stack.Children.Add(_xfButton);
                _stack.Children.Add(_layoutTypeControl);
                _stack.Children.Add(_touchType1);
                _stack.Children.Add(_touchType2);

                content = _stack;
            }
            else
            {
                _stack?.Children.Clear();
                if (_grid == null)
                {
                    _grid = new Xamarin.Forms.Grid
                    {
                        BackgroundColor = Color.LightGreen,
                        RowDefinitions = new RowDefinitionCollection
                        {
                            new RowDefinition{Height = 40 },
                            new RowDefinition{Height = 40 },
                            new RowDefinition{Height = 40 },
                            new RowDefinition{Height = 40 },
                            new RowDefinition{Height = 40 },
                            new RowDefinition{Height = 40 },
                            new RowDefinition{Height = 40 },
                            new RowDefinition{Height = 40 },
                            new RowDefinition{Height = 40 },
                            new RowDefinition{Height = 40 },
                            new RowDefinition{Height = 40 },
                        },
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition{ Width = new GridLength(2, GridUnitType.Star) },
                            new ColumnDefinition{ Width = GridLength.Star }
                        }
                    };
                    var gridListener = FormsGestures.Listener.For(_grid);
                    SetupTouchEvents(gridListener);
                }
                int row = 0;
                _grid.Children.Add(_boxView, 0, 2, row, ++row);
                _grid.Children.Add(_slider, 0, 2, row, ++row);
                _grid.Children.Add(new Xamarin.Forms.Label { Text = "Content in ScrollView" }, 0, 1, row, row + 1);
                _grid.Children.Add(_inScrollViewSwitch, 1, 2, row, ++row);
                _grid.Children.Add(new Xamarin.Forms.Label { Text = "Long Press Enabled" }, 0, 1, row, row + 1);
                _grid.Children.Add(_longPressEnabledSwitch, 1, 2, row, ++row);
                _grid.Children.Add(new Xamarin.Forms.Label { Text = "Dark Background" }, 0, 1, row, row + 1);
                _grid.Children.Add(_darkBackgroundSwitch, 1, 2, row, ++row);
                _grid.Children.Add(new Xamarin.Forms.Label { Text = "Page Padding" }, 0, 1, row, row + 1);
                _grid.Children.Add(_pagePadding, 1, 2, row, ++row);
                _grid.Children.Add(_f9pButton, 0, 2, row, ++row);
                _grid.Children.Add(_xfButton, 0, 2, row, ++row);
                _grid.Children.Add(_layoutTypeControl, 0, 2, row, ++row);
                _grid.Children.Add(_touchType1, 0, 2, row, ++row);
                _grid.Children.Add(_touchType2, 0, 2, row, ++row);
                content = _grid;
            }
            if (_inScrollViewSwitch.IsToggled)
                scrollView.Content = content;
            else
                Content = content;
        }

        void SetScrollViewEmbed()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (_inScrollViewSwitch.IsToggled && Content != scrollView)
                {
                    var content = Content;
                    Content = null;
                    scrollView.Content = content;
                    Content = scrollView;
                }
                else if (!_inScrollViewSwitch.IsToggled && Content == scrollView)
                {
                    var content = scrollView.Content;
                    scrollView.Content = null;
                    Content = content;
                }
            });
        }

        void SetupTouchEvents(FormsGestures.Listener listener)
        {
            listener.Down += ShowTouch;
            listener.Up += ShowTouch;
            listener.Tapping += ShowTouch;
            listener.Tapped += ShowTouch;
            listener.DoubleTapped += ShowTouch;
            listener.LongPressing += ShowTouch;
            listener.LongPressed += ShowTouch;
        }

        void ShowTouch(object sender, FormsGestures.BaseGestureEventArgs e)
        {
            var point = e.WindowTouches[0];
            if (e.Event == nameof(FormsGestures.Listener.Down) && _touchType1.SelectedSegments.Any(s => s.Text == e.Event))
                PopBox(point, Color.Red);
            else if (e.Event == nameof(FormsGestures.Listener.Up) && _touchType1.SelectedSegments.Any(s => s.Text == e.Event))
                PopBox(point, Color.Blue);
            else if (e.Event == nameof(FormsGestures.Listener.Tapping) && _touchType1.SelectedSegments.Any(s => s.Text == e.Event))
                PopBox(point, Color.Yellow);
            else if (e.Event == nameof(FormsGestures.Listener.Tapped) && _touchType1.SelectedSegments.Any(s => s.Text == e.Event))
                PopBox(point, Color.Green);
            else if (e.Event == nameof(FormsGestures.Listener.DoubleTapped) && _touchType1.SelectedSegments.Any(s => s.Text == e.Event))
                PopBox(point, Color.Orange);
            else if (e.Event == nameof(FormsGestures.Listener.LongPressing) && _touchType1.SelectedSegments.Any(s => s.Text == e.Event))
                PopBox(point, Color.Magenta);
            else if (e.Event == nameof(FormsGestures.Listener.LongPressed) && _touchType1.SelectedSegments.Any(s => s.Text == e.Event))
                PopBox(point, Color.Purple);
        }

        Color ButtonBackgroundColor => _darkBackgroundSwitch.IsToggled
                ? Color.Gray
                : Color.LightGray;

        void UpdateButtonsBackgroundColor()
        {
            _f9pButton.BackgroundColor = ButtonBackgroundColor;
            _xfButton.BackgroundColor = ButtonBackgroundColor;
        }

        void FlashBox(Color color)
        {
            _boxView.Color = color;
            var value = 1.0;
            Device.StartTimer(TimeSpan.FromMilliseconds(20), () =>
             {
                 value -= 0.1;
                 _boxView.Color = Color.Transparent.RgbaBlend(color, value);
                 return value > 0;
             });
        }

        void PopBox(Point point, Color color)
        {
            var box = new BoxView
            {
                Margin = 0,
                TranslationX = point.X - 20,
                TranslationY = point.Y - 20,
                WidthRequest = 40,
                HeightRequest = 40,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Color = color,
                InputTransparent = true,
            };
            var popup = new Forms9Patch.Elements.Popups.Core.PopupPage
            {
                IsAnimationEnabled = false,
                HasSystemPadding = false,
                Content = box,
                Padding = 0,
                IsVisible = true,
                BackgroundColor = Color.Transparent
            };

            Forms9Patch.Elements.Popups.Core.PopupNavigation.Instance.PushAsync(popup);

            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
            {
                box.TranslationX += 1;
                box.TranslationY += 1;
                box.WidthRequest -= 2;
                box.HeightRequest -= 2;

                if (box.WidthRequest < 1 || box.HeightRequest < 1)
                {
                    Forms9Patch.Elements.Popups.Core.PopupNavigation.Instance.PopAsync();
                    return false;
                }
                return true;
            });

        }

        void SetPagePadding()
        {
            Padding = _pagePadding.IsToggled ? 40 : 0;
        }

    }
}