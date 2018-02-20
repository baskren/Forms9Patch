using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: Dependency(typeof(Forms9Patch.iOS.ClipboardService))]
namespace Forms9Patch.iOS
{
    public class ClipboardService : Forms9Patch.IClipboardService
    {
        public string Value
        {
            get
            {
                return UIPasteboard.General.String;
            }
            set
            {
                UIPasteboard.General.String = value;
            }
        }
    }
}