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

        internal NSDataItem() { }

        private NSDataItem(IMimeItem item)
        {
            var nsObject = item.Value.ToNSObject();
            if (nsObject != null)
            {
                NSUti = item.ToNsUti();
                KeyedArchiver = NSKeyedArchiver.ArchivedDataWithRootObject(nsObject);
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