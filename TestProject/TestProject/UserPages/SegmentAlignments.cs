using System;
using Xamarin.Forms;
using Forms9Patch;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;

namespace Forms9PatchDemo
{
    public class SegmentAlignments : ContentPage
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

        Switch _imposedHeightSwitch = new Switch();

        Slider _spacingSlider = new Slider
        {
            Maximum = 50,
            Minimum = 0,
            Value = 5,

        };

        /*
        MaterialButton _iconAndTextButton = new MaterialButton
        {
            Text = "Text",
            ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.info"),
        };
        */

        MaterialButton _iconTextAndTextButton = new MaterialButton
        {
            Text = "Text",
            IconText = "©"
        };

        /*
        MaterialSegmentedControl _textOnlySegmentsElement = new MaterialSegmentedControl
        {
            Segments =
                        {
                            new Segment
                            {
                                Text = "TA",
                            },
                            new Segment
                            {
                                Text = "TB"
                            },
                            new Segment
                            {
                                Text = "TC"
                            },
                        }
        };

        MaterialSegmentedControl _iconTextOnlySegmentsElement = new MaterialSegmentedControl
        {
            Segments =
            {
                new Segment
                {
                    IconText = "IA"
                },
                new Segment
                {
                    IconText = "IB"
                },
                new Segment
                {
                    IconText = "IC"
                },
            }
        };


        MaterialSegmentedControl _iconOnlySegmentsElement = new MaterialSegmentedControl
        {
            Segments =
            {
                new Segment
                {
                    ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.info"),
                },
                new Segment
                {
                    ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.ArrowR")
                },
                new Segment
                {
                    ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Exit")
                },
            }
        };

        MaterialButton _textOnlyButtonElement = new MaterialButton
        {
            Text = "TA"
        };

        MaterialButton _iconTextOnlyButtonElement = new MaterialButton
        {
            IconText = "IA"
        };

        MaterialButton _iconOnlyButtonElement = new MaterialButton
        {
            ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.ArrowR")
        };
        */


        Forms9Patch.Label _labelElement = new Forms9Patch.Label { Text = "Text" };
        public SegmentAlignments()
        {
            Padding = new Thickness(40, 20, 20, 20);
            Content = new Xamarin.Forms.ScrollView
            {
                Content = new Xamarin.Forms.StackLayout
                {
                    Children =
                    {
                        new Xamarin.Forms.Label { Text = "Horizontal Alignment:"},
                        _hzAlignmentElement,

                        new Xamarin.Forms.Label { Text = "Options:"},
                        _optionsElement,

                        new Xamarin.Forms.Label { Text = "Spacing:"},
                        _spacingSlider,

                        new Xamarin.Forms.Label { Text = "Imposed Height:"},
                        _imposedHeightSwitch,

                        new BoxView { HeightRequest = 1 },

                        _iconTextAndTextButton,

                        /*

                        _iconAndTextButton,

                        new BoxView { HeightRequest = 1 },

                        new Xamarin.Forms.Label { Text = "Text Only Segments:"},
                        _textOnlySegmentsElement,

                        new Xamarin.Forms.Label { Text = "IconText Only Segments:"},
                        _iconTextOnlySegmentsElement,

                        new Xamarin.Forms.Label { Text = "Icon Only Segments:"},
                        _iconOnlySegmentsElement,

                        new BoxView { HeightRequest = 1 },

                        new Xamarin.Forms.Label { Text = "Text Only Segments:"},
                        _textOnlyButtonElement,

                        new Xamarin.Forms.Label { Text = "IconText Only Segments:"},
                        _iconTextOnlyButtonElement,

                        new Xamarin.Forms.Label { Text = "Icon Only Segments:"},
                        _iconOnlyButtonElement,

                        new Xamarin.Forms.Label { Text = "Label:"},
                        _labelElement
                        */
                    }
                }
            };

            _hzAlignmentElement.SegmentTapped += (sender, e) =>
            {
                TextAlignment alignment;
                var buttonText = string.Concat(e.Segment.Text.ToUpper().Substring(0, 1), e.Segment.Text.ToLower().Substring(1));
                if (!Enum.TryParse<TextAlignment>(buttonText, out alignment))
                    throw new Exception("doh");
                /*
                _iconOnlyButtonElement.Alignment = alignment;
                _textOnlyButtonElement.Alignment = alignment;
                _iconTextOnlyButtonElement.Alignment = alignment;
				_iconAndTextButton.Alignment = alignment;
				*/
                _labelElement.HorizontalTextAlignment = alignment;
                _iconTextAndTextButton.Alignment = alignment;
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
            };

            _spacingSlider.ValueChanged += (sender, e) =>
            {
                _iconTextAndTextButton.Spacing = _spacingSlider.Value;
            };

            _imposedHeightSwitch.Toggled += (sender, e) =>
            {
                _iconTextAndTextButton.HeightRequest = _imposedHeightSwitch.IsToggled ? 60 : -1;
            };

            var defaultAlignment = _iconTextAndTextButton.Alignment;
            if (defaultAlignment == TextAlignment.Start)
                _hzAlignmentElement.SelectIndex(0);
            else if (defaultAlignment == TextAlignment.Center)
                _hzAlignmentElement.SelectIndex(1);
            else
                _hzAlignmentElement.SelectIndex(2);

        }
    }
}
