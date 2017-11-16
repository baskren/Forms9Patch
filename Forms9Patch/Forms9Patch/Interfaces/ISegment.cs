using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    public interface ISegment 
    {

        Forms9Patch.Image IconImage { get; set; }

        string IconText { get; set; }

        string Text { get; set; }

        string HtmlText { get; set; }

        Xamarin.Forms.Color TextColor { get; set; }

        Xamarin.Forms.FontAttributes FontAttributes { get; set; }

        bool IsEnabled { get; set; }

        bool IsSelected { get; set; }

        Xamarin.Forms.StackOrientation Orientation { get; set; }

        System.Windows.Input.ICommand Command { get; set; }

        object CommandParameter { get; set; }

        Xamarin.Forms.VisualElement VisualElement { get; }



    }

}
