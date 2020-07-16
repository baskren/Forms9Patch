using System;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class FlyoutDemo : ContentPage
    {
        #region VisualElements
        Forms9Patch.FlyoutPopup _flyout = new Forms9Patch.FlyoutPopup
        {
            Content = new Label { Text = "Your content here!", TextColor = Color.Green },
            IsAnimationEnabled = true,
            BackgroundColor = Color.White,
        };

        Forms9Patch.SegmentedControl _orientationControl = new Forms9Patch.SegmentedControl
        {
            TextColor = Color.White,
            Segments =
            {
                new Forms9Patch.Segment("Horizontal"),
                new Forms9Patch.Segment("Vertical")
            },
        };

        Forms9Patch.SegmentedControl _alignmentControl = new Forms9Patch.SegmentedControl
        {
            TextColor = Color.White,
            Segments =
            {
                new Forms9Patch.Segment("Start"),
                new Forms9Patch.Segment("End")
            }
        };

        Forms9Patch.SegmentedControl _optionsControl = new Forms9Patch.SegmentedControl
        {
            TextColor = Color.White,
            Segments =
            {
                new Forms9Patch.Segment("Apply Margins"),
                new Forms9Patch.Segment("Round Corners")
            },
            GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect
        };

        #endregion

        public FlyoutDemo()
        {
            BackgroundColor = Color.Blue;
            Padding = 30;
            Content = new StackLayout
            {
                Children =
                {
                    new Label{Text = "Orientation:", TextColor = Color.White},
                    _orientationControl,
                    new Label{Text = "Alignment:", TextColor = Color.White},
                    _alignmentControl,
                    new Label{Text = "Options:", TextColor = Color.White},
                    _optionsControl
                }
            };

            _orientationControl.SelectIndex(0);
            _alignmentControl.SelectIndex(0);
            _optionsControl.SelectIndex(1);

            _orientationControl.SegmentTapped += (sender, e) =>
            {
                _flyout.Orientation = e.Segment.Text == "Horizontal"
                    ? StackOrientation.Horizontal
                    : StackOrientation.Vertical;
                _flyout.IsVisible = true;
            };

            _alignmentControl.SegmentTapped += (sender, e) =>
            {
                _flyout.Alignment = e.Segment.Text == "Start"
                    ? Forms9Patch.FlyoutAlignment.Start
                    : Forms9Patch.FlyoutAlignment.End;
                _flyout.IsVisible = true;
            };

            _optionsControl.SegmentTapped += (sender, e) =>
            {
                _flyout.Margin = _optionsControl.Segments[0].IsSelected ? 30 : 0;
                _flyout.OutlineRadius = _optionsControl.Segments[1].IsSelected ? 5 : 0;
            };


        }
    }
}