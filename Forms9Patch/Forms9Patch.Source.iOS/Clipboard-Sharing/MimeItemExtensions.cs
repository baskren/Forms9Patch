using System;
using System.Collections.Generic;
using Foundation;
using MobileCoreServices;
using UIKit;
using System.IO;
using System.Linq;
using Xamarin.Forms.Platform.iOS;
using System.Diagnostics;
using ObjCRuntime;
using System.Threading.Tasks;

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

        public static string ToMime(this NSString nsUti) => nsUti.ToString().ToMime();


        public static List<NSItemProvider> AsNSItemProviders(this Forms9Patch.IMimeItemCollection entry)
        {
            var itemProviders = new List<NSItemProvider>();
            foreach (var mimeItem in entry.Items)
            {
                if (mimeItem.MimeType?.ToNsUti() is NSString nsUti)
                {
                    NSItemProvider itemProvider = null;
                    /*
                    if (mimeItem.Value is Uri uri)
                    {
                        // from: https://stackoverflow.com/questions/36685160/unicode-url-could-not-initialize-an-instance-of-the-type-foundation-nsurl-t?rq=1
                        var idn = new System.Globalization.IdnMapping();
                        Console.WriteLine(uri.AbsoluteUri);
                        NSUrl nsURL = new NSUrl (uri.Scheme, idn.GetAscii (uri.DnsSafeHost), uri.PathAndQuery);
                        Console.WriteLine(nsURL.AbsoluteString);
                        itemProvider = new NSItemProvider(nsUri);
                    }
                    else if (mimeItem.Value is FileInfo fileInfo && !mimeItem.MimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Apple apps don't seem to support this approach 
                        // tried with .jpg and .pdf with Notes and Email app without success
                        Console.WriteLine(fileInfo.FullName);
                        NSUrl nsURL = NSUrl.CreateFileUrl(new string[] { fileInfo.FullName });
                        Console.WriteLine(nsURL.AbsoluteString);
                        itemProvider = new NSItemProvider(nsURL);
                    }
                    else 
                    */
                    if (mimeItem.Value is FileInfo fileInfo) // && mimeItem.MimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase))
                    {

                        // this works with Email and Notes apps for PDFs and images!!

                        Console.WriteLine(fileInfo.FullName);
                        itemProvider = new NSItemProvider();

                        itemProvider.RegisterFileRepresentation(nsUti, NSItemProviderFileOptions.OpenInPlace, NSItemProviderRepresentationVisibility.All, (completionHandler) =>
                        {
                            Console.WriteLine(fileInfo.FullName);
                            NSUrl nsURL = NSUrl.CreateFileUrl(new string[] { fileInfo.FullName });
                            Console.WriteLine(nsURL.AbsoluteString);
                            NSError nsError = null;
                            completionHandler.Invoke(nsURL, false, nsError);
                            var progress = new NSProgress
                            {
                                FileTotalCount = 1,
                                FileCompletedCount = 1,
                                TotalUnitCount = 1,
                                CompletedUnitCount = 1
                            };
                            return progress;
                        });

                    }
                    else if (mimeItem.Value is string text)
                    {
                        // from: https://josephduffy.co.uk/ios-share-sheets-the-proper-way-locations
                        var nsString = (NSString)text;
                        var utf8 = NSData.FromString(text, NSStringEncoding.UTF8);
                        itemProvider = new NSItemProvider(utf8, nsUti);
                    }
                    else if (mimeItem.Value.ToNSObject() is NSObject nsObject)
                        itemProvider = new NSItemProvider(nsObject, nsUti);
                    if (itemProvider != null)
                        itemProviders.Add(itemProvider);

                }
            }
            return itemProviders;
        }


    }

}