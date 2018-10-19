
using Xamarin.Forms;
using System;

namespace Forms9Patch
{
    /// <summary>
    /// Toast Popup: Plain and simple
    /// </summary>
    public class Toast : ModalPopup
    {
        #region Factory
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
        #endregion


        #region Properties

        /// <summary>
        /// The title property backing store.
        /// </summary>
        public static readonly new BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(Toast), default(string));
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public new string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        #region Text Properties
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
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// The text color property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(Toast), Color.Black);
        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        #endregion


        #region Button Properties

        #region Button Text Property

        /// <summary>
        /// OBSOLETE: USE ButtonTextProperty
        /// </summary>
        [Obsolete("Use ButtonTextProperty")]
        public static readonly BindableProperty OkTextProperty = ButtonTextProperty;
        /// <summary>
        /// OBSOLETE: Use ButtonText property
        /// </summary>
        [Obsolete("Use ButtonText property")]
        public string OkText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        /// <summary>
        /// The ButtonText property backing store
        /// </summary>
        public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create("ButtonText", typeof(string), typeof(Toast), null);
        /// <summary>
        /// The ButtonText property
        /// </summary>
		public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }
        #endregion

        #region Button BackgroundColor Property
        /// <summary>
        /// OBSOLETE: Use ButtonBackgroundColorProperty instead
        /// </summary>
        [Obsolete("Use ButtonBackgroundColorProperty")]
        public static readonly BindableProperty OkButtonColorProperty = ButtonBackgroundColorProperty;
        /// <summary>
        /// OBSOLETE: Use ButtonBackgroundColor property instead
        /// </summary>
        [Obsolete("Use ButtonBackgroundColor property")]
        public Color OkButtonColor
        {
            get => (Color)GetValue(ButtonBackgroundColorProperty);
            set => SetValue(ButtonBackgroundColorProperty, value);
        }
        /// <summary>
        /// The ButtongBackgroundColor property backing store
        /// </summary>
        public static readonly BindableProperty ButtonBackgroundColorProperty = BindableProperty.Create("ButtonColor", typeof(Color), typeof(Toast), default(Color));
        /// <summary>
        /// The ButtonBackgroundColor property
        /// </summary>
		public Color ButtonBackgroundColor
        {
            get => (Color)GetValue(ButtonBackgroundColorProperty);
            set => SetValue(ButtonBackgroundColorProperty, value);
        }
        #endregion

        #region ButtonTextColor property
        /// <summary>
        /// OBSOLETE: Use ButtonTextColorColorProperty instead
        /// </summary>
        [Obsolete("Use ButtonTextColorProperty")]
        public static readonly BindableProperty OkTextColorColorProperty = ButtonTextColorColorProperty;
        /// <summary>
        /// OBSOLETE: Use ButtonTextColor instead
        /// </summary>
        [Obsolete("Use ButtonTextColor")]
        public Color OkTextColor
        {
            get => (Color)GetValue(ButtonTextColorColorProperty);
            set => SetValue(ButtonTextColorColorProperty, value);
        }
        /// <summary>
        /// ButtonTextColor property backing store
        /// </summary>
        public static readonly BindableProperty ButtonTextColorColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(Toast), default(Color));
        /// <summary>
        /// ButtonTextColor property
        /// </summary>
		public Color ButtonTextColor
        {
            get => (Color)GetValue(ButtonTextColorColorProperty);
            set => SetValue(ButtonTextColorColorProperty, value);
        }
        #endregion

        #endregion

        #endregion


        #region Fields 
        readonly Label _titleLabel = new Label
        {
            FontSize = 22,
            FontAttributes = FontAttributes.Bold,
            TextColor = (Color)TextColorProperty.DefaultValue,
            //HorizontalOptions = LayoutOptions.Fill,
        };
        readonly Label _textLabel = new Label
        {
            FontSize = 16,
            TextColor = (Color)TextColorProperty.DefaultValue,
            //HorizontalOptions = LayoutOptions.Fill,
        };
        readonly Button _okButton = new Button
        {
            //HorizontalOptions = LayoutOptions.Fill
        };
        #endregion


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Toast"/> class.
        /// </summary>
        public Toast()
        {
            _okButton.BackgroundColor = ButtonBackgroundColor;
            _okButton.TextColor = ButtonTextColor;
            _okButton.HtmlText = ButtonText;

            WidthRequest = 200;
            HeightRequest = 200;

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
            throw new NotSupportedException(P42.Utils.ReflectionExtensions.CallerMemberName() + " is obsolete.");
        }
        #endregion


        #region PropertyChange management
        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (P42.Utils.Environment.IsOnMainThread)
                base.OnPropertyChanged(propertyName);
            else
                Device.BeginInvokeOnMainThread(() => base.OnPropertyChanged(propertyName));

            if (propertyName == TitleProperty.PropertyName)
                _titleLabel.HtmlText = Title;
            else if (propertyName == TextProperty.PropertyName)
                _textLabel.HtmlText = Text;
            else if (propertyName == ButtonTextProperty.PropertyName)
                _okButton.HtmlText = ButtonText;
            else if (propertyName == ButtonBackgroundColorProperty.PropertyName)
                _okButton.BackgroundColor = ButtonBackgroundColor;
            else if (propertyName == ButtonTextColorColorProperty.PropertyName)
                _okButton.TextColor = ButtonTextColor;
            else if (propertyName == TextColorProperty.PropertyName)
            {
                _textLabel.TextColor = TextColor;
                _titleLabel.TextColor = TextColor;
            }

        }

        void UpdateOKButton()
        {
            if (P42.Utils.Environment.IsOnMainThread)
            {
                if (ButtonText != null || (ButtonBackgroundColor != default(Color) && ButtonBackgroundColor != Color.Default) || (ButtonTextColor != default(Color) && ButtonTextColor != Color.Default))
                {
                    if (!((StackLayout)Content).Children.Contains(_okButton))
                        ((StackLayout)Content).Children.Add(_okButton);
                    if (ButtonTextColor != default(Color) && ButtonTextColor != Color.Default)
                        _okButton.TextColor = Color.Blue;
                }
                else if (((StackLayout)Content).Children.Contains(_okButton))
                    ((StackLayout)Content).Children.Remove(_okButton);
            }
            else
                Device.BeginInvokeOnMainThread(UpdateOKButton);
        }
        #endregion
    }
}

