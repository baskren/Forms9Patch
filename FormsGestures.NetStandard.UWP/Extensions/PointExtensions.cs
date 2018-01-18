using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsGestures.UWP
{
    public static class PointExtensions
    {
        public static Xamarin.Forms.Point ToXfPoint(this Windows.Foundation.Point point) =>new Xamarin.Forms.Point(point.X , point.Y );

        public static Windows.Foundation.Point ToUwpPoint(this Xamarin.Forms.Point point) => new Windows.Foundation.Point(point.X, point.Y);
    }
}
