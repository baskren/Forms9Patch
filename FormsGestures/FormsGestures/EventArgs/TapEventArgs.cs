
namespace FormsGestures
{
	/// <summary>
	/// FormsGestures Tap event arguments.
	/// </summary>
	public class TapEventArgs : BaseGestureEventArgs {
		
        /// <summary>
        /// number of taps
        /// </summary>
		public virtual int NumberOfTaps { get; protected set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newListener"></param>
		public TapEventArgs(TapEventArgs source=null, Listener newListener=null) : base(source, newListener) {
			if (source != null)
				NumberOfTaps = source.NumberOfTaps;
		}

        /// <summary>
        /// Updates properties from the values from another instance
        /// </summary>
        /// <param name="source"></param>
        public void ValueFrom(TapEventArgs source) 
        {
            base.ValueFrom(source);
            NumberOfTaps = source.NumberOfTaps;
        }
    }
}
