using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    public interface IButtonState : IBackground, ILabel
    {
        Forms9Patch.Image IconImage { get; set; }
        string IconText { get; set; }

        bool TrailingIcon { get; set; }
        bool TintIcon { get; set; }

        bool HasTightSpacing { get; set; }
        double Spacing { get; set; }

        Xamarin.Forms.StackOrientation Orientation { get; set; }
    }
}
