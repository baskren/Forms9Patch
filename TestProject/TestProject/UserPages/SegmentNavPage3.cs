// /*******************************************************************
//  *
//  * SegmentNavPage1.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public class SegmentNavPage3 : ContentPage
	{
		public SegmentNavPage3()
		{
			Padding = new Thickness(5);
			BackgroundColor = Color.Blue;

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

            var buttonStack = new StackLayout { Orientation = StackOrientation.Horizontal };
            var button1 = new Button { Text = "Back" };
            var button2 = new Button { Text = "Page 1" };
            var button3 = new Button { Text = "Page 2" };
            button1.Command = backCommand;
            button2.Command = navigateCommand;
            button2.CommandParameter = typeof(SegmentNavPage1);
            button3.Command = navigateCommand;
            button3.CommandParameter = typeof(SegmentNavPage2);
            buttonStack.Children.Add(button1);
            buttonStack.Children.Add(button2);
            buttonStack.Children.Add(button3);

            var labelStack = new StackLayout { Orientation = StackOrientation.Horizontal };
            var label1 = new Forms9Patch.Label { Text = "X" };
            var label2 = new Forms9Patch.Label { Text = "Page 1" };
            var label3 = new Forms9Patch.Label { Text = "Page 2" };
            labelStack.Children.Add(new Forms9Patch.Frame { Content = label1, OutlineColor = Color.Black, OutlineWidth = 2 });
            labelStack.Children.Add(new Forms9Patch.Frame { Content = label2, OutlineColor = Color.Black, OutlineWidth = 2 });
            labelStack.Children.Add(new Forms9Patch.Frame { Content = label3, OutlineColor = Color.Black, OutlineWidth = 2 });


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
						Text = "Page 2",
						Command = navigateCommand,
						CommandParameter = typeof(SegmentNavPage2),
					}
				},
			};

			var layout = new StackLayout
			{
				Children = {
					new Label { Text = "Segment Nav Page 3" },
					segmentControl,
                    new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },
                    buttonStack,
                    new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },
                    labelStack,
                    new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },
                    new Label {
						Text = "3",
						HorizontalOptions = LayoutOptions.Center,
						FontSize = 50,
					}
				}
			};

			Content = layout;
		}
	}
}
