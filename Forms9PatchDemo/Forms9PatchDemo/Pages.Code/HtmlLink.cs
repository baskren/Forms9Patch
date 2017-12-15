using System;
using Xamarin.Forms;
namespace Forms9PatchDemo
{
    public class HtmlLink : ContentPage
    {
        public HtmlLink()
        {
            var label = new Forms9Patch.Label
            {
                HtmlText = "Contact us on <a id=\"phone\" href=\"tel:+353015546889\">015546889</a> or <a id=\"email\" href=\"mailto:email@hotmail.com\">email@hotmail.com</a>",
                HorizontalTextAlignment = TextAlignment.Center
            };
            label.ActionTagTapped += (object sender, Forms9Patch.ActionTagEventArgs e) =>
            {
                var id = e.Id;
                var href = e.Href;
                var uri = new Uri(e.Href);
                Device.OpenUri(uri);
            };
            var stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    new Forms9Patch.Label("Forms9Patch.Label.HtmlText &lt;a&gt; example.") { HorizontalTextAlignment = TextAlignment.Center },
                    new BoxView { BackgroundColor = Color.Black, HeightRequest = 1 },
                    label,
                    new BoxView { BackgroundColor = Color.Black, HeightRequest = 1 },
                }
            };

            if (Device.RuntimePlatform == Device.iOS)
                stack.Children.Add(new Forms9Patch.Label("<b><i>NOTE:</i></b> Does not work in iOS Simulator.") { HorizontalTextAlignment = TextAlignment.Center });

            Content = stack;
        }
    }
}
