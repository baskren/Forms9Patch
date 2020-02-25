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

        IMimeItemCollection _lastEntry = null;
        bool _lastChangedByThis = false;

        public IMimeItemCollection Entry
        {
            get
            {
                if (EntryCaching && _lastEntry != null)
                    return _lastEntry;
                var result = new MimeItemCollection();
                _lastEntry = result;
                return result;
            }
            set
            {
                try
                {
                    if (value == null)
                        return;

                    var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();


                    dataPackage.Source(value);

                    _lastEntry = value;
                    _lastChangedByThis = true;
                    Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
                }
                catch (Exception e)
                {
                    Forms9Patch.Debug.RequestUserHelp(e);
                }
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

