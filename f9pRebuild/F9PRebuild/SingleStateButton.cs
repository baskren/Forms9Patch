using System;

using Xamarin.Forms;

namespace F9PRebuild
{
    public class SingleStateButton : ContentPage
    {
        public SingleStateButton()
        {
            Content = new StackLayout
            {
                Margin = 40,
                Children = {
                    new Forms9Patch.StateButton
                    {
                        ToggleBehavior = true,
                        HeightRequest = 80,
                        HorizontalTextAlignment = TextAlignment.Start,
                        DefaultState = new Forms9Patch.ButtonState
                        {
                            Text = "Toggle Default State",
                            TextColor = Color.Blue,
                            BackgroundImage = new Forms9Patch.Image("F9PRebuild.Resources.button"),
                            IconImage = new Forms9Patch.Image("F9PRebuild.Resources.Info"),
                        },
                        SelectedState = new Forms9Patch.ButtonState
                        {
                            Text = "Selected",
                            TextColor = Color.Red,
                            BackgroundImage = new Forms9Patch.Image("F9PRebuild.Resources.image")
                        },
                    },
                }
            };
        }
    }
}

