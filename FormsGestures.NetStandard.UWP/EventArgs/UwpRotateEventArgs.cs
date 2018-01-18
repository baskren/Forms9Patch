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
            ViewPosition = element.GetXfViewFrame();
            Touches = new Xamarin.Forms.Point[] { args.Position.ToXfPoint() };
            Angle = 0;
            DeltaAngle = args.Delta.Rotation;
            TotalAngle = args.Cumulative.Rotation;
        }

        public UwpRotateEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs args)
        {
            ViewPosition = element.GetXfViewFrame();
            Touches = new Xamarin.Forms.Point[] { args.Position.ToXfPoint() };
            Angle = 0;
            TotalAngle = args.Cumulative.Rotation;
        }
    }
}
