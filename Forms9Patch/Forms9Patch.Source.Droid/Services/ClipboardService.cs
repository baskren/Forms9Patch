using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(Forms9Patch.Droid.ClipboardService))]
namespace Forms9Patch.Droid
{
    public class ClipboardService : Forms9Patch.IClipboardService
    {
        public string Value
        {
            get
            {
                return LaunchActivity.AndroidClipboardManager.Text;
            }
            set
            {
                LaunchActivity.AndroidClipboardManager.Text = value;
            }
        }
    }
}