using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms9PatchDemo
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public partial class XamlSingleStateButtonPage : ContentPage
    {
        public XamlSingleStateButtonPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            BindingContext = null;
            Content = null;
            base.OnDisappearing();
            GC.Collect();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            label.Text = "Scale=" + Forms9Patch.Display.Scale;
        }
    }
}

