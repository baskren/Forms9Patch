using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Forms9Patch
{
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class BusyToast : BubblePopup
    {
        #region SpinnerColor property
        /// <summary>
        /// The title property backing store.
        /// </summary>
        public static readonly new BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(BusyToast), default(string));
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
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(BusyToast), default(string));
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }


        public static readonly BindableProperty SpinnerColorProperty = BindableProperty.Create(nameof(SpinnerColor), typeof(Color), typeof(BusyToast), default(Color));
        public Color SpinnerColor
        {
            get { return (Color)GetValue(SpinnerColorProperty); }
            set { SetValue(SpinnerColorProperty, value); }
        }
        #endregion

        protected readonly ActivityIndicator _spinner = new ActivityIndicator
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.Transparent,
        };

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

        public static BusyToast Create(VisualElement target, string title, string text, Color spinnerColor = default, TimeSpan popAfter = default)
            => new BusyToast(target) { Title = title, Text = text, PopAfter = popAfter, SpinnerColor = spinnerColor, IsVisible = true };

        public static BusyToast Create(string title, string text, Color spinnerColor = default, TimeSpan popAfter = default)
            => new BusyToast(null) { Title = title, Text = text, PopAfter = popAfter, SpinnerColor = spinnerColor, IsVisible = true };

        public BusyToast(VisualElement target) : base(target)
        {
            var scroll = new ScrollView
            {
                Content = _textLabel
            };
            Grid.SetRowSpan(_spinner, 2);
            Grid.SetColumn(_titleLabel, 1);
            Grid.SetColumn(scroll, 1);
            Grid.SetRow(scroll, 1);
            Content = new Xamarin.Forms.Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = 40 },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Star }
                },
                Children =
                {
                    _titleLabel,
                    scroll,
                    _spinner
                }
            };
        }

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
                else if (propertyName == SpinnerColorProperty.PropertyName)
                    _spinner.Color = SpinnerColor;
                else if (propertyName == IsVisibleProperty.PropertyName)
                    _spinner.IsRunning = IsVisible;
            });
        }
        #endregion


    }
}
