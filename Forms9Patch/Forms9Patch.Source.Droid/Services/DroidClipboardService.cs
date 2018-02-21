using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Webkit;

[assembly: Dependency(typeof(Forms9Patch.Droid.DroidClipboardService))]
namespace Forms9Patch.Droid
{
    public class DroidClipboardService : Forms9Patch.IClipboardService
    {
        ClipboardManager _clipboardManager;
        ClipboardManager Clipboard
        {
            get
            {
                _clipboardManager = _clipboardManager ?? (ClipboardManager)Settings.Activity.GetSystemService(Context.ClipboardService);
                return _clipboardManager;
            }
        }

        public ClipboardData Data
        {
            get
            {
                if (!Clipboard.HasPrimaryClip)
                    return null;
                var result = new ClipboardData();
                var description = Clipboard.PrimaryClipDescription;
                var clipData = Clipboard.PrimaryClip;

                result.Description = description.Label;

                for (int i = 0; i < clipData.ItemCount; i++)
                {
                    var item = clipData.GetItemAt(i);
                    if (!string.IsNullOrEmpty(item.HtmlText))
                        result.HtmlText = item.HtmlText;
                    if (!string.IsNullOrEmpty(item.Text))
                        result.PlainText = item.Text;

                }
                return result;
            }
            set
            {
                if (value == null)
                    return;
                ClipData clipData = null;
                if (string.IsNullOrEmpty(value.HtmlText))
                    clipData = ClipData.NewPlainText(value.Description, value.PlainText);
                else
                    clipData = ClipData.NewHtmlText(value.Description, value.PlainText, value.HtmlText);
                Clipboard.PrimaryClip = clipData;
            }
        }
    }
}
