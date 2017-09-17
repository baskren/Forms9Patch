using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace Forms9Patch.UWP
{
    public class LayoutRenderer<TElement> : ViewRenderer<TElement, FrameworkElement> where TElement : View //, IBackgroundImage
    {
    }
}
