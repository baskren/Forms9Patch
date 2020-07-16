using System;
using Xamarin.Forms;
using Forms9Patch;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ButtonTapped : Xamarin.Forms.ContentPage
    {

        public ButtonTapped()
        {
            Padding = 40;

            var button = new Forms9Patch.Button
            {
                Text = "Button",
                TextColor = Color.Pink,
                FontSize = 20,
                HeightRequest = 30,
                WidthRequest = 200,
                OutlineColor = Color.Pink,
                OutlineRadius = 5,
                OutlineWidth = 2,
                IsVisible = true,
                BackgroundColor = Color.White,
            };
            button.Tapped += OnTapped;

            var stateButton = new StateButton
            {
                Text = "StateButton",
                TextColor = Color.Green,
                FontSize = 20,
                HeightRequest = 30,
                WidthRequest = 200,
                OutlineColor = Color.Green,
                OutlineRadius = 5,
                OutlineWidth = 2,
                IsVisible = true,
                BackgroundColor = Color.White
            };
            stateButton.Tapped += OnTapped;

            Content = new Xamarin.Forms.StackLayout
            {
                Children = {
                    button, stateButton
                }
            };
        }

        void OnTapped(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Tapped");
        }
    }
}
