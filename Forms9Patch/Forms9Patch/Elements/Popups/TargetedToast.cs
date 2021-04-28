using Xamarin.Forms;
using System;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// A simple toast that points to an element
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class TargetedToast : BubblePopup
    {
        #region Factory
        /// <summary>
        /// Create a toast, pointing at the specified target, with title, text, and optional timeout (popAfter)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="popAfter"></param>
        /// <returns></returns>
        public static TargetedToast Create(VisualElement target, string title, string text, TimeSpan popAfter = default, FeedbackEffect pushedFeedback = FeedbackEffect.Info)
            => new TargetedToast(target) { Title = title, Text = text, PopAfter = popAfter, PushedFeedback = pushedFeedback, IsVisible = true };

        #endregion


        #region Properties
        /// <summary>
        /// The title property backing store.
        /// </summary>
        public static readonly new BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(TargetedToast), default(string));
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
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(TargetedToast), default(string));
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
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TargetedToast), Color.Black);
        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }


        /// <summary>
        /// The ok text property.
        /// </summary>
        public static readonly BindableProperty OkTextProperty = BindableProperty.Create(nameof(OkText), typeof(string), typeof(TargetedToast), "OK");
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
        public static readonly BindableProperty OkButtonColorProperty = BindableProperty.Create(nameof(OkButtonColor), typeof(Color), typeof(TargetedToast), default(Color));
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
        public static readonly BindableProperty OkTextColorProperty = BindableProperty.Create(nameof(OkTextColor), typeof(Color), typeof(TargetedToast), Color.Blue);
        /// <summary>
        /// Gets or sets the color of the ok text.
        /// </summary>
        /// <value>The color of the ok text.</value>
        public Color OkTextColor
        {
            get => (Color)GetValue(OkTextColorProperty);
            set => SetValue(OkTextColorProperty, value);
        }

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
#pragma warning disable CC0033 // Dispose Fields Properly
        readonly Button _okButton = new Button
        {
            AutomationId = nameof(_okButton)
        };
#pragma warning restore CC0033 // Dispose Fields Properly
        #endregion


        #region Constructor / Disposer
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.TargetedToast"/> class.
        /// </summary>
        /// <param name="target">Target.</param>
        public TargetedToast(VisualElement target) : base(target)
        {
            _okButton.BackgroundColor = OkButtonColor;
            _okButton.TextColor = OkTextColor;
            _okButton.HtmlText = OkText;

            _okButton.Tapped += OnOkButtonTappedAsync;
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

        bool _disposed;
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                _okButton.Tapped -= OnOkButtonTappedAsync;
                _okButton.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion


        #region Events
        async void OnOkButtonTappedAsync(object sender, EventArgs e)
            => await CancelAsync(_okButton);
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
                {
                    _okButton.HtmlText = OkText;
                    if (!string.IsNullOrWhiteSpace(_okButton.HtmlText) && !((StackLayout)Content).Children.Contains(_okButton))
                        ((StackLayout)Content).Children.Add(_okButton);
                    else if (string.IsNullOrWhiteSpace(_okButton.HtmlText) && ((StackLayout)Content).Children.Contains(_okButton))
                        ((StackLayout)Content).Children.Remove(_okButton);
                }
                else if (propertyName == OkButtonColorProperty.PropertyName)
                    _okButton.BackgroundColor = OkButtonColor;
                else if (propertyName == OkTextColorProperty.PropertyName)
                    _okButton.TextColor = OkTextColor;
                else if (propertyName == TextColorProperty.PropertyName)
                {
                    _titleLabel.TextColor = TextColor;
                    _textLabel.TextColor = TextColor;
                }
            });
        }
        #endregion
    }
}
