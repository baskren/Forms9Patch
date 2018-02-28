using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;
using MobileCoreServices;
using P42.Utils;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Diagnostics;

[assembly: Dependency(typeof(Forms9Patch.iOS.ClipboardService))]
namespace Forms9Patch.iOS
{
    public class ClipboardService : Forms9Patch.IClipboardService
    {
        static NSString TypeListUti = new NSString(UTType.CreatePreferredIdentifier(UTType.TagClassMIMEType, "application/f9p-clipboard-typelist", null));

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
        nint _changeCount = int.MinValue;
        ClipboardEntry _lastEntry = null;
        public ClipboardEntry Entry
        {
            get
            {
                if (EntryCaching && _changeCount == UIPasteboard.General.ChangeCount)
                    return _lastEntry;

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                var items = UIPasteboard.General.Items[0]; //UIPasteboard.General.GetDictionaryOfValuesFromKeys(new NSString[] { new NSString("public.html") });
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
                        ReturnClipboardEntryItem entryItem = null;

                        foreach (var typeKvp in typelist)
                        {
                            var typeMime = typeKvp.ToMime();
                            if (typeKvp.Key.ToString() == nsUti.ToString())
                            {
                                entryItem = new ReturnClipboardEntryItem(kvp, typeKvp.Value?.ToString());
                                break;
                            }
                        }
                        if (entryItem == null)
                            entryItem = new ReturnClipboardEntryItem(kvp);

                        result.AdditionalItems.Add(entryItem);
                    }
                }

                stopWatch.Stop();
                System.Diagnostics.Debug.WriteLine("SET: " + stopWatch.ElapsedMilliseconds);

                _changeCount = UIPasteboard.General.ChangeCount;
                _lastEntry = result;
                return result;
            }
            set
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var items = new NSMutableDictionary();
                var typeDictionary = new NSMutableDictionary();
                if (!string.IsNullOrEmpty(value.PlainText))
                    items.Add(new NSString("public.utf8-plain-text"), new NSString(value.PlainText));
                if (!string.IsNullOrEmpty(value.HtmlText))
                    items.Add(new NSString("public.html"), new NSString(value.HtmlText));

                int t = 0;
                long lastT = 0;
                foreach (var item in value.AdditionalItems)
                {
                    var tMark = stopWatch.ElapsedMilliseconds;
                    System.Diagnostics.Debug.WriteLine("tMark[" + (t++) + "]=[" + tMark + "] elapsed=[" + (tMark - lastT) + "]");
                    lastT = tMark;
                    NSData keyedArchive = null;
                    var nsUti = item.ToNsUti();
                    var typeInfo = item.Type.GetTypeInfo();
                    if (item.Type == typeof(byte[]))
                    {
                        var nsData = NSData.FromArray(item.Value as byte[]);
                        keyedArchive = NSKeyedArchiver.ArchivedDataWithRootObject(nsData);
                    }
                    else if (item.Value is IList ilist && typeInfo.IsGenericType)
                    {
                        var nsArray = ilist.ToNSArray();
                        keyedArchive = NSKeyedArchiver.ArchivedDataWithRootObject(nsArray);
                    }
                    else if (item.Value is IDictionary dictionary && typeInfo.IsGenericType)
                    {
                        var nsDictionary = dictionary.ToNSDictionary();
                        keyedArchive = NSKeyedArchiver.ArchivedDataWithRootObject(nsDictionary);
                    }
                    else
                    {
                        var nsObject = NSObject.FromObject(item.Value);
                        if (nsObject != null)
                            keyedArchive = NSKeyedArchiver.ArchivedDataWithRootObject(nsObject);
                        else
                            throw new InvalidDataException("Cannot convert [" + item.Type + "] to NSObject");
                    }

                    if (keyedArchive != null)
                    {
                        items.Add(nsUti, keyedArchive);
                        typeDictionary.Add(nsUti, new NSString(item.Type.FullName));
                    }
                    /*
                    if (item.Type == typeof(byte[]))
                        items.Add(nsUti, NSData.FromArray(item.Value as byte[]));
                    else if (item.Type == typeof(string))
                    {
                        var nsString = NSString.FromObject(item.Value);
                        items.Add(nsUti, nsString);
                    }
                    else if (item.Type is IList list && item.Type.IsGenericType)
                    {
                        var nsObject = NSObject.FromObject(item.Value);
                        if ()
                    }
                    else if (item.Type == typeof(char))
                        items.Add(nsUti, NSObject.FromObject(item.Value));
                        */
                }
                var tX = stopWatch.ElapsedMilliseconds;
                System.Diagnostics.Debug.WriteLine("tX=[" + tX + "] elapsedT=[" + (tX - lastT) + "]");
                lastT = tX;

                items.Add(TypeListUti, NSKeyedArchiver.ArchivedDataWithRootObject(typeDictionary));

                var tY = stopWatch.ElapsedMilliseconds;
                System.Diagnostics.Debug.WriteLine("tY=[" + tY + "] elapsedT=[" + (tY - lastT) + "]");
                lastT = tY;

                var changeCount = UIPasteboard.General.ChangeCount;

                UIPasteboard.General.Items = new NSDictionary[] { items };

                var tZ = stopWatch.ElapsedMilliseconds;
                System.Diagnostics.Debug.WriteLine("tZ=[" + tZ + "] elapsedT=[" + (tZ - lastT) + "]");
                lastT = tZ;

                _lastEntry = value;
                _changeCount = UIPasteboard.General.ChangeCount;

                stopWatch.Stop();
                System.Diagnostics.Debug.WriteLine("SET: " + stopWatch.ElapsedMilliseconds);
            }
        }

        public bool EntryCaching { get; set; } = true;

        #endregion

        #region ReturnClipboadEntryItem
        class ReturnClipboardEntryItem : IClipboardEntryItem
        {
            public string MimeType { get; private set; }

            public object Value { get; private set; }

            public Type Type { get; private set; }

            public ReturnClipboardEntryItem(KeyValuePair<NSObject, NSObject> kvp, string typeCodeString = null)
            {
                var uti = kvp.Key.ToString();
                //var values = UIPasteboard.General.DataForPasteboardType(uti);
                MimeType = UTType.GetPreferredTag(uti, UTType.TagClassMIMEType);
                var keyedArchive = (NSData)kvp.Value;
                var nsObject = NSKeyedUnarchiver.UnarchiveObject(keyedArchive);

                Type type = null;
                if (typeCodeString != null)
                    type = Type.GetType(typeCodeString);
                var tuple = nsObject.ToObject(type);
                Value = tuple.Item1;
                Type = tuple.Item2;
            }
        }
        #endregion
    }

    static class IClipboardEntryItemExtensions
    {
        public static NSString ToNsUti(this IClipboardEntryItem item)
        {
            return new NSString(UTType.CreatePreferredIdentifier(UTType.TagClassMIMEType, item.MimeType, null));
        }

        public static string ToMime(this KeyValuePair<NSObject, NSObject> kvp)
        {
            var nsUti = kvp.Key as NSString;
            var mime = UTType.GetPreferredTag(nsUti.ToString(), UTType.TagClassMIMEType);
            return mime;
        }
    }
}