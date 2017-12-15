// /*******************************************************************
//  *
//  * LayoutWithLabel.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public class LayoutWithLabel : ContentPage
	{
		public LayoutWithLabel()
		{
			/*
			Forms9Patch.Label leftLabel = new Forms9Patch.Label { 
				Text = "left",
				HorizontalOptions = LayoutOptions.StartAndExpand,
				BackgroundColor = Color.Blue,
				TextColor = Color.Red,
				AutoFit = Forms9Patch.LabelFit.Lines,
				Lines = 1,
			};
			Forms9Patch.Label rightLabel = new Forms9Patch.Label { 
				Text = "right",
				HorizontalOptions = LayoutOptions.EndAndExpand,
				BackgroundColor = Color.Red,
				TextColor = Color.Blue,
				AutoFit = Forms9Patch.LabelFit.Lines,
				HorizontalTextAlignment = TextAlignment.End,
				Lines=1,
			};
*/
			StackLayout stackLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					//leftLabel,
					//rightLabel
				}
			};


			//Content = stackLayout;

			Forms9Patch.Label textLabel = new Forms9Patch.Label
			{
				//HtmlText = "⌫",
				//HtmlText = "*",
				//HtmlText = "&#xE14A;",
				HtmlText = "pizza cheese",
				//HorizontalOptions = LayoutOptions.Center,
				VerticalTextAlignment = TextAlignment.Center,
				BackgroundColor = Color.Green,
				TextColor = Color.White,
				AutoFit = Forms9Patch.AutoFit.Width,
				HorizontalTextAlignment = TextAlignment.Center,
				Lines = 1,
				FontSize = 200,
				FontAttributes = FontAttributes.Italic,
				//FontFamily = "Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf",
				FontFamily = "AppleSDGothicNeo-Thin",
			};


			AbsoluteLayout absLayout = new AbsoluteLayout { BackgroundColor = Color.Gray };

			AbsoluteLayout.SetLayoutBounds(stackLayout, new Rectangle(0.5, 0.0, 0.95, 0.1));
			AbsoluteLayout.SetLayoutFlags(stackLayout, AbsoluteLayoutFlags.All);
			absLayout.Children.Add(stackLayout);

			AbsoluteLayout.SetLayoutBounds(textLabel, new Rectangle(0.5, 0.5, 0.8, 0.8));
			AbsoluteLayout.SetLayoutFlags(textLabel, AbsoluteLayoutFlags.All);
			absLayout.Children.Add(textLabel);

			//Content = absLayout;

			Grid grid = new Grid
			{
				RowDefinitions = new RowDefinitionCollection
				{
					new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
					new RowDefinition { Height = new GridLength(1.35, GridUnitType.Star) },
					new RowDefinition { Height = new GridLength(1.35, GridUnitType.Star) },
					new RowDefinition { Height = new GridLength(1.35, GridUnitType.Star) },
					new RowDefinition { Height = new GridLength(1.35, GridUnitType.Star) },
					new RowDefinition { Height = new GridLength(1.35, GridUnitType.Star) },
					//new RowDefinition { Height = new GridLength(1.35, GridUnitType.Star) },
				},
				ColumnDefinitions = new ColumnDefinitionCollection
				{
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				},
				ColumnSpacing = 1,
				RowSpacing = 1,
			};

			grid.Children.Add(absLayout,1,1);

			Content = grid;
		}
	}
}


