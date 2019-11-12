using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;

namespace FormsGestures
{
    /// <summary>
    /// FormsGestures Listener
    /// </summary>
    public class Listener : BindableObject, IDisposable
    {
#pragma warning disable IDE1006 // Naming Styles

        //const bool _debugEvents = false;

        // ancestors will procede descendents in this list so that interuptions can found quickly and fired in an orderly fashion
        internal static readonly List<Listener> Listeners = new List<Listener>();

        readonly VisualElement _element;
        /// <summary>
        /// VisualElement that is the focus of this Listener
        /// </summary>
        public VisualElement Element => _element;



        #region Events / Commands

        #region Event / Command Property List
        internal static readonly string[] AllEventsAndCommands = {
            nameof(Down),
            nameof(DownCommand),
            nameof(DownCommandParameter),
            nameof(DownCallback),
            nameof(DownCallbackParameter),

            nameof(Up),
            nameof(UpCommand),
            nameof(UpCommandParameter),
            nameof(UpCallback),
            nameof(UpCallbackParameter),

            nameof(Tapping),
            nameof(TappingCommand),
            nameof(TappingCommandParameter),
            nameof(TappingCallback),
            nameof(TappingCallbackParameter),

            nameof(Tapped),
            nameof(TappedCommand),
            nameof(TappedCommandParameter),
            nameof(TappedCallback),
            nameof(TappedCallbackParameter),

            nameof(DoubleTapped),
            nameof(DoubleTappedCommand),
            nameof(DoubleTappedCommandParameter),
            nameof(DoubleTappedCallback),
            nameof(DoubleTappedCallbackParameter),

            nameof(LongPressing),
            nameof(LongPressingCommand),
            nameof(LongPressingCommandParameter),
            nameof(LongPressingCallback),
            nameof(LongPressingCallbackParameter),

            nameof(LongPressed),
            nameof(LongPressedCommand),
            nameof(LongPressedCommandParameter),
            nameof(LongPressedCallback),
            nameof(LongPressedCallbackParameter),

            nameof(Pinching),
            nameof(PinchingCommand),
            nameof(PinchingCommandParameter),
            nameof(PinchingCallback),
            nameof(PinchingCallbackParameter),

            nameof(Pinched),
            nameof(PinchedCommand),
            nameof(PinchedCommandParameter),
            nameof(PinchedCallback),
            nameof(PinchedCallbackParameter),

            nameof(Panning),
            nameof(PanningCommand),
            nameof(PanningCommandParameter),
            nameof(PanningCallback),
            nameof(PanningCallbackParameter),

            nameof(Panned),
            nameof(PannedCommand),
            nameof(PannedCommandParameter),
            nameof(PannedCallback),
            nameof(PannedCallbackParameter),

            nameof(Swiped),
            nameof(SwipedCommand),
            nameof(SwipedCommandParameter),
            nameof(SwipedCallback),
            nameof(SwipedCallbackParameter),

            nameof(Rotating),
            nameof(RotatingCommand),
            nameof(RotatingCommandParameter),
            nameof(RotatingCallback),
            nameof(RotatingCallbackParameter),

            nameof(Rotated),
            nameof(RotatedCommand),
            nameof(RotatedCommandParameter),
            nameof(RotatedCallback),
            nameof(RotatedCallbackParameter),

            nameof(RightClicked),
            nameof(RightClickedCommand),
            nameof(RightClickedCommandParameter),
            nameof(RightClickedCallback),
            nameof(RightClickedCallbackParameter),

        };
        #endregion

