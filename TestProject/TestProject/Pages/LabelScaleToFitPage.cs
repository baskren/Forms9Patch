// /*******************************************************************
//  *
//  * LabelScaleToFitPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;

namespace FormsMod
{
	public class LabelScaleToFitPage : ContentPage
	{

		string text1 = "Żyłę;^`g Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
		string text2 = "Curabitur tincidunt ultrices facilisis. Duis posuere metus a mattis fermentum. Etiam volutpat lobortis justo non porttitor. Nullam porta, nunc nec egestas viverra, lacus risus semper turpis, eu rutrum lacus nulla ac est. Nullam blandit lorem eget urna euismod, in sodales est ultricies. Integer tellus velit, cursus id dignissim id, tempus eu nulla. Aliquam non efficitur mauris. Pellentesque et facilisis augue, vel faucibus libero. Aenean rhoncus vestibulum magna ac porttitor. Nunc vestibulum bibendum orci ut efficitur. Integer nisi tortor, luctus non molestie eget, luctus vel elit. Nullam elit tortor, pretium a placerat vitae, semper vitae eros. Integer sagittis nisi eu lorem finibus varius. Duis feugiat risus eget posuere commodo. Nullam non mauris malesuada, congue orci sit amet, pellentesque magna. Mauris molestie faucibus ex.";
		string text3 = "Cras sollicitudin velit dignissim convallis ultrices. Vestibulum lacinia tortor vitae ipsum sodales mattis. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent lorem leo, condimentum eu orci quis, faucibus tempor purus. Maecenas nulla erat, pulvinar eu risus ac, tincidunt eleifend eros. Aliquam accumsan pellentesque porta. Nullam cursus ipsum ac semper luctus. Duis et lectus sed dolor hendrerit porttitor in sit amet mi. Vivamus neque urna, vehicula sed rutrum in, feugiat eu nisl. Aenean dui magna, venenatis sit amet gravida a, ultricies a enim.";
		string text4 = "";

		string text = "Żyłę;^`g Lorem ipsum dolor sit amet, consectetur adipiscing elit";

		LineBreakMode breakMode = LineBreakMode.TailTruncation;

		Forms9Patch.LabelFit fit = Forms9Patch.LabelFit.Width;

		public LabelScaleToFitPage()
		{

			for (int i = 0; i < 256; i++)
				text4 += (i + 1) + " Żyłę;^`g\n";
			Content = new ScrollView
			{
				Content = new StackLayout
				{
					Children = {
						
						new Label { Text = "Lines=0" },
						new Forms9Patch.Label {
							Text = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 0,
							LineBreakMode = breakMode,
						},
						new Label { Text = "Lines=0 ScaleFontToFitBounds=true" },
						new Forms9Patch.Label {
							Text = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 0,
							Fit = fit,
							LineBreakMode = breakMode,
						},

						new Label { Text = "Lines=1" },
						new Forms9Patch.Label {
							Text = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 1,
							LineBreakMode = breakMode,
						},
						new Label { Text = "Lines=1 ScaleFontToFitBounds=true" },
						new Forms9Patch.Label {
							Text = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 1,
							Fit = fit,
							LineBreakMode = breakMode,
						},
						new Label { Text = "Lines=2" },
						new Forms9Patch.Label {
							Text = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 2,
							LineBreakMode = breakMode,
						},
						new Label { Text = "Lines=2 ScaleFontToFitBounds=true" },
						new Forms9Patch.Label {
							Text = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 2,
							Fit = fit,
							LineBreakMode = breakMode,
						},

						new Label { Text = "Lines=3:" },
						new Forms9Patch.Label {
							Text = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 3,
							LineBreakMode = breakMode,
						},

						new Label { Text = "Lines=3 ScaleFontToFitBounds=true" },
						new Forms9Patch.Label {
							Text = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 3,
							Fit = fit,
							LineBreakMode = breakMode,
						},


						new Label { Text = "Lines=4:" },
						new Forms9Patch.Label {
							Text = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 4,
							LineBreakMode = breakMode,
						},

						new Label { Text = "Lines=4 ScaleFontToFitBounds=true" },
						new Forms9Patch.Label {
							Text = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 4,
							Fit = fit,
							LineBreakMode = breakMode,
						},

						new Label { Text = "Lines=12 ScaleFontToFitBounds=true" },
						new Forms9Patch.Label {
							Text = text + "\n" + text2,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 150,
							Lines = 12,
							Fit = fit,
							LineBreakMode = breakMode,
						},

						new Label { Text = "Lines=96 ScaleFontToFitBounds=true" },
						new Forms9Patch.Label {
							//Text = text + "\n" + text2 + "\n" + text3 + "\n" +  text + "\n" + text2 + "\n" + text3,
							Text = text4,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 1200,
							Lines = 96,
							Fit = fit,
							LineBreakMode = breakMode,
						},
}

				}
			};
		}
	}
}


