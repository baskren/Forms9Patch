using Xamarin.Forms;
using System;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Toast Popup: Plain and simple
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
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
        public static Toast Create(string title, string text, TimeSpan popAfter = default, FeedbackEffect pushedFeedback = FeedbackEffect.Info)
            => new Toast { Title = title, Text = text, PopAfter = popAfter, PushedFeedback = pushedFeedback, IsVisible = true };

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

        public event EventHandler LongPressed;

        #endregion


        #region Fields 
        readonly Label _titleLabel = new Label
        {
            FontSize = 22,
            FontAttributes = FontAttributes.Bold,
            TextColor = (Color)TextColorProperty.DefaultValue
        };
        readonly Label _textLabel = new Label
        {
            FontSize = 16,
            TextColor = (Color)TextColorProperty.DefaultValue
        };

        readonly FormsGestures.Listener listener;

        //Forms9Patch.TargetedMenu targetedMenu;
        #endregion


        #region Events
        /// <summary>
        /// Occurs when HtmlText wrapped with an action (&lt;a&gt;) tag is tapped.
        /// </summary>
        public event EventHandler<ActionTagEventArgs> ActionTagTapped;
        #endregion


        #region Construction / Disposal
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Toast"/> class.
        /// </summary>
        public Toast()
        {
            Content = new StackLayout
            {
                Children =
                {
                    _titleLabel,
                    new ScrollView
                    {
                        Content = _textLabel
                    }
                }
            };
            listener = FormsGestures.Listener.For(Content);
            listener.LongPressing += OnListener_LongPressing;
            _textLabel.ActionTagTapped += OnTextLabel_ActionTagTapped;
        }


        private bool _disposed;
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;

                listener.LongPressing -= OnListener_LongPressing;
                _textLabel.ActionTagTapped -= OnTextLabel_ActionTagTapped;
                ActionTagTapped = null;
                listener.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion


        #region Gesture Handlers
        private async void OnListener_LongPressing(object sender, FormsGestures.LongPressEventArgs e)
        {
            LongPressed?.Invoke(this, new EventArgs());
            using (var targetedMenu = new TargetedMenu(this) { Segments = { new Segment("Copy") } })
            {
                targetedMenu.IsVisible = true;
                targetedMenu.SegmentTapped += OnTargetedMenu_SegmentTapped;
                await targetedMenu.WaitForPoppedAsync();
            }
        }
        private void OnTextLabel_ActionTagTapped(object sender, ActionTagEventArgs e)
        {
            ActionTagTapped?.Invoke(this, e);
        }


        private void OnTargetedMenu_SegmentTapped(object sender, SegmentedControlEventArgs e)
        {
            var entry = new MimeItemCollection
            {
                HtmlText = "<H3>" + Title + "</H3><p>" + Text + "</p>",
                PlainText = Title + "\n" + Text
            };
            Clipboard.Entry = entry;
        }
        #endregion


        #region PropertyChange management
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
                else if (propertyName == TextColorProperty.PropertyName)
                {
                    _textLabel.TextColor = TextColor;
                    _titleLabel.TextColor = TextColor;
                }
            });
        }
        #endregion
    }
}

