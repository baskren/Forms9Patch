using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Android.Views;
using P42.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FormsGestures.Droid
{
    class NativeGestureHandler : Behavior<VisualElement>, IDisposable
    {
        //bool _debugEvents;

        static readonly BindableProperty GestureHandlerProperty = BindableProperty.Create("GestureHandler", typeof(NativeGestureHandler), typeof(NativeGestureHandler), null);

        //global::Android.Views.View _view;
        Java.Lang.Ref.WeakReference _weakReferenceView;

        NativeGestureListener nativeListener;

        NativeGestureDetector nativeDetector;

        MotionEventActions lastAction = MotionEventActions.Up;

        long lastEventTime;

        int lastPointerCount;

        internal VisualElement _element { get; set; }

        List<Listener> _listeners;


        void AddElementListener(VisualElement element, Listener listener)
        {

            if (_listeners != null && !(_listeners.Contains(listener)))
            {
                _listeners.Add(listener);
                listener.Disposing += RemoveListener;
                OnChildAdded(element, null);
            }
        }

        void RemoveListener(object sender, EventArgs e = null)
        {
            var listener = sender as Listener;
            if (_listeners != null)
            {
                if (_listeners.Contains(listener))
                {
                    RemoveChildren();
                    _listeners.Remove(listener);
                }
                if (_listeners.Count < 1)
                {
                    // no one is listening so shut down
                    _element.SetValue(GestureHandlerProperty, null);
                    if (_element is Xamarin.Forms.Layout layout)
                    {
                        layout.ChildAdded -= OnChildAdded;
                        layout.ChildRemoved -= OnChildRemoved;
                    }
                }
            }
        }


        void OnChildAdded(object parentElement, ElementEventArgs e)
        {
            if (parentElement is Layout<Xamarin.Forms.View> vLayout)
            {
                foreach (var child in vLayout.Children)
                    ConnectChildElementToItsParentListeners(child);
            }
            else
            {
                if (parentElement is ContentView contentView)
                    ConnectChildElementToItsParentListeners(contentView.Content);
                else
                {
                    if (parentElement is ScrollView scrollView)
                        ConnectChildElementToItsParentListeners(scrollView.Content);
                }
            }
        }

        void ConnectChildElementToItsParentListeners(Xamarin.Forms.View child)
        {
            if (child != null)
            {
                if (_listeners != null)
                    foreach (var parentListener in _listeners)
                        GetInstanceForElementAndListener(child, parentListener);
                if (!_children.Contains(child))
                    _children.Add(child);
            }
        }


        void OnChildRemoved(object parentElement, ElementEventArgs e)
        {
            if (e.Element is Xamarin.Forms.View child)
                DisconnectChildElementFromItsParentListeners(child);
        }

        void RemoveChildren()
        {
            for (int i = _children.Count - 1; i >= 0; i--)
                DisconnectChildElementFromItsParentListeners(_children[i]);
        }

        void DisconnectChildElementFromItsParentListeners(Xamarin.Forms.View child)
        {
            if (child != null)
            {
                if (_children.Contains(child))
                    _children.Remove(child);
                if (_listeners != null)
                {
                    foreach (var parentListener in _listeners)
                    {
                        var gestureHandler = GetInstanceForElement(child);
                        gestureHandler?.RemoveListener(parentListener);
                    }

                }
            }
        }


        static NativeGestureHandler GetInstanceForElement(VisualElement element)
        {
            return (NativeGestureHandler)element.GetValue(GestureHandlerProperty);
        }

        static internal NativeGestureHandler GetInstanceForListener(Listener listener)
        {
            /*
			var gestureHandler = GetInstanceForElement(listener.Element) ?? new NativeGestureHandler (listener.Element);
			gestureHandler.AddListener (listener);
			return gestureHandler;
			*/
            return GetInstanceForElementAndListener(listener.Element, listener);
        }

        static NativeGestureHandler GetInstanceForElementAndListener(VisualElement element, Listener listener)
        {
            var gestureHandler = GetInstanceForElement(element) ?? new NativeGestureHandler(element);
            gestureHandler.AddElementListener(element, listener);
            if (element is Xamarin.Forms.Layout layout)
            {
                layout.ChildAdded += gestureHandler.OnChildAdded;
                layout.ChildRemoved += gestureHandler.OnChildRemoved;
            }
            return gestureHandler;
        }




        List<Xamarin.Forms.View> _children = new List<Xamarin.Forms.View>();

        static int instances;
        int _id;
        NativeGestureHandler(VisualElement element)
        {
            _id = instances++;
            _element = element;
            _element.Behaviors.Add(this);
            _listeners = new List<Listener>();
        }



        Delegate _onRendererElementChangedHandler;
        void AddOnElementChanged_T_Handler(object renderer)
        {
            var eventInfo = renderer.GetType().GetTypeInfo().GetEvent("ElementChanged");
            if (_onRendererElementChangedHandler == null)
            {
                var eventInfoType = eventInfo.EventHandlerType;
                if (eventInfoType.IsGenericType)
                {
                    var eventArgumentType = eventInfoType.GenericTypeArguments[0];  //.GenericTypeArguments[0];
                    if (eventArgumentType.IsGenericType)
                        eventArgumentType = eventArgumentType.GenericTypeArguments[0];
                    else
                        System.Diagnostics.Debug.WriteLine("");
                    var onRendererChangedMethodInfo = GetType().GetTypeInfo().GetMethod("GenericOnRendererElementChanged");//, new Type[] { typeof(object), eventArgumentType });
                    var genericOnRendererChangedMethodInfo = onRendererChangedMethodInfo.MakeGenericMethod(eventArgumentType);
                    _onRendererElementChangedHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, genericOnRendererChangedMethodInfo);

                }
                else
                    throw new NotSupportedException("renderer.ElementChanged event is not generic.");

            }
            eventInfo.AddEventHandler(renderer, _onRendererElementChangedHandler);
        }



        void RemoveOnElementChanged_T_Handler(object renderer)
        {
            var eventInfo = renderer.GetType().GetTypeInfo().GetEvent("ElementChanged");
            if (_onRendererElementChangedHandler == null)
            {
                var eventInfoType = eventInfo.EventHandlerType;
                var eventArgumentType = eventInfoType.GenericTypeArguments[0].GenericTypeArguments[0];
                var onRendererChangedMethodInfo = GetType().GetTypeInfo().GetMethod("GenericOnRendererElementChanged");//, new Type[] { typeof(object), eventArgumentType });
                var genericOnRendererChangedMethodInfo = onRendererChangedMethodInfo.MakeGenericMethod(eventArgumentType);
                _onRendererElementChangedHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, genericOnRendererChangedMethodInfo);
            }
            eventInfo.RemoveEventHandler(renderer, _onRendererElementChangedHandler);
        }


        void GenericRendererDisconnect<T>(T element, object renderer = null) where T : VisualElement
        {
            if (renderer == null)
                renderer = element.GetRenderer();
            if (renderer == null)
                renderer = Platform.CreateRenderer(element);
            if (renderer is VisualElementRenderer<Xamarin.Forms.View> defaultRenderer)
                defaultRenderer.ElementChanged -= GenericOnRendererElementChanged;
            else if (renderer is VisualElementRenderer<T> genericRenderer)
                genericRenderer.ElementChanged -= GenericOnRendererElementChanged;
            else if (renderer is FormsViewGroup)
                RemoveOnElementChanged_T_Handler(renderer);
            else if (renderer is IVisualElementRenderer iRenderer)
                iRenderer.ElementChanged -= OnRendererElementChanged;
            else
                throw new NotSupportedException("Unsupported renderer");

        }

        void GenericRendererConnect<T>(T element, object renderer = null) where T : VisualElement
        {
            if (renderer == null)
                renderer = element.GetRenderer();
            if (renderer == null)
                renderer = Platform.CreateRenderer(element);
            if (renderer is VisualElementRenderer<Xamarin.Forms.View> defaultRenderer)
                defaultRenderer.ElementChanged += GenericOnRendererElementChanged;
            else if (renderer is VisualElementRenderer<T> genericRenderer)
                genericRenderer.ElementChanged += GenericOnRendererElementChanged;
            else if (renderer is FormsViewGroup)
                AddOnElementChanged_T_Handler(renderer);
            else if (renderer is IVisualElementRenderer iRenderer)
                iRenderer.ElementChanged += OnRendererElementChanged;
            else
                throw new NotSupportedException("Unsupported renderer");
        }


        public void GenericOnRendererElementChanged<T>(object sender, ElementChangedEventArgs<T> e) where T : Xamarin.Forms.VisualElement
        {
            if (e.OldElement != null)
                //RendererDisconnect();
                GenericRendererDisconnect(e.OldElement, sender);
        }

        void OnRendererElementChanged(object sender, ElementChangedEventArgs<VisualElement> e)
        {
            if (e.OldElement != null)
                //RendererDisconnect();
                GenericRendererDisconnect(e.OldElement, sender);
        }

        void OnElementPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            if (sender is VisualElement element)
            {
                if (e.PropertyName == "Renderer")
                {
                    //RendererDisconnect();
                    GenericRendererDisconnect(_element, null);
                    //if (_debugEvents) System.Diagnostics.Debug.WriteLine("OnElementPropertyChanging RendererDisconnect");
                }
                else if (e.PropertyName == GestureHandlerProperty.PropertyName)
                {
                    _element.Behaviors.Remove(this);
                }

            }
        }

        void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is VisualElement element)
            {
                if (e.PropertyName == "Renderer")
                {
                    //RendererConnect();
                    GenericRendererConnect(_element);
                    //if (_debugEvents) System.Diagnostics.Debug.WriteLine("OnElementPropertyChanged RendererConnect");
                }
            }
        }


        protected override void OnAttachedTo(VisualElement bindable)
        {
            bindable.SetValue(GestureHandlerProperty, this);
            bindable.PropertyChanging += OnElementPropertyChanging;
            bindable.PropertyChanged += OnElementPropertyChanged;
            //RendererConnect();
            GenericRendererConnect(bindable);
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine("OnAttachedTo RendererConnect");
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            // arguably, none of this is necessary since Element owns everything.
            // silence events
            //RendererDisconnect();
            GenericRendererDisconnect(bindable);
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine("OnDetachingFrom RendererDisconnect");
            bindable.PropertyChanging -= OnElementPropertyChanging;
            bindable.PropertyChanged -= OnElementPropertyChanged;
            // cleanup listeners
            //foreach (var listener in _listeners) 
            //	listener.PropertyChanged -= OnListenerPropertyChanged;
            // cleanup properties
            //bindable.SetValue (Gesture_listenersProperty, null);
            _listeners = null;
            bindable.SetValue(GestureHandlerProperty, null);
            base.OnDetachingFrom(bindable);
        }

        internal void AttachNativeGestureHandler(Listener listener)
        {
            NativeGestureHandler.GetInstanceForListener(listener);
        }


        internal void ResetGestureRecognizers(Android.Views.View view)
        {

            ClearGestureRecognizers();
            nativeListener = new NativeGestureListener(view, _listeners);
            nativeDetector = new NativeGestureDetector(Droid.Settings.Context, nativeListener);
            view.Touch += HandleTouch;
            _weakReferenceView = new Java.Lang.Ref.WeakReference(view);
        }

        internal void ClearGestureRecognizers()
        {
            nativeDetector?.Dispose();
            nativeDetector = null;
            nativeListener?.Dispose();
            nativeListener = null;
            RemoveTouchHandler();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                ClearGestureRecognizers();
                _disposed = true;
            }
        }



        void RemoveTouchHandler()
        {
            var _view = (Android.Views.View)_weakReferenceView?.Get();
            if (_view != null)
            {
                try
                {

                    _view.Touch -= HandleTouch;
                    //_view.SetOnTouchListener(null);
                }
#pragma warning disable 0168
                catch (ArgumentException e)
#pragma warning restore 0168
                {
                    string s = "You got this Exception because FormsGestures doesn't want to leak nor have gestures go to the wrong listeners.  Ignore it.  It's really ok.";
                    Console.WriteLine(s);
                    //throw new IgnoreThisException (s);
                }
#pragma warning disable 0168
                catch (ObjectDisposedException e)
#pragma warning restore 0168
                {
                    string s = "You got this Exception because FormsGestures doesn't want to leak nor have gestures go to the wrong listeners.  Ignore it.  It's really ok.";
                    Console.WriteLine(s);
                }
                _view = null;
                _weakReferenceView?.Clear();
                _weakReferenceView = null;
            }

        }

        /*
		bool ShareMotionEvent(Element element, MotionEvent e, VisualElement eventElement) {
			if (element.Parent  != null) {
				foreach (Listener listener in Listener.Listeners) {
					if (listener.Element == element.Parent) {
						NativeGestureHandler parentNativeGestureHandler = GetInstanceForElement (listener.Element);

						var parentEvent = MotionEvent.Obtain (e);
						Point offset = VisualElementExtensions.CoordTransform (eventElement, Point.Zero, listener.Element);
						var scale = Forms.Context.Resources.DisplayMetrics.Density;
						parentEvent.OffsetLocation ((float)(offset.X * scale), (float)(offset.Y * scale));
						if (_debugEvents) System.Diagnostics.Debug.WriteLine ("\t parentEvent=["+parentEvent+"]");
						return parentNativeGestureHandler.HandleMotionEvent (parentEvent);
					}
				}
				return ShareMotionEvent (element.Parent, e, eventElement);
			} else {
				return false;
			}
		}
		*/

        void HandleTouch(object sender, global::Android.Views.View.TouchEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("NativeGestureHandler.HandlTouch");
            var handled = HandleMotionEvent(e.Event);
            if (e.Event.Action == MotionEventActions.Down)
            {
                e.Handled = true;
                XamarinForms_2_4_WorkAround();
            }
        }

        void XamarinForms_2_4_WorkAround()
        {
            if (_weakReferenceView?.Get() is FormsViewGroup formsViewGroup)
            {
                var parent = formsViewGroup.Parent;
                if (parent != null)
                {
                    var parentType = parent.GetType();
                    var fieldName = "_notReallyHandled";
                    var objType = typeof(Platform).GetNestedType("DefaultRenderer", System.Reflection.BindingFlags.NonPublic);
                    if (objType == null)
                        objType = parentType.GetNestedType("DefaultRenderer", System.Reflection.BindingFlags.NonPublic);
                    if (objType != null)
                    {
                        var fieldInfo = P42.Utils.ReflectionExtensions.GetFieldInfo(objType, fieldName);
                        if (fieldInfo != null)
                            try
                            {
                                //var _notReallyHandled = (bool)fieldInfo.GetValue(parent);
                                fieldInfo.SetValue(parent, false);
                            }
                            catch (System.ArgumentException)
                            {
                                // this seems to be happening occasionally.  Need to get to the bottom of it!
                                System.Diagnostics.Debug.WriteLine("IGNORE THIS EXCEPTION.  EVERYTHING IS OK");
                            }
                    }
                }
            }
        }


        bool lastEventHandled;
        bool HandleMotionEvent(MotionEvent e)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine("[{0}.{1}] [{2}] [{3}]", GetType().Name, Debug.CurrentMethod(), _id, _element);
            //ShareMotionEvent (_element, e, _element);
            if (!_element.IsVisible)
                return false;
            if (MatchesLastMotionEvent(e))
                return false;
            lastEventHandled &= e.Action != MotionEventActions.Down;
            if (nativeDetector != null)
                lastEventHandled |= nativeDetector.OnTouchEvent(e);
            //if (!lastEventHandled) 
            //if (!lastEventHandled) {
            object scrollEnabled = _element.GetPropertyValue("ScrollEnabled");
            if (scrollEnabled == null || ((bool)scrollEnabled) || e.Action != MotionEventActions.Move)
            {
                var _view = (Android.Views.View)_weakReferenceView?.Get();
                if (_view != null)
                {
                    var renderer = Platform.GetRenderer(_element);
                    //var currentView = (renderer?.GetPropertyValue("Control") as Android.Views.View) ?? renderer?.ViewGroup;
                    var currentView = (renderer?.GetPropertyValue("Control") as Android.Views.View) ?? renderer?.View;
                    if (currentView != null && _view == currentView)
                        _view.OnTouchEvent(e);
                }
            }
            //}
            return true;  // we want to be sure we get the updates to this element's events
        }

        /*
		internal static void PrintMotionEvent(string title, MotionEvent e) {
			var stringBuilder = new StringBuilder(title);
			stringBuilder.AppendFormat(": Action: {0}", e.Action);
			for (int i = 0; i < e.PointerCount; i++)
				stringBuilder.AppendFormat(", Pointer {0}: {1}/{2}", i, e.GetX(i), e.GetY(i));
			Console.WriteLine(stringBuilder);
		}
*/

        bool MatchesLastMotionEvent(MotionEvent e)
        {
            if (e.Action == lastAction && e.EventTime == lastEventTime && e.PointerCount == lastPointerCount)
                return true;
            lastAction = e.Action;
            lastEventTime = e.EventTime;
            lastPointerCount = e.PointerCount;
            return false;
        }
    }
}
