using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace FormsGestures.UWP
{
    public static class FrameworkElementExtensions
    {
        public static Xamarin.Forms.Rectangle GetXfViewFrame(this FrameworkElement element)
        {
            var transform = element.TransformToVisual(null);
            var point = transform.TransformPoint(new Windows.Foundation.Point(0, 0));
            //var xfPoint = new Xamarin.Forms.Point(point.X / Display.Scale, point.Y / Display.Scale);
            var xfPoint = point.ToXfPoint();
            var xfSize = new Xamarin.Forms.Size(element.ActualWidth / Display.Scale, element.ActualHeight / Display.Scale);
            return new Xamarin.Forms.Rectangle(xfPoint, xfSize);
        }
    }
}
