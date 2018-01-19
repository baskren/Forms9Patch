using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FormsGestures
{
    static class RectangleExtensions
    {
        public static Windows.Foundation.Rect ToWinRect(this Xamarin.Forms.Rectangle rectangle) => new Windows.Foundation.Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

        public static Xamarin.Forms.Rectangle ToXfRectangle(this Windows.Foundation.Rect rect) => new Xamarin.Forms.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
    }
}
