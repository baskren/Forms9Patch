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
            ElementPosition = element.GetXfViewFrame();
            var point = args.GetPosition(element);
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            NumberOfTaps = numberOfTaps;
        }

        public static bool FireTapped(FrameworkElement element, TappedRoutedEventArgs e, int numberOfTaps, Listener listener)
        {
            var args = new UwpTapEventArgs(element, e, numberOfTaps)
            {
                Listener = listener
            };
            listener.OnTapped(args);
            e.Handled = args.Handled;
            return e.Handled;
        }


        //PointerRoutedEventArgs
        public UwpTapEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.PointerRoutedEventArgs args, int numberOfTaps)
        {
            ElementPosition = element.GetXfViewFrame();
            //ElementTouches = new Xamarin.Forms.Point[] { args.GetCurrentPoint(null).Position.ToXfPoint() };
            var point = args.GetCurrentPoint(element).Position;
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            NumberOfTaps = numberOfTaps;
        }

        public static bool FireTapped(FrameworkElement element, PointerRoutedEventArgs e, int numberOfTaps, Listener listener)
        {
            var args = new UwpTapEventArgs(element, e, numberOfTaps)
            {
                Listener = listener
            };
            listener.OnTapped(args);
            e.Handled = args.Handled;
            return e.Handled;
        }

        public static bool FireTapping(FrameworkElement element, PointerRoutedEventArgs e, int numberOfTaps, Listener listener)
        {
            var args = new UwpTapEventArgs(element, e, numberOfTaps)
            {
                Listener = listener
            };
            listener.OnTapping(args);
            e.Handled = args.Handled;
            return e.Handled;
        }
        public static bool FireDoubleTapped(FrameworkElement element, PointerRoutedEventArgs e, int numberOfTaps, Listener listener)
        {
            var args = new UwpTapEventArgs(element, e, numberOfTaps)
            {
                Listener = listener
            };
            listener.OnDoubleTapped(args);
            e.Handled = args.Handled;
            return e.Handled;
        }



        //DoubleTappedRoutedEventArgs
        public UwpTapEventArgs(FrameworkElement element, DoubleTappedRoutedEventArgs args, int numberOfTaps)
        {
            ElementPosition = element.GetXfViewFrame();
            var point = args.GetPosition(element);
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            NumberOfTaps = numberOfTaps;
        }

        public static bool FireDoubleTapped(FrameworkElement element, DoubleTappedRoutedEventArgs e, int numberOfTaps, Listener listener)
        {
            var args = new UwpTapEventArgs(element, e, numberOfTaps)
            {
                Listener = listener
            };
            listener.OnDoubleTapped(args);
            e.Handled = args.Handled;
            return e.Handled;
        }

        public UwpTapEventArgs(FrameworkElement element, int numberOfTaps)
        {
            ElementPosition = element.GetXfViewFrame();
            var point = element.GetCurrentPointerPosition();
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            NumberOfTaps = numberOfTaps;
        }

        public static bool FireTapped(FrameworkElement element, int numberOfTaps, Listener listener)
        {
            var args = new UwpTapEventArgs(element, numberOfTaps)
            {
                Listener = listener
            };
            listener.OnTapped(args);
            return args.Handled;
        }

    }
}
