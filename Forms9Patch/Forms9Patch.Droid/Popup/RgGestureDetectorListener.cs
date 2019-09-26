using System;
using Android.Views;

namespace Forms9Patch.Droid
{
    internal class RgGestureDetectorListener : GestureDetector.SimpleOnGestureListener
    {
        public event EventHandler<MotionEvent> Clicked;

        public override bool OnSingleTapUp(MotionEvent e)
        {
            Clicked?.Invoke(this, e);

            return false;
        }
    }
}