using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Webkit;
using Android.Database;
using Android.Net;
using System.Collections.Generic;
using Java.Lang.Reflect;
using System.Runtime.InteropServices;
using Java.Nio.FileNio;
using System.Collections;
using System.IO;
using System.Linq;
using Android.OS;
using System.Reflection;

namespace Forms9Patch.Droid
{
    class ClipboardEntry : IClipboardEntry
    {
        #region Static implementation
        readonly internal static Dictionary<Android.Net.Uri, Forms9Patch.IMimeItem> UriItems = new Dictionary<Android.Net.Uri, IMimeItem>();

        static ClipboardManager _clipboardManager;
        static ClipboardManager Clipboard
        {
            get
            {
                _clipboardManager = _clipboardManager ?? (ClipboardManager)Settings.Activity.GetSystemService(Context.ClipboardService);
                return _clipboardManager;
            }
        }

        public string Description => Clipboard.PrimaryClipDescription.Label;

        string _plainText;
        public string PlainText => _plainText;

        string _htmlText;
        public string HtmlText => _htmlText;

        public System.Uri Uri => throw new NotImplementedException();

        List<string> _mimeTypes = new List<string>();
        public List<string> MimeTypes => _mimeTypes;

        //List<IMimeItem> _mimeItems = new List<IMimeItem>();
        //public List<IMimeItem> MimeItems => _mimeItems;

        public ClipboardEntry()
        {
            var clipData = Clipboard.PrimaryClip;

            if (clipData != null)
            {
                for (int i = 0; i < clipData.ItemCount; i++)
                {
                    var item = clipData.GetItemAt(i);
                    if (!string.IsNullOrEmpty(item.HtmlText))
                    {
                        _htmlText = item.HtmlText;
                        MimeTypes.Add("text/html");
                    }
                    if (!string.IsNullOrEmpty(item.Text))
                    {
                        _plainText = item.Text;
                        MimeTypes.Add("text/plain");
                    }
                    var mimeItem = new ReturnMimeItem(item.Uri);
                    if (!MimeTypes.Contains(mimeItem.MimeType))
                        MimeTypes.Add(mimeItem.MimeType);
                }
            }
        }

        public IMimeItem<T> GetItem<T>(string mimeType)
        {
            if (Clipboard == null || mimeType == null)
                return null;
            var untypedItem = GetUntypedItem(mimeType);
            return new ReturnMimeItem<T>(untypedItem);
        }

        ReturnMimeItem GetUntypedItem(string mimeType)
        {
            if (mimeType == "text/plain")
                return new ReturnMimeItem { MimeType = "text/plain", Type = typeof(string), Value = PlainText };
            if (mimeType == "text/url")
                return new ReturnMimeItem { MimeType = "text/url", Type = typeof(string), Value = Uri.AbsolutePath };
            for (int i = 0; i < Clipboard.PrimaryClip.ItemCount; i++)
            {
                var item = Clipboard.PrimaryClip.GetItemAt(i);
                var mimeItem = new ReturnMimeItem(item.Uri);
                if (mimeItem.MimeType == mimeType)
                    return mimeItem;
            }
            return null;
        }
        #endregion


    }
}