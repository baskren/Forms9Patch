using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;


namespace Forms9Patch.UWP
{
    class ReturnKeyValueMimeItem : IMimeItem
    {


        #region Properties
        public string MimeType => FormatId.FormatIdToMimeType();

        readonly object _value;
        public object Value => _value;

        private string FormatId { get; set; }
        #endregion


        #region Constructor
        internal ReturnKeyValueMimeItem(string formatId, object value)
        {
            FormatId = formatId;
            _value = value;
        }
        #endregion
    }

    class ReturnDataMimeItem : IMimeItem
    {
        #region Properties
        public string MimeType => FormatId.FormatIdToMimeType();

        object _value;
        public object Value => _value = _value ?? AsyncHelper.RunSync<object>(()=>DataPackageView.GetDataAsync(FormatId).AsTask());

        private string FormatId { get; set; }

        private DataPackageView DataPackageView { get; set; }
        #endregion




        #region Constructor
        internal ReturnDataMimeItem(DataPackageView dataPackageView, string formatId)
        {
            DataPackageView = dataPackageView;
            FormatId = formatId;
        }
        #endregion
    }

    class StorageFileReturnMimeItem : IMimeItem
    {
        #region Properties
        StorageFile StorageFile { get; set; }

        public string MimeType => StorageFile.ContentType;
    

        byte[] _value;

        public object Value =>  _value = _value ?? AsyncHelper.RunSync<byte[]>(() => GetValueAsync()) ;

        async Task<byte[]> GetValueAsync()
        {
            if (_value != null)
                return _value;
            var stream = await StorageFile.OpenReadAsync();
            var bytes = new byte[(int)stream.Size];
            //await stream.ReadAsync(bytes, 0, (int)stream.Size);
            //var buffer = new Windows.Storage.Streams.Buffer((uint)stream.Size);
            var buffer = bytes.AsBuffer();
            await stream.ReadAsync(buffer, (uint)stream.Size, Windows.Storage.Streams.InputStreamOptions.ReadAhead);
            return _value = bytes;
        }
        #endregion

        #region Constructor
        internal StorageFileReturnMimeItem(StorageFile file)
        {
            StorageFile = file;
        }
        #endregion
    }


    static class ReturnMimeItemExtensions
    {
        public static string FormatIdToMimeType(this string formatId)
        {
            if (formatId == StandardDataFormats.Bitmap)
                return "image/bmp";
            if (formatId == StandardDataFormats.Html)
                return "text/html";
            if (formatId == StandardDataFormats.Rtf)
                return "text/richtext";
            if (formatId == StandardDataFormats.Text)
                return "text/plain";
            return formatId;

        }
    }
}


