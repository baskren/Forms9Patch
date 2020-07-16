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
    public partial class XamlCapsInsetPage : ContentPage
    {
        public XamlCapsInsetPage()
        {
            InitializeComponent();
        }
    }
}