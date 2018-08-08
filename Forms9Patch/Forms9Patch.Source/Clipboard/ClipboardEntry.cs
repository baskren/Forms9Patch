using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using P42.Utils;
using System.Linq;

namespace Forms9Patch
{
    #region Clipboard Entry
    /// <summary>
    /// Base class that implements Forms9Patch.IClipboardEntry
    /// </summary>
    public class ClipboardEntry : IClipboardEntry
    {
        #region IClipboardEntry implemntation
        /// <summary>
        /// Convenience factory for entry with just PlainText
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static ClipboardEntry ForPlainText(string plainText, string description = null)
        {
            return new ClipboardEntry
            {
                PlainText = plainText,
                Description = description
            };
        }

        /// <summary>
        /// Short, descriptive text that can be used by app to display
        /// to the user what this data represents.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; } = ApplicationInfoService.Name + " Clipboard";

        /// <summary>
        /// Gets or sets the plain text representation of this data (if any)
        /// </summary>
        /// <value>The plain text.</value>
        public string PlainText
        {
            get => Items.FirstOrDefault((mimeItem) => mimeItem.MimeType == "text/plain")?.Value as string;
            set => AddValue("text/plain", value);
        }

        /// <summary>
        /// Gets or sets the html text representation of this data (if any)
        /// </summary>
        /// <value>The html text.</value>
        public string HtmlText
        {
            get => Items.FirstOrDefault((mimeItem) => mimeItem.MimeType == "text/html")?.Value as string;
            set => AddValue("text/html", value);
        }

        /// <summary>
        /// Gets or sets the items for this ClipboardEntry
        /// </summary>
        /// <value>The items.</value>
        public List<IMimeItem> Items { get; set; } = new List<IMimeItem>();
        #endregion


        #region Convenience methods
        /// <summary>
        /// Add an item to the clipboard for value and mimeType
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mimeType"></param>
        /// <param name="value"></param>
        public void AddValue(string mimeType, object value) => Items.Add(new MimeItem(mimeType, value));
        #endregion


        /*
        public void AddOnDemandValue<T>(string mimeType, Func<T> onDemandFunction)
        {
            _items.Add(new LazyClipboardEntryItem<T>(mimeType, onDemandFunction));
        }
        */


        // Notes to self: 

        // iOS.UIPasteBoard
        // .string
        // .image
        // .url
        // .color
        // setData:(NSData*)data forPasteboardType:(NSString*)string
        // setValue:(id)value forPasteboardType:(NSString*)string

        // Android.ClipData.Item:
        // - text
        // - htmlText
        // - uri
        // - intent

        // Windows.ApplicationModel.DataTransfer.DataPackage
        // - GetBitmapAsync
        // - GetDataAsync
        // - GetHtmlFormatAsync
        // - GetResourceMapAsync
        // - GetRtfAsync
        // - GetStorageItemsAsync (files or folders)
        // - GetTextAsync
        // - GetTextAsync(formatId) (fomatId is typically StandardDataFormats.text)
        // - GetUriAsync()
        // - GetWebLinkAsync()
        // 

        // Add Image as a future type?   
        // - iOS: UIPasteboard supports 
        // How about byte[] with a mime type?  
        // - iOS: 
        // - Android: save as Android.Net.Uri with mime type (support for multiple instances)
        // - UWP:
        // Would that work with UWP? 

    }
    #endregion

}