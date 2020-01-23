using UIKit;

namespace FormsGestures.iOS
{
	public class iOSLongPressEventArgs : LongPressEventArgs
	{
		public iOSLongPressEventArgs(UILongPressGestureRecognizer gr, long duration)
		{
			Cancelled = (gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed);
			ElementPosition = gr.View.BoundsInFormsCoord();
			ElementTouches = iOSEventArgsHelper.GetTouches(gr, gr.View);
			WindowTouches = iOSEventArgsHelper.GetTouches(gr, null);
			Duration = duration;
		}
	}
}
