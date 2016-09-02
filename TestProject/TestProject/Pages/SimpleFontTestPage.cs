using System;

using Xamarin.Forms;

namespace TestProject
{
	public class SimpleFontTestPage : ContentPage
	{
		public SimpleFontTestPage ()
		{
			Padding = 20;
			Content = new StackLayout {
				VerticalOptions = LayoutOptions.Center,
				Children = {
					new Forms9Patch.Label {
						//HorizontalTextAlignment = TextAlignment.Center,
						//Text = "",
						HtmlText = "&#xE14E; &#xE195;  &#xEB3E;",
						FontFamily = "TestProject.Resources.Fonts.MaterialIcons-Regular.ttf",
						BackgroundColor = Color.Gray
					}
				}
			};
		}
	}
}


