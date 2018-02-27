using System;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch.Clipboard class
    /// </summary>
    public static class Clipboard
    {
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
            get => Service.Entry;

            set => Service.Entry = value;
        }

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