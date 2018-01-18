// /*******************************************************************
//  *
//  * AndroidViewExtensions.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Android.Views;
namespace FormsGestures.Droid
{
	public static class AndroidViewExtensions
	{
		public static bool TouchUpViewHeirarchy(this View view, MotionEvent e)
		{
			if (view == null)
				return true;
			var parent = view.Parent as View;
			if (parent == null)
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
			System.Diagnostics.Debug.WriteLine("TouchUpViewHeirarchy: x=["+e.GetX()+"] y=["+e.GetY()+"] type=["+parent.GetType()+"] action=["+e.Action+"] handled=["+handled+"] ");
			if (!handled)
				handled = parent.TouchUpViewHeirarchy(e);
			e.OffsetLocation(offsetX, offsetY);
			return handled;
		}

	}
}
