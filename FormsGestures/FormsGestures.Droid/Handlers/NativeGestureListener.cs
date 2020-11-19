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

        int _numberOfTaps;

        System.Timers.Timer TappedTimer;
        int _tappedTimerNumberOfTaps;

        System.Timers.Timer LongPressTimer;

        PanEventArgs _lastPanArgs;

        DownUpEventArgs _downUpEventArgs;
        #endregion


        #region Properties
        bool HandlesTest(Func<Listener, bool> test)
        {
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    if (handler.Listener != null && test(handler.Listener))
                        return true;
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
                return false;
        }
        public bool HandlesPressTapEvents => HandlesTapped || HandlesDoubleTapped || HandlesLongPresses;

        public bool HandlesTapped 
            => HandlesTest(listener => listener.HandlesTapped);

        public bool HandlesDoubleTapped 
            => HandlesTest(listener => listener.HandlesDoubleTapped);

        public bool HandlesPan 
            => HandlesTest(listener => listener.HandlesPanned || listener.HandlesPanning);

        public bool HandlesMove 
            => HandlesTest(listener => listener.HandlesPanned || listener.HandlesPanning || listener.HandlesPinched || listener.HandlesPinching || listener.HandlesSwiped || listener.HandlesRotated || listener.HandlesRotating);

        public bool HandlesSwiped
             => HandlesTest(listener => listener.HandlesSwiped);

        public bool HandlesLongPresses
             => HandlesTest(listener => listener.HandlesLongPressed || listener.HandlesLongPressing);

        bool RendererDisposed => Element==null || Xamarin.Forms.Platform.Android.Platform.GetRenderer(Element as VisualElement) == null;

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
                }
                else
                {
                    _start = null;
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
            P42.Utils.DebugExtensions.AddToCensus(this);

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
                _tappedTimerUpMotionEvent?.Dispose();
                LongPressTimer?.Dispose();
                TappedTimer?.Dispose();
                P42.Utils.DebugExtensions.RemoveFromCensus(this);
            }
            base.Dispose(disposing);
        }
        #endregion


        #region LongPress Timer
        void LongPressingTimerStop()
        {
            LongPressTimer?.Stop();
            if (LongPressTimer != null)
                LongPressTimer.Elapsed -= OnLongPressTimerElapsed;
            LongPressTimer?.Dispose();
            LongPressTimer = null;
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
                        var handler = NativeGestureHandler.InstanceForElement(Element);
                        while (handler != null)
                        {
                            var listener = handler.Listener;
                            if (listener != null && listener.HandlesLongPressing)
                            {
                                var args = new AndroidLongPressEventArgs(Start, null, _view, listener);
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
        void TappedTimerStop()
        {
            if (TappedTimer != null)
            {
                TappedTimer.Stop();
                TappedTimer.Elapsed -= OnTappedTimerElapsed;
                TappedTimer.Dispose();
                TappedTimer = null;
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
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (!RendererDisposed)
                    {
                        var handler = NativeGestureHandler.InstanceForElement(Element);
                        while (handler != null)
                        {
                            var listener = handler.Listener;
                            if (listener != null && listener.HandlesTapped)
                            {
                                var args = new AndroidTapEventArgs(_tappedTimerUpMotionEvent, _view, _tappedTimerNumberOfTaps, listener);
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
            });
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
            _downUpEventArgs = null;
        }

        void StopTapLongPress()
        {
            TappedTimerStop();
            LongPressingTimerStop();
            _numberOfTaps = 0;
        }
        #endregion


        #region Down / Up / 'ed events
        public override bool OnDown(MotionEvent e)
        {
            if (e.Action != MotionEventActions.Down)
                return false;

            Start = e;
            _panning = false;
            _pinching = false;
            _rotating = false;
            _multiMoving = false;
            TappedTimerStop();
            LongPressTimerStart();

            var handler = NativeGestureHandler.InstanceForElement(Element);
            while (handler != null)
            {
                var listener = handler.Listener;
                if (listener != null && listener.HandlesDown)
                {
                    var args = new AndroidDownUpEventArgs(e, _view, listener);
                    _downUpEventArgs = args;
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

        public bool OnUp(MotionEvent ev)
        {
            if (ev.Action != MotionEventActions.Up)
                return false;

            _numberOfTaps++;
            LongPressingTimerStop();
            TappedTimerStart(ev, _numberOfTaps);

            var handled = CallOnUp(ev);

            if (_panning)
            {
                handled |= CallOnPanned(ev);
            }
            else if (_longPressed)
            {
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    var listener = handler.Listener;
                    if (listener != null && listener.HandlesLongPressed)
                    {
                        LongPressEventArgs args = new AndroidLongPressEventArgs(Start, ev, _view, listener);
                        listener.OnLongPressed(args);
                        handled |= args.Handled;
                        if (args.Handled)
                            break;
                    }
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
            }
            else if (!_pinching && !_rotating)
            {
                // Tapping
                {
                    var handler = NativeGestureHandler.InstanceForElement(Element);
                    while (handler != null)
                    {
                        var listener = handler.Listener;
                        if (listener != null && listener.HandlesTapping)
                        {
                            TapEventArgs args = new AndroidTapEventArgs(ev, _view, _numberOfTaps, listener);
                            listener.OnTapping(args);
                            handled = handled || args.Handled;
                            if (args.Handled)
                                break;
                        }
                        handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                    }
                }
                // Double Tapped
                if (_numberOfTaps % 2 == 0)
                {
                    var handler = NativeGestureHandler.InstanceForElement(Element);
                    while (handler != null)
                    {
                        var listener = handler.Listener;
                        if (listener != null && listener.HandlesDoubleTapped)
                        {
                            TapEventArgs args = new AndroidTapEventArgs(ev, _view, _numberOfTaps, listener);
                            listener.OnDoubleTapped(args);
                            handled = handled || args.Handled;
                            if (args.Handled)
                                break;
                        }
                        handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                    }
                }
            }

            _lastPanArgs = null;
            LastPan = null;
            _downUpEventArgs = null;
            return handled;
        }

        bool CallOnUp(MotionEvent ev)
        {
            var handled = false;
            var handler = NativeGestureHandler.InstanceForElement(Element);
            while (handler != null)
            {
                var listener = handler.Listener;
                if (listener != null && listener.HandlesUp)
                {
                    DownUpEventArgs args = new AndroidDownUpEventArgs(ev, _view, listener);
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
            var handler = NativeGestureHandler.InstanceForElement(Element);
            while (handler != null)
            {
                var listener = handler.Listener;
                if (listener != null && listener.HandlesPanned)
                {
                    PanEventArgs args = new AndroidPanEventArgs(LastPan ?? Start, ev, (BaseGestureEventArgs)_lastPanArgs ?? _downUpEventArgs, _view, listener);
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
            StopTapLongPress();
            if (!_multiMoving)
            {
                _panning = true;
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    var listener = handler.Listener;
                    if (listener != null && (listener.HandlesPanning || listener.HandlesPanned))
                    {
                        PanEventArgs args = new AndroidPanEventArgs(LastPan ?? Start, e2, (BaseGestureEventArgs) _lastPanArgs ?? _downUpEventArgs, _view, listener);
                        listener.OnPanning(args);
                        _lastPanArgs = args;
                        LastPan = e2;
                        if (args.Handled)
                            return true;
                    }
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
            }
            return false;
        }

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            StopTapLongPress();

            double hzVel = Math.Abs(velocityX);
            double vtVel = Math.Abs(velocityY);
            Direction direction = Direction.NotClear;
            if (hzVel > 2 * vtVel)
                direction = ((velocityX > 0f) ? Direction.Right : Direction.Left);
            else if (vtVel > 2 * hzVel)
                direction = ((velocityY > 0f) ? Direction.Down : Direction.Up);
            var handler = NativeGestureHandler.InstanceForElement(Element);
            while (handler != null)
            {
                var listener = handler.Listener;
                if (listener != null && listener.HandlesSwiped)
                {
                    SwipeEventArgs args = new AndroidSwipeEventArgs(e2, _view, direction, listener);
                    listener.OnSwiped(args);
                    if (args.Handled)
                        return true;
                }
                handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
            }
            if (_panning)
                CallOnPanned(e2);
            return false;
        }
        #endregion


        #region Multi-point gestures
        Dictionary<Listener, PinchEventArgs> _previousPinchArgs = new Dictionary<Listener, PinchEventArgs>();
        Dictionary<Listener, RotateEventArgs> _previousRotateArgs = new Dictionary<Listener, RotateEventArgs>();

        bool _multiMoving;
        bool _multiInBounds;

        protected readonly int _touchSlop;

        MotionEvent.PointerCoords[] _startCoords;
        public bool OnMultiDown(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            StopTapLongPress();
            _multiMoving = false;
            _startCoords = coords;
            return false;
        }

        public bool OnMultiMove(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            StopTapLongPress();
            if (_view != null && !_multiMoving && (coords?.Length > 1 || false) && (_startCoords?.Length > 1 || false) )
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
            bool handled = false;
            if (_multiMoving)
            {
                StopTapLongPress();
                handled = handled || OnPinched(ev, coords);
                handled = handled || OnRotated(ev, coords);
                _multiMoving = false;
            }
            return handled;
        }

        bool OnPinching(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            StopTapLongPress();
            _pinching = true;
            bool handled = false;
            
            var handler = NativeGestureHandler.InstanceForElement(Element);
            while (handler != null)
            {
                if (handler.Listener is Listener listener)
                {
                    var previousArgs = _previousPinchArgs.ContainsKey(listener)
                        ? _previousPinchArgs[listener]
                        : null;
                    PinchEventArgs args = new AndroidPinchEventArgs(ev, coords, previousArgs, _view, listener);
                    if (listener != null && listener.HandlesPinching)
                    {
                        listener.OnPinching(args);
                        handled = handled || args.Handled;
                        //if (args.Handled)
                        //	break;
                        _previousPinchArgs[listener] = new PinchEventArgs(args, listener);
                    }
                }
                handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
            }
            
            return handled;
        }

        bool OnRotating(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            StopTapLongPress();
            _rotating = true;
            bool handled = false;
            
            var handler = NativeGestureHandler.InstanceForElement(Element);
            while (handler != null)
            {
                if (handler.Listener is Listener listener)
                {
                    var previousArgs = _previousRotateArgs.ContainsKey(listener)
                    ? _previousRotateArgs[listener]
                    : null;
                    RotateEventArgs args = new AndroidRotateEventArgs(ev, coords, previousArgs, _view, listener);
                    if (listener != null && listener.HandlesRotating)
                    {
                        listener.OnRotating(args);
                        handled = handled || args.Handled;
                        //if (args.Handled)
                        //	break;
                        _previousRotateArgs[listener] = new RotateEventArgs(args, listener);
                    }
                }
                handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
            }
            
            return handled;
        }

        bool OnPinched(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            StopTapLongPress();
            _pinching = true;
            bool handled = false;
            
            if (_previousPinchArgs != null)
            {
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    if (handler.Listener is Listener listener)
                    {
                        var previousArgs = _previousPinchArgs.ContainsKey(listener)
                        ? _previousPinchArgs[listener]
                        : null;
                        PinchEventArgs args = new AndroidPinchEventArgs(ev, coords, previousArgs, _view, listener);
                        if (listener != null && (listener.HandlesPinching || listener.HandlesPinched))
                        {
                            listener.OnPinched(args);
                            handled = handled || args.Handled;
                            //if (args.Handled)
                            //	break;
                            _previousPinchArgs[listener] = new PinchEventArgs(args, listener);
                        }
                    }
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
                _previousPinchArgs.Clear();
            }
            
            return handled;
        }

        bool OnRotated(MotionEvent ev, MotionEvent.PointerCoords[] coords)
        {
            StopTapLongPress();
            _rotating = true;
            bool handled = false;
            
            if (_previousRotateArgs != null)
            {
                var handler = NativeGestureHandler.InstanceForElement(Element);
                while (handler != null)
                {
                    if (handler.Listener is Listener listener)
                    {
                        var previousArgs = _previousRotateArgs.ContainsKey(listener)
                        ? _previousRotateArgs[listener]
                        : null;
                        RotateEventArgs args = new AndroidRotateEventArgs(ev, coords, previousArgs, _view, listener);
                        if (listener != null && (listener.HandlesRotating || listener.HandlesRotated))
                        {
                            listener.OnRotated(args);
                            handled = handled || args.Handled;
                            //if (args.Handled)
                            //	break;
                            _previousRotateArgs[listener] = new RotateEventArgs(args, listener);
                        }
                    }
                    handler = NativeGestureHandler.InstanceForElement(handler.Element?.Parent);
                }
                _previousRotateArgs.Clear();
            }
            
            return handled;
        }

        #endregion
    }
}
