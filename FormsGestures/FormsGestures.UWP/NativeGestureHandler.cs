using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
		#region debug helpers
		readonly static int DebugVerbosity = 2;  // "1" (instantiation method names), "2" (usage method names), "3" (manipulation method details)
		void DebugMethodName(int verbosity = 0, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
		{
			if (DebugVerbosity >= verbosity && DebugCondition)
			{
				System.Diagnostics.Debug.WriteLine("⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽⎽");
				System.Diagnostics.Debug.WriteLine(callerName + " Element=[" + _xfElement + "] id=[" + _xfElement.Id + "]");
				System.Diagnostics.Debug.WriteLine("⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺⎺");
			}
		}



		void DebugMessage(string message, int verbosity = 3)
		{
			if (DebugVerbosity >= verbosity && DebugCondition)
				System.Diagnostics.Debug.WriteLine("\t" + message);
		}


		bool DebugCondition
		{
			get
			{
				return false;
				//return _xfElement.GetType().ToString() == "Forms9Patch.BaseCellView";
			}
		}

		bool PointerRoutedDebugMessage(PointerRoutedEventArgs e, string commandName)
		{
			var currentPoint = e.GetCurrentPoint(null);
			if (!currentPoint.IsInContact)
				return false;
			DebugMethodName(1, commandName);
			DebugMessage("CurrentPoint: id=[" + currentPoint.PointerId + "] device=[" + currentPoint.PointerDevice + "] pos=[" + currentPoint.Position.X + "," + currentPoint.Position.Y + "] IsInContact=[" + currentPoint.IsInContact + "]");
			return true;
		}


		void ModesDebugMessage(ManipulationModes modes)
		{
			var modesString = new StringBuilder();
			foreach (ManipulationModes mode in Enum.GetValues(typeof(ManipulationModes)))
			{
				if ((mode & modes) != 0)
					modesString.Append("[" + mode + "]");
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
			DebugMessage("Position: x=[" + point.X + "] y=[" + point.Y + "]");
		}

		void PointerDeviceTypeDebugMessage(Windows.Devices.Input.PointerDeviceType types)
		{
			var deviceString = "";
			foreach (Windows.Devices.Input.PointerDeviceType type in Enum.GetValues(typeof(Windows.Devices.Input.PointerDeviceType)))
			{
				if ((type & types) != 0)
#pragma warning disable CC0039 // Don't concatenate strings in loops
					deviceString += "[" + type + "]";
#pragma warning restore CC0039 // Don't concatenate strings in loops
			}
			DebugMessage("Device: " + deviceString);
		}

		void ManipulationDeltaDebugMessage(ManipulationDelta delta, string name)
		{
			DebugMessage("" + name + ": trans=[" + delta.Translation.X + "," + delta.Translation.Y + "] dip");
			DebugMessage("       expan=[" + delta.Expansion + "] dip");
			DebugMessage("       scale=[" + delta.Scale + "] %");
			DebugMessage("       rotat=[" + delta.Rotation + "] degrees");
		}

		void HandledDebugString(bool handled)
		{
			DebugMessage("Handled=[" + handled + "] Element=[" + _xfElement + "]");
		}

		void VelocitiesDebugString(ManipulationVelocities velocities)
		{
			DebugMessage("Velocities: linear=[" + velocities.Linear.X + "," + velocities.Linear.Y + "] dip");
			DebugMessage("            expans=[" + velocities.Expansion + "] dip/ms");
			DebugMessage("            angulr =[" + velocities.Angular + "] degrees/ms");
		}
		#endregion


		#region Fields

		readonly VisualElement _xfElement;

		bool _longPressed;
		bool _panning;
		bool _pinching;
		bool _rotating;
		//static bool _longPressing;

		int _upFires;
		int _numberOfTaps;

		System.Timers.Timer TappedTimer;
		int _tappedTimerNumberOfTaps;

		int _downFires;
		DateTime _onDownDateTime = DateTime.MinValue;

		System.Timers.Timer LongPressTimer;

		//Stopwatch _holdTimer;
		//Stopwatch _releaseTimer;

		#endregion

		static readonly BindableProperty GestureHandlerProperty = BindableProperty.Create("GestureHandler", typeof(NativeGestureHandler), typeof(NativeGestureHandler), null);

		#region Construction / Disposal
		public NativeGestureHandler()
		{
			P42.Utils.Debug.AddToCensus(this);
		}

		bool _disposed;
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed && disposing)
			{
				_disposed = true;
				if (_xfElement != null)
				{
					_xfElement.PropertyChanging -= OnElementPropertyChanging;
					_xfElement.PropertyChanged -= OnElementPropertyChanged;
				}
				DisconnectFrameworkEvents();
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
				P42.Utils.Debug.RemoveFromCensus(this);
			}
		}
		#endregion


		#region CurrentManipulationModes
		ManipulationModes CurrentManipulationModes
		{
			get
			{
				var modes = ManipulationModes.None;
				if (HandlesPans)
				{
					/*
                    var element = (_listeners != null && _listeners.Count > 0 ? _listeners[0].Element : null);
                    if (element.GetType().ToString() == "Forms9Patch.BaseCellView")
                    {
                        modes = ManipulationModes.System;
                        // 2. Find which direction it can scroll
                        //if (scrollView.HorizontalScrollMode == Windows.UI.Xaml.Controls.ScrollMode.Disabled)
                        modes |= ManipulationModes.TranslateX;
                        //if (scrollView.VerticalScrollMode == Windows.UI.Xaml.Controls.ScrollMode.Disabled)
                            modes |= ManipulationModes.TranslateY;
                        //FrameworkElement.ManipulationMode = FrameworkElement.ManipulationMode ^ (scrollView.HorizontalScrollMode > 0 ? ManipulationModes.)
                        // 3. Filter out that/those directions from the ManipulationMode
                    }
                    else
                    */
					modes |= ManipulationModes.TranslateX | ManipulationModes.TranslateY;
					if (modes != ManipulationModes.None)
						modes |= ManipulationModes.TranslateInertia;
				}
				if (HandlesRotates)
					modes |= ManipulationModes.Rotate | ManipulationModes.RotateInertia;
				if (HandlesPinches)
					modes |= ManipulationModes.Scale | ManipulationModes.ScaleInertia;
				//System.Diagnostics.Debug.WriteLine("CurrentManipulationModes[" + _xfElement + "]["+modes+"]");
				// NOTE: Scrolling on UWP touch screens doesn't work without the following two lines;
				if (modes == ManipulationModes.None)
					modes = ManipulationModes.System;
				return modes;
			}
		}
		#endregion


		#region Setup FrameworkElement
		void ConnectFrameworkEvents()
		{
			if (FrameworkElement != null)
			{
				if (_renderer != null)
					_renderer.ElementChanged += OnRendererElementChanged;

				//FrameworkElement.ManipulationStarting += OnManipulationStarting;


				FrameworkElement.ManipulationStarted += OnManipulationStarted;
				FrameworkElement.ManipulationDelta += OnManipulationDelta;
				FrameworkElement.ManipulationInertiaStarting += OnManipulationInertiaStarting;
				FrameworkElement.ManipulationCompleted += OnManipulationComplete;

				FrameworkElement.ManipulationMode = CurrentManipulationModes;




				//FrameworkElement.Tapped += _UwpElement_Tapped;
				FrameworkElement.RightTapped += OnElementRightTapped;
				//FrameworkElement.PointerWheelChanged += _UwpElement_PointerWheelChanged;

				FrameworkElement.PointerPressed += OnPointerDown;
				FrameworkElement.PointerReleased += OnPointerUp;

				//FrameworkElement.Tapped += OnTapped;

				//_UwpElement.PointerMoved += OnPointerMoved;
				//FrameworkElement.PointerExited += _UwpElement_PointerExited;
				//FrameworkElement.PointerEntered += _UwpElement_PointerEntered;
				FrameworkElement.PointerCaptureLost += OnPointerCaptureLost;
				FrameworkElement.PointerCanceled += OnPointerCancelled;

				//FrameworkElement.Holding += OnHolding;  // holding event doesn't work with Mouse (https://stackoverflow.com/questions/34995594/holding-event-for-desktop-app-not-firing)

				//FrameworkElement.DoubleTapped += OnDoubleTapped;
				//FrameworkElement.IsDoubleTapEnabled = true;
			}
		}

		void DisconnectFrameworkEvents()
		{
			if (FrameworkElement != null)
			{
				FrameworkElement.ManipulationMode = ManipulationModes.None;

				if (_renderer != null)
					_renderer.ElementChanged -= OnRendererElementChanged;

				//FrameworkElement.ManipulationMode = ManipulationModes.None;  // commented this out because line 112 was commented out
				//FrameworkElement.ManipulationStarting -= OnManipulationStarting;

				FrameworkElement.ManipulationStarted -= OnManipulationStarted;
				FrameworkElement.ManipulationDelta -= OnManipulationDelta;
				FrameworkElement.ManipulationInertiaStarting -= OnManipulationInertiaStarting;
				FrameworkElement.ManipulationCompleted -= OnManipulationComplete;

				//FrameworkElement.Tapped -= _UwpElement_Tapped;
				FrameworkElement.RightTapped -= OnElementRightTapped;
				//FrameworkElement.PointerWheelChanged -= _UwpElement_PointerWheelChanged;

				FrameworkElement.PointerPressed -= OnPointerDown;
				FrameworkElement.PointerReleased -= OnPointerUp;

				//FrameworkElement.Tapped -= OnTapped;

				//_UwpElement.PointerMoved -= OnPointerMoved;
				//FrameworkElement.PointerExited -= _UwpElement_PointerExited;
				//FrameworkElement.PointerEntered -= _UwpElement_PointerEntered;
				FrameworkElement.PointerCaptureLost -= OnPointerCaptureLost;
				FrameworkElement.PointerCanceled -= OnPointerCancelled;

				//FrameworkElement.Holding -= OnHolding;

				//FrameworkElement.DoubleTapped -= OnDoubleTapped;
				//FrameworkElement.IsDoubleTapEnabled = false;

				/*
				_downHandled = null;
				_upHandled = null;
				_tapHandled = null;
				_longPressHandled = null;
				_doubleTapHandled = null;
				*/
			}
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0069:Disposable fields should be disposed", Justification = "we don't own _renderer")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "<Pending>")]
        IVisualElementRenderer _renderer;
		void SetRenderer()
		{
			var value = Platform.GetRenderer(_xfElement);
			if (value != _renderer)
			{
				DisconnectFrameworkEvents();

				_renderer = value;

				ConnectFrameworkEvents();
			}
		}

		FrameworkElement FrameworkElement => _renderer as FrameworkElement;

		#endregion


		#region Wiring between XF element / FG listener / Uwp FrameworkElement
		List<Listener> _listeners = new List<Listener>();

		void AddListener(Listener listener)
		{
			if (!(_listeners.Contains(listener)))
			{
				_listeners.Add(listener);

				listener.Disposing += OnListenerDisposing;
				listener.HandlesPannedChanged += OnListenerHandlesManipulationChanged;
				listener.HandlesPanningChanged += OnListenerHandlesManipulationChanged;
				listener.HandlesPinchedChanged += OnListenerHandlesManipulationChanged;
				listener.HandlesPinchingChanged += OnListenerHandlesManipulationChanged;
				listener.HandlesRotatedChanged += OnListenerHandlesManipulationChanged;
				listener.HandlesRotatingChanged += OnListenerHandlesManipulationChanged;
				listener.HandlesSwipedChanged += OnListenerHandlesManipulationChanged;
			}
		}

		private void OnListenerDisposing(object sender, System.EventArgs e)
		{
			if (sender is Listener listener && _listeners != null)
			{
				listener.HandlesPannedChanged -= OnListenerHandlesManipulationChanged;
				listener.HandlesPanningChanged -= OnListenerHandlesManipulationChanged;
				listener.HandlesPinchedChanged -= OnListenerHandlesManipulationChanged;
				listener.HandlesPinchingChanged -= OnListenerHandlesManipulationChanged;
				listener.HandlesRotatedChanged -= OnListenerHandlesManipulationChanged;
				listener.HandlesRotatingChanged -= OnListenerHandlesManipulationChanged;
				listener.HandlesSwipedChanged -= OnListenerHandlesManipulationChanged;

				if (_listeners.Contains(listener))
					_listeners.Remove(listener);
				if (_listeners.Count < 1 && _xfElement != null)
				{
					// no one is listening so shut down
					_xfElement.SetValue(GestureHandlerProperty, null);
					_xfElement.PropertyChanging -= OnElementPropertyChanging;
					_xfElement.PropertyChanged -= OnElementPropertyChanged;
				}
			}
		}

		static NativeGestureHandler GetInstanceForElement(VisualElement element)
		{
			return (NativeGestureHandler)element.GetValue(GestureHandlerProperty);
		}

		static internal NativeGestureHandler GetInstanceForListener(Listener listener)
		{
			var uwpGestureHandler = GetInstanceForElement(listener.Element) ?? new NativeGestureHandler(listener.Element);
			uwpGestureHandler.AddListener(listener);
			return uwpGestureHandler;
		}


		void OnRendererElementChanged(object sender, ElementChangedEventArgs<VisualElement> e)
		{
			SetRenderer();
		}

		void OnElementPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
		{

			if (sender is VisualElement)
			{
				if (e.PropertyName == "Renderer")
					SetRenderer();
				else if (e.PropertyName == GestureHandlerProperty.PropertyName)
					_xfElement.Behaviors.Remove(this);
			}
		}

		void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (sender is VisualElement)
			{
				if (e.PropertyName == "Renderer")
					SetRenderer();
			}
		}

		// only called when a GestureHandler doesn't already exist for this VisualElement
		NativeGestureHandler(VisualElement element)
		{
			_xfElement = element;
			_xfElement.PropertyChanging += OnElementPropertyChanging;
			_xfElement.PropertyChanged += OnElementPropertyChanged;
			_xfElement.Behaviors.Add(this);
		}

		protected override void OnAttachedTo(VisualElement bindable)
		{
			bindable.SetValue(GestureHandlerProperty, this);
			SetRenderer();
			base.OnAttachedTo(bindable);
		}

		protected override void OnDetachingFrom(VisualElement bindable)
		{
			// arguably, none of this is necessary since Element owns everything.
			// silence events
			SetRenderer();
			bindable.SetValue(GestureHandlerProperty, null);
			base.OnDetachingFrom(bindable);
		}

		void OnListenerHandlesManipulationChanged(object sender, bool state)
		{
			if (FrameworkElement != null)
				FrameworkElement.ManipulationMode = CurrentManipulationModes;
		}
		#endregion


		#region Handles tests
		bool HandlesTest(Func<Listener, bool> test)
		{
			foreach (var listener in _listeners)
				if (test(listener))
					return true;
			return false;
		}

		bool HandlesDownUps => HandlesTest(listener => listener.HandlesDown || listener.HandlesUp);

		bool HandlesLongPresses => HandlesTest(listener => listener.HandlesLongPressing || listener.HandlesLongPressed);

		bool HandlesTapped => HandlesTest(listener => listener.HandlesTapped);

		bool HandlesTaps => HandlesTest(listener => listener.HandlesTapping || listener.HandlesTapped || listener.HandlesDoubleTapped);

		bool HandlesDoubleTaps => HandlesTest(listener => listener.HandlesDoubleTapped);

		bool HandlesPans => HandlesTest(listener => listener.HandlesPanning || listener.HandlesPanned);

		bool HandlesRotates => HandlesTest(listener => listener.HandlesRotating || listener.HandlesRotated);

		bool HandlesPinches => HandlesTest(listener => listener.HandlesPinching || listener.HandlesPinched);

		bool RendererExists(Listener listener) => listener?.Element!=null && Platform.GetRenderer(listener.Element as VisualElement) != null;
		#endregion


		#region Properties

		PointerRoutedEventArgs _tappedTimerUpMotionEvent;
		PointerRoutedEventArgs TappedTimerUpMotionEvent
		{
			get => _tappedTimerUpMotionEvent;
			set => _tappedTimerUpMotionEvent = value;
			
		}
		#endregion


		#region LongPress Timer
		void LongPressingTimerStop()
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			if (LongPressTimer != null)
			{
				LongPressTimer.Stop();
				LongPressTimer.Elapsed -= OnLongPressTimerElapsed;
				LongPressTimer = null;
			}
		}

		void LongPressTimerStart()
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
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
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			LongPressingTimerStop();

			long elapsed = (long)(DateTime.Now - _onDownDateTime).TotalMilliseconds;

			if (!_panning && !_longPressed && !_pinching && !_rotating)
			{
				_longPressed = true;
				Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				{
					foreach (var listener in _listeners)
					{
						if (listener.HandlesLongPressing && RendererExists(listener))
						{
							var args = new UwpLongPressEventArgs(FrameworkElement, elapsed)
							{
								Listener = listener
							};
							listener?.OnLongPressing(args);
							System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] listener.Element["+listener?.Element+"] handled["+args.Handled+"]");
							if (args.Handled)
								break;
						}
					}
				});
			}
		}
		#endregion


		#region Tapped Timer
		void TappedTimerStop()
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			if (TappedTimer != null)
			{
				TappedTimer.Stop();
				TappedTimer.Elapsed -= OnTappedTimerElapsed;
				TappedTimer = null;
				//_tappedTimerUpMotionEvent = null;
			}
		}

		void TappedTimerStart(PointerRoutedEventArgs upEvent, int taps)
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
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
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			TappedTimerStop();
			_numberOfTaps = 0;
			if (!_panning && !_longPressed && !_pinching && !_rotating)
				OnTappedTimerElapsedInner();
		}

		void OnTappedTimerElapsedInner()
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			if (P42.Utils.Environment.IsOnMainThread)
			{
				try
				{
					foreach (var listener in _listeners)
					{
						if (listener.HandlesTapped && RendererExists(listener))
						{
							if (UwpTapEventArgs.FireTapped(FrameworkElement, _numberOfTaps, listener))
							{
								System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": listener.Element[" + listener.Element + "] _numberOfTaps[" + _numberOfTaps + "] HANDLED");
								break;
							}
						}
					}
				}
				catch (Exception) { }
			}
			else
				Device.BeginInvokeOnMainThread(() => OnTappedTimerElapsedInner());
		}

		#endregion


		#region Cancelation
		public void Cancel(object sender, PointerRoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			StopTapLongPress();

			CallOnUp(sender, e);
			//TODO: Not necessary?
			//if (_panning)
			//	CallOnPanned(sender, e);
			_panning = false;
			_pinching = false;
			_rotating = false;
			//_multiMoving = false;

		}

		void StopTapLongPress()
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			TappedTimerStop();
			LongPressingTimerStop();
			_numberOfTaps = 0;
		}
		#endregion


		#region Up / Panned Wrappers
		bool CallOnUp(object sender, PointerRoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			foreach (var listener in _listeners)
			{
				if (listener.HandlesUp && UwpDownUpArgs.FireUp(FrameworkElement, e, listener))
				{
					System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ["+listener?.Element+"] HANDLED");
					return true;
				}
			}
			return false;
		}

		// TODO: Not necessary?
		bool CallOnPanned(object sender, PointerRoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			return false;
		}
		#endregion


		#region UWP Pointer Cancel Event Responders
		private void OnPointerCancelled(object sender, PointerRoutedEventArgs e)
		{
			Cancel(sender, e);
			/*
			_downHandled = null;

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
				if (_tapHandled == null && listener.HandlesTapped)
				{
					System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": listener.Element[" + listener.Element + "] e.Handled[" + e.Handled + "]");
					var args = new UwpTapEventArgs(FrameworkElement, e, _numberOfTaps)
					{
						Listener = listener,
						Cancelled = true
					};
					listener?.OnTapped(args);
					e.Handled = args.Handled;
					_tapHandled = listener;
				}
				if (_longPressHandled == null && _longPressing && listener.HandlesLongPressed)
				{
					System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": listener.Element[" + listener.Element + "] e.Handled[" + e.Handled + "]");
					var args = new UwpLongPressEventArgs(FrameworkElement, e, elapsed)
					{
						Listener = listener,
						Cancelled = true
					};
					listener?.OnLongPressed(args);
					e.Handled = args.Handled;
					_longPressHandled = listener;
				}
				if (listener.HandlesUp)
				{
					System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": listener.Element[" + listener.Element + "] e.Handled[" + e.Handled + "]");
					var args = new UwpDownUpArgs(FrameworkElement, e)
					{
						Listener = listener,
						Cancelled = true
					};
					listener.OnUp(args);
					e.Handled = args.Handled;
					_upHandled = listener;
					if (e.Handled)
						return;
				}

			}
			*/
		}

		private void OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
		{
			Cancel(sender, e);
			/*
			_downHandled = null;
			_upHandled = null;
			_tapHandled = null;
			_doubleTapHandled = null;
			_longPressHandled = null;

			foreach (var listener in _listeners)
				System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": listener.Element[" + listener.Element + "] e.Handled[" + e.Handled + "]");
			PointerRoutedDebugMessage(e, "POINTER CAPTURE LOST");
			//OnPointerCancelled
			*/
		}
        #endregion


        #region UWP Pointer Down/Up Event Responders
        private void OnElementRightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			if (!_xfElement.IsVisible || FrameworkElement == null)
				return;

			foreach (var listener in _listeners)
			{
				if (listener.HandlesRightClicked && UwpRightClickEventArgs.Fire(FrameworkElement, e, listener))
				{
					System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] [" + listener?.Element + "] HANDLED ");
					break;
				}
			}
		}

		//object _downSource;
		private void OnPointerDown(object sender, PointerRoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			if (!_xfElement.IsVisible || FrameworkElement == null)
				return;

			_onDownDateTime = DateTime.Now;
			_downFires++;

			_panning = false;
			_pinching = false;
			_rotating = false;
			//_multimoving = false;
			TappedTimerStop();
			LongPressTimerStart();

			foreach (var listener in _listeners)
			{
				if (listener.HandlesDown && UwpDownUpArgs.FireDown(FrameworkElement, e, listener))
				{
					System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "]  [" + listener?.Element + "] HANDLED");
					return;
				}
			}




		}

		/*
		static object _downHandled;
		static object _upHandled;
		static object _tapHandled;
		static object _doubleTapHandled;
		static object _longPressHandled;

		bool _runningTapCounterResetter;
		*/
		private void OnPointerUp(object sender, PointerRoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");

			if (!_xfElement.IsVisible || FrameworkElement == null)
				return;

			_numberOfTaps++;
			LongPressingTimerStop();
			TappedTimerStart(e, _numberOfTaps);

			_upFires++;

			long elapsed = (long)(DateTime.Now - _onDownDateTime).TotalMilliseconds;

			CallOnUp(sender, e);

			if (_panning)
				CallOnPanned(sender, e);
			else if (_longPressed)
			{
				foreach (var listener in _listeners)
				{
					if (listener.HandlesLongPressed && UwpLongPressEventArgs.FireLongPressed(FrameworkElement, e, elapsed, listener))
					{
						System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": FireLongPressed[" + null + "]");
						break;
					}
				}
			}
			else if (!_pinching && !_rotating)
			{
				foreach (var listener in _listeners)
				{
					if (listener.HandlesTapping && UwpTapEventArgs.FireTapping(FrameworkElement, e, _numberOfTaps, listener))
					{
						System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": FireTapping[" + null + "]");
						break;
					}
					if (_numberOfTaps % 2 == 0 && listener.HandlesDoubleTapped && UwpTapEventArgs.FireDoubleTapped(FrameworkElement, e, _numberOfTaps, listener))
					{
						System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": FireOnDoubleTapping[" + null + "]");
						break;
					}

				}
			}



			/*
			_downHandled = null;

			//_pointerSource = e.OriginalSource;

			DebugMethodName(2);

			var senderRenderer = sender as Xamarin.Forms.Platform.UWP.LayoutRenderer;

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
					if (_releaseTimer == null || _releaseTimer.Elapsed > Settings.Tapp)
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
				System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": listener.Element[" + listener.Element + "] _numberOfTaps[" + _numberOfTaps + "]  sender=[" + senderRenderer?.Name + "] [" + senderRenderer?.Element + "] e.Handled[" + e.Handled + "]");
				if (_tapHandled == null && listener.HandlesTapped && UwpTapEventArgs.FireTapped(FrameworkElement, e, _numberOfTaps, listener))
				{
					e.Handled = true;
					_tapHandled = listener;
					System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": FireTapped[" + null + "]");
					break;
				}
			}

			foreach (var listener in _listeners)
				if (_longPressHandled == null && _longPressing && listener.HandlesLongPressed && UwpLongPressEventArgs.FireLongPressed(FrameworkElement, e, elapsed, listener))
				{
					e.Handled = true;
					_longPressHandled = listener;
					System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": FireLongPressed[" + null + "]");
					break;
				}


			foreach (var listener in _listeners)
				if (_upHandled == null && listener.HandlesDown && UwpDownUpArgs.FireUp(FrameworkElement, e, listener))
				{
					e.Handled = true;
					_upHandled = listener;
					System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": FireUp[" + null + "]");
					break;
				}


			_longPressing = false;
			*/
		}

		/*
		private void OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
		{
			if (!_xfElement.IsVisible || FrameworkElement == null)
				return;


			//if (!_pointerTracking)
			//    return;

			DebugMethodName(2);
			foreach (var listener in _listeners)
				if (_doubleTapHandled != null && listener.HandlesDoubleTapped && UwpTapEventArgs.FireDoubleTapped(FrameworkElement, e, _numberOfTaps, listener))
				{
					e.Handled = true;
					_doubleTapHandled = listener;
					break;
				}
			_longPressing = false;
		}
		*/
		/*
        
        private void OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            DebugMethodName(2);
        }

            
        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            DebugMethodName(2);
            throw new NotImplementedException();
        }

        void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            // USE OnManipulationDelta INSTEAD
            if (!_xfElement.IsVisible || FrameworkElement == null)
                return;
            DebugMethodName(2);


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


		#region UWP Manipulations (multi-touch)

#pragma warning disable CC0057 // Unused parameters
#pragma warning disable IDE0060 // Remove unused parameter
		void OnManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CC0057 // Unused parameters
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			/*
			DebugMethodName(2);
			ModesDebugMessage(e.Mode);
			PivotDebugMessage(e.Pivot);
			ContainerDebugMessage(e.Container);
			*/
		}


		void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			/*
			DebugMethodName(2);
			DebugMessage("Element=[" + _xfElement + "]");
			PointerDeviceTypeDebugMessage(e.PointerDeviceType);
			PositionDebugMessage(e.Position);
			ManipulationDeltaDebugMessage(e.Cumulative, "Cumul");
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
			DebugMessage("elapse=[" + elapsed + "]");
			if (!_panning || !_pinching || !_rotating)
			{
				_longPressing = elapsed > 750;
				if (_longPressing)
				{
					foreach (var listener in _listeners)
					{
						if (listener.HandlesLongPressing)
						{
							var args = new UwpLongPressEventArgs(FrameworkElement, e, elapsed)
							{
								Listener = listener
							};
							listener?.OnLongPressing(args);
							e.Handled = args.Handled;
							DebugMessage("LongPressing Handled=[" + e.Handled + "]");
							if (e.Handled)
								break;
						}
					}
				}
			}
			HandledDebugString(e.Handled);
			*/
		}

		private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			//System.Diagnostics.Debug.WriteLine(P42.Utils.ReflectionExtensions.CallerMemberName() +"("+sender+", [d:{"+e.Delta.Translation.X+","+e.Delta.Translation.Y+"} c:{"+e.Cumulative.Translation.Y+","+e.Cumulative.Translation.Y+"}])");

			//_longPressing = false;
			//_holdTimer?.Stop();
			//_holdTimer = null;

			if (!_xfElement.IsVisible || FrameworkElement == null)
				return;

			if (!_panning && (Math.Abs(e.Cumulative.Translation.X) > 0 || Math.Abs(e.Cumulative.Translation.Y) > 0))
				_panning = true;
			if (!_pinching && Math.Abs(e.Cumulative.Scale - 1) > 0)
				_pinching = true;
			if (!_rotating && Math.Abs(e.Cumulative.Rotation) > 0)
				_rotating = true;

			/*
			DebugMethodName(2);
			DebugMessage("Element=[" + _xfElement + "]");
			PointerDeviceTypeDebugMessage(e.PointerDeviceType);
			PositionDebugMessage(e.Position);
			ManipulationDeltaDebugMessage(e.Delta, "Delta");
			ManipulationDeltaDebugMessage(e.Cumulative, "Cumul");
			VelocitiesDebugString(e.Velocities);
			DebugMessage("IsIntertial=[" + e.IsInertial + "]");
			ContainerDebugMessage(e.Container);
			HandledDebugString(e.Handled);
			*/
			foreach (var listener in _listeners)
			{
				if (_panning && listener.HandlesPanning)
				{
					var args = new UwpPanEventArgs(FrameworkElement, e)
					{
						Listener = listener
					};
					listener?.OnPanning(args);
					e.Handled = e.Handled || args.Handled;
					//DebugMessage("Panning Handled=[" + e.Handled + "]");
				}
				if (_pinching && listener.HandlesPinching)
				{
					var args = new UwpPinchEventArgs(FrameworkElement, e)
					{
						Listener = listener
					};
					listener?.OnPinching(args);
					e.Handled = e.Handled || args.Handled;
					//DebugMessage("Pinching Handled=[" + e.Handled + "]");
				}
				if (_rotating && listener.HandlesRotating)
				{
					var args = new UwpRotateEventArgs(FrameworkElement, e)
					{
						Listener = listener
					};
					listener?.OnRotating(args);
					e.Handled = e.Handled || args.Handled;
					//DebugMessage("Rotating Handled=[" + e.Handled + "]");
				}
				if (e.Handled)
					break;
			}
			//HandledDebugString(e.Handled);
		}

		private void OnManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			/*
			DebugMethodName(2);
			DebugMessage("Element=[" + _xfElement + "]");
			PointerDeviceTypeDebugMessage(e.PointerDeviceType);
			ManipulationDeltaDebugMessage(e.Delta, "Delta");
			ManipulationDeltaDebugMessage(e.Cumulative, "Cumul");
			VelocitiesDebugString(e.Velocities);
			ContainerDebugMessage(e.Container);
			DebugMessage("TranslationBehavioir: disp=[" + e.TranslationBehavior.DesiredDisplacement + "] decl=[" + e.TranslationBehavior.DesiredDeceleration + "]");
			DebugMessage("RotationBehavior: rot=[" + e.RotationBehavior.DesiredRotation + "] decl=[" + e.RotationBehavior.DesiredDeceleration + "]");
			DebugMessage("ExpansionBehavior: exp=[" + e.ExpansionBehavior.DesiredExpansion + "] decl=[" + e.ExpansionBehavior.DesiredDeceleration + "]");
			HandledDebugString(e.Handled);
			*/
		}

		private void OnManipulationComplete(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerString() + "] ");
			if (!_xfElement.IsVisible || FrameworkElement == null)
				return;
			/*
			DebugMethodName(2);
			DebugMessage("Element=[" + _xfElement + "]");
			PointerDeviceTypeDebugMessage(e.PointerDeviceType);
			PositionDebugMessage(e.Position);
			ManipulationDeltaDebugMessage(e.Cumulative, "Cumul");
			VelocitiesDebugString(e.Velocities);
			DebugMessage("IsIntertial=[" + e.IsInertial + "]");
			ContainerDebugMessage(e.Container);
			HandledDebugString(e.Handled);
			*/
			foreach (var listener in _listeners)
			{
				if (_panning && listener.HandlesPanning)
				{
					var args = new UwpPanEventArgs(FrameworkElement, e)
					{
						Listener = listener
					};
					listener?.OnPanned(args);
					e.Handled = e.Handled || args.Handled;
					//DebugMessage("Panned tHandled=[" + e.Handled + "]");
				}
				if (_pinching && listener.HandlesPinching)
				{
					var args = new UwpPinchEventArgs(FrameworkElement, e)
					{
						Listener = listener
					};
					listener?.OnPinched(args);
					e.Handled = e.Handled || args.Handled;
					//DebugMessage("Pinched Handled=[" + e.Handled + "]");
				}
				if (_rotating && listener.HandlesRotating)
				{
					var args = new UwpRotateEventArgs(FrameworkElement, e)
					{
						Listener = listener
					};
					listener?.OnRotated(args);
					e.Handled = e.Handled || args.Handled;
					//DebugMessage("Rotated Handled=[" + e.Handled + "]");
				}

				DebugMessage("Handled=[" + e.Handled + "] Element=[" + _xfElement + "]");
				if (e.Handled)
					break;
			}

			_panning = false;
			_pinching = false;
			_rotating = false;
			//_longPressing = false;
		}
		#endregion



	}
}
