using Android.Views;

namespace FormsGestures.Droid
{
	interface IMultiTouchGestureListener
	{
		bool onMoved(MotionEvent current);

		bool onMoving(MotionEvent current);
	}
}
