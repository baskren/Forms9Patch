using Xamarin.Forms;
using System;

namespace Forms9Patch
{
    /// <summary>
    /// Permission popup.
    /// </summary>
    public class PermissionPopup : BubblePopup
    {

        #region Factories
        /// <summary>
        /// Create the specified title, text, okText, cancelText, okButtonColor, cancelButtonColor, okTextColor and cancelTextColor.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="okText">Ok text.</param>
        /// <param name="cancelText">Cancel text.</param>
        /// <param name="okButtonColor">Ok button color.</param>
        /// <param name="cancelButtonColor">Cancel button color.</param>
        /// <param name="okTextColor">Ok text color.</param>
        /// <param name="cancelTextColor">Cancel text color.</param>
        public static PermissionPopup Create(string title, string text, string okText = "OK", string cancelText = "Cancel", Color okButtonColor = default(Color), Color cancelButtonColor = default(Color), Color okTextColor = default(Color), Color cancelTextColor = default(Color))
        {
            var popup = new PermissionPopup { Title = title, Text = text, OkText = okText, CancelText = cancelText };
            if (okTextColor != default(Color))
                popup.OkTextColor = okTextColor;
            if (okButtonColor != default(Color))
                popup.OkButtonColor = okButtonColor;
            if (cancelTextColor != default(Color))
                popup.CancelTextColor = cancelTextColor;
            if (cancelButtonColor != default(Color))
                popup.CancelButtonColor = cancelButtonColor;
            popup.IsVisible = true;
            return popup;
        }

        /// <summary>
        /// Create the specified target, title, text, okText, cancelText, okButtonColor, cancelButtonColor, okTextColor and cancelTextColor.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="target">Target.</param>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="okText">Ok text.</param>
        /// <param name="cancelText">Cancel text.</param>
        /// <param name="okButtonColor">Ok button color.</param>
        /// <param name="cancelButtonColor">Cancel button color.</param>
        /// <param name="okTextColor">Ok text color.</param>
        /// <param name="cancelTextColor">Cancel text color.</param>
        public static PermissionPopup Create(VisualElement target, string title, string text, string okText = "OK", string cancelText = "Cancel", Color okButtonColor = default(Color), Color cancelButtonColor = default(Color), Color okTextColor = default(Color), Color cancelTextColor = default(Color))
        {
            var popup = new PermissionPopup(target) { Title = title, Text = text, OkText = okText, CancelText = cancelText };
            if (okTextColor != default(Color))
                popup.OkTextColor = okTextColor;
            if (okButtonColor != default(Color))
                popup.OkButtonColor = okButtonColor;
            if (cancelTextColor != default(Color))
                popup.CancelTextColor = cancelTextColor;
            if (cancelButtonColor != default(Color))
                popup.CancelButtonColor = cancelButtonColor;
            popup.IsVisible = true;
            return popup;
        }
        #endregion


        #region Properties

        /// <summary>
        /// The title property backing store.
        /// </summary>
        public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(PermissionPopup), default(string));
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// The text property backing store.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(PermissionPopup), default(string));
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
        /// The ok text property.
        /// </summary>
        public static readonly BindableProperty OkTextProperty = BindableProperty.Create("OkText", typeof(string), typeof(PermissionPopup), "OK");
        /// <summary>
        /// Gets or sets the ok text.
        /// </summary>
        /// <value>The ok text.</value>
        public string OkText
        {
            get => (string)GetValue(OkTextProperty);
            set => SetValue(OkTextProperty, value);
        }

        /// <summary>
        /// The ok button color property.
        /// </summary>
        public static readonly BindableProperty OkButtonColorProperty = BindableProperty.Create("OkButtonColor", typeof(Color), typeof(PermissionPopup), Color.Blue);
        /// <summary>
        /// Gets or sets the color of the ok button.
        /// </summary>
        /// <value>The color of the ok button.</value>
        public Color OkButtonColor
        {
            get => (Color)GetValue(OkButtonColorProperty);
            set => SetValue(OkButtonColorProperty, value);
        }

