using System;
using Foundation;
using UIKit;
using MobileCoreServices;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace Forms9Patch.iOS
{
    class NSDataItem
    {
        public NSData KeyedArchiver { get; private set; }

        public NSString NSUti { get; private set; }

        private NSDataItem(IMimeItem item)
        {
            NSUti = item.ToNsUti();
            var typeInfo = item.Type.GetTypeInfo();
            if (item.Type == typeof(byte[]))
            {
                NSObject nsObject = null;
                var nsData = NSData.FromArray(item.Value as byte[]);
                if (item.MimeType == "image/png" || item.MimeType == "image/jpeg" || item.MimeType == "image/jpg")
                {
                    nsObject = UIImage.LoadFromData(nsData);
                }
                else
                {
                    nsObject = nsData;
                }
                if (nsObject != null)
                    KeyedArchiver = NSKeyedArchiver.ArchivedDataWithRootObject(nsObject);
            }
            else if (item.Value is IList ilist && typeInfo.IsGenericType)
            {
                var nsArray = ilist.ToNSArray();
                KeyedArchiver = NSKeyedArchiver.ArchivedDataWithRootObject(nsArray);
            }
            else if (item.Value is IDictionary dictionary && typeInfo.IsGenericType)
            {
                var nsDictionary = dictionary.ToNSDictionary();
                KeyedArchiver = NSKeyedArchiver.ArchivedDataWithRootObject(nsDictionary);
            }
            else if (item.Value is Uri uri)
            {
                if (uri.IsFile)
                {
                    var nsData = NSData.FromUrl(uri);
                    KeyedArchiver = NSKeyedArchiver.ArchivedDataWithRootObject(nsData);
                }
                else
                {
                    var nsUri = new NSUrl(uri.AbsoluteUri);
                    KeyedArchiver = NSKeyedArchiver.ArchivedDataWithRootObject(nsUri);
                }
            }
            else
            {
                var nsObject = NSObject.FromObject(item.Value);
                if (nsObject != null)
                    KeyedArchiver = NSKeyedArchiver.ArchivedDataWithRootObject(nsObject);
                else
                    throw new InvalidDataException("Cannot convert [" + item.Type + "] to NSObject");
            }
        }

        public static NSDataItem Create(IMimeItem item)
        {
            var result = new NSDataItem(item);
            if (result.KeyedArchiver == null)
                return null;
            return result;
        }
    }
}