using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms9PatchDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new HomePage());
            /*
            P42.Utils.Debug.IsMessagesEnabled = true;
            P42.Utils.Debug.ConditionFunc = (object obj) =>
            {
                if (obj is Forms9Patch.Label label)
                    return (label.HtmlText ?? label.Text) == "Segment A";

                if (obj is Forms9Patch.Button button)
                    return (button.HtmlText ?? button.Text) == "Segment A";
                if (obj is Forms9Patch.Segment segment)
                    return segment.Text == "Segment A";
                return false;
            };
            MainPage = new Pages.Code.PopupsPage();
            */
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
