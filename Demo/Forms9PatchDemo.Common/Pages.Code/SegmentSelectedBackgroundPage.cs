using System;

using Xamarin.Forms;
using Forms9Patch;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class SegmentSelectedBackgroundPage : Xamarin.Forms.ContentPage
    {
        SegmentedControl segCtrl = new SegmentedControl
        {
            HorizontalOptions = LayoutOptions.Start,
            WidthRequest = 900,
            //Padding = 4,
            //Padding = 0,
            OutlineWidth = 0,
            BackgroundColor = Color.FromRgb(112, 128, 144).MultiplyAlpha(0.5),
            OutlineColor = Color.FromRgb(112, 128, 144).WithLuminosity(0.25),
            Segments =
                {
                    new Segment { Text = "Orange" },
                    new Segment { Text = "Blue" },
                    new Segment { Text = "Yellow" },
                    new Segment { Text = "Orange" },
                    new Segment { Text = "Blue" },
                    new Segment { Text = "Yellow" }
                },
            //SyncSegmentFontSizes = false
        };

        Forms9Patch.Button runReduceWidthRequestButton = new Forms9Patch.Button
        {
            Text = "Run Reduce WidthRequest",
            ToggleBehavior = true,
            //Padding = ,
        };

        SegmentedControl borderSegCtrl = new SegmentedControl
        {
            HorizontalOptions = LayoutOptions.Start,
            WidthRequest = 900,
            //Padding = 4,
            //OutlineWidth = 0,
            BackgroundColor = Color.FromRgb(112, 128, 144).MultiplyAlpha(0.5),
            OutlineColor = Color.FromRgb(112, 128, 144).WithLuminosity(0.25),
            Segments =
                {
                    new Segment { Text = "Orange" },
                    new Segment { Text = "Blue" },
                    new Segment { Text = "Yellow" },
                    new Segment { Text = "Orange" },
                    new Segment { Text = "Blue" },
                    new Segment { Text = "Yellow" }
                }
        };

        BoxView boxView = new BoxView
        {
            WidthRequest = 900,
            HorizontalOptions = LayoutOptions.Start,
            Color = Color.Blue
        };

        public SegmentSelectedBackgroundPage()
        {
            borderSegCtrl.SegmentSelected += OnSegmentSelected;
            segCtrl.SegmentSelected += OnSegmentSelected;

            Content = new Xamarin.Forms.StackLayout
            {
                Padding = new Thickness(20),
                Children = {
                    new Xamarin.Forms.Label { Text = "SegmentSelectedBackgroundPage" },
                    borderSegCtrl,
                    segCtrl,
                    boxView,
                    runReduceWidthRequestButton
                }
            };

        }

        private void OnSegmentSelected(object sender, SegmentedControlEventArgs e)
        {
            if (sender is SegmentedControl control)
                switch (e.Segment.Text)
                {
                    case "Orange":
                        control.SelectedBackgroundColor = Color.Orange;
                        control.SelectedTextColor = Color.Default;
                        break;
                    case "Blue":
                        control.SelectedBackgroundColor = Color.Blue;
                        control.SelectedTextColor = Color.White;
                        break;
                    case "Yellow":
                        control.SelectedBackgroundColor = Color.Yellow;
                        control.SelectedTextColor = Color.Default;
                        break;
                }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.StartTimer(TimeSpan.FromSeconds(0.2), () =>
            {
                //System.Diagnostics.Debug.WriteLine("=================================================");
                if (runReduceWidthRequestButton.IsSelected)
                {
                    segCtrl.WidthRequest -= 0.1;
                    boxView.WidthRequest = segCtrl.WidthRequest;
                    borderSegCtrl.WidthRequest = segCtrl.WidthRequest;
                }
                return true;
            });
        }
    }
}

