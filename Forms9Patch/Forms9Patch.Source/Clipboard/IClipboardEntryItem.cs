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
    /// Generic Interface for ClipboardEntryITem
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IClipboardEntryItem<T> : IClipboardEntryItem
    {
    }


    /// <summary>
    /// Interface for a ClipboardEntryItem
    /// </summary>
    public interface IClipboardEntryItem
    {
        /// <summary>
        /// MimeType for an item
        /// </summary>
        string MimeType { get; }

        /// <summary>
        /// Value of the item
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Type of the item (to help you with type conversion)
        /// </summary>
        Type Type { get; }
    }

}