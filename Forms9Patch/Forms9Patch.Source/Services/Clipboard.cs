using System;

namespace Forms9Patch
{
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

        public static ClipboardEntry Entry
        {
            get => Service.Entry;

            set => Service.Entry = value;
        }

    }
}