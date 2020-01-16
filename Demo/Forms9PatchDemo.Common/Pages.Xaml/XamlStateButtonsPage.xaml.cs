using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms9PatchDemo
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class XamlStateButtonsPage : ContentPage
    {
        public XamlStateButtonsPage()
        {
            InitializeComponent();

            Forms9Patch.KeyboardService.Activate();
            Forms9Patch.KeyboardService.HeightChanged += (sender, height) => System.Diagnostics.Debug.WriteLine("Keyboard Height=[" + height + "]");
        }
    }
}

