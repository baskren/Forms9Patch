using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace FormsGestures.UWP
{
    class UwpDownUpArgs : DownUpEventArgs
    {
        public UwpDownUpArgs(FrameworkElement element, PointerRoutedEventArgs args)
        {
            Cancelled = false;
            ElementPosition = element.GetXfViewFrame();
            var currentPoint = args.GetCurrentPoint(element);
            var point = currentPoint.Position;
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            TriggeringTouches = new[] { 0 };
        }

        public static bool FireDown(FrameworkElement frameworkElement, PointerRoutedEventArgs e, Listener listener)
        {
            if (frameworkElement is FrameworkElement element)
            {
                var args = new UwpDownUpArgs(element, e)
                {
                    Listener = listener
                };
                listener.OnDown(args);
                e.Handled = args.Handled;
                return e.Handled;
            }
            return false;
        }

        public static bool FireUp(FrameworkElement frameworkElement, PointerRoutedEventArgs e, Listener listener)
        {
            if (frameworkElement is FrameworkElement element)
            {
                var args = new UwpDownUpArgs(element, e)
                {
                    Listener = listener
                };
                listener.OnUp(args);
                e.Handled = args.Handled;
                return e.Handled;
            }
            return false;
        }

        public UwpDownUpArgs(FrameworkElement element, TappedRoutedEventArgs args)
        {
            Cancelled = false;
            ElementPosition = element.GetXfViewFrame();
            var point = args.GetPosition(element);
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            TriggeringTouches = new[] { 0 };
        }

        public static bool FireDown(FrameworkElement frameworkElement, TappedRoutedEventArgs e, Listener listener)
        {
            if (frameworkElement is FrameworkElement element)
            {
                var args = new UwpDownUpArgs(element, e)
                {
                    Listener = listener
                };
                listener.OnDown(args);
                e.Handled = args.Handled;
                return e.Handled;
            }
            return false;
        }

        public static bool FireUp(FrameworkElement frameworkElement, TappedRoutedEventArgs e, Listener listener)
        {
            if (frameworkElement is FrameworkElement element)
            {
                var args = new UwpDownUpArgs(element, e)
                {
                    Listener = listener
                };
                listener.OnUp(args);
                e.Handled = args.Handled;
                return e.Handled;
            }
            return false;
        }



    }
}
