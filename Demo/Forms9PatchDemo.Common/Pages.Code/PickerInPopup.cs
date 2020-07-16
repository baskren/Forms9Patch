using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class PickerInPopup : ContentPage
    {
        
        Forms9Patch.SinglePicker _singlePicker = new Forms9Patch.SinglePicker
        {
            BackgroundColor = Color.White
        };
        
        Forms9Patch.Button _singlePickerButton = new Forms9Patch.Button
        {
            HtmlText = "<i>select year</i>",
            TextColor = Color.DarkGray,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            WidthRequest = -1,
        };
        
        List<string> _years = new List<string>();

        public PickerInPopup()
        {
            Title = "Picker in popup";

            for (int i = DateTime.Now.Year - 13; i > 1899; i--)
                _years.Add(i.ToString());
            
            _singlePicker.ItemsSource = _years;

            _singlePicker.PropertyChanged += OnSinglePickerPropertyChanged;
            var stackLayout = new StackLayout
            {
                Padding = 10,
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label { Text = "Year:", VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Fill },
                            _singlePickerButton,
                            new Label { Text = "Scale: " + Forms9Patch.Display.Scale, VerticalOptions = LayoutOptions.End},
                        }
                    },
                }
            };

            Content = stackLayout;
            _singlePickerButton.Clicked += OnSinglePickerButtonClicked;
            
        }

        
        private void OnSinglePickerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Forms9Patch.SinglePicker.SelectedItemProperty.PropertyName)
            {
                _singlePickerButton.HtmlText = (_singlePicker.SelectedItem as string) ?? "select year";
                _singlePickerButton.TextColor = _singlePicker.SelectedItem == null ? Color.DarkGray : Color.Blue;

            }
        }

        private void OnSinglePickerButtonClicked(object sender, EventArgs e)
        {
            var doneButton = new Forms9Patch.Button
            {
                BackgroundColor = Color.Blue,
                TextColor = Color.White,
                OutlineColor = Color.White,
                OutlineRadius = 4,
                OutlineWidth = 1,
                Text = "Done",
                HorizontalOptions = LayoutOptions.End,
            };
            var cancelButton = new Forms9Patch.Button
            {
                BackgroundColor = Color.Black,
                TextColor = Color.White,
                OutlineColor = Color.White,
                OutlineRadius = 4,
                OutlineWidth = 1,
                Text = "Cancel",
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };
            var buttonBar = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    cancelButton, doneButton
                }
            };
            var bubbleContent = new StackLayout
            {
                WidthRequest = 300,
                HeightRequest = 300,
                Children =
                {
                    buttonBar,
                    _singlePicker
                }
            };
            var bubblePopup = new Forms9Patch.BubblePopup(_singlePickerButton)
            {
                PointerDirection = Forms9Patch.PointerDirection.Vertical,
                Content = bubbleContent,
                BackgroundColor = Color.Black,
            };

            var selectedItemAtStart = _singlePicker.SelectedItem;
            doneButton.Clicked += async (s, args) => await bubblePopup.CancelAsync();
            cancelButton.Clicked += async (s, args) =>
            {
                _singlePicker.SelectedItem = selectedItemAtStart;
                await bubblePopup.CancelAsync();
            };

            bubblePopup.IsVisible = true;
        }
        
    }
}