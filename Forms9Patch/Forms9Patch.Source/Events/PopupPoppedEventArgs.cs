using System;

namespace Forms9Patch
{
    public class PopupPoppedEventArgs : EventArgs
    {
        public PopupPoppedCause Cause { get; private set; }

        public object Trigger { get; private set; }

        public PopupPoppedEventArgs(PopupPoppedCause cause, object trigger)
        {
            Cause = cause;
            Trigger = trigger;
        }
    }
}