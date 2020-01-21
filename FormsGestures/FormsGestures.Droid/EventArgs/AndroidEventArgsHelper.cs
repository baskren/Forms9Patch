using Android.Views;
using Xamarin.Forms;

namespace FormsGestures.Droid
{
	public static class AndroidEventArgsHelper
	{
		public static Rectangle GetViewPosition(this global::Android.Views.View view)
        {
			if (view == null)
				return Rectangle.Zero;

            int[] viewLocation = { 0, 0 };
            try
            {
				//view.GetLocationInWindow(viewLocation);
				view.GetLocationOnScreen(viewLocation);
            } catch (System.Exception) { return Rectangle.Zero; }

            int width = view.Width;
			int height = view.Height;
			return DIP.ToRectangle(viewLocation[0], viewLocation[1], width, height);
		}

        public static Point[] GetTouches(MotionEvent current, Android.Views.View view, int[] startLocation, Listener listener)
        {
			/*
			var decoreView = Settings.Activity.Window.DecorView;
			var decoreHeight = decoreView.Height;
			var decoreWidth = decoreView.Width;

			var visibleRect = new Android.Graphics.Rect();
			decoreView.GetWindowVisibleDisplayFrame(visibleRect);
            System.Diagnostics.Debug.WriteLine("AndroidEventArgsHelper decoreView: ["+visibleRect.Left+","+visibleRect.Top+"] [" + visibleRect.Width() + "," + visibleRect.Height() + "]");
            */

			// the below works ... except it doesn't account for padding on the Page's Padd
			/*
            var sourceViewBounds = view.BoundsInScreenCoord();
			var listenerBounds = VisualElementExtensions.BoundsToWinCoord(listener.Element);
			var delta = new Point(sourceViewBounds.X - listenerBounds.X,  sourceViewBounds.Y - listenerBounds.Y);

			var pointerCoords = new MotionEvent.PointerCoords();
			int pointerCount = current.PointerCount;
			var array = new Point[pointerCount];
			for (int i = 0; i < pointerCount; i++)
			{
				current.GetPointerCoords(i, pointerCoords);
				array[i] = DIP.ToPoint(pointerCoords.X, pointerCoords.Y);
				array[i] = array[i].Add(delta);
			}
			pointerCoords.Dispose();
			return array;
            */
            /*
			System.Diagnostics.Debug.WriteLine("===============================");
			var listenerViewLocation = VisualElementExtensions.LocationToWinCoord(listener.Element);
			var touchViewLocation = AndroidViewExtensions.LocationInDipCoord(view);
			var delta = touchViewLocation.Subtract(listenerViewLocation);

			System.Diagnostics.Debug.WriteLine("[GetTouches.A."
				+ P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
	            + P42.Utils.ReflectionExtensions.CallerLineNumber()
	            + "] lvl["+listenerViewLocation+"]");
			System.Diagnostics.Debug.WriteLine("[GetTouches.A."
				+ P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
	            + P42.Utils.ReflectionExtensions.CallerLineNumber()
	            + "] tvl["+touchViewLocation+"]");
			System.Diagnostics.Debug.WriteLine("[GetTouches.A."
				+ P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
	            + P42.Utils.ReflectionExtensions.CallerLineNumber()
	            + "] delta["+delta+"]");
            */

			int pointerCount = current.PointerCount;
			var pointerCoords = new MotionEvent.PointerCoords[pointerCount];
			using (var pointerCoord = new MotionEvent.PointerCoords())
			{
				for (int i = 0; i < pointerCount; i++)
				{
					current.GetPointerCoords(i, pointerCoord);
					pointerCoords[i] = new MotionEvent.PointerCoords(pointerCoord);
				}
			}

			System.Diagnostics.Debug.WriteLine("===============================");

			return GetTouches(pointerCoords, view, startLocation, listener);
		}

