using Android.Views;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Globalization;

namespace FormsGestures.Droid
{
    class NativeGestureListener : GestureDetector.SimpleOnGestureListener, IDisposable
    {
        #region Fields
        Android.Views.View _view;

        readonly Xamarin.Forms.Element Element;

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
        public bool HandlesPressTapEvents => HandlesTapped || HandlesDoubleTapped || HandlesLongPresses;

        public bool HandlesTapped
        {
            get
            {
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    var listener = handler.Listener;
                    if (listener != null && listener.HandlesTapped)
                        return true;
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
                return false;
            }
        }

        public bool HandlesDoubleTapped
        {
            get
            {
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    var listener = handler.Listener;
                    if (listener != null && listener.HandlesDoubleTapped)
                        return true;
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
                return false;
            }
        }

        public bool HandlesPan
        {
            get
            {
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    var listener = handler.Listener;
                    if (listener != null && (listener.HandlesPanned || listener.HandlesPanning))
                        return true;
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
                return false;
            }
        }


        public bool HandlesMove
        {
            get
            {
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    var listener = handler.Listener;
                    if (listener != null && (listener.HandlesPanned || listener.HandlesPanning || listener.HandlesPinched || listener.HandlesPinching || listener.HandlesSwiped || listener.HandlesRotated || listener.HandlesRotating))
                        return true;
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
                return false;
            }
        }

        public bool HandlesSwiped
        {
            get
            {
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    var listener = handler.Listener;
                    if (listener != null && listener.HandlesSwiped)
                        return true;
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
                return false;
            }
        }

        public bool HandlesLongPresses
        {
            get
            {
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    var listener = handler.Listener;
                    if (listener != null && (listener.HandlesLongPressed || listener.HandlesLongPressing))
                        return true;
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
                return false;
            }
        }

        bool RendererDisposed => Xamarin.Forms.Platform.Android.Platform.GetRenderer(Element as VisualElement) == null;

        MotionEvent _start;
        MotionEvent Start
        {
            get => _start;
            set
            {
                _start?.Recycle();
                if (value != null)
                {
                    _start = MotionEvent.Obtain(value);
                    _view?.GetLocationInWindow(_viewLocationAtOnDown);
                }
                else
                {
                    _start = null;
                    _viewLocationAtOnDown = new[] { 0, 0 };
                }
            }
        }

        MotionEvent _lastPan;
        MotionEvent LastPan
        {
            get => _lastPan;
            set
            {
                _secondToLastPan?.Recycle();
                _secondToLastPan = ((_lastPan != null && value != null) ? MotionEvent.Obtain(_lastPan) : null);

                _lastPan?.Recycle();
                _lastPan = (value != null) ? MotionEvent.Obtain(value) : null;
            }
        }

        MotionEvent _secondToLastPan;
        //MotionEvent SecondToLastPan => _secondToLastPan;


        PanEventArgs LastPanArgs
        {
            get => _lastPanArgs;
            set
            {
                _secondToLastPanArgs = ((value != null) ? _lastPanArgs : null);
                _lastPanArgs = value;
            }
        }

        MotionEvent _tappedTimerUpMotionEvent;
        MotionEvent TappedTimerUpMotionEvent
        {
            get => _tappedTimerUpMotionEvent;
            set
            {
                _tappedTimerUpMotionEvent?.Recycle();
                _tappedTimerUpMotionEvent = (value != null) ? MotionEvent.Obtain(value) : null;
            }
        }

        //PanEventArgs SecondToLastPanArgs => _secondToLastPanArgs;

        #endregion


