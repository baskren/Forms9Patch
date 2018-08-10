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
    public class ClipboardEntry : BaseClipboardEntry
    {
        #region IClipboardEntry implemntation
        /// <summary>
        /// Short, descriptive text that can be used by app to display
        /// to the user what this data represents.
        /// </summary>
        /// <value>The description.</value>
        public new string Description { get; set; } = ApplicationInfoService.Name + " Clipboard";

        /// <summary>
        /// Gets or sets the plain text representation of this data (if any)
        /// </summary>
        /// <value>The plain text.</value>
        public new string PlainText
        {
            get => base.PlainText;
            set => AddValue("text/plain", value);
        }

        /// <summary>
        /// Gets or sets the html text representation of this data (if any)
        /// </summary>
        /// <value>The html text.</value>
        public new string HtmlText
        {
            get => base.HtmlText;
            set => AddValue("text/html", value);
        }

        #endregion


        #region Constructor
        public ClipboardEntry()
        {
            _items = new List<IMimeItem>();
        }
        #endregion


        #region Convenience methods
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
        /// Add an item to the clipboard for value and mimeType
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mimeType"></param>
        /// <param name="value"></param>
        public void AddValue(string mimeType, object value) => Items.Add(new MimeItem(mimeType, value));
        #endregion


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


    #region BaseClipboardEntry 
    public abstract class BaseClipboardEntry : IClipboardEntry
    {
        protected string _description;
        /// <summary>
        /// Short, descriptive text that can be used by app to display
        /// to the user what this data represents.
        /// </summary>
        /// <value>The description.</value>
        public virtual string Description => _description;

        /// <summary>
        /// Gets or sets the plain text representation of this data (if any)
        /// </summary>
        /// <value>The plain text.</value>
        public virtual string PlainText
        {
            get
            {
                foreach (var item in Items)
                    if (item.MimeType == "text/plain" && item.Value is string text)
                        return text;
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the html text representation of this data (if any)
        /// </summary>
        /// <value>The html text.</value>
        public virtual string HtmlText
        {
            get
            {
                foreach (var item in Items)
                    if (item.MimeType == "text/html" && item.Value is string text)
                        return text;
                return null;
            }
        }

        protected List<IMimeItem> _items;
        /// <summary>
        /// Gets or sets the items for this ClipboardEntry
        /// </summary>
        /// <value>The items.</value>
        public virtual List<IMimeItem> Items => _items;

    }
    #endregion

}