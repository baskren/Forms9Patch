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
                var text = "In the end, it's not the years in your life that count. It's the life in your years.";
                if (obj is Forms9Patch.Label label)
                    return (label.HtmlText ?? label.Text) == text;
                if (obj is Xamarin.Forms.Label xfLabel1)
                    return xfLabel1.Text == text;
                
                if (obj is Forms9Patch.Button button)
                    return (button.HtmlText ?? button.Text) == text;
                if (obj is Forms9Patch.Segment segment)
                    return segment.Text == text;
                    

                if (obj is Forms9Patch.Frame frame && frame.Content is Xamarin.Forms.Label xflabel)
                    return xflabel.Text == text;
                return false;
            };
            MainPage = new ChatListPage();
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
