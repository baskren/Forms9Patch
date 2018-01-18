using Android.Content;
using Android.Views;
using System;

namespace FormsGestures.Droid
{
	class MultiTouchGestureDetector
	{
		protected readonly Context Context;

		protected readonly IMultiTouchGestureListener Listener;

		protected readonly int touchSlop;

		bool _gestureInProgress;

		MotionEvent _start;

		public bool IsInProgress {
			get { return _gestureInProgress; }
		}

		MotionEvent Start {
			get { return _start; }
			set {
				if (_start != null)
					_start.Recycle();
				_start = ((value != null) ? MotionEvent.Obtain(value) : null);
			}
		}

		internal MultiTouchGestureDetector(Context context, IMultiTouchGestureListener listener) {
			Context = context;
			Listener = listener;
			touchSlop = ViewConfiguration.Get(Context).ScaledTouchSlop;
		}

		public bool OnTouchEvent(MotionEvent e) {
			bool result = false;
			if (e.PointerCount >= 2) {
				MotionEventActions action = e.Action;
				switch (action) {
				case MotionEventActions.Move:
					if (Start == null)
						Start = e;
					if (!_gestureInProgress) {
						for (int i = 0; i < e.PointerCount; i++) {
							int num = Start.FindPointerIndex(e.GetPointerId(i));
							if (num >= 0) {
								float num2 = Math.Abs(Start.GetX(num) - e.GetX(i));
								float num3 = Math.Abs(Start.GetY(num) - e.GetY(i));
								if (num2 > (float)touchSlop || num3 > (float)touchSlop)
									_gestureInProgress = true;
							}
						}
					}
					if (_gestureInProgress) {
						result = Listener.onMoving(e);
						return result;
					}
					return result;
				case MotionEventActions.Cancel:
					break;
				default:
					switch (action) {
					case MotionEventActions.Pointer2Down:
						Start = e;
						return result;
					case MotionEventActions.Pointer2Up:
						break;
					default:
						return result;
					}
					break;
				}
				result = EndGesture(e);
			} else {
				result = EndGesture(e);
			}
			return result;
		}

		bool EndGesture(MotionEvent e) {
			bool result = false;
			if (_gestureInProgress)
				result = Listener.onMoved(e);
			Start = null;
			_gestureInProgress = false;
			return result;
		}
	}
}
