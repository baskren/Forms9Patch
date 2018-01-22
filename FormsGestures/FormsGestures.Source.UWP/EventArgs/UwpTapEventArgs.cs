using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsGestures.UWP
{
    class UwpTapEventArgs : TapEventArgs
    {
        public UwpTapEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.TappedRoutedEventArgs args, int numberOfTaps)
        {
            ViewPosition = element.GetXfViewFrame();
            Touches = new Xamarin.Forms.Point[] { args.GetPosition(null).ToXfPoint() };
            NumberOfTaps = numberOfTaps;
        }

        //PointerRoutedEventArgs
        public UwpTapEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.PointerRoutedEventArgs args, int numberOfTaps)
        {
            ViewPosition = element.GetXfViewFrame();
            Touches = new Xamarin.Forms.Point[] { args.GetCurrentPoint(null).Position.ToXfPoint() };
            NumberOfTaps = numberOfTaps;
        }

        //DoubleTappedRoutedEventArgs
        public UwpTapEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs args, int numberOfTaps)
        {
            ViewPosition = element.GetXfViewFrame();
            Touches = new Xamarin.Forms.Point[] { args.GetPosition(null).ToXfPoint() };
            NumberOfTaps = numberOfTaps;
        }
    }
}
