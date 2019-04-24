using System;

namespace FormsGestures
{
	/// <summary>
	/// FormsGestures Down up event arguments.
	/// </summary>
	public class DownUpEventArgs : BaseGestureEventArgs {
		
        /// <summary>
        /// enumerates the touches that triggered the gesture
        /// </summary>
		public virtual int[] TriggeringTouches { get; protected set; }

        /// <summary>
        /// creates new DownUpEventArgs
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newListener"></param>
		public DownUpEventArgs(DownUpEventArgs source=null, Listener newListener=null) : base(source, newListener) {
			if (source != null)
				TriggeringTouches = (int[])source.TriggeringTouches.Clone ();
		}

        /// <summary>
        /// Updates properties from the values from another instance
        /// </summary>
        /// <param name="source"></param>
        public void ValueFrom(DownUpEventArgs source)
        {
            base.ValueFrom(source);
            TriggeringTouches = source.TriggeringTouches;
        }
	}
}
