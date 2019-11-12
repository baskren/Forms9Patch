using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Webkit;
using Android.Database;
using Android.Net;
using System.Collections.Generic;
using Java.Lang.Reflect;
using System.Runtime.InteropServices;
using Java.Nio.FileNio;
using System.Collections;
using System.IO;
using System.Linq;
using Android.OS;
using System.Reflection;
using Android.Content.Res;
using Android.Content.PM;
using Java.IO;
using P42.Utils;
//using Java.Util;

namespace Forms9Patch.Droid
{
    static class MimeItemCollectionExtentions
    {
        public static List<ClipData.Item> AsClipDataItems(this IMimeItemCollection mimeItemCollection)
        {
            ClipboardContentProvider.Clear();
            var result = new List<ClipData.Item>();

            string text = null;
            string html = null;

            // this has to happen because GMAIL will only paste the first "text/html" AND it won't parse correctly unless it is supplied as ClipData.Item(string,string)
            foreach (var item in mimeItemCollection.Items)
            {
                if (text == null && item.MimeType == "text/plain")
                    text = item.Value as string;
                else if (html == null && item.MimeType == "text/html")
                    html = item.Value as string;
            }
            if (html != null)
                result.Add(new ClipData.Item(text ?? html, html));
            else if (text != null)
                result.Add(new ClipData.Item(text));

            foreach (var item in mimeItemCollection.Items)
            {
                ClipData.Item androidClipItem = null;


                // The following block was added to support copying images by intent. 
                // However, I have yet to see where it actually works with 3rd party apps.
                // Maybe I'm not doing it right?

                // START OF BLOCK
                if (item.MimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase) || item.Value is FileInfo)
                {
                    Java.IO.File file = null;

                    if (item.Value is FileInfo fileInfo)
                        file = new Java.IO.File(fileInfo.FullName);
                    else if (item.Value is byte[] byteArray && MimeSharp.Current.Extension(item.MimeType) is List<string> extensions && extensions.Count > 0)
                    {
                        var ext = extensions[0];
                        var fileName = Guid.NewGuid() + "." + ext;
                        var dir = P42.Utils.Environment.TemporaryStoragePath;
                        var path = Path.Combine(dir, fileName);
                        System.IO.File.WriteAllBytes(path, byteArray);
                        file = new Java.IO.File(path);
                    }

                    if (file != null && file.Exists())
                    {
                        Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
                        var intent = new Intent(Intent.ActionSend);
                        intent.SetType(item.MimeType);
                        intent.PutExtra(Intent.ExtraStream, uri);
                        intent.SetFlags(ActivityFlags.GrantReadUriPermission);
                        androidClipItem = new ClipData.Item(intent);
                    }
                    file?.Dispose();
                }
                if (androidClipItem == null)
                    // END OF BLOCK
                    androidClipItem = ClipboardContentProvider.AddAsClipDataItem(item);

                result.Add(androidClipItem);
            }


            return result;
        }

        public static List<Android.Net.Uri> AsContentUris(this IMimeItemCollection mimeItemCollection)
        {
            ClipboardContentProvider.Clear();
            var result = new List<Android.Net.Uri>();
            foreach (var item in mimeItemCollection.Items)
            {
                var uri = ClipboardContentProvider.AsAsAndroidUri(item);
                if (uri != null)
                    result.Add(uri);
            }
            return result;
        }

        public static string LowestCommonMimeType(this IMimeItemCollection mimeItemCollection)
        {
            if (mimeItemCollection.Items.Count > 0)
            {
                var parts = mimeItemCollection.Items[0].MimeType.Split('/');
                var prefix = parts[0];
                var suffix = parts[1];
                foreach (var item in mimeItemCollection.Items)
                {
                    parts = item.MimeType.Split('/');
                    if (parts[1] != suffix)
                        suffix = "*";
                    if (parts[0] != prefix)
                    {
                        suffix = "*";
                        prefix = "*";
                        break;
                    }
                }
                return prefix + "/" + suffix;
            }
            return "*/*";
        }
    }
}