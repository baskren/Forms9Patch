using Foundation;
using System;
using System.Linq;
using UIKit;

namespace FormsGestures.iOS
{
    class DownUpGestureRecognizer : UIGestureRecognizer
    {
        internal Action<DownUpGestureRecognizer, UITouch[]> DownAction { get; set; }

        internal Action<DownUpGestureRecognizer, UITouch[]> UpAction { get; set; }

        internal DownUpGestureRecognizer(Action<DownUpGestureRecognizer, UITouch[]> downAction, Action<DownUpGestureRecognizer, UITouch[]> upAction)
        {
            DownAction = downAction;
            UpAction = upAction;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            UITouch[] arg = Enumerable.Where<UITouch>(Enumerable.OfType<UITouch>(touches), t => t.Phase == UITouchPhase.Began).ToArray();
            DownAction?.Invoke(this, arg);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            UITouch[] arg = Enumerable.Where<UITouch>(Enumerable.OfType<UITouch>(touches), t => t.Phase == UITouchPhase.Ended).ToArray();
            UpAction?.Invoke(this, arg);
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
            State = UIGestureRecognizerState.Failed;
        }

        public DownUpGestureRecognizer()
        {
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                DownAction = null;
                UpAction = null;
            }
            base.Dispose(disposing);
        }
    }
}
