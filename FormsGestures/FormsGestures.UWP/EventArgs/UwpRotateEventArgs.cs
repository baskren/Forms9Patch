using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsGestures.UWP
{
    public class UwpRotateEventArgs : RotateEventArgs
    {
        public UwpRotateEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs args)
        {
            ElementPosition = element.GetXfViewFrame();
            var point = args.Position;
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            Angle = 0;
            DeltaAngle = args.Delta.Rotation;
            TotalAngle = args.Cumulative.Rotation;
        }

        public UwpRotateEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs args)
        {
            ElementPosition = element.GetXfViewFrame();
            var point = args.Position;
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            Angle = 0;
            TotalAngle = args.Cumulative.Rotation;
        }
    }
}
