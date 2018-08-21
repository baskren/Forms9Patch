using System;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;

namespace Forms9Patch.UWP
{
    class ReturnMimeItem : IMimeItem
    {
        #region public (static) implementation
        public static IMimeItem Parse(KeyValuePair<string, object> kvp)
        {
            if (!Forms9Patch.MimeItem.ValidValueType(kvp.Value.GetType()))
                return null;
            return new ReturnMimeItem(kvp.Key, kvp.Value);
        }
        #endregion


        #region Properties
        public string MimeType
        {
            get
            {
                if (FormatId == StandardDataFormats.Bitmap)
                    return "image/bmp";
                if (FormatId == StandardDataFormats.Html)
                    return "text/html";
                if (FormatId == StandardDataFormats.Rtf)
                    return "text/richtext";
                if (FormatId == StandardDataFormats.Text)
                    return "text/plain";
                return FormatId;
            }
        }


        public object Value { get; private set;}

        private string FormatId { get; set; }
        #endregion


        #region Constructor
        private ReturnMimeItem(string formatId, object value)
        {
            FormatId = formatId;
            Value = value;
        }

        
        #endregion
    }
}