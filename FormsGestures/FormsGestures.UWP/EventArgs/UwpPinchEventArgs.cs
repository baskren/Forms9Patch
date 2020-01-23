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
            ElementPosition = element.GetXfViewFrame();
            var point = args.Position;
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            DeltaScale = args.Delta.Scale;
            TotalScale = args.Cumulative.Scale;
        }

        public UwpPinchEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs args)
        {
            ElementPosition = element.GetXfViewFrame();
            var point = args.Position;
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            TotalScale = args.Cumulative.Scale;
        }
    }
}
