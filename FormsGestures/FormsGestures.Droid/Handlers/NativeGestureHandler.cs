using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Views;
using P42.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace FormsGestures.Droid
{
    class NativeGestureHandler : Behavior<VisualElement>, IDisposable
    {
        //bool _debugEvents;

        static readonly BindableProperty GestureHandlerProperty = BindableProperty.Create("GestureHandler", typeof(NativeGestureHandler), typeof(NativeGestureHandler), null);

        //global::Android.Views.View _view;
        //Java.Lang.Ref.WeakReference _weakReferenceView;

        //NativeGestureListener nativeListener;

        //NativeGestureDetector nativeDetector;

        //MotionEventActions lastAction = MotionEventActions.Up;

        //long lastEventTime;

        //int lastPointerCount;

        // When we setup a NativeGestureHandler (NGH) for an element, there is a pretty good chance
        // that children of this element may be blocking touches.  To work around this, have to setup
        // NGH for these children as well.  If a touch is detected on a child, it will then first try 
        // to notify any gesture handlers directly listening to it.  If none of those handles the touch,
        // then we will pass the touch event along to its parent element.


        /// <summary>
        /// Gets or sets the element for which we want to detect gestures.
        /// </summary>
        /// <value>The element.</value>
        internal VisualElement Element { get; set; }

        /// <summary>
        /// All the FG.Listener elements (should be Listeners from parent VisualElements) that are listening to gestures on this _element
        /// </summary>
        //List<Listener> _listeners;
        internal Listener Listener;


        /// <summary>
        /// The renderer for _element
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "We don't own Renderer")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0069:Disposable fields should be disposed", Justification = "We don't own Renderer")]
        internal IVisualElementRenderer Renderer = null;

        /// <summary>
        /// Any children of the _element (if it's a Xamarin.Forms.Layout)
        /// </summary>
        //List<Xamarin.Forms.VisualElement> _children = new List<Xamarin.Forms.VisualElement>();


        OnTouchListener _onTouchListener;

        static int instances;
        readonly int _id;


        #region Activate/Deactivate NativeGestureDetector
        static internal NativeGestureHandler ActivateInstanceForListener(Listener listener)
        {
            var gestureHandler = ActivateInstanceForElement(listener.Element);
            gestureHandler.Listener = listener;
            return gestureHandler;
        }

        internal static NativeGestureHandler ActivateInstanceForElement(VisualElement element)
        {
            var result = (NativeGestureHandler)element.GetValue(GestureHandlerProperty) ?? new NativeGestureHandler(element);
            if (element is Xamarin.Forms.Layout layout)
            {
                layout.ChildAdded += result.OnChildAdded;
                layout.ChildRemoved += result.OnChildRemoved;
            }
            return result;
        }

        static void DeactivateInstanceForElement(VisualElement element, bool force = false)
        {
            if (InstanceForElement(element) is NativeGestureHandler ngh && (ngh.Listener == null || force))
                ngh.Deactivate(force);
        }

        bool _deactivated;
        void Deactivate(bool force = false)
        {
            if (_deactivated)
                return;
            _onTouchListener?.Dispose();
            _onTouchListener = null;
            if (Renderer?.View != null)
                Renderer.View.Touch -= View_Touch;
            _deactivated = true;
            DeactivateInstancesForChildren(force);
            DisconnectRenderer();
            Element.Behaviors.Remove(this);
            Element.SetValue(GestureHandlerProperty, null);
            Element = null;
            Listener = null;

        }
        #endregion


        #region Get/Remove existing NativeGestureDetector
        internal static NativeGestureHandler InstanceForElement(Xamarin.Forms.Element element)
        {
            var result = (NativeGestureHandler)element.GetValue(GestureHandlerProperty);
            return result;
        }

        #endregion


        #region Constructor / Disposer
        NativeGestureHandler(VisualElement element)
        {
            P42.Utils.Debug.AddToCensus(this);

            _id = instances++;
            Element = element;
            Element.Behaviors.Add(this);
            ActivateInstancesForChildren();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                Deactivate();

                P42.Utils.Debug.RemoveFromCensus(this);
            }
        }
        #endregion


        #region Connecting Parent to Children
        void ActivateInstancesForChildren()
        {
            List<Xamarin.Forms.Element> children = new List<Element>(Element.LogicalChildren);
            foreach (var child in children)
                if (child is Xamarin.Forms.VisualElement view)
                    ActivateInstanceForElement(view);
        }

        void DeactivateInstancesForChildren(bool force = false)
        {
            List<Xamarin.Forms.Element> children = new List<Element>(Element.LogicalChildren);
            foreach (var child in children)
                if (child is Xamarin.Forms.VisualElement view)
                    DeactivateInstanceForElement(view, force);
        }

        void OnChildAdded(object parentElement, ElementEventArgs e)
        {
            if (e.Element is Xamarin.Forms.VisualElement child)
                ActivateInstanceForElement(child);
        }

        void OnChildRemoved(object parentElement, ElementEventArgs e)
        {
            if (e.Element is Xamarin.Forms.VisualElement child)
                DeactivateInstanceForElement(child);
        }
        #endregion


        #region Add/Remove Android Gesture Recognizers
        void RendererReset()
        {
            var currentRenderer = Platform.GetRenderer(Element);
            if (currentRenderer != Renderer)
            {
                if (Renderer != null)
                    DisconnectRenderer();
                Renderer = currentRenderer;
                if (Renderer != null)
                {
                    //var view = renderer.GetPropertyValue("Control") as Android.Views.View ?? renderer.ViewGroup;
                    //var view = currentRenderer.GetPropertyValue("Control") as Android.Views.View ?? currentRenderer.View;
                    // will be needed for 2.3.5
                    //var _view = renderer.View;
                    if (Renderer.View == null)
                        //{
                        //System.Diagnostics.Debug.WriteLine("");
                        return;
                    //}
                    //ResetGestureRecognizers(_renderer.View);
                    //nativeListener = new NativeGestureListener(_renderer.View, Element);
                    //nativeDetector = new NativeGestureDetector(Droid.Settings.Context, nativeListener);


                    // approaches:
                    // _renderer.View.Touch += OnTouchEventHandler
                    // _renderer.View.TouchDelegate = OnTouchDelegate;
                    //  _renderer.View.SetOnTouchListener(Android.Views.View.IOnTouchListener l);

                    //var t = _renderer.View.TouchDelegate;  // null
                    // DONT FORGET DISPOSE
                    //var l = new OnTouchListener(this);

                    Renderer.View.SetOnTouchListener(new OnTouchListener(this));
                    //_onTouchListener = _onTouchListener ?? new OnTouchListener(this);
                    //Renderer.View.Touch += View_Touch;


                    //_renderer.View.Touch += HandleTouch;
                    //System.Diagnostics.Debug.WriteLine("NativeGestureHandler.RendererReset() _renderer.View.Touch += HandleTouch Element[" + Element + "]");
                }
            }
        }

        private void View_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            var action = e.Event.Action;
            if (action != MotionEventActions.Down
                && action != MotionEventActions.Up)
                System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + " Event=[" + e.Event + "]");
            //if (sender is Android.Views.View view)
            //    _onTouchListener?.OnTouch(view, e.Event);
        }

        internal void DisconnectRenderer()
        {
            /*
            nativeDetector?.Dispose();
            nativeDetector = null;
            nativeListener?.Dispose();
            nativeListener = null;
            */
            if (Renderer?.View is Android.Views.View view && !(Renderer?.View is ScrollViewRenderer))
            {
                try
                {

                    //view.Touch -= HandleTouch;
                    view.Touch -= View_Touch;
                    //view.SetOnTouchListener(null);
                }
#pragma warning disable 0168
                catch (ArgumentException e)
#pragma warning restore 0168
                {
                    string s = "You got this Exception because FormsGestures doesn't want to leak nor have gestures go to the wrong listeners.  Ignore it.  It's really ok.";
                    Console.WriteLine(s);
                }
#pragma warning disable 0168
                catch (ObjectDisposedException e)
#pragma warning restore 0168
                {
                    string s = "You got this Exception because FormsGestures doesn't want to leak nor have gestures go to the wrong listeners.  Ignore it.  It's really ok.";
                    Console.WriteLine(s);
                }
            }

            Renderer = null;

        }

        #endregion


        #region Xamarin.Behavior Attach / Detach
        protected override void OnAttachedTo(VisualElement element)
        {
            element.SetValue(GestureHandlerProperty, this);
            element.PropertyChanged += OnElementPropertyChanged;
            RendererReset();
            base.OnAttachedTo(element);
        }

        protected override void OnDetachingFrom(VisualElement element)
        {
            // arguably, none of this is necessary since Element owns everything.
            //RendererReset();
            Deactivate();
            element.PropertyChanged -= OnElementPropertyChanged;
            base.OnDetachingFrom(element);
        }

        #endregion


        void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Renderer")
                RendererReset();
        }



        /*
        void HandleTouch(object sender, global::Android.Views.View.TouchEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("NativeGestureHandler.HandleTouch(" + sender + "," + e.Event.Action + ")");
            var handled = HandleMotionEvent(e.Event);
            if (e.Event.Action == MotionEventActions.Down)
            {
                //e.Handled = true;
                //e.Handled = handled;
                //XamarinForms_2_4_WorkAround();
            }
            //System.Diagnostics.Debug.WriteLine("NativeGestureHandler.HandleTouch(" + sender + "," + e.Event.Action + ") e.Handled = " + e.Handled);
        }

        void XamarinForms_2_4_WorkAround()
        {
            Android.Views.View renderer = _renderer?.View;
            //if (view == null)
            //    view = _weakReferenceView?.Get();
            if (renderer is FormsViewGroup formsViewGroup)
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
                        //var fieldInfo = P42.Utils.ReflectionExtensions.GetFieldInfo(objType, fieldName);
                        var fieldInfo = P42.Utils.ReflectionExtensions.GetFieldInfo(parentType, fieldName);
                        if (fieldInfo != null) // && parentType.GetField(fieldName) != null)  THIS CODE CAUSES the Listener for PoupBase._pageOverlay to not work.
                            try
                            {
                                //var _notReallyHandled = (bool)fieldInfo.GetValue(parent);
                                fieldInfo?.SetValue(parent, false);
                            }
                            catch (System.ArgumentException e)
                            {
                                // this seems to be happening occasionally.  Need to get to the bottom of it!
                                System.Diagnostics.Debug.WriteLine("IGNORE THIS EXCEPTION.  EVERYTHING IS OK. _element=[" + Element + "] _element.Parent=[" + Element?.Parent + "] view=[" + renderer + "] parent=[" + parent + "] e=[" + e.Message + "]");
                            }
                    }
                }
            }
        }


        bool lastEventHandled;
        bool HandleMotionEvent(MotionEvent e)
        {
            //if (_debugEvents) System.Diagnostics.Debug.WriteLine("[{0}.{1}] [{2}] [{3}]", GetType().Name, Debug.CurrentMethod(), _id, _element);
            //System.Diagnostics.Debug.WriteLine("NativeGestureHandler.HandleMotionEvent: " + e.Action);
            //ShareMotionEvent (_element, e, _element);
            if (!Element.IsVisible)
                return false;
            if (MatchesLastMotionEvent(e))
                return false;
            lastEventHandled &= e.Action != MotionEventActions.Down;


            // the following has me confused.  It is preventing an OnUp event from being delivered to NativeGestureDetector
            //if (nativeDetector != null)
            //    lastEventHandled = lastEventHandled || nativeDetector.OnTouchEvent(e);

            if (nativeDetector != null)
                lastEventHandled = nativeDetector.OnTouchEvent(e) || lastEventHandled;

            object scrollEnabled = Element.GetPropertyValue("ScrollEnabled");
            if (scrollEnabled == null || ((bool)scrollEnabled) || e.Action != MotionEventActions.Move)
            {
                var view = _renderer?.View;
                //var view = (Android.Views.View)_weakReferenceView?.Get();
                if (view != null)
                {
                    var renderer = Platform.GetRenderer(Element);
                    //var currentView = (renderer?.GetPropertyValue("Control") as Android.Views.View) ?? renderer?.ViewGroup;
                    var currentView = (renderer?.GetPropertyValue("Control") as Android.Views.View) ?? renderer?.View;
                    if (currentView != null && view == currentView)
                        view.OnTouchEvent(e);
                }
            }
            return lastEventHandled;  // we want to be sure we get the updates to this element's events
        }


        bool MatchesLastMotionEvent(MotionEvent e)
        {
            if (e.Action == lastAction && e.EventTime == lastEventTime && e.PointerCount == lastPointerCount)
                return true;
            lastAction = e.Action;
            lastEventTime = e.EventTime;
            lastPointerCount = e.PointerCount;
            return false;
        }
        */
    }
}
