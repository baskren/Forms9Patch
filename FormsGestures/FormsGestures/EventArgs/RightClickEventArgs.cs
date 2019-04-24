using System;

namespace FormsGestures
{
    /// <summary>
    /// Arguments for RightClick gesture event
    /// </summary>
    public class RightClickEventArgs : BaseGestureEventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="listener"></param>
        public RightClickEventArgs(RightClickEventArgs source=null, Listener listener=null) : base(source, listener) { }
    }
}