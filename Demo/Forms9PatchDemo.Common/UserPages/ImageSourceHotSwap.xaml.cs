using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public partial class ImageSourceHotSwap : ContentPage
    {
        public ImageSourceHotSwap()
        {
            InitializeComponent();

            _uriImageSource1.PropertyChanged += OnUriPropertyChanged;
            //_uriImageSource2.PropertyChanged += OnUriPropertyChanged;

        }

        private void OnUriPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("PropertyName: " + e.PropertyName);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            BindingContext = BindingContext == null ? "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png" : null;
        }
    }
}
