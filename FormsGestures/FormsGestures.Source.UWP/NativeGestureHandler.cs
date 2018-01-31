using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace FormsGestures.UWP
{
    class NativeGestureHandler : Behavior<VisualElement>, IDisposable
    {
        static int DebugVerbosity = 0;  // "1" (method names), "2" (method details)
        static void DebugMethodName([System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
        {
            if (DebugVerbosity > 0)
            {
                System.Diagnostics.Debug.WriteLine("⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽");
                System.Diagnostics.Debug.WriteLine(callerName + ":");
                System.Diagnostics.Debug.WriteLine("⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺");
            }
        }

        static void DebugMessage(string message)
        {
            if (DebugVerbosity > 1)
                System.Diagnostics.Debug.WriteLine("\t"+message);
        }

        static readonly BindableProperty GestureHandlerProperty = BindableProperty.Create("GestureHandler", typeof(NativeGestureHandler), typeof(NativeGestureHandler), null);

        readonly VisualElement _xfElement;

        WeakReference<Windows.UI.Xaml.FrameworkElement> _uwpElementWeakReference;  
        FrameworkElement FrameworkElement
        {
            get
            {
                if (_uwpElementWeakReference == null || !_uwpElementWeakReference.TryGetTarget(out FrameworkElement element))
                    return null;
                return element;
            }
            set
            {
                if (FrameworkElement != null)
                {
                    FrameworkElement.ManipulationMode = ManipulationModes.None;
                    //FrameworkElement.ManipulationStarting -= OnManipulationStarting;
                    FrameworkElement.ManipulationStarted -= OnManipulationStarted;
                    FrameworkElement.ManipulationDelta -= OnManipulationDelta;
                    FrameworkElement.ManipulationInertiaStarting -= OnManipulationInertiaStarting;
                    FrameworkElement.ManipulationCompleted -= OnManipulationComplete;

                    //FrameworkElement.Tapped -= _UwpElement_Tapped;
                    //FrameworkElement.RightTapped -= _UwpElement_RightTapped;
                    //FrameworkElement.PointerWheelChanged -= _UwpElement_PointerWheelChanged;

                    //FrameworkElement.PointerReleased -= OnPointerReleased;
                    FrameworkElement.PointerPressed -= OnPointerPressed;

                    FrameworkElement.Tapped -= OnTapped;

                    //_UwpElement.PointerMoved -= OnPointerMoved;
                    //FrameworkElement.PointerExited -= _UwpElement_PointerExited;
                    //FrameworkElement.PointerEntered -= _UwpElement_PointerEntered;
                    FrameworkElement.PointerCaptureLost -= OnPointerCaptureLost;
                    FrameworkElement.PointerCanceled -= OnPointerCancelled;
                    //FrameworkElement.Holding -= OnHolding;
                    FrameworkElement.DoubleTapped -= OnDoubleTapped;

                    FrameworkElement.IsDoubleTapEnabled = false;
                    
                }
                if (value==null)
                {
                    _uwpElementWeakReference = null;
                    return;
                }
                _uwpElementWeakReference = new WeakReference<FrameworkElement>(value);
                //FrameworkElement.ManipulationStarting += OnManipulationStarting;
                FrameworkElement.ManipulationStarted += OnManipulationStarted;
                FrameworkElement.ManipulationDelta += OnManipulationDelta;
                FrameworkElement.ManipulationInertiaStarting += OnManipulationInertiaStarting;
                FrameworkElement.ManipulationCompleted += OnManipulationComplete;
                FrameworkElement.ManipulationMode = ManipulationModes.All;

                
                //FrameworkElement.Tapped += _UwpElement_Tapped;
                //FrameworkElement.RightTapped += _UwpElement_RightTapped;
                //FrameworkElement.PointerWheelChanged += _UwpElement_PointerWheelChanged;

                //FrameworkElement.PointerReleased += OnPointerReleased;
                FrameworkElement.PointerPressed += OnPointerPressed;

                FrameworkElement.Tapped += OnTapped;

                //_UwpElement.PointerMoved += OnPointerMoved;
                //FrameworkElement.PointerExited += _UwpElement_PointerExited;
                //FrameworkElement.PointerEntered += _UwpElement_PointerEntered;
                FrameworkElement.PointerCaptureLost += OnPointerCaptureLost;
                FrameworkElement.PointerCanceled += OnPointerCancelled;
                //FrameworkElement.Holding += OnHolding;  // holding event doesn't work with Mouse (https://stackoverflow.com/questions/34995594/holding-event-for-desktop-app-not-firing)
                FrameworkElement.DoubleTapped += OnDoubleTapped;

                FrameworkElement.IsDoubleTapEnabled = true;

            }
        }

        bool _panning;
        bool _pinching;
        bool _rotating;
        bool _longPressing;

        int _numberOfTaps;

        Stopwatch _holdTimer;
        Stopwatch _releaseTimer;
        /*

        CancellationTokenSource _cancelTappedRaiser;


        PanEventArgs _lastPanArgs;

        PinchEventArgs _previousPinchArgs;

        RotateEventArgs _previousRotateArgs;


        private TransformGroup _transformGroup;
        private MatrixTransform _previousTransform;
        private CompositeTransform _compositeTransform;
        private bool forceManipulationsToEnd;
        */
        List<Listener> _listeners = new List<Listener>();

        #region Wiring between XF element / FG listener / Uwp element
        void AddListener(Listener listener)
        {
            if (!(_listeners.Contains(listener)))
            {
                _listeners.Add(listener);
                // listener.Disposing += RemoveListener;
                listener.Disposing += OnListenerDisposing;
            }
        }

        private void OnListenerDisposing(object sender, System.EventArgs e)
//        void RemoveListener(object sender, EventArgs e)
        {
            var listener = sender as Listener;
            //if (_listeners!=null) {
            if (_listeners.Contains(listener))
                _listeners.Remove(listener);
            if (_listeners.Count < 1)
                // no one is listening so shut down
                _xfElement.SetValue(GestureHandlerProperty, null);
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
            FrameworkElement = null;
            IVisualElementRenderer renderer = Platform.GetRenderer(_xfElement);
            if (renderer != null)
                renderer.ElementChanged -= OnRendererElementChanged;
        }

        void RendererConnect()
        {
            IVisualElementRenderer renderer = Platform.GetRenderer(_xfElement);
            if (renderer != null)
            {
                renderer.ElementChanged += OnRendererElementChanged;
                FrameworkElement = renderer as FrameworkElement;
                if (FrameworkElement==null)
                    throw new Exception("If we have a renderer, should it not always be a FrameworkElement?");
            }
        }

        void OnRendererElementChanged(object sender, ElementChangedEventArgs<VisualElement> e)
        {
            if (e.OldElement != null)
                RendererDisconnect();
        }

        void OnElementPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            if (sender is VisualElement element)
            {
                if (e.PropertyName == "Renderer")
                    RendererDisconnect();
                else if (e.PropertyName == GestureHandlerProperty.PropertyName)
                {
                    _xfElement.Behaviors.Remove(this);
                }

            }
        }

        void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is VisualElement element)
            {
                if (e.PropertyName == "Renderer")
                    RendererConnect();
            }
        }

        // only called when a GestureHandler doesn't already exist for this VisualElement
        NativeGestureHandler(VisualElement element)
        {
            _xfElement = element;
            _xfElement.Behaviors.Add(this);
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
            bindable.SetValue(GestureHandlerProperty, null);
            base.OnDetachingFrom(bindable);
        }
        #endregion

        #region Handles tests
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
        #endregion


        #region debug message methods
        bool PointerRoutedDebugMessage(PointerRoutedEventArgs e, string commandName)
        {
            var currentPoint = e.GetCurrentPoint(null);
            if (!currentPoint.IsInContact)
                return false;
            DebugMethodName(commandName);
            DebugMessage("CurrentPoint: id=[" + currentPoint.PointerId + "] device=[" + currentPoint.PointerDevice + "] pos=[" + currentPoint.Position.X + "," + currentPoint.Position.Y + "] IsInContact=[" + currentPoint.IsInContact + "]");
            return true;
        }


        void ModesDebugMessage(ManipulationModes modes)
        {
            string modesString = "";
            foreach (ManipulationModes mode in Enum.GetValues(typeof(ManipulationModes)))
            {
                if ((mode & modes) != 0)
                    modesString += "[" + mode + "]";
            }
            DebugMessage("Modes=" + modesString);
        }

        void PivotDebugMessage(ManipulationPivot pivot)
        {
            if (pivot == null)
                return;
            var result = " x=[" + pivot.Center.X + "] y=[" + pivot.Center.Y + "] r=[" + pivot.Radius + "]";
            DebugMessage("Pivot:" + result);
        }

        void ContainerDebugMessage(UIElement container)
        {
            DebugMessage("Containter=" + container + " sameAsFrameworkElement=[" + (container == FrameworkElement) + "]");
        }

        void PositionDebugMessage(Windows.Foundation.Point point)
        {
            DebugMessage("Position: x=["+ point.X + "] y=[" + point.Y + "]");
        }

        void PointerDeviceTypeDebugMessage(Windows.Devices.Input.PointerDeviceType types)
        {
            string deviceString = "";
            foreach (Windows.Devices.Input.PointerDeviceType type in Enum.GetValues(typeof(Windows.Devices.Input.PointerDeviceType)))
            {
                if ((type & types) != 0)
                    deviceString += "[" + type + "]";
            }
            DebugMessage("Device: "+ deviceString);
        }

        void ManipulationDeltaDebugMessage(ManipulationDelta delta, string name)
        {
            DebugMessage(""+name+": trans=["+delta.Translation.X+","+delta.Translation.Y+"] dip");
            DebugMessage("       expan=[" + delta.Expansion + "] dip");
            DebugMessage("       scale=[" + delta.Scale +"] %");
            DebugMessage("       rotat=[" + delta.Rotation + "] degrees");
        }

        void HandledDebugString(bool handled)
        {
            DebugMessage("Handled=["+handled+"]");
        }

        void VelocitiesDebugString(ManipulationVelocities velocities)
        {
            DebugMessage("Velocities: linear=[" + velocities.Linear.X + "," + velocities.Linear.Y + "] dip");
            DebugMessage("            expans=[" + velocities.Expansion + "] dip/ms");
            DebugMessage("            angulr =[" + velocities.Angular + "] degrees/ms");
        }
        #endregion


        #region UWP Manipulations (multi-touch)
        /*
        void OnManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            DebugMethodName("OnManipulationStarting:");
            ModesDebugMessage(e.Mode);
            PivotDebugMessage(e.Pivot);
            ContainerDebugMessage(e.Container);
        }
        */

        void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            DebugMethodName("OnManipulationStarted:");
            PointerDeviceTypeDebugMessage(e.PointerDeviceType);
            PositionDebugMessage(e.Position);
            ManipulationDeltaDebugMessage(e.Cumulative,"Cumul");
            ContainerDebugMessage(e.Container);
            HandledDebugString(e.Handled);

            if (!_xfElement.IsVisible || FrameworkElement == null)
                return;

            long elapsed = 0;
            if (_holdTimer != null)
            {
                elapsed = _holdTimer.ElapsedMilliseconds;
                _holdTimer?.Stop();
                _holdTimer = null;
            }
            System.Diagnostics.Debug.WriteLine("\t elapse=["+elapsed+"]");
            if (!_panning || !_pinching || !_rotating)
            {
                _longPressing = elapsed > 750;
                if (_longPressing)
                {
                    foreach (var listener in _listeners)
                    {
                        if (listener.HandlesLongPressing)
                        {
                            var args = new UwpLongPressEventArgs(FrameworkElement, e, elapsed);
                            args.Listener = listener;
                            listener?.OnLongPressing(args);
                            e.Handled = args.Handled;
                            if (e.Handled)
                                break;
                        }
                    }
                }
            }
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            _longPressing = false;
            _holdTimer?.Stop();
            _holdTimer = null;

            if (!_xfElement.IsVisible || FrameworkElement == null)
                return;

            if (!_panning && (Math.Abs(e.Cumulative.Translation.X) > 0 || Math.Abs(e.Cumulative.Translation.Y) > 0))
                _panning = true;
            if (!_pinching && Math.Abs(e.Cumulative.Scale - 1) > 0)
                _pinching = true;
            if (!_rotating && Math.Abs(e.Cumulative.Rotation) > 0)
                _rotating = true;

            DebugMethodName("OnManipulationDelta");
            PointerDeviceTypeDebugMessage(e.PointerDeviceType);
            PositionDebugMessage(e.Position);
            ManipulationDeltaDebugMessage(e.Delta, "Delta");
            ManipulationDeltaDebugMessage(e.Cumulative, "Cumul");
            VelocitiesDebugString(e.Velocities);
            DebugMessage("IsIntertial=[" + e.IsInertial + "]");
            ContainerDebugMessage(e.Container);
            HandledDebugString(e.Handled);

            foreach (var listener in _listeners)
            {
                if (_panning && listener.HandlesPanning)
                {
                    var args = new UwpPanEventArgs(FrameworkElement, e);
                    args.Listener = listener;
                    listener?.OnPanning(args);
                    e.Handled |= args.Handled;
                }
                if (_pinching && listener.HandlesPinching)
                {
                    var args = new UwpPinchEventArgs(FrameworkElement, e);
                    args.Listener = listener;
                    listener?.OnPinching(args);
                    e.Handled |= args.Handled;
                }
                if (_rotating && listener.HandlesRotating)
                {
                    var args = new UwpRotateEventArgs(FrameworkElement, e);
                    args.Listener = listener;
                    listener?.OnRotating(args);
                    e.Handled |= args.Handled;
                }
                if (e.Handled)
                    break;
            }
        }

        private void OnManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            DebugMethodName("OnManipulationInertiaStarting");
            PointerDeviceTypeDebugMessage(e.PointerDeviceType);
            ManipulationDeltaDebugMessage(e.Delta, "Delta");
            ManipulationDeltaDebugMessage(e.Cumulative, "Cumul");
            VelocitiesDebugString(e.Velocities);
            ContainerDebugMessage(e.Container);
            DebugMessage("TranslationBehavioir: disp=[" + e.TranslationBehavior.DesiredDisplacement + "] decl=[" + e.TranslationBehavior.DesiredDeceleration + "]");
            DebugMessage("RotationBehavior: rot=[" + e.RotationBehavior.DesiredRotation + "] decl=[" + e.RotationBehavior.DesiredDeceleration + "]");
            DebugMessage("ExpansionBehavior: exp=[" + e.ExpansionBehavior.DesiredExpansion + "] decl=[" + e.ExpansionBehavior.DesiredDeceleration + "]");
            HandledDebugString(e.Handled);
        }

        private void OnManipulationComplete(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (!_xfElement.IsVisible || FrameworkElement == null)
                return;

            DebugMethodName("OnManipulationComplete");
            PointerDeviceTypeDebugMessage(e.PointerDeviceType);
            PositionDebugMessage(e.Position);
            ManipulationDeltaDebugMessage(e.Cumulative, "Cumul");
            VelocitiesDebugString(e.Velocities);
            DebugMessage("IsIntertial=[" + e.IsInertial + "]");
            ContainerDebugMessage(e.Container);
            HandledDebugString(e.Handled);

            foreach (var listener in _listeners)
            {
                if (_panning && listener.HandlesPanning)
                {
                    var args = new UwpPanEventArgs(FrameworkElement, e);
                    args.Listener = listener;
                    listener?.OnPanned(args);
                    e.Handled |= args.Handled;
                }
                if (_pinching && listener.HandlesPinching)
                {
                    var args = new UwpPinchEventArgs(FrameworkElement, e);
                    args.Listener = listener;
                    listener?.OnPinched(args);
                    e.Handled |= args.Handled;
                }
                if (_rotating && listener.HandlesRotating)
                {
                    var args = new UwpRotateEventArgs(FrameworkElement, e);
                    args.Listener = listener;
                    listener?.OnRotated(args);
                    e.Handled |= args.Handled;
                }

                if (e.Handled)
                    break;
            }

            _panning = false;
            _pinching = false;
            _rotating = false;
            _longPressing = false;
        }
        #endregion


        #region UWP Pointer Event Responders
        private void OnPointerCancelled(object sender, PointerRoutedEventArgs e)
        {

            if (!_xfElement.IsVisible || FrameworkElement == null)
                return;
            PointerRoutedDebugMessage(e, "POINTER CANCELLED");

            long elapsed = 0;
            if (_holdTimer != null)
            {
                elapsed = _holdTimer.ElapsedMilliseconds;
                _holdTimer?.Stop();
                _holdTimer = null;
            }

            _releaseTimer?.Stop();
            _releaseTimer = null;

            foreach (var listener in _listeners)
            {
                if (listener.HandlesTapped)
                {
                    var args = new UwpTapEventArgs(FrameworkElement, e, _numberOfTaps);
                    args.Listener = listener;
                    args.Cancelled = true;
                    listener?.OnTapped(args);
                    e.Handled = args.Handled;
                }
                if (_longPressing && listener.HandlesLongPressed)
                {
                    var args = new UwpLongPressEventArgs(FrameworkElement, e, elapsed);
                    args.Listener = listener;
                    args.Cancelled = true;
                    listener?.OnLongPressed(args);
                    e.Handled = args.Handled;
                }
                if (listener.HandlesDown)
                {
                    var args = new UWPDownUpArgs(FrameworkElement, e);
                    args.Listener = listener;
                    args.Cancelled = true;
                    listener.OnUp(args);
                    e.Handled = args.Handled;
                    if (e.Handled)
                        return;
                }

            }
        }

        private void OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            PointerRoutedDebugMessage(e, "POINTER CAPTURE LOST");
            //OnPointerCancelled
        }

        /*
        private void _UwpElement_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            PointerRoutedDebugMessage(e, "POINTER ENTERED");
        }

        private void _UwpElement_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            PointerRoutedDebugMessage(e, "POINTER EXITED");
        }

        private void _UwpElement_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            PointerRoutedDebugMessage(e, "POINTER MOVED");

        }

        private void _UwpElement_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PointerRoutedDebugMessage(e, "POINTER PRESSED");
        }

        private void _UwpElement_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            PointerRoutedDebugMessage(e,"POINTER RELEASED");
        }

        private void _UwpElement_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            PointerRoutedDebugMessage(e, "POINTER WHEEL CHANGED");
        }

        private void _UwpElement_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var currentPoint = e.GetPosition(null);
            DebugMethodName("RIGHT TAPPED");
            DebugMessage("CurrentPoint: pos=[" + currentPoint.X + "," + currentPoint.Y + "] Handled=["+e.Handled+"] type=["+e.PointerDeviceType+"]");
        }

        private void _UwpElement_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            var currentPoint = e.GetPosition(null);
            DebugMethodName("TAPPED");
            DebugMessage("CurrentPoint: pos=[" + currentPoint.X + "," + currentPoint.Y + "] Handled=[" + e.Handled + "] type=[" + e.PointerDeviceType + "]");

            long elapsed = 0;
            if (_pressTimer != null)
            {
                elapsed = _pressTimer.ElapsedMilliseconds;
                _pressTimer?.Stop();
                _pressTimer = null;
            }

            if (!_xfElement.IsVisible || FrameworkElement == null)
                return;

            foreach (var listener in _listeners)
            {
                if (listener.HandlesTapped)
                {
                    var args = new UwpTapEventArgs(FrameworkElement, e, _numberOfTaps);
                    args.Listener = listener;
                    listener?.OnTapped(args);
                    e.Handled = args.Handled;
                }
                if (_longPressing && listener.HandlesLongPressed)
                {
                    var args = new UwpLongPressEventArgs(FrameworkElement, e, elapsed);
                    args.Listener = listener;
                    listener?.OnLongPressed(args);
                    e.Handled = args.Handled;
                }
                if (e.Handled)
                    break;
            }

            _longPressing = false;
            
        }
        */

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {            
            if (!_xfElement.IsVisible || FrameworkElement==null)
                return;
            DebugMethodName();

            if (_releaseTimer == null || _releaseTimer.Elapsed.TotalMilliseconds > 750)
            {
                _releaseTimer?.Stop();
                _releaseTimer = null;
                _numberOfTaps = 0;
            }
            _numberOfTaps++;

            _holdTimer?.Stop();
            _holdTimer = new Stopwatch();
            _holdTimer.Start();

            _rotating = false;
            _pinching = false;
            _panning = false;
            _longPressing = false;

            foreach (var listener in _listeners)
            {
                if (listener.HandlesDown)
                {
                    var args = new UWPDownUpArgs(FrameworkElement, e);
                    args.Listener = listener;
                    listener.OnDown(args);
                    e.Handled = args.Handled;
                    if (e.Handled)
                        return;
                }
            }
        }

        bool _runningTapCounterResetter;

        //private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (!_xfElement.IsVisible || FrameworkElement == null)
                return;

            DebugMethodName();

            long elapsed = 0;
            if (_holdTimer != null)
            {
                elapsed = _holdTimer.ElapsedMilliseconds;
                _holdTimer?.Stop();
                _holdTimer = null;
            }

            _releaseTimer?.Stop();
            _releaseTimer = new Stopwatch();
            _releaseTimer.Start();

            if (!_runningTapCounterResetter)
            {
                _runningTapCounterResetter = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (_releaseTimer==null || _releaseTimer.Elapsed.Milliseconds > 750)
                    {
                        _numberOfTaps = 0;
                        _releaseTimer?.Stop();
                        _releaseTimer = null;
                        _runningTapCounterResetter = false;
                    }
                    return _runningTapCounterResetter;
                });
            }

            foreach (var listener in _listeners)
            {
                if (listener.HandlesTapped)
                {
                    var args = new UwpTapEventArgs(FrameworkElement, e, _numberOfTaps);
                    args.Listener = listener;
                    listener?.OnTapped(args);
                    e.Handled = args.Handled;
                }
                if (_longPressing && listener.HandlesLongPressed)
                {
                    var args = new UwpLongPressEventArgs(FrameworkElement, e, elapsed);
                    args.Listener = listener;
                    listener?.OnLongPressed(args);
                    e.Handled = args.Handled;
                }
                if (listener.HandlesDown)
                {
                    var args = new UWPDownUpArgs(FrameworkElement, e);
                    args.Listener = listener;
                    listener.OnUp(args);
                    e.Handled = args.Handled;
                    if (e.Handled)
                        return;
                }
            }
            _longPressing = false;
        }

        private void OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (!_xfElement.IsVisible || FrameworkElement == null)
                return;
            DebugMethodName();
            foreach (var listener in _listeners)
            {
                if (listener.HandlesDoubleTapped)
                {
                    var args = new UwpTapEventArgs(FrameworkElement, e, _numberOfTaps);
                    args.Listener = listener;
                    listener.OnDoubleTapped(args);
                }
            }
            _longPressing = false;
        }

        /*
        
        private void OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            DebugMethodName();
        }

            
        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            DebugMethodName();
            throw new NotImplementedException();
        }

        void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            // USE OnManipulationDelta INSTEAD
            if (!_xfElement.IsVisible || _UwpElement == null)
                return;
            var currentPoint = e.GetCurrentPoint(null);
            if (!currentPoint.IsInContact)
                return;
            foreach (var listener in _listeners)
            {
                if (listener.HandlesPanning)
                {
                    var args = new UWPDownUpArgs(_UwpElement, e);
                    args.Listener = listener;
                    listener.OnUp(args);
                    e.Handled = args.Handled;
                    if (e.Handled)
                        return;
                }
            }

            throw new NotImplementedException();
        }
        */
        #endregion


        #region Disposal
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
                /*
                if (_gestureRecognizers != null)
                {
                    UIGestureRecognizer[] array = _gestureRecognizers;
                    foreach (var uIGestureRecognizer in array)
                        uIGestureRecognizer.View.RemoveGestureRecognizer(uIGestureRecognizer);
                    _gestureRecognizers = null;
                }
                */
                _listeners = null;
                _disposed = true;
            }
        }
        #endregion


    }
}
