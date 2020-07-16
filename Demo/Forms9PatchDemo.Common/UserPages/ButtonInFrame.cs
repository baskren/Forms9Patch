using System.Collections.Generic;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    class ButtonInFrame : ContentPage
    {
        StackLayout _stackLayout = new StackLayout();

        Forms9Patch.Button _button1 = new Forms9Patch.Button("Button1") { Margin = 5, BackgroundColor = Color.Blue, TextColor = Color.White };
        Forms9Patch.Button _button2 = new Forms9Patch.Button("Button2") { Margin = 5, BackgroundColor = Color.Blue, TextColor = Color.White };

        Frame _frame = new Frame { BackgroundColor = Color.Orange, };

        public ButtonInFrame()
        {
            _frame.Content = _button1;
            _stackLayout.Children.Add(_frame);
            _stackLayout.Children.Add(_button2);

            Content = _stackLayout;

            _button1.Clicked += (sender, e) => System.Diagnostics.Debug.WriteLine("BUTTON 1 CLICKED");
            _button2.Clicked += (sender, e) => System.Diagnostics.Debug.WriteLine("BUTTON 2 CLICKED");



        }

    }
}