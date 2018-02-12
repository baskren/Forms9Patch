using System;

namespace FormsGestures
{
	/// <summary>
	/// FormsGestures Long press event arguments.
	/// </summary>
	public class LongPressEventArgs : TapEventArgs {
		
        /// <summary>
        /// Duration of long press
        /// </summary>
		public virtual long Duration { get; protected set; }

        /// <summary>
        /// constructs new LongPressEnventArgs
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newListener"></param>
		public LongPressEventArgs(LongPressEventArgs source=null, Listener newListener=null) : base(source, newListener) {
			if (source!=null)
				Duration = source.Duration;
		}

        public void ValueFrom(LongPressEventArgs source)
        {
            base.ValueFrom(source);
            Duration = source.Duration;
        }
	}
}
