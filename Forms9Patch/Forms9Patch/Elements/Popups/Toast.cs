using System;

using Xamarin.Forms;

namespace Forms9Patch
{
	public class Toast : ModalPopup
	{
		#region Properties
		public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(Toast), default(string));
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(Toast), default(string));
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
		#endregion

		public Toast()
		{
			var okButton = new MaterialButton { Text = "OK" };
			Content = new StackLayout
			{
				//WidthRequest = Width - 80,
				Children = {
						new Label { HtmlText = Title, TextColor = Color.Black, FontAttributes=FontAttributes.Bold },
						new Label { HtmlText = Text, TextColor = Color.Black },
						okButton
					}
			};
			BackgroundColor = Color.FromRgb(1.0, 1.0, 1.0);
			HasShadow = true;
			OutlineRadius = 4;
			okButton.Tapped += (s, args) => Cancel();
		}
	}
}

