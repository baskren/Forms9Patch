using System;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using Xamarin.Forms.Internals;
using P42.Utils;

namespace Forms9Patch
{
    #region Clipboard Entry
    /// <summary>
    /// Base class that implements Forms9Patch.IClipboardEntry
    /// </summary>
    public class ClipboardEntry : IClipboardEntry
    {
        /// <summary>
        /// Test to determine if type can be safely put on clipboard across platforms (without crazy schema gymnastics)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ValidItemType(Type type)
        {
            if (type == typeof(byte[]))
                return true;
            if (type == typeof(byte))
                return true;
            if (type == typeof(char))
                return true;
            //if (type == typeof(ushort))  
            //    return true;
            if (type == typeof(short))
                return true;
            //if (type == typeof(uint))
            //    return true;
            if (type == typeof(int))
                return true;
            //if (type == typeof(ulong))
            //    return true;
            if (type == typeof(long))
                return true;
            if (type == typeof(float))
                return true;
            if (type == typeof(double))
                return true;
            //if (type == typeof(decimal))
            //    return true;
            if (type == typeof(string))
                return true;
            var typeInfo = type.GetTypeInfo();
            //if (item.Value is IList ilist && typeInfo.IsGenericType)
            if (typeInfo.ImplementedInterfaces.Contains(typeof(IList)) && typeInfo.IsGenericType)
            {
                var elementType = typeInfo.GenericTypeArguments[0];
                if (elementType == typeof(string))  // need to do this because string is an IEnumerable
                    return true;
                var elementTypeInfo = elementType.GetTypeInfo();
                if (elementTypeInfo.ImplementedInterfaces.Contains(typeof(IDictionary)) && typeInfo.IsGenericType)
                    return ValidDictionary(elementTypeInfo);
                if (elementTypeInfo.ImplementedInterfaces.Contains(typeof(IEnumerable)))
                    return false;
                return ValidItemType(elementType);
            }
            if (typeInfo.ImplementedInterfaces.Contains(typeof(IDictionary)) && typeInfo.IsGenericType)
                return ValidDictionary(typeInfo);// && ValidItemType(valueType);

            //if (type == typeof(System.IO.File))
            //    return true;
            return false;
        }

        static bool ValidDictionary(TypeInfo typeInfo)
        {
            var keyType = typeInfo.GenericTypeArguments[0];
            var valueType = typeInfo.GenericTypeArguments[1];
            if (keyType != typeof(string))
                return false;
            if (valueType == typeof(string))
                return true;
            if (valueType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IEnumerable)) || valueType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IDictionary)))
                return false;
            return ValidItemType(keyType);// && ValidItemType(valueType);
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
        public string PlainText { get; set; }

        /// <summary>
        /// Gets or sets the html text representation of this data (if any)
        /// </summary>
        /// <value>The html text.</value>
        public string HtmlText { get; set; }

        readonly List<IClipboardEntryItem> _items = new List<IClipboardEntryItem>();
        /// <summary>
        /// Any additional items (IClipboardEntryItem) in this ClipboardEntry
        /// </summary>
        public List<IClipboardEntryItem> AdditionalItems
        {
            get => _items;
            set
            {
                _items.Clear();
                foreach (var item in value)
                    _items.Add(item);
            }
        }

        /// <summary>
        /// List of MimeTypes for the items in this ClipboardEntry
        /// </summary>
        public List<string> MimeTypes
        {
            get
            {
                var result = new List<string>();
                if (!string.IsNullOrEmpty(PlainText))
                    result.Add("text/plain");
                if (!string.IsNullOrEmpty(HtmlText))
                    result.Add("text/html");
                foreach (var item in _items)
                    result.Add(item.MimeType);
                return result;
            }
        }

        /// <summary>
        /// Does this ClipboardEntry contain an item of a particular mimeType?
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public bool ContainsMimeType(string mimeType)
        {
            return MimeTypes.Contains(mimeType);
        }

        /// <summary>
        /// Get me the item in this ClipboardEntry that has a particular mimeType
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public IClipboardEntryItem GetItem(string mimeType)
        {
            if (mimeType == "text/plain" && PlainText != null)
                return new PlaceholderEntryItem(mimeType, PlainText);
            if (mimeType == "text/html" && HtmlText != null)
                return new PlaceholderEntryItem(mimeType, HtmlText);
            foreach (var item in _items)
                if (item.MimeType == mimeType)
                    return item;
            return null;
        }

        /// <summary>
        /// Add an item to the clipboard for value and mimeType
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mimeType"></param>
        /// <param name="value"></param>
        public void AddValue<T>(string mimeType, T value)
        {
            _items.Add(new ClipboardEntryItem<T>(mimeType, value));
        }

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

        class PlaceholderEntryItem : ClipboardEntryItem<string>
        {
            public PlaceholderEntryItem(string mimeType, string value) : base(mimeType, value) { }
        }
    }
    #endregion

}