using System;

using Xamarin.Forms;
using Forms9Patch;

namespace Forms9PatchDemo
{
	public class ModalPopupWithNavigationPages : ContentPage
	{
		public ModalPopupWithNavigationPages ()
		{
			var button = new Button {
				Text = "Show Test Page",
			};
			button.Clicked += (sender, e) => this.Navigation.PushModalAsync (new BubbonPushModalAsyncPage ());
			// The root page of your application
			var content = new Xamarin.Forms.ContentView
			{
				//Title = "BubbleTest",
				Content = new Xamarin.Forms.StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Xamarin.Forms.Label {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						},
						button,
					}
				}
			};
			Content = content;
		}
	}

	public class BubbonPushModalAsyncPage : ContentPage
	{
		Button button1;
		Button button2;
		BubblePopup bubble;

		public BubbonPushModalAsyncPage()
		{
			button1 = new Button { Text = "No Target" };
			button2 = new Button { Text = "Target" };

			Content = new Xamarin.Forms.StackLayout
			{
				VerticalOptions = LayoutOptions.Center,
				Children = {
					button1,
					button2,
				}
			};

			bubble = new BubblePopup(this)
			{
				Padding =25,
				BackgroundColor = Color.Blue,
				//Target = button, // Uncomment this line, and it crashes too
				Content = new Xamarin.Forms.StackLayout
				{
					Children = {
						new Xamarin.Forms.Label { Text = "This is a test bubble" }
					}
				}
			};

			button1.Clicked += (sender, e) => {
				bubble.Target = null;
				bubble.IsVisible = true;
			};

			button2.Clicked += (sender, e) => {
				bubble.Target = button1;
				bubble.IsVisible = true;
			};
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			//bubble.IsVisible = true;
		}
	}
}




