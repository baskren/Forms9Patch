// /*******************************************************************
//  *
//  * AndroidViewExtensions.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using Android.Graphics;
using Android.Views;
using Xamarin.Forms;

namespace FormsGestures.Droid
{
    public static class AndroidViewExtensions
    {
        public static bool TouchUpViewHeirarchy(this Android.Views.View view, MotionEvent e)
        {
            if (view == null)
                return true;
            if (!(view.Parent is Android.Views.View parent))
                return true;

            // If the number of pointers is the same and we don't need to perform any fancy
            // irreversible transformations, then we can reuse the motion event for this
            // dispatch as long as we are careful to revert any changes we make.
            // Otherwise we need to make a copy.
            //MotionEvent newEvent = MotionEvent.Obtain(e);
            float offsetX = parent.ScrollX - view.Left;
            float offsetY = parent.ScrollY - view.Top;
            e.OffsetLocation(-offsetX, -offsetY);

            var handled = parent.OnTouchEvent(MotionEvent.Obtain(e));
            //var handled = parent.DispatchTouchEvent(e);
            //System.Diagnostics.Debug.WriteLine("TouchUpViewHeirarchy: x=["+e.GetX()+"] y=["+e.GetY()+"] type=["+parent.GetType()+"] action=["+e.Action+"] handled=["+handled+"] ");
            if (!handled)
                handled = parent.TouchUpViewHeirarchy(e);
            e.OffsetLocation(offsetX, offsetY);
            return handled;
        }

        public static Android.Graphics.Point LocationInNativeCoord(this Android.Views.View view)
        {
            var p = new int[2];
            view.GetLocationOnScreen(p);
            return new Android.Graphics.Point(p[0], p[1]);
        }

        public static Xamarin.Forms.Point LocationInFormsCoord(this Android.Views.View view)
        {
            var location = LocationInNativeCoord(view);
            return new Xamarin.Forms.Point(location.X / Display.Scale, location.Y / Display.Scale);
        }


        public static Android.Graphics.Rect BoundsInNativeCoord(this Android.Views.View view)
        {
            var location = LocationInNativeCoord(view);
            return new Android.Graphics.Rect(location.X, location.Y, view.Width, view.Height);
        }

        public static Rectangle BoundsInFormsCoord(this Android.Views.View view)
        {
            var rect = BoundsInNativeCoord(view);
            return new Rectangle(
                rect.Left / Display.Scale,
                rect.Top / Display.Scale,
                rect.Width() / Display.Scale,
                rect.Height() / Display.Scale);
        }


    }
}
