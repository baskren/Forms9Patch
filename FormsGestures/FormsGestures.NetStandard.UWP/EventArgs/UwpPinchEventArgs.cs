using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsGestures.UWP
{
    class UwpPinchEventArgs : PinchEventArgs
    {
        public UwpPinchEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs args)
        {
            ViewPosition = element.GetXfViewFrame();
            Touches = new Xamarin.Forms.Point[] { args.Position.ToXfPoint() };
            DeltaScale = args.Delta.Scale;
            TotalScale = args.Cumulative.Scale;
        }

        public UwpPinchEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs args)
        {
            ViewPosition = element.GetXfViewFrame();
            Touches = new Xamarin.Forms.Point[] { args.Position.ToXfPoint() };
            TotalScale = args.Cumulative.Scale;
        }
    }
}
