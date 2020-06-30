using System;
using Android.Views;
using Android.Content;

namespace FormsGestures.Droid
{
    internal class NativeGestureDetector : GestureDetector, View.IOnTouchListener
    {
        NativeGestureListener _listener;
        //int _lastEventPointerCount;

        public NativeGestureDetector(Context context, NativeGestureListener listener) : base(context, listener)
        {
            _listener = listener;
            IsLongpressEnabled = false;
            _avgCoords = new MotionEvent.PointerCoords[6];
            for (int i = 0; i < 6; i++)
                _avgCoords[i] = new MotionEvent.PointerCoords();
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                _listener = null;
                //IsLongpressEnabled = false;
            }
            base.Dispose(disposing);
        }


        MotionEvent.PointerCoords[] _lastCoords = null;
        readonly MotionEvent.PointerCoords[] _avgCoords;
        MotionEvent _lastMotionEvent;
        public override bool OnTouchEvent(MotionEvent e)
        {
            //System.Diagnostics.Debug.WriteLine("=============================================================");
            //System.Diagnostics.Debug.WriteLine("NativeGestureDetector.OnTouchEvent e.Action=[" + e.Action + "]");
            bool handled = base.OnTouchEvent(e);
            //System.Diagnostics.Debug.WriteLine("NativeGestureDetector.OnTouchEvent handled=" + handled);

            if (e.PointerCount > 1 && _listener != null)
            {
                // multi point gesture ?
                bool[] valid = new bool[6];
                MotionEvent.PointerCoords[] coords = new MotionEvent.PointerCoords[6];
                for (int i = 0; i < Math.Min(e.PointerCount, 6); i++)
                {
                    coords[i] = new MotionEvent.PointerCoords();
                    var index = e.FindPointerIndex(i);
                    if (index > -1 && index < 6)
                    {
                        valid[index] = true;
                        e.GetPointerCoords(index, coords[i]);
                        if (_lastCoords != null && _lastCoords[i] != null)
                        {
                            _avgCoords[i].X = (float)((coords[i].X + _lastCoords[i].X) / 2.0);
                            _avgCoords[i].Y = (float)((coords[i].Y + _lastCoords[i].Y) / 2.0);
                        }
                        _lastCoords = coords;
                    }
                }

                if (e.Action == MotionEventActions.Down || e.Action == MotionEventActions.Pointer1Down || e.Action == MotionEventActions.Pointer2Down)
                {
                    handled = handled || _listener.OnMultiDown(e, _lastCoords);
                }
                else if (e.Action == MotionEventActions.Move)
                {
                    handled = handled || _listener.OnMultiMove(e, _avgCoords);
                }
                else if (e.Action == MotionEventActions.Cancel || e.Action == MotionEventActions.Up || e.Action == MotionEventActions.Pointer1Up || e.Action == MotionEventActions.Pointer2Up)
                {
                    handled = handled || _listener.OnMultiUp(e, _avgCoords);
                    _lastCoords = null;
                }
            }
            // the following line was once needed but now it seems to cause double "UPS" to be called when used with Bc3.Forms.KeypadButton;
            //else if (e.ActionMasked == MotionEventActions.Up || e.ActionMasked == MotionEventActions.Pointer1Up || (e.Action == MotionEventActions.Down && _lastMotionEvent != null && _lastMotionEvent.Action == MotionEventActions.Down))
            else if (e.ActionMasked == MotionEventActions.Up || e.ActionMasked == MotionEventActions.Pointer1Up)
                _listener?.OnUp(e);
            else if (e.Action == MotionEventActions.Cancel)
                _listener?.Cancel(e);
            else if (e.Action == MotionEventActions.Move)
                handled = handled || _listener.HandlesMove;


            _lastMotionEvent = e;
            //System.Diagnostics.Debug.WriteLine("NativeGestureDetector.OnTouchEvent handled=" + handled);
            return handled;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            //System.Diagnostics.Debug.WriteLine("NativeGestureDetector.OnTouch(" + v + ", " + e.Action + ")");
            var handled = OnTouchEvent(e);
            //System.Diagnostics.Debug.WriteLine("\n\nTouch: Action=[" + e.Action + "] Handled=[" + handled + "]");
            if (!handled)
                v?.TouchUpViewHeirarchy(e);
            return handled | e?.Action == MotionEventActions.Down;
        }
    }
}

