using System;
using Xamarin.Forms;

namespace FormsGestures
{
    /// <summary>
    /// FormsGestures Base gesture event arguments.
    /// </summary>
    public class BaseGestureEventArgs : EventArgs
    {
        /// <summary>
        /// Name of event that has been triggered
        /// </summary>
        public string Event { get; internal set; }

        Point _center;

        /// <summary>
        /// gets/sets if the gesture was handled
        /// </summary>
		public bool Handled { get; set; }


        /// <summary>
        /// gets/sets Listener
        /// </summary>
		public virtual Listener Listener { get; set; }

        /// <summary>
        /// has the touch been cancelled (and thus this is the last of the touch sequence)
        /// </summary>
		public virtual bool Cancelled { get; internal set; }

        /// <summary>
        /// current position of VisualElement in Window coordinates
        /// </summary>
		public virtual Rectangle ElementPosition { get; protected set; }

        /// <summary>
        /// Set of touch points, in VisualElement coordinates, that make up this event
        /// </summary>
		public virtual Point[] ElementTouches { get; internal set; }

        /// <summary>
        /// Set of touch points, in Window coordinates, that make up this touch event
        /// </summary>
        public virtual Point[] WindowTouches { get; internal set; }

        /// <summary>
        /// Number of touches in touch event
        /// </summary>
		public virtual int NumberOfTouches => WindowTouches.Length;

        /// <summary>
        /// center of a set of touch points
        /// </summary>
        public virtual Point Center(Point[] touches)
        {
            var num = touches.Length;
            var num2 = 0.0;
            var num3 = 0.0;
            for (int i = 0; i < num; i++)
            {
                num2 += touches[i].X;
                num3 += touches[i].Y;
            }
            return new Point(num2 / num, num3 / num);
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newListener"></param>
		public BaseGestureEventArgs(BaseGestureEventArgs source = null, Listener newListener = null)
        {
            if (source != null)
            {
                Listener = newListener ?? source.Listener;
                Cancelled = source.Cancelled;
                if (Listener != null)
                {
                    WindowTouches = new Point[source.WindowTouches.Length];
                    ElementTouches = new Point[source.WindowTouches.Length];
                    for (int i = 0; i < source.WindowTouches.Length; i++)
                    {
                        WindowTouches[i] = source.WindowTouches[i];
                        ElementTouches[i] = Listener.Element.PointInElementCoord(source.ElementTouches[i], newListener.Element);
                    }
                    ElementPosition = VisualElementExtensions.CoordTransform(Listener.Element, source.ElementPosition, newListener.Element);
                }
                else
                {
                    WindowTouches = (Point[])source.WindowTouches.Clone();
                    ElementTouches = (Point[])source.ElementTouches.Clone();
                    ElementPosition = source.ElementPosition;
                }
            }
        }

        /// <summary>
        /// Equal test
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return obj is BaseGestureEventArgs baseGestureEventArgs && Equals(baseGestureEventArgs);
        }

        /// <summary>
        /// Equal test
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
		public bool Equals(BaseGestureEventArgs other)
        {
            if (other == null)
                return false;
            if (WindowTouches == null && other.WindowTouches == null)
                return true;
            if (WindowTouches.Length != other.WindowTouches.Length)
                return false;
            for (int i = 0; i < WindowTouches.Length; i++)
                if (!Equals(WindowTouches[i], other?.WindowTouches[i]))
                    return false;
            return true;
        }

        /// <summary>
        /// HasCode getter
        /// </summary>
        /// <returns></returns>
		public override int GetHashCode()
        {
            return WindowTouches.GetHashCode();
        }

        /// <summary>
        /// Updates properties of this instance with values from an other instance
        /// </summary>
        /// <param name="other"></param>
        public void ValueFrom(BaseGestureEventArgs other)
        {
            Handled = other.Handled;
            _center = other._center;
            Listener = other.Listener;
            Cancelled = other.Cancelled;
            ElementPosition = other.ElementPosition;
            ElementTouches = other.ElementTouches;
            WindowTouches = other.WindowTouches;
        }

        /// <summary>
        /// Tests if a point, in Window coordinates, is within the bounds of the view
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool Contains(Point p)
            => ElementPosition.Contains(p);

        /// <summary>
        /// Tests if the TouchCenter is within the bounds of the view
        /// </summary>
        public bool IsTouchCenterInView
            => Contains(Center(WindowTouches));
    }
}
