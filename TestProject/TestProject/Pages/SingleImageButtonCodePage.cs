using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    public class SingleImageButtonCodePage : ContentPage, IDisposable
    {
        static void OnImageButtonTapped(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Tapped Button Text=[" + ((Forms9Patch.ImageButton)sender).Text + "]");
        }

        static void OnImageButtonSelected(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Selected Button Text=[" + ((Forms9Patch.ImageButton)sender).Text + "]");
        }

        static void OnImageButtonLongPressing(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressing Button Text=[" + ((Forms9Patch.ImageButton)sender).Text + "]");
        }

        static void OnImageButtonLongPressed(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressed Button Text=[" + ((Forms9Patch.ImageButton)sender).Text + "]");
        }


        #region ImageButtons
        Forms9Patch.ImageButton b2 = new Forms9Patch.ImageButton
        {
            DefaultState = new Forms9Patch.ImageButtonState
            {
                BackgroundImage = new Forms9Patch.Image
                {
                    Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button"),
                },
                Image = new Forms9Patch.Image
                {
                    Source = ImageSource.FromFile("five.png"),
                },
                FontColor = Color.White,
                Text = "Sticky w/ SelectedState",
            },
            SelectedState = new Forms9Patch.ImageButtonState
            {
                BackgroundImage = new Forms9Patch.Image
                {
                    Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image"),
                },
                FontColor = Color.Red,
                Text = "Selected",
            },
            ToggleBehavior = true,
            HeightRequest = 50,
            HorizontalTextAlignment = TextAlignment.Start,
        };


        Forms9Patch.ImageButton b3 = new Forms9Patch.ImageButton
        {
            DefaultState = new Forms9Patch.ImageButtonState
            {
                BackgroundImage = new Forms9Patch.Image
                {
                    Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button"),
                },
                Image = new Forms9Patch.Image
                {
                    Source = ImageSource.FromFile("five.png"),
                },
                FontColor = Color.FromRgb(0.0, 0.0, 0.8),
                Text = "Sticky w/o SelectedState",
            },
            PressingState = new Forms9Patch.ImageButtonState
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

        Forms9Patch.ImageButton b4 = new Forms9Patch.ImageButton
        {
            DefaultState = new Forms9Patch.ImageButtonState
            {
                BackgroundImage = new Forms9Patch.Image
                {
                    Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button"),
                },
                Image = new Forms9Patch.Image
                {
                    Source = ImageSource.FromFile("five.png"),
                },
                FontColor = Color.White,
                Text = "Not toggle",
            },
            //ToggleBehavior = true,
            HeightRequest = 50,
            HorizontalTextAlignment = TextAlignment.End,
        };

        #endregion


        public SingleImageButtonCodePage()
        {
            Padding = 10;
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Buttons Page" },
                    b2,b3,b4,

                }
            };
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    b2.Tapped -= OnImageButtonTapped;
                    b2.Selected -= OnImageButtonSelected;
                    b2.LongPressing -= OnImageButtonLongPressing;
                    b2.LongPressed -= OnImageButtonLongPressed;

                    b3.Tapped -= OnImageButtonTapped;
                    b3.Selected -= OnImageButtonSelected;
                    b3.LongPressing -= OnImageButtonLongPressing;
                    b3.LongPressed -= OnImageButtonLongPressed;

                    b4.Tapped -= OnImageButtonTapped;
                    b4.Selected -= OnImageButtonSelected;
                    b4.LongPressing -= OnImageButtonLongPressing;
                    b4.LongPressed -= OnImageButtonLongPressed;

                    b2.Dispose();
                    b3.Dispose();
                    b4.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SingleImageButtonCodePage() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Dispose();
        }
    }
}


