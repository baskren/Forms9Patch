using Android.Views;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace FormsGestures.Droid
{
    class NativeGestureListener : GestureDetector.SimpleOnGestureListener, IDisposable
    {
        #region Fields
        Java.Lang.Ref.WeakReference _weakReferenceView;

        List<Listener> _listeners;

        MotionEvent _start;
        MotionEvent _lastPan;
        MotionEvent _secondToLastPan;

        bool _longPressed;
        bool _panning;
        bool _pinching;
        bool _rotating;

        //static bool _cancelled;

        PanEventArgs _lastPanArgs;
        PanEventArgs _secondToLastPanArgs;

        int[] _viewLocationAtOnDown = { 0, 0 };
        #endregion


        #region Properties
        bool HandlesTapped
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesTapped)
                        return true;
                return false;
            }
        }

        bool HandlesDoubleTapped
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesDoubleTapped)
                        return true;
                return false;
            }
        }

        bool HandlesSwiped
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesSwiped)
                        return true;
                return false;
            }
        }

        bool HandlesLongPresses
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesLongPressed || listener.HandlesLongPressing)
                        return true;
                return false;
            }
        }



        MotionEvent Start
        {
            get { return _start; }
            set
            {
                if (value != null)
                {
                    _start?.Recycle();
                    _start = MotionEvent.Obtain(value);
                    var _view = (Android.Views.View)_weakReferenceView?.Get();
                    _view?.GetLocationInWindow(_viewLocationAtOnDown);
                }
                else
                {
                    _start = null;
                    _viewLocationAtOnDown = new[] { 0, 0 };
                }
            }
        }

        MotionEvent LastPan
        {
            get { return _lastPan; }
            set
            {
                if (_secondToLastPan != null)
                    _secondToLastPan.Recycle();
                _secondToLastPan = ((_lastPan != null && value != null) ? MotionEvent.Obtain(_lastPan) : null);

                if (_lastPan != null)
                    _lastPan.Recycle();
                _lastPan = ((value != null) ? MotionEvent.Obtain(value) : null);
            }
        }

        MotionEvent SecondToLastPan
        {
            get { return _secondToLastPan; }
        }

        PanEventArgs LastPanArgs
        {
            get { return _lastPanArgs; }
            set
            {
                _secondToLastPanArgs = ((value != null) ? _lastPanArgs : null);
                _lastPanArgs = value;
            }
        }

        PanEventArgs SecondToLastPanArgs
        {
            get { return _secondToLastPanArgs; }
        }
        #endregion


        #region Constructor / Disposer
        static int _instances;
        readonly int _id;
        internal NativeGestureListener(Android.Views.View view, List<Listener> listeners)
        {
            _id = _instances++;
            //_view = view;
            _weakReferenceView = new Java.Lang.Ref.WeakReference(view);
            //Views.Add(_view);
            _listeners = listeners;
            _touchSlop = ViewConfiguration.Get(Droid.Settings.Context).ScaledTouchSlop;
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                StopTapLongPress();
                _weakReferenceView?.Clear();
                _weakReferenceView = null;
                _listeners = null;
                _disposed = true;
            }
            base.Dispose(disposing);
        }
        #endregion


        #region LongPress Timer
        System.Timers.Timer LongPressTimer;

        void LongPressingTimerStop()
        {
            if (LongPressTimer != null)
            {
                LongPressTimer.Stop();
                LongPressTimer.Elapsed -= OnLongPressTimerElapsed;
                LongPressTimer = null;
            }
        }

        void LongPressTimerStart()
        {
            _longPressed = false;
            if (!HandlesLongPresses)
                return;
            if (!_panning && !_longPressed && !_pinching && !_rotating)
            {
                LongPressingTimerStop();
                LongPressTimer = new System.Timers.Timer(Settings.LongPressedThreshold.TotalMilliseconds);
                LongPressTimer.Elapsed += OnLongPressTimerElapsed;
                LongPressTimer.Start();
            }
        }

        void OnLongPressTimerElapsed(object sender, ElapsedEventArgs e)
        {
            LongPressingTimerStop();
            if (!_panning && !_longPressed && !_pinching && !_rotating)
            {
                _longPressed = true;
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    var _view = (Android.Views.View)_weakReferenceView?.Get();
                    if (_view == null || _listeners == null || !_listeners.Any())
                        return;
                    foreach (var listener in _listeners)
                    {
                        if (listener.HandlesLongPressing)
                        {
                            var args = new AndroidLongPressEventArgs(Start, null, _view, _viewLocationAtOnDown);
                            args.Listener = listener;
                            listener.OnLongPressing(args);
                            //if (args.Handled)
                            //  break;
                        }
                    }
                });
            }
        }
        #endregion


        #region Tapped Timer
        System.Timers.Timer TappedTimer;
        MotionEvent _tappedTimerUpMotionEvent;
        int _tappedTimerNumberOfTaps;

        void TappedTimerStop()
        {
            if (TappedTimer != null)
            {
                TappedTimer.Stop();
                TappedTimer.Elapsed -= OnTappedTimerElapsed;
                TappedTimer = null;
                //_tappedTimerUpMotionEvent = null;
            }
        }

        void TappedTimerStart(MotionEvent upEvent, int taps)
        {
            if (!HandlesTapped)
                return;
            TappedTimerStop();
            if (!_panning && !_longPressed && !_pinching && !_rotating)
            {
                _tappedTimerUpMotionEvent = upEvent;
                _tappedTimerNumberOfTaps = taps;
                TappedTimer = new System.Timers.Timer(Settings.TappedThreshold.TotalMilliseconds);
                TappedTimer.Elapsed += OnTappedTimerElapsed;
                TappedTimer.Start();
            }
        }

        void OnTappedTimerElapsed(object sender, ElapsedEventArgs e)
        {
            TappedTimerStop();
            if (!_panning && !_longPressed && !_pinching && !_rotating)
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (_listeners == null)
                        return;
                    var _view = (Android.Views.View)_weakReferenceView?.Get();
                    if (_view == null || _listeners == null || !_listeners.Any())
                        return;

                    bool tappedHandled = false;
                    foreach (var listener in _listeners)
                    {
                        if (listener.HandlesTapped)
                        {
                            TapEventArgs taskArgs = new AndroidTapEventArgs(_tappedTimerUpMotionEvent, _view, _tappedTimerNumberOfTaps, _viewLocationAtOnDown);
                            taskArgs.Listener = listener;
                            listener.OnTapped(taskArgs);
                            tappedHandled = taskArgs.Handled;
                            if (tappedHandled)
                                break;
                        }
                    }
                    _numberOfTaps = 0;
                });
            }
            //_panning = false;
            //_longPressed = false;
        }
        #endregion


        void StopTapLongPress()
        {
            TappedTimerStop();
            LongPressingTimerStop();
            _numberOfTaps = 0;
        }


        #region Down / Up / 'ed events
        DateTime _onDownDateTime = DateTime.MinValue;
        public override bool OnDown(MotionEvent e)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("OnDown [{0}]",_id);
            //System.Diagnostics.Debug.WriteLine("NativeGestureListener." + P42.Utils.ReflectionExtensions.CallerMemberName() + " Index:" + e);

            if (e.Action != MotionEventActions.Down)
                return false;

            _onDownDateTime = DateTime.Now;
            Start = e;
            _panning = false;
            _pinching = false;
            _rotating = false;
            _multiMoving = false;
            TappedTimerStop();
            LongPressTimerStart();
            bool handled = false;
            foreach (var listener in _listeners)
            {
                if (listener.HandlesDown)
                {
                    var _view = (Android.Views.View)_weakReferenceView?.Get();
                    _view?.GetLocationInWindow(_viewLocationAtOnDown);
                    var args = new AndroidDownUpEventArgs(e, _view, _viewLocationAtOnDown);
                    args.Listener = listener;
                    listener.OnDown(args);
                    handled |= args.Handled;
                    //if (args.Handled)
                    //	return true;
                }
            }
            //return handled; // we are going to (in NativeGestureDetector) always capture a DOWN touch event so we can receive all updates to this gesture
            return true;
        }


        int _numberOfTaps;
        public bool OnUp(MotionEvent ev)
        {
            //System.Diagnostics.Debug.WriteLine("NativeGestureListener." + P42.Utils.ReflectionExtensions.CallerMemberName() + " action:" + ev.Action + " index" + ev.ActionIndex + " e:" + ev);

            if (ev.Action != MotionEventActions.Up)
                return false;

            var touchDuration = DateTime.Now - _onDownDateTime;

            _numberOfTaps++;
            LongPressingTimerStop();
            TappedTimerStart(ev, _numberOfTaps);

            var _view = (Android.Views.View)_weakReferenceView?.Get();
            if (_view == null || _listeners == null || !_listeners.Any())
                return false;

            bool handled = false;
            foreach (var listener in _listeners)
            {
                if (listener.HandlesUp)
                {
                    DownUpEventArgs args = new AndroidDownUpEventArgs(ev, _view, _viewLocationAtOnDown);
                    args.Listener = listener;
                    listener.OnUp(args);
                    handled |= args.Handled;
                    //if (args.Handled)
                    //	break;
                }
            }
            if (_panning)
            {
                _numberOfTaps = 0;
                foreach (var listener in _listeners)
                {
                    if (listener.HandlesPanned)
                    {
                        PanEventArgs args = new AndroidPanEventArgs(LastPan ?? Start, ev, LastPanArgs, _view, _viewLocationAtOnDown);
                        args.Listener = listener;
                        listener.OnPanned(args);
                        handled |= args.Handled;
                        //if (args.Handled)
                        //  break;

                    }
                }
            }
            else if (_longPressed)
            {
                foreach (var listener in _listeners)
                {
                    if (listener.HandlesLongPressed)
                    {
                        LongPressEventArgs args = new AndroidLongPressEventArgs(Start, ev, _view, _viewLocationAtOnDown);
                        args.Listener = listener;
                        listener.OnLongPressed(args);
                        handled |= args.Handled;
                        //if (args.Handled)
                        //  break;
                    }
                }
            }
            else if (!_pinching && !_rotating)
            {
                foreach (var listener in _listeners)
                {
                    if (listener.HandlesTapping)
                    {
                        TapEventArgs args = new AndroidTapEventArgs(ev, _view, _numberOfTaps, _viewLocationAtOnDown);
                        args.Listener = listener;
                        listener.OnTapping(args);
                        handled |= args.Handled;
                        //if (args.Handled)
                        //	break;
                    }
                }
                if (_numberOfTaps % 2 == 0)
                    foreach (var listener in _listeners)
                    {
                        if (listener.HandlesDoubleTapped)
                        {
                            TapEventArgs args = new AndroidTapEventArgs(ev, _view, _numberOfTaps, _viewLocationAtOnDown);
                            args.Listener = listener;
                            listener.OnDoubleTapped(args);
                            handled |= args.Handled;
                            //if (args.Handled)
                            //  break;
                        }
                    }
            }

            LastPanArgs = null;
            LastPan = null;

            return handled;
        }
        #endregion


        #region Pan / Fling
        public override bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("OnScroll ["+eN.Action+"]["+_id+"] e1=["+e0+"] e2=["+eN+"]");
            //System.Diagnostics.Debug.WriteLine("SCROLL");
            bool handled = false;
            StopTapLongPress();
            if (!_multiMoving)
            {

                var _view = (Android.Views.View)_weakReferenceView?.Get();
                if (_view == null || _listeners == null || !_listeners.Any())
                    return false;

                _panning = true;
                foreach (var listener in _listeners)
                {
                    if (listener.HandlesPanning || listener.HandlesPanned)
                    {
                        PanEventArgs args = new AndroidPanEventArgs(LastPan ?? Start, e2, LastPanArgs, _view, _viewLocationAtOnDown);
                        args.Listener = listener;
                        listener.OnPanning(args);
                        LastPanArgs = args;
                        LastPan = e2;
                        //handled = true;
                        handled |= args.Handled;
                        //if (args.Handled)
                        //	break;
                    }
                }
            }
            return handled;
        }

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("OnFling [{0}]",_id);
            StopTapLongPress();

            var _view = (Android.Views.View)_weakReferenceView?.Get();
            if (_view == null || _listeners == null || !_listeners.Any())
                return false;

            double hzVel = Math.Abs(velocityX);
            double vtVel = Math.Abs(velocityY);
            Direction direction = Direction.NotClear;
            if (hzVel > 2 * vtVel)
                direction = ((velocityX > 0f) ? Direction.Right : Direction.Left);
            else if (vtVel > 2 * hzVel)
                direction = ((velocityY > 0f) ? Direction.Down : Direction.Up);
            bool handled = false;
            foreach (var listener in _listeners)
            {
                if (listener.HandlesSwiped)
                {
                    SwipeEventArgs args = new AndroidSwipeEventArgs(e2, _view, direction, _viewLocationAtOnDown);
                    args.Listener = listener;
                    listener.OnSwiped(args);
                    handled |= args.Handled;
                    //if (args.Handled)
                    //	break;
                }
            }
            // OnFling overrides OnUp??
            OnUp(e2);
            return handled;
        }
        #endregion


        #region Multi-point gestures
        PinchEventArgs _previousPinchArgs;
        RotateEventArgs _previousRotateArgs;

        bool _multiMoving;
        bool _multiInBounds;

        protected readonly int _touchSlop;

        MotionEvent _multiStart;
        MotionEvent MultiStart
        {
            get { return _multiStart; }
            set
            {
                if (value != null)
                {
                    _multiStart?.Recycle();
                    _multiStart = MotionEvent.Obtain(value);
                }
                else
                {
                    _multiStart = null;
                }
            }
        }

        MotionEvent.PointerCoords[] _startCoords;
        //MotionEvent.PointerCoords[] _lastCoords;
        public bool OnMultiDown(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            StopTapLongPress();
            MultiStart = ev;
            _multiMoving = false;
            //_lastCoords = coords;
            _startCoords = coords;
            return false;
        }

        public bool OnMultiMove(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            StopTapLongPress();
            var _view = (Android.Views.View)_weakReferenceView?.Get();
            if (_view != null && !_multiMoving)
            {
                _multiInBounds = true;
                for (int i = 0; i < 2; i++)
                {
                    var pX = coords[i].X;
                    var pY = coords[i].Y;
                    if (pX < _view.Width && pY < _view.Height)
                    {
                        float xDist = Math.Abs(_startCoords[i].X - pX);
                        float yDist = Math.Abs(_startCoords[i].Y - pY);
                        _multiMoving |= (xDist > _touchSlop || yDist > _touchSlop);
                    }
                    else
                        _multiInBounds = false;
                }
            }
            bool handled = false;
            if (_multiMoving && _multiInBounds)
            {
                handled |= OnPinching(ev, coords);
                handled |= OnRotating(ev, coords);
            }
            return handled;
        }

        public bool OnMultiUp(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            bool handled = false;
            if (_multiMoving)
            {
                StopTapLongPress();
                handled |= OnPinched(ev, coords);
                handled |= OnRotated(ev, coords);
                MultiStart = null;
                _multiMoving = false;
            }
            return handled;
        }


        /*
		public bool onMoving(MotionEvent current)
		{
			if (_debugEvents) System.Diagnostics.Debug.WriteLine ("onMoving [{0}]",_id);

			timerStop ();
			bool result = OnScroll(Start, current, 0f, 0f);
			onPinching(current);
			onRotating(current);
			return result;
		}
		*/


        bool OnPinching(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine("onPinching [{0}]", _id);
            StopTapLongPress();
            _pinching = true;

            var _view = (Android.Views.View)_weakReferenceView?.Get();
            if (_view == null || _listeners == null || !_listeners.Any())
                return false;

            PinchEventArgs pinchArgs = new AndroidPinchEventArgs(ev, coords, _previousPinchArgs, _view, _viewLocationAtOnDown);
            bool handled = false;
            foreach (var listener in _listeners)
                if (listener.HandlesPinching)
                {
                    var args = new PinchEventArgs(pinchArgs);
                    args.Listener = listener;
                    listener.OnPinching(args);
                    handled |= args.Handled;
                    //if (args.Handled)
                    //	break;
                }
            _previousPinchArgs = pinchArgs;
            return handled;
        }

        bool OnRotating(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine("onRotating [{0}]", _id);
            StopTapLongPress();
            _rotating = true;

            var _view = (Android.Views.View)_weakReferenceView?.Get();
            if (_view == null || _listeners == null || !_listeners.Any())
                return false;

            RotateEventArgs rotateArgs = new AndroidRotateEventArgs(ev, coords, _previousRotateArgs, _view, _viewLocationAtOnDown);
            bool handled = false;
            foreach (var listener in _listeners)
                if (listener.HandlesRotating)
                {
                    var args = new RotateEventArgs(rotateArgs);
                    args.Listener = listener;
                    listener.OnRotating(args);
                    handled |= args.Handled;
                    //if (args.Handled)
                    //	break;
                }
            _previousRotateArgs = rotateArgs;
            return handled;
        }


        bool OnPinched(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine("onPinched [{0}]", _id);
            StopTapLongPress();
            _pinching = true;

            var _view = (Android.Views.View)_weakReferenceView?.Get();
            if (_view == null || _listeners == null || !_listeners.Any())
                return false;

            bool handled = false;
            if (_previousPinchArgs != null)
            {
                foreach (var listener in _listeners)
                {
                    if (listener.HandlesPinching || listener.HandlesPinched)
                    {
                        PinchEventArgs args = new AndroidPinchEventArgs(ev, coords, _previousPinchArgs, _view, _viewLocationAtOnDown);
                        args.Listener = listener;
                        listener.OnPinched(args);
                        handled |= args.Handled;
                        //if (args.Handled)
                        //	break;
                    }
                }
                _previousPinchArgs = null;
            }
            return handled;
        }

        bool OnRotated(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine("onRotated [{0}]", _id);
            StopTapLongPress();
            _rotating = true;

            var _view = (Android.Views.View)_weakReferenceView?.Get();
            if (_view == null || _listeners == null || !_listeners.Any())
                return false;

            bool handled = false;
            if (_previousRotateArgs != null)
            {
                foreach (var listener in _listeners)
                {
                    if (listener.HandlesRotating || listener.HandlesRotated)
                    {
                        RotateEventArgs args = new AndroidRotateEventArgs(ev, coords, _previousRotateArgs, _view, _viewLocationAtOnDown);
                        args.Listener = listener;
                        listener.OnRotated(args);
                        handled |= args.Handled;
                        //if (args.Handled)
                        //	break;
                    }
                }
                _previousRotateArgs = null;
            }
            return handled;
        }

        #endregion
    }
}
