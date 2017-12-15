using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    public class OldImageButtonCodePage : ContentPage
    {
        static void OnImageButtonTapped(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Tapped Button Text=[" + ((Forms9Patch.StateButton)sender).Text + "]");
        }

        static void OnImageButtonSelected(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Selected Button Text=[" + ((Forms9Patch.StateButton)sender).Text + "]");
        }

        static void OnImageButtonLongPressing(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressing Button Text=[" + ((Forms9Patch.StateButton)sender).Text + "]");
        }

        static void OnImageButtonLongPressed(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressed Button Text=[" + ((Forms9Patch.StateButton)sender).Text + "]");
        }

        public OldImageButtonCodePage()
        {

            #region ImageButtons
            var b2 = new Forms9Patch.StateButton
            {
                DefaultState = new Forms9Patch.ButtonState
                {
                    BackgroundImage = new Forms9Patch.Image
                    {
                        Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button"),
                    },
                    IconImage = new Forms9Patch.Image
                    {
                        Source = ImageSource.FromFile("five.png"),
                    },
                    TextColor = Color.White,
                    Text = "Toggle w/ SelectedState",
                },
                SelectedState = new Forms9Patch.ButtonState
                {
                    BackgroundImage = new Forms9Patch.Image
                    {
                        Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image"),
                    },
                    TextColor = Color.Red,
                    Text = "Selected",
                },
                ToggleBehavior = true,
                HeightRequest = 50,
                HorizontalTextAlignment = TextAlignment.Start,
            };
            b2.Tapped += OnImageButtonTapped;
            b2.Selected += OnImageButtonSelected;
            b2.LongPressing += OnImageButtonLongPressing;
            b2.LongPressed += OnImageButtonLongPressed;


            var b3 = new Forms9Patch.StateButton
            {
                DefaultState = new Forms9Patch.ButtonState
                {
                    BackgroundImage = new Forms9Patch.Image
                    {
                        Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button"),
                    },
                    IconImage = new Forms9Patch.Image
                    {
                        Source = ImageSource.FromFile("five.png"),
                    },
                    TextColor = Color.FromRgb(0.0, 0.0, 0.8),
                    Text = "Toggle w/o SelectedState",
                },
                PressingState = new Forms9Patch.ButtonState
                {
                    BackgroundImage = new Forms9Patch.Image
                    {
                        Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redButton"),
                    },
                },
                ToggleBehavior = true,
                HeightRequest = 50,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            b3.Tapped += OnImageButtonTapped;
            b3.Selected += OnImageButtonSelected;
            b3.LongPressing += OnImageButtonLongPressing;
            b3.LongPressed += OnImageButtonLongPressed;

            var b4 = new Forms9Patch.StateButton
            {
                DefaultState = new Forms9Patch.ButtonState
                {
                    BackgroundImage = new Forms9Patch.Image
                    {
                        Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button"),
                    },
                    IconImage = new Forms9Patch.Image
                    {
                        Source = ImageSource.FromFile("five.png"),
                    },
                    TextColor = Color.White,
                    Text = "Not toggle",
                },
                //ToggleBehavior = true,
                HeightRequest = 50,
                HorizontalTextAlignment = TextAlignment.End,
            };
            b4.Tapped += OnImageButtonTapped;
            b4.Selected += OnImageButtonSelected;
            b4.LongPressing += OnImageButtonLongPressing;
            b4.LongPressed += OnImageButtonLongPressed;

            #endregion


            var textEntry = new Entry { Text = "Text Entry Text" };

            b2.DefaultState.SetBinding(Forms9Patch.ButtonState.TextProperty, "Text");
            b2.DefaultState.BindingContext = textEntry;

            Padding = 10;

            Content = new StackLayout
            {
                Children = {
                    textEntry,
                    new Label { Text = "Buttons Page" },
                    b2,b3,b4,

                }
            };
        }
    }
}


