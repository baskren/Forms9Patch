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
    /// <summary>
    /// ClipboardEntryItem class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MimeItem<T> : MimeItemBase, IMimeItem<T>
    {
        /// <summary>
        /// Gets or sets the value of this ClipboadEntryItem.
        /// </summary>
        /// <value>The value.</value>
        new public T Value
        {
            get => (T)base.Value;
            set => base.Value = value;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mimeType"></param>
        /// <param name="value"></param>
        public MimeItem(string mimeType, T value) : base(mimeType)
        {
            Type = typeof(T);
            if (!ClipboardEntry.ValidItemType(Type))
                throw new ArgumentException("Item type [" + Type + "] is not a valid ClipboardEntryItem type.");
            Value = value;
        }
    }

    /// <summary>
    /// Base class for a ClipboardEntryItem
    /// </summary>
    public abstract class MimeItemBase : IMimeItem //, IPlatformKey
    {
        string _mimeType;
        /// <summary>
        /// Get the MimeType of this ClipboardEntryItem
        /// </summary>
        public string MimeType
        {
            get => _mimeType;
            protected set
            {
                _mimeType = value?.ToLower();
            }
        }

        object _item;
        /// <summary>
        /// Get the Value of thie ClipboardEntryItem
        /// </summary>
        public virtual object Value
        {
            get => _item;
            protected set => _item = value;
        }

        /// <summary>
        /// Get the Type of the Value of this ClipboardEntryItem (less than perfect but it's better than nothing)
        /// </summary>
        public Type Type { get; protected set; }

        //string IPlatformKey.PlatformKey { get; set; }

        /// <summary>
        /// Constructor for ClipboardItemBase
        /// </summary>
        /// <param name="mimeType"></param>
        protected MimeItemBase(string mimeType)
        {
            if (string.IsNullOrWhiteSpace(mimeType))
                throw new InvalidDataContractException("Empty or null mime type is not allowed.");
            if (mimeType == "text/plain")
                throw new InvalidDataContractException("Use ClipboardEntry.PlainText instead.");
            if (mimeType == "text/html")
                throw new InvalidDataContractException("Use ClipboardEntry.HtmlText instead.");
            MimeType = mimeType;
        }
    }


    /*
    public class LazyClipboardEntryItem<T> : ClipboardItemBase<T>
    {
        readonly Func<T> _onDemandFunction;
        public override object Value => _onDemandFunction.Invoke();

        public LazyClipboardEntryItem(string mimeType, Func<T> onDemandFunction) : base(mimeType)
        {
            _onDemandFunction = onDemandFunction ?? throw new Exception("Must set a valid Func<T> for onDemandFunction");
        }
    }
    */

    /*
    public class FilePathEntryItem : ClipboardEntryItem<string>
    {
        public FilePathEntryItem(string mimeType, string path) : base(mimeType, path) { }
    }
    */

}

