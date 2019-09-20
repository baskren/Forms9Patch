using System;
using Xamarin.Forms;

namespace leakquest
{
    public class Popup : Forms9Patch.ModalPopup
    {
        /*
        Forms9Patch.Label label = new Forms9Patch.Label
        {
            HtmlText = "popup <b>HTML</b>"
        };
        */

        /*
        Forms9Patch.Image image = new Forms9Patch.Image()
        {
            //Source = Forms9Patch.ImageSource.FromResource("leakquest.Resources.balloons.jpg"),
            //Source = Forms9Patch.ImageSource.FromResource("leakquest.Resources.HW_US_20_(IA).svg"),
            Source = Forms9Patch.ImageSource.FromResource("leakquest.Resources.kid1.jpeg"),
            //.BackgroundColor = Color.Red,
            //OutlineColor = Color.Green,
            //OutlineRadius = 5,
            //OutlineWidth = 1,
        };
        */

        //SkiaObject skiaObject = new SkiaObject();

        Forms9Patch.Button button = new Forms9Patch.Button
        {
            BackgroundImage = new Forms9Patch.Image("leakquest.Resources.ltblueUp.png"),
            IconImage = new Forms9Patch.Image("leakquest.Resources.tick.png"),
            Text = "Click Me!"
        };

        public Popup()
        {
            //Content = label;
            Content = button;
            button.Clicked += (s, e) => Console.WriteLine("message");
            //Content = skiaObject;
        }

        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                IsVisible = false;
                return false;
            });
        }
    }
}
