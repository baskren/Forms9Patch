using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Forms9Patch.UWP
{
    class ClipboardEntry : BaseClipboardEntry
    {
        #region Static Implementation
        #endregion


        #region Properties
        public override string Description => base.Description;

        public override List<IMimeItem> Items
        {
            get
            {
                if (_items != null)
                    return _items;

                _items = new List<IMimeItem>();

                var dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();

                if (dataPackageView?.AvailableFormats!=null)
                {
                    foreach (var formatId in dataPackageView.AvailableFormats)
                    {
                        IMimeItem mimeItem = null;
                        /*
                        if (formatId == Windows.ApplicationModel.DataTransfer.StandardDataFormats.Bitmap)
                            .AddValue("image/bmp", GetBitmap(dataPackageView));
                        else if (formatId == Windows.ApplicationModel.DataTransfer.StandardDataFormats.Rtf)
                            result.AddValue("text/richtext", GetRichTextFormat(dataPackageView));
                        else
                            result.AddValue(formatId, GetByteArray(dataPackageView, formatId));
                            */
                        

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


                    for (int i = 0; i < clipData.ItemCount; i++)
                    {
                        var item = clipData.GetItemAt(i);
                        var returnMimeItem = ReturnMimeItem.Parse(item);
                        if (returnMimeItem != null)
                            _items.Add(returnMimeItem);
                    }
                }
                return _items;
            }
        }
        #endregion
    }
}