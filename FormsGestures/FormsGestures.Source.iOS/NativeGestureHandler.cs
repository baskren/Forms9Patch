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

        readonly VisualElement _element;

        UIView _view;

        UIGestureRecognizer[] _gestureRecognizers;

        int _numberOfTaps;

        CancellationTokenSource _cancelTappedRaiser;

        bool _longPressing;

        Stopwatch _startPressing;

        PanEventArgs _lastPanArgs;

        bool _pinching;

        bool _rotating;

        static bool _cancelled;

        PinchEventArgs _previousPinchArgs;

        RotateEventArgs _previousRotateArgs;

        List<Listener> _listeners = new List<Listener>();

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
                _listeners.Remove(listener);
            if (_listeners.Count < 1)
                // no one is listening so shut down
                _element.SetValue(GestureHandlerProperty, null);
            //}
        }


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


        void RendererDisconnect()
        {
            clearGestureRecognizers();
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
                resetGestureRecognizers(renderer?.NativeView);
            }
        }


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
                instance?.resetGestureRecognizers(Platform.GetRenderer(listener.Element)?.NativeView);
            }
        }

        void OnElementPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            var element = sender as VisualElement;
            if (element != null)
            {
                if (e.PropertyName == "Renderer")
                    RendererDisconnect();
                else if (e.PropertyName == GestureHandlerProperty.PropertyName)
                {
                    _element.Behaviors.Remove(this);
                }

            }
        }


        void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var element = sender as VisualElement;
            if (element != null)
            {
                if (e.PropertyName == "Renderer")
                    RendererConnect();
            }
        }

        // only called when a iOSGestureHandler doesn't already exist for this VisualElement
        NativeGestureHandler(VisualElement element)
        {
            _element = element;
            _element.Behaviors.Add(this);
        }

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


        internal void AttachNativeGestureHandler(Listener listener)
        {
            NativeGestureHandler.GetInstanceForListener(listener);
            listener.PropertyChanged += OnListenerPropertyChanged;
        }

        void clearGestureRecognizers()
        {
            if (_gestureRecognizers != null)
            {
                UIGestureRecognizer[] array = _gestureRecognizers;
                foreach (var uIGestureRecognizer in array)
                    uIGestureRecognizer.View.RemoveGestureRecognizer(uIGestureRecognizer);
                _gestureRecognizers = null;
            }
        }

        void resetGestureRecognizers(UIView view)
        {
            if (_view != view)
            {
                clearGestureRecognizers();
            }
            _view = view;
            if (_view != null)
            {
                _gestureRecognizers = CreateGestureRecognizers();
                UIGestureRecognizer[] array = _gestureRecognizers;
                foreach (var gestureRecognizer in array)
                    view.AddGestureRecognizer(gestureRecognizer);
            }
        }

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

        bool HandlesDoubleTaps
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

        UIGestureRecognizer[] CreateGestureRecognizers()
        {
            var list = new List<UIGestureRecognizer>();
            //if (HandlesDownUps)  WE NEED TO ALWAYS MONITOR FOR DOWN TO BE ABLE TO UNDO A CANCEL 
            {  // commenting out if (HanglesDownUps) causes FormsDragNDrop listview to not recognize cell selections after a scroll.  Why?  I have no clue.
               // Let's always trigger the down gesture recognizer so we can get the starting location
                var downUpGestureRecognizer = new DownUpGestureRecognizer(new Action<DownUpGestureRecognizer, UITouch[]>(OnDown), new Action<DownUpGestureRecognizer, UITouch[]>(OnUp));
                downUpGestureRecognizer.ShouldRecognizeSimultaneously = ((thisGr, otherGr) => false);
                downUpGestureRecognizer.ShouldReceiveTouch = ((UIGestureRecognizer gr, UITouch touch) => !(touch.View is UIControl));
                list.Add(downUpGestureRecognizer);
                //downUpGestureRecognizer.ShouldReceiveTouch = (recognizer, touch) => {
                //	return _element.get_IgnoreChildrenTouches() ? touch.View==_view : true;
                //};
            }
            UILongPressGestureRecognizer uILongPressGestureRecognizer = null;
            if (HandlesLongs)
            {
                uILongPressGestureRecognizer = new UILongPressGestureRecognizer(new Action<UILongPressGestureRecognizer>(OnLongPressed));
                uILongPressGestureRecognizer.ShouldRecognizeSimultaneously = ((thisGr, otherGr) => false);
                list.Add(uILongPressGestureRecognizer);
                //uILongPressGestureRecognizer.ShouldReceiveTouch = (recognizer, touch) => {
                //	return _element.get_IgnoreChildrenTouches() ? touch.View==_view : true;
                //};
            }
            if (HandlesTaps)
            {
                var uITapGestureRecognizer = new UITapGestureRecognizer(new Action<UITapGestureRecognizer>(OnTapped));
                uITapGestureRecognizer.ShouldRecognizeSimultaneously = ((thisGr, otherGr) =>
                {
                    return thisGr.GetType() != otherGr.GetType();
                });
                uITapGestureRecognizer.ShouldReceiveTouch = ((UIGestureRecognizer gr, UITouch touch) =>
                {
                    // these are handled BEFORE the touch call is passed to the listener.
                    return !(touch.View is UIControl);
                    //return touch.View == gr.View;
                });
                if (uILongPressGestureRecognizer != null)
                    uITapGestureRecognizer.RequireGestureRecognizerToFail(uILongPressGestureRecognizer);
                list.Add(uITapGestureRecognizer);
                //uITapGestureRecognizer.ShouldReceiveTouch = (recognizer, touch) => {
                //	return _element.get_IgnoreChildrenTouches() ? touch.View==_view : true;
                //};
            }
            if (HandlesPans)
            {
                var uIPanGestureRecognizer = new UIPanGestureRecognizer(new Action<UIPanGestureRecognizer>(OnPanned));
                uIPanGestureRecognizer.MinimumNumberOfTouches = 1;
                uIPanGestureRecognizer.ShouldRecognizeSimultaneously = ((thisGr, otherGr) => true);
                list.Add(uIPanGestureRecognizer);
                //uIPanGestureRecognizer.ShouldReceiveTouch = (recognizer, touch) => {
                //	return _element.get_IgnoreChildrenTouches() ? touch.View==_view : true;
                //};
            }
            if (HandlesPinches)
            {
                var uIPinchGestureRecognizer = new UIPinchGestureRecognizer(new Action<UIPinchGestureRecognizer>(OnPinched));
                uIPinchGestureRecognizer.ShouldRecognizeSimultaneously = ((thisGr, otherGr) => true);
                list.Add(uIPinchGestureRecognizer);
                //uIPinchGestureRecognizer.ShouldReceiveTouch = (recognizer, touch) => {
                //	return _element.get_IgnoreChildrenTouches() ? touch.View==_view : true;
                //};
            }
            if (HandlesRotates)
            {
                var uIRotationGestureRecognizer = new UIRotationGestureRecognizer(new Action<UIRotationGestureRecognizer>(OnRotated));
                uIRotationGestureRecognizer.ShouldRecognizeSimultaneously = ((thisGr, otherGr) => true);
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

        bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (!_disposed && disposing)
            {
                if (_gestureRecognizers != null)
                {
                    UIGestureRecognizer[] array = _gestureRecognizers;
                    foreach (var uIGestureRecognizer in array)
                        uIGestureRecognizer.View.RemoveGestureRecognizer(uIGestureRecognizer);
                    _gestureRecognizers = null;
                }
                if (_listeners != null)
                {
                    foreach (var listener in _listeners)
                        listener.PropertyChanged -= OnListenerPropertyChanged;
                    _listeners = null;
                }
                _disposed = true;
            }
        }

        CoreGraphics.CGPoint ViewLocationInWindow(UIView view)
        {
            return view.ConvertPointToView(CoreGraphics.CGPoint.Empty, null);
        }

        public static void Cancel()
        {
            _cancelled = true;
        }

        int touchCount;
        CoreGraphics.CGPoint _viewLocationAtOnDown;
        void OnDown(DownUpGestureRecognizer gr, UITouch[] touchesBegan)
        {
            _cancelled = false;

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
                    DownUpEventArgs args = new iOSDownUpEventArgs(gr, touchesBegan, _viewLocationAtOnDown);
                    args.Listener = listener;
                    listener.OnDown(args);
                    handled |= args.Handled;
                    if (handled)
                        break;
                }
            }
            //gr.CancelsTouchesInView = handled;
            //System.Diagnostics.Debug.WriteLine("\thandled=["+handled+"]");
        }

        void OnUp(DownUpGestureRecognizer gr, UITouch[] touchesEnded)
        {

            if (_cancelled)
            {
                touchCount = 0;
                return;
            }

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
                    DownUpEventArgs args = new iOSDownUpEventArgs(gr, touchesEnded, _viewLocationAtOnDown);
                    args.Listener = listener;
                    listener.OnUp(args);
                    handled |= args.Handled;
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

        void OnTapped(UITapGestureRecognizer gr)
        {
            if (_cancelled)
                return;

            if (!_element.IsVisible)
                return;
            if (touchCount == 0)
            {
                //System.Diagnostics.Debug.WriteLine("onTapped set _viewLocationAtOnDown");
                _viewLocationAtOnDown = ViewLocationInWindow(gr.View);
            }
            if (_cancelTappedRaiser != null)
            {
                try
                {
                    _cancelTappedRaiser.Cancel();
                }
                catch
                {
                    // do nothing!
                    System.Diagnostics.Debug.WriteLine("Tap cancelled");
                }
            }
            _numberOfTaps++;
            TapEventArgs args = new iOSTapEventArgs(gr, _numberOfTaps, _viewLocationAtOnDown);
            //System.Diagnostics.Debug.WriteLine("NativeGestureHandler.OnTapped args.Handled=[" + args.Handled + "]");
            bool handled = false;
            foreach (var listener in _listeners)
            {
                if (listener.HandlesTapping)
                {
                    var taskArgs = new TapEventArgs(args);
                    taskArgs.Listener = listener;
                    //System.Diagnostics.Debug.WriteLine("\tNativeGestureHandler.OnTapped OnTapping taskArgs.Handled=[" + taskArgs.Handled + "]");
                    listener.OnTapping(taskArgs);
                    //System.Diagnostics.Debug.WriteLine("\tNativeGestureHandler.OnTapped OnTapping taskArgs.Handled=[" + taskArgs.Handled + "]");
                    if (taskArgs.Handled)
                        break;
                }
            }
            _cancelTappedRaiser = new CancellationTokenSource();
            Task.Run(async delegate
            {
                if (HandlesDoubleTaps)
                    await Task.Delay(Settings.TappedThreshold, _cancelTappedRaiser.Token);
                _cancelTappedRaiser = null;
                _numberOfTaps = 0;
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (_cancelled)
                        return;

                    foreach (var listener in _listeners)
                    {
                        //if (handled)
                        //	break;
                        if (args.NumberOfTaps == 1 && listener.HandlesTapped)
                        {
                            var taskArgs = new TapEventArgs(args);
                            taskArgs.Listener = listener;
                            //System.Diagnostics.Debug.WriteLine("\tNativeGestureHandler.OnTapped OnTapped taskArgs.Handled=[" + taskArgs.Handled + "]");
                            listener.OnTapped(taskArgs);
                            //System.Diagnostics.Debug.WriteLine("\tNativeGestureHandler.OnTapped OnTapped taskArgs.Handled=[" + taskArgs.Handled + "]");
                            handled |= args.Handled;
                            if (taskArgs.Handled)
                                break;
                        }
                        else if (args.NumberOfTaps == 2 && listener.HandlesDoubleTapped)
                        {
                            var taskArgs = new TapEventArgs(args);
                            taskArgs.Listener = listener;
                            //System.Diagnostics.Debug.WriteLine("\tNativeGestureHandler.OnDoubleTapped OnTapped taskArgs.Handled=[" + taskArgs.Handled + "]");
                            listener.OnDoubleTapped(taskArgs);
                            //System.Diagnostics.Debug.WriteLine("\tNativeGestureHandler.OnDoubleTapped OnTapped taskArgs.Handled=[" + taskArgs.Handled + "]");
                            handled |= args.Handled;
                            if (taskArgs.Handled)
                                break;
                        }
                    }
                    //gr.CancelsTouchesInView = handled;
                });
            }, _cancelTappedRaiser.Token);
        }

        void OnLongPressed(UILongPressGestureRecognizer gr)
        {
            if (_cancelled)
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
                            LongPressEventArgs args = new iOSLongPressEventArgs(gr, _startPressing.ElapsedMilliseconds, _viewLocationAtOnDown);
                            args.Listener = listener;
                            listener.OnLongPressed(args);
                            handled |= args.Handled;
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
                        LongPressEventArgs args = new iOSLongPressEventArgs(gr, 0L, _viewLocationAtOnDown);
                        args.Listener = listener;
                        listener.OnLongPressing(args);
                        handled |= args.Handled;
                        if (handled)
                            break;
                    }
                }
                //gr.CancelsTouchesInView = handled;
            }
        }

        void OnPanned(UIPanGestureRecognizer gr)
        {
            if (_cancelled)
                return;

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
                            SwipeEventArgs args = new iOSSwipeEventArgs(gr, direction, _viewLocationAtOnDown);
                            args.Listener = listener;
                            args.VelocityX = velocity.X;
                            args.VelocityY = velocity.Y;
                            listener.OnSwiped(args);
                            //gr.CancelsTouchesInView = swipeEventArgs.Handled;
                            //return;
                            handled |= args.Handled;
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
                        var taskArgs = new PanEventArgs(panEventArgs);
                        taskArgs.Listener = listener;
                        listener.OnPanned(taskArgs);
                        //gr.CancelsTouchesInView = panEventArgs.Handled;
                        //return;
                        handled |= taskArgs.Handled;
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
                bool handled = false;
                foreach (var listener in _listeners)
                {
                    //System.Diagnostics.Debug.WriteLine("\tlistener.handlesPanning=["+listener.HandlesPanning+"]");
                    //if (handled)
                    //	break;
                    if (!panEventArgs.Equals(_lastPanArgs) && listener.HandlesPanning)
                    {
                        var taskArgs = new PanEventArgs(panEventArgs);
                        taskArgs.Listener = listener;
                        listener.OnPanning(taskArgs);
                        handled |= taskArgs.Handled;
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
            if (_cancelled)
                return;

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
            if (_cancelled)
                return;

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
            if (_cancelled)
                return;

            PinchEventArgs pinchEventArgs = new iOSPinchEventArgs(gr, _previousPinchArgs, _viewLocationAtOnDown);
            RotateEventArgs rotateEventArgs = new iOSRotateEventArgs(gr, _previousRotateArgs, _viewLocationAtOnDown);
            if (touchCount == 0)
            {
                //System.Diagnostics.Debug.WriteLine("onPinchAndRotate set _viewLocationAtOnDown");
                _viewLocationAtOnDown = ViewLocationInWindow(gr.View);
                touchCount++;
            }
            bool handled = false;
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
                        handled |= pinchEventArgs.Handled;
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
                        handled |= rotateEventArgs.Handled;
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
        }


    }
}
