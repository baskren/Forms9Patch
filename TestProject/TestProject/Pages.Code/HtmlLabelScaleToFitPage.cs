// /*******************************************************************
//  *
//  * LabelScaleToFitPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;

namespace FormsMod
{
	public class HtmlLabelScaleToFitPage : ContentPage
	{

		string text = "<b>Lorem</b> ipsum <em>dolor</em> sit <u>amet</u>, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
		LineBreakMode breakMode = LineBreakMode.TailTruncation;

		public HtmlLabelScaleToFitPage()
		{
			Content = new ScrollView
			{
				Content = new StackLayout
				{
					Children = {
						/*
						new Label { Text = "Lines=0" },
						new Forms9Patch.Label {
							HtmlText = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 0,
							LineBreakMode = breakMode,
						},
						new Label { Text = "Lines=0 ScaleFontToFitBounds=true" },
						new Forms9Patch.Label {
							HtmlText = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 0,
							ScaleFontToFitBounds = true,
							LineBreakMode = breakMode,
						},
						new Label { Text = "Lines=1" },
						new Forms9Patch.Label {
							HtmlText = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 1,
							LineBreakMode = breakMode,
						},
						new Label { Text = "Lines=1 ScaleFontToFitBounds=true" },
						new Forms9Patch.Label {
							HtmlText = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 1,
							ScaleFontToFitBounds = true,
							LineBreakMode = breakMode,
						},
						new Label { Text = "Lines=2" },
						new Forms9Patch.Label {
							HtmlText = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 2,
							LineBreakMode = breakMode,
						},
						new Label { Text = "Lines=2 ScaleFontToFitBounds=true" },
						new Forms9Patch.Label {
							HtmlText = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 2,
							ScaleFontToFitBounds = true,
							LineBreakMode = breakMode,
						},
						new Label { Text = "Lines=3:" },
						new Forms9Patch.Label {
							HtmlText = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 3,
							LineBreakMode = breakMode,
						},
*/
						new Label { Text = "Lines=3 ScaleFontToFitBounds=true" },
						new Forms9Patch.Label {
							HtmlText = text,
							BackgroundColor = Color.Gray,
							TextColor = Color.Black,
							HeightRequest = 50,
							Lines = 3,
							Fit = Forms9Patch.LabelFit.Width,
							LineBreakMode = breakMode,
						},
					}

				}
			};
		}
	}
}


