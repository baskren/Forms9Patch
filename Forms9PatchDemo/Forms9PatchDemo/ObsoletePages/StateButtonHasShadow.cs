using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    public class StateButtonHasShadow : ContentPage
    {
        Switch _hasShadowSwitch = new Switch { IsToggled = true };

        Forms9Patch.StateButton _stateButton = new Forms9Patch.StateButton
        {
            DefaultState = new Forms9Patch.ButtonState
            {
                BackgroundImage = new Forms9Patch.Image("Forms9PatchDemo.Resources.button"),
                Text = "Default Text",
                HasShadow = true
            },
        };

        public StateButtonHasShadow()
        {
            Padding = 20;

            _hasShadowSwitch.Toggled += (sender, e) => _stateButton.DefaultState.HasShadow = _hasShadowSwitch.IsToggled;

            Content = new StackLayout
            {
                Children = {
                    _hasShadowSwitch, _stateButton
                }
            };
        }
    }
}

