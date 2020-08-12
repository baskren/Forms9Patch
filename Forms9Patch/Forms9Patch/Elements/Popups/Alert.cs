using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Permission popup.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class Alert : BubblePopup
    {

#pragma warning disable CC0021 // Use nameof
        #region Factories
        /// <summary>
        /// Create the specified title, text, okText, cancelText, okButtonColor, cancelButtonColor, okTextColor and cancelTextColor.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="okText">Ok text.</param>
        /// <param name="okButtonColor">Ok button color.</param>
        /// <param name="okTextColor">Ok text color.</param>
        public static Alert Create(string title, string text, string okText = "OK", Color okButtonColor = default, Color okTextColor = default)
        {
            var popup = new Alert { Title = title, Text = text, OkText = okText };
            if (okTextColor != default)
                popup.OkTextColor = okTextColor;
            if (okButtonColor != default)
                popup.OkButtonColor = okButtonColor;
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
        /// <param name="okButtonColor">Ok button color.</param>
        /// <param name="okTextColor">Ok text color.</param>
        public static Alert Create(VisualElement target, string title, string text, string okText = "OK", Color okButtonColor = default, Color okTextColor = default)
        {
            var popup = new Alert(target) { Title = title, Text = text, OkText = okText };
            if (okTextColor != default)
                popup.OkTextColor = okTextColor;
            if (okButtonColor != default)
                popup.OkButtonColor = okButtonColor;
            popup.IsVisible = true;
            return popup;
        }
        #endregion
#pragma warning restore CC0021 // Use nameof


        #region Properties

        /// <summary>
        /// The title property backing store.
        /// </summary>
        public static readonly new BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(Alert), default(string));
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public new string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// The text property backing store.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(Alert), default(string));
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
        public static readonly BindableProperty OkTextProperty = BindableProperty.Create(nameof(OkText), typeof(string), typeof(Alert), "OK");
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
        public static readonly BindableProperty OkButtonColorProperty = BindableProperty.Create(nameof(OkButtonColor), typeof(Color), typeof(Alert), Color.Blue);
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
        public static readonly BindableProperty OkTextColorProperty = BindableProperty.Create(nameof(OkTextColor), typeof(Color), typeof(Alert), Color.White);
        /// <summary>
        /// Gets or sets the color of the ok text.
        /// </summary>
        /// <value>The color of the ok text.</value>
        public Color OkTextColor
        {
            get => (Color)GetValue(OkTextColorProperty);
            set => SetValue(OkTextColorProperty, value);
        }

        #region PermissionState property
        static readonly BindablePropertyKey PermissionStatePropertyKey = BindableProperty.CreateReadOnly(nameof(PermissionState), typeof(PermissionState), typeof(Alert), default(PermissionState));
        /// <summary>
        /// PermissionState Bindable Property
        /// </summary>
        public static readonly BindableProperty PermissionStateProperty = PermissionStatePropertyKey.BindableProperty;
        /// <summary>
        /// The current permission process state of the Permision Popup
        /// </summary>
        public PermissionState PermissionState
        {
            get { return (PermissionState)GetValue(PermissionStateProperty); }
            protected set { SetValue(PermissionStatePropertyKey, value); }
        }
        #endregion

        #endregion


        #region Events
        /// <summary>
        /// Occurs when ok button is tapped.
        /// </summary>
        public event EventHandler OkTapped;
        #endregion


        #region Visual Elements
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

        /// <summary>
        /// OK Button 
        /// </summary>
        protected readonly Button _okButton = new Button
        {
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        #endregion


        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Alert"/> class.
        /// </summary>
        public Alert(VisualElement target = null) : base(target)
        {
            _okButton.BackgroundColor = OkButtonColor;
            _okButton.TextColor = OkTextColor;
            _okButton.HtmlText = OkText;

            _okButton.Tapped += OnOkButtonTappedAsync;
            Content = new Xamarin.Forms.StackLayout
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
            CancelOnBackButtonClick = false;
            CancelOnPageOverlayTouch = false;
        }

        bool _disposed;
        /// <summary>
        /// Dispose instance
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                OkTapped = null;
                _okButton.Tapped -= OnOkButtonTappedAsync;
                _okButton.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion


        #region Event Handlers
        internal async void OnOkButtonTappedAsync(object sender, EventArgs e)
        {
            P42.Utils.BreadCrumbs.Add(GetType(), "ok");
            PermissionState = PermissionState.Ok;
            await PopAsync(_okButton, lastAction: () => OkTapped?.Invoke(this, EventArgs.Empty));
        }

        /// <summary>
        /// Cancel the Alert
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public override Task CancelAsync(object trigger = null)
        {
            if (PermissionState == PermissionState.Pending)
                PermissionState = PermissionState.Cancelled;
            return base.CancelAsync(trigger);
        }

        /// <summary>
        /// Called when appearing
        /// </summary>
        protected override void OnAppearing()
        {
            PermissionState = PermissionState.Pending;
            base.OnAppearing();
        }
        #endregion


        #region Property Change Management
        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
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
            });
        }
        #endregion
    }
}
