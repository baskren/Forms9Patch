using System;
using Xamarin.Forms;
using static Android.Views.MotionEvent;

namespace FormsGestures
{
    public static class DIP
    {
        public static readonly float Density = Droid.Settings.Context.Resources.DisplayMetrics.Density;

        public static Point ToPoint(double dipX, double dipY)
        {
            return new Point(dipX / (double)DIP.Density, dipY / (double)DIP.Density);
        }

        public static Point ToPoint(PointerCoords pointerCoords)
            => ToPoint(pointerCoords.X, pointerCoords.Y);

        public static Rectangle ToRectangle(double dipX, double dipY, double dipWidth, double dipHeight)
        {
            return new Rectangle(dipX / (double)DIP.Density, dipY / (double)DIP.Density, dipWidth / (double)DIP.Density, dipHeight / (double)DIP.Density);
        }
    }
}
