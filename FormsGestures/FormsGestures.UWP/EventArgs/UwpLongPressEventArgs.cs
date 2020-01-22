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
        public UwpLongPressEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs args, long elapsedMilliseconds)
        {
            ViewPosition = element.GetXfViewFrame();
            Touches = new Xamarin.Forms.Point[] { args.Position.ToXfPoint() };
            Duration = elapsedMilliseconds;
        }

        public UwpLongPressEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.TappedRoutedEventArgs args, long elapsedMilliseconds)
        {
            ViewPosition = element.GetXfViewFrame();
            Touches = new Xamarin.Forms.Point[] { args.GetPosition(element).ToXfPoint() };
            Duration = elapsedMilliseconds;
        }

        public static bool FireLongPressed(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.TappedRoutedEventArgs e, long elapsedMilliseconds, Listener listener)
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
        public UwpLongPressEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.PointerRoutedEventArgs args, long elapsedMilliseconds)
        {
            ViewPosition = element.GetXfViewFrame();
            //Touches = new Xamarin.Forms.Point[] { args.GetCurrentPoint(null).Position.ToXfPoint() };
            Touches = new Xamarin.Forms.Point[] { args.GetCurrentPoint(element).Position.ToXfPoint() };
            Duration = elapsedMilliseconds;
        }

        public static bool FireLongPressed(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.PointerRoutedEventArgs e, long elapsedMilliseconds, Listener listener)
        {
            var args = new UwpLongPressEventArgs(element, e, elapsedMilliseconds)
            {
                Listener = listener
            };
            listener?.OnLongPressed(args);
            e.Handled = args.Handled;
            return e.Handled;
        }

        public UwpLongPressEventArgs(Windows.UI.Xaml.FrameworkElement element, long elapsedMilliseconds)
        {
            ViewPosition = element.GetXfViewFrame();
            var pointerPositionInScreenCoord = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;
            var pointerPositionInAppWindowX = pointerPositionInScreenCoord.X - Window.Current.Bounds.X;
            var pointerPositionInAppWindowY = pointerPositionInScreenCoord.Y - Window.Current.Bounds.Y;
            var transform = element.TransformToVisual(Window.Current.Content);
            var elementOrigin = transform.TransformPoint(Xamarin.Forms.Point.Zero.ToUwpPoint());
            var pointerPositionInElementCoordX = pointerPositionInAppWindowX - elementOrigin.X;
            var pointerPositionInElementCoordY = pointerPositionInAppWindowY - elementOrigin.Y;
            Touches = new Xamarin.Forms.Point[] { new Xamarin.Forms.Point(pointerPositionInElementCoordX, pointerPositionInElementCoordY) };
            Duration = elapsedMilliseconds;
        }

        public static bool FireLongPressed(Windows.UI.Xaml.FrameworkElement element, long elapsedMilliseconds, Listener listener)
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
