using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace FormsGestures.UWP
{
    class UWPDownUpArgs : DownUpEventArgs
    {
        public UWPDownUpArgs(FrameworkElement element, PointerRoutedEventArgs args)
        {
            Cancelled = false;
            ViewPosition = element.GetXfViewFrame();
            var currentPoint = args.GetCurrentPoint(null);
            Touches = new Xamarin.Forms.Point[] { currentPoint.Position.ToXfPoint() };
            TriggeringTouches = new[] { 0 };
        }

        public UWPDownUpArgs(FrameworkElement element, TappedRoutedEventArgs args)
        {
            Cancelled = false;
            ViewPosition = element.GetXfViewFrame();
            var currentPoint = args.GetPosition(null);
            Touches = new Xamarin.Forms.Point[] { currentPoint.ToXfPoint() };
            TriggeringTouches = new[] { 0 };
        }
    }
}
