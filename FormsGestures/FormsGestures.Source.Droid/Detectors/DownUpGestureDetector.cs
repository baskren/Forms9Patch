using Android.Views;

namespace FormsGestures.Droid
{
	class DownUpGestureDetector
	{
		protected readonly DownUpGestureListener DownUpListener;
		protected readonly NativeGestureListener SimpleListener;

		internal DownUpGestureDetector(DownUpGestureListener downupListener, NativeGestureListener listener) {
			DownUpListener = downupListener;
			SimpleListener = listener;
		}

		public bool OnTouchEvent(MotionEvent e) {
			bool result = false;
			switch (e.ActionMasked) {
			case MotionEventActions.Down:
			case MotionEventActions.Pointer1Down:
				//result = DownUpListener.onDown(e);
				break;
			case MotionEventActions.Up:
			case MotionEventActions.Pointer1Up:
				//result = DownUpListener.onUp (e);
				//SimpleListener.EndGestures (e);
				break;
			}
			return result;
		}
	}
}
