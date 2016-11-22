
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Toast Popup: Plain and simple
	/// </summary>
	public class Toast : ModalPopup
	{
		#region Properties
		/// <summary>
		/// The title property backing store.
		/// </summary>
		public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(Toast), default(string));
		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		/// <summary>
		/// The text property backing store.
		/// </summary>
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(Toast), default(string));
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
		#endregion


		#region Fields 
		readonly Label _titleLabel = new Label
		{
			FontSize = 24,
			FontAttributes = FontAttributes.Bold,
			TextColor = Color.Black
		};
		readonly Label _textLabel = new Label
		{
			FontSize = 16,
			TextColor = Color.Black,
			Lines = 0,
			VerticalOptions = LayoutOptions.Fill,
		};
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.Toast"/> class.
		/// </summary>
		public Toast()
		{
			//Margin = 40;
			HorizontalOptions = LayoutOptions.Center;
			VerticalOptions = LayoutOptions.Center;

			#region bindings
			_titleLabel.SetBinding(Label.HtmlTextProperty, TitleProperty.PropertyName);
			_titleLabel.BindingContext = this;
			_textLabel.SetBinding(Label.HtmlTextProperty, TextProperty.PropertyName);
			_textLabel.BindingContext = this;
			#endregion
			var okButton = new MaterialButton { Text = "OK" };
			/*
			Content = new StackLayout
			{
				Children = 
				{
					_titleLabel,
					_textLabel,
					okButton
				}
			};
			*/
			Content = _textLabel;
			//BackgroundColor = Color.FromRgb(1.0, 1.0, 1.0);
			HasShadow = true;
			OutlineRadius = 4;
			okButton.Tapped += (s, args) => Cancel();
		}
	}
}

