using Android.Views;
using System.Collections.Generic;

namespace FormsGestures.Droid
{
    class DownUpGestureListener
    {
        readonly View _view;

        readonly List<Listener> _listeners;

        //int[] _viewLocationAtOnDown = { 0,0 };


        internal DownUpGestureListener(View view, List<Listener> listeners)
        {
            //_element = element;
            _view = view;
            _listeners = listeners;
        }

        bool HandlesDown
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesDown)
                        return true;
                return false;
            }
        }

        bool HandlesUp
        {
            get
            {
                foreach (var listener in _listeners)
                    if (listener.HandlesUp)
                        return true;
                return false;
            }
        }



        public bool onDown(MotionEvent e)
        {
            //System.Diagnostics.Debug.WriteLine("DownUpGestureListener." + P42.Utils.ReflectionExtensions.CallerMemberName(), " e:" + e);
            //System.Console.WriteLine ("onDown: " + e);
            //bool result = false;
            foreach (var listener in _listeners)
            {
                if (listener.HandlesDown)
                {
                    int[] viewLocation = { 0, 0 };
                    _view?.GetLocationInWindow(viewLocation);
                    DownUpEventArgs args = new AndroidDownUpEventArgs(e, _view, viewLocation);
                    args.Listener = listener;
                    listener.OnDown(args);
                    if (args.Handled)
                        break;
                }
            }
            return false;
        }

        public bool onUp(MotionEvent e)
        {
            //System.Diagnostics.Debug.WriteLine("DownUpGestureListener." + P42.Utils.ReflectionExtensions.CallerMemberName(), " e:" + e);
            //System.Console.WriteLine ("onUP: " + e);
            //bool result = false;
            foreach (var listener in _listeners)
            {
                if (listener.HandlesUp)
                {
                    int[] viewLocation = { 0, 0 };
                    _view?.GetLocationInWindow(viewLocation);
                    DownUpEventArgs args = new AndroidDownUpEventArgs(e, _view, viewLocation);
                    args.Listener = listener;
                    listener.OnUp(args);
                    if (args.Handled)
                        break;
                }
            }
            return false;
        }
    }
}
