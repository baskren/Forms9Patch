using System;
using Xamarin.Forms;
using Forms9Patch;

namespace Forms9PatchDemo
{
    public class ButtonTapped : ContentPage
    {

        public ButtonTapped()
        {
            var materialButton = new MaterialButton
            {
                Text = "MaterialButton",
                FontColor = Color.Pink,
                FontSize = 20,
                HeightRequest = 30,
                WidthRequest = 200,
                OutlineColor = Color.Pink,
                OutlineRadius = 5,
                OutlineWidth = 2,
                IsVisible = true,
                BackgroundColor = Color.White,
            };
            materialButton.Tapped += OnTapped;

            var imageButton = new ImageButton
            {
                Text = "ImageButton",
                FontColor = Color.Green,
                FontSize = 20,
                HeightRequest = 30,
                WidthRequest = 200,
                OutlineColor = Color.Green,
                OutlineRadius = 5,
                OutlineWidth = 2,
                IsVisible = true,
                BackgroundColor = Color.White
            };

            Content = new Xamarin.Forms.StackLayout
            {
                Children = {
                    materialButton, imageButton
                }
            };
        }

        private async void OnTapped(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Tapped");
        }
    }
}
