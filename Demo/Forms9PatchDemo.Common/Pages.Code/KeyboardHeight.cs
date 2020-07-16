using System;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    class KeyboardHeight : ContentPage
    {
        StackLayout _layout = new StackLayout();

        Entry _entry = new Entry();

        Label _label = new Label();

        BoxView _fillBox = new BoxView
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand,
            BackgroundColor = Color.Aqua
        };

        BoxView _endBox = new BoxView
        {
            HeightRequest = 20,
            WidthRequest = 20,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End,
            BackgroundColor = Color.Black
        };

        ContentView _contentView = new ContentView
        {
            VerticalOptions = LayoutOptions.Start,
        };


        public KeyboardHeight()
        {
            Forms9Patch.KeyboardService.Activate();
            Forms9Patch.KeyboardService.HeightChanged += KeyboardService_HeightChanged;

            _layout.Children.Add(_entry);
            _layout.Children.Add(_label);
            _layout.Children.Add(_fillBox);
            _layout.Children.Add(_endBox);

            SizeChanged += OnSizeChanged;

            _contentView.Content = _layout;

            Content = _contentView;
        }

        Size _pageSize;

        void OnSizeChanged(object sender, EventArgs e)
        {
            _pageSize = Bounds.Size;
            _contentView.HeightRequest = (_pageSize.Height - Forms9Patch.KeyboardService.Height);// / Forms9Patch.Display.Scale - 80;
            _label.Text = "Keyboard Height: " + Forms9Patch.KeyboardService.Height + "\nRequest: " + _contentView.HeightRequest.ToString() + "\nBounds: " + _contentView.Bounds.Height.ToString();
        }

        void KeyboardService_HeightChanged(object sender, double e)
        {
            _contentView.HeightRequest = (_pageSize.Height - Forms9Patch.KeyboardService.Height);// / Forms9Patch.Display.Scale - 80;
            _label.Text = "Keyboard Height: " + Forms9Patch.KeyboardService.Height + "\nRequest: " + _contentView.HeightRequest.ToString() + "\nBounds: " + _contentView.Bounds.Height.ToString();
        }
    }
}