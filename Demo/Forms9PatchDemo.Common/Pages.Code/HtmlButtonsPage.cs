using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class HtmlButtonsPage : ContentPage
    {
        Forms9Patch.Button mb1 = new Forms9Patch.Button
        {
            IconText = "<font size=\"4\" face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\"></font>",
            HtmlText = "<i>Markup</i> button",
            TrailingIcon = true,
            HasTightSpacing = true,
            Spacing = 10,
            //Text = "Pizza",
            BackgroundColor = Color.FromRgb(200, 200, 200),
            TextColor = Color.Blue,
            HasShadow = true,
            ToggleBehavior = true,
        };

        public HtmlButtonsPage()
        {
            Padding = 20;
            BackgroundColor = Color.White;

            #region Material Button
            mb1.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine("Tapped");
            mb1.Selected += (sender, e) => System.Diagnostics.Debug.WriteLine("Selected");
            mb1.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine("Long Pressing");
            mb1.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine("Long Pressed");
            #endregion

            #region Segmented Button
            var sc1 = new Forms9Patch.SegmentedControl
            {
                HasShadow = true,
                BackgroundColor = Color.FromRgb(200, 200, 200),
                FontSize = 15,
                Padding = 5,
                //TextColor = Color.Blue,
                Segments = {

                    new Forms9Patch.Segment {
                        Text = "Cart",
                        IconText = "<font size=\"4\" face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\"></font>",
                    },
                    new Forms9Patch.Segment {
                        Text = "Pay",
                        IconText = "<font size=\"4\" face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\"></font>",
                    },
                    new Forms9Patch.Segment {
                        Text = "Ship",
                        IconText = "<font size=\"4\" face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\"></font>",
                    },
                    new Forms9Patch.Segment {
                        Text = "Email",
                        IconText = "<font size=\"4\" face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\"></font>",
                    },
                },
                HasTightSpacing = true,
                IntraSegmentSpacing = 10
            };
            sc1.SegmentSelected += OnSegmentSelected;
            sc1.SegmentTapped += OnSegmentTapped;
            sc1.SegmentLongPressing += OnSegmentLongPressing;
            sc1.SegmentLongPressed += OnSegmentLongPressed;
            #endregion

            #region Image Button
            var ib1 = new Forms9Patch.StateButton
            {
                DefaultState = new Forms9Patch.ButtonState
                {
                    BackgroundImage = new Forms9Patch.Image
                    {
                        Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button"),
                    },
                    IconImage = new Forms9Patch.Image
                    {
                        Source = ImageSource.FromFile("five.png"),
                    },
                    TextColor = Color.White,
                    //Text = "Toggle w/ SelectedState",
                    HtmlText = "<b>Toggle</b> with <i>SelectedState</i>",
                },
                SelectedState = new Forms9Patch.ButtonState
                {
                    BackgroundImage = new Forms9Patch.Image
                    {
                        Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image"),
                    },
                    TextColor = Color.Red,
                    //Text = "Selected",
                    HtmlText = "<b><i>Selected</i></b>",

                },
                ToggleBehavior = true,
                HeightRequest = 50,
                HorizontalTextAlignment = TextAlignment.Start,
            };
            ib1.Tapped += OnImageButtonTapped;
            ib1.Selected += OnImageButtonSelected;
            ib1.LongPressing += OnImageButtonLongPressing;
            ib1.LongPressed += OnImageButtonLongPressed;
            #endregion

            Content = new StackLayout
            {
                Children = {
                    new Forms9Patch.Label { HtmlText = "<b>HTML Buttons</b>" },
                    mb1,
                    sc1,
                    ib1,
                }
            };


        }

        #region Touch event responders
        void OnSegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Tapped Segment[" + e.Index + "] Text=[" + e.Segment.HtmlText + "]");
        }

        void OnSegmentSelected(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Selected Segment[" + e.Index + "] Text=[" + e.Segment.HtmlText + "]");
        }

        void OnSegmentLongPressing(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressing Segment[" + e.Index + "] Text=[" + e.Segment.HtmlText + "]");
        }

        void OnSegmentLongPressed(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressed Segment[" + e.Index + "] Text=[" + e.Segment.HtmlText + "]");
        }

        void OnImageButtonTapped(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Tapped Button Text=[" + ((Forms9Patch.StateButton)sender).Text + "]");
            mb1.IsEnabled = !mb1.IsEnabled;
        }

        void OnImageButtonSelected(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Selected Button Text=[" + ((Forms9Patch.StateButton)sender).Text + "]");
        }

        void OnImageButtonLongPressing(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressing Button Text=[" + ((Forms9Patch.StateButton)sender).Text + "]");
        }

        void OnImageButtonLongPressed(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressed Button Text=[" + ((Forms9Patch.StateButton)sender).Text + "]");
        }
        #endregion


    }
}


