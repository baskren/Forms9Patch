using System;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class FlyoutGestureFail : ContentPage
    {
        public FlyoutGestureFail()
        {
            var button = new Forms9Patch.Button
            {
                Text = "Present Flyout",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.White,
                TextColor = Color.Black
            };

            button.Clicked += Button_Clicked;

            BackgroundColor = Color.LightGray;
            Content = button;
        }

        async void Button_Clicked(object sender, EventArgs e)
        {

            // Wrapping the popup in a using will cause the popup to be disposed when the FlyoutPopup is out of scope
            using (var popup = new Forms9Patch.FlyoutPopup
            {
                BackgroundColor = Color.LightPink
            })
            {
                var content = new Xamarin.Forms.Label
                {
                    Text = "Close",
                    Padding = 20,
                    BackgroundColor = Color.Gray,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    /*
                    GestureRecognizers = { new TapGestureRecognizer
                    {
                        Command = new Command(() => popup.IsVisible = false)
                    }}
                    */
                };



                popup.Content = content;
                popup.IsVisible = true;

                // just like the popup, listeners should be disposed.  A using sure helps make this easy!
                using (var listener = FormsGestures.Listener.For(content))
                {
                    listener.Tapped += (s, a) =>
                    {
                        popup.IsVisible = false;
                    };

                    // this prevents the popup form leaving this scope until it is popped (set invisible);
                    await popup.WaitForPoppedAsync();
                }
            }
        }
    }
}