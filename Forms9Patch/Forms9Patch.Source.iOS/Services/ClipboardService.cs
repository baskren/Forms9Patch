using System;
using Xamarin.Forms;
using UIKit;
using Foundation;
using MobileCoreServices;
using System.Collections.Generic;
using System.Linq;

[assembly: Dependency(typeof(Forms9Patch.iOS.ClipboardService))]
namespace Forms9Patch.iOS
{
    public class ClipboardService : Forms9Patch.IClipboardService
    {
        static internal NSString TypeListUti = new NSString(UTType.CreatePreferredIdentifier(UTType.TagClassMIMEType, "application/f9p-clipboard-typelist", null));

        static ClipboardService()
        {
            UIPasteboard.Notifications.ObserveChanged(OnPasteboardChanged);
            UIPasteboard.Notifications.ObserveRemoved(OnPasteboardChanged);
        }

        static void OnPasteboardChanged(object sender, UIPasteboardChangeEventArgs e)
        {
            Forms9Patch.Clipboard.OnContentChanged(null, EventArgs.Empty);
        }

        #region Entry property
        nint _lastEntryChangeCount = int.MinValue;
        IClipboardEntry _lastEntry = null;
        public IClipboardEntry Entry
        {
            get
            {
                if (!EntryCaching || _lastEntryChangeCount != UIPasteboard.General.ChangeCount)
                    _lastEntry = new ClipboardEntry();
                _lastEntryChangeCount = UIPasteboard.General.ChangeCount;
                return _lastEntry;
            }
            set
            {
                if (value is Forms9Patch.ClipboardEntry entry)
                {
                    var items = new NSMutableDictionary();
                    var images = new List<NSMutableDictionary>();
                    var typeDictionary = new NSMutableDictionary();

                    foreach (var item in entry.MimeHash.Values)
                    {
                        if ((item.MimeType == "image/png" || item.MimeType == "image/jpeg" || item.MimeType == "image/jpg") && item.Value is byte[] byteArray)
                        {
                            var nsData = NSData.FromArray(byteArray);
                            var nsUti = item.ToNsUti();
                            items.Add(nsUti, nsData);
                            typeDictionary.Add(nsUti, new NSString(byteArray.GetType().FullName));
                        }
                        else if (item.MimeType == "text/url" && item.Value is string urlString)
                        {
                            var nsUrl = new NSUrl(urlString);
                            var nsUti = item.ToNsUti();
                            items.Add(nsUti, nsUrl);
                            typeDictionary.Add(nsUti, new NSString(typeof(string).ToString()));
                        }
                        else if (NSDataItem.Create(item) is NSDataItem dataItem)
                        {
                            items.Add(dataItem.NSUti, dataItem.KeyedArchiver);
                            typeDictionary.Add(dataItem.NSUti, new NSString(item.Type.FullName));
                        }
                    }
                    items.Add(TypeListUti, NSKeyedArchiver.ArchivedDataWithRootObject(typeDictionary));
                    var changeCount = UIPasteboard.General.ChangeCount;
                    UIPasteboard.General.Items = new NSDictionary[] { items };

                    _lastEntry = value;
                    _lastEntryChangeCount = UIPasteboard.General.ChangeCount;
                }

            }
        }

        public bool EntryCaching { get; set; } = false;

        #endregion

    }

}