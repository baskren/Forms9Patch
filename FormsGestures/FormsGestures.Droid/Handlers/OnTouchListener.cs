using System;
using Xamarin.Forms;
using Android.Views;
using P42.Utils;
using Xamarin.Forms.Platform.Android;
using System.Globalization;

namespace FormsGestures.Droid
{
    public class OnTouchListener : Java.Lang.Object, Android.Views.View.IOnTouchListener, IDisposable
    {
        #region Fields
        NativeGestureListener _nativeListener;
        NativeGestureDetector _nativeDetector;
        NativeGestureHandler _nativeGestureHandler;

        MotionEventActions _lastAction = MotionEventActions.Up;

        long _lastEventTime;
        int _lastPointerCount;
        bool _lastEventHandled;
        #endregion

        internal OnTouchListener(NativeGestureHandler nativeGestureHandler)
        {
            _nativeGestureHandler = nativeGestureHandler;
            _nativeListener = new NativeGestureListener(_nativeGestureHandler.Renderer.View, _nativeGestureHandler.Element);
            _nativeDetector = new NativeGestureDetector(Droid.Settings.Context, _nativeListener);
        }

        public bool OnTouch(Android.Views.View v, MotionEvent e)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + " e:" + e.Action + " Element:[" + _nativeGestureHandler.Element.Id + "] y:" + e.RawY + "  y:" + e.GetY() + " count:" + e.PointerCount);
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + " e:" + e.Action + " " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));

            if (!_nativeGestureHandler.Element.IsVisible)
                return false;
            if (MatchesLastMotionEvent(e))
                return false;
            _lastEventHandled &= e.Action != MotionEventActions.Down;


            // the following has me confused.  It is preventing an OnUp event from being delivered to NativeGestureDetector
            //if (nativeDetector != null)
            //    lastEventHandled = lastEventHandled || nativeDetector.OnTouchEvent(e);

            if (_nativeDetector != null)
                _lastEventHandled = _nativeDetector.OnTouchEvent(e) || _lastEventHandled;

            /*
            object scrollEnabled = _nativeGestureHandler.Element.GetPropertyValue("ScrollEnabled");
            if (scrollEnabled == null || ((bool)scrollEnabled) || e.Action != MotionEventActions.Move)
            {
                //var view = _nativeGestureHandler.Renderer.View;
                //var view = (Android.Views.View)_weakReferenceView?.Get();
                if (_nativeGestureHandler.Renderer?.View is Android.Views.View view)
                {
                    var renderer = Platform.GetRenderer(_nativeGestureHandler.Element);
                    //var currentView = (renderer?.GetPropertyValue("Control") as Android.Views.View) ?? renderer?.ViewGroup;
                    var currentView = (renderer?.GetPropertyValue("Control") as Android.Views.View) ?? renderer?.View;
                    if (currentView != null && view == currentView)
                        view.OnTouchEvent(e);
                }
            }
            */

            if (_nativeGestureHandler.Renderer?.View is Android.Views.View view)
            {
                if (_nativeGestureHandler.Element is Xamarin.Forms.Button)
                {
                    //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": Action:[" + e.Action + "] Element [" + _nativeGestureHandler.Element + "]");
                    var renderer = Platform.GetRenderer(_nativeGestureHandler.Element);
                    //var currentView = (renderer?.GetPropertyValue("Control") as Android.Views.View) ?? renderer?.ViewGroup;
                    var currentView = (renderer?.GetPropertyValue("Control") as Android.Views.View) ?? renderer?.View;
                    if (currentView != null && view == currentView)
                        view.OnTouchEvent(e);
                }


                view.Parent?.RequestDisallowInterceptTouchEvent(true);


                bool handled = false;
                var parent = view.Parent;
                while (parent != null)
                {
                    Android.Views.View scrollable = null;
                    if (parent is Android.Widget.ListView listView)
                        scrollable = listView;
                    else if (parent is Android.Widget.ScrollView scrollView)
                        scrollable = scrollView;
                    if (scrollable != null)
                    {
                        int[] location = new int[2];
                        scrollable.GetLocationOnScreen(location);
                        var x = MotionEvent.Obtain(e.DownTime, e.EventTime, e.Action, e.RawX - location[0], e.RawY - location[1], e.MetaState);
                        handled = scrollable.OnTouchEvent(x);
                        x.Recycle();
                        if (handled)
                            break;
                    }
                    parent = parent.Parent;
                }
            }

            //System.Diagnostics.Debug.WriteLine(GetType() + "\t _lastEventHandled=[" + _lastEventHandled + "]");
            return _lastEventHandled;  // we want to be sure we get the updates to this element's events




            //return _nativeGestureDetector.OnTouchEvent(e);


            /*
            if (e.Action == MotionEventActions.Down)
            {
                // do stuff
                return true;
            }
            if (e.Action == MotionEventActions.Up)
            {
                // do other stuff
                return true;
            }

            return false;
            */
        }

        bool MatchesLastMotionEvent(MotionEvent e)
        {
            if (e.Action == _lastAction && e.EventTime == _lastEventTime && e.PointerCount == _lastPointerCount)
                return true;
            _lastAction = e.Action;
            _lastEventTime = e.EventTime;
            _lastPointerCount = e.PointerCount;
            return false;
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                _nativeDetector?.Dispose();
                _nativeDetector = null;
                _nativeListener?.Dispose();
                _nativeListener = null;
                _nativeGestureHandler = null;

            }
            base.Dispose(disposing);

        }

    }
}

