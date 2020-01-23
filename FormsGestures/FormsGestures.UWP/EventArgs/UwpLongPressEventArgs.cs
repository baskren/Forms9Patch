using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace FormsGestures.UWP
{
    public class UwpLongPressEventArgs : LongPressEventArgs
    {
        public UwpLongPressEventArgs(FrameworkElement element, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs args, long elapsedMilliseconds)
        {
            ElementPosition = element.GetXfViewFrame();
            var point = args.Position;
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            Duration = elapsedMilliseconds;
        }

        public UwpLongPressEventArgs(FrameworkElement element, Windows.UI.Xaml.Input.TappedRoutedEventArgs args, long elapsedMilliseconds)
        {
            ElementPosition = element.GetXfViewFrame();
            var point = args.GetPosition(element);
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            Duration = elapsedMilliseconds;
        }

        public static bool FireLongPressed(FrameworkElement element, Windows.UI.Xaml.Input.TappedRoutedEventArgs e, long elapsedMilliseconds, Listener listener)
        {
            var args = new UwpLongPressEventArgs(element, e, elapsedMilliseconds)
            {
                Listener = listener
            };
            listener?.OnLongPressed(args);
            e.Handled = args.Handled;
            return e.Handled;
        }

        //PointerRoutedEventArgs
        public UwpLongPressEventArgs(FrameworkElement element, Windows.UI.Xaml.Input.PointerRoutedEventArgs args, long elapsedMilliseconds)
        {
            ElementPosition = element.GetXfViewFrame();
            var point = args.GetCurrentPoint(element).Position;
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            Duration = elapsedMilliseconds;
        }

        public static bool FireLongPressed(FrameworkElement element, Windows.UI.Xaml.Input.PointerRoutedEventArgs e, long elapsedMilliseconds, Listener listener)
        {
            var args = new UwpLongPressEventArgs(element, e, elapsedMilliseconds)
            {
                Listener = listener
            };
            listener?.OnLongPressed(args);
            e.Handled = args.Handled;
            return e.Handled;
        }

        public UwpLongPressEventArgs(FrameworkElement element, long elapsedMilliseconds)
        {
            ElementPosition = element.GetXfViewFrame();

            var point = element.GetCurrentPointerPosition();
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            Duration = elapsedMilliseconds;
        }

        public static bool FireLongPressed(FrameworkElement element, long elapsedMilliseconds, Listener listener)
        {
            var args = new UwpLongPressEventArgs(element, elapsedMilliseconds)
            {
                Listener = listener
            };
            listener?.OnLongPressed(args);
            return args.Handled;
        }
    }
}
