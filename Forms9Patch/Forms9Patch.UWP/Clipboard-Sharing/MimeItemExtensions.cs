using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace Forms9Patch.UWP
{
    static class MimeItemExtensions
    {
        public static StorageFile ToStorageFile(this IMimeItem mimeItem)
        {
            StorageFile result = null;

            if (MimeSharp.Current.Extension(mimeItem.MimeType) is List<string> extensions && extensions.Count>0 && extensions[0] is string ext)
            {
                var value = mimeItem.Value;
                if (value is string text)
                {
                    var fileName = Guid.NewGuid().ToString() + "." + ext;
                    var path = Path.Combine(ApplicationData.Current.TemporaryFolder.Path, fileName);
                    File.WriteAllText(path, text);
                    result = AsyncHelper.RunSync(() => StorageFile.GetFileFromPathAsync(path).AsTask());
                }
                else if (value is byte[] byteArray)
                {
                    var fileName = Guid.NewGuid().ToString() + "." + ext;
                    var path = Path.Combine(ApplicationData.Current.TemporaryFolder.Path, fileName);
                    File.WriteAllBytes(path, byteArray);
                    result = AsyncHelper.RunSync(() => StorageFile.GetFileFromPathAsync(path).AsTask());
                }
                else if (value is FileInfo fileInfo)
                {
                    var path = fileInfo.FullName;
                    result = AsyncHelper.RunSync(() => StorageFile.GetFileFromPathAsync(path).AsTask());
                }
            }
            return result;
        }


        public static string AsString(this IMimeItem mimeItem)
        {
            var value = mimeItem.Value;
            if (value is FileInfo fileInfo && fileInfo.Exists && fileInfo.Length > 0)
                return File.ReadAllText(fileInfo.FullName);
            if (value is string text)
                return text;
            return null;
        }

        public static string AsWindowsHtmlFragment(this IMimeItem mimeItem)
        {
            if (mimeItem.MimeType=="text/html" && mimeItem.Value is string html)
            {
                var start = html.Substring(0, Math.Min(html.Length, 300));
                if (!start.ToLower().Contains("<html>"))
                {
                    // we are going to assume we were given a fragment and need to encapsulate it for other Windows apps to recognize it (argh!)
                    var fragment = html;

                    var htmlStartIndex = 105;
                    var fragStartIndex = htmlStartIndex + 36;
                    var fragEndIndex = fragStartIndex + fragment.Length;
                    var htmlEndIndex = fragEndIndex + 36;

                    html = "Version:0.9";
                    html += "\r\nStartHTML:" + htmlStartIndex.ToString("D10");
                    html += "\r\nEndHTML:" + htmlEndIndex.ToString("D10");
                    html += "\r\nStartFragment:" + fragStartIndex.ToString("D10");
                    html += "\r\nEndFragment:" + fragEndIndex.ToString("D10");
                    html += "\r\n<html>\r\n<body>\r\n<!--StartFragment-->";
                    html += fragment;
                    html += "<!--EndFragment-->\r\n</body>\r\n</html>";
                }
                return html;
            }
            return null;
        }

        static string GetFormatId(string mime)
        {
            if (mime == "image/bmp")
                return Windows.ApplicationModel.DataTransfer.StandardDataFormats.Bitmap;
            if (mime == "text/richtext")
                return Windows.ApplicationModel.DataTransfer.StandardDataFormats.Rtf;
            if (mime == "text/html")
                return Windows.ApplicationModel.DataTransfer.StandardDataFormats.Html;
            if (mime == "text/plain")
                return Windows.ApplicationModel.DataTransfer.StandardDataFormats.Text;
            return mime;
        }


        public static void Source(this Windows.ApplicationModel.DataTransfer.DataPackage dataPackage, IMimeItemCollection mimeItemCollection)
        {
            dataPackage.RequestedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;

            var properties = dataPackage.Properties;
            if (mimeItemCollection.Description != null)
                properties.Description = mimeItemCollection.Description ?? Forms9Patch.ApplicationInfoService.Name;
            properties.ApplicationName = Forms9Patch.ApplicationInfoService.Name;

            properties.Title = mimeItemCollection.Description ?? "Share " + Forms9Patch.ApplicationInfoService.Name + " data ...";


            var storageItems = new List<IStorageItem>();

            var textSet = false;
            var htmlSet = false;
            var rtfSet = false;
            var uriSet = false;

            var htmlItems = mimeItemCollection.GetMimeItems<string>("text/html");


            foreach (var item in mimeItemCollection.Items)
            {
                var mimeType = item.MimeType.ToLower();
                if (mimeType == "text/plain" && !textSet && item.AsString() is string text)
                {
                    dataPackage.SetText(text);
                    textSet = true;
                }
                // else if (mimeType == "text/html" && !htmlSet && item.AsString() is string html)
                else if (mimeType == "text/html" && !htmlSet && item.AsWindowsHtmlFragment() is string html)
                {
                    dataPackage.SetHtmlFormat(html);
                    htmlSet = true;
                }
                else if ((mimeType == "text/rtf" ||
                        mimeType == "text/richtext" ||
                        mimeType == "application/rtf" ||
                        mimeType == "application/x-rtf") &&
                        !rtfSet && item.AsString() is string rtf)
                {
                    dataPackage.SetRtf(rtf);
                    rtfSet = true;
                }
                else if (item.Value is Uri uri && !uriSet && uri.Scheme.StartsWith("http",StringComparison.OrdinalIgnoreCase))
                {
                    dataPackage.SetWebLink(uri);
                    uriSet = true;
                }

                if (item.ToStorageFile() is StorageFile storageFile)
                    storageItems.Add(storageFile);
                else
                    properties.Add(GetFormatId(item.MimeType), item.Value);
            }

            if (storageItems.Count > 0)
                dataPackage.SetStorageItems(storageItems);

        }
    }
}