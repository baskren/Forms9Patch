using System;
using Xamarin.Forms;
namespace Forms9PatchDemo
{
    public class LabelLink : ContentPage
    {
        public LabelLink()
        {
            var label = new Forms9Patch.Label
            {
                HtmlText = "Contact us on <a id=\"phone\" href=\"tel:+353015546889\">015546889</a> or <a id=\"email\" href=\"mailto:email@hotmail.com\">email@hotmail.com</a>"
            };
            label.ActionTagTapped += (object sender, Forms9Patch.ActionTagEventArgs e) =>
            {
                var id = e.Id;
                var href = e.Href;
                var uri = new Uri(e.Href);
                Device.OpenUri(uri);
            };
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    new Label { Text = "Forms9Patch.Label.HtmlText <a> example" },
                    new BoxView { BackgroundColor = Color.Black, HeightRequest = 1 },
                    label
                }
            };
        }
    }
}
