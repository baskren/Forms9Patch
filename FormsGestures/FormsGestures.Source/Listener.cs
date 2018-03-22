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

        const bool _debugEvents = false;

        // ancestors will procede descendents in this list so that interuptions can found quickly and fired in an orderly fashion
        internal static readonly List<Listener> Listeners = new List<Listener>();

        VisualElement _element;

        /// <summary>
        /// VisualElement that is the focus of this Listener
        /// </summary>
        public VisualElement Element
        {
            get { return _element; }
        }


        #region Events / Commands

        #region Event / Command Property List
        internal static readonly string[] AllEventsAndCommands = {
            "Down",
            "DownCommand",
            "DownCommandParameter",
            "DownCallback",
            "DownCallbackParameter",

            "Up",
            "UpCommand",
            "UpCommandParameter",
            "UpCallback",
            "UpCallbackParameter",

            "Tapping",
            "TappingCommand",
            "TappingCommandParameter",
            "TappingCallback",
            "TappingCallbackParameter",

            "Tapped",
            "TappedCommand",
            "TappedCommandParameter",
            "TappedCallback",
            "TappedCallbackParameter",

            "DoubleTapped",
            "DoubleTappedCommand",
            "DoubleTappedCommandParameter",
            "DoubleTappedCallback",
            "DoubleTappedCallbackParameter",

            "LongPressing",
            "LongPressingCommand",
            "LongPressingCommandParameter",
            "LongPressingCallback",
            "LongPressingCallbackParameter",

            "LongPressed",
            "LongPressedCommand",
            "LongPressedCommandParameter",
            "LongPressedCallback",
            "LongPressedCallbackParameter",

            "Pinching",
            "PinchingCommand",
            "PinchingCommandParameter",
            "PinchingCallback",
            "PinchingCallbackParameter",

            "Pinched",
            "PinchedCommand",
            "PinchedCommandParameter",
            "PinchedCallback",
            "PinchedCallbackParameter",

            "Panning",
            "PanningCommand",
            "PanningCommandParameter",
            "PanningCallback",
            "PanningCallbackParameter",

            "Panned",
            "PannedCommand",
            "PannedCommandParameter",
            "PannedCallback",
            "PannedCallbackParameter",

            "Swiped",
            "SwipedCommand",
            "SwipedCommandParameter",
            "SwipedCallback",
            "SwipedCallbackParameter",

            "Rotating",
            "RotatingCommand",
            "RotatingCommandParameter",
            "RotatingCallback",
            "RotatingCallbackParameter",

            "Rotated",
            "RotatedCommand",
            "RotatedCommandParameter",
            "RotatedCallback",
            "RotatedCallbackParameter",

            "RightClicked",
            "RightClickedCommand",
            "RightClickedCommandParameter",
            "RightClickedCallback",
            "RightClickedCallbackParameter",

        };
        #endregion

        #region Down
        /// <summary>
        /// Down event handler
        /// </summary>
        public event EventHandler<DownUpEventArgs> Down;
        /// <summary>
        /// backing store for command invoked upon down event
        /// </summary>
        public static readonly BindableProperty DownCommandProperty = BindableProperty.Create("DownCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for down command parameter
        /// </summary>
        public static readonly BindableProperty DownCommandParameterProperty = BindableProperty.Create("DownCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// command invoked upon down event
        /// </summary>
        public ICommand DownCommand
        {
            get { return (ICommand)GetValue(DownCommandProperty); }
            set { SetValue(DownCommandProperty, value); }
        }
        /// <summary>
        /// parameter passed with command invoked upon down event
        /// </summary>
        public object DownCommandParameter
        {
            get { return GetValue(DownCommandParameterProperty); }
            set { SetValue(DownCommandParameterProperty, value); }
        }
        /// <summary>
        /// backing store for DownCallback property
        /// </summary>
        public static readonly BindableProperty DownCallbackProperty = BindableProperty.Create("DownCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for DownCallbackParameter property
        /// </summary>
        public static readonly BindableProperty DownCallbackParameterProperty = BindableProperty.Create("DownCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked invoked upon down event
        /// </summary>
        public Action<Listener, object> DownCallback
        {
            get { return (Action<Listener, object>)GetValue(DownCallbackProperty); }
            set { SetValue(DownCallbackProperty, value); }
        }
        /// <summary>
        /// parameter passed with to Action invoked invoked upon down event
        /// </summary>
        public object DownCallbackParameter
        {
            get { return GetValue(DownCallbackParameterProperty); }
            set { SetValue(DownCallbackParameterProperty, value); }
        }
        /// <summary>
        /// returns if Listener is configured to handle down touch 
        /// </summary>
        public bool HandlesDown
        {
            get { return Down != null || DownCommand != null || DownCallback != null; }
        }
        internal bool OnDown(DownUpEventArgs args)
        {
            bool result = false;
            if (HandlesDown)
            {
                RaiseEvent<DownUpEventArgs>(Down, args);
                ExecuteCommand(DownCommand, DownCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Up
        /// <summary>
        /// Up event motion handler
        /// </summary>
        public event EventHandler<DownUpEventArgs> Up;
        /// <summary>
        /// backing store for UpCommand
        /// </summary>
        public static readonly BindableProperty UpCommandProperty = BindableProperty.Create("UpCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for UpCommandParameter
        /// </summary>
        public static readonly BindableProperty UpCommandParameterProperty = BindableProperty.Create("UpCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// command invoked upon up touch
        /// </summary>
        public ICommand UpCommand
        {
            get { return (ICommand)GetValue(UpCommandProperty); }
            set { SetValue(UpCommandProperty, value); }
        }
        /// <summary>
        /// parameter passed to command invoked upon up touch
        /// </summary>
        public object UpCommandParameter
        {
            get { return GetValue(UpCommandParameterProperty); }
            set { SetValue(UpCommandParameterProperty, value); }
        }
        /// <summary>
        /// Backing store for UpCallback
        /// </summary>
        public static readonly BindableProperty UpCallbackProperty = BindableProperty.Create("UpCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// Backing store for UpCallbackParameter
        /// </summary>
        public static readonly BindableProperty UpCallbackParameterProperty = BindableProperty.Create("UpCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked upon up touch
        /// </summary>
        public Action<Listener, object> UpCallback
        {
            get { return (Action<Listener, object>)GetValue(UpCallbackProperty); }
            set { SetValue(UpCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter passed to Action invoked upon up touch
        /// </summary>
        public object UpCallbackParameter
        {
            get { return GetValue(UpCallbackParameterProperty); }
            set { SetValue(UpCallbackParameterProperty, value); }
        }
        /// <summary>
        /// Does this Listener invoke anything upon an up touch?
        /// </summary>
        public bool HandlesUp
        {
            get { return Up != null || UpCommand != null || UpCallback != null; }
        }
        internal bool OnUp(DownUpEventArgs args)
        {
            bool result = false;
            if (HandlesUp)
            {
                RaiseEvent<DownUpEventArgs>(Up, args);
                ExecuteCommand(UpCommand, UpCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Tapping
        /// <summary>
        /// Tapping event handler
        /// </summary>
        public event EventHandler<TapEventArgs> Tapping;
        /// <summary>
        /// backing store for the TappingCommand property
        /// </summary>
        public static readonly BindableProperty TappingCommandProperty = BindableProperty.Create("TappingCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the TappingCommandParameter property
        /// </summary>
        public static readonly BindableProperty TappingCommandParameterProperty = BindableProperty.Create("TappingCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked during tap event
        /// </summary>
        public ICommand TappingCommand
        {
            get { return (ICommand)GetValue(TappingCommandProperty); }
            set { SetValue(TappingCommandProperty, value); }
        }
        /// <summary>
        /// Parameter padded to command invoked during tap event
        /// </summary>
        public object TappingCommandParameter
        {
            get { return GetValue(TappingCommandParameterProperty); }
            set { SetValue(TappingCommandParameterProperty, value); }
        }
        /// <summary>
        /// backing store for the TappingCallback property
        /// </summary>
        public static readonly BindableProperty TappingCallbackProperty = BindableProperty.Create("TappingCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the TappingCallbackParameter property
        /// </summary>
        public static readonly BindableProperty TappingCallbackParameterProperty = BindableProperty.Create("TappingCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked upon tap event
        /// </summary>
        public Action<Listener, object> TappingCallback
        {
            get { return (Action<Listener, object>)GetValue(TappingCallbackProperty); }
            set { SetValue(TappingCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter passed to Action invoked during tap event
        /// </summary>
        public object TappingCallbackParameter
        {
            get { return GetValue(TappingCallbackParameterProperty); }
            set { SetValue(TappingCallbackParameterProperty, value); }
        }
        /// <summary>
        /// does this Listner invoke anything during a tap motion?
        /// </summary>
        public bool HandlesTapping
        {
            get { return Tapping != null || TappingCommand != null || TappingCallback != null; }
        }
        internal bool OnTapping(TapEventArgs args)
        {
            bool result = false;
            if (HandlesTapping)
            {
                RaiseEvent<TapEventArgs>(Tapping, args);
                ExecuteCommand(TappingCommand, TappingCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Tapped
        /// <summary>
        /// Tapped event handler
        /// </summary>
        public event EventHandler<TapEventArgs> Tapped;
        /// <summary>
        /// backing store for the TappedCommand property
        /// </summary>
        public static readonly BindableProperty TappedCommandProperty = BindableProperty.Create("TappedCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the TappedCommandParameter property
        /// </summary>
        public static readonly BindableProperty TappedCommandParameterProperty = BindableProperty.Create("TappedCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after a tap motion
        /// </summary>
        public ICommand TappedCommand
        {
            get { return (ICommand)GetValue(TappedCommandProperty); }
            set { SetValue(TappedCommandProperty, value); }
        }
        /// <summary>
        /// Parameter passed with Command invoked after a tap motion
        /// </summary>
        public object TappedCommandParameter
        {
            get { return GetValue(TappedCommandParameterProperty); }
            set { SetValue(TappedCommandParameterProperty, value); }
        }
        /// <summary>
        /// backing store for a TappedCallback property
        /// </summary>
        public static readonly BindableProperty TappedCallbackProperty = BindableProperty.Create("TappedCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for a TappedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty TappedCallbackParameterProperty = BindableProperty.Create("TappedCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after a tap motion
        /// </summary>
        public Action<Listener, object> TappedCallback
        {
            get { return (Action<Listener, object>)GetValue(TappedCallbackProperty); }
            set { SetValue(TappedCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter passed to Action invoked after a tap motion
        /// </summary>
        public object TappedCallbackParameter
        {
            get { return GetValue(TappedCallbackParameterProperty); }
            set { SetValue(TappedCallbackParameterProperty, value); }
        }
        /// <summary>
        /// does this Listener invoke anything after a tap motion?
        /// </summary>
        public bool HandlesTapped
        {
            get { return Tapped != null || TappedCommand != null || TappedCallback != null; }
        }
        internal bool OnTapped(TapEventArgs args)
        {
            bool result = false;
            if (HandlesTapped)
            {
                RaiseEvent<TapEventArgs>(Tapped, args);
                ExecuteCommand(TappedCommand, TappedCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region DoubleTapped
        /// <summary>
        /// DoubleTapped event handler
        /// </summary>
        public event EventHandler<TapEventArgs> DoubleTapped;
        /// <summary>
        /// backing store for the DoubleTappedCommand property
        /// </summary>
        public static readonly BindableProperty DoubleTappedCommandProperty = BindableProperty.Create("DoubleTappedCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the DoubleTappedCommmandParameter property
        /// </summary>
        public static readonly BindableProperty DoubleTappedCommandParameterProperty = BindableProperty.Create("DoubleTappedCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after a double tap motion
        /// </summary>
        public ICommand DoubleTappedCommand
        {
            get { return (ICommand)GetValue(DoubleTappedCommandProperty); }
            set { SetValue(DoubleTappedCommandProperty, value); }
        }
        /// <summary>
        /// Parameter sent with Command invoked after a double tap motion
        /// </summary>
        public object DoubleTappedCommandParameter
        {
            get { return GetValue(DoubleTappedCommandParameterProperty); }
            set { SetValue(DoubleTappedCommandParameterProperty, value); }
        }
        /// <summary>
        /// backing store for DoubleTappedCallback property
        /// </summary>
        public static readonly BindableProperty DoubleTappedCallbackProperty = BindableProperty.Create("DoubleTappedCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for DoubleTappedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty DoubleTappedCallbackParameterProperty = BindableProperty.Create("DoubleTappedCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after a double tap motion
        /// </summary>
        public Action<Listener, object> DoubleTappedCallback
        {
            get { return (Action<Listener, object>)GetValue(DoubleTappedCallbackProperty); }
            set { SetValue(DoubleTappedCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter sent to Action invoked after a double tap motion
        /// </summary>
        public object DoubleTappedCallbackParameter
        {
            get { return GetValue(DoubleTappedCallbackParameterProperty); }
            set { SetValue(DoubleTappedCallbackParameterProperty, value); }
        }
        /// <summary>
        /// does this Listener invoke anything upon double tap motion?
        /// </summary>
        public bool HandlesDoubleTapped
        {
            get { return DoubleTapped != null || DoubleTappedCommand != null || DoubleTappedCallback != null; }
        }
        internal bool OnDoubleTapped(TapEventArgs args)
        {
            bool result = false;
            if (HandlesDoubleTapped)
            {
                RaiseEvent<TapEventArgs>(DoubleTapped, args);
                ExecuteCommand(DoubleTappedCommand, DoubleTappedCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region LongPressing
        /// <summary>
        /// LongPressing event handler
        /// </summary>
        public event EventHandler<LongPressEventArgs> LongPressing;
        /// <summary>
        /// backing store for LongPressingCommand property
        /// </summary>
        public static readonly BindableProperty LongPressingCommandProperty = BindableProperty.Create("LongPressingCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for LongPressingCommandParameter property
        /// </summary>
        public static readonly BindableProperty LongPressingCommandParameterProperty = BindableProperty.Create("LongPressingCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked during long pressing motion
        /// </summary>
        public ICommand LongPressingCommand
        {
            get { return (ICommand)GetValue(LongPressingCommandProperty); }
            set { SetValue(LongPressingCommandProperty, value); }
        }
        /// <summary>
        /// Parameter sent to Command invoked during long pressing motion
        /// </summary>
        public object LongPressingCommandParameter
        {
            get { return GetValue(LongPressingCommandParameterProperty); }
            set { SetValue(LongPressingCommandParameterProperty, value); }
        }
        /// <summary>
        /// backing store for LongPressingCallback property
        /// </summary>
        public static readonly BindableProperty LongPressingCallbackProperty = BindableProperty.Create("LongPressingCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for LongPressingCallbackParameter property
        /// </summary>
        public static readonly BindableProperty LongPressingCallbackParameterProperty = BindableProperty.Create("LongPressingCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked during long pressing motion
        /// </summary>
        public Action<Listener, object> LongPressingCallback
        {
            get { return (Action<Listener, object>)GetValue(LongPressingCallbackProperty); }
            set { SetValue(LongPressingCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter sent to Action invoked during long pressing motion
        /// </summary>
        public object LongPressingCallbackParameter
        {
            get { return GetValue(LongPressingCallbackParameterProperty); }
            set { SetValue(LongPressingCallbackParameterProperty, value); }
        }
        /// <summary>
        /// Does this Listner invoke anything during long press motion?
        /// </summary>
        public bool HandlesLongPressing
        {
            get { return LongPressing != null || LongPressingCommand != null || LongPressingCallback != null; }
        }
        internal bool OnLongPressing(LongPressEventArgs args)
        {
            bool result = false;
            if (HandlesLongPressing)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<LongPressEventArgs>(LongPressing, args);
                ExecuteCommand(LongPressingCommand, LongPressingCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region LongPressed
        /// <summary>
        /// LongPressed event handler
        /// </summary>
        public event EventHandler<LongPressEventArgs> LongPressed;
        /// <summary>
        /// backing store for LongPressedCommand property
        /// </summary>
        public static readonly BindableProperty LongPressedCommandProperty = BindableProperty.Create("LongPressedCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for LongPressedCommandParameter property
        /// </summary>
        public static readonly BindableProperty LongPressedCommandParameterProperty = BindableProperty.Create("LongPressedCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after long press motion
        /// </summary>
        public ICommand LongPressedCommand
        {
            get { return (ICommand)GetValue(LongPressedCommandProperty); }
            set { SetValue(LongPressedCommandProperty, value); }
        }
        /// <summary>
        /// Parameter sent with Command invoked after long press motion
        /// </summary>
        public object LongPressedCommandParameter
        {
            get { return GetValue(LongPressedCommandParameterProperty); }
            set { SetValue(LongPressedCommandParameterProperty, value); }
        }
        /// <summary>
        /// backing store for LongPressedCallback property
        /// </summary>
        public static readonly BindableProperty LongPressedCallbackProperty = BindableProperty.Create("LongPressedCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for LongPressedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty LongPressedCallbackParameterProperty = BindableProperty.Create("LongPressedCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after long press motion
        /// </summary>
        public Action<Listener, object> LongPressedCallback
        {
            get { return (Action<Listener, object>)GetValue(LongPressedCallbackProperty); }
            set { SetValue(LongPressedCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter sent with Action invoked after long press motion
        /// </summary>
        public object LongPressedCallbackParameter
        {
            get { return GetValue(LongPressedCallbackParameterProperty); }
            set { SetValue(LongPressedCallbackParameterProperty, value); }
        }
        /// <summary>
        /// Does this Listener invoke anything after a long press
        /// </summary>
        public bool HandlesLongPressed
        {
            get { return LongPressed != null || LongPressedCommand != null || LongPressedCallback != null; }
        }
        internal bool OnLongPressed(LongPressEventArgs args)
        {
            bool result = false;
            if (HandlesLongPressed)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<LongPressEventArgs>(LongPressed, args);
                ExecuteCommand(LongPressedCommand, LongPressedCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Pinching
        /// <summary>
        /// Pinching event listener
        /// </summary>
        public event EventHandler<PinchEventArgs> Pinching;
        /// <summary>
        /// backing store for the PinchingCommand property
        /// </summary>
        public static readonly BindableProperty PinchingCommandProperty = BindableProperty.Create("PinchingCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the PinchingCommandParameter property
        /// </summary>
        public static readonly BindableProperty PinchingCommandParameterProperty = BindableProperty.Create("PinchingCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked during pinch motion
        /// </summary>
        public ICommand PinchingCommand
        {
            get { return (ICommand)GetValue(PinchingCommandProperty); }
            set { SetValue(PinchingCommandProperty, value); }
        }
        /// <summary>
        /// Parameter sent with Command invoked during pinch motion
        /// </summary>
        public object PinchingCommandParameter
        {
            get { return GetValue(PinchingCommandParameterProperty); }
            set { SetValue(PinchingCommandParameterProperty, value); }
        }
        /// <summary>
        /// backing store for the PinchingCallback property
        /// </summary>
        public static readonly BindableProperty PinchingCallbackProperty = BindableProperty.Create("PinchingCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the PinchingCallbackParameter property
        /// </summary>
        public static readonly BindableProperty PinchingCallbackParameterProperty = BindableProperty.Create("PinchingCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked during pinch motion
        /// </summary>
        public Action<Listener, object> PinchingCallback
        {
            get { return (Action<Listener, object>)GetValue(PinchingCallbackProperty); }
            set { SetValue(PinchingCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter sent to Action invoked during pinch motion
        /// </summary>
        public object PinchingCallbackParameter
        {
            get { return GetValue(PinchingCallbackParameterProperty); }
            set { SetValue(PinchingCallbackParameterProperty, value); }
        }
        /// <summary>
        /// does this Listener invoke anything during pinch motion?
        /// </summary>
        public bool HandlesPinching
        {
            get { return Pinching != null || PinchingCommand != null || PinchingCallback != null; }
        }
        internal bool OnPinching(PinchEventArgs args)
        {
            bool result = false;
            if (HandlesPinching)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<PinchEventArgs>(Pinching, args);
                ExecuteCommand(PinchingCommand, PinchingCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Pinched
        /// <summary>
        /// Pinched event handler
        /// </summary>
        public event EventHandler<PinchEventArgs> Pinched;
        /// <summary>
        /// backing store for the PinchedCommand property
        /// </summary>
        public static readonly BindableProperty PinchedCommandProperty = BindableProperty.Create("PinchedCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the PinchedCommandParameter property
        /// </summary>
        public static readonly BindableProperty PinchedCommandParameterProperty = BindableProperty.Create("PinchedCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after pinch motion
        /// </summary>
        public ICommand PinchedCommand
        {
            get { return (ICommand)GetValue(PinchedCommandProperty); }
            set { SetValue(PinchedCommandProperty, value); }
        }
        /// <summary>
        /// Parameter sent with Command invoked after pinch motion
        /// </summary>
        public object PinchedCommandParameter
        {
            get { return GetValue(PinchedCommandParameterProperty); }
            set { SetValue(PinchedCommandParameterProperty, value); }
        }
        /// <summary>
        /// backing store for the PinchedCallback property
        /// </summary>
        public static readonly BindableProperty PinchedCallbackProperty = BindableProperty.Create("PinchedCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the PinchedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty PinchedCallbackParameterProperty = BindableProperty.Create("PinchedCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after pinch motion
        /// </summary>
        public Action<Listener, object> PinchedCallback
        {
            get { return (Action<Listener, object>)GetValue(PinchedCallbackProperty); }
            set { SetValue(PinchedCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter passed to Action invoked after pinch motion
        /// </summary>
        public object PinchedCallbackParameter
        {
            get { return GetValue(PinchedCallbackParameterProperty); }
            set { SetValue(PinchedCallbackParameterProperty, value); }
        }
        /// <summary>
        /// Does this Listener invoke anything after pinch motion
        /// </summary>
        public bool HandlesPinched
        {
            get { return Pinched != null || PinchedCommand != null || PinchedCallback != null; }
        }
        internal bool OnPinched(PinchEventArgs args)
        {
            bool result = false;
            if (HandlesPinched)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<PinchEventArgs>(Pinched, args);
                ExecuteCommand(PinchedCommand, PinchedCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Panning
        /// <summary>
        /// Panning event handler
        /// </summary>
        public event EventHandler<PanEventArgs> Panning;
        /// <summary>
        /// backing store for the PanningCommand parameter
        /// </summary>
        public static readonly BindableProperty PanningCommandProperty = BindableProperty.Create("PanningCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the PanningCommandParameter parameter
        /// </summary>
        public static readonly BindableProperty PanningCommandParameterProperty = BindableProperty.Create("PanningCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked duing pan motion
        /// </summary>
        public ICommand PanningCommand
        {
            get { return (ICommand)GetValue(PanningCommandProperty); }
            set { SetValue(PanningCommandProperty, value); }
        }
        /// <summary>
        /// Parameter sent with Command invoked duing pan motion
        /// </summary>
        public object PanningCommandParameter
        {
            get { return GetValue(PanningCommandParameterProperty); }
            set { SetValue(PanningCommandParameterProperty, value); }
        }
        /// <summary>
        /// backing store for the PanningCallback parameter
        /// </summary>
        public static readonly BindableProperty PanningCallbackProperty = BindableProperty.Create("PanningCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the PanningCallbackParameter parameter
        /// </summary>
        public static readonly BindableProperty PanningCallbackParameterProperty = BindableProperty.Create("PanningCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked duing pan motion
        /// </summary>
        public Action<Listener, object> PanningCallback
        {
            get { return (Action<Listener, object>)GetValue(PanningCallbackProperty); }
            set { SetValue(PanningCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter sent to Action invoked duing pan motion
        /// </summary>
        public object PanningCallbackParameter
        {
            get { return GetValue(PanningCallbackParameterProperty); }
            set { SetValue(PanningCallbackParameterProperty, value); }
        }
        /// <summary>
        /// Does Listener invoke anything during pan motion?
        /// </summary>
        public bool HandlesPanning
        {
            get { return Panning != null || PanningCommand != null || PanningCallback != null; }
        }
        internal bool OnPanning(PanEventArgs args)
        {
            bool result = false;
            if (HandlesPanning)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<PanEventArgs>(Panning, args);
                ExecuteCommand(PanningCommand, PanningCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Panned
        /// <summary>
        /// Pannded event handler
        /// </summary>
        public event EventHandler<PanEventArgs> Panned;
        /// <summary>
        /// backing store for the PannedCommand property
        /// </summary>
        public static readonly BindableProperty PannedCommandProperty = BindableProperty.Create("PannedCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the PannedCommandParameter property
        /// </summary>
        public static readonly BindableProperty PannedCommandParameterProperty = BindableProperty.Create("PannedCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after pan motion
        /// </summary>
        public ICommand PannedCommand
        {
            get { return (ICommand)GetValue(PannedCommandProperty); }
            set { SetValue(PannedCommandProperty, value); }
        }
        /// <summary>
        /// Parameter sent with Command invoked after pan motion
        /// </summary>
        public object PannedCommandParameter
        {
            get { return GetValue(PannedCommandParameterProperty); }
            set
            {
                SetValue(PannedCommandParameterProperty, value);
            }
        }
        /// <summary>
        /// backing store for the PanndedCallback property
        /// </summary>
        public static readonly BindableProperty PannedCallbackProperty = BindableProperty.Create("PannedCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the PanndedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty PannedCallbackParameterProperty = BindableProperty.Create("PannedCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after pan motion
        /// </summary>
        public Action<Listener, object> PannedCallback
        {
            get { return (Action<Listener, object>)GetValue(PannedCallbackProperty); }
            set { SetValue(PannedCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter sent with Action invoked after pan motion
        /// </summary>
        public object PannedCallbackParameter
        {
            get { return GetValue(PannedCallbackParameterProperty); }
            set
            {
                SetValue(PannedCallbackParameterProperty, value);
            }
        }
        /// <summary>
        /// Does this Listener invoke anything after pan motion?
        /// </summary>
        public bool HandlesPanned
        {
            get { return Panned != null || PannedCommand != null || PannedCallback != null; }
        }
        internal bool OnPanned(PanEventArgs args)
        {
            bool result = false;
            if (HandlesPanned)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<PanEventArgs>(Panned, args);
                ExecuteCommand(PannedCommand, PannedCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Swiped
        /// <summary>
        /// Swiped event handler
        /// </summary>
        public event EventHandler<SwipeEventArgs> Swiped;
        /// <summary>
        /// backing store for the SwipedCommand property
        /// </summary>
        public static readonly BindableProperty SwipedCommandProperty = BindableProperty.Create("SwipedCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the SwipedCommandParameter property
        /// </summary>
        public static readonly BindableProperty SwipedCommandParameterProperty = BindableProperty.Create("SwipedCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after swipe motion
        /// </summary>
        public ICommand SwipedCommand
        {
            get { return (ICommand)GetValue(SwipedCommandProperty); }
            set { SetValue(SwipedCommandProperty, value); }
        }
        /// <summary>
        /// Parameter sent with Command invoked after swipe motion
        /// </summary>
        public object SwipedCommandParameter
        {
            get { return GetValue(SwipedCommandParameterProperty); }
            set
            {
                SetValue(SwipedCommandParameterProperty, value);
            }
        }
        /// <summary>
        /// backing store for the SwipedCallback property
        /// </summary>
        public static readonly BindableProperty SwipedCallbackProperty = BindableProperty.Create("SwipedCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the SwipedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty SwipedCallbackParameterProperty = BindableProperty.Create("SwipedCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after swipe motion
        /// </summary>
        public Action<Listener, object> SwipedCallback
        {
            get { return (Action<Listener, object>)GetValue(SwipedCallbackProperty); }
            set { SetValue(SwipedCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter sent with Action invoked after swipe motion
        /// </summary>
        public object SwipedCallbackParameter
        {
            get { return GetValue(SwipedCallbackParameterProperty); }
            set
            {
                SetValue(SwipedCallbackParameterProperty, value);
            }
        }
        /// <summary>
        /// Does this Listener invoke anything after swipe motion
        /// </summary>
        public bool HandlesSwiped
        {
            get { return Swiped != null || SwipedCommand != null || SwipedCallback != null; }
        }
        internal bool OnSwiped(SwipeEventArgs args)
        {
            bool result = false;
            if (HandlesSwiped)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<SwipeEventArgs>(Swiped, args);
                ExecuteCommand(SwipedCommand, SwipedCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Rotating
        /// <summary>
        /// Rotating event handler
        /// </summary>
        public event EventHandler<RotateEventArgs> Rotating;
        /// <summary>
        /// backing store for the RotatingCommand property
        /// </summary>
        public static readonly BindableProperty RotatingCommandProperty = BindableProperty.Create("RotatingCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the RotatingCommandParameter property
        /// </summary>
        public static readonly BindableProperty RotatingCommandParameterProperty = BindableProperty.Create("RotatingCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked during rotation motion
        /// </summary>
        public ICommand RotatingCommand
        {
            get { return (ICommand)GetValue(RotatingCommandProperty); }
            set
            {
                SetValue(RotatingCommandProperty, value);
            }
        }
        /// <summary>
        /// Parameter sent with Command invoked during rotation motion
        /// </summary>
        public object RotatingCommandParameter
        {
            get { return GetValue(RotatingCommandParameterProperty); }
            set { SetValue(RotatingCommandParameterProperty, value); }
        }
        /// <summary>
        /// backing store for the RotatingCallback property
        /// </summary>
        public static readonly BindableProperty RotatingCallbackProperty = BindableProperty.Create("RotatingCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the RotatingCallbackParameter property
        /// </summary>
        public static readonly BindableProperty RotatingCallbackParameterProperty = BindableProperty.Create("RotatingCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked during rotation motion
        /// </summary>
        public Action<Listener, object> RotatingCallback
        {
            get { return (Action<Listener, object>)GetValue(RotatingCallbackProperty); }
            set
            {
                SetValue(RotatingCallbackProperty, value);
            }
        }
        /// <summary>
        /// Parameter sent with Action invoked during rotation motion
        /// </summary>
        public object RotatingCallbackParameter
        {
            get { return GetValue(RotatingCallbackParameterProperty); }
            set { SetValue(RotatingCallbackParameterProperty, value); }
        }
        /// <summary>
        /// Does Listener invoke anything during rotation motion?
        /// </summary>
        public bool HandlesRotating
        {
            get { return Rotating != null || RotatingCommand != null || RotatingCallback != null; }
        }
        internal bool OnRotating(RotateEventArgs args)
        {
            bool result = false;
            if (HandlesRotating)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<RotateEventArgs>(Rotating, args);
                ExecuteCommand(RotatingCommand, RotatingCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region Rotated
        /// <summary>
        /// Rotated event handler
        /// </summary>
        public event EventHandler<RotateEventArgs> Rotated;
        /// <summary>
        /// backing store for the RotatedCommand property
        /// </summary>
        public static readonly BindableProperty RotatedCommandProperty = BindableProperty.Create("RotatedCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the RotatedCommandParameter property
        /// </summary>
        public static readonly BindableProperty RotatedCommandParameterProperty = BindableProperty.Create("RotatedCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after rotation motion
        /// </summary>
        public ICommand RotatedCommand
        {
            get { return (ICommand)GetValue(RotatedCommandProperty); }
            set { SetValue(RotatedCommandProperty, value); }
        }
        /// <summary>
        /// Parameter sent with Command invoked after rotation motion
        /// </summary>
        public object RotatedCommandParameter
        {
            get { return GetValue(RotatedCommandParameterProperty); }
            set { SetValue(RotatedCommandParameterProperty, value); }
        }
        /// <summary>
        /// backing store for the RotatedCallback property
        /// </summary>
        public static readonly BindableProperty RotatedCallbackProperty = BindableProperty.Create("RotatedCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for the RotatedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty RotatedCallbackParameterProperty = BindableProperty.Create("RotatedCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after rotation motion
        /// </summary>
        public Action<Listener, object> RotatedCallback
        {
            get { return (Action<Listener, object>)GetValue(RotatedCallbackProperty); }
            set { SetValue(RotatedCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter sent with Action invoked after rotation motion
        /// </summary>
        public object RotatedCallbackParameter
        {
            get { return GetValue(RotatedCallbackParameterProperty); }
            set { SetValue(RotatedCallbackParameterProperty, value); }
        }
        /// <summary>
        /// Does Listener invoke anything after rotation motion?
        /// </summary>
        public bool HandlesRotated
        {
            get { return Rotated != null || RotatedCommand != null || RotatedCallback != null; }
        }
        internal bool OnRotated(RotateEventArgs args)
        {
            bool result = false;
            if (HandlesRotated)
            {
                //if (_debugEvents) System.Diagnostics.Debug.WriteLine ("[{0}.{1}] [{2}] [{3}]",this.GetType().Name, FormsGestures.Debug.CurrentMethod() ,_id,_element);
                RaiseEvent<RotateEventArgs>(Rotated, args);
                ExecuteCommand(RotatedCommand, RotatedCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion

        #region RightClicked
        /// <summary>
        /// Tapped event handler
        /// </summary>
        public event EventHandler<RightClickEventArgs> RightClicked;
        /// <summary>
        /// backing store for the TappedCommand property
        /// </summary>
        public static readonly BindableProperty RightClickedCommandProperty = BindableProperty.Create("RightClickedCommand", typeof(ICommand), typeof(Listener), null);
        /// <summary>
        /// backing store for the TappedCommandParameter property
        /// </summary>
        public static readonly BindableProperty RightClickedCommandParameterProperty = BindableProperty.Create("RightClickedCommandParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Command invoked after a tap motion
        /// </summary>
        public ICommand RightClickedCommand
        {
            get { return (ICommand)GetValue(RightClickedCommandProperty); }
            set { SetValue(RightClickedCommandProperty, value); }
        }
        /// <summary>
        /// Parameter passed with Command invoked after a tap motion
        /// </summary>
        public object RightClickedCommandParameter
        {
            get { return GetValue(RightClickedCommandParameterProperty); }
            set { SetValue(RightClickedCommandParameterProperty, value); }
        }
        /// <summary>
        /// backing store for a TappedCallback property
        /// </summary>
        public static readonly BindableProperty RightClickedCallbackProperty = BindableProperty.Create("RightClickedCallback", typeof(Action<Listener, object>), typeof(Listener), null);
        /// <summary>
        /// backing store for a TappedCallbackParameter property
        /// </summary>
        public static readonly BindableProperty RightClickedCallbackParameterProperty = BindableProperty.Create("RightClickedCallbackParameter", typeof(object), typeof(Listener), null);
        /// <summary>
        /// Action invoked after a tap motion
        /// </summary>
        public Action<Listener, object> RightClickedCallback
        {
            get { return (Action<Listener, object>)GetValue(RightClickedCallbackProperty); }
            set { SetValue(RightClickedCallbackProperty, value); }
        }
        /// <summary>
        /// Parameter passed to Action invoked after a tap motion
        /// </summary>
        public object RightClickedCallbackParameter
        {
            get { return GetValue(RightClickedCallbackParameterProperty); }
            set { SetValue(RightClickedCallbackParameterProperty, value); }
        }
        /// <summary>
        /// does this Listener invoke anything after a tap motion?
        /// </summary>
        public bool HandlesRightClicked
        {
            get { return RightClicked != null || RightClickedCommand != null || RightClickedCallback != null; }
        }
        internal bool OnRightClicked(RightClickEventArgs args)
        {
            bool result = false;
            if (HandlesRightClicked)
            {
                RaiseEvent<RightClickEventArgs>(RightClicked, args);
                ExecuteCommand(RightClickedCommand, RightClickedCommandParameter, args);
                result = args.Handled;
            }
            return result;
        }
        #endregion


        #region Command / Event executors
        void RaiseEvent<T>(EventHandler<T> handler, T args) where T : BaseGestureEventArgs => handler?.Invoke(this, args);

        void ExecuteCommand(ICommand command, object parameter, BaseGestureEventArgs args)
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
            //foreach (var listener in Listeners)
            for (int i = 0; i < Listeners.Count; i++)
                if (Listeners[i].Element == element)
                    return Listeners[i];
            return new Listener(element);
        }

        //static int instances = 0;
        //int _id=0;
        private Listener(VisualElement element)
        {
            //_id = instances++;
            _element = element;
            bool inserted = false;
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

        /// <summary>
        /// Cancels the active gestures.
        /// </summary>
        public static void CancelActiveGestures()
        {
            GestureService.Cancel();
        }

        bool disposed;
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
            if (disposed)
                return;
            if (disposing)
            {
                Listeners.Remove(this);
                Disposing?.Invoke(this, EventArgs.Empty);
            }
            disposed = true;
        }
        #endregion



    }



}
