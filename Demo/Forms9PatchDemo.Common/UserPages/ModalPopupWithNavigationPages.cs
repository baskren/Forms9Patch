using System;

using Xamarin.Forms;
using Forms9Patch;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ModalPopupWithNavigationPages : Xamarin.Forms.ContentPage
    {
        public ModalPopupWithNavigationPages()
        {
            Padding = 20;

            var button = new Forms9Patch.Button("Show Test Page");
            button.Clicked += (sender, e) => this.Navigation.PushModalAsync(new BubbonPushModalAsyncPage());
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

    public class BubbonPushModalAsyncPage : Xamarin.Forms.ContentPage
    {
        Forms9Patch.Button button1;
        Forms9Patch.Button button2;
        Forms9Patch.Button returnButton = new Forms9Patch.Button("RETURN");
        BubblePopup bubble;

        public BubbonPushModalAsyncPage()
        {
            Padding = 20;

            button1 = new Forms9Patch.Button("No Target");
            button2 = new Forms9Patch.Button("Target");


            Content = new Xamarin.Forms.StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    button1,
                    button2,
                    returnButton,
                }
            };

            bubble = new BubblePopup(button1)
            {
                Padding = 25,
                BackgroundColor = Color.Blue,
                //Target = button, // Uncomment this line, and it crashes too
                Content = new Xamarin.Forms.StackLayout
                {
                    Children = {
                        new Xamarin.Forms.Label { Text = "This is a test bubble" }
                    }
                }
            };

            button1.Clicked += (sender, e) =>
            {
                bubble.Target = null;
                bubble.IsVisible = true;
            };

            button2.Clicked += (sender, e) =>
            {
                bubble.Target = button1;
                bubble.IsVisible = true;
            };

            returnButton.Clicked += (sender, e) =>
            {
                this.Navigation.PopModalAsync();
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //bubble.IsVisible = true;
        }
    }
}




