using System;
using Xamarin.Forms;
using Forms9Patch;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;

namespace Forms9PatchDemo
{
    public class MaterialButtonAndSegmentAlignments : ContentPage
    {


        MaterialSegmentedControl _hzAlignmentElement = new MaterialSegmentedControl
        {
            Segments =
            {
                new Segment { Text = "START", },
                new Segment { Text = "CENTER" },
                new Segment { Text = "END" },
            }
        };

        MaterialSegmentedControl _vtAlignmentElement = new MaterialSegmentedControl
        {
            Segments =
            {
                new Segment { Text = "START", },
                new Segment { Text = "CENTER" },
                new Segment { Text = "END" },
            }
        };

        MaterialSegmentedControl _optionsElement = new MaterialSegmentedControl
        {
            GroupToggleBehavior = GroupToggleBehavior.Multiselect,
            Segments =
            {
                new Segment { Text = "TIGHT" },
                new Segment { Text = "TRAILING" },
                new Segment { Text = "VERTICAL" }
            }
        };

        MaterialSegmentedControl _iconElement = new MaterialSegmentedControl
        {
            HasTightSpacing = true,
            //FontSize = 10,
            Segments =
            {
                new Segment { HtmlText = "NONE" },
                new Segment { HtmlText = "x"},
                new Segment { HtmlText = "©" },
                new Segment { HtmlText = "<font face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\"></font>" },
                new Segment { ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info") }
            }
        };



        Switch _imposedHeightSwitch = new Switch();

        Slider _spacingSlider = new Slider
        {
            Maximum = 50,
            Minimum = 0,
            Value = 5,

        };

        MaterialButton _iconTextAndTextButton = new MaterialButton
        {
            Text = "Text",
        };


        MaterialSegmentedControl _hzSegmentsElement = new MaterialSegmentedControl
        {
            Segments =
                        {
                            new Segment
                            {
                                Text = "T1",
                            },
                            new Segment
                            {
                                Text = "T2"
                            },
                            new Segment
                            {
                                Text = "T3"
                            },
                        }
        };

        MaterialSegmentedControl _vtSegmentsElement = new MaterialSegmentedControl
        {
            Orientation = StackOrientation.Vertical,
            Segments =
                        {
                            new Segment
                            {
                                Text = "T1",
                            },
                            new Segment
                            {
                                Text = "T2"
                            },
                            new Segment
                            {
                                Text = "T3"
                            },
                        }
        };

        Xamarin.Forms.Grid _grid1 = new Xamarin.Forms.Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(19) }, new ColumnDefinition { Width = GridLength.Star } },
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto } }
        };

        Xamarin.Forms.Grid _grid2 = new Xamarin.Forms.Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star }, new ColumnDefinition { Width = GridLength.Star } },
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto } }
        };

        Forms9Patch.Label _labelElement = new Forms9Patch.Label { Text = "Text" };
        public MaterialButtonAndSegmentAlignments()
        {

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




            Padding = new Thickness(40, 20, 20, 20);
            Content = new Xamarin.Forms.ScrollView
            {
                Content = new Xamarin.Forms.StackLayout
                {
                    Children =
                    {
                        _grid1,
                        _grid2,


                        new BoxView { HeightRequest = 1, Color = Color.Black },

                        _iconTextAndTextButton,
                        _hzSegmentsElement,
                        _vtSegmentsElement
                    }
                }
            };

            _hzAlignmentElement.SegmentTapped += (sender, e) =>
            {
                TextAlignment alignment;
                var buttonText = string.Concat(e.Segment.Text.ToUpper().Substring(0, 1), e.Segment.Text.ToLower().Substring(1));
                if (!Enum.TryParse<TextAlignment>(buttonText, out alignment))
                    throw new Exception("doh");
                _labelElement.HorizontalTextAlignment = alignment;
                _iconTextAndTextButton.HorizontalTextAlignment = alignment;
                _hzSegmentsElement.HorizontalTextAlignment = alignment;
                _vtSegmentsElement.HorizontalTextAlignment = alignment;
            };

            _vtAlignmentElement.SegmentTapped += (sender, e) =>
            {
                TextAlignment alignment;
                var buttonText = string.Concat(e.Segment.Text.ToUpper().Substring(0, 1), e.Segment.Text.ToLower().Substring(1));
                if (!Enum.TryParse<TextAlignment>(buttonText, out alignment))
                    throw new Exception("doh");
                _labelElement.VerticalTextAlignment = alignment;
                _iconTextAndTextButton.VerticalTextAlignment = alignment;
                _hzSegmentsElement.VerticalTextAlignment = alignment;
                _vtSegmentsElement.VerticalTextAlignment = alignment;
            };

            _optionsElement.SegmentTapped += (sender, e) =>
            {
                var options = new List<string>();
                foreach (var segment in _optionsElement.SelectedSegments)
                    options.Add(segment.Text.ToUpper());
                var hasTightSpacing = options.Contains("TIGHT");
                var trailingImage = options.Contains("TRAILING");
                var orientation = options.Contains("VERTICAL") ? StackOrientation.Vertical : StackOrientation.Horizontal;


                _iconTextAndTextButton.HasTightSpacing = hasTightSpacing;
                _iconTextAndTextButton.TrailingImage = trailingImage;
                _iconTextAndTextButton.Orientation = orientation;

                _hzSegmentsElement.HasTightSpacing = hasTightSpacing;
                _hzSegmentsElement.TrailingImage = trailingImage;
                _hzSegmentsElement.IntraSegmentOrientation = orientation;

                _vtSegmentsElement.HasTightSpacing = hasTightSpacing;
                _vtSegmentsElement.TrailingImage = trailingImage;
                _vtSegmentsElement.IntraSegmentOrientation = orientation;
            };

            _iconElement.SegmentTapped += (sender, e) =>
            {
                if (e.Segment.ImageSource != null)
                {
                    _iconTextAndTextButton.ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info");
                    foreach (var segment in _hzSegmentsElement.Segments)
                        segment.ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info");
                    foreach (var segment in _vtSegmentsElement.Segments)
                        segment.ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info");
                }
                else
                    SetIconText(e.Segment.HtmlText);
            };

            _spacingSlider.ValueChanged += (sender, e) =>
            {
                _iconTextAndTextButton.Spacing = _spacingSlider.Value;
                _hzSegmentsElement.IntraSegmentSpacing = _spacingSlider.Value;
                _vtSegmentsElement.IntraSegmentSpacing = _spacingSlider.Value;
            };

            _imposedHeightSwitch.Toggled += (sender, e) =>
            {
                _iconTextAndTextButton.HeightRequest = _imposedHeightSwitch.IsToggled ? 60 : -1;
                _hzSegmentsElement.HeightRequest = _imposedHeightSwitch.IsToggled ? 60 : -1;
                _vtSegmentsElement.HeightRequest = _imposedHeightSwitch.IsToggled ? 180 : -1;

                _vtAlignmentElement.IsEnabled = _imposedHeightSwitch.IsToggled;
                if (!_vtAlignmentElement.IsEnabled)
                    _vtAlignmentElement.DeselectAll();


            };

            var defaultHzAlignment = _iconTextAndTextButton.HorizontalTextAlignment;
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
            _iconTextAndTextButton.ImageSource = null;
            foreach (var segment in _hzSegmentsElement.Segments)
                segment.ImageSource = null;
            foreach (var segment in _vtSegmentsElement.Segments)
                segment.ImageSource = null;
            if (iconTextSetting == "NONE")
                _iconTextAndTextButton.IconText = null;
            else
                _iconTextAndTextButton.IconText = iconTextSetting;
            foreach (var segment in _hzSegmentsElement.Segments)
                segment.IconText = _iconTextAndTextButton.IconText;
            foreach (var segment in _vtSegmentsElement.Segments)
                segment.IconText = _iconTextAndTextButton.IconText;
        }
    }
}
