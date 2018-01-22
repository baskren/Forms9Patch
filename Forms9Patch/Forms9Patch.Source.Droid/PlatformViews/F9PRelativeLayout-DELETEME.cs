// /*******************************************************************
//  *
//  * F9PRelativeLayout.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Android.Content;
using Android.Util;

namespace Forms9Patch.Droid
{
	public class F9PRelativeLayout : global::Android.Widget.RelativeLayout
	{
		public F9PRelativeLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base (context, attrs, defStyleAttr)
		{
		}

		public F9PRelativeLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base (context, attrs, defStyleAttr, defStyleRes)
		{
		}

		public F9PRelativeLayout(Context context) : base(context)
		{
		}

		public F9PRelativeLayout(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		public override bool OnInterceptTouchEvent(Android.Views.MotionEvent ev)
		{
			System.Diagnostics.Debug.WriteLine("F9PRelativeLayout.OnInterceptTouchEvent");
			if (FormsGestures.Droid.NativeGestureHandler.Views.Contains(this))
			{
				System.Diagnostics.Debug.WriteLine("\tTRUE");
				return true;
			}
			System.Diagnostics.Debug.WriteLine("\t????");
			return base.OnInterceptTouchEvent(ev);
		}
	}
}