        #region Down
        event EventHandler<DownUpEventArgs> _down;
        /// <summary>
        /// Down event handler
        /// </summary>
        public event EventHandler<DownUpEventArgs> Down
        {
            add
            {
                var oldHandlesDownState = HandlesDown;
                _down += value;
                if (HandlesDown != oldHandlesDownState)
                    HandlesDownChanged?.Invoke(this, !oldHandlesDownState);
            }
            remove
            {
                var oldHandlesDownState = HandlesDown;
                _down -= value;
                if (HandlesDown != oldHandlesDownState)
                    HandlesDownChanged?.Invoke(this, !oldHandlesDownState);
            }
        }
        /// <summary>
        /// backing store for command invoked upon down event
        /// </summary>
        public static readonly BindableProperty DownCommandProperty = BindableProperty.Create(nameof(DownCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for down command parameter
        /// </summary>
        public static readonly BindableProperty DownCommandParameterProperty = BindableProperty.Create(nameof(DownCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// command invoked upon down event
        /// </summary>
        public ICommand DownCommand
        {
            get { return (ICommand)GetValue(DownCommandProperty); }
            set
            {
                var oldHandlesDownState = HandlesDown;
                SetValue(DownCommandProperty, value);
                if (HandlesDown != oldHandlesDownState)
                    HandlesDownChanged?.Invoke(this, !oldHandlesDownState);
            }
        }
        /// <summary>
        /// parameter passed with command invoked upon down event
        /// </summary>
        public object DownCommandParameter
        {
            get => GetValue(DownCommandParameterProperty);
            set => SetValue(DownCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for DownCallback property
        /// </summary>
        public static readonly BindableProperty DownCallbackProperty = BindableProperty.Create(nameof(DownCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for DownCallbackParameter property
        /// </summary>
        public static readonly BindableProperty DownCallbackParameterProperty = BindableProperty.Create(nameof(DownCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked invoked upon down event
        /// </summary>
        public Action<Listener, object> DownCallback
        {
            get { return (Action<Listener, object>)GetValue(DownCallbackProperty); }
            set
            {
                var oldHandlesDownState = HandlesDown;
                SetValue(DownCallbackProperty, value);
                if (HandlesDown != oldHandlesDownState)
                    HandlesDownChanged?.Invoke(this, !oldHandlesDownState);
            }
        }
        /// <summary>
        /// parameter passed with to Action invoked invoked upon down event
        /// </summary>
        public object DownCallbackParameter
        {
            get => GetValue(DownCallbackParameterProperty);
            set => SetValue(DownCallbackParameterProperty, value);
        }
        /// <summary>
        /// returns if Listener is configured to handle down touch 
        /// </summary>
        public bool HandlesDown => _down != null || DownCommand != null || DownCallback != null;

        /// <summary>
        /// Event to notify if the ability to handle down events has changed
        /// </summary>
        public event EventHandler<bool> HandlesDownChanged;

        internal bool OnDown(DownUpEventArgs args)
        {
            var result = false;
            if (HandlesDown)
            {
                RaiseEvent<DownUpEventArgs>(_down, args);
                ExecuteCommand(DownCommand, DownCommandParameter, args);
                ExecuteCallback(DownCallback, DownCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Up
        event EventHandler<DownUpEventArgs> _up;
        /// <summary>
        /// Up event motion handler
        /// </summary>
        public event EventHandler<DownUpEventArgs> Up
        {
            add
            {
                var oldHandlesUp = HandlesUp;
                _up += value;
                if (HandlesUp != oldHandlesUp)
                    HandlesUpChanged?.Invoke(this, !oldHandlesUp);
            }
            remove
            {
                var oldHandlesUp = HandlesUp;
                _up -= value;
                if (HandlesUp != oldHandlesUp)
                    HandlesUpChanged?.Invoke(this, !oldHandlesUp);
            }
        }
        /// <summary>
        /// backing store for UpCommand
        /// </summary>
        public static readonly BindableProperty UpCommandProperty = BindableProperty.Create(nameof(UpCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for UpCommandParameter
        /// </summary>
        public static readonly BindableProperty UpCommandParameterProperty = BindableProperty.Create(nameof(UpCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// command invoked upon up touch
        /// </summary>
        public ICommand UpCommand
        {
            get { return (ICommand)GetValue(UpCommandProperty); }
            set
            {
                var oldHandlesUp = HandlesUp;
                SetValue(UpCommandProperty, value);
                if (HandlesUp != oldHandlesUp)
                    HandlesUpChanged?.Invoke(this, !oldHandlesUp);
            }
        }
        /// <summary>
        /// parameter passed to command invoked upon up touch
        /// </summary>
        public object UpCommandParameter
        {
            get => GetValue(UpCommandParameterProperty);
            set => SetValue(UpCommandParameterProperty, value);
        }
        /// <summary>
        /// Backing store for UpCallback
        /// </summary>
        public static readonly BindableProperty UpCallbackProperty = BindableProperty.Create(nameof(UpCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// Backing store for UpCallbackParameter
        /// </summary>
        public static readonly BindableProperty UpCallbackParameterProperty = BindableProperty.Create(nameof(UpCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked upon up touch
        /// </summary>
        public Action<Listener, object> UpCallback
        {
            get { return (Action<Listener, object>)GetValue(UpCallbackProperty); }
            set
            {
                var oldHandlesUp = HandlesUp;
                SetValue(UpCallbackProperty, value);
                if (HandlesUp != oldHandlesUp)
                    HandlesUpChanged?.Invoke(this, !oldHandlesUp);
            }
        }
        /// <summary>
        /// Parameter passed to Action invoked upon up touch
        /// </summary>
        public object UpCallbackParameter
        {
            get => GetValue(UpCallbackParameterProperty);
            set => SetValue(UpCallbackParameterProperty, value);
        }
        /// <summary>
        /// Does this Listener invoke anything upon an up touch?
        /// </summary>
        public bool HandlesUp => _up != null || UpCommand != null || UpCallback != null;


        /// <summary>
        /// Event triggered when HandlesUp status has changed;
        /// </summary>
        public event EventHandler<bool> HandlesUpChanged;

        internal bool OnUp(DownUpEventArgs args)
        {
            var result = false;
            if (HandlesUp)
            {
                RaiseEvent<DownUpEventArgs>(_up, args);
                ExecuteCommand(UpCommand, UpCommandParameter, args);
                ExecuteCallback(UpCallback, UpCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Tapping
        event EventHandler<TapEventArgs> _tapping;
        /// <summary>
        /// Tapping event handler
        /// </summary>
        public event EventHandler<TapEventArgs> Tapping
        {
            add
            {
                var oldHandlesTapping = HandlesTapping;
                _tapping += value;
                if (oldHandlesTapping != HandlesTapping)
                    HandlesTappingChanged?.Invoke(this, !oldHandlesTapping);
            }
            remove
            {
                var oldHandlesTapping = HandlesTapping;
                _tapping -= value;
                if (oldHandlesTapping != HandlesTapping)
                    HandlesTappingChanged?.Invoke(this, !oldHandlesTapping);
            }
        }
        /// <summary>
        /// backing store for the TappingCommand property
        /// </summary>
        public static readonly BindableProperty TappingCommandProperty = BindableProperty.Create(nameof(TappingCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the TappingCommandParameter property
        /// </summary>
        public static readonly BindableProperty TappingCommandParameterProperty = BindableProperty.Create(nameof(TappingCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked during tap event
        /// </summary>
        public ICommand TappingCommand
        {
            get { return (ICommand)GetValue(TappingCommandProperty); }
            set
            {
                var oldHandlesTapping = HandlesTapping;
                SetValue(TappingCommandProperty, value);
                if (oldHandlesTapping != HandlesTapping)
                    HandlesTappingChanged?.Invoke(this, !oldHandlesTapping);
            }
        }
        /// <summary>
        /// Parameter padded to command invoked during tap event
        /// </summary>
        public object TappingCommandParameter
        {
            get => GetValue(TappingCommandParameterProperty);
            set => SetValue(TappingCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for the TappingCallback property
        /// </summary>
        public static readonly BindableProperty TappingCallbackProperty = BindableProperty.Create(nameof(TappingCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the TappingCallbackParameter property
        /// </summary>
        public static readonly BindableProperty TappingCallbackParameterProperty = BindableProperty.Create(nameof(TappingCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked upon tap event
        /// </summary>
        public Action<Listener, object> TappingCallback
        {
            get => (Action<Listener, object>)GetValue(TappingCallbackProperty);
            set
            {
                var oldHandlesTapping = HandlesTapping;
                SetValue(TappingCallbackProperty, value);
                if (oldHandlesTapping != HandlesTapping)
                    HandlesTappingChanged?.Invoke(this, !oldHandlesTapping);
            }
        }
        /// <summary>
        /// Parameter passed to Action invoked during tap event
        /// </summary>
        public object TappingCallbackParameter
        {
            get => GetValue(TappingCallbackParameterProperty);
            set => SetValue(TappingCallbackParameterProperty, value);
        }
        /// <summary>
        /// does this Listner invoke anything during a tap motion?
        /// </summary>
        public bool HandlesTapping => _tapping != null || TappingCommand != null || TappingCallback != null;


        /// <summary>
        /// Event triggered when HandlesTapping state has changed
        /// </summary>
        public event EventHandler<bool> HandlesTappingChanged;

        internal bool OnTapping(TapEventArgs args)
        {
            var result = false;
            if (HandlesTapping)
            {
                RaiseEvent<TapEventArgs>(_tapping, args);
                ExecuteCommand(TappingCommand, TappingCommandParameter, args);
                ExecuteCallback(TappingCallback, TappingCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Tapped
        event EventHandler<TapEventArgs> _tapped;
        /// <summary>
        /// Tapped event handler
        /// </summary>
        public event EventHandler<TapEventArgs> Tapped
        {
            add
            {
                var oldHandlesTapped = HandlesTapped;
                _tapped += value;
                if (oldHandlesTapped != HandlesTapped)
                    HandlesTappedChanged?.Invoke(this, !oldHandlesTapped);
            }
            remove
            {
                _tapped -= value;
            }
        }
        /// <summary>
        /// backing store for the TappedCommand property
        /// </summary>
        public static readonly BindableProperty TappedCommandProperty = BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the TappedCommandParameter property
        /// </summary>
        public static readonly BindableProperty TappedCommandParameterProperty = BindableProperty.Create(nameof(TappedCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after a tap motion
        /// </summary>
        public ICommand TappedCommand
        {
            get => (ICommand)GetValue(TappedCommandProperty);
            set
            {
                var oldHandlesTapped = HandlesTapped;
                SetValue(TappedCommandProperty, value);
                if (oldHandlesTapped != HandlesTapped)
                    HandlesTappedChanged?.Invoke(this, !oldHandlesTapped);
            }
        }
        /// <summary>
        /// Parameter passed with Command invoked after a tap motion
        /// </summary>
        public object TappedCommandParameter
        {
            get => GetValue(TappedCommandParameterProperty);
            set => SetValue(TappedCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for a TappedCallback property
        /// </summary>
        public static readonly BindableProperty TappedCallbackProperty = BindableProperty.Create(nameof(TappedCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for a TappedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty TappedCallbackParameterProperty = BindableProperty.Create(nameof(TappedCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after a tap motion
        /// </summary>
        public Action<Listener, object> TappedCallback
        {
            get => (Action<Listener, object>)GetValue(TappedCallbackProperty);
            set
            {
                var oldHandlesTapped = HandlesTapped;
                SetValue(TappedCallbackProperty, value);
                if (oldHandlesTapped != HandlesTapped)
                    HandlesTappedChanged?.Invoke(this, !oldHandlesTapped);
            }
        }
        /// <summary>
        /// Parameter passed to Action invoked after a tap motion
        /// </summary>
        public object TappedCallbackParameter
        {
            get => GetValue(TappedCallbackParameterProperty);
            set
            {
                SetValue(TappedCallbackParameterProperty, value);
            }
        }
        /// <summary>
        /// does this Listener invoke anything after a tap motion?
        /// </summary>
        public bool HandlesTapped => _tapped != null || TappedCommand != null || TappedCallback != null;


        /// <summary>
        /// Event triggered when HandlesTapped has changed
        /// </summary>
        public event EventHandler<bool> HandlesTappedChanged;

        internal bool OnTapped(TapEventArgs args)
        {
            var result = false;
            if (HandlesTapped)
            {
                RaiseEvent<TapEventArgs>(_tapped, args);
                ExecuteCommand(TappedCommand, TappedCommandParameter, args);
                ExecuteCallback(TappedCallback, TappedCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region DoubleTapped
        event EventHandler<TapEventArgs> _doubleTapped;

        /// <summary>
        /// DoubleTapped event handler
        /// </summary>
        public event EventHandler<TapEventArgs> DoubleTapped
        {
            add
            {
                var oldHandlesDoubleTapped = HandlesDoubleTapped;
                _doubleTapped += value;
                if (oldHandlesDoubleTapped != HandlesDoubleTapped)
                    HandlesDoubleTappedChanged?.Invoke(this, !oldHandlesDoubleTapped);
            }
            remove
            {
                var oldHandlesDoubleTapped = HandlesDoubleTapped;
                _doubleTapped -= value;
                if (oldHandlesDoubleTapped != HandlesDoubleTapped)
                    HandlesDoubleTappedChanged?.Invoke(this, !oldHandlesDoubleTapped);
            }
        }
        /// <summary>
        /// backing store for the DoubleTappedCommand property
        /// </summary>
        public static readonly BindableProperty DoubleTappedCommandProperty = BindableProperty.Create(nameof(DoubleTappedCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the DoubleTappedCommmandParameter property
        /// </summary>
        public static readonly BindableProperty DoubleTappedCommandParameterProperty = BindableProperty.Create(nameof(DoubleTappedCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after a double tap motion
        /// </summary>
        public ICommand DoubleTappedCommand
        {
            get => (ICommand)GetValue(DoubleTappedCommandProperty);
            set
            {
                var oldHandlesDoubleTapped = HandlesDoubleTapped;
                SetValue(DoubleTappedCommandProperty, value);
                if (oldHandlesDoubleTapped != HandlesDoubleTapped)
                    HandlesDoubleTappedChanged?.Invoke(this, !oldHandlesDoubleTapped);
            }
        }
        /// <summary>
        /// Parameter sent with Command invoked after a double tap motion
        /// </summary>
        public object DoubleTappedCommandParameter
        {
            get => GetValue(DoubleTappedCommandParameterProperty);
            set => SetValue(DoubleTappedCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for DoubleTappedCallback property
        /// </summary>
        public static readonly BindableProperty DoubleTappedCallbackProperty = BindableProperty.Create(nameof(DoubleTappedCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for DoubleTappedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty DoubleTappedCallbackParameterProperty = BindableProperty.Create(nameof(DoubleTappedCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after a double tap motion
        /// </summary>
        public Action<Listener, object> DoubleTappedCallback
        {
            get => (Action<Listener, object>)GetValue(DoubleTappedCallbackProperty);
            set
            {
                var oldHandlesDoubleTapped = HandlesDoubleTapped;
                SetValue(DoubleTappedCallbackProperty, value);
                if (oldHandlesDoubleTapped != HandlesDoubleTapped)
                    HandlesDoubleTappedChanged?.Invoke(this, !oldHandlesDoubleTapped);
            }
        }
        /// <summary>
        /// Parameter sent to Action invoked after a double tap motion
        /// </summary>
        public object DoubleTappedCallbackParameter
        {
            get => GetValue(DoubleTappedCallbackParameterProperty);
            set => SetValue(DoubleTappedCallbackParameterProperty, value);
        }
        /// <summary>
        /// does this Listener invoke anything upon double tap motion?
        /// </summary>
        public bool HandlesDoubleTapped => _doubleTapped != null || DoubleTappedCommand != null || DoubleTappedCallback != null;

        /// <summary>
        /// Occurs when handles double tapped changed.
        /// </summary>
        public event EventHandler<bool> HandlesDoubleTappedChanged;

        internal bool OnDoubleTapped(TapEventArgs args)
        {
            var result = false;
            if (HandlesDoubleTapped)
            {
                RaiseEvent<TapEventArgs>(_doubleTapped, args);
                ExecuteCommand(DoubleTappedCommand, DoubleTappedCommandParameter, args);
                ExecuteCallback(DoubleTappedCallback, DoubleTappedCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region LongPressing
        event EventHandler<LongPressEventArgs> _longPressing;

        /// <summary>
        /// LongPressing event handler
        /// </summary>
        public event EventHandler<LongPressEventArgs> LongPressing
        {
            add
            {
                var oldHandlesLongPressing = HandlesLongPressing;
                _longPressing += value;
                if (oldHandlesLongPressing != HandlesLongPressing)
                    HandlesLongPressingChanged?.Invoke(this, !oldHandlesLongPressing);
            }
            remove
            {
                var oldHandlesLongPressing = HandlesLongPressing;
                _longPressing -= value;
                if (oldHandlesLongPressing != HandlesLongPressing)
                    HandlesLongPressingChanged?.Invoke(this, !oldHandlesLongPressing);
            }
        }
        /// <summary>
        /// backing store for LongPressingCommand property
        /// </summary>
        public static readonly BindableProperty LongPressingCommandProperty = BindableProperty.Create(nameof(LongPressingCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for LongPressingCommandParameter property
        /// </summary>
        public static readonly BindableProperty LongPressingCommandParameterProperty = BindableProperty.Create(nameof(LongPressingCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked during long pressing motion
        /// </summary>
        public ICommand LongPressingCommand
        {
            get => (ICommand)GetValue(LongPressingCommandProperty);
            set
            {
                var oldHandlesLongPressing = HandlesLongPressing;
                SetValue(LongPressingCommandProperty, value);
                if (oldHandlesLongPressing != HandlesLongPressing)
                    HandlesLongPressingChanged?.Invoke(this, !oldHandlesLongPressing);
            }
        }
        /// <summary>
        /// Parameter sent to Command invoked during long pressing motion
        /// </summary>
        public object LongPressingCommandParameter
        {
            get => GetValue(LongPressingCommandParameterProperty);
            set => SetValue(LongPressingCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for LongPressingCallback property
        /// </summary>
        public static readonly BindableProperty LongPressingCallbackProperty = BindableProperty.Create(nameof(LongPressingCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for LongPressingCallbackParameter property
        /// </summary>
        public static readonly BindableProperty LongPressingCallbackParameterProperty = BindableProperty.Create(nameof(LongPressingCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked during long pressing motion
        /// </summary>
        public Action<Listener, object> LongPressingCallback
        {
            get => (Action<Listener, object>)GetValue(LongPressingCallbackProperty);
            set
            {
                var oldHandlesLongPressing = HandlesLongPressing;
                SetValue(LongPressingCallbackProperty, value);
                if (oldHandlesLongPressing != HandlesLongPressing)
                    HandlesLongPressingChanged?.Invoke(this, !oldHandlesLongPressing);
            }
        }
        /// <summary>
        /// Parameter sent to Action invoked during long pressing motion
        /// </summary>
        public object LongPressingCallbackParameter
        {
            get => GetValue(LongPressingCallbackParameterProperty);
            set => SetValue(LongPressingCallbackParameterProperty, value);
        }
        /// <summary>
        /// Does this Listner invoke anything during long press motion?
        /// </summary>
        public bool HandlesLongPressing
        {
            get
            {
                var result = _longPressing != null || LongPressingCommand != null || LongPressingCallback != null;
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine("Listener[" + Element + "].HandlesLongPressing=[" + result + "]");
                return result;
            }
        }

        /// <summary>
        /// Event triggered when HandlesLongPressing has changed
        /// </summary>
        public event EventHandler<bool> HandlesLongPressingChanged;

        internal bool OnLongPressing(LongPressEventArgs args)
        {
            var result = false;
            if (HandlesLongPressing)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<LongPressEventArgs>(_longPressing, args);
                ExecuteCommand(LongPressingCommand, LongPressingCommandParameter, args);
                ExecuteCallback(LongPressingCallback, LongPressingCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region LongPressed
        event EventHandler<LongPressEventArgs> _longPressed;

        /// <summary>
        /// LongPressed event handler
        /// </summary>
        public event EventHandler<LongPressEventArgs> LongPressed
        {
            add
            {
                var oldHandlesLongPressed = HandlesLongPressed;
                _longPressed += value;
                if (oldHandlesLongPressed != HandlesLongPressed)
                    HandlesLongPressedChanged?.Invoke(this, !oldHandlesLongPressed);
            }
            remove
            {
                var oldHandlesLongPressed = HandlesLongPressed;
                _longPressed -= value;
                if (oldHandlesLongPressed != HandlesLongPressed)
                    HandlesLongPressedChanged?.Invoke(this, !oldHandlesLongPressed);
            }
        }
        /// <summary>
        /// backing store for LongPressedCommand property
        /// </summary>
        public static readonly BindableProperty LongPressedCommandProperty = BindableProperty.Create(nameof(LongPressedCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for LongPressedCommandParameter property
        /// </summary>
        public static readonly BindableProperty LongPressedCommandParameterProperty = BindableProperty.Create(nameof(LongPressedCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after long press motion
        /// </summary>
        public ICommand LongPressedCommand
        {
            get => (ICommand)GetValue(LongPressedCommandProperty);
            set
            {
                var oldHandlesLongPressed = HandlesLongPressed;
                SetValue(LongPressedCommandProperty, value);
                if (oldHandlesLongPressed != HandlesLongPressed)
                    HandlesLongPressedChanged?.Invoke(this, !oldHandlesLongPressed);
            }
        }
        /// <summary>
        /// Parameter sent with Command invoked after long press motion
        /// </summary>
        public object LongPressedCommandParameter
        {
            get => GetValue(LongPressedCommandParameterProperty);
            set => SetValue(LongPressedCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for LongPressedCallback property
        /// </summary>
        public static readonly BindableProperty LongPressedCallbackProperty = BindableProperty.Create(nameof(LongPressedCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for LongPressedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty LongPressedCallbackParameterProperty = BindableProperty.Create(nameof(LongPressedCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after long press motion
        /// </summary>
        public Action<Listener, object> LongPressedCallback
        {
            get => (Action<Listener, object>)GetValue(LongPressedCallbackProperty);
            set
            {
                var oldHandlesLongPressed = HandlesLongPressed;
                SetValue(LongPressedCallbackProperty, value);
                if (oldHandlesLongPressed != HandlesLongPressed)
                    HandlesLongPressedChanged?.Invoke(this, !oldHandlesLongPressed);
            }
        }
        /// <summary>
        /// Parameter sent with Action invoked after long press motion
        /// </summary>
        public object LongPressedCallbackParameter
        {
            get => GetValue(LongPressedCallbackParameterProperty);
            set => SetValue(LongPressedCallbackParameterProperty, value);
        }
        /// <summary>
        /// Does this Listener invoke anything after a long press
        /// </summary>
        public bool HandlesLongPressed
        {
            get
            {
                var result = _longPressed != null || LongPressedCommand != null || LongPressedCallback != null;
                //System.Diagnostics.Debug.WriteLine("Listener[" + Element + "].HandlesLongPressed=[" + result + "]");
                return result;
            }

        }


        /// <summary>
        /// Event triggered when HandlesLongPressed had changed
        /// </summary>
        public event EventHandler<bool> HandlesLongPressedChanged;


        internal bool OnLongPressed(LongPressEventArgs args)
        {
            var result = false;
            if (HandlesLongPressed)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<LongPressEventArgs>(_longPressed, args);
                ExecuteCommand(LongPressedCommand, LongPressedCommandParameter, args);
                ExecuteCallback(LongPressedCallback, LongPressingCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Pinching
        event EventHandler<PinchEventArgs> _pinching;

        /// <summary>
        /// Pinching event listener
        /// </summary>
        public event EventHandler<PinchEventArgs> Pinching
        {
            add
            {
                var oldHandlesPinching = HandlesPinching;
                _pinching += value;
                if (oldHandlesPinching != HandlesPinching)
                    HandlesPinchingChanged?.Invoke(this, !oldHandlesPinching);
            }
            remove
            {
                var oldHandlesPinching = HandlesPinching;
                _pinching -= value;
                if (oldHandlesPinching != HandlesPinching)
                    HandlesPinchingChanged?.Invoke(this, !oldHandlesPinching);
            }
        }
        /// <summary>
        /// backing store for the PinchingCommand property
        /// </summary>
        public static readonly BindableProperty PinchingCommandProperty = BindableProperty.Create(nameof(PinchingCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the PinchingCommandParameter property
        /// </summary>
        public static readonly BindableProperty PinchingCommandParameterProperty = BindableProperty.Create(nameof(PinchingCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked during pinch motion
        /// </summary>
        public ICommand PinchingCommand
        {
            get => (ICommand)GetValue(PinchingCommandProperty);
            set
            {
                var oldHandlesPinching = HandlesPinching;
                SetValue(PinchingCommandProperty, value);
                if (oldHandlesPinching != HandlesPinching)
                    HandlesPinchingChanged?.Invoke(this, !oldHandlesPinching);
            }
        }
        /// <summary>
        /// Parameter sent with Command invoked during pinch motion
        /// </summary>
        public object PinchingCommandParameter
        {
            get => GetValue(PinchingCommandParameterProperty);
            set => SetValue(PinchingCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for the PinchingCallback property
        /// </summary>
        public static readonly BindableProperty PinchingCallbackProperty = BindableProperty.Create(nameof(PinchingCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the PinchingCallbackParameter property
        /// </summary>
        public static readonly BindableProperty PinchingCallbackParameterProperty = BindableProperty.Create(nameof(PinchingCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked during pinch motion
        /// </summary>
        public Action<Listener, object> PinchingCallback
        {
            get => (Action<Listener, object>)GetValue(PinchingCallbackProperty);
            set
            {
                var oldHandlesPinching = HandlesPinching;
                SetValue(PinchingCallbackProperty, value);
                if (oldHandlesPinching != HandlesPinching)
                    HandlesPinchingChanged?.Invoke(this, !oldHandlesPinching);
            }
        }
        /// <summary>
        /// Parameter sent to Action invoked during pinch motion
        /// </summary>
        public object PinchingCallbackParameter
        {
            get => GetValue(PinchingCallbackParameterProperty);
            set => SetValue(PinchingCallbackParameterProperty, value);
        }
        /// <summary>
        /// does this Listener invoke anything during pinch motion?
        /// </summary>
        public bool HandlesPinching => _pinching != null || PinchingCommand != null || PinchingCallback != null;

        /// <summary>
        /// Event triggered when HandlesPinching has changed
        /// </summary>
        public event EventHandler<bool> HandlesPinchingChanged;


        internal bool OnPinching(PinchEventArgs args)
        {
            var result = false;
            if (HandlesPinching)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<PinchEventArgs>(_pinching, args);
                ExecuteCommand(PinchingCommand, PinchingCommandParameter, args);
                ExecuteCallback(PinchingCallback, PinchingCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Pinched
        event EventHandler<PinchEventArgs> _pinched;

        /// <summary>
        /// Pinched event handler
        /// </summary>
        public event EventHandler<PinchEventArgs> Pinched
        {
            add
            {
                var oldHandlesPinched = HandlesPinched;
                _pinched += value;
                if (oldHandlesPinched != HandlesPinched)
                    HandlesPinchedChanged?.Invoke(this, !oldHandlesPinched);
            }
            remove
            {
                var oldHandlesPinched = HandlesPinched;
                _pinched -= value;
                if (oldHandlesPinched != HandlesPinched)
                    HandlesPinchedChanged?.Invoke(this, !oldHandlesPinched);
            }
        }
        /// <summary>
        /// backing store for the PinchedCommand property
        /// </summary>
        public static readonly BindableProperty PinchedCommandProperty = BindableProperty.Create(nameof(PinchedCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the PinchedCommandParameter property
        /// </summary>
        public static readonly BindableProperty PinchedCommandParameterProperty = BindableProperty.Create(nameof(PinchedCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after pinch motion
        /// </summary>
        public ICommand PinchedCommand
        {
            get => (ICommand)GetValue(PinchedCommandProperty);
            set
            {
                var oldHandlesPinched = HandlesPinched;
                SetValue(PinchedCommandProperty, value);
                if (oldHandlesPinched != HandlesPinched)
                    HandlesPinchedChanged?.Invoke(this, !oldHandlesPinched);
            }
        }
        /// <summary>
        /// Parameter sent with Command invoked after pinch motion
        /// </summary>
        public object PinchedCommandParameter
        {
            get => GetValue(PinchedCommandParameterProperty);
            set => SetValue(PinchedCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for the PinchedCallback property
        /// </summary>
        public static readonly BindableProperty PinchedCallbackProperty = BindableProperty.Create(nameof(PinchedCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the PinchedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty PinchedCallbackParameterProperty = BindableProperty.Create(nameof(PinchedCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after pinch motion
        /// </summary>
        public Action<Listener, object> PinchedCallback
        {
            get => (Action<Listener, object>)GetValue(PinchedCallbackProperty);
            set
            {
                var oldHandlesPinched = HandlesPinched;
                SetValue(PinchedCallbackProperty, value);
                if (oldHandlesPinched != HandlesPinched)
                    HandlesPinchedChanged?.Invoke(this, !oldHandlesPinched);
            }
        }
        /// <summary>
        /// Parameter passed to Action invoked after pinch motion
        /// </summary>
        public object PinchedCallbackParameter
        {
            get => GetValue(PinchedCallbackParameterProperty);
            set => SetValue(PinchedCallbackParameterProperty, value);
        }
        /// <summary>
        /// Does this Listener invoke anything after pinch motion
        /// </summary>
        public bool HandlesPinched => _pinched != null || PinchedCommand != null || PinchedCallback != null;

        /// <summary>
        /// Event triggered when HandlesPinched has changed
        /// </summary>
        public event EventHandler<bool> HandlesPinchedChanged;


        internal bool OnPinched(PinchEventArgs args)
        {
            var result = false;
            if (HandlesPinched)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<PinchEventArgs>(_pinched, args);
                ExecuteCommand(PinchedCommand, PinchedCommandParameter, args);
                ExecuteCallback(PinchedCallback, PinchingCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Panning
        event EventHandler<PanEventArgs> _panning;

        /// <summary>
        /// Panning event handler
        /// </summary>
        public event EventHandler<PanEventArgs> Panning
        {
            add
            {
                var oldHandlesPanning = HandlesPanning;
                _panning += value;
                if (oldHandlesPanning != HandlesPanning)
                    HandlesPanningChanged?.Invoke(this, !oldHandlesPanning);
            }
            remove
            {
                var oldHandlesPanning = HandlesPanning;
                _panning -= value;
                if (oldHandlesPanning != HandlesPanning)
                    HandlesPanningChanged?.Invoke(this, !oldHandlesPanning);
            }
        }
        /// <summary>
        /// backing store for the PanningCommand parameter
        /// </summary>
        public static readonly BindableProperty PanningCommandProperty = BindableProperty.Create(nameof(PanningCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the PanningCommandParameter parameter
        /// </summary>
        public static readonly BindableProperty PanningCommandParameterProperty = BindableProperty.Create(nameof(PanningCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked duing pan motion
        /// </summary>
        public ICommand PanningCommand
        {
            get => (ICommand)GetValue(PanningCommandProperty);
            set
            {
                var oldHandlesPanning = HandlesPanning;
                SetValue(PanningCommandProperty, value);
                if (oldHandlesPanning != HandlesPanning)
                    HandlesPanningChanged?.Invoke(this, !oldHandlesPanning);
            }
        }
        /// <summary>
        /// Parameter sent with Command invoked duing pan motion
        /// </summary>
        public object PanningCommandParameter
        {
            get => GetValue(PanningCommandParameterProperty);
            set => SetValue(PanningCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for the PanningCallback parameter
        /// </summary>
        public static readonly BindableProperty PanningCallbackProperty = BindableProperty.Create(nameof(PanningCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the PanningCallbackParameter parameter
        /// </summary>
        public static readonly BindableProperty PanningCallbackParameterProperty = BindableProperty.Create(nameof(PanningCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked duing pan motion
        /// </summary>
        public Action<Listener, object> PanningCallback
        {
            get => (Action<Listener, object>)GetValue(PanningCallbackProperty);
            set
            {
                var oldHandlesPanning = HandlesPanning;
                SetValue(PanningCallbackProperty, value);
                if (oldHandlesPanning != HandlesPanning)
                    HandlesPanningChanged?.Invoke(this, !oldHandlesPanning);
            }
        }
        /// <summary>
        /// Parameter sent to Action invoked duing pan motion
        /// </summary>
        public object PanningCallbackParameter
        {
            get => GetValue(PanningCallbackParameterProperty);
            set => SetValue(PanningCallbackParameterProperty, value);
        }
        /// <summary>
        /// Does Listener invoke anything during pan motion?
        /// </summary>
        public bool HandlesPanning
        {
            get
            {
                var result = _panning != null || PanningCommand != null || PanningCallback != null;
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine("Listener[" + Element + "].HandlesPanning=[" + result + "]");
                return result;
            }
        }

        /// <summary>
        /// Event triggered when HandlesPanning has changed
        /// </summary>
        public event EventHandler<bool> HandlesPanningChanged;


        internal bool OnPanning(PanEventArgs args)
        {
            var result = false;
            if (HandlesPanning)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<PanEventArgs>(_panning, args);
                ExecuteCommand(PanningCommand, PanningCommandParameter, args);
                ExecuteCallback(PanningCallback, PanningCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Panned
        event EventHandler<PanEventArgs> _panned;

        /// <summary>
        /// Pannded event handler
        /// </summary>
        public event EventHandler<PanEventArgs> Panned
        {
            add
            {
                var oldHandlesPanned = HandlesPanned;
                _panned += value;
                if (oldHandlesPanned != HandlesPanned)
                    HandlesPannedChanged?.Invoke(this, !oldHandlesPanned);
            }
            remove
            {
                var oldHandlesPanned = HandlesPanned;
                _panned -= value;
                if (oldHandlesPanned != HandlesPanned)
                    HandlesPannedChanged?.Invoke(this, !oldHandlesPanned);
            }
        }
        /// <summary>
        /// backing store for the PannedCommand property
        /// </summary>
        public static readonly BindableProperty PannedCommandProperty = BindableProperty.Create(nameof(PannedCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the PannedCommandParameter property
        /// </summary>
        public static readonly BindableProperty PannedCommandParameterProperty = BindableProperty.Create(nameof(PannedCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after pan motion
        /// </summary>
        public ICommand PannedCommand
        {
            get => (ICommand)GetValue(PannedCommandProperty);
            set
            {
                var oldHandlesPanned = HandlesPanned;
                SetValue(PannedCommandProperty, value);
                if (oldHandlesPanned != HandlesPanned)
                    HandlesPannedChanged?.Invoke(this, !oldHandlesPanned);
            }
        }
        /// <summary>
        /// Parameter sent with Command invoked after pan motion
        /// </summary>
        public object PannedCommandParameter
        {
            get => GetValue(PannedCommandParameterProperty);
            set => SetValue(PannedCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for the PanndedCallback property
        /// </summary>
        public static readonly BindableProperty PannedCallbackProperty = BindableProperty.Create(nameof(PannedCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the PanndedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty PannedCallbackParameterProperty = BindableProperty.Create(nameof(PannedCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after pan motion
        /// </summary>
        public Action<Listener, object> PannedCallback
        {
            get => (Action<Listener, object>)GetValue(PannedCallbackProperty);
            set
            {
                var oldHandlesPanned = HandlesPanned;
                SetValue(PannedCallbackProperty, value);
                if (oldHandlesPanned != HandlesPanned)
                    HandlesPannedChanged?.Invoke(this, !oldHandlesPanned);
            }
        }
        /// <summary>
        /// Parameter sent with Action invoked after pan motion
        /// </summary>
        public object PannedCallbackParameter
        {
            get => GetValue(PannedCallbackParameterProperty);
            set => SetValue(PannedCallbackParameterProperty, value);
        }
        /// <summary>
        /// Does this Listener invoke anything after pan motion?
        /// </summary>
        public bool HandlesPanned => _panned != null || PannedCommand != null || PannedCallback != null;

        /// <summary>
        /// Event triggered when HandlesPanned has changed
        /// </summary>
        public event EventHandler<bool> HandlesPannedChanged;


        internal bool OnPanned(PanEventArgs args)
        {
            var result = false;
            if (HandlesPanned)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<PanEventArgs>(_panned, args);
                ExecuteCommand(PannedCommand, PannedCommandParameter, args);
                ExecuteCallback(PannedCallback, PannedCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Swiped
        event EventHandler<SwipeEventArgs> _swiped;

        /// <summary>
        /// Swiped event handler
        /// </summary>
        public event EventHandler<SwipeEventArgs> Swiped
        {
            add
            {
                var oldHandlesSwiped = HandlesSwiped;
                _swiped += value;
                if (oldHandlesSwiped != HandlesSwiped)
                    HandlesSwipedChanged?.Invoke(this, !oldHandlesSwiped);
            }
            remove
            {
                var oldHandlesSwiped = HandlesSwiped;
                _swiped -= value;
                if (oldHandlesSwiped != HandlesSwiped)
                    HandlesSwipedChanged?.Invoke(this, !oldHandlesSwiped);
            }
        }

        /// <summary>
        /// backing store for the SwipedCommand property
        /// </summary>
        public static readonly BindableProperty SwipedCommandProperty = BindableProperty.Create(nameof(SwipedCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the SwipedCommandParameter property
        /// </summary>
        public static readonly BindableProperty SwipedCommandParameterProperty = BindableProperty.Create(nameof(SwipedCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after swipe motion
        /// </summary>
        public ICommand SwipedCommand
        {
            get => (ICommand)GetValue(SwipedCommandProperty);
            set
            {
                var oldHandlesSwiped = HandlesSwiped;
                SetValue(SwipedCommandProperty, value);
                if (oldHandlesSwiped != HandlesSwiped)
                    HandlesSwipedChanged?.Invoke(this, !oldHandlesSwiped);
            }
        }
        /// <summary>
        /// Parameter sent with Command invoked after swipe motion
        /// </summary>
        public object SwipedCommandParameter
        {
            get => GetValue(SwipedCommandParameterProperty);
            set => SetValue(SwipedCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for the SwipedCallback property
        /// </summary>
        public static readonly BindableProperty SwipedCallbackProperty = BindableProperty.Create(nameof(SwipedCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the SwipedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty SwipedCallbackParameterProperty = BindableProperty.Create(nameof(SwipedCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after swipe motion
        /// </summary>
        public Action<Listener, object> SwipedCallback
        {
            get => (Action<Listener, object>)GetValue(SwipedCallbackProperty);
            set
            {
                var oldHandlesSwiped = HandlesSwiped;
                SetValue(SwipedCallbackProperty, value);
                if (oldHandlesSwiped != HandlesSwiped)
                    HandlesSwipedChanged?.Invoke(this, !oldHandlesSwiped);
            }
        }
        /// <summary>
        /// Parameter sent with Action invoked after swipe motion
        /// </summary>
        public object SwipedCallbackParameter
        {
            get => GetValue(SwipedCallbackParameterProperty);
            set => SetValue(SwipedCallbackParameterProperty, value);
        }
        /// <summary>
        /// Does this Listener invoke anything after swipe motion
        /// </summary>
        public bool HandlesSwiped => _swiped != null || SwipedCommand != null || SwipedCallback != null;

        /// <summary>
        /// Event triggered when HandlesSwiped has changed
        /// </summary>
        public event EventHandler<bool> HandlesSwipedChanged;


        internal bool OnSwiped(SwipeEventArgs args)
        {
            var result = false;
            if (HandlesSwiped)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<SwipeEventArgs>(_swiped, args);
                ExecuteCommand(SwipedCommand, SwipedCommandParameter, args);
                ExecuteCallback(SwipedCallback, SwipedCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Rotating
        event EventHandler<RotateEventArgs> _rotating;
        /// <summary>
        /// Rotating event handler
        /// </summary>
        public event EventHandler<RotateEventArgs> Rotating
        {
            add
            {
                var oldHandlesRotating = HandlesRotating;
                _rotating += value;
                if (oldHandlesRotating != HandlesRotating)
                    HandlesRotatingChanged?.Invoke(this, !oldHandlesRotating);
            }
            remove
            {
                var oldHandlesRotating = HandlesRotating;
                _rotating -= value;
                if (oldHandlesRotating != HandlesRotating)
                    HandlesRotatingChanged?.Invoke(this, !oldHandlesRotating);
            }
        }
        /// <summary>
        /// backing store for the RotatingCommand property
        /// </summary>
        public static readonly BindableProperty RotatingCommandProperty = BindableProperty.Create(nameof(RotatingCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the RotatingCommandParameter property
        /// </summary>
        public static readonly BindableProperty RotatingCommandParameterProperty = BindableProperty.Create(nameof(RotatingCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked during rotation motion
        /// </summary>
        public ICommand RotatingCommand
        {
            get => (ICommand)GetValue(RotatingCommandProperty);
            set
            {
                var oldHandlesRotating = HandlesRotating;
                SetValue(RotatingCommandProperty, value);
                if (oldHandlesRotating != HandlesRotating)
                    HandlesRotatingChanged?.Invoke(this, !oldHandlesRotating);
            }
        }
        /// <summary>
        /// Parameter sent with Command invoked during rotation motion
        /// </summary>
        public object RotatingCommandParameter
        {
            get => GetValue(RotatingCommandParameterProperty);
            set => SetValue(RotatingCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for the RotatingCallback property
        /// </summary>
        public static readonly BindableProperty RotatingCallbackProperty = BindableProperty.Create(nameof(RotatingCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the RotatingCallbackParameter property
        /// </summary>
        public static readonly BindableProperty RotatingCallbackParameterProperty = BindableProperty.Create(nameof(RotatingCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked during rotation motion
        /// </summary>
        public Action<Listener, object> RotatingCallback
        {
            get => (Action<Listener, object>)GetValue(RotatingCallbackProperty);
            set
            {
                var oldHandlesRotating = HandlesRotating;
                SetValue(RotatingCallbackProperty, value);
                if (oldHandlesRotating != HandlesRotating)
                    HandlesRotatingChanged?.Invoke(this, !oldHandlesRotating);
            }
        }
        /// <summary>
        /// Parameter sent with Action invoked during rotation motion
        /// </summary>
        public object RotatingCallbackParameter
        {
            get => GetValue(RotatingCallbackParameterProperty);
            set => SetValue(RotatingCallbackParameterProperty, value);
        }
        /// <summary>
        /// Does Listener invoke anything during rotation motion?
        /// </summary>
        public bool HandlesRotating => _rotating != null || RotatingCommand != null || RotatingCallback != null;

        /// <summary>
        /// Event trigged when HandlesRotating has changed
        /// </summary>
        public event EventHandler<bool> HandlesRotatingChanged;


        internal bool OnRotating(RotateEventArgs args)
        {
            var result = false;
            if (HandlesRotating)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<RotateEventArgs>(_rotating, args);
                ExecuteCommand(RotatingCommand, RotatingCommandParameter, args);
                ExecuteCallback(RotatingCallback, RotatingCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Rotated
        event EventHandler<RotateEventArgs> _rotated;
        /// <summary>
        /// Rotated event handler
        /// </summary>
        public event EventHandler<RotateEventArgs> Rotated
        {
            add
            {
                var oldHandlesRotated = HandlesRotated;
                _rotated += value;
                if (oldHandlesRotated != HandlesRotated)
                    HandlesRotatedChanged?.Invoke(this, !oldHandlesRotated);
            }
            remove
            {
                var oldHandlesRotated = HandlesRotated;
                _rotated -= value;
                if (oldHandlesRotated != HandlesRotated)
                    HandlesRotatedChanged?.Invoke(this, !oldHandlesRotated);
            }
        }
        /// <summary>
        /// backing store for the RotatedCommand property
        /// </summary>
        public static readonly BindableProperty RotatedCommandProperty = BindableProperty.Create(nameof(RotatedCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the RotatedCommandParameter property
        /// </summary>
        public static readonly BindableProperty RotatedCommandParameterProperty = BindableProperty.Create(nameof(RotatedCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after rotation motion
        /// </summary>
        public ICommand RotatedCommand
        {
            get => (ICommand)GetValue(RotatedCommandProperty);
            set
            {
                var oldHandlesRotated = HandlesRotated;
                SetValue(RotatedCommandProperty, value);
                if (oldHandlesRotated != HandlesRotated)
                    HandlesRotatedChanged?.Invoke(this, !oldHandlesRotated);
            }
        }
        /// <summary>
        /// Parameter sent with Command invoked after rotation motion
        /// </summary>
        public object RotatedCommandParameter
        {
            get => GetValue(RotatedCommandParameterProperty);
            set => SetValue(RotatedCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for the RotatedCallback property
        /// </summary>
        public static readonly BindableProperty RotatedCallbackProperty = BindableProperty.Create(nameof(RotatedCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the RotatedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty RotatedCallbackParameterProperty = BindableProperty.Create(nameof(RotatedCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after rotation motion
        /// </summary>
        public Action<Listener, object> RotatedCallback
        {
            get => (Action<Listener, object>)GetValue(RotatedCallbackProperty);
            set
            {
                var oldHandlesRotated = HandlesRotated;
                SetValue(RotatedCallbackProperty, value);
                if (oldHandlesRotated != HandlesRotated)
                    HandlesRotatedChanged?.Invoke(this, !oldHandlesRotated);
            }
        }
        /// <summary>
        /// Parameter sent with Action invoked after rotation motion
        /// </summary>
        public object RotatedCallbackParameter
        {
            get => GetValue(RotatedCallbackParameterProperty);
            set => SetValue(RotatedCallbackParameterProperty, value);
        }
        /// <summary>
        /// Does Listener invoke anything after rotation motion?
        /// </summary>
        public bool HandlesRotated => _rotated != null || RotatedCommand != null || RotatedCallback != null;

        /// <summary>
        /// Event trigged when HandlesRotated has changed
        /// </summary>
        public event EventHandler<bool> HandlesRotatedChanged;


        internal bool OnRotated(RotateEventArgs args)
        {
            var result = false;
            if (HandlesRotated)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<RotateEventArgs>(_rotated, args);
                ExecuteCommand(RotatedCommand, RotatedCommandParameter, args);
                ExecuteCallback(RotatedCallback, RotatingCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region RightClicked
        event EventHandler<RightClickEventArgs> _rightClicked;
        /// <summary>
        /// Tapped event handler
        /// </summary>
        public event EventHandler<RightClickEventArgs> RightClicked
        {
            add
            {
                var oldHandlesRightClicked = HandlesRightClicked;
                _rightClicked += value;
                if (oldHandlesRightClicked != HandlesRightClicked)
                    HandlesRightClickedChanged?.Invoke(this, !oldHandlesRightClicked);
            }
            remove
            {
                var oldHandlesRightClicked = HandlesRightClicked;
                _rightClicked -= value;
                if (oldHandlesRightClicked != HandlesRightClicked)
                    HandlesRightClickedChanged?.Invoke(this, !oldHandlesRightClicked);
            }
        }
        /// <summary>
        /// backing store for the TappedCommand property
        /// </summary>
        public static readonly BindableProperty RightClickedCommandProperty = BindableProperty.Create(nameof(RightClickedCommand), typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the TappedCommandParameter property
        /// </summary>
        public static readonly BindableProperty RightClickedCommandParameterProperty = BindableProperty.Create(nameof(RightClickedCommandParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after a tap motion
        /// </summary>
        public ICommand RightClickedCommand
        {
            get => (ICommand)GetValue(RightClickedCommandProperty);
            set
            {
                var oldHandlesRightClicked = HandlesRightClicked;
                SetValue(RightClickedCommandProperty, value);
                if (oldHandlesRightClicked != HandlesRightClicked)
                    HandlesRightClickedChanged?.Invoke(this, !oldHandlesRightClicked);
            }
        }
        /// <summary>
        /// Parameter passed with Command invoked after a tap motion
        /// </summary>
        public object RightClickedCommandParameter
        {
            get => GetValue(RightClickedCommandParameterProperty);
            set => SetValue(RightClickedCommandParameterProperty, value);
        }
        /// <summary>
        /// backing store for a TappedCallback property
        /// </summary>
        public static readonly BindableProperty RightClickedCallbackProperty = BindableProperty.Create(nameof(RightClickedCallback), typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for a TappedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty RightClickedCallbackParameterProperty = BindableProperty.Create(nameof(RightClickedCallbackParameter), typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after a tap motion
        /// </summary>
        public Action<Listener, object> RightClickedCallback
        {
            get => (Action<Listener, object>)GetValue(RightClickedCallbackProperty);
            set
            {
                var oldHandlesRightClicked = HandlesRightClicked;
                SetValue(RightClickedCallbackProperty, value);
                if (oldHandlesRightClicked != HandlesRightClicked)
                    HandlesRightClickedChanged?.Invoke(this, !oldHandlesRightClicked);
            }
        }
        /// <summary>
        /// Parameter passed to Action invoked after a tap motion
        /// </summary>
        public object RightClickedCallbackParameter
        {
            get => GetValue(RightClickedCallbackParameterProperty);
            set => SetValue(RightClickedCallbackParameterProperty, value);
        }
        /// <summary>
        /// does this Listener invoke anything after a tap motion?
        /// </summary>
        public bool HandlesRightClicked => _rightClicked != null || RightClickedCommand != null || RightClickedCallback != null;

        /// <summary>
        /// Event triggered when HandlesRightClicked has changed
        /// </summary>
        public event EventHandler<bool> HandlesRightClickedChanged;


        internal bool OnRightClicked(RightClickEventArgs args)
        {
            var result = false;
            if (HandlesRightClicked)
            {
                RaiseEvent<RightClickEventArgs>(_rightClicked, args);
                ExecuteCommand(RightClickedCommand, RightClickedCommandParameter, args);
                ExecuteCallback(RightClickedCallback, RightClickedCallbackParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion


        #region Command / Event executors
        void RaiseEvent<T>(EventHandler<T> handler, T args) where T : BaseGestureEventArgs => handler?.Invoke(this, args);

        static void ExecuteCommand(ICommand command, object parameter, BaseGestureEventArgs args)
        {
            parameter = (parameter ?? args);
            if (command != null && command.CanExecute(parameter))
                command.Execute(parameter);
        }

        void ExecuteCallback(Action<Listener, object> callback, object parameter, BaseGestureEventArgs args)
        {
            parameter = (parameter ?? args);
            callback?.Invoke(this, parameter);
        }
        #endregion

        #endregion

        #region Constructor / Disposer

        static IGestureService _gestureService;
        static IGestureService GestureService
        {
            get
            {
                _gestureService = _gestureService ?? DependencyService.Get<IGestureService>();
                if (_gestureService == null)
                    throw new MissingMemberException("FormsGestures: Failed to load IGestureService instance");
                return _gestureService;
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Listener For(VisualElement element)
        {
            for (int i = 0; i < Listeners.Count; i++)
                if (Listeners[i].Element == element)
                    return Listeners[i];
            return new Listener(element);
        }

        private Listener(VisualElement element)
        {
            P42.Utils.Debug.AddToCensus(this);

            _element = element;
            var inserted = false;
            for (int i = Listeners.Count - 1; i >= 0; i--)
            {
                if (element.IsDescendentOf(Listeners[i].Element))
                {
                    Listeners.Insert(i + 1, this);
                    inserted = true;
                    break;
                }
            }
            if (!inserted)
                Listeners.Insert(0, this);
            GestureService.For(this);
        }

        bool _disposed;
        /// <summary>
        /// Disposer
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Event handler for Disposing event
        /// </summary>
        public event EventHandler Disposing;
        /// <summary>
        /// Dispoer
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;

                HandlesDownChanged = null;
                _down = null;
                HandlesUpChanged = null;
                _up = null;
                HandlesTappingChanged = null;
                _tapping = null;
                HandlesTappedChanged = null;
                _tapped = null;
                HandlesDoubleTappedChanged = null;
                _doubleTapped = null;
                HandlesLongPressingChanged = null;
                _longPressing = null;
                HandlesLongPressedChanged = null;
                _longPressed = null;
                HandlesPinchingChanged = null;
                _pinching = null;
                HandlesPinchedChanged = null;
                _pinched = null;
                HandlesPanningChanged = null;
                _panning = null;
                HandlesPannedChanged = null;
                _panned = null;
                HandlesSwipedChanged = null;
                _swiped = null;
                HandlesRotatingChanged = null;
                _rotating = null;
                HandlesRotatedChanged = null;
                _rotated = null;
                HandlesRightClickedChanged = null;
                _rightClicked = null;

                Listeners.Remove(this);
                Disposing?.Invoke(this, EventArgs.Empty);
                Disposing = null;

                P42.Utils.Debug.RemoveFromCensus(this);
            }
        }

        #endregion



#pragma warning restore IDE1006 // Naming Styles
    }



}
