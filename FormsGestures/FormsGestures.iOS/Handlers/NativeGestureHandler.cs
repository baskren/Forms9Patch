using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FormsGestures.iOS
{
    class NativeGestureHandler : Behavior<VisualElement>, IDisposable
    {
        static readonly BindableProperty GestureHandlerProperty = BindableProperty.Create("GestureHandler", typeof(NativeGestureHandler), typeof(NativeGestureHandler), null);

        #region Fields
        readonly VisualElement _element;

        UIView _view;

        UIGestureRecognizer[] _gestureRecognizers;

        int _numberOfTaps;

        Stopwatch _startPressing;

        PanEventArgs _lastPanArgs;

        bool _longPressing;
        bool _pinching;
        bool _rotating;
        bool _panning;

        PinchEventArgs _previousPinchArgs;

        RotateEventArgs _previousRotateArgs;

        List<Listener> _listeners = new List<Listener>();
        readonly List<UIGestureRecognizer> _toDispose = new List<UIGestureRecognizer>();
        #endregion


        #region Listener connections
        void AddListener(Listener listener)
        {
            if (!(_listeners.Contains(listener)))
            {
                _listeners.Add(listener);
                listener.Disposing += RemoveListener;
            }
        }

        void RemoveListener(object sender, EventArgs e)
        {
            var listener = sender as Listener;
            //if (_listeners!=null) {
            if (_listeners.Contains(listener))
            {
                listener.PropertyChanged -= OnListenerPropertyChanged;
                listener.Disposing -= RemoveListener;
                _listeners.Remove(listener);
            }
            if (_listeners.Count < 1)
            {
                // no one is listening so shut down
                _element.SetValue(GestureHandlerProperty, null);
                Dispose();
            }
            //}
        }
        #endregion


        #region Constructor / Factories / Disposer
        static NativeGestureHandler GetInstanceForElement(VisualElement element)
        {
            return (NativeGestureHandler)element.GetValue(GestureHandlerProperty);
        }

        static internal NativeGestureHandler GetInstanceForListener(Listener listener)
        {
            var iOSGestureHandler = GetInstanceForElement(listener.Element) ?? new NativeGestureHandler(listener.Element);
            iOSGestureHandler.AddListener(listener);
            return iOSGestureHandler;
        }

        NativeGestureHandler(VisualElement element)
        {
            P42.Utils.Debug.AddToCensus(this);
            _element = element;
            _element.Behaviors.Add(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                if (_gestureRecognizers != null)
                {
                    UIGestureRecognizer[] array = _gestureRecognizers;
                    foreach (var uIGestureRecognizer in array)
                    {
                        uIGestureRecognizer.View.RemoveGestureRecognizer(uIGestureRecognizer);
                        uIGestureRecognizer.Dispose();
                    }
                    _gestureRecognizers = null;
                }
                foreach (var uiGestureRecognizer in _toDispose)
                {
                    try
                    {
                        uiGestureRecognizer.Dispose();
                    }
                    catch (Exception) { }
                }
                _toDispose.Clear();
                if (_listeners != null)
                {
                    foreach (var listener in _listeners)
                        listener.PropertyChanged -= OnListenerPropertyChanged;
                    _listeners = null;
                }
                P42.Utils.Debug.RemoveFromCensus(this);
            }
        }

        bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "<Pending>")]
        internal void AttachNativeGestureHandler(Listener listener)
        {
            GetInstanceForListener(listener);
            listener.PropertyChanged += OnListenerPropertyChanged;
        }

        #endregion


        #region Renderer connections
        void RendererDisconnect()
        {
            ClearGestureRecognizers();
            IVisualElementRenderer renderer = Platform.GetRenderer(_element);
            if (renderer != null)
                renderer.ElementChanged -= OnRendererElementChanged;
        }

        void RendererConnect()
        {
            IVisualElementRenderer renderer = Platform.GetRenderer(_element);
            if (renderer != null)
            {
                renderer.ElementChanged += OnRendererElementChanged;
                ResetGestureRecognizers(renderer?.NativeView);
            }
        }
        #endregion


        #region Element/Property change handling
        void OnRendererElementChanged(object sender, ElementChangedEventArgs<VisualElement> e)
        {
            if (e.OldElement != null)
                RendererDisconnect();
        }

        static void OnListenerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var listener = sender as Listener;
            if (listener?.Element != null && Listener.AllEventsAndCommands.Contains(e.PropertyName))
            {
                NativeGestureHandler instance = NativeGestureHandler.GetInstanceForListener(listener);
                instance?.ResetGestureRecognizers(Platform.GetRenderer(listener.Element)?.NativeView);
            }
        }

        void OnElementPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            if (sender is VisualElement)
            {
                if (e.PropertyName == "Renderer")
                    RendererDisconnect();
                else if (e.PropertyName == GestureHandlerProperty.PropertyName)
                    _element.Behaviors.Remove(this);
            }
        }

        void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is VisualElement)
                if (e.PropertyName == "Renderer")
                    RendererConnect();
        }
        #endregion


        #region Element connections
        // only called when a iOSGestureHandler doesn't already exist for this VisualElement
        protected override void OnAttachedTo(VisualElement bindable)
        {
            bindable.SetValue(GestureHandlerProperty, this);
            bindable.PropertyChanging += OnElementPropertyChanging;
            bindable.PropertyChanged += OnElementPropertyChanged;
            RendererConnect();
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            // arguably, none of this is necessary since Element owns everything.
            // silence events
            RendererDisconnect();
            bindable.PropertyChanging -= OnElementPropertyChanging;
            bindable.PropertyChanged -= OnElementPropertyChanged;
            // cleanup listeners
            foreach (var listener in _listeners)
                listener.PropertyChanged -= OnListenerPropertyChanged;
            // cleanup properties
            //bindable.SetValue (Gesture_listenersProperty, null);
            //_listeners = null;
            bindable.SetValue(GestureHandlerProperty, null);
            base.OnDetachingFrom(bindable);
        }
        #endregion


        #region iOS Gesture Recognizers
        void ClearGestureRecognizers()
        {
            if (_gestureRecognizers != null)
            {
                UIGestureRecognizer[] array = _gestureRecognizers;
                foreach (var uIGestureRecognizer in array)
                {
                    uIGestureRecognizer.View.RemoveGestureRecognizer(uIGestureRecognizer);
                    // the following causes everything to freeze!
                    //uIGestureRecognizer.Dispose();
                    _toDispose.Add(uIGestureRecognizer);
                }
                _gestureRecognizers = null;
            }
        }

        void ResetGestureRecognizers(UIView view)
        {
            ClearGestureRecognizers();
            _view = view;
            if (_view != null)
            {
                _gestureRecognizers = CreateGestureRecognizers();
                UIGestureRecognizer[] array = _gestureRecognizers;
                foreach (var gestureRecognizer in array)
                    view.AddGestureRecognizer(gestureRecognizer);
            }
        }

        UIGestureRecognizer[] CreateGestureRecognizers()
        {
            var list = new List<UIGestureRecognizer>();
            //if (HandlesDownUps)  WE NEED TO ALWAYS MONITOR FOR DOWN TO BE ABLE TO UNDO A CANCEL 
            {  // commenting out if (HanglesDownUps) causes FormsDragNDrop listview to not recognize cell selections after a scroll.  Why?  I have no clue.
               // Let's always trigger the down gesture recognizer so we can get the starting location
                var downUpGestureRecognizer = new DownUpGestureRecognizer(new Action<DownUpGestureRecognizer, UITouch[]>(OnDown), new Action<DownUpGestureRecognizer, UITouch[]>(OnUp))
                {
                    ShouldRecognizeSimultaneously = ((thisGr, otherGr) => false),
                    ShouldReceiveTouch = ((UIGestureRecognizer gr, UITouch touch) => !(touch.View is UIControl))
                };
                list.Add(downUpGestureRecognizer);
                //downUpGestureRecognizer.ShouldReceiveTouch = (recognizer, touch) => {
                //	return _element.get_IgnoreChildrenTouches() ? touch.View==_view : true;
                //};
            }
            UILongPressGestureRecognizer uILongPressGestureRecognizer = null;
            if (HandlesLongs)
            {
                uILongPressGestureRecognizer = new UILongPressGestureRecognizer(new Action<UILongPressGestureRecognizer>(OnLongPressed))
                {
                    ShouldRecognizeSimultaneously = ((thisGr, otherGr) => false)
                };
                list.Add(uILongPressGestureRecognizer);
                //uILongPressGestureRecognizer.ShouldReceiveTouch = (recognizer, touch) => {
                //	return _element.get_IgnoreChildrenTouches() ? touch.View==_view : true;
                //};
            }
            if (HandlesTaps)
            {
                var uITapGestureRecognizer = new UITapGestureRecognizer(new Action<UITapGestureRecognizer>(OnTapped))
                {
                    ShouldRecognizeSimultaneously = ((thisGr, otherGr) =>
                    {
                        return thisGr.GetType() != otherGr.GetType();
                    }),
                    ShouldReceiveTouch = ((UIGestureRecognizer gr, UITouch touch) =>
                    {
                        // these are handled BEFORE the touch call is passed to the listener.
                        return !(touch.View is UIControl);
                        //return touch.View == gr.View;
                    })
                };
                if (uILongPressGestureRecognizer != null)
                    uITapGestureRecognizer.RequireGestureRecognizerToFail(uILongPressGestureRecognizer);
                list.Add(uITapGestureRecognizer);
                //uITapGestureRecognizer.ShouldReceiveTouch = (recognizer, touch) => {
                //	return _element.get_IgnoreChildrenTouches() ? touch.View==_view : true;
                //};
            }
            if (HandlesPans)
            {
                var uIPanGestureRecognizer = new UIPanGestureRecognizer(new Action<UIPanGestureRecognizer>(OnPanned))
                {
                    MinimumNumberOfTouches = 1,
                    ShouldRecognizeSimultaneously = ((thisGr, otherGr) => true)
                };
                list.Add(uIPanGestureRecognizer);
                //uIPanGestureRecognizer.ShouldReceiveTouch = (recognizer, touch) => {
                //	return _element.get_IgnoreChildrenTouches() ? touch.View==_view : true;
                //};
            }
            if (HandlesPinches)
            {
                var uIPinchGestureRecognizer = new UIPinchGestureRecognizer(new Action<UIPinchGestureRecognizer>(OnPinched))
                {
                    ShouldRecognizeSimultaneously = ((thisGr, otherGr) => true)
                };
                list.Add(uIPinchGestureRecognizer);
                //uIPinchGestureRecognizer.ShouldReceiveTouch = (recognizer, touch) => {
                //	return _element.get_IgnoreChildrenTouches() ? touch.View==_view : true;
                //};
            }
            if (HandlesRotates)
            {
                var uIRotationGestureRecognizer = new UIRotationGestureRecognizer(new Action<UIRotationGestureRecognizer>(OnRotated))
                {
                    ShouldRecognizeSimultaneously = ((thisGr, otherGr) => true)
                };
                list.Add(uIRotationGestureRecognizer);
                //uIRotationGestureRecognizer.ShouldReceiveTouch = (recognizer, touch) => {
                //	return _element.get_IgnoreChildrenTouches() ? touch.View==_view : true;
                //};
            }
            /*
			var control = (UIView)_view.GetPropertyValue ("Control");
			if (control is UIButton)
				control.UserInteractionEnabled = !(HandlesTaps || HandlesDownUps);
				*/
            return list.ToArray();
        }
        #endregion


        #region Handles? properties
        bool HandlesDownUps
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesDown || listener.HandlesUp)
                        return true;
                return false;
            }
        }

        bool HandlesLongs
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesLongPressing || listener.HandlesLongPressed)
                        return true;
                return false;
            }
        }

        bool HandlesTaps
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesTapping || listener.HandlesTapped || listener.HandlesDoubleTapped)
                        return true;
                return false;
            }
        }

        bool HandlesTapping
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesTapping)
                        return true;
                return false;
            }
        }

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

        bool HandlesPans
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesPanning || listener.HandlesPanned || listener.HandlesSwiped)
                        return true;
                return false;
            }
        }

        bool HandlesPinches
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesPinching || listener.HandlesPinched)
                        return true;
                return false;
            }
        }

        bool HandlesRotates
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesRotating || listener.HandlesRotated)
                        return true;
                return false;
            }
        }
        #endregion


        #region Touch events
        CoreGraphics.CGPoint ViewLocationInWindow(UIView view)
        {
            return view.ConvertPointToView(CoreGraphics.CGPoint.Empty, null);
        }

        int touchCount;
        CoreGraphics.CGPoint _viewLocationAtOnDown;
        void OnDown(DownUpGestureRecognizer gr, UITouch[] touchesBegan)
        {
            _panning = false;
            if (!_element.IsVisible)
                return;
            /*
			var control = (UIView)_view.GetPropertyValue ("Control");
			if (control is UIButton)
				((UIButton)control).SendActionForControlEvents (UIControlEvent.TouchDown);
				*/
            bool handled = false;
            if (touchCount == 0)
            {
                //System.Diagnostics.Debug.WriteLine("onDown set _viewLocationAtOnDown");
                _viewLocationAtOnDown = ViewLocationInWindow(gr.View);
            }
            touchCount++;
            //System.Diagnostics.Debug.WriteLine("\tNativeGestureHandler.onDown _touchCount=["+_touchCount+"] _viewLocation=["+_viewLocationAtOnDown+"]");
            foreach (var listener in _listeners)
            {
                //if (handled)
                //	break;
                if (listener.HandlesDown)
                {
                    DownUpEventArgs args = new iOSDownUpEventArgs(gr, touchesBegan, _viewLocationAtOnDown)
                    {
                        Listener = listener
                    };
                    listener.OnDown(args);
                    handled = handled || args.Handled;
                    if (handled)
                        break;
                }
            }
            //gr.CancelsTouchesInView = handled;
            //System.Diagnostics.Debug.WriteLine("\thandled=["+handled+"]");
        }

        void OnUp(DownUpGestureRecognizer gr, UITouch[] touchesEnded)
        {

            if (!_element.IsVisible)
                return;
            /*
			var control = (UIView)_view.GetPropertyValue ("Control");
			if (control is UIButton)
				((UIButton)control).SendActionForControlEvents (UIControlEvent.TouchUpInside);
				*/
            bool handled = false;
            foreach (var listener in _listeners)
            {
                //if (handled)
                //	break;
                if (listener.HandlesUp)
                {
                    DownUpEventArgs args = new iOSDownUpEventArgs(gr, touchesEnded, _viewLocationAtOnDown)
                    {
                        Listener = listener
                    };
                    listener.OnUp(args);
                    handled = handled || args.Handled;
                    if (handled)
                        break;
                }
            }
            touchCount--;
            //System.Diagnostics.Debug.WriteLine("onUp touchCount-- = ["+_touchCount+"] touchesEnded.Length=["+touchesEnded.Length+"]");
            //System.Diagnostics.Debug.WriteLine("\tNativeGestureHandler.onUp _touchCount=[" + _touchCount + "] _viewLocation=[" + _viewLocationAtOnDown + "]");
            //gr.CancelsTouchesInView = handled;
            //System.Diagnostics.Debug.WriteLine("\thandled=[" + handled + "]");
        }

        bool _waitingForTapsToFinish;
        DateTime _lastTap;
        TapEventArgs _lastTapEventArgs;
        void OnTapped(UITapGestureRecognizer gr)
        {
            if (!_element.IsVisible)
                return;

            if (touchCount == 0)
                _viewLocationAtOnDown = ViewLocationInWindow(gr.View);

            _numberOfTaps++;
            _lastTap = DateTime.Now;
            _lastTapEventArgs = new iOSTapEventArgs(gr, _numberOfTaps, _viewLocationAtOnDown);
            bool tappingHandled = false;
            bool doubleTappedHandled = false;
            foreach (var listener in _listeners)
            {
                if (!tappingHandled && listener.HandlesTapping)
                {
                    var taskArgs = new TapEventArgs(_lastTapEventArgs, listener);
                    listener.OnTapping(taskArgs);
                    tappingHandled = taskArgs.Handled;
                }
                if (!doubleTappedHandled && _numberOfTaps % 2 == 0 && listener.HandlesDoubleTapped)
                {
                    var taskArgs = new TapEventArgs(_lastTapEventArgs, listener);
                    listener.OnDoubleTapped(taskArgs);
                    doubleTappedHandled = taskArgs.Handled;
                }
                if (tappingHandled && doubleTappedHandled)
                    break;
            }

            if (!_waitingForTapsToFinish && HandlesTapped)
            {
                _waitingForTapsToFinish = true;
                Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                {
                    if (DateTime.Now - _lastTap < Settings.TappedThreshold)
                        return true;
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        if (_listeners == null)
                        {
                            _numberOfTaps = 0;
                            _waitingForTapsToFinish = false;
                            return;
                        }

                        bool handled = false;
                        foreach (var listener in _listeners)
                        {
                            if (listener.HandlesTapped)
                            {
                                var taskArgs = new TapEventArgs(_lastTapEventArgs, listener);
                                listener.OnTapped(taskArgs);
                                handled = taskArgs.Handled;
                                if (handled)
                                    break;
                            }
                        }
                        _numberOfTaps = 0;
                        _waitingForTapsToFinish = false;
                    });
                    return false;
                });
            }
        }

        void OnLongPressed(UILongPressGestureRecognizer gr)
        {
            if (_panning)
                return;
            if (!_element.IsVisible)
                return;
            if (touchCount == 0)
            {
                //System.Diagnostics.Debug.WriteLine("onLongPressed set _viewLocationAtOnDown");
                _viewLocationAtOnDown = ViewLocationInWindow(gr.View);
            }
            if (gr.State == UIGestureRecognizerState.Ended || gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed)
            {
                _longPressing = false;
                if (_startPressing != null)
                {
                    _startPressing.Stop();
                    bool handled = false;
                    foreach (var listener in _listeners)
                    {
                        //if (handled)
                        //	break;
                        if (listener.HandlesLongPressed)
                        {
                            LongPressEventArgs args = new iOSLongPressEventArgs(gr, _startPressing.ElapsedMilliseconds, _viewLocationAtOnDown)
                            {
                                Listener = listener
                            };
                            listener.OnLongPressed(args);
                            handled = handled || args.Handled;
                            if (handled)
                                break;
                        }
                    }
                    //gr.CancelsTouchesInView = handled;
                    //if (handled)
                    //	return;
                }
            }
            else if (!_longPressing)
            {
                _longPressing = true;
                if (_startPressing == null)
                    _startPressing = new Stopwatch();
                else
                    _startPressing.Reset();
                _startPressing.Start();
                bool handled = false;
                foreach (var listener in _listeners)
                {
                    //if (handled)
                    //	break;
                    if (listener.HandlesLongPressing)
                    {
                        LongPressEventArgs args = new iOSLongPressEventArgs(gr, 0L, _viewLocationAtOnDown)
                        {
                            Listener = listener
                        };
                        listener.OnLongPressing(args);
                        handled = handled || args.Handled;
                        if (handled)
                            break;
                    }
                }
                //gr.CancelsTouchesInView = handled;
            }
        }

        void OnPanned(UIPanGestureRecognizer gr)
        {
            if (!_element.IsVisible)
                return;
            if (touchCount == 0)
            {
                //System.Diagnostics.Debug.WriteLine("new _viewLocationAtOnDown");
                //System.Diagnostics.Debug.WriteLine("onPanned set _viewLocationAtOnDown");
                _viewLocationAtOnDown = ViewLocationInWindow(gr.View);
                touchCount++;
            }
            //System.Diagnostics.Debug.WriteLine("NativeGestureHandler.onPanned");
            PanEventArgs panEventArgs = new iOSPanEventArgs(gr, _lastPanArgs, _viewLocationAtOnDown);
            if (gr.State == UIGestureRecognizerState.Ended || gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed)
            {
                touchCount = 0;
                //System.Diagnostics.Debug.WriteLine("\tonPanned _touchCount=0 = [" + _touchCount + "]");
                //System.Diagnostics.Debug.WriteLine("\tend");
                if (_lastPanArgs == null)
                    return;
                _lastPanArgs = null;
                _pinching = false;
                _rotating = false;
                _panning = false;
                _longPressing = false;
                bool handled = false;
                foreach (var listener in _listeners)
                {
                    //if (handled)
                    //	break;
                    if (listener.HandlesSwiped)
                    {
                        Point velocity = panEventArgs.Velocity;
                        bool xTriggered = Math.Abs(velocity.X) > Settings.SwipeVelocityThreshold.X;
                        bool yTriggered = Math.Abs(velocity.Y) > Settings.SwipeVelocityThreshold.Y;
                        if (xTriggered || yTriggered)
                        {
                            Direction direction = Direction.NotClear;
                            if (!yTriggered)
                                direction = ((velocity.X > 0.0) ? Direction.Right : Direction.Left);
                            else if (!xTriggered)
                                direction = ((velocity.Y > 0.0) ? Direction.Down : Direction.Up);
                            SwipeEventArgs args = new iOSSwipeEventArgs(gr, direction, _viewLocationAtOnDown)
                            {
                                Listener = listener,
                                VelocityX = velocity.X,
                                VelocityY = velocity.Y
                            };
                            listener.OnSwiped(args);
                            //gr.CancelsTouchesInView = swipeEventArgs.Handled;
                            //return;
                            handled = handled || args.Handled;
                            if (handled)
                                break;
                        }
                    }
                }
                //gr.CancelsTouchesInView = handled;
                //if (handled)
                //	return;
                foreach (var listener in _listeners)
                {
                    //if (handled)
                    //	break;
                    if (listener.HandlesPanning || listener.HandlesPanned)
                    {
                        var taskArgs = new PanEventArgs(panEventArgs)
                        {
                            Listener = listener
                        };
                        listener.OnPanned(taskArgs);
                        //gr.CancelsTouchesInView = panEventArgs.Handled;
                        //return;
                        handled = handled || taskArgs.Handled;
                        if (handled)
                            break;
                    }
                }
                //gr.CancelsTouchesInView = handled;
                //if (handled)
                //	return;
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine("\tmove");
                _panning = true;
                bool handled = false;
                foreach (var listener in _listeners)
                {
                    //System.Diagnostics.Debug.WriteLine("\tlistener.handlesPanning=["+listener.HandlesPanning+"]");
                    //if (handled)
                    //	break;
                    if (!panEventArgs.Equals(_lastPanArgs) && listener.HandlesPanning)
                    {
                        var taskArgs = new PanEventArgs(panEventArgs)
                        {
                            Listener = listener
                        };
                        listener.OnPanning(taskArgs);
                        handled = handled || taskArgs.Handled;
                        if (handled)
                            break;
                    }
                }
                //gr.CancelsTouchesInView = handled;
                _lastPanArgs = panEventArgs;
                //System.Diagnostics.Debug.WriteLine("_viewLocationAtOnDown ["+_viewLocationAtOnDown+"]");
            }
        }

        void OnPinched(UIGestureRecognizer gr)
        {
            if (!_element.IsVisible)
                return;
            if (!_rotating)
            {
                _pinching = true;
                OnPinchAndRotate(gr);
            }
        }

        void OnRotated(UIGestureRecognizer gr)
        {
            if (!_element.IsVisible)
                return;
            if (!_pinching)
            {
                _rotating = true;
                OnPinchAndRotate(gr);
            }
        }

        void OnPinchAndRotate(UIGestureRecognizer gr)
        {
            PinchEventArgs pinchEventArgs = new iOSPinchEventArgs(gr, _previousPinchArgs, _viewLocationAtOnDown);
            RotateEventArgs rotateEventArgs = new iOSRotateEventArgs(gr, _previousRotateArgs, _viewLocationAtOnDown);
            if (touchCount == 0)
            {
                //System.Diagnostics.Debug.WriteLine("onPinchAndRotate set _viewLocationAtOnDown");
                _viewLocationAtOnDown = ViewLocationInWindow(gr.View);
                touchCount++;
            }
            bool handled = false;

            if (gr.State == UIGestureRecognizerState.Ended || gr.State == UIGestureRecognizerState.Cancelled || gr.State == UIGestureRecognizerState.Failed)
            {
                if (_previousPinchArgs == null && _previousRotateArgs == null)
                    return;
                foreach (var listener in _listeners)
                {
                    //if (handled)
                    //  break;
                    if (listener.HandlesPinching || listener.HandlesPinched)
                    {
                        var taskArgs = new PinchEventArgs(pinchEventArgs)
                        {
                            Listener = listener
                        };
                        listener.OnPinched(taskArgs);
                        if (taskArgs.Handled)
                            break;
                    }
                }
                foreach (var listener in _listeners)
                {
                    //if (handled)
                    //  break;
                    if (listener.HandlesRotating || listener.HandlesRotated)
                    {
                        var taskArgs = new RotateEventArgs(rotateEventArgs)
                        {
                            Listener = listener
                        };
                        listener.OnRotated(taskArgs);
                        if (taskArgs.Handled)
                            break;
                    }
                }
                //gr.CancelsTouchesInView = handled;
                _previousPinchArgs = null;
                _previousRotateArgs = null;
                _pinching = false;
                _rotating = false;
                _panning = false;
                _longPressing = false;
            }
            else
            {
                foreach (var listener in _listeners)
                {
                    if (listener.HandlesPinching)
                    {
                        var taskArgs = new PinchEventArgs(pinchEventArgs)
                        {
                            Listener = listener
                        };
                        listener.OnPinching(taskArgs);
                        handled = handled || pinchEventArgs.Handled;
                        if (taskArgs.Handled)
                            break;
                    }
                }
                foreach (var listener in _listeners)
                {
                    if (listener.HandlesRotating)
                    {
                        var taskArgs = new RotateEventArgs(rotateEventArgs)
                        {
                            Listener = listener
                        };
                        listener.OnRotating(taskArgs);
                        handled = handled || rotateEventArgs.Handled;
                        if (taskArgs.Handled)
                            break;
                    }
                }
                _previousPinchArgs = pinchEventArgs;
                _previousRotateArgs = rotateEventArgs;
            }

            /*
            foreach (var listener in _listeners)
            {
                //if (handled)
                //	break;
                if (gr.State != UIGestureRecognizerState.Ended && gr.State != UIGestureRecognizerState.Cancelled && gr.State != UIGestureRecognizerState.Failed)
                {
                    if (listener.HandlesPinching)
                    {
                        var taskArgs = new PinchEventArgs(pinchEventArgs);
                        taskArgs.Listener = listener;
                        listener.OnPinching(taskArgs);
                        handled = handled ||  pinchEventArgs.Handled;
                        if (taskArgs.Handled)
                            break;
                    }
                }
            }
            foreach (var listener in _listeners)
            {
                //if (handled)
                //	break;
                if (gr.State != UIGestureRecognizerState.Ended && gr.State != UIGestureRecognizerState.Cancelled && gr.State != UIGestureRecognizerState.Failed)
                {
                    if (listener.HandlesRotating)
                    {
                        var taskArgs = new RotateEventArgs(rotateEventArgs);
                        taskArgs.Listener = listener;
                        listener.OnRotating(taskArgs);
                        handled = handled ||  rotateEventArgs.Handled;
                        if (taskArgs.Handled)
                            break;
                    }
                }
            }
            _previousPinchArgs = pinchEventArgs;
            _previousRotateArgs = rotateEventArgs;
            //gr.CancelsTouchesInView = handled;
            if (handled)
                return;
            if (_previousPinchArgs == null && _previousRotateArgs == null)
                return;
            foreach (var listener in _listeners)
            {
                //if (handled)
                //	break;
                if (listener.HandlesPinching || listener.HandlesPinched)
                {
                    var taskArgs = new PinchEventArgs(pinchEventArgs);
                    taskArgs.Listener = listener;
                    listener.OnPinched(taskArgs);
                    if (taskArgs.Handled)
                        break;
                }
            }
            foreach (var listener in _listeners)
            {
                //if (handled)
                //	break;
                if (listener.HandlesRotating || listener.HandlesRotated)
                {
                    var taskArgs = new RotateEventArgs(rotateEventArgs);
                    taskArgs.Listener = listener;
                    listener.OnRotated(taskArgs);
                    if (taskArgs.Handled)
                        break;
                }
            }
            //gr.CancelsTouchesInView = handled;
            _previousPinchArgs = null;
            _previousRotateArgs = null;
            _pinching = false;
            _rotating = false;
            _panning = false;
            _longPressing = false;
            */
        }

        #endregion

    }
}
