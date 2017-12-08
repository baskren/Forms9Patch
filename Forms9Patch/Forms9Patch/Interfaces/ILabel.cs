using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    public interface ILabel : ILabelStyle, IElement
    {
        string Text { get; set; }
        string HtmlText { get; set; }

        double OptimalFontSize { get; }
        double SynchronizedFontSize { get; set; }
    }
}
