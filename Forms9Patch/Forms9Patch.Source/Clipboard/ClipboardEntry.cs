using System;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using Xamarin.Forms.Internals;
using P42.Utils;
using System.IO;
using System.Linq;

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

            //if (type == typeof(Uri))
            //    return true;

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
            get => GetItem<string>("text/plain")?.Value;
            set => AddValue("text/plain", value);
        }

        /// <summary>
        /// Gets or sets the html text representation of this data (if any)
        /// </summary>
        /// <value>The html text.</value>
        public string HtmlText
        {
            get => GetItem<string>("text/html")?.Value;
            set => AddValue("text/html", value);
        }

        public Uri Uri
        {
            get
            {
                if (GetItem<string>("text/url")?.Value is string text)
                    return new Uri(text);
                return null;
            }
            set => AddValue("text/url", value.ToString());
        }

        // only used when creating / adding to 
        //readonly List<IMimeItem> _items = new List<IMimeItem>();
        internal readonly Dictionary<string, IMimeItem> MimeHash = new Dictionary<string, IMimeItem>();

        /// <summary>
        /// List of MimeTypes for the items in this ClipboardEntry
        /// </summary>
        public List<string> MimeTypes => MimeHash.Keys.ToList();

        /// <summary>
        /// Does this ClipboardEntry contain an item of a particular mimeType?
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public bool ContainsMimeType(string mimeType)
        {
            return MimeTypes.Contains(mimeType);
        }

        IMimeItem GetUntypedItem(string mimeType)
        {
            if (MimeHash.ContainsKey(mimeType))
                return MimeHash[mimeType];
            return null;
        }

        /// <summary>
        /// Get me the item in this ClipboardEntry that has a particular mimeType
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="mimeType">MIME type.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public IMimeItem<T> GetItem<T>(string mimeType)
        {
            var untypedItem = GetUntypedItem(mimeType);
            var typedItem = untypedItem as IMimeItem<T>;
            return typedItem;
        }

        /// <summary>
        /// Add an item to the clipboard for value and mimeType
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mimeType"></param>
        /// <param name="value"></param>
        public void AddValue<T>(string mimeType, T value)
        {
            if (!ValidItemType(typeof(T)))
                return;
            MimeHash[mimeType] = new MimeItem<T>(mimeType, value);
        }


        public byte[] AddContentOfFile(string mimeType, string path)
        {
            // we may want to change this to URL!
            if (ByteArrayFromFile(path) is byte[] byteArray)
            {
                MimeHash[mimeType] = new MimeItem<byte[]>(mimeType, byteArray);
                return byteArray;
            }
            return null;
        }

        static byte[] ByteArrayFromFile(String path)
        {
            using (FileStream filestream = new FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                if (filestream != null)
                {
                    System.Diagnostics.Debug.WriteLine("embedded resource length: " + filestream.Length);
                    using (BinaryReader br = new BinaryReader(filestream))
                    {
                        if (br != null)
                        {
                            //using (FileStream fs = new FileStream(path, FileMode.Create))
                            using (MemoryStream ms = new MemoryStream((int)filestream.Length))
                            {
                                if (ms != null)
                                {
                                    using (BinaryWriter bw = new BinaryWriter(ms))
                                    {
                                        if (bw != null)
                                        {
                                            byte[] ba = new byte[filestream.Length];
                                            filestream.Read(ba, 0, ba.Length);
                                            bw.Write(ba);
                                            var fileInfo = new System.IO.FileInfo(path);
                                            System.Diagnostics.Debug.WriteLine("output length: " + fileInfo.Length);
                                            System.Diagnostics.Debug.WriteLine("output length: " + ms.Length);
                                            return ms.ToArray();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
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

        class PlaceholderEntryItem : MimeItem<string>
        {
            public PlaceholderEntryItem(string mimeType, string value) : base(mimeType, value) { }
        }
    }
    #endregion

}