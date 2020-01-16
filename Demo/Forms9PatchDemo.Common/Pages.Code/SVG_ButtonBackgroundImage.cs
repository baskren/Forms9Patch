using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    public class SVG_ButtonBackgroundImage : ContentPage
    {
        public SVG_ButtonBackgroundImage()
        {
            Padding = 40;

            var stateButton = new Forms9Patch.StateButton { ToggleBehavior = true };
            stateButton.DefaultState.BackgroundImage = new Forms9Patch.Image("Forms9PatchDemo.Resources.redWhiteGradient.svg");
            stateButton.DefaultState.Text = "Forms9Patch.StateButton.DefaultState";

            // only the default state is created by default.  So have to create a pressing state
            stateButton.SelectedState = new Forms9Patch.ButtonState
            {
                BackgroundImage = new Forms9Patch.Image("Forms9PatchDemo.Resources.blueWhiteGradient.svg"),
                Text = "Forms9Patch.StateButton.SelectedState"
            };
            var button = new Forms9Patch.Button
            {
                Text = "Forms9Patch.Button",
                BackgroundImage = new Forms9Patch.Image("Forms9PatchDemo.Resources.greenWhiteGradient.svg")
            };

            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "SVG Button.BackgroundImage demo" },
                    stateButton,
                    button,
                }
            };
        }
    }
}

