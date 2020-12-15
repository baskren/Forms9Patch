using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    public partial class LineHeight : ContentPage
    {
        public LineHeight()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            htmlWithSpace.HtmlText = "color: <div style=\"color: #7fb22d\">#7fb22d Text</div> plain";
            htmlWithoutSpace.HtmlText = "color: <div style=\"color:#7fb22d\">#7fb22d Text</div> plain";
        }
    }
}
