using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    internal interface ILabel : IFontElement //TODO:, IElement
    {
        string Text { get; set; }
        string HtmlText { get; set; }

        Color TextColor { get; set; }

        TextAlignment HorizontalTextAlignment { get; set; }
        TextAlignment VerticalTextAlignment { get; set; }

        LineBreakMode LineBreakMode { get; set; }
        LabelFit Fit { get; set; }
        int Lines { get; set; }
        double MinFontSize { get; set; }
    
    }
}
