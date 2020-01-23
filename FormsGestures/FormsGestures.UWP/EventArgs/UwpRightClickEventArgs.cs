using System;
using FormsGestures;


namespace FormsGestures.UWP
{
    class UwpRightClickEventArgs : RightClickEventArgs
    {
        public UwpRightClickEventArgs(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs args, FormsGestures.Listener listener) 
        {
            ElementPosition = element.GetXfViewFrame();
            var point = args.GetPosition(element);
            ElementTouches = new Xamarin.Forms.Point[] { point.ToXfPoint() };
            WindowTouches = new Xamarin.Forms.Point[] { element.PointInNativeAppWindowCoord(point).ToXfPoint() };
            Listener = listener;
        }

        public static bool Fire(Windows.UI.Xaml.FrameworkElement element, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs args, FormsGestures.Listener listener)
        {
            var f9pArgs = new UwpRightClickEventArgs(element, args, listener);
            listener.OnRightClicked(f9pArgs);
            args.Handled = f9pArgs.Handled;
            return args.Handled;
        }
    }
}
