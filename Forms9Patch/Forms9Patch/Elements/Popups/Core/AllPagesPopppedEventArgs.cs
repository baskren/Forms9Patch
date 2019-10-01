using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Forms9Patch.Elements.Popups.Core
{
    /// <summary>
    /// Event arguments when all popups are popped
    /// </summary>
    public class AllPagesPoppedEventArgs : EventArgs
    {
        /// <summary>
        /// Popped popups
        /// </summary>
        public IEnumerable<Page> PoppedPages { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="poppedPages"></param>
        public AllPagesPoppedEventArgs(IEnumerable<Page> poppedPages)
            => PoppedPages = poppedPages ?? throw new ArgumentNullException(nameof(poppedPages));
    }
}