		public static Point[] GetTouches(MotionEvent current, int requiredTouches, BaseGestureEventArgs previous, Android.Views.View view, int[] startLocation, Listener listener) {
			int pointerCount = current.PointerCount;
			if (pointerCount < requiredTouches && previous != null) {
				System.Diagnostics.Debug.WriteLine ("\tPointerCount < requiredTouches");
				return previous.Touches;
			}
			return AndroidEventArgsHelper.GetTouches(current, view, startLocation, listener);
		}

		public static Point[] GetTouches(MotionEvent.PointerCoords[] pointerCoords, Android.Views.View view, int[] startLocation, Listener listener) {
			//System.Diagnostics.Debug.WriteLine("c0=["+current.GetX()+","+current.GetY()+"]");
			/*
			int[] viewLocation = { 0,0 };
            try
            { 
			    view?.GetLocationInWindow (viewLocation);
            }
            catch (System.Exception) { return System.Array.Empty<Point>(); }

			var listenerPosition = VisualElementExtensions.BoundsToWinCoord(listener.Element);
			var deltaX = viewLocation[0] - listenerPosition.X;
			var deltaY = viewLocation[1] - listenerPosition.Y;

			int pointerCount = coords.Length;
			var array = new Point[pointerCount];
			for (int i = 0; i < pointerCount; i++) {
				array[i] = DIP.ToPoint(coords[i].X+viewLocation[0]-startLocation[0]-deltaX, coords[i].Y+viewLocation[1]-startLocation[1]-deltaY);
				//System.Diagnostics.Debug.WriteLine ("i=["+i+"] pc=["+pointerCoords.X+", "+pointerCoords.Y+"] a=["+array[i]+"]");
			}

			return array;
            */
			System.Diagnostics.Debug.WriteLine("===============================");

			var listenerViewLocation = VisualElementExtensions.LocationInWindowCoord(listener.Element);
			var touchViewLocation = AndroidViewExtensions.LocationInDipCoord(view);
			var delta = touchViewLocation.Subtract(listenerViewLocation);

			System.Diagnostics.Debug.WriteLine("[GetTouches.B."
	            + P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
	            + P42.Utils.ReflectionExtensions.CallerLineNumber()
	            + "] lvl[" + listenerViewLocation + "] ["+listener.Element+"] Bounds["+listener.Element.Bounds+"]");
			System.Diagnostics.Debug.WriteLine("[GetTouches.B."
				+ P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
				+ P42.Utils.ReflectionExtensions.CallerLineNumber()
				+ "] tvl[" + touchViewLocation + "] ["+view+"]");
			System.Diagnostics.Debug.WriteLine("[GetTouches.B."
				+ P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
				+ P42.Utils.ReflectionExtensions.CallerLineNumber()
				+ "] delta[" + delta + "]");


			var pointerCount = pointerCoords.Length;
			var array = new Point[pointerCount];
			for (int i = 0; i < pointerCount; i++)
			{
				System.Diagnostics.Debug.WriteLine("[GetTouches.B."
					+ P42.Utils.ReflectionExtensions.CallerMemberName() + ":"
					+ P42.Utils.ReflectionExtensions.CallerLineNumber()
					+ "] poitnerCoord[" + pointerCoords[i] + "]");
				array[i] = DIP.ToPoint(pointerCoords[i]);
				array[i] = array[i].Add(delta);
			}

			System.Diagnostics.Debug.WriteLine("===============================");

			return array;
		}

		public static Point[] GetTouches(MotionEvent.PointerCoords[] coords, BaseGestureEventArgs previous, Android.Views.View view, int[] startLocation, Listener listener) {
			int pointerCount = coords.Length;
			if (pointerCount < 2 && previous != null) {
				System.Diagnostics.Debug.WriteLine ("\tPointerCount < requiredTouches");
				return previous.Touches;
			}
			return AndroidEventArgsHelper.GetTouches(coords, view, startLocation, listener);
		}

	}
}
