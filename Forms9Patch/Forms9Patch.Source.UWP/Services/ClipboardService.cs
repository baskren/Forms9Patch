using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: Dependency(typeof(Forms9Patch.UWP.ClipboardService))]
namespace Forms9Patch.UWP
{
    public class ClipboardService : Forms9Patch.IClipboardService
    {
        public ClipboardService()
        {
            Windows.ApplicationModel.DataTransfer.Clipboard.ContentChanged += (sender, e) =>
            {
                if (!_lastChangedByThis)
                    _lastEntry = null;
                _lastChangedByThis = false;
                Clipboard.OnContentChanged(this, EventArgs.Empty);
            };
        }

        public Windows.ApplicationModel.DataTransfer.DataPackage DataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();

        IClipboardEntry _lastEntry = null;
        bool _lastChangedByThis = false;

        public IClipboardEntry Entry
        {
            get
            {
                if (EntryCaching && _lastEntry != null)
                    return _lastEntry;
                var result = new ClipboardEntry();
                _lastEntry = result;
                return result;
            }
            set
            {
                if (value == null)
                    return;

                var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();
                dataPackage.RequestedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
                var properties = dataPackage.Properties;
                if (value.Description != null)
                    properties.Description = value.Description ?? Forms9Patch.ApplicationInfoService.Name;
                properties.ApplicationName = Forms9Patch.ApplicationInfoService.Name;

                List<IStorageItem> storageItems = new List<IStorageItem>();

                bool textSet = false;
                bool htmlSet = false;
                bool rtfSet = false;

                foreach (var item in value.Items)
                {
                    var mimeType = item.MimeType.ToLower();
                    if (mimeType == "text/plain" && !textSet && item.AsString() is string text)
                    {
                        dataPackage.SetText(text);
                        textSet = true;
                    }
                    else if (mimeType == "text/html" && !htmlSet && item.AsString() is string html)
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
                    if (item.ToStorageFile() is StorageFile storageFile)
                        storageItems.Add(storageFile);
                    else
                        properties.Add(GetFormatId(item.MimeType), item.Value);
                }

                if (storageItems.Count > 1)
                    dataPackage.SetStorageItems(storageItems);


                _lastEntry = value;
                _lastChangedByThis = true;
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
            }
        }


        public bool EntryCaching { get; set; } = false;

        /*
        static byte[] GetByteArray(DataPackageView dpv, string formatId)
        {
            var task = Task<byte[]>.Run(async () =>
           {
               try
               {
                    //var reference = (IRandomAccessStreamReference)await dpv.GetDataAsync(formatId);
                    var random = (IRandomAccessStream)await dpv.GetDataAsync(formatId);
                    //return reference as byte[];

                    //var random = (Windows.Storage.Streams.IRandomAccessStream)reference.OpenReadAsync();

                    //using (var reader = new DataReader(random.GetInputStreamAt(0)))
                    using (var reader = new DataReader(random))
                   {
                       var bytes = new byte[random.Size];
                       await reader.LoadAsync((uint)random.Size);
                       reader.ReadBytes(bytes);
                       return bytes;
                   }

               }
               catch (Exception)
               {
                   return null;
               }
           });
            return task.Result;
        }

        static byte[] GetBitmap(DataPackageView dpv)
        {
            var task = Task<byte[]>.Run(async () =>
            {
                var reference = await dpv.GetBitmapAsync() as Windows.Storage.Streams.RandomAccessStreamReference;
                //var random = (Windows.Storage.Streams.IRandomAccessStream)reference.OpenReadAsync();
                var random = await reference.OpenReadAsync();
                Windows.Graphics.Imaging.BitmapDecoder decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(random);
                Windows.Graphics.Imaging.PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
                return pixelData.DetachPixelData();
            });
            return task.Result;
        }

        static string GetRichTextFormat(DataPackageView dpv)
        {
            var task = Task<string>.Run(async () =>
            {
                var rtf = await dpv.GetRtfAsync();
                return rtf;
            });
            return task.Result;
        }
        */

        string GetFormatId(string mime)
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

        /*
        internal static IRandomAccessStream ToIRandomAccessStream(byte[] arr)
        {
            return arr.AsBuffer().AsStream().AsRandomAccessStream();
        }

        internal static RandomAccessStreamReference ToRandomAccessStreamReference(byte[] arr)
        {
            return RandomAccessStreamReference.CreateFromStream(ToIRandomAccessStream(arr));
        }
        */

    }

    /*
    static class DataPackageViewExtensions
    {
        public static string GetText(this Windows.ApplicationModel.DataTransfer.DataPackageView dpv)
        {
            var task = Task.Run(async () =>
            {
                var result = await dpv.GetTextAsync();
                return result;
            });
            return task.Result;
        }

        public static string GetHtmlText(this Windows.ApplicationModel.DataTransfer.DataPackageView dpv)
        {
            var task = Task.Run(async () =>
            {
                var result = await dpv.GetHtmlFormatAsync();
                return result;
            });
            return task.Result;
        }


    }
    */

    #region Content Provider

    #endregion
}

