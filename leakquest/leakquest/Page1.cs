using System;
using Xamarin.Forms;

namespace leakquest
{
    public class Page1 : ContentPage, IDisposable
    {
        /*
        //Forms9Patch.Button button = new Forms9Patch.Button
        Xamarin.Forms.Button button = new Xamarin.Forms.Button
        {
            Text = "go back",
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
        };
        */

        Xamarin.Forms.Label label = new Xamarin.Forms.Label
        {
            Text = "Xamarin.Forms.Label"
        };

        FormsGestures.Listener listener;

        public Page1()
        {
            //button.Clicked += OnButtonClicked;
            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                }
            };

            //grid.Children.Add(new SkiaObject());
            //grid.Children.Add(button, 0, 1);
            //grid.Children.Add(new Forms9Patch.Label { Text = "pizza" }, 0, 2);
            grid.Children.Add(label, 0, 1);

            listener = FormsGestures.Listener.For(label);

            Content = grid;
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        #region IDisposable Support
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                if (disposing)
                {
                    //button.Clicked -= OnButtonClicked;
                    listener?.Dispose();
                    listener = null;
                }
            }
        }

        public void Dispose()
            => Dispose(true);
        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GC.Collect();
            //System.Diagnostics.Debug.WriteLine("Page1.OnAppearing: " + GC.GetTotalMemory(true).ToString("n"));


            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Navigation.PopAsync();
                // the following results in no leak!
                listener.Dispose();
                listener = null;
                return false;
            });
        }

    }
}
