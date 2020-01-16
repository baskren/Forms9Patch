using System;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    public class VariableWidthButton : ContentPage
    {
        #region VisualElements
        readonly Forms9Patch.Button _button = new Forms9Patch.Button
        {
            BackgroundColor = Color.DarkGray,
            Text = "button",
            OutlineWidth = 0,
            HorizontalOptions = LayoutOptions.Center,
        };

        readonly Slider _slider = new Xamarin.Forms.Slider
        {
            Value = 100,
            Maximum = 800,
            Minimum = 100,
        };
        #endregion

        public VariableWidthButton()
        {
            Padding = 40;
            Content = new StackLayout
            {
                Padding = 0,
                Margin = 0,
                Spacing = 0,
                Children =
                {
                    _slider,
                    _button
                }
            };

            _slider.ValueChanged += (s,e) => _button.WidthRequest = _slider.Value;
            _button.WidthRequest = _slider.Value;
        }
    }
}