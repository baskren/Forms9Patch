using System;
using F9PButtonTest.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace F9PButtonTest.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Buttons2 : ContentView
    {
        private readonly InfoMsg _infoMsg;

        public Buttons2()
        {
            InitializeComponent();

            _infoMsg = MainPage.IMsg;
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            string btnText = (sender.GetType().Namespace == "Xamarin.Forms") ? (sender as Button).Text : (sender as Forms9Patch.Button).Text;

            _infoMsg.MsgText = btnText + " Clicked";
        }

        private void Btn_LongPressing(object sender, EventArgs e)
        {
            string btnText = (sender.GetType().Namespace == "Xamarin.Forms") ? (sender as Button).Text : (sender as Forms9Patch.Button).Text;

            _infoMsg.MsgText = btnText + " LongPressing";
        }

        private void Btn_LongPressed(object sender, EventArgs e)
        {
            string btnText = (sender.GetType().Namespace == "Xamarin.Forms") ? (sender as Button).Text : (sender as Forms9Patch.Button).Text;

            _infoMsg.MsgText = btnText + " LongPressed";
        }
    }
}