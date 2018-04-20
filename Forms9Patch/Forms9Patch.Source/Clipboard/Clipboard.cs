using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch.Clipboard class
    /// </summary>
    public static class Clipboard
    {
        static Clipboard()
        {
            Settings.ConfirmInitialization();
        }

        static IClipboardService _service;
        static IClipboardService Service
        {
            get
            {
                _service = _service ?? Xamarin.Forms.DependencyService.Get<IClipboardService>();
                return _service;
            }
        }

        /// <summary>
        /// Gets/Sets the current Entry on the clipboard
        /// </summary>
        public static ClipboardEntry Entry
        {
            get => Service.Entry ?? new ClipboardEntry();

            set => Service.Entry = value;
        }

        /// <summary>
        /// Turns of caching latest clipboard entry (for speed, of course).  Mostly for test purposes.
        /// </summary>
        /// <value><c>true</c> if the latest clipboard entry is cached; otherwise, <c>false</c>.</value>
        public static bool EntryCaching
        {
            get => Service.EntryCaching;
            set => Service.EntryCaching = value;
        }

        /*
        /// <summary>
        /// Turns off caching of the Type of each ClipboardEntryItem set by application.  
        /// This will, in some cases, result in different ClipboardEntryItems having types being returned by Clipboard than was set.
        /// This feature (in combination with EntryCaching) is meant to allow developers to test/debug inter-app copy and paste (without writing a different app).
        /// </summary>
        /// <value><c>true</c> if entry item types are cached; otherwise, <c>false</c>.</value>
        public static bool EntryItemTypeCaching
        {
            get => Service.EntryItemTypeCaching;
            set => Service.EntryItemTypeCaching = value;
        }
        */

        /// <summary>
        /// Event fired when the content on the clipboard has changed
        /// </summary>
        public static event EventHandler ContentChanged;

        internal static void OnContentChanged(object sender, EventArgs args)
        {
            ContentChanged?.Invoke(null, args);
        }



    }
}