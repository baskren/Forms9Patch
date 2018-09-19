using System;

using Xamarin.Forms;

namespace F9PRebuild
{
    public class XImage : ContentPage
    {
        public XImage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "XImage Test" },
                    new BoxView { HeightRequest = 1, BackgroundColor = Color.Black},
                    //new Forms9Patch.XImage(),
                    new BoxView { HeightRequest = 1, BackgroundColor = Color.Black},
                }
            };
        }
    }
}

