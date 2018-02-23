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
    public interface IClipboardEntry
    {
        string Description { get; }

        string PlainText { get; }

        string HtmlText { get; }

        List<IClipboardEntryItem> AdditionalItems { get; }
    }

    public class ClipboardEntry : IClipboardEntry
    {
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

        public bool ContainsMimeType(string mimeType)
        {
            return MimeTypes.Contains(mimeType);
        }

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

        public void AddItem<T>(string mimeType, T item)
        {
            _items.Add(new ClipboardEntryItem<T>(mimeType, item));
        }

        public void AddOnDemandItem<T>(string mimeType, Func<T> onDemandFunction)
        {
            _items.Add(new LazyClipboardEntryItem<T>(mimeType, onDemandFunction));
        }


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
            public PlaceholderEntryItem(string mimeType, string item) : base(mimeType, item) { }
        }
    }
    #endregion

    #region ClipboardEntryItem
    public interface IClipboardEntryItem
    {
        string MimeType { get; }

        object Item { get; }

        Type Type { get; }
    }


    public interface IClipboardEntryItem<T> : IClipboardEntryItem
    {
    }

    interface IPlatformKey
    {
        string PlatformKey { get; set; }
    }

    public abstract class ClipboardItemBase<T> : IClipboardEntryItem<T>, IPlatformKey
    {
        public static bool ValidEntryItemType(Type type)
        {
            if (type == typeof(byte))
                return true;
            if (type == typeof(byte[]))
                return true;
            if (type == typeof(char))
                return true;
            if (type == typeof(ushort))
                return true;
            if (type == typeof(short))
                return true;
            if (type == typeof(uint))
                return true;
            if (type == typeof(int))
                return true;
            if (type == typeof(ulong))
                return true;
            if (type == typeof(long))
                return true;
            if (type == typeof(float))
                return true;
            if (type == typeof(double))
                return true;
            if (type == typeof(decimal))
                return true;
            if (type == typeof(string))
                return true;
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.ImplementedInterfaces.Contains(typeof(IList)) && typeInfo.IsGenericType)
            {
                var elementType = typeInfo.GenericTypeArguments[0];
                if (elementType == typeof(string))
                    return true;
                if (elementType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IEnumerable)))
                    return false;
                return ValidEntryItemType(elementType);
            }
            //if (type == typeof(System.IO.File))
            //    return true;
            return false;
        }


        public string MimeType { get; protected set; }

        object _item;
        public virtual object Item
        {
            get => _item;
            protected set => _item = value;
        }

        public Type Type { get; protected set; }

        string IPlatformKey.PlatformKey { get; set; }

        protected ClipboardItemBase(string mimeType)
        {
            Type = typeof(T);
            if (!ValidEntryItemType(Type))
                throw new ArgumentException("Item type [" + Type + "] is not a valid ClipboardEntryItem type.");
            if (string.IsNullOrWhiteSpace(mimeType))
                throw new InvalidDataContractException("Empty or null mime type is not allowed.");
            if (mimeType == "text/plain")
                throw new InvalidDataContractException("Use ClipboardEntry.PlainText instead.");
            if (mimeType == "text/html")
                throw new InvalidDataContractException("Use ClipboardEntry.HtmlText instead.");
            MimeType = mimeType;
        }
    }

    public class ClipboardEntryItem<T> : ClipboardItemBase<T>
    {

        public ClipboardEntryItem(string mimeType, T item) : base(mimeType)
        {
            Item = item;
        }
    }

    public class LazyClipboardEntryItem<T> : ClipboardItemBase<T>
    {
        readonly Func<T> _onDemandFunction;
        public override object Item => _onDemandFunction.Invoke();

        public LazyClipboardEntryItem(string mimeType, Func<T> onDemandFunction) : base(mimeType)
        {
            _onDemandFunction = onDemandFunction ?? throw new Exception("Must set a valid Func<T> for onDemandFunction");
        }
    }

    /*
    public class FilePathEntryItem : ClipboardEntryItem<string>
    {
        public FilePathEntryItem(string mimeType, string path) : base(mimeType, path) { }
    }
    */
    #endregion
}