        #region Constructor / Disposer
        static int _instances;
        readonly int _id;
        internal NativeGestureListener(Android.Views.View view, Xamarin.Forms.Element element)
        {
            P42.Utils.Debug.AddToCensus(this);

            _id = _instances++;
            _view = view;
            Element = element;
            _touchSlop = ViewConfiguration.Get(Droid.Settings.Context).ScaledTouchSlop;
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                StopTapLongPress();
                _view = null;
                _start?.Dispose();
                _lastPan?.Dispose();
                _secondToLastPan?.Dispose();
                _tappedTimerUpMotionEvent.Dispose();
                LongPressTimer?.Dispose();
                TappedTimer?.Dispose();
                P42.Utils.Debug.RemoveFromCensus(this);
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
            // System.Diagnostics.Debug.WriteLine("NativeGestureListener.LongPressTimerStart ENTER");
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
            //System.Diagnostics.Debug.WriteLine("NativeGestureListener.LongPressTimerElapsed ENTER");
            LongPressingTimerStop();

            if (!_panning && !_longPressed && !_pinching && !_rotating)
            {
                _longPressed = true;
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (!RendererDisposed)
                    {
                        var args = new AndroidLongPressEventArgs(Start, null, _view, _viewLocationAtOnDown);
                        var handler = NativeGestureHandler.InstanceForElement(Element);
                        while (handler != null)
                        {
                            var listener = handler.Listener;
                            if (listener != null && listener.HandlesLongPressing)
                            {
                                args.Listener = listener;
                                //System.Diagnostics.Debug.WriteLine("NativeGestureListener.LongPressTimerElapsed LongPressing");
                                listener.OnLongPressing(args);
                                if (args.Handled)
                                    break;
                            }
                            handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                        }
                    }
                });
            }
        }
        #endregion


        #region Tapped Timer
        System.Timers.Timer TappedTimer;
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
                TappedTimerUpMotionEvent = upEvent;
                _tappedTimerNumberOfTaps = taps;
                TappedTimer = new System.Timers.Timer(Settings.TappedThreshold.TotalMilliseconds);
                TappedTimer.Elapsed += OnTappedTimerElapsed;
                TappedTimer.Start();
            }
        }

        void OnTappedTimerElapsed(object sender, ElapsedEventArgs e)
        {
            TappedTimerStop();
            _numberOfTaps = 0;
            if (!_panning && !_longPressed && !_pinching && !_rotating)
                OnTappedTimerElapsedInner();
        }

        void OnTappedTimerElapsedInner()
        {
            if (P42.Utils.Environment.IsOnMainThread)
            {
                try
                {
                    if (!RendererDisposed)
                    {
                        var args = new AndroidTapEventArgs(_tappedTimerUpMotionEvent, _view, _tappedTimerNumberOfTaps, _viewLocationAtOnDown);
                        var handler = NativeGestureHandler.InstanceForElement(Element);
                        while (handler != null)
                        {
                            var listener = handler.Listener;
                            if (listener != null && listener.HandlesTapped)
                            {
                                args.Listener = listener;
                                //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
                                listener.OnTapped(args);
                                if (args.Handled)
                                    break;
                            }
                            handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                        }
                    }
                }
                catch (Exception) { }
            }
            else
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => OnTappedTimerElapsedInner());
        }
        #endregion


        #region Cancelation
        public void Cancel(MotionEvent e)
        {

            StopTapLongPress();

            CallOnUp(e);
            if (_panning)
                CallOnPanned(e);
            _panning = false;
            _pinching = false;
            _rotating = false;
            _multiMoving = false;

        }

        void StopTapLongPress()
        {
            TappedTimerStop();
            LongPressingTimerStop();
            _numberOfTaps = 0;
        }
        #endregion


        #region Down / Up / 'ed events
        int _downFires;
        DateTime _onDownDateTime = DateTime.MinValue;
        public override bool OnDown(MotionEvent e)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("OnDown [{0}]",_id);
            //System.Diagnostics.Debug.WriteLine("NativeGestureListener." + P42.Utils.ReflectionExtensions.CallerMemberName() + " Index:" + e);
            //System.Diagnostics.Debug.WriteLine("NativeGestureListener.OnDown ENTER  Element[" + Element + "]");

            if (e.Action != MotionEventActions.Down)
            {
                //System.Diagnostics.Debug.WriteLine("NativeGestureListener.OnDown invalid e.Action [" + e.Action + "]");
                return false;
            }

            //System.Diagnostics.Debug.WriteLine(GetType() + ".OnDown fires=" + _downFires);
            _downFires++;

            _onDownDateTime = DateTime.Now;
            Start = e;
            _panning = false;
            _pinching = false;
            _rotating = false;
            _multiMoving = false;
            TappedTimerStop();
            LongPressTimerStart();
            // TODO: Make sure _vliewLocaionAtDown is property translated into Listener.Element coordinates
            _view?.GetLocationInWindow(_viewLocationAtOnDown);
            var args = new AndroidDownUpEventArgs(e, _view, _viewLocationAtOnDown);
            var handler = NativeGestureHandler.InstanceForElement(Element);
            while (handler != null)
            {
                var listener = handler.Listener;
                if (listener != null && listener.HandlesDown)
                {
                    args.Listener = listener;
                    args.Handled = true;
                    listener.OnDown(args);
                    //if (!args.Handled)
                    //    return false;
                    if (args.Handled)
                        return true;
                }
                handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
            }
            //return handled; // we are going to (in NativeGestureDetector) always capture a DOWN touch event so we can receive all updates to this gesture
            return true;
        }

        int _upFires;
        int _numberOfTaps;
        public bool OnUp(MotionEvent ev)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) );
            //System.Diagnostics.Debug.WriteLine("NativeGestureListener." + P42.Utils.ReflectionExtensions.CallerMemberName() + " action:" + ev.Action + " index" + ev.ActionIndex + " e:" + ev);
            //System.Diagnostics.Debug.WriteLine("NativeGestureListener.OnUp ENTER Element[" + Element + "]");
            //if (ev.Action == MotionEventActions.Down)
            //    System.Diagnostics.Debug.WriteLine("");
            if (ev.Action != MotionEventActions.Up)
            {
                //System.Diagnostics.Debug.WriteLine("NativeGestureListener.OnUp invalid e.Action [" + ev.Action + "]");
                return false;
            }

            //var touchDuration = DateTime.Now - _onDownDateTime;

            _numberOfTaps++;
            LongPressingTimerStop();
            TappedTimerStart(ev, _numberOfTaps);

            //System.Diagnostics.Debug.WriteLine("NativeGestureListener.OnUp e.Action [" + ev.Action + "]");
            //System.Diagnostics.Debug.WriteLine(GetType() + ".OnUp fires=" + _upFires);
            _upFires++;

            bool handled = false;
            {
                handled = CallOnUp(ev);

                if (_panning)
                    handled = CallOnPanned(ev);
                else if (_longPressed)
                {
                    LongPressEventArgs args = new AndroidLongPressEventArgs(Start, ev, _view, _viewLocationAtOnDown);
                    var handler = NativeGestureHandler.InstanceForElement(Element);
                    while (handler != null)
                    {
                        var listener = handler.Listener;
                        if (listener != null && listener.HandlesLongPressed)
                        {
                            args.Listener = listener;
                            listener.OnLongPressed(args);
                            handled = handled || args.Handled;
                            if (args.Handled)
                                break;
                        }
                        handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                    }
                }
                else if (!_pinching && !_rotating)
                {
                    {
                        TapEventArgs args = new AndroidTapEventArgs(ev, _view, _numberOfTaps, _viewLocationAtOnDown);
                        var handler = NativeGestureHandler.InstanceForElement(Element);
                        while (handler != null)
                        {
                            var listener = handler.Listener;
                            if (listener != null && listener.HandlesTapping)
                            {
                                args.Listener = listener;
                                listener.OnTapping(args);
                                handled = handled || args.Handled;
                                if (args.Handled)
                                    break;
                            }
                            handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                        }
                    }
                    if (_numberOfTaps % 2 == 0)
                    {
                        TapEventArgs args = new AndroidTapEventArgs(ev, _view, _numberOfTaps, _viewLocationAtOnDown);
                        var handler = NativeGestureHandler.InstanceForElement(Element);
                        while (handler != null)
                        {
                            var listener = handler.Listener;
                            if (listener != null && listener.HandlesDoubleTapped)
                            {
                                args.Listener = listener;
                                listener.OnDoubleTapped(args);
                                handled = handled || args.Handled;
                                if (args.Handled)
                                    break;
                            }
                            handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                        }
                    }
                }
            }

            LastPanArgs = null;
            LastPan = null;

            return handled;
        }

        bool CallOnUp(MotionEvent ev)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
            //System.Diagnostics.Debug.WriteLine(GetType() + ".CallOnUp");
            var handled = false;
            DownUpEventArgs args = new AndroidDownUpEventArgs(ev, _view, _viewLocationAtOnDown);
            var handler = NativeGestureHandler.InstanceForElement(Element);
            while (handler != null)
            {
                var listener = handler.Listener;
                if (listener != null && listener.HandlesUp)
                {
                    args.Listener = listener;
                    listener.OnUp(args);
                    handled = handled || args.Handled;
                    if (args.Handled)
                        break;
                }
                handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
            }
            return handled;
        }

        bool CallOnPanned(MotionEvent ev)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + ".CallOnPanned");
            var handled = false;
            _numberOfTaps = 0;
            PanEventArgs args = new AndroidPanEventArgs(LastPan ?? Start, ev, LastPanArgs, _view, _viewLocationAtOnDown);
            var handler = NativeGestureHandler.InstanceForElement(Element);
            while (handler != null)
            {
                var listener = handler.Listener;
                if (listener != null && listener.HandlesPanned)
                {
                    args.Listener = listener;
                    listener.OnPanned(args);
                    handled = handled || args.Handled;
                    if (args.Handled)
                        break;
                }
                handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
            }
            return handled;
        }
        #endregion


        #region Pan / Fling
        public override bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("OnScroll ["+eN.Action+"]["+_id+"] e1=["+e0+"] e2=["+eN+"]");
            //System.Diagnostics.Debug.WriteLine("NativeGestureListener.OnScroll ENTER  Element[" + Element + "]");
            bool handled = false;
            StopTapLongPress();
            if (!_multiMoving)
            {
                _panning = true;
                PanEventArgs args = new AndroidPanEventArgs(LastPan ?? Start, e2, LastPanArgs, _view, _viewLocationAtOnDown);
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    var listener = handler.Listener;
                    if (listener != null && (listener.HandlesPanning || listener.HandlesPanned))
                    {
                        args.Listener = listener;
                        //System.Diagnostics.Debug.WriteLine("~~~ listener.Element=[" + listener.Element + "]");
                        listener.OnPanning(args);
                        LastPanArgs = args;
                        LastPan = e2;
                        handled = handled || args.Handled;
                        if (args.Handled)
                            return true;
                    }
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
            }
            return handled;
        }

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            //System.Diagnostics.Debug.WriteLine("NativeGestureListener.OnFling ENTER Element[" + Element + "]");
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("OnFling [{0}]",_id);
            StopTapLongPress();

            bool handled = false;
            double hzVel = Math.Abs(velocityX);
            double vtVel = Math.Abs(velocityY);
            Direction direction = Direction.NotClear;
            if (hzVel > 2 * vtVel)
                direction = ((velocityX > 0f) ? Direction.Right : Direction.Left);
            else if (vtVel > 2 * hzVel)
                direction = ((velocityY > 0f) ? Direction.Down : Direction.Up);
            SwipeEventArgs args = new AndroidSwipeEventArgs(e2, _view, direction, _viewLocationAtOnDown);
            var handler = NativeGestureHandler.InstanceForElement(Element);
            while (handler != null)
            {
                var listener = handler.Listener;
                if (listener != null && listener.HandlesSwiped)
                {
                    args.Listener = listener;
                    listener.OnSwiped(args);
                    handled = handled || args.Handled;
                    if (args.Handled)
                        return true;
                }
                handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
            }
            // OnFling overrides OnUp??
            //System.Diagnostics.Debug.WriteLine(GetType() + ".OnFling calling OnUp");
            //OnUp(e2);
            if (_panning)
                CallOnPanned(e2);
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
            get => _multiStart;
            set
            {
                _multiStart?.Recycle();
                _multiStart = (value != null) ? MotionEvent.Obtain(value) : null;
            }
        }

        MotionEvent.PointerCoords[] _startCoords;
        public bool OnMultiDown(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            //System.Diagnostics.Debug.WriteLine("NativeGestureListeners.OnMultiDown ENTER");
            StopTapLongPress();
            MultiStart = ev;
            _multiMoving = false;
            //_lastCoords = coords;
            _startCoords = coords;
            return false;
        }

        public bool OnMultiMove(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            //System.Diagnostics.Debug.WriteLine("NativeGestureListeners.OnMultiMove ENTER");
            StopTapLongPress();
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
                        _multiMoving = _multiMoving || (xDist > _touchSlop || yDist > _touchSlop);
                    }
                    else
                        _multiInBounds = false;
                }
            }
            bool handled = false;
            if (_multiMoving && _multiInBounds)
            {
                handled = handled || OnPinching(ev, coords);
                handled = handled || OnRotating(ev, coords);
            }
            return handled;
        }

        public bool OnMultiUp(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            //System.Diagnostics.Debug.WriteLine("NativeGestureListeners.OnMultiUp ENTER");
            bool handled = false;
            if (_multiMoving)
            {
                StopTapLongPress();
                handled = handled || OnPinched(ev, coords);
                handled = handled || OnRotated(ev, coords);
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
            bool handled = false;
            {
                PinchEventArgs pinchArgs = new AndroidPinchEventArgs(ev, coords, _previousPinchArgs, _view, _viewLocationAtOnDown);
                var args = new PinchEventArgs(pinchArgs);
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    var listener = handler.Listener;
                    if (listener != null && listener.HandlesPinching)
                    {
                        args.Listener = listener;
                        listener.OnPinching(args);
                        handled = handled || args.Handled;
                        //if (args.Handled)
                        //	break;
                    }
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
                _previousPinchArgs = pinchArgs;
            }
            return handled;
        }

        bool OnRotating(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine("onRotating [{0}]", _id);
            StopTapLongPress();
            _rotating = true;
            bool handled = false;
            {
                RotateEventArgs rotateArgs = new AndroidRotateEventArgs(ev, coords, _previousRotateArgs, _view, _viewLocationAtOnDown);
                var args = new RotateEventArgs(rotateArgs);
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    var listener = handler.Listener;
                    if (listener != null && listener.HandlesRotating)
                    {
                        args.Listener = listener;
                        listener.OnRotating(args);
                        handled = handled || args.Handled;
                        //if (args.Handled)
                        //	break;
                    }
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
                _previousRotateArgs = rotateArgs;
            }
            return handled;
        }


        bool OnPinched(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine("onPinched [{0}]", _id);
            StopTapLongPress();
            _pinching = true;
            bool handled = false;
            {
                if (_previousPinchArgs != null)
                {
                    PinchEventArgs args = new AndroidPinchEventArgs(ev, coords, _previousPinchArgs, _view, _viewLocationAtOnDown);
                    var handler = NativeGestureHandler.InstanceForElement(Element);
                    while (handler != null)
                    {
                        var listener = handler.Listener;
                        if (listener != null && (listener.HandlesPinching || listener.HandlesPinched))
                        {
                            args.Listener = listener;
                            listener.OnPinched(args);
                            handled = handled || args.Handled;
                            //if (args.Handled)
                            //	break;
                        }
                        handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                    }
                    _previousPinchArgs = null;
                }
            }
            return handled;
        }

        bool OnRotated(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine("onRotated [{0}]", _id);
            StopTapLongPress();
            _rotating = true;
            bool handled = false;
            {
                if (_previousRotateArgs != null)
                {
                    RotateEventArgs args = new AndroidRotateEventArgs(ev, coords, _previousRotateArgs, _view, _viewLocationAtOnDown);
                    var handler = NativeGestureHandler.InstanceForElement(Element);
                    while (handler != null)
                    {
                        var listener = handler.Listener;
                        if (listener != null && (listener.HandlesRotating || listener.HandlesRotated))
                        {
                            args.Listener = listener;
                            listener.OnRotated(args);
                            handled = handled || args.Handled;
                            //if (args.Handled)
                            //	break;
                        }
                        handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                    }
                    _previousRotateArgs = null;
                }
            }
            return handled;
        }

        #endregion
    }
}
