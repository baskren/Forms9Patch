using System;
using System.Threading.Tasks;
using Android.Views;

namespace Forms9Patch.Droid
{
    static class AndroidViewExtensions
    {
        public static T GetClosestAncestor<T>(this View view) where T : class
        {
            while (view?.Parent != null)
            {
                var parent = view.Parent as T;
                if (parent != null)
                    return parent;
                view = view.Parent as View;
            }
            return null;
        }

        public static T GetFurthestAncestor<T>(this View view) where T : class
        {
            T root = view?.Parent as T;
            while (view?.Parent != null)
            {
                var parent = view.Parent as T;
                if (parent != null)
                    root = parent;
                view = view.Parent as View;
            }
            return root;
        }

        public static T GetChild<T>(this ViewGroup viewGroup) where T : class
        {
            T child = null;
            int numVisuals = viewGroup.ChildCount;
            for (int i = 0; i < numVisuals; i++)
            {
                //var v = VisualTreeHelper.GetChild(viewGroup, i);
                var v = viewGroup.GetChildAt(i);
                child = v as T;
                if (child != null)
                    return child;
                if (v is ViewGroup childViewGroup)
                    child = childViewGroup.GetChild<T>();
                if (child != null)
                    return child;
            }
            return child;
        }

        public static void GetBitmap(this Android.Views.View view, System.Action<Android.Graphics.Bitmap> callback)
        {
            var window = Settings.Activity.Window;
            var bitmap = Android.Graphics.Bitmap.CreateBitmap(view.Width, view.Height, Android.Graphics.Bitmap.Config.Argb8888);
            var locationInWindow = new int[2];
            view.GetLocationInWindow(locationInWindow);
            try
            {
                var rect = new Android.Graphics.Rect(locationInWindow[0], locationInWindow[1], locationInWindow[0] + view.Width, locationInWindow[1] + view.Height);
                PixelCopy.Request(window, rect, bitmap, new PixelCopyListener(callback, bitmap), null);
            }
            catch (Java.Lang.IllegalArgumentException e)
            {
                e.PrintStackTrace();
            }
        }

    }

    internal class PixelCopyListener : Java.Lang.Object, PixelCopy.IOnPixelCopyFinishedListener
    {
        System.Action<Android.Graphics.Bitmap> _callback;
        Android.Graphics.Bitmap _bitmap;

        public PixelCopyListener(System.Action<Android.Graphics.Bitmap> callback, Android.Graphics.Bitmap bitmap)
        {
            _callback = callback;
            _bitmap = bitmap;
        }

        public void OnPixelCopyFinished(int copyResult)
        {
            if (copyResult == (int)PixelCopyResult.Success)
                _callback?.Invoke(_bitmap);
        }
    }
}