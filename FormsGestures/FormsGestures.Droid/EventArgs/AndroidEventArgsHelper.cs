using Android.Views;
using Xamarin.Forms;

namespace FormsGestures.Droid
{
	public static class AndroidEventArgsHelper
	{
		public static Rectangle GetViewPosition(this global::Android.Views.View view)
        {
            int[] viewLocation = { 0, 0 };
            try
            {
                view?.GetLocationInWindow(viewLocation);
            } catch (System.Exception) { return Rectangle.Zero; }
            //var scale = P42.Utils.ReflectionExtensions.GetPropertyValue()
            //var scale = Display.Scale;
            //int left = view.Left;
            //int top = view.Top;
            int width = view.Width;
			int height = view.Height;
			return DIP.ToRectangle(viewLocation[0], viewLocation[1], width, height);
		}

		public static Rectangle GetViewGroupPosition(this ViewGroup viewGroup) {
            //int left = viewGroup.Left;
            //int top = viewGroup.Top;
            int[] viewLocation = { 0, 0 };
            try
            {
                viewGroup?.GetLocationInWindow(viewLocation);
            }
            catch (System.Exception) { return Rectangle.Zero; }

            int width = viewGroup.Width;
			int height = viewGroup.Height;
            //return DIP.ToRectangle((double)left, (double)top, (double)width, (double)height);
            return DIP.ToRectangle(viewLocation[0], viewLocation[1], width, height);
        }

        public static Point[] GetTouches(MotionEvent current, Android.Views.View view, int[] startLocation) {
			//System.Diagnostics.Debug.WriteLine("c0=["+current.GetX()+","+current.GetY()+"]");

			int[] viewLocation = { 0,0 };
            try
            { 
			view?.GetLocationInWindow (viewLocation);
            }
            catch (System.Exception) { return System.Array.Empty<Point>(); }
            var pointerCoords = new MotionEvent.PointerCoords();
			int pointerCount = current.PointerCount;
			var array = new Point[pointerCount];
			for (int i = 0; i < pointerCount; i++) {
				current.GetPointerCoords(i, pointerCoords);
				array[i] = DIP.ToPoint((double)(pointerCoords.X+viewLocation[0]-startLocation[0]), (double)(pointerCoords.Y+viewLocation[1]-startLocation[1]));
				//System.Diagnostics.Debug.WriteLine ("i=["+i+"] pc=["+pointerCoords.X+", "+pointerCoords.Y+"] a=["+array[i]+"]");
			}
            pointerCoords.Dispose();
			return array;
		}

		public static Point[] GetTouches(MotionEvent current, int requiredTouches, BaseGestureEventArgs previous, Android.Views.View view, int[] startLocation) {
			int pointerCount = current.PointerCount;
			if (pointerCount < requiredTouches && previous != null) {
				System.Diagnostics.Debug.WriteLine ("\tPointerCount < requiredTouches");
				return previous.Touches;
			}
			return AndroidEventArgsHelper.GetTouches(current, view, startLocation);
		}

		public static Point[] GetTouches(MotionEvent.PointerCoords[] coords, Android.Views.View view, int[] startLocation) {
			//System.Diagnostics.Debug.WriteLine("c0=["+current.GetX()+","+current.GetY()+"]");

			int[] viewLocation = { 0,0 };
            try
            { 
			view?.GetLocationInWindow (viewLocation);
            }
            catch (System.Exception) { return System.Array.Empty<Point>(); }

            int pointerCount = coords.Length;
			var array = new Point[pointerCount];
			for (int i = 0; i < pointerCount; i++) {
				array[i] = DIP.ToPoint((double)(coords[i].X+viewLocation[0]-startLocation[0]), (double)(coords[i].Y+viewLocation[1]-startLocation[1]));
				//System.Diagnostics.Debug.WriteLine ("i=["+i+"] pc=["+pointerCoords.X+", "+pointerCoords.Y+"] a=["+array[i]+"]");
			}

			return array;
		}

		public static Point[] GetTouches(MotionEvent.PointerCoords[] coords, BaseGestureEventArgs previous, Android.Views.View view, int[] startLocation) {
			int pointerCount = coords.Length;
			if (pointerCount < 2 && previous != null) {
				System.Diagnostics.Debug.WriteLine ("\tPointerCount < requiredTouches");
				return previous.Touches;
			}
			return AndroidEventArgsHelper.GetTouches(coords, view, startLocation);
		}

	}
}
