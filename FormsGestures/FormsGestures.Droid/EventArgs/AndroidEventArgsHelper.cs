using Android.Views;
using Xamarin.Forms;

namespace FormsGestures.Droid
{
	public static class AndroidEventArgsHelper
	{
        public static Point[] GetTouches(MotionEvent current, Android.Views.View view, Listener listener)
        {
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
			return GetTouches(pointerCoords, view, listener);
		}

		public static Point[] GetTouches(MotionEvent.PointerCoords[] coords, BaseGestureEventArgs previous, Android.Views.View view, Listener listener)
		{
			int pointerCount = coords.Length;
			if (pointerCount < 2 && previous != null)
			{
				System.Diagnostics.Debug.WriteLine("\tPointerCount < requiredTouches");
				return previous.ElementTouches;
			}
			return GetTouches(coords, view, listener);
		}

		public static Point[] GetTouches(MotionEvent.PointerCoords[] pointerCoords, Android.Views.View sourceView, Listener listener)
        {
			var listenerViewLocation = listener!=null
                ? VisualElementExtensions.LocationInWindowCoord(listener.Element)
                : Point.Zero;
			var touchViewLocation = AndroidViewExtensions.LocationInFormsCoord(sourceView);
			Point delta = touchViewLocation.Subtract(listenerViewLocation);

			var pointerCount = pointerCoords.Length;
			var array = new Point[pointerCount];
			for (int i = 0; i < pointerCount; i++)
			{
				array[i] = DIP.ToPoint(pointerCoords[i]);
				array[i] = array[i].Add(delta);
			}
			return array;
		}

	}
}
