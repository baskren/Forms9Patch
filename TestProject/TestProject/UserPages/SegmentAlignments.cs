using System;
using Xamarin.Forms;
using Forms9Patch;
using System.Runtime.InteropServices;

namespace Forms9PatchDemo
{
    public class SegmentAlignments : ContentPage
    {


        MaterialSegmentedControl _alignmentElement = new MaterialSegmentedControl
        {
            Segments =
            {
                new Segment
                {
                    Text = "START",
                },
                new Segment
                {
                    Text = "CENTER"
                },
                new Segment
                {
                    Text = "END"
                },
            }
        };

        Switch _hasTightSpacingElement = new Switch
        {
        };

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

        Forms9Patch.Label _labelElement = new Forms9Patch.Label { Text = "Text" };
        public SegmentAlignments()
        {
            Padding = new Thickness(40, 20, 20, 20);
            Content = new Xamarin.Forms.StackLayout
            {
                Children =
                {
                    new Xamarin.Forms.Label { Text = "Alignment:"},
                    _alignmentElement,

                    new Xamarin.Forms.Label { Text = "HasTightSpacing:"},
                    _hasTightSpacingElement,

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
                }
            };

            _alignmentElement.SegmentTapped += (sender, e) =>
            {
                TextAlignment alignment;
                if (Enum.TryParse(e.Segment.Text, out alignment))
                    throw new Exception("doh");
                _iconOnlyButtonElement.Alignment = alignment;
                _textOnlyButtonElement.Alignment = alignment;
                _iconTextOnlyButtonElement.Alignment = alignment;
                _labelElement.HorizontalTextAlignment = alignment;
            };

            _hasTightSpacingElement.Toggled += (sender, e) =>
            {
                _textOnlySegmentsElement.HasTightSpacing = _hasTightSpacingElement.IsToggled;
                _iconTextOnlySegmentsElement.HasTightSpacing = _hasTightSpacingElement.IsToggled;
                _iconOnlySegmentsElement.HasTightSpacing = _hasTightSpacingElement.IsToggled;
                _textOnlyButtonElement.HasTightSpacing = _hasTightSpacingElement.IsToggled;
                _iconTextOnlyButtonElement.HasTightSpacing = _hasTightSpacingElement.IsToggled;
                _iconOnlyButtonElement.HasTightSpacing = _hasTightSpacingElement.IsToggled;
            };

            var defaultAlignment = _iconOnlyButtonElement.Alignment;
            if (defaultAlignment == TextAlignment.Start)
                _alignmentElement.SelectIndex(0);
            else if (defaultAlignment == TextAlignment.Center)
                _alignmentElement.SelectIndex(1);
            else
                _alignmentElement.SelectIndex(2);

            var defaultHasTightSpacing = _iconOnlyButtonElement.HasTightSpacing;
            _hasTightSpacingElement.IsToggled = defaultHasTightSpacing;
        }
    }
}
