using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class JHGamerPage : ContentPage
    {
        #region Variables
        String _strText = "SAMPLE TEXT ENTRY";
        readonly Forms9Patch.Label _lblTest;
        #endregion

        public JHGamerPage()
        {
            #region Page creation
            #region Not related to Forms9Patch
            Image item1 = new Image
            {
                Source = ImageSource.FromFile("blkbox_small.jpg")
            };
            Image item2 = new Image
            {
                Source = ImageSource.FromFile("blkbox_large.jpg")
            };
            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            Button btnWelcome = new Button
            {
                Text = "Welcome! Click me",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            btnWelcome.Clicked += BtnWelcome_Clicked;
            stackLayout.Children.Add(btnWelcome);
            #endregion

            _lblTest = new Forms9Patch.Label
            {
                Text = _strText,
                TextColor = Color.Black,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 40,
                Lines = 1,
                AutoFit = Forms9Patch.AutoFit.Width,
            };

            Grid gridTest = new Grid
            {
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Auto) },
                },
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.White,
                Padding = new Thickness(10, 0),
            };
            gridTest.Children.Add(item1, 0, 0);
            gridTest.Children.Add(_lblTest, 1, 0);
            gridTest.Children.Add(item2, 2, 0);

            Grid gridPage = new Grid
            {
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
            };
            gridPage.RowDefinitions.Add(new RowDefinition { Height = new GridLength(.07, GridUnitType.Star) });
            gridPage.RowDefinitions.Add(new RowDefinition { Height = new GridLength(.9, GridUnitType.Star) });
            gridPage.Children.Add(gridTest, 0, 0);
            gridPage.Children.Add(stackLayout, 0, 1);


            gridPage.Padding = new Thickness(0, 30, 0, 0);
            #endregion

            Content = gridPage;
        }

        #region Methods
        private void TextSwitch()
        {
            Console.WriteLine("TextSwitch(); entered");

            Device.BeginInvokeOnMainThread(() =>
            {
                _lblTest.Text = _strText;
            });
        }
        #endregion

        #region Events
        private void BtnWelcome_Clicked(object sender, EventArgs e)
        {
            switch (_strText)
            {
                default:
                case "SAMPLE TEXT ENTRY":
                    _strText = "ANOTHER SAMPLE";
                    break;
                case "ANOTHER SAMPLE":
                    _strText = "TESTING LONG TEXT ENTRY";
                    break;
                case "TESTING LONG TEXT ENTRY":
                    _strText = "SAMPLE TEXT ENTRY";
                    break;
            }

            //  UI update
            TextSwitch();
        }
        #endregion
    }
}