using System;
using System.Collections;
using System.Collections.Generic;

namespace Forms9Patch
{
    /// <summary>
    /// Interface to allow Forms9Patch events to be shared with an analytics provider used by the host application
    /// </summary>
    public class Analytics
    {
        /// <summary>
        /// Delegate definition for publishing a Forms9Patch analytics event to an application's analytics provider
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="properties"></param>
        public delegate void TrackEventDelegate(string eventName, IDictionary<string, string> properties);

        /// <summary>
        /// Delegate definition for publishing a Forms9Patch exception to an application's analytics provider
        /// </summary>
        /// <param name="e"></param>
        /// <param name="properties"></param>
        public delegate void TrackExceptionDelegate(Exception e, IDictionary<string, string> properties = null);

        /// <summary>
        /// Property for the TrackEvent delegate.  Used for publishing a Forms9Patch analytics event to an application's analytics provider
        /// </summary>
        public static TrackEventDelegate TrackEvent { get; set; }

        /// <summary>
        /// Property for the TrackException delegate.  Used for publishing a Forms9Patch exception to an application's analytics provider
        /// </summary>
        public static TrackExceptionDelegate TrackException { get; set; }

    }

}