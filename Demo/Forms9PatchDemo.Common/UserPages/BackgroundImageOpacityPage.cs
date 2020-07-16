// /*******************************************************************
//  *
//  * BackgroundImageOpacityPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	public class BackgroundImageOpacityPage : ContentPage
	{
		public BackgroundImageOpacityPage()
		{

            Forms9Patch.Image image = new Forms9Patch.Image
            {
                Source = Forms9Patch.ImageSource.FromMultiResource("FormsGestures.Resources.rocket"),
                Opacity = 1,
                Fill = Forms9Patch.Fill.AspectFit
            };

            Slider slider = new Slider
			{
				Minimum = 0.0,
				Maximum = 1.0,
                Value = 1.0
			};
			slider.ValueChanged += (sender, e) => image.Opacity = e.NewValue;
            slider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(0.01));

			Forms9Patch.StackLayout stack = new Forms9Patch.StackLayout
			{
				Children = { 
					new Label { Text = "Opacity:" },
					slider 
				},
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			// replace between the two lines - in the first the opacity is ok, in the second there is no opacity
			//stack.Children.Add(image);
			stack.BackgroundImage = image;
			Content = stack;
		}
	}
}