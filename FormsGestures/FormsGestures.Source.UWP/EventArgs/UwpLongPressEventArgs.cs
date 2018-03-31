using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //Touches = new Xamarin.Forms.Point[] { args.GetPosition(null).ToXfPoint() };
            Touches = new Xamarin.Forms.Point[] { args.GetPosition(element).ToXfPoint() };
            Duration = elapsedMilliseconds;
        }

        public static bool FireLongPressed(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.TappedRoutedEventArgs e, long elapsedMilliseconds, Listener listener)
        {
            var args = new UwpLongPressEventArgs(element, e, elapsedMilliseconds);
            args.Listener = listener;
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

    }
}
