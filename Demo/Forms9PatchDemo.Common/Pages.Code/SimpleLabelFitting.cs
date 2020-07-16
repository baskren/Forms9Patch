using System;

using Xamarin.Forms;
using Forms9Patch;

namespace Forms9PatchDemo.Pages.Code
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class SimpleLabelFitting : Xamarin.Forms.ContentPage
    {
        public SimpleLabelFitting()
        {
            Title = "Simple Label Fitting";
            Padding = new Thickness(0, 60, 0, 0);
            BackgroundColor = "#252525".ToColor();
            Content = new Xamarin.Forms.StackLayout
            {
                Padding = 10,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    /*
                    new Forms9Patch.Label
                    {
                        Text = "Lines=0",
                        Margin = new Thickness(20,0,20,0),
                        FontSize = 60,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Start,
                        TextColor = Color.Red,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.Fill,
                        Lines=0,
                        BackgroundColor = Color.Beige,

                    },
                    */
                    new Forms9Patch.Label
                    {
                        Text = "AutoFit.Width, Lines=0",
                        Margin = new Thickness(20,0,20,0),
                        FontSize = 60,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Start,
                        TextColor = Color.Red,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.Fill,
                        Lines=0,
                        AutoFit=AutoFit.Width,
                        BackgroundColor = Color.Beige,
                    },
                    /*
                    new Forms9Patch.Label
                    {
                        Text = "AutoFit.Width, Lines=1",
                        Margin = new Thickness(20,0,20,0),
                        FontSize = 60,
                        Lines=1,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Start,
                        TextColor = Color.Red,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.Fill,
                        AutoFit=AutoFit.Width,
                        BackgroundColor = Color.Beige,
                    },
                    new Forms9Patch.Frame
                    {
                        Padding = 2,
                        OutlineColor = Color.DarkGray,
                        OutlineRadius = 3,
                        OutlineWidth = 1,
                        HeightRequest = 50,
                        Content = new Forms9Patch.Label
                        {
                            Text = "Lines=0",
                            Margin = new Thickness(20,0,20,0),
                            FontSize = 60,
                            FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Start,
                        TextColor = Color.Red,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.Fill,
                            Lines=0,
                            BackgroundColor = Color.Beige,
                        }
                    },
                    new Forms9Patch.Frame
                    {
                        Padding = 2,
                        OutlineColor = Color.DarkGray,
                        OutlineRadius = 3,
                        OutlineWidth = 1,
                        HeightRequest = 50,
                        Content = new Forms9Patch.Label
                        {
                            Text = "AutoFit.Width, Lines=1",
                            Margin = new Thickness(20,0,20,0),
                            FontSize = 60,
                            FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Start,
                        TextColor = Color.Red,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.Fill,
                            Lines=1,
                            AutoFit=AutoFit.Width,
                            BackgroundColor = Color.Beige,
                        },
                    },
                    new Forms9Patch.Frame
                    {
                        Padding = 2,
                        OutlineColor = Color.DarkGray,
                        OutlineRadius = 3,
                        OutlineWidth = 1,
                        HeightRequest = 50,
                        Content = new Forms9Patch.Label
                        {
                            Text = "AutoFit.Lines, Lines=1",
                            Margin = new Thickness(20,0,20,0),
                            FontSize = 60,
                            FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Start,
                        TextColor = Color.Red,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.Fill,
                            Lines=1,
                            AutoFit=AutoFit.Lines,
                            BackgroundColor = Color.Beige,
                        }
                    },
                    */
                }
            };
        }
    }
}

