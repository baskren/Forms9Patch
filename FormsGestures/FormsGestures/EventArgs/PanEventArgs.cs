using Xamarin.Forms;

namespace FormsGestures
{
    /// <summary>
    /// FormsGestures Pan event arguments.
    /// </summary>
    public class PanEventArgs : BaseGestureEventArgs
    {

        /// <summary>
        /// distance since last sample of pan motion
        /// </summary>
        public virtual Point DeltaDistance { get; protected set; }

        /// <summary>
        /// total distance of pan motion
        /// </summary>
		public virtual Point TotalDistance { get; protected set; }

        /// <summary>
        /// currently velocity of pan motion
        /// </summary>
		public virtual Point Velocity { get; protected set; }

        /// <summary>
        /// constructs new PanEventArgs
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newListener"></param>
		public PanEventArgs(PanEventArgs source = null, Listener newListener = null) : base(source, newListener)
        {
            if (source != null)
            {
                DeltaDistance = source.DeltaDistance;
                TotalDistance = source.TotalDistance;
                Velocity = source.Velocity;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:FormsGestures.PanEventArgs"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:FormsGestures.PanEventArgs"/>.</returns>
        public override string ToString()
        {
            return "[d:" + DeltaDistance + " t:" + TotalDistance + " v:" + Velocity + "]";
        }


        /// <summary>
        /// calculates the distance traversed since last sample
        /// </summary>
        /// <param name="previous"></param>
		protected void CalculateDistances(BaseGestureEventArgs previous) //, Point locationAtStart)
        {
            if (previous == null)
            {
                DeltaDistance = new Point(0.0, 0.0);
                TotalDistance = new Point(0.0, 0.0);
                return;
            }
            DeltaDistance = Center(WindowTouches).Subtract(Center(previous.WindowTouches));
            if (previous is PanEventArgs previousPan)
            {
                if (WindowTouches.Length != previous.WindowTouches.Length)
                {
                    DeltaDistance = new Point(0.0, 0.0);
                    TotalDistance = previousPan.TotalDistance;
                    return;
                }
                TotalDistance = previousPan.TotalDistance.Add(DeltaDistance);
            }
            else
                TotalDistance = DeltaDistance;
        }

        internal PanEventArgs Diff(PanEventArgs lastArgs)
        {
            return new PanEventArgs
            {
                Cancelled = Cancelled,
                Handled = Handled,
                ElementPosition = ElementPosition,
                ElementTouches = ElementTouches,
                WindowTouches = WindowTouches,
                Listener = Listener,
                Velocity = Velocity,
                TotalDistance = TotalDistance,
                DeltaDistance = TotalDistance.Subtract(lastArgs.TotalDistance)
            };
        }
        /// <summary>
        /// Equality test
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return obj is PanEventArgs panEventArgs && Equals(panEventArgs);
        }

        /// <summary>
        /// Equality test
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
		public bool Equals(PanEventArgs other)
        {
            return other != null && DeltaDistance.Equals(other.DeltaDistance) && TotalDistance.Equals(other.TotalDistance) && Velocity.Equals(other.Velocity) && base.Equals(other);
        }

        /// <summary>
        /// returns hash code
        /// </summary>
        /// <returns></returns>
		public override int GetHashCode()
        {
            //return base.GetHashCode() ^ DeltaDistance.GetHashCode() ^ TotalDistance.GetHashCode() ^ Velocity.GetHashCode();
            var hash = 13;
            hash = hash * 23 + base.GetHashCode();
            hash = hash * 23 + DeltaDistance.GetHashCode();
            hash = hash * 23 + TotalDistance.GetHashCode();
            hash = hash * 23 + Velocity.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Updates properties from the values from another instance
        /// </summary>
        /// <param name="source"></param>
        public void ValueFrom(PanEventArgs source)
        {
            base.ValueFrom(source);
            DeltaDistance = source.DeltaDistance;
            TotalDistance = source.TotalDistance;
            Velocity = source.Velocity;
        }
    }
}
