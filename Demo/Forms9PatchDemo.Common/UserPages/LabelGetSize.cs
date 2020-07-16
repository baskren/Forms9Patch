using System;

using Xamarin.Forms;
using FormsGestures;

namespace Forms9PatchDemo
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	public class LabelGetSize : ContentPage
	{
		Forms9Patch.Label _label = new Forms9Patch.Label
		{
			Text = "This is some text.  It's about pizza and cheese.  And pepperoni.",
            TextColor = Color.Black
		};

		public LabelGetSize()
		{
			Content = new StackLayout
			{
				Children = {
					_label
				}
			};

			SizeChanged += (object sender, EventArgs e) =>
			{
				var size1 = _label.SizeForWidthAndFontSize(50, 12);
				var size2 = _label.SizeForWidthAndFontSize(100, 24);

				System.Diagnostics.Debug.WriteLine("size1=[" + size1 + "] size2=[" + size2 + "]");
			};
		}


	}
}

