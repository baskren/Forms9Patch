using System;
using System.Collections.Generic;
using Foundation;
using MobileCoreServices;

namespace Forms9Patch.iOS
{
    static class IMimeItemExtensions
    {
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
            if (nsUti == UTType.URL || nsUti == UTType.FileURL)
                return "text/url";
            if (nsUti == UTType.UTF8PlainText)
                System.Diagnostics.Debug.WriteLine("");
            var mime = UTType.GetPreferredTag(nsUti.ToString(), UTType.TagClassMIMEType);
            return mime.ToLower();
        }

        public static string ToMime(this string nsUti)
        {
            return UTType.GetPreferredTag(nsUti, UTType.TagClassMIMEType).ToLower();
        }
    }

}