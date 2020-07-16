using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
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


            var svgString = $@"
<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 96 105"">
  <g fill=""#97C024"" stroke=""#97C024"" stroke-linejoin=""round"" stroke-linecap=""round"">
    <path d=""M14,40v24M81,40v24M38,68v24M57,68v24M28,42v31h39v-31z"" stroke-width=""12""/>
    <path d=""M32,5l5,10M64,5l-6,10"" stroke-width=""2""/>
  </g>
  <path d=""M22,35h51v10h-51zM22,33c0-31,51-31,51,0"" fill=""#97C024""/>
  <g fill=""#FFF"">
    <circle cx=""36"" cy=""22"" r=""2""/>
    <circle cx=""59"" cy=""22"" r=""2""/>
  </g>
</svg>
";

            var image = new Forms9Patch.Image
            {
                Source = Forms9Patch.ImageSource.FromSvgText(svgString),
                HeightRequest = 200,
                WidthRequest = 200,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Content = new StackLayout
            {
                Children = {
                    image,
                }
            };
        }
    }
}

