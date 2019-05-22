
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
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="popAfter">Will dissappear after popAfter TimeSpan</param>
        /// <returns></returns>
        public static Toast Create(string title, string text, TimeSpan popAfter = default(TimeSpan))
            => new Toast() { Title = title, Text = text, PopAfter = popAfter, IsVisible = true };

        #endregion


        #region Properties

        /// <summary>
        /// The title property backing store.
        /// </summary>
        public static readonly new BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(Toast), default(string));
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
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(Toast), default(string));
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
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Toast), Color.Black);
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
        /*
        readonly Button _okButton = new Button
        {
            //HorizontalOptions = LayoutOptions.Fill
        };
        */
        #endregion


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Toast"/> class.
        /// </summary>
        public Toast()
        {
            /*
            _okButton.BackgroundColor = ButtonBackgroundColor;
            _okButton.TextColor = ButtonTextColor;
            _okButton.HtmlText = ButtonText;
            */
            WidthRequest = 200;
            //HeightRequest = 200;

            //_okButton.Tapped += (s, args) => Cancel();
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
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            base.OnPropertyChanged(propertyName);

            if (propertyName == TitleProperty.PropertyName)
                _titleLabel.HtmlText = Title;
            else if (propertyName == TextProperty.PropertyName)
                _textLabel.HtmlText = Text;
            /*
            else if (propertyName == ButtonTextProperty.PropertyName)
                _okButton.HtmlText = ButtonText;
            else if (propertyName == ButtonBackgroundColorProperty.PropertyName)
                _okButton.BackgroundColor = ButtonBackgroundColor;
            else if (propertyName == ButtonTextColorColorProperty.PropertyName)
                _okButton.TextColor = ButtonTextColor;
                */
            else if (propertyName == TextColorProperty.PropertyName)
            {
                _textLabel.TextColor = TextColor;
                _titleLabel.TextColor = TextColor;
            }

        }
        /*
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
        */
        #endregion
    }
}

