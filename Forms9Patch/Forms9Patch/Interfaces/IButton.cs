using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Forms9Patch
{
    internal interface IButton :  Xamarin.Forms.IButtonController, IButtonState
    {
        Xamarin.Forms.Color SelectedTextColor { get; set; }

        Xamarin.Forms.Color SelectedBackgroundColor { get; set; }


        ICommand Command { get; set; }

        object CommandParameter { get; set; }


        bool ToggleBehavior { get; set; }

        bool IsEnabled { get; set; }

        bool IsSelected { get; set; }


        HapticEffect HapticEffect { get; set; }

        KeyClicks HapticMode { get; set; }




    }
}
