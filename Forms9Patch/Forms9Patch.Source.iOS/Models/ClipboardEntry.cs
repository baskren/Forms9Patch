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
                if (_pasteBoard != null)
                {
                    var nsObj = _pasteBoard[UTType.UTF8PlainText] ?? _pasteBoard[UTType.Text] ?? _pasteBoard[UTType.UTF8TabSeparatedText] ?? _pasteBoard[UTType.DelimitedText] ?? _pasteBoard[UTType.CommaSeparatedText] ?? _pasteBoard[UTType.TabSeparatedText];
                    return nsObj as NSString;
                }
                return null;
            }
        }

        public string HtmlText => GetItem<string>("text/html")?.Value;

        public Uri Uri
        {
            get
            {
                if (_pasteBoard != null)
                {
                    var nsObj = _pasteBoard[UTType.URL] ?? _pasteBoard[UTType.FileURL];
                    if (nsObj is NSUrl nsUrl && nsUrl.AbsoluteString != null)
                        return new Uri(nsUrl.AbsoluteString);
                }
                return null;
            }
        }

        NSDictionary _typeList;
        NSDictionary TypeList
        {
            get
            {
                if (_typeList != null)
                    return _typeList;
                if (_pasteBoard != null && _pasteBoard[ClipboardService.TypeListUti] is NSData archive)
                {
                    var unkeyedArchiave = NSKeyedUnarchiver.UnarchiveObject(archive);
                    _typeList = unkeyedArchiave as NSDictionary;
                }
                return _typeList;
            }
        }


        List<string> _mimeTypes;
        public List<string> MimeTypes
        {
            get
            {
                if (_mimeTypes != null)
                    return _mimeTypes;
                if (_pasteBoard == null)
                    return null;
                _mimeTypes = new List<string>();
                if (TypeList is NSDictionary typeList)
                {
                    foreach (var kvp in typeList)
                        _mimeTypes.Add(kvp.ToMime());
                }
                else
                {
                    foreach (var key in _pasteBoard.Keys)
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

        public IMimeItem<T> GetItem<T>(string mimeType)
        {
            if (_pasteBoard == null || mimeType == null)
                return null;
            var untypedItem = GetUntypedItem(mimeType);
            return new ReturnMimeItem<T>(untypedItem);
        }

        ReturnMimeItem GetUntypedItem(string mimeType)
        {
            if (mimeType == "text/plain")
                return new ReturnMimeItem { MimeType = "text/plain", Type = typeof(string), Value = PlainText };
            if (mimeType == "text/url")
                return new ReturnMimeItem { MimeType = "text/url", Type = typeof(string), Value = Uri.AbsolutePath };
            if (_pasteBoard != null && mimeType != null)
            {
                mimeType = mimeType.ToLower();
                var uti = mimeType.ToNsUti();
                foreach (var item in _pasteBoard)
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
            }
            return null;
        }

        NSDictionary _pasteBoard;
        public ClipboardEntry()
        {
            Description = UIPasteboard.General?.Name;
            _pasteBoard = UIPasteboard.General?.Items[0];
        }

        /*
         *                 NSDictionary[] pasteboardItems = UIPasteboard.General?.Items;
                if (pasteboardItems == null || pasteboardItems.Length < 1)
                    return null;

                var items = pasteboardItems[0]; //UIPasteboard.General.GetDictionaryOfValuesFromKeys(new NSString[] { new NSString("public.html") });
                var plainText = items["public.utf8-plain-text"] as NSString;
                var htmlText = items["public.html"] as NSString;
                var result = new ClipboardEntry
                {
                    PlainText = plainText,
                    HtmlText = htmlText,
                };

                NSDictionary typelist = null;
                var keyedArchive = UIPasteboard.General.DataForPasteboardType(TypeListUti.ToString());
                if (keyedArchive != null)
                {
                    var unkeyedArchiave = NSKeyedUnarchiver.UnarchiveObject(keyedArchive);
                    typelist = unkeyedArchiave as NSDictionary;
                }


                foreach (var kvp in items)
                {
                    if ((NSString)kvp.Key != TypeListUti)
                    {
                        var nsUti = kvp.Key;
                        var mime = kvp.ToMime();
                        ReturnMimeItem entryItem = null;

                        if (typelist != null)
                        {
                            foreach (var typeKvp in typelist)
                            {
                                var typeMime = typeKvp.ToMime();
                                if (typeKvp.Key.ToString() == nsUti.ToString())
                                    entryItem = ReturnMimeItem.Parse(kvp, typeKvp.Value?.ToString());
                            }
                        }

                        if (entryItem == null)
                            entryItem = ReturnMimeItem.Parse(kvp);

                        if (entryItem != null)
                        {
                            var entryItemType = typeof(ReturnMimeItem<>).MakeGenericType(entryItem.Type);
                            var typedEntryItem = (IMimeItem)Activator.CreateInstance(entryItemType, new object[] { entryItem });
                            result._item.Add(typedEntryItem);
                        }
                    }
                }

                _changeCount = UIPasteboard.General.ChangeCount;
                _lastEntry = result;
                return result;

*/
    }
}