using System;
using Android.Views;

namespace FormsGestures.Droid
{
	public class OnTouchListener : Java.Lang.Object, View.IOnTouchListener
	{
		public bool OnTouch(View v, MotionEvent e)
		{
			if (e.Action == MotionEventActions.Down)
			{
				// do stuff
				return true;
			}
			if (e.Action == MotionEventActions.Up)
			{
				// do other stuff
				return true;
			}

			return false;
		}
	}
}

