using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace FormsGestures.UWP
{
    class UwpPanEventArgs : PanEventArgs
    {
        public UwpPanEventArgs(FrameworkElement element, ManipulationDeltaRoutedEventArgs args)
        {
            ElementPosition = element.GetXfViewFrame();
            ElementTouches = new Xamarin.Forms.Point[] { args.Position.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(args.Position).ToXfPoint() };
            DeltaDistance = args.Delta.Translation.ToXfPoint();
            TotalDistance = args.Cumulative.Translation.ToXfPoint();
            Velocity = args.Velocities.Linear.ToXfPoint();
        }

        public UwpPanEventArgs(FrameworkElement element, ManipulationCompletedRoutedEventArgs args)
        {
            ElementPosition = element.GetXfViewFrame();
            ElementTouches = new Xamarin.Forms.Point[] { args.Position.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(args.Position).ToXfPoint() };
            TotalDistance = args.Cumulative.Translation.ToXfPoint();
            Velocity = args.Velocities.Linear.ToXfPoint();
        }

    }
}
