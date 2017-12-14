using System;
using Xamarin.Forms;
using Forms9Patch;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;

namespace Forms9PatchDemo
{
    public class ImageButtonAlignments : ContentPage
    {


        SegmentedControl _hzAlignmentElement = new SegmentedControl
        {
            Segments =
            {
                new Segment { Text = "START", },
                new Segment { Text = "CENTER" },
                new Segment { Text = "END" },
            }
        };

        SegmentedControl _vtAlignmentElement = new SegmentedControl
        {
            Segments =
            {
                new Segment { Text = "START", },
                new Segment { Text = "CENTER" },
                new Segment { Text = "END" },
            }
        };

        SegmentedControl _optionsElement = new SegmentedControl
        {
            GroupToggleBehavior = GroupToggleBehavior.Multiselect,
            Segments =
            {
                new Segment { Text = "TIGHT" },
                new Segment { Text = "TRAILING" },
                new Segment { Text = "VERTICAL" }
            }
        };


        SegmentedControl _iconElement = new SegmentedControl
        {
            HasTightSpacing = true,
            //FontSize = 10,
            Segments =
            {
                new Segment { HtmlText = "NONE" },
                new Segment { HtmlText = "x"},
                new Segment { HtmlText = "©" },
                new Segment { HtmlText = "<font face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\"></font>" },
                new Segment { IconImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info") } }
            }
        };



        Switch _imposedHeightSwitch = new Switch();

        Slider _spacingSlider = new Slider
        {
            Maximum = 50,
            Minimum = 0,
            Value = 5,

        };

        StateButton _iconTextAndTextButton = new StateButton
        {
            DefaultState = new ButtonState
            {
                BackgroundImage = new Forms9Patch.Image
                {
                    Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button"),
                },
                IconImage = new Forms9Patch.Image
                {
                    Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info"),
                },
                TextColor = Color.White,
                Text = "Unselected",
            },
            SelectedState = new Forms9Patch.ButtonState
            {
                BackgroundImage = new Forms9Patch.Image
                {
                    Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image"),
                },
                TextColor = Color.Red,
                Text = "Selected",
            },
            ToggleBehavior = true,
            HeightRequest = -1,
            //HorizontalTextAlignment = TextAlignment.Start,
        };



        Xamarin.Forms.Grid _grid1 = new Xamarin.Forms.Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(19) }, new ColumnDefinition { Width = GridLength.Star } },
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto } }
        };

        Xamarin.Forms.Grid _grid2 = new Xamarin.Forms.Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star }, new ColumnDefinition { Width = GridLength.Star } },
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto }, new RowDefinition { Height = GridLength.Auto } }
        };

        Forms9Patch.Label _labelElement = new Forms9Patch.Label { Text = "Text" };
        public ImageButtonAlignments()
        {

            _grid1.Children.Add(new Xamarin.Forms.Label { Text = "HZ", VerticalTextAlignment = TextAlignment.Center, FontSize = 9 }, 0, 0);
            _grid1.Children.Add(_hzAlignmentElement, 1, 0);
            _grid1.Children.Add(new Xamarin.Forms.Label { Text = "VT", VerticalTextAlignment = TextAlignment.Center, FontSize = 9 }, 0, 1);
            _grid1.Children.Add(_vtAlignmentElement, 1, 1);
            _grid1.Children.Add(new Xamarin.Forms.Label { Text = "Opt", VerticalTextAlignment = TextAlignment.Center, FontSize = 9 }, 0, 2);
            _grid1.Children.Add(_optionsElement, 1, 2);
            //_grid1.Children.Add(new Xamarin.Forms.Label { Text = "Icon", VerticalTextAlignment = TextAlignment.Center, FontSize = 9 }, 0, 3);
            //_grid1.Children.Add(_iconElement, 1, 3);

            _grid2.Children.Add(new Xamarin.Forms.Label { Text = "Spacing:" }, 0, 0);
            _grid2.Children.Add(_spacingSlider, 0, 1);
            _grid2.Children.Add(new Xamarin.Forms.Label { Text = "Imposed Ht:" }, 1, 0);
            _grid2.Children.Add(_imposedHeightSwitch, 1, 1);


            _iconTextAndTextButton.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine("Tapped");
            _iconTextAndTextButton.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine("LongPressed");
            _iconTextAndTextButton.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine("LongPressing");

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
            };

            _vtAlignmentElement.SegmentTapped += (sender, e) =>
            {
                TextAlignment alignment;
                var buttonText = string.Concat(e.Segment.Text.ToUpper().Substring(0, 1), e.Segment.Text.ToLower().Substring(1));
                if (!Enum.TryParse<TextAlignment>(buttonText, out alignment))
                    throw new Exception("doh");
                _labelElement.VerticalTextAlignment = alignment;
                _iconTextAndTextButton.VerticalTextAlignment = alignment;
            };

            _optionsElement.SegmentTapped += (sender, e) =>
            {
                _iconTextAndTextButton.HasTightSpacing = _optionsElement.IsIndexSelected(0);
                _iconTextAndTextButton.TrailingIcon = _optionsElement.IsIndexSelected(1);
                _iconTextAndTextButton.Orientation = _optionsElement.IsIndexSelected(2) ? StackOrientation.Vertical : StackOrientation.Horizontal;
            };

            _iconElement.SegmentTapped += (sender, e) =>
            {
                if (e.Segment.IconImage != null)
                    _iconTextAndTextButton.IconImage = new Forms9Patch.Image { Source = e.Segment.IconImage.Source }; // Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info");
                else
                    SetIconText(e.Segment.HtmlText);
            };

            _spacingSlider.ValueChanged += (sender, e) =>
            {
                _iconTextAndTextButton.Spacing = _spacingSlider.Value;
            };

            _imposedHeightSwitch.Toggled += (sender, e) =>
            {
                _iconTextAndTextButton.HeightRequest = _imposedHeightSwitch.IsToggled ? 60 : -1;

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
            _iconTextAndTextButton.IconImage = null;
            if (iconTextSetting == "NONE")
                _iconTextAndTextButton.IconText = null;
            else
                _iconTextAndTextButton.IconText = iconTextSetting;
        }
    }
}
