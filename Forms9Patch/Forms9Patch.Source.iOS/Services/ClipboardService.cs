using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;

[assembly: Dependency(typeof(Forms9Patch.iOS.ClipboardService))]
namespace Forms9Patch.iOS
{
    public class ClipboardService : Forms9Patch.IClipboardService
    {
        public ClipboardEntry Entry
        {
            get
            {
                var items = UIPasteboard.General.Items[0]; //UIPasteboard.General.GetDictionaryOfValuesFromKeys(new NSString[] { new NSString("public.html") });
                var plainText = items["public.utf8-plain-text"] as NSString;
                var htmlText = items["public.html"] as NSString;
                var result = new ClipboardEntry
                {
                    PlainText = plainText,
                    HtmlText = htmlText,
                };
                return result;
            }
            set
            {
                NSMutableDictionary<NSString, NSObject> items = new NSMutableDictionary<NSString, NSObject>();
                if (!string.IsNullOrEmpty(value.PlainText))
                    items.Add(new NSString("public.utf8-plain-text"), new NSString(value.PlainText));
                if (!string.IsNullOrEmpty(value.HtmlText))
                    items.Add(new NSString("public.html"), new NSString(value.HtmlText));
                UIPasteboard.General.Items = new NSDictionary[] { items };
            }
        }
    }
}