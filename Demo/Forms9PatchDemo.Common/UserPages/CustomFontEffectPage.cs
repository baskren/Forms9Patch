// /*******************************************************************
//  *
//  * EmbeddedResourceFontEffectPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
namespace Forms9PatchDemo
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	public class ChrisEmbeddedResourceFontEffectPage : ContentPage
	{
		public ChrisEmbeddedResourceFontEffectPage()
		{
			var label = new Xamarin.Forms.Label
			{
				Text = "Xamarin.Forms.Label",
				FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
			};
            Forms9Patch.EmbeddedResourceFontEffect.ApplyTo(label);

			var editor = new Xamarin.Forms.Editor
			{
				Text = "Xamarin.Forms.Editor",
				FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
			};
            Forms9Patch.EmbeddedResourceFontEffect.ApplyTo(editor);

			var entry = new Xamarin.Forms.Entry
			{
				Text = "Xamarin.Forms.Entry",
				FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
			};
            Forms9Patch.EmbeddedResourceFontEffect.ApplyTo(entry);

			var button = new Xamarin.Forms.Button
			{
				Text = "Xamarin.Forms.Button",
				FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
			};
            Forms9Patch.EmbeddedResourceFontEffect.ApplyTo(button);
            
            var F9Plabel1 = new Forms9Patch.Label
            {
                Text = "Forms9Patch.Label - luximb",
                FontFamily = "Forms9PatchDemo.Resources.Fonts.luximb.ttf",
                //TextColor = Color.Black
            };
            
            var F9Plabel2 = new Forms9Patch.Label
            {
                Text = "Forms9Patch.Label - CPMono",
                FontFamily = "Forms9PatchDemo.Resources.Fonts.CPMono_v07_Bold.otf",
                //TextColor = Color.Black
            };
            
            var F9Plabel3 = new Forms9Patch.Label
            {
                Text = "Forms9Patch.Label - default FontFamily",
                // FontFamily = "Forms9PatchDemo.Resources.Fonts.CPMono_v07 Bold.otf"
                //TextColor = Color.Black
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

