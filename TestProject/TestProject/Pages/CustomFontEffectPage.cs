// /*******************************************************************
//  *
//  * CustomFontEffectPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
namespace TestProject
{
	public class CustomFontEffectPage : ContentPage
	{
		public CustomFontEffectPage()
		{
			var label = new Xamarin.Forms.Label
			{
				Text = "Xamarin.Forms.Label",
				FontFamily = "TestProject.Resources.Fonts.Pacifico.ttf"
			};
			label.Effects.Add(Effect.Resolve("Forms9Patch.CustomFontEffect"));

			var editor = new Xamarin.Forms.Editor
			{
				Text = "Xamarin.Forms.Editor",
				FontFamily = "TestProject.Resources.Fonts.Pacifico.ttf"
			};
			editor.Effects.Add(Effect.Resolve("Forms9Patch.CustomFontEffect"));

			var entry = new Xamarin.Forms.Entry
			{
				Text = "Xamarin.Forms.Entry",
				FontFamily = "TestProject.Resources.Fonts.Pacifico.ttf"
			};
			entry.Effects.Add(Effect.Resolve("Forms9Patch.CustomFontEffect"));

			var button = new Xamarin.Forms.Button
			{
				Text = "Xamarin.Forms.Button",
				FontFamily = "TestProject.Resources.Fonts.Pacifico.ttf"
			};
			button.Effects.Add(Effect.Resolve("Forms9Patch.CustomFontEffect"));

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

