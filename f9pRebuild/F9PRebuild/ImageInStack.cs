using System;

using Xamarin.Forms;

namespace F9PRebuild
{
    public class ImageInStack : ContentPage
    {
        public ImageInStack()
        {

            Content = new StackLayout
            {
                Spacing = 0,
                Children =
                {
                    //new Label { Text = "Hello ContentPage", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center }
                    /*
                    new BoxView { Color = Color.Blue},
                    new Forms9Patch.Image
                    //new Xamarin.Forms.Image
                    {
                        Source = Xamarin.Forms.ImageSource.FromResource("F9PRebuild.Resources.redribbon.png"),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        BackgroundColor = Color.Pink,
                        Margin = 10,
                    },
                    */
                    new BoxView { Color = Color.Blue},
                    new Forms9Patch.ContentView
                    {

                        Content = new Label { Text = "THIS IS A LABEL"},
                        BackgroundImage = new Forms9Patch.Image
                        {
                            Source = Xamarin.Forms.ImageSource.FromResource("F9PRebuild.Resources.redribbon.png"),
                            BackgroundColor = Color.Pink,
                            Margin = 1,
                            OutlineWidth = 1,
                            OutlineColor = Color.Blue,
                            OutlineRadius = 4,
                        },
                        LimitMinSizeToBackgroundImageSize = true,
                    },
                    new BoxView { Color = Color.Blue},
                }
            };
        }
    }
}