        /// <summary>
        /// The ok text color property.
        /// </summary>
        public static readonly BindableProperty OkTextColorProperty = BindableProperty.Create("OkTextColor", typeof(Color), typeof(PermissionPopup), Color.White);
        /// <summary>
        /// Gets or sets the color of the ok text.
        /// </summary>
        /// <value>The color of the ok text.</value>
        public Color OkTextColor
        {
            get => (Color)GetValue(OkTextColorProperty);
            set => SetValue(OkTextColorProperty, value);
        }

        /// <summary>
        /// The cancel text property.
        /// </summary>
        public static readonly BindableProperty CancelTextProperty = BindableProperty.Create("CancelText", typeof(string), typeof(PermissionPopup), default(string));
        /// <summary>
        /// Gets or sets the cancel text.
        /// </summary>
        /// <value>The cancel text.</value>
        public string CancelText
        {
            get => (string)GetValue(CancelTextProperty);
            set => SetValue(CancelTextProperty, value);
        }

        /// <summary>
        /// The cancel button color property.
        /// </summary>
        public static readonly BindableProperty CancelButtonColorProperty = BindableProperty.Create("CancelButtonColor", typeof(Color), typeof(PermissionPopup), default(Color));
        /// <summary>
        /// Gets or sets the color of the cancel button.
        /// </summary>
        /// <value>The color of the cancel button.</value>
        public Color CancelButtonColor
        {
            get => (Color)GetValue(CancelButtonColorProperty);
            set => SetValue(CancelButtonColorProperty, value);
        }

        /// <summary>
        /// The cancel text color property.
        /// </summary>
        public static readonly BindableProperty CancelTextColorProperty = BindableProperty.Create("CancelTextColor", typeof(Color), typeof(PermissionPopup), default(Color));
        /// <summary>
        /// Gets or sets the color of the cancel text.
        /// </summary>
        /// <value>The color of the cancel text.</value>
        public Color CancelTextColor
        {
            get => (Color)GetValue(CancelTextColorProperty);
            set => SetValue(CancelTextColorProperty, value);
        }

        #endregion


        #region Events
        /// <summary>
        /// Occurs when ok button is tapped.
        /// </summary>
        public event EventHandler OkTapped;
        #endregion


        #region Fields 
        readonly Label _titleLabel = new Label
        {
            FontSize = 22,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.Black
        };
        readonly Label _textLabel = new Label
        {
            FontSize = 16,
            TextColor = Color.Black
        };
        readonly Button _okButton = new Button
        {
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        readonly Button _cancelButton = new Button
        {
            HorizontalOptions = LayoutOptions.FillAndExpand

        };
        #endregion


        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.PermissionPopup"/> class.
        /// </summary>
        public PermissionPopup(VisualElement target = null) : base(target)
        {
            _cancelButton.BackgroundColor = CancelButtonColor;
            _cancelButton.TextColor = CancelTextColor;
            _cancelButton.HtmlText = CancelText;
            _okButton.BackgroundColor = OkButtonColor;
            _okButton.TextColor = OkTextColor;
            _okButton.HtmlText = OkText;


            _cancelButton.Tapped += (s, args) => Cancel();
            _okButton.Tapped += (s, args) =>
            {
                OkTapped?.Invoke(this, EventArgs.Empty);
                IsVisible = false;
            };
            Content = new StackLayout
            {
                Children =
                {
                    _titleLabel,
                    new ScrollView
                    {
                        Content = _textLabel
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            _cancelButton, _okButton
                        }
                    }
                }
            };

        }
        #endregion


        #region Property Change Management
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
                _okButton.TextColor = OkTextColor;
            else if (propertyName == CancelTextProperty.PropertyName)
                _cancelButton.HtmlText = CancelText;
            else if (propertyName == CancelTextColorProperty.PropertyName)
                _cancelButton.TextColor = CancelTextColor;
            else if (propertyName == CancelButtonColorProperty.PropertyName)
                _cancelButton.BackgroundColor = CancelButtonColor;
        }
        #endregion
    }
}
