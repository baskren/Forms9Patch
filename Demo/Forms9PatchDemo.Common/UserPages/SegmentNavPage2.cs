// /*******************************************************************
//  *
//  * SegmentNavPage1.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class SegmentNavPage2 : ContentPage
    {
        public SegmentNavPage2()
        {
            Padding = new Thickness(5);
            BackgroundColor = Color.Green;

            // Define command for the items in the SegmentedController.
            var navigateCommand = new Command<Type>(async (Type pageType) =>
            {
                var page = (Page)Activator.CreateInstance(pageType);
                await this.Navigation.PushAsync(page);
            });

            var backCommand = new Command<Type>(async (obj) =>
            {
                await this.Navigation.PopAsync();
            });



            var segmentControl = new Forms9Patch.SegmentedControl
            {
                Segments = {
                    new Forms9Patch.Segment
                    {
                        Text = "Back",
                        Command = backCommand

                    },
                    new Forms9Patch.Segment
                    {
                        Text = "Page 1",
                        Command = navigateCommand,
                        CommandParameter = typeof(SegmentNavPage1),
                    },
                    new Forms9Patch.Segment
                    {
                        Text = "Page 3",
                        Command = navigateCommand,
                        CommandParameter = typeof(SegmentNavPage3),
                    }
                },
            };

            var showPopupButton = new Button
            {
                Text = "Show popup"
            };
            showPopupButton.Clicked += (s, e) =>
            {
                Forms9Patch.Toast.Create("Page2 Popup", "Test of Forms9Patch in nested, modal pages.");
            };

            var layout = new StackLayout
            {
                Spacing = 10,
                Children = {
                    new Label { Text = "Segment Nav Page 2" },
                    segmentControl,
                    new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },
                    new Label {
                        Text = "2",
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = 50,
                    },
                    showPopupButton
                }
            };

            Content = layout;
        }
    }
}
