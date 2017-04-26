// /*******************************************************************
//  *
//  * CustomFontEffectPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
namespace Forms9PatchDemo
{
	public class ChrisCustomFontEffectPage : ContentPage
	{
		public ChrisCustomFontEffectPage()
		{
			var label = new Xamarin.Forms.Label
			{
				Text = "Xamarin.Forms.Label",
				FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
			};
			label.Effects.Add(Effect.Resolve("Forms9Patch.CustomFontEffect"));

			var editor = new Xamarin.Forms.Editor
			{
				Text = "Xamarin.Forms.Editor",
				FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
			};
			editor.Effects.Add(Effect.Resolve("Forms9Patch.CustomFontEffect"));

			var entry = new Xamarin.Forms.Entry
			{
				Text = "Xamarin.Forms.Entry",
				FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
			};
			entry.Effects.Add(Effect.Resolve("Forms9Patch.CustomFontEffect"));

			var button = new Xamarin.Forms.Button
			{
				Text = "Xamarin.Forms.Button",
				FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
			};
			button.Effects.Add(Effect.Resolve("Forms9Patch.CustomFontEffect"));
            
            var F9Plabel1 = new Forms9Patch.Label
            {
                Text = "Forms9Patch.Label - luximb",
                FontFamily = "Forms9PatchDemo.Resources.Fonts.luximb.ttf"
            };
            
            var F9Plabel2 = new Forms9Patch.Label
            {
                Text = "Forms9Patch.Label - CPMono",
                FontFamily = "Forms9PatchDemo.Resources.Fonts.CPMono_v07_Bold.otf"
            };
            
            var F9Plabel3 = new Forms9Patch.Label
            {
                Text = "Forms9Patch.Label - default FontFamily",
                // FontFamily = "Forms9PatchDemo.Resources.Fonts.CPMono_v07 Bold.otf"
            };


			Content = new StackLayout
			{
				Children =
				{
					label,
					editor,
					entry,
					button,
					F9Plabel1,
					F9Plabel2,
					F9Plabel3
				}
			};
		}
	}
}

