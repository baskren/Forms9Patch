using System;
using UIKit;

namespace FormsGestures.iOS
{
	public class iOSSwipeEventArgs : SwipeEventArgs
	{
		public iOSSwipeEventArgs(UISwipeGestureRecognizer gr, CoreGraphics.CGPoint locationAtStart)
		{
			Initialize(gr,locationAtStart);
			Direction = GetDirection(gr);
		}

		public iOSSwipeEventArgs(UIPanGestureRecognizer gr, Direction direction, CoreGraphics.CGPoint locationAtStart)
		{
			Initialize(gr,locationAtStart);
			Direction = direction;
		}

		private void Initialize(UIGestureRecognizer gr, CoreGraphics.CGPoint locationAtStart)
		{
			Cancelled = (gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed);
			ViewPosition = gr.View.BoundsInDipCoord();
			Touches = iOSEventArgsHelper.GetTouches(gr,locationAtStart);
		}

		private Direction GetDirection(UISwipeGestureRecognizer gr)
		{
			UISwipeGestureRecognizerDirection direction = gr.Direction;
			if (direction <= UISwipeGestureRecognizerDirection.Up)
			{
				if (direction < UISwipeGestureRecognizerDirection.Right)
				{
					return Direction.NotClear;
				}
				switch ((int)(direction - UISwipeGestureRecognizerDirection.Right))
				{
				case 0:
					return Direction.Right;
				case 1:
					return Direction.Left;
				case 2:
					return Direction.NotClear;
				case 3:
					return Direction.Up;
				}
			}
			if (direction == UISwipeGestureRecognizerDirection.Down)
			{
				return Direction.Down;
			}
			return Direction.NotClear;
		}
	}
}
