using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms9PatchDemo
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public partial class XamlCDATA : ContentPage
    {
        public XamlCDATA()
        {
            InitializeComponent();
            PhoneLabel.ActionTagTapped += ActionTagTapped;
            EmailLabel.ActionTagTapped += ActionTagTapped;
        }

        private void ActionTagTapped(object sender, Forms9Patch.ActionTagEventArgs e)
        {
            var href = e.Href;
            if (Device.RuntimePlatform == Device.UWP && href.StartsWith("tel:", StringComparison.Ordinal))
                href = href.Replace("tel:", "callto:");
            var uri = new Uri(href);
            Xamarin.Essentials.Launcher.OpenAsync(uri);
        }
    }
}