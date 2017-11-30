using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms9PatchDemo
{
    [ContentProperty("HtmlText")]
    class HtmlTextExtension : IMarkupExtension
    {
        public string HtmlText { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            System.Diagnostics.Debug.WriteLine("");
            return HtmlText;
        }
    }
}
