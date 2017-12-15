// /*******************************************************************
//  *
//  * EmbeddedResourceFontEffectPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
namespace Forms9PatchDemo
{
	public class EmbeddedResourceFontEffectPage : ContentPage
	{
		public EmbeddedResourceFontEffectPage()
		{
            var label = new Xamarin.Forms.Label
            {
                Text = "Xamarin.Forms.Label",
                FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
			};
			label.Effects.Add(new Forms9Patch.EmbeddedResourceFontEffect());

			var editor = new Xamarin.Forms.Editor
			{
				Text = "Xamarin.Forms.Editor",
                FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
            };
			editor.Effects.Add(new Forms9Patch.EmbeddedResourceFontEffect());

			var entry = new Xamarin.Forms.Entry
			{
				Text = "Xamarin.Forms.Entry",
                FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
            };
			entry.Effects.Add(new Forms9Patch.EmbeddedResourceFontEffect());

			var button = new Xamarin.Forms.Button
			{
				Text = "Xamarin.Forms.Button",
                FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
            };
			button.Effects.Add(new Forms9Patch.EmbeddedResourceFontEffect());

			Content = new StackLayout
			{
				Children =
				{
					label,
					editor,
					entry,
					button
				}
			};
		}
	}
}

