using System;
using System.Collections.Generic;
using Foundation;
using MobileCoreServices;
using UIKit;
using System.Timers;
using System.Diagnostics;

namespace Forms9Patch.iOS
{

    class LazyMimeItem : INativeMimeItem
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

        object _value;
        public object Value
        {
            get
            {
                if (_value != null)
                    return _value;
                return _value = GetValueAs(null);

            }
        }

        public object GetValueAs(Type type)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            NSObject nsObject = null;

            if (MimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase) && _kvp.Value is UIImage uiImage)
            {
                switch (MimeType)
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

            //System.Diagnostics.Debug.WriteLine("\t\t\t GetValueAs 1 stopwatch.Elapsed: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();

            // IMPORTANT:  
            // Must try NSKeyedUnarchiver.UnarchiveObject() **before** NSPropertyListSerialization.PropertyListWithData()
            // The reverse will not work reliabily because NSKeyedArchive is a special form of NSPropertyListSerialization

            if (nsObject == null && _kvp.Value is NSData nsData)
                nsObject = NSKeyedUnarchiver.UnarchiveObject(nsData);
            else
                nsData = null;
            //System.Diagnostics.Debug.WriteLine("\t\t\t GetValueAs 2 stopwatch.Elapsed: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();


            if (nsObject == null && nsData != null)
            {
                NSPropertyListFormat propertyListFormat = new NSPropertyListFormat();
                nsObject = NSPropertyListSerialization.PropertyListWithData(nsData, NSPropertyListReadOptions.Immutable, ref propertyListFormat, out NSError nsError);
                nsError?.Dispose();
            }
            //System.Diagnostics.Debug.WriteLine("\t\t\t GetValueAs 3 stopwatch.Elapsed: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();


            if (nsObject == null)
                nsObject = _kvp.Value;
            //System.Diagnostics.Debug.WriteLine("\t\t\t GetValueAs 4 stopwatch.Elapsed: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();


            var result = nsObject?.ToObject(type);
            //System.Diagnostics.Debug.WriteLine("\t\t\t GetValueAs 5 stopwatch.Elapsed: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();

            return result;
        }

        public Type Type { get; internal set; }

        readonly KeyValuePair<NSObject, NSObject> _kvp;
        readonly string _typeCodeString;

        internal LazyMimeItem() { }

        private LazyMimeItem(KeyValuePair<NSObject, NSObject> kvp, string typeCodeString = null)
        {
            if (kvp.Key == null)
                return;
            var uti = kvp.Key.ToString();
            if (uti == null)
                return;
            //var values = UIPasteboard.General.DataForPasteboardType(uti);
            MimeType = uti.ToMime(); // UTType.GetPreferredTag(uti, UTType.TagClassMIMEType);

            _kvp = kvp;
            _typeCodeString = typeCodeString;

        }

        public static LazyMimeItem Parse(KeyValuePair<NSObject, NSObject> kvp, string typeCodeString = null)
        {
            var result = new LazyMimeItem(kvp, typeCodeString);
            return result.MimeType != null ? result : null;
        }
    }

    /*
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
    */
}