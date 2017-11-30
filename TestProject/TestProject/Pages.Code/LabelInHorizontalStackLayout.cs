// /*******************************************************************
//  *
//  * LabelInHorizontalStackLayout.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public class LabelInHorizontalStackLayout : ContentPage
	{
		static string text1 = "Żyłę;^`g <b><em>Lorem</em></b> ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";

		Entry entry = new Entry { Text = text1, };
		List<Forms9Patch.Image> images = new List<Forms9Patch.Image>();
		List<Forms9Patch.Label> labels = new List<Forms9Patch.Label>();
		List<StackLayout> layouts = new List<StackLayout>();

		StackLayout content = new StackLayout();

		public LabelInHorizontalStackLayout()
		{
			content.Children.Add(entry);
			content.Children.Add(new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Pink,
				Children = 
				{ 
					new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button")}				
				}
			});
			for (int i = 0; i < 6; i++)
			{
				images.Add(new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button") });
				labels.Add(new Forms9Patch.Label { BackgroundColor = Color.Green});
				labels[i].SetBinding(Forms9Patch.Label.HtmlTextProperty, "Text");
				labels[i].BindingContext = entry;
				if (i > 3)
				{
					labels[i].Lines = 2;
					labels[i].AutoFit = Forms9Patch.AutoFit.Lines;
				}
				else if (i > 1)
				{
					labels[i].Lines = 2;
					labels[i].AutoFit = Forms9Patch.AutoFit.Width;
				}
				labels[i].LineBreakMode = LineBreakMode.TailTruncation;
				layouts.Add(new StackLayout
				{
					Orientation = StackOrientation.Horizontal,
					BackgroundColor = Color.Pink,
					Children = { images[i], labels[i] }
				});
				if (i % 2 == 0)
					layouts[i].HeightRequest = 50;
				content.Children.Add(new Label { Text = "Lines="+labels[i].Lines+"\tHeightRequest="+layouts[i].HeightRequest+"\tFit="+labels[i].AutoFit });
				content.Children.Add(layouts[i]);
			}

			Content = new ScrollView
			{
				Content = content
			};
		}
	}
}


