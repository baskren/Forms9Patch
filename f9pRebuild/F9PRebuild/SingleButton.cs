using System;

using Xamarin.Forms;

namespace F9PRebuild
{
    public class SingleButton : ContentPage
    {
        Forms9Patch.Button _button = new Forms9Patch.Button
        {
            Text = "TEXT 1",
            BackgroundImage = "F9PRebuild.Resources.button",
            IconImage = "F9PRebuild.Resources.Info",
            TextColor = Color.Red,
            ToggleBehavior = true
        };

        Forms9Patch.Image _image = new Forms9Patch.Image("F9PRebuild.Resources.Info")
        {
            TintColor = Color.Blue
        };

        public SingleButton()
        {
            Padding = 40;
            Content = new StackLayout
            {
                Children = {
                    _button,
                    _image
                }
            };

            _button.Tapped += (sender, e) =>
            {
                if (_button.IsSelected)
                    _button.BackgroundImage = "F9PRebuild.Resources.image";
                else
                    _button.BackgroundImage = "F9PRebuild.Resources.button";
            };
        }
    }
}

