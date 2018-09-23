using System;

using Xamarin.Forms;

namespace F9PRebuild
{
    public class Frame : ContentPage
    {
        Forms9Patch.Frame _frame1 = new Forms9Patch.Frame
        {
            Content = new Label { Text = "FRAME CONTENT" },
            BackgroundColor = Color.White,
            OutlineColor = Color.Blue,
            OutlineWidth = 1,
            OutlineRadius = 5,
            HasShadow = false,
            Padding = 0,
        };

        Forms9Patch.Frame _frame2 = new Forms9Patch.Frame
        {
            Content = new Label { Text = "FRAME CONTENT" },
            BackgroundColor = Color.White,
            OutlineColor = Color.Blue,
            OutlineWidth = 1,
            OutlineRadius = 5,
            HasShadow = true,
            Padding = 0,
        };

        public Frame()
        {
            Padding = 50;

            Content = new StackLayout
            {
                Children = {
                    _frame1, _frame2
                }
            };

            var l1 = FormsGestures.Listener.For(_frame1);
            var l2 = FormsGestures.Listener.For(_frame2);

            l1.Tapped += (sender, e) => _frame1.HasShadow = !_frame1.HasShadow;
            l2.Tapped += (sender, e) => _frame2.HasShadow = !_frame2.HasShadow;
        }
    }
}

