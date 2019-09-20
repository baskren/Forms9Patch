using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace leakquest
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        Button button = new Button
        {
            Text = "Show page 1"
        };

        public MainPage()
        {
            //InitializeComponent();
            Content = button;

            button.Clicked += OnButtonClicked;
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new Page1());
            var popup = new Popup();
            popup.IsVisible = true;
            popup.Cancelled += Popup_Cancelled;
            activated = true;
        }

        private void Popup_Cancelled(object sender, Forms9Patch.PopupPoppedEventArgs e)
        {
            GC.Collect();
            System.Diagnostics.Debug.WriteLine("MainPage.Popup_Cancelled : " + (appearances++) + " : " + GC.GetTotalMemory(true).ToString("n"));

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (activated)
                {
                    var popup = new Popup();
                    popup.IsVisible = true;
                    popup.Cancelled += Popup_Cancelled;
                }
                return false;
            });
        }

        bool activated;
        int appearances;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GC.Collect();
            System.Diagnostics.Debug.WriteLine("MainPage.OnAppearing : " + (appearances++) + " : " + GC.GetTotalMemory(true).ToString("n"));
            /*
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (activated)
                Navigation.PushAsync(new Page1());
                return false;
            });
            */
        }
    }
}
