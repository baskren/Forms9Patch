using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    public class SingleImageButtonCodePage : ContentPage, IDisposable
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


        #region VisualElements
        Forms9Patch.StateButton b2 = new Forms9Patch.StateButton
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

        Forms9Patch.SegmentedControl _iconSwitcher = new Forms9Patch.SegmentedControl
        {
            Segments =
            {
                new Forms9Patch.Segment
                {
                    IconText = "",
                },
                new Forms9Patch.Segment
                {
                    IconText = "@",
                },
                 new Forms9Patch.Segment
                {
                    IconText = "&amp;",
                },
                new Forms9Patch.Segment
                {
                    //ImageSource = ImageSource.FromFile("five.png"),
                    IconImage = new Forms9Patch.Image { Source = ImageSource.FromFile("five.png")}
                },
                new Forms9Patch.Segment
                {
                    //ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.ArrowR"),
                    IconImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.ArrowR") }
                },
                new Forms9Patch.Segment
                {
                    //ImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info"),
                    IconImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info") }
                },
            }
        };
        #endregion
        //Forms9PatchDemo.Resources.Info


        public SingleImageButtonCodePage()
        {
            Padding = 10;
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Buttons Page" },
                    b2

                }
            };

            _iconSwitcher.SegmentTapped += (sender, e) =>
            {
                if (e.Segment.IconText != null)
                    b2.DefaultState.IconText = e.Segment.IconText;
                else
                    b2.DefaultState.IconImage = new Forms9Patch.Image { Source = e.Segment.IconImage.Source };
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
                    b2.Dispose();
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


