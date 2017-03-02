// /*******************************************************************
//  *
//  * BackgroundImageOpacityPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public class BackgroundImageOpacityPage : ContentPage
	{
		public BackgroundImageOpacityPage()
		{

			Forms9Patch.Image image = new Forms9Patch.Image();
			image.Source = Forms9Patch.ImageSource.FromMultiResource("FormsGestures.Resources.rocket");
			image.Opacity = 0.3f;
			image.Fill = Forms9Patch.Fill.Fill;

			Slider slider = new Slider
			{
				Minimum = 0.0,
				Maximum = 1.0
			};
			slider.ValueChanged += (sender, e) => image.Opacity = e.NewValue;

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