using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
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
						Text = "[] [] [] [] [] [] [] []",
						FontFamily = "Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf",
					},
                    new Forms9Patch.Label {
						HtmlText = "[&#xE14E;] [&#xE195;] [] [&#xEB3E;]",
						FontFamily = "Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf",
                    },
                    new Forms9Patch.Label {
						HtmlText = "[]", // "[&#xE14E;]",// [&#xE195;] [] [&#xEB3E;]",
						FontFamily = "Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf",
                    }
                }
            };
		}
	}
}


