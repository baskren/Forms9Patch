using System;

namespace Forms9Patch
{
    public class Clipboard
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

        public static string Value
        {
            get => Service.Value;
            set => Service.Value = value;   
        }
    }
}