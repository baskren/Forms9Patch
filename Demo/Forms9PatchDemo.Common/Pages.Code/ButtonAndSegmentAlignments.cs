using System;
using Xamarin.Forms;
using Forms9Patch;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ButtonAndSegmentAlignments : Xamarin.Forms.ContentPage
    {

        const float radius = 4;
        const float width = 1;
        const bool hasShadow = true;
        static readonly Color outlineColor = Color.Default; // Color.Red.WithAlpha(0.25);
        static readonly Color backgroundColor = Color.White;
        static readonly bool ShadowInverted = true;

        readonly Switch _hasShadowSwitch = new Switch { IsToggled = hasShadow };

        readonly SegmentedControl _hzAlignmentElement = new SegmentedControl
        {
            ShadowInverted = ShadowInverted,
            HasShadow = hasShadow,
            BackgroundColor = backgroundColor,
            OutlineRadius = radius,
            OutlineWidth = width,
            OutlineColor = outlineColor,
            //SelectedTextColor = Color.Blue,

            Segments =
            {
                new Segment { Text = "START", },
                new Segment { Text = "CENTER" },
                new Segment { Text = "END" },
            }
        };

        readonly SegmentedControl _vtAlignmentElement = new SegmentedControl
        {
            ShadowInverted = ShadowInverted,
            HasShadow = hasShadow,
            BackgroundColor = backgroundColor,
            OutlineRadius = radius,
            OutlineWidth = width,
            OutlineColor = outlineColor,
            Segments =
            {
                new Segment { Text = "START", },
                new Segment { Text = "CENTER" },
                new Segment { Text = "END" },
            }
        };

        readonly SegmentedControl _optionsElement = new SegmentedControl
        {
            ShadowInverted = ShadowInverted,
            HasShadow = hasShadow,
            BackgroundColor = backgroundColor,
            GroupToggleBehavior = GroupToggleBehavior.Multiselect,
            OutlineRadius = radius,
            OutlineWidth = width,
            OutlineColor = outlineColor,
            Segments =
            {
                new Segment { Text = "TIGHT" },
                new Segment { Text = "TRAILING" },
                new Segment { Text = "VERTICAL" }
            }
        };

        readonly SegmentedControl _iconElement = new SegmentedControl
        {
            ShadowInverted = ShadowInverted,
            HasShadow = hasShadow,
            BackgroundColor = backgroundColor,
            HasTightSpacing = true,
            //FontSize = 10,
            OutlineRadius = radius,
            OutlineWidth = width,
            OutlineColor = outlineColor,
            Segments =
            {
                new Segment { Text = "NONE" },
                new Segment { IconText = "x"},
                new Segment { IconText = "©" },
                new Segment { IconText = "<font face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\"></font>" },
                new Segment { IconImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info"), TintColor=Color.Green }  }
            }
        };

        readonly Switch _imposedHeightSwitch = new Switch();

        readonly Slider _spacingSlider = new Slider(0, 50, 5);

        readonly Slider _outlineWidthSlider = new Slider(0, 7, width);

        readonly Slider _outlineRadiusSlider = new Slider(0, 16.0, radius);

        readonly Slider _iconFontSizeSlider = new Slider(0, 40, 12);

        readonly Slider _paddingSlider = new Slider(0, 20, 4);


        readonly Forms9Patch.StateButton _stateButton = new Forms9Patch.StateButton
        {
            ShadowInverted = ShadowInverted,
            BackgroundColor = backgroundColor,
            OutlineRadius = radius,
            OutlineWidth = width,
            OutlineColor = outlineColor,
            DefaultState = new ButtonState
            {
                Text = "StateButton",
                BackgroundImage = new Forms9Patch.Image("Forms9PatchDemo.Resources.button"),
                TextColor = Color.Black,
                HasShadow = hasShadow
            },
            SelectedState = new ButtonState
            {
                Text = "Selected",
                BackgroundImage = new Forms9Patch.Image("Forms9PatchDemo.Resources.ghosts") { Fill = Fill.Tile },
                OutlineColor = Color.Red,
                TextColor = Color.White,
            },
            ToggleBehavior = true
        };

        readonly Forms9Patch.Button _button = new Forms9Patch.Button
        {
            ShadowInverted = ShadowInverted,
            HasShadow = hasShadow,
            BackgroundColor = backgroundColor,
            OutlineRadius = radius,
            OutlineWidth = width,
            OutlineColor = outlineColor,
            Text = "H4 BUTTON",
        };

        readonly SegmentedControl _hzSegmentsElement = new SegmentedControl
        {
            ShadowInverted = ShadowInverted,
            HasShadow = hasShadow,
            BackgroundColor = backgroundColor,
            OutlineRadius = radius,
            OutlineWidth = width,
            OutlineColor = outlineColor,
            Segments =
                        {
                            new Segment
                            {
                                Text = "H1",
                            },
                            new Segment
                            {
                                Text = "H2"
                            },
                            new Segment
                            {
                                Text = "H3"
                            },
                        }
        };

        readonly SegmentedControl _vtSegmentsElement = new SegmentedControl
        {
            ShadowInverted = ShadowInverted,
            HasShadow = hasShadow,
            BackgroundColor = backgroundColor,
            OutlineRadius = radius,
            OutlineWidth = width,
            Orientation = StackOrientation.Vertical,
            OutlineColor = outlineColor,
            Segments =
                        {
                            new Segment
                            {
                                Text = "V1",
                            },
                            new Segment
                            {
                                Text = "V2"
                            },
                            new Segment
                            {
                                Text = "V3"
                            },
                        }
        };

        readonly Xamarin.Forms.Grid _grid1 = new Xamarin.Forms.Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(19) }, new ColumnDefinition { Width = GridLength.Star } },
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto } }
        };

        readonly Xamarin.Forms.Grid _grid2 = new Xamarin.Forms.Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star }, new ColumnDefinition { Width = GridLength.Star } },
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto } }
        };

        readonly Xamarin.Forms.Grid _grid3 = new Xamarin.Forms.Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star }, new ColumnDefinition { Width = GridLength.Star }, new ColumnDefinition { Width = GridLength.Star } },
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto } }
        };



        readonly Xamarin.Forms.Label _outlineWidthLabel = new Xamarin.Forms.Label { Text = "line W:" + width };
        readonly Xamarin.Forms.Label _outlineRadiusLabel = new Xamarin.Forms.Label { Text = "line R: " + radius };
        readonly Forms9Patch.Label _labelElement = new Forms9Patch.Label { Text = "Text" };

        public ButtonAndSegmentAlignments()
        {
            _iconFontSizeSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1));
            _paddingSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1));

            _grid1.Children.Add(new Xamarin.Forms.Label { Text = "HZ", VerticalTextAlignment = TextAlignment.Center, FontSize = 9 }, 0, 0);
            _grid1.Children.Add(_hzAlignmentElement, 1, 0);
            _grid1.Children.Add(new Xamarin.Forms.Label { Text = "VT", VerticalTextAlignment = TextAlignment.Center, FontSize = 9 }, 0, 1);
            _grid1.Children.Add(_vtAlignmentElement, 1, 1);
            _grid1.Children.Add(new Xamarin.Forms.Label { Text = "Opt", VerticalTextAlignment = TextAlignment.Center, FontSize = 9 }, 0, 2);
            _grid1.Children.Add(_optionsElement, 1, 2);
            _grid1.Children.Add(new Xamarin.Forms.Label { Text = "Icon", VerticalTextAlignment = TextAlignment.Center, FontSize = 9 }, 0, 3);
            _grid1.Children.Add(_iconElement, 1, 3);

            _grid2.Children.Add(new Xamarin.Forms.Label { Text = "Spacing:" }, 0, 0);
            _grid2.Children.Add(_spacingSlider, 0, 1);
            _grid2.Children.Add(new Xamarin.Forms.Label { Text = "Imposed Ht:" }, 1, 0);
            _grid2.Children.Add(_imposedHeightSwitch, 1, 1);
            _grid2.Children.Add(_outlineWidthLabel, 2, 0);
            _grid2.Children.Add(_outlineWidthSlider, 2, 1);
            _grid2.Children.Add(_outlineRadiusLabel, 3, 0);
            _grid2.Children.Add(_outlineRadiusSlider, 3, 1);

            _grid3.Children.Add(new Xamarin.Forms.Label { Text = "HasShadow:" }, 0, 0);
            _grid3.Children.Add(_hasShadowSwitch, 0, 1);
            _grid3.Children.Add(new Forms9Patch.Label("Icon Font Size:"), 1, 0);
            _grid3.Children.Add(_iconFontSizeSlider, 1, 1);
            _grid3.Children.Add(new Xamarin.Forms.Label { Text = "Padding:" }, 2, 0);
            _grid3.Children.Add(_paddingSlider, 2, 1);

            Padding = new Thickness(40, 20, 20, 20);
            Content = new Xamarin.Forms.ScrollView
            {
                Content = new Xamarin.Forms.StackLayout
                {
                    Children =
                    {

                        new Xamarin.Forms.Label { Text = "version 0.9.15.1"},
                        new BoxView { HeightRequest = 1, Color = Color.Gray },
                        _grid1,
                        new BoxView { HeightRequest = 1, Color = Color.Gray },
                        _grid2,
                        new BoxView { HeightRequest = 1, Color = Color.Gray },
                        _grid3,
                        
                        

                        new BoxView { HeightRequest = 1, Color = Color.Black },

                        _stateButton,
                        new BoxView { HeightRequest = 1, Color = Color.Gray },
                        _button,
                        new BoxView { HeightRequest = 1, Color = Color.Gray },
                        _hzSegmentsElement,
                        new BoxView { HeightRequest = 1, Color = Color.Gray },
                        _vtSegmentsElement,
                        new BoxView { HeightRequest = 1, Color = Color.Gray },
                        new Xamarin.Forms.Label { Text="Display.Scale=["+Display.Scale+"]" },
                        new BoxView { HeightRequest = 1, Color = Color.Gray },

                    }
                }
            };

            _paddingSlider.ValueChanged += (sender, e) =>
            {
                _hzAlignmentElement.Padding = e.NewValue;
                _vtAlignmentElement.Padding = e.NewValue;
                _optionsElement.Padding = e.NewValue;
                _iconElement.Padding = e.NewValue;
                _button.Padding = e.NewValue;
                _stateButton.Padding = e.NewValue;
                _hzSegmentsElement.Padding = e.NewValue;
                _vtSegmentsElement.Padding = e.NewValue;
            };

            _iconFontSizeSlider.ValueChanged += (sender, e) =>
            {
                _hzAlignmentElement.IconFontSize = e.NewValue;
                _vtAlignmentElement.IconFontSize = e.NewValue;
                _optionsElement.IconFontSize = e.NewValue;
                _iconElement.IconFontSize = e.NewValue;
                _button.IconFontSize = e.NewValue;
                _stateButton.IconFontSize = e.NewValue;
                _hzSegmentsElement.IconFontSize = e.NewValue;
                _vtSegmentsElement.IconFontSize = e.NewValue;
            };

            _hasShadowSwitch.Toggled += (sender, e) =>
            {
                _hzAlignmentElement.HasShadow = _hasShadowSwitch.IsToggled;
                _vtAlignmentElement.HasShadow = _hasShadowSwitch.IsToggled;
                _optionsElement.HasShadow = _hasShadowSwitch.IsToggled;
                _iconElement.HasShadow = _hasShadowSwitch.IsToggled;
                _button.HasShadow = _hasShadowSwitch.IsToggled;
                _stateButton.DefaultState.HasShadow = _hasShadowSwitch.IsToggled;
                _hzSegmentsElement.HasShadow = _hasShadowSwitch.IsToggled;
                _vtSegmentsElement.HasShadow = _hasShadowSwitch.IsToggled;
            };

            _hzAlignmentElement.SegmentTapped += (sender, e) =>
            {
                var buttonText = string.Concat(e.Segment.Text.ToUpper().Substring(0, 1), e.Segment.Text.ToLower().Substring(1));
                if (!Enum.TryParse<TextAlignment>(buttonText, out TextAlignment alignment))
                    throw new Exception("doh");
                _labelElement.HorizontalTextAlignment = alignment;
                _button.HorizontalTextAlignment = alignment;
                _stateButton.DefaultState.HorizontalTextAlignment = alignment;
                _hzSegmentsElement.HorizontalTextAlignment = alignment;
                _vtSegmentsElement.HorizontalTextAlignment = alignment;
            };

            _vtAlignmentElement.SegmentTapped += (sender, e) =>
            {
                var buttonText = string.Concat(e.Segment.Text.ToUpper().Substring(0, 1), e.Segment.Text.ToLower().Substring(1));
                if (!Enum.TryParse<TextAlignment>(buttonText, out TextAlignment alignment))
                    throw new Exception("doh");
                _labelElement.VerticalTextAlignment = alignment;
                _button.VerticalTextAlignment = alignment;
                _stateButton.DefaultState.VerticalTextAlignment = alignment;
                _hzSegmentsElement.VerticalTextAlignment = alignment;
                _vtSegmentsElement.VerticalTextAlignment = alignment;
            };

            _optionsElement.SegmentTapped += (sender, e) =>
            {
                var options = new List<string>();
                foreach (var segment in _optionsElement.SelectedSegments)
                    options.Add(segment.Text.ToUpper());
                var hasTightSpacing = options.Contains("TIGHT");
                var trailingIcon = options.Contains("TRAILING");
                var orientation = options.Contains("VERTICAL") ? StackOrientation.Vertical : StackOrientation.Horizontal;


                _button.HasTightSpacing = hasTightSpacing;
                _button.TrailingIcon = trailingIcon;
                _button.Orientation = orientation;

                _stateButton.DefaultState.HasTightSpacing = hasTightSpacing;
                _stateButton.DefaultState.TrailingIcon = trailingIcon;
                _stateButton.DefaultState.Orientation = orientation;

                _hzSegmentsElement.HasTightSpacing = hasTightSpacing;
                _hzSegmentsElement.TrailingIcon = trailingIcon;
                _hzSegmentsElement.IntraSegmentOrientation = orientation;

                _vtSegmentsElement.HasTightSpacing = hasTightSpacing;
                _vtSegmentsElement.TrailingIcon = trailingIcon;
                _vtSegmentsElement.IntraSegmentOrientation = orientation;
            };

            _iconElement.SegmentTapped += (sender, e) =>
            {
                //if (e.Segment.IconImage != null)
                //{
                /*
                _iconTextAndTextButton.IconImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info") };
                foreach (var segment in _hzSegmentsElement.Segments)
                    segment.IconImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info") };
                foreach (var segment in _vtSegmentsElement.Segments)
                    segment.IconImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info") };
                    */
                SetIconImage(e.Segment.IconImage);
                //}
                //else
                SetIconText(e.Segment.IconText);
            };

            _spacingSlider.ValueChanged += (sender, e) =>
            {
                _button.Spacing = _spacingSlider.Value;
                //_stateButton.Spacing = _spacingSlider.Value;
                _stateButton.DefaultState.Spacing = _spacingSlider.Value;
                _hzSegmentsElement.IntraSegmentSpacing = _spacingSlider.Value;
                _vtSegmentsElement.IntraSegmentSpacing = _spacingSlider.Value;
            };

            _imposedHeightSwitch.Toggled += (sender, e) =>
            {
                _button.HeightRequest = _imposedHeightSwitch.IsToggled ? 60 : -1;
                _stateButton.HeightRequest = _imposedHeightSwitch.IsToggled ? 60 : -1;
                _hzSegmentsElement.HeightRequest = _imposedHeightSwitch.IsToggled ? 60 : -1;
                _vtSegmentsElement.HeightRequest = _imposedHeightSwitch.IsToggled ? 180 : -1;

                _vtAlignmentElement.IsEnabled = _imposedHeightSwitch.IsToggled;
                if (!_vtAlignmentElement.IsEnabled)
                    _vtAlignmentElement.DeselectAll();


            };

            _outlineRadiusSlider.ValueChanged += (sender, e) =>
            {
                int value = (int)_outlineRadiusSlider.Value;
                _hzAlignmentElement.OutlineRadius = value;
                _vtAlignmentElement.OutlineRadius = value;
                _button.OutlineRadius = value;
                _stateButton.DefaultState.OutlineRadius = value;
                _optionsElement.OutlineRadius = value;
                _iconElement.OutlineRadius = value;
                _hzSegmentsElement.OutlineRadius = value;
                _vtSegmentsElement.OutlineRadius = value;
                _outlineRadiusLabel.Text = "line R: " + value;
            };

            _outlineWidthSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(0.5));
            _outlineWidthSlider.ValueChanged += (sender, e) =>
            {
                float value = (float)_outlineWidthSlider.Value; // (float)(Math.Round(_outlineWidthSlider.Value*2.0)/2.0);
                _hzAlignmentElement.OutlineWidth = value;
                _vtAlignmentElement.OutlineWidth = value;
                _button.OutlineWidth = value;
                _stateButton.DefaultState.OutlineWidth = value;
                _optionsElement.OutlineWidth = value;
                _iconElement.OutlineWidth = value;
                _hzSegmentsElement.OutlineWidth = value;
                _vtSegmentsElement.OutlineWidth = value;
                _outlineWidthLabel.Text = "line W: " + _outlineWidthSlider.Value;// + value;
            };

            var defaultHzAlignment = _button.HorizontalTextAlignment;
            if (defaultHzAlignment == TextAlignment.Start)
                _hzAlignmentElement.SelectIndex(0);
            else if (defaultHzAlignment == TextAlignment.Center)
                _hzAlignmentElement.SelectIndex(1);
            else
                _hzAlignmentElement.SelectIndex(2);


            _iconElement.SelectIndex(0);
            _vtAlignmentElement.IsEnabled = false;
        }

        void SetIconText(string iconTextSetting)
        {
            if (iconTextSetting == "NONE")
                _button.IconText = null;
            else
                _button.IconText = iconTextSetting;
            _stateButton.DefaultState.IconText = _button.IconText;
            foreach (var segment in _hzSegmentsElement.Segments)
                segment.IconText = _button.IconText;
            foreach (var segment in _vtSegmentsElement.Segments)
                segment.IconText = _button.IconText;

        }

        void SetIconImage(Forms9Patch.Image image)
        {
            var source = image?.Source;
            if (source != null)
            {
                _button.IconImage = new Forms9Patch.Image((Xamarin.Forms.ImageSource)source);
                _stateButton.DefaultState.IconImage = new Forms9Patch.Image((Xamarin.Forms.ImageSource)source);
                foreach (var segment in _hzSegmentsElement.Segments)
                    segment.IconImage = new Forms9Patch.Image((Xamarin.Forms.ImageSource)source); //new Forms9Patch.Image(image);
                foreach (var segment in _vtSegmentsElement.Segments)
                    segment.IconImage = new Forms9Patch.Image((Xamarin.Forms.ImageSource)source);
            }
            else
            {
                _button.IconImage = null;
                _stateButton.DefaultState.IconImage = null;
                foreach (var segment in _hzSegmentsElement.Segments)
                    segment.IconImage = null;
                foreach (var segment in _vtSegmentsElement.Segments)
                    segment.IconImage = null;
            }
        }
    }
}
