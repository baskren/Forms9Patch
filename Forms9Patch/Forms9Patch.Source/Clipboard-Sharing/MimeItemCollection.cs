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
    /// Class used to supply data to Clipboard and Interapp Sharing
    /// </summary>
    public class MimeItemCollection : BaseMimeItemCollection
    {
        #region IMimeItemCollection implemntation
        /// <summary>
        /// Short, descriptive text that can be used by app to display
        /// to the user what this data represents.
        /// </summary>
        /// <value>The description.</value>
        public new string Description { get; set; } = ApplicationInfoService.Name + " Clipboard";

        bool _plainTextSet;
        /// <summary>
        /// Gets or sets the plain text representation of this data (if any)
        /// </summary>
        /// <value>The plain text.</value>
        public new string PlainText
        {
            get => base.PlainText;
            set
            {
                if (_items.Count > 0 && _items[0].MimeType == "text/plain")
                    _items.RemoveAt(0);
                _items.Insert(0, new MimeItem("text/plain", value));
                _plainTextSet = true;
            }
        }

        /// <summary>
        /// Gets or sets the html text representation of this data (if any)
        /// </summary>
        /// <value>The html text.</value>
        public new string HtmlText
        {
            get => base.HtmlText;
            set
            {
                if (_items.Count > 1 && _items[0].MimeType == "text/plain" && _items[1].MimeType == "text/html")
                {
                    _items.RemoveAt(1);
                    if (!_plainTextSet)
                        _items.RemoveAt(0);
                }
                if (!_plainTextSet)
                    _items.Insert(0, new MimeItem("text/plain", value));
                _items.Insert(1, new MimeItem("text/html", value));
            }
        }

        #endregion


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.MimeItemCollection"/> class.
        /// </summary>
        public MimeItemCollection()
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
        public static MimeItemCollection ForPlainText(string plainText, string description = null)
        {
            return new MimeItemCollection
            {
                PlainText = plainText,
                Description = description
            };
        }

        /// <summary>
        /// Adds a new item to the MimeItemCollection
        /// </summary>
        /// <param name="mimeType">MIME type.</param>
        /// <param name="value">Value.</param>
        public void AddValue(string mimeType, object value)
        {
            Items.Add(new MimeItem(mimeType, value));
        }
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
    /// <summary>
    /// Do not use! Use MimeItemCollection instead!
    /// </summary>
    public abstract class BaseMimeItemCollection : IMimeItemCollection
    {
        /// <summary>
        /// The backing store for the MimeItemCollection's description
        /// </summary>
        protected string _description;

        /// <summary>
        /// Short, descriptive text that can be used by app to display
        /// to the user what this collection represents.
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

        /// <summary>
        /// The backing store for the collection's items.
        /// </summary>
        protected List<IMimeItem> _items;
        /// <summary>
        /// Gets or sets the items for this MimeItemCollection
        /// </summary>
        /// <value>The items.</value>
        public virtual List<IMimeItem> Items => _items;

    }
    #endregion

}