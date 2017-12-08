using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    public interface ILabelStyle : IFontElement
    {
        Color TextColor { get; set; }

        TextAlignment HorizontalTextAlignment { get; set; }
        TextAlignment VerticalTextAlignment { get; set; }

        LineBreakMode LineBreakMode { get; set; }
        AutoFit AutoFit { get; set; }
        int Lines { get; set; }
        double MinFontSize { get; set; }

    }
}
