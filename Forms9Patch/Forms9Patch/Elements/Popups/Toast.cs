
using Xamarin.Forms;
using System;

namespace Forms9Patch
{
	/// <summary>
	/// Toast Popup: Plain and simple
	/// </summary>
	public class Toast : ModalPopup
	{
		/// <summary>
		/// Create the specified title and text.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="text">Text.</param>
		public static Toast Create(string title, string text)
		{
			var toast = new Toast() { Title = title, Text = text };
			toast.IsVisible = true;
			return toast;
		}

		#region Properties
		/// <summary>
		/// The title property backing store.
		/// </summary>
		public static readonly BindableProperty TitleProperty = BindableProperty.Create("ToastTitle", typeof(string), typeof(Toast), default(string));
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
		public static readonly BindableProperty TextProperty = BindableProperty.Create("ToastText", typeof(string), typeof(Toast), default(string));
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		/// The ok text property.
		/// </summary>
		public static readonly BindableProperty OkTextProperty = BindableProperty.Create("ToastOkText", typeof(string), typeof(Toast), "OK");
		/// <summary>
		/// Gets or sets the ok text.
		/// </summary>
		/// <value>The ok text.</value>
		public string OkText
		{
			get { return (string)GetValue(OkTextProperty); }
			set { SetValue(OkTextProperty, value); }
		}

		/// <summary>
		/// The ok button color property.
		/// </summary>
		public static readonly BindableProperty OkButtonColorProperty = BindableProperty.Create("ToastOkButtonColor", typeof(Color), typeof(Toast), default(Color));
		/// <summary>
		/// Gets or sets the color of the ok button.
		/// </summary>
		/// <value>The color of the ok button.</value>
		public Color OkButtonColor
		{
			get { return (Color)GetValue(OkButtonColorProperty); }
			set { SetValue(OkButtonColorProperty, value); }
		}

		/// <summary>
		/// The ok text color property.
		/// </summary>
		public static readonly BindableProperty OkTextColorProperty = BindableProperty.Create("ToastOkTextColor", typeof(Color), typeof(Toast), Color.Blue);
		/// <summary>
		/// Gets or sets the color of the ok text.
		/// </summary>
		/// <value>The color of the ok text.</value>
		public Color OkTextColor
		{
			get { return (Color)GetValue(OkTextColorProperty); }
			set { SetValue(OkTextColorProperty, value); }
		}

		#endregion


		#region Fields 
		readonly Label _titleLabel = new Label
		{
			FontSize = 24,
			FontAttributes = FontAttributes.Bold,
			TextColor = Color.Black,
			//HorizontalOptions = LayoutOptions.Fill,
		};
		readonly Label _textLabel = new Label
		{
			FontSize = 16,
			TextColor = Color.Black,
			//HorizontalOptions = LayoutOptions.Fill,
		};
		readonly MaterialButton _okButton = new MaterialButton
		{
			//HorizontalOptions = LayoutOptions.Fill
		};
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.Toast"/> class.
		/// </summary>
		public Toast() 
		{
			_okButton.BackgroundColor = OkButtonColor;
			_okButton.FontColor = OkTextColor;
			_okButton.HtmlText = OkText;

			Margin = 30;
			HasShadow = true;
			OutlineRadius = 4;

			WidthRequest = 200;
			HeightRequest = 200;

			//HorizontalOptions = LayoutOptions.Center;

			_okButton.Tapped += (s, args) => Cancel();
			Content = new StackLayout
			{
				Children = 
				{
					_titleLabel,
					new ScrollView
					{
						Content = _textLabel
					},
					_okButton
				}
			};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.Toast"/> class.
		/// </summary>
		/// <param name="target">Target.</param>
		[Obsolete]
		public Toast(VisualElement target = null) : base(target)
		{
			throw new NotSupportedException(PCL.Utils.ReflectionExtensions.CallerMemberName() + " is obsolete.");
		}

		/// <summary>
		/// Ons the property changed.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == TitleProperty.PropertyName)
				_titleLabel.HtmlText = Title;
			else if (propertyName == TextProperty.PropertyName)
				_textLabel.HtmlText = Text;
			else if (propertyName == OkTextProperty.PropertyName)
				_okButton.HtmlText = OkText;
			else if (propertyName == OkButtonColorProperty.PropertyName)
				_okButton.BackgroundColor = OkButtonColor;
			else if (propertyName == OkTextColorProperty.PropertyName)
				_okButton.FontColor = OkTextColor;
		}

	}
}

