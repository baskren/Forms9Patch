using System;
using Xamarin.Forms;

namespace FormsGestures
{
	/// <summary>
	/// FormsGestures Base gesture event arguments.
	/// </summary>
	public class BaseGestureEventArgs : EventArgs
	{
		bool _handled = false;

		Point _center;

        /// <summary>
        /// gets/sets if the gesture was handled
        /// </summary>
		public bool Handled {
			get { return _handled; }
			set { _handled = value; }
		}


        /// <summary>
        /// gets/sets Listener
        /// </summary>
		public virtual Listener Listener { get; set; }

        /// <summary>
        /// has the touch been cancelled (and thus this is the last of the touch sequence)
        /// </summary>
		public virtual bool Cancelled { get; internal set; }

        /// <summary>
        /// current position of view 
        /// </summary>
		public virtual Rectangle ViewPosition { get; protected set; }

        /// <summary>
        /// all of the touch points that make up this touch event
        /// </summary>
		public virtual Point[] Touches { get; protected set; }

        /// <summary>
        /// Number of touches in touch event
        /// </summary>
		public virtual int NumberOfTouches {
			get { return Touches.Length; }
		}

        /// <summary>
        /// center of touch event
        /// </summary>
		public virtual Point Center {
			get {
				if (_center.IsEmpty) {
					int num = Touches.Length;
					double num2 = 0.0;
					double num3 = 0.0;
					for (int i = 0; i < num; i++) {
						num2 += Touches[i].X;
						num3 += Touches[i].Y;
					}
					_center = new Point(num2 / (double)num, num3 / (double)num);
				}
				return _center;
			}

			protected set { _center = value; }
		}

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newListener"></param>
		public BaseGestureEventArgs(BaseGestureEventArgs source=null, Listener newListener=null) {
			if (source != null) {
				Listener = newListener ?? source.Listener;
				Cancelled = source.Cancelled;
				if (Listener != null) {
					Touches = new Point[source.Touches.Length];
					for (int i = 0; i < source.Touches.Length; i++)
						Touches [i] = source.Listener.Element.ToEleCoord (source.Touches [i], newListener.Element);
					ViewPosition = VisualElementExtensions.CoordTransform (source.Listener.Element, source.ViewPosition, newListener.Element);
				} else {
					Touches = (Point[])source.Touches.Clone ();
					ViewPosition = source.ViewPosition;
				}
			}
		}

        /// <summary>
        /// Equal test
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		public override bool Equals(object obj) {
			if (obj == null)
				return false;
			var baseGestureEventArgs = obj as BaseGestureEventArgs;
			return baseGestureEventArgs != null && Equals(baseGestureEventArgs);
		}

        /// <summary>
        /// Equal test
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
		public bool Equals(BaseGestureEventArgs other) {
			if (other == null)
				return false;
			if (Touches == null && other.Touches == null)
				return true;
			if (Touches.Length != other.Touches.Length)
				return false;
			for (int i = 0; i < Touches.Length; i++)
				if (!Equals(Touches[i],other?.Touches[i]))
					return false;
			return true;
		}

        /// <summary>
        /// HasCode getter
        /// </summary>
        /// <returns></returns>
		public override int GetHashCode() {
			return Touches.GetHashCode();
		}

        /// <summary>
        /// Updates properties of this instance with values from an other instance
        /// </summary>
        /// <param name="other"></param>
        public void ValueFrom(BaseGestureEventArgs other)
        {
            _handled = other._handled;
            _center = other._center;
            Listener = other.Listener;
            Cancelled = other.Cancelled;
            ViewPosition = other.ViewPosition;
            Touches = other.Touches;
        }
	}
}
