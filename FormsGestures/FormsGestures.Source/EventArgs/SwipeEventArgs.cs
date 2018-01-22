
namespace FormsGestures
{
	/// <summary>
	/// FormsGestures Swipe event arguments.
	/// </summary>
	public class SwipeEventArgs : BaseGestureEventArgs {
		
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
		public SwipeEventArgs(SwipeEventArgs source=null, Listener newListener=null) : base(source, newListener) {
			if (source != null) {
				Direction = source.Direction;
				VelocityX = source.VelocityX;
				VelocityY = source.VelocityY;

			}
		}

	}
}
