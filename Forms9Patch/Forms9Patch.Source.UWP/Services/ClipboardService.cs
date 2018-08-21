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
//using Windows.ApplicationModel.DataTransfer;

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

        ClipboardEntry _lastEntry = null;
        bool _lastChangedByThis = false;

        public ClipboardEntry Entry
        {
            get
            {
                if (EntryCaching && _lastEntry != null)
                    return _lastEntry;
                var result = new ClipboardEntry();
                var dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
                if (dataPackageView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.Text))
                    result.PlainText = dataPackageView.GetText();
                if (dataPackageView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.Html))
                    result.HtmlText = dataPackageView.GetHtmlText();

                //result.Description = dataPackageView.D
                foreach (var formatId in dataPackageView.AvailableFormats)
                {
                    if (formatId == Windows.ApplicationModel.DataTransfer.StandardDataFormats.Bitmap)
                        result.AddValue("image/bmp", GetBitmap(dataPackageView));
                    else if (formatId == Windows.ApplicationModel.DataTransfer.StandardDataFormats.Rtf)
                        result.AddValue("text/richtext", GetRichTextFormat(dataPackageView));
                    else
                        result.AddValue(formatId, GetByteArray(dataPackageView, formatId));
                }

                foreach (var property in dataPackageView.Properties)
                {
                    var key = property.Key;
                    var value = property.Value;
                    var type = value.GetType();
                    if (!ClipboardEntry.ValidItemType(type))
                        continue;
                    var constructedListType = typeof(ClipboardEntryItem<>).MakeGenericType(type);
                    var item = (IClipboardEntryItem)Activator.CreateInstance(constructedListType, new object[] { key, value });
                    result.AdditionalItems.Add(item);
                }
                _lastEntry = result;
                return result;
            }
            set
            {
                if (value == null)
                    return;

                var images = new List<IMimeItem>();
                var htmls = new List<IMimeItem>();
                var rtfs = new List<IMimeItem>();
                var texts = new List<IMimeItem>();
                var others = new List<IMimeItem>();

                // categorized into types recognized by UWP DataSharing
                foreach (var item in value.Items)
                {
                    if (item.MimeType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                        images.Add(item);
                    else if (item.MimeType.ToLower() == "text/html")
                        htmls.Add(item);
                    else if (item.MimeType.ToLower() == "text/rtf" ||
                        item.MimeType.ToLower() == "text/richtext" ||
                        item.MimeType.ToLower() == "application/rtf" ||
                        item.MimeType.ToLower() == "application/x-rtf")
                        rtfs.Add(item);
                    else if (item.MimeType.ToLower() == "text/plain")
                        texts.Add(item);
                    else
                        others.Add(item);
                }
                var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();
                dataPackage.RequestedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
                var properties = dataPackage.Properties;
                if (value.Description != null)
                    properties.Description = value.Description ?? Forms9Patch.ApplicationInfoService.Name;
                if (texts.Count == 1 && texts[0].Value is string text)
                    dataPackage.SetText(text);
                if (rtfs.Count == 1 && rtfs[0].Value is string rtf)
                    dataPackage.SetRtf(rtf);
                if (htmls.Count == 1 && htmls[0].Value is string html)
                    dataPackage.SetHtmlFormat(html);
                if (images.Count == 1)
                {
                    var item = images[0];
                    BitmapImage image = new BitmapImage();
                    if (item.Value is byte[] byteArray)
                        //dataPackage.SetBitmap(ToRandomAccessStreamReference(byteArray));
                        image.SetSource(ToIRandomAccessStream(byteArray));
                    else if (item.Value is FileInfo fileInfo)
                    {
                        var storageFile = await StorageFile.GetFileFromPathAsync(fileInfo.Name);
                        var stream = await storageFile.OpenAsync(FileAccessMode.Read);
                        image.SetSource(stream);

                    }
                }


                if (value == null)
                    return;
                var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();
                var properties = dataPackage.Properties;
                //properties.ApplicationName = Forms9Patch.ApplicationInfoService.Name;
                if (value.Description!=null)
                    properties.Description = value.Description;
                dataPackage.RequestedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
                if (!string.IsNullOrEmpty(value.PlainText))
                    dataPackage.SetText(value.PlainText);
                if (!string.IsNullOrEmpty(value.HtmlText))
                    dataPackage.SetHtmlFormat(value.HtmlText);
                foreach (var item in value.AdditionalItems)
                {
                    var formatId = GetFormatId(item.MimeType);
                    if (formatId == Windows.ApplicationModel.DataTransfer.StandardDataFormats.Bitmap)
                        dataPackage.SetBitmap(ToRandomAccessStreamReference(item.Value as byte[]));
                    else if (formatId == Windows.ApplicationModel.DataTransfer.StandardDataFormats.Rtf)
                        dataPackage.SetRtf(item.Value as string);
                    else if (item.Type == typeof(byte[]))
                        dataPackage.SetData(formatId, ToIRandomAccessStream(item.Value as byte[]));
                    //dataPackage.SetData(formatId, item.Value);
                    else
                        properties.Add(formatId, item.Value);
                }
                _lastEntry = value;
                _lastChangedByThis = true;
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
            }
        }

        public bool EntryCaching { get; set; } = true;

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

        string GetFormatId(string mime)
        {
            if (mime == "image/bmp")
                return Windows.ApplicationModel.DataTransfer.StandardDataFormats.Bitmap;
            if (mime == "text/richtext")
                return Windows.ApplicationModel.DataTransfer.StandardDataFormats.Rtf;
            return mime;
        }

        internal static IRandomAccessStream ToIRandomAccessStream(byte[] arr)
        {
            return arr.AsBuffer().AsStream().AsRandomAccessStream();
        }

        internal static RandomAccessStreamReference ToRandomAccessStreamReference(byte[] arr)
        {
            return RandomAccessStreamReference.CreateFromStream(ToIRandomAccessStream(arr));
        }
    }

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

    #region Content Provider

    #endregion
}

