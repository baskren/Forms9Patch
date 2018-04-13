
namespace FormsGestures
{
    /// <summary>
    /// FormsGestures Swipe event arguments.
    /// </summary>
    public class SwipeEventArgs : BaseGestureEventArgs
    {

        /// <summary>
        /// Direction of swipe
        /// </summary>
        public virtual Direction Direction { get; protected set; }

        /// <summary>
        /// current x velocity
        /// </summary>
		public double VelocityX { get; internal set; }

        /// <summary>
        /// current y velocity
        /// </summary>
		public double VelocityY { get; internal set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newListener"></param>
		public SwipeEventArgs(SwipeEventArgs source = null, Listener newListener = null) : base(source, newListener)
        {
            if (source != null)
            {
                Direction = source.Direction;
                VelocityX = source.VelocityX;
                VelocityY = source.VelocityY;

            }
        }

        /// <summary>
        /// Updates properties from the values from another instance
        /// </summary>
        /// <param name="source"></param>
        public void ValueFrom(SwipeEventArgs source)
        {
            base.ValueFrom(source);
            Direction = source.Direction;
            VelocityX = source.VelocityX;
            VelocityY = source.VelocityY;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:FormsGestures.SwipeEventArgs"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:FormsGestures.SwipeEventArgs"/>.</returns>
        public override string ToString()
        {
            return "[dir:" + Direction + "; VelX:" + VelocityX + "; VelY:" + VelocityY + ";]";
        }
    }
}
