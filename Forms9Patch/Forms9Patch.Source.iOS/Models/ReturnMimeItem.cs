using System;
using System.Collections.Generic;
using Foundation;
using MobileCoreServices;
using UIKit;

namespace Forms9Patch.iOS
{
    class ReturnMimeItem<T> : IMimeItem<T>
    {
        ReturnMimeItem _source;

        public string MimeType => _source.MimeType.ToLower();
        public T Value => (T)_source.Value;
        public Type Type => _source.Type;

        object IMimeItem.Value => _source.Value;

        public ReturnMimeItem(ReturnMimeItem source)
        {
            _source = source;
        }
    }

    class ReturnMimeItem : IMimeItem
    {
        string _mimeType;
        public string MimeType
        {
            get => _mimeType;
            internal set
            {
                _mimeType = value?.ToLower();
            }
        }

        public object Value { get; internal set; }

        public Type Type { get; internal set; }

        public static ReturnMimeItem Parse(KeyValuePair<NSObject, NSObject> kvp, string typeCodeString = null)
        {
            if (kvp.Key == null)
                return null;
            var uti = kvp.Key.ToString();
            if (uti == null)
                return null;
            //var values = UIPasteboard.General.DataForPasteboardType(uti);
            var mimeType = UTType.GetPreferredTag(uti, UTType.TagClassMIMEType);
            if (mimeType == null)
                return null;

            NSObject nsObject = null;
            if (mimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase) && kvp.Value is UIImage uiImage)
            {
                switch (mimeType)
                {
                    case "image/jpeg":
                    case "image/jpg":
                        nsObject = uiImage.AsJPEG();
                        break;
                    case "image/png":
                        nsObject = uiImage.AsPNG();
                        break;
                }
            }
            if (nsObject == null)
            {
                var keyedArchive = kvp.Value as NSData;
                if (keyedArchive == null)
                    return null;
                nsObject = NSKeyedUnarchiver.UnarchiveObject(keyedArchive);
            }
            if (nsObject == null)
                return null;

            Type type = null;
            if (typeCodeString != null)
                type = Type.GetType(typeCodeString);
            var tuple = nsObject.ToObject(type);
            if (tuple == null)
                return null;

            var result = new ReturnMimeItem();
            result.Value = tuple.Item1;
            result.Type = tuple.Item2;
            result.MimeType = mimeType;
            return result;
        }
    }
}