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
    /// Interface for a Forms9Patch.ClipboardEntry
    /// </summary>
    public interface IClipboardEntry
    {
        /// <summary>
        /// Description of this ClipboardEntry  on clipboard (really only applies to Android)
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Plain text item in this ClipboardEntry
        /// </summary>
        string PlainText { get; }

        /// <summary>
        /// HtmlText item in this ClipboardEntry
        /// </summary>
        string HtmlText { get; }

        Uri Uri { get; }

        IMimeItem<T> GetItem<T>(string mimeType);

        //        IMimeItem GetUntypedItem(string mimeType);

        List<string> MimeTypes { get; }
    }
}