using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace FormsGestures.UWP
{
    class UwpTapEventArgs : TapEventArgs
    {
        public UwpTapEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.TappedRoutedEventArgs args, int numberOfTaps)
        {
            ViewPosition = element.GetXfViewFrame();
            //Touches = new Xamarin.Forms.Point[] { args.GetPosition(null).ToXfPoint() };
            Touches = new Xamarin.Forms.Point[] { args.GetPosition(element).ToXfPoint() };
            NumberOfTaps = numberOfTaps;
        }

        public static bool FireTapped(FrameworkElement element, TappedRoutedEventArgs e, int numberOfTaps, Listener listener)
        {
            var args = new UwpTapEventArgs(element, e, numberOfTaps);
            args.Listener = listener;
            listener.OnTapped(args);
            e.Handled = args.Handled;
            return e.Handled;
        }


        //PointerRoutedEventArgs
        public UwpTapEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.PointerRoutedEventArgs args, int numberOfTaps)
        {
            ViewPosition = element.GetXfViewFrame();
            //Touches = new Xamarin.Forms.Point[] { args.GetCurrentPoint(null).Position.ToXfPoint() };
            Touches = new Xamarin.Forms.Point[] { args.GetCurrentPoint(element).Position.ToXfPoint() };
            NumberOfTaps = numberOfTaps;
        }

        //DoubleTappedRoutedEventArgs
        public UwpTapEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs args, int numberOfTaps)
        {
            ViewPosition = element.GetXfViewFrame();
            //Touches = new Xamarin.Forms.Point[] { args.GetPosition(null).ToXfPoint() };
            Touches = new Xamarin.Forms.Point[] { args.GetPosition(element).ToXfPoint() };
            NumberOfTaps = numberOfTaps;
        }

        public static bool FireDoubleTapped(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e, int numberOfTaps, Listener listener)
        {
            var args = new UwpTapEventArgs(element, e, numberOfTaps);
            args.Listener = listener;
            listener.OnDoubleTapped(args);
            e.Handled = args.Handled;
            return e.Handled;
        }
    }
}
