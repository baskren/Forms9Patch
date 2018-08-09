using System;
using System.Collections.Generic;
using Forms9Patch;
using UIKit;
using Foundation;
using System.Linq;
using MobileCoreServices;

namespace Forms9Patch.iOS
{
    class ClipboardEntry : IClipboardEntry
    {
        public string Description { get; private set; }

        public string PlainText
        {
            get
            {
                if (_firstPasteboardItem != null)
                {
                    var nsObj = _firstPasteboardItem[UTType.UTF8PlainText] ?? _firstPasteboardItem[UTType.Text] ?? _firstPasteboardItem[UTType.UTF8TabSeparatedText] ?? _firstPasteboardItem[UTType.DelimitedText] ?? _firstPasteboardItem[UTType.CommaSeparatedText] ?? _firstPasteboardItem[UTType.TabSeparatedText];
                    return nsObj as NSString;
                }
                return null;
            }
        }

        public string HtmlText
        {
            get
            {
                if (_firstPasteboardItem != null)
                    return _firstPasteboardItem[UTType.HTML] as NSString;
                return null;
            }
        }

        List<IMimeItem> _items;
        public List<IMimeItem> Items
        {
            get
            {
                if (_items != null)
                    return _items;
                if (UIPasteboard.General?.Items == null || UIPasteboard.General.Items.Length < 1)
                    return _items = new List<IMimeItem>();

                _items = new List<IMimeItem>();
                foreach (var item in UIPasteboard.General.Items)
                {
                    foreach (var kvp in item)
                    {
                        var returnMimeItem = LazyMimeItem.Parse(kvp);
                        if (returnMimeItem != null)
                            _items.Add(returnMimeItem);
                    }
                }
                return _items;
            }
        }

        /*
        NSDictionary _typeList;
        NSDictionary TypeList
        {
            get
            {
                if (_typeList != null)
                    return _typeList;
                if (_firstPasteboardItem != null && _firstPasteboardItem[ClipboardService.TypeListUti] is NSData archive)
                {
                    var unkeyedArchiave = NSKeyedUnarchiver.UnarchiveObject(archive);
                    _typeList = unkeyedArchiave as NSDictionary;
                }
                return _typeList;
            }
        }
        */

        /*
        List<string> _mimeTypes;
        public List<string> MimeTypes
        {
            get
            {
                if (_mimeTypes != null)
                    return _mimeTypes;
                _mimeTypes = new List<string>();
                if (_firstPasteboardItem == null)
                    return _mimeTypes;
                if (TypeList is NSDictionary typeList)
                {
                    foreach (var kvp in typeList)
                        _mimeTypes.Add(kvp.ToMime());
                }
                else
                {
                    foreach (var key in _firstPasteboardItem.Keys)
                    {
                        if (key is NSString nsUti)
                        {
                            if (nsUti == UTType.URL || nsUti == UTType.FileURL)
                                _mimeTypes.Add("text/url");
                            else if (nsUti == UTType.UTF8PlainText)
                                _mimeTypes.Add("text/plain");
                            else
                            {
                                string mimeType = null;
                                mimeType = UTType.GetPreferredTag(nsUti, UTType.TagClassMIMEType);
                                if (mimeType != null)
                                    _mimeTypes.Add(mimeType);
                            }
                        }
                    }
                }
                return _mimeTypes;
            }
        }
        */

        /*
        public IMimeItem<T> GetItem<T>(string mimeType)
        {
            if (_firstPasteboardItem == null || mimeType == null)
                return null;
            var untypedItem = GetUntypedItem(mimeType);
            return new ReturnMimeItem<T>(untypedItem);
        }

        ReturnMimeItem GetUntypedItem(string mimeType)
        {
            if (_firstPasteboardItem == null || mimeType == null)
                return null;
            if (mimeType == "text/plain")
                return new ReturnMimeItem { MimeType = "text/plain", Type = typeof(string), Value = PlainText };
            if (mimeType == "text/url")
                return new ReturnMimeItem { MimeType = "text/url", Type = typeof(string), Value = Uri.AbsolutePath };
            mimeType = mimeType.ToLower();
            var uti = mimeType.ToNsUti();
            foreach (var item in _firstPasteboardItem)
            {
                var nsUtix = item.Key as NSString;
                var itemMimeType = item.ToMime();
                if (itemMimeType == mimeType)
                {
                    string typeCodeString = null;
                    if (TypeList != null && item.Key is NSString nsUti)
                        typeCodeString = TypeList[nsUti]?.ToString();
                    var result = ReturnMimeItem.Parse(item, typeCodeString);
                    return result;
                }
            }
            return null;
        }
        */

        NSDictionary _firstPasteboardItem;
        public ClipboardEntry()
        {
            Description = UIPasteboard.General?.Name;
            if (UIPasteboard.General?.Items != null && UIPasteboard.General.Items.Length > 0)
                _firstPasteboardItem = UIPasteboard.General?.Items[0];  // this is a mistake.  There can me more than one item on the clipboard.
        }

    }

}