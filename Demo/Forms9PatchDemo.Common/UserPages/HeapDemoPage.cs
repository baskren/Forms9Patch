// /*******************************************************************
//  *
//  * HeapDemoPage.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class HeapDemoPage : ContentPage
    {
        public HeapDemoPage()
        {
            Title = "Forms9Patch Demo";
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    new Forms9Patch.StateButton
                    {
                        DefaultState = new Forms9Patch.ButtonState
                        {
                            BackgroundImage = new Forms9Patch.Image
                            {
                                Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.delay")
                            },
                        },
                        SelectedState = new Forms9Patch.ButtonState
                        {
                            BackgroundImage = new Forms9Patch.Image
                            {
                                Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.tick")
                            },
                        },
                        WidthRequest = 150,
                        HeightRequest = 150,
                        HorizontalOptions = LayoutOptions.Center, // needed for WidthRequest
						ToggleBehavior = true,
                    }
                }
            };
        }
    }
}
