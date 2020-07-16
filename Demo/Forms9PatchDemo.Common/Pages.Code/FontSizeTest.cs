using System;

using Xamarin.Forms;

namespace Forms9PatchDemo.Pages
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class FontSizeTest : ContentPage
    {
        public FontSizeTest()
        {
            var label = new Xamarin.Forms.Label
            {
                Text = "",
                FontFamily = "Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf",
                FontSize = 30,
                BackgroundColor = Color.FromRgb(218, 112, 214),
                TextColor = Color.White
            };
            Forms9Patch.EmbeddedResourceFontEffect.ApplyTo(label);


            Padding = new Thickness(10, 30, 10, 10);
            Content = new StackLayout
            {
                Children = {
                    new StackLayout
                    {
                        Children = {
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Spacing = 0,
                                Children =
                                {
                                    new Label { Text = "Xamarin:", TextColor = Color.Black },
                                    new BoxView {
                                        //HeightRequest = 30,
                                        BackgroundColor = Color.Blue
                                    },
                                    label,
                                    new Frame {
                                        HasShadow = false,
                                        //CornerRadius = 0,
                                        Padding = 0,
                                        BackgroundColor = Color.Red,
                                        HeightRequest = 30,
                                        Content = new Xamarin.Forms.Label
                                        {
                                            FontSize = 30,
                                            Text = "Qwetry",
                                            TextColor = Color.Black,
                                            VerticalTextAlignment = TextAlignment.Center
                                        },

                                    },
                                }
                            }
                        },
                    },
                    new Label
                    {
                        Text = "Scale=[" + FormsGestures.Display.Scale + "]"
                    },
                    new StackLayout
                    {
                        Children = {
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Spacing = 0,
                                Children =
                                {
                                    new Label { Text = "Forms9Patch:", TextColor = Color.Black },
                                    new BoxView {
                                        HeightRequest = 30,
                                        BackgroundColor = Color.Blue
                                    },
                                    new Forms9Patch.Label
                                    {
                                        Text = "",
                                        FontFamily = "Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf",
                                        FontSize = 30,
                                        BackgroundColor = Color.FromRgb(218, 112, 214),
                                        TextColor = Color.White,
                                        AutoFit = Forms9Patch.AutoFit.None
                                    },
                                    new Frame {
                                        HasShadow = false,
                                        //CornerRadius = 0,
                                        Padding = 0,
                                        BackgroundColor = Color.Red,
                                        //HeightRequest = 30,
                                        Content = new Forms9Patch.Label
                                        {
                                            FontSize = 30,
                                            Text = "Qwetry",
                                            TextColor = Color.Black,
                                            VerticalTextAlignment = TextAlignment.Center
                                        }
                                    },

                                }
                            }
                        },
                    },

                    new Button
                    {
                        Text="Xamarin",
                        BackgroundColor = Color.Blue,
                        TextColor = Color.White
                    },
                    new Forms9Patch.Button
                    {
                        Text = "Forms9Patch",
                        BackgroundColor = Color.Blue,
                        TextColor = Color.White
                    }

                }
            };
        }
    }
}

