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
            ViewPosition = element.GetXfViewFrame();
            Touches = new Xamarin.Forms.Point[] { args.Position.ToXfPoint() };
            DeltaDistance = args.Delta.Translation.ToXfPoint();
            TotalDistance = args.Cumulative.Translation.ToXfPoint();
            Velocity = args.Velocities.Linear.ToXfPoint();
        }

        public UwpPanEventArgs(FrameworkElement element, ManipulationCompletedRoutedEventArgs args)
        {
            ViewPosition = element.GetXfViewFrame();
            Touches = new Xamarin.Forms.Point[] { args.Position.ToXfPoint() };
            TotalDistance = args.Cumulative.Translation.ToXfPoint();
            Velocity = args.Velocities.Linear.ToXfPoint();
        }

    }
}
