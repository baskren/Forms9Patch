using System;
using System.Collections.Generic;
using Foundation;
using MobileCoreServices;
using UIKit;
using System.IO;

namespace Forms9Patch.iOS
{
    static class IMimeItemExtensions
    {
        public static KeyValuePair<NSString, NSObject> ToUiPasteboardItem(this IMimeItem mimeItem)
        {
            if (mimeItem.MimeType?.ToNsUti() is NSString nsUti) // && mimeItem.Value.ToNSObject() is NSObject nSObject)
            {
                NSData nsData = null;
                if (mimeItem.Value is byte[] byteArray)
                    nsData = NSData.FromArray(byteArray);
                else if (mimeItem.Value is FileInfo fileInfo)
                    nsData = NSData.FromFile(fileInfo.FullName);
                else if (mimeItem.Value is Uri uri)
                {
                    var nsUrl = new NSUrl(uri.AbsoluteUri);
                    nsData = NSData.FromUrl(nsUrl);
                }
                if (mimeItem.MimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase) && nsData != null && nsData.Length > 0)
                {
                    var uiImage = UIImage.LoadFromData(nsData);
                    return new KeyValuePair<NSString, NSObject>(nsUti, uiImage);
                }
                NSObject nsObject = nsData ?? mimeItem.Value.ToNSObject();
                if (nsObject != null)
                    return new KeyValuePair<NSString, NSObject>(nsUti, nsObject);
            }
            return new KeyValuePair<NSString, NSObject>();
        }

        public static NSString ToNsUti(this IMimeItem item)
        {
            return item.MimeType.ToNsUti();
        }

        public static NSString ToNsUti(this string mimeType)
        {
            var uuType = UTType.CreatePreferredIdentifier(UTType.TagClassMIMEType, mimeType.ToLower(), null);

            if (mimeType == "text/url")
            {
                if (mimeType.StartsWith("file://", StringComparison.InvariantCultureIgnoreCase))
                    return UTType.FileURL;
                return UTType.URL;
            }
            //var uuType = UTType.CreatePreferredIdentifier(UTType.TagClassMIMEType, mimeType.ToLower(), null);
            return new NSString(uuType);
        }

        public static string ToMime(this KeyValuePair<NSObject, NSObject> kvp)
        {
            var nsUti = kvp.Key as NSString;
            var mime = nsUti.ToMime(); // UTType.GetPreferredTag(nsUti.ToString(), UTType.TagClassMIMEType);
            return mime;
        }

        public static string ToMime(this string uti)
        {
            var result = UTType.GetPreferredTag(uti, UTType.TagClassMIMEType);
            if (result == null)
            {
                if (uti == UTType.URL || uti == UTType.FileURL)
                    return "text/url";
                if (uti == UTType.UTF8PlainText || uti == UTType.Text || uti == UTType.PlainText)
                    return "text/plain";
                System.Console.WriteLine("Unknown UTI: " + uti);
                return null;
            }
            return result.ToLower();
        }

        public static string ToMime(this NSString nsUti)
        {
            return nsUti.ToString().ToMime();
        }


    }

}