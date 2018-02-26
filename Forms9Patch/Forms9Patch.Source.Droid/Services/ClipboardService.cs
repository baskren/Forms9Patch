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

[assembly: Dependency(typeof(Forms9Patch.Droid.ClipboardService))]
namespace Forms9Patch.Droid
{
    public class ClipboardService : Forms9Patch.IClipboardService
    {

        #region Static implementation
        readonly internal static Dictionary<Android.Net.Uri, Forms9Patch.IClipboardEntryItem> UriItems = new Dictionary<Android.Net.Uri, IClipboardEntryItem>();

        static ClipboardManager _clipboardManager;
        static ClipboardManager Clipboard
        {
            get
            {
                _clipboardManager = _clipboardManager ?? (ClipboardManager)Settings.Activity.GetSystemService(Context.ClipboardService);
                return _clipboardManager;
            }
        }
        #endregion


        class ReturnClipboardEntryItem : IClipboardEntryItem
        {
            public string MimeType { get; protected set; }

            ICursor _cursor = null;
            ICursor Cursor
            {
                get
                {
                    if (_cursor == null)
                    {
                        var loader = new CursorLoader(Settings.Activity, _uri, null, null, null, null);
                        _cursor = (ICursor)loader.LoadInBackground();
                    }
                    return _cursor;
                }
            }

            public object Value
            {
                get
                {
                    if (Cursor.Count == 0)
                        return null;
                    if (Cursor.Count == 1)
                    {
                        switch (Cursor.GetType(0))
                        {
                            case FieldType.Blob:
                                return Cursor.GetBlob(0);
                            case FieldType.Float:
                                return Cursor.GetDouble(0);
                            case FieldType.Integer:
                                return Cursor.GetInt(0);
                            case FieldType.Null:
                                return null;
                            case FieldType.String:
                                return Cursor.GetString(0);
                        }
                    }
                    return null;
                }
            }

            Type _type;
            public Type Type
            {
                get
                {
                    _type = _type ?? DetermineType();
                    return _type;
                }
            }

            readonly Android.Net.Uri _uri;

            public ReturnClipboardEntryItem(Android.Net.Uri uri)
            {
                _uri = uri;
                MimeType = Settings.Activity.ContentResolver.GetType(uri);
            }

            Type DetermineType()
            {
                if (Cursor.ColumnCount == 0)
                    return null;
                if (Cursor.ColumnCount == 1)
                {
                    var type = Cursor.GetType(0).ToCSharpType();
                    return type;
                }
                for (int i = 0; i < Cursor.ColumnCount; i++)
                    System.Diagnostics.Debug.WriteLine("column[" + i + "] type=[" + Cursor.GetType(i) + "]");
                return null;
            }
        }


        #region IClipboardService
        public ClipboardEntry Entry
        {
            get
            {
                if (!Clipboard.HasPrimaryClip)
                    return null;
                var entry = new ClipboardEntry();
                var description = Clipboard.PrimaryClipDescription;
                var clipData = Clipboard.PrimaryClip;

                entry.Description = description.Label;

                for (int i = 0; i < clipData.ItemCount; i++)
                {
                    var item = clipData.GetItemAt(i);
                    if (!string.IsNullOrEmpty(item.HtmlText))
                        entry.HtmlText = item.HtmlText;
                    if (!string.IsNullOrEmpty(item.Text))
                        entry.PlainText = item.Text;
                    if (item.Uri != null)
                    {
                        var entryItem = new ReturnClipboardEntryItem(item.Uri);
                        entry.AdditionalItems.Add(entryItem);
                    }
                }
                return entry;
            }
            set
            {
                if (value == null)
                    return;
                ClipData clipData = null;

                if (string.IsNullOrEmpty(value.HtmlText))
                    clipData = ClipData.NewPlainText(value.Description, value.PlainText);
                else
                    clipData = ClipData.NewHtmlText(value.Description, value.PlainText, value.HtmlText);

                UriItems.Clear();
                foreach (var item in value.AdditionalItems)
                {
                    // here is where we would detect if the item is a FilePathEntryItem or the item.Type is a System.IO.File and then setup things to use a android.support.v4.content.FileProvider
                    var uri = ClipboardContentProvider.NextItemUri;
                    UriItems[uri] = item;
                    //}
                    var androidClipItem = new ClipData.Item(uri);
                    clipData.AddItem(androidClipItem);
                }

                //ContentValues contentValues = new ContentValues(2);
                //contentValues.Put(Android.Provider.MediaStore.Images.Media, "application/json");

                Clipboard.PrimaryClip = clipData;
            }
        }
        #endregion
    }




    class IListCursor : AbstractCursor
    {
        IList _list;
        string _mimeType;
        FieldType _fieldType;
        Type _itemsType;

        public IListCursor(string mimeType, IList list, Type itemsType)
        {
            _list = list;
            _mimeType = mimeType;
            _fieldType = itemsType.ToAndroidFieldType();
            _itemsType = itemsType;
        }

        public override int Count => _list.Count;

        public override string[] GetColumnNames()
        {
            return new string[] { _mimeType };
        }

        public override double GetDouble(int column)
        {
            if (_fieldType == FieldType.Float || _fieldType == FieldType.Integer)
                return (double)_list[Position];
            throw new InvalidCastException("itemsType [" + _itemsType + "] cannot be cast to double");
        }

        public override float GetFloat(int column)
        {
            if (_fieldType == FieldType.Float || _fieldType == FieldType.Integer)
                return (float)_list[Position];
            throw new InvalidCastException("itemsType [" + _itemsType + "] cannot be cast to float");
        }

        public override int GetInt(int column)
        {
            if (_fieldType == FieldType.Integer || _fieldType == FieldType.Integer)
                return (int)_list[Position];
            throw new InvalidCastException("itemsType [" + _itemsType + "] cannot be cast to int");
        }

        public override long GetLong(int column)
        {
            if (_fieldType == FieldType.Integer || _fieldType == FieldType.Integer)
                return (long)_list[Position];
            throw new InvalidCastException("itemsType [" + _itemsType + "] cannot be cast to long");
        }

        public override short GetShort(int column)
        {
            if (_fieldType == FieldType.Integer || _fieldType == FieldType.Integer)
                return (short)_list[Position];
            throw new InvalidCastException("itemsType [" + _itemsType + "] cannot be cast to short");
        }

        public override string GetString(int column)
        {
            if (_fieldType == FieldType.String)
                return (string)_list[Position];
            throw new InvalidCastException("itemsType [" + _itemsType + "] cannot be cast to string");
        }

        public override bool IsNull(int column)
        {
            return _list[Position] == null;
        }
    }

    class PrimativeCursor : AbstractCursor
    {
        readonly object _primative;
        readonly string _mimeType;

        public PrimativeCursor(string mimeType, object primative)
        {
            _primative = primative;
            if (primative.GetType().ToAndroidFieldType() == FieldType.Null && primative != null)
            {
                throw new InvalidDataException("PrimativeCursor does not work with [" + primative.GetType() + "]");
            }
        }

        public override int Count => 1;

        public override int ColumnCount => 1;


        public override string[] GetColumnNames()
        {
            return new string[] { _mimeType };
        }

        public override double GetDouble(int column)
        {
            return (double)_primative;
        }

        public override float GetFloat(int column)
        {
            return (float)_primative;
        }

        public override int GetInt(int column)
        {
            return (int)_primative;
        }

        public override long GetLong(int column)
        {
            return (long)_primative;
        }

        public override short GetShort(int column)
        {
            return (short)_primative;
        }

        public override string GetString(int column)
        {
            if (_primative is string str)
                return str;
            return _primative.ToString();
        }

        public override bool IsNull(int column)
        {
            return _primative == null;
        }

        public override byte[] GetBlob(int column)
        {
            if (_primative is byte[] byteArray)
                return byteArray;
            return base.GetBlob(column);
        }

        public override FieldType GetType(int column)
        {
            if (_primative == null)
                return FieldType.Null;
            var fieldType = _primative.GetType().ToAndroidFieldType();
            if (fieldType != FieldType.Null)
                return fieldType;
            return FieldType.String;
        }

    }

    [ContentProvider(new string[] { "@string/forms9patch_copy_paste_authority" })]
    public class ClipboardContentProvider : ContentProvider
    {
        static Forms9Patch.IClipboardEntryItem ItemForUri(Android.Net.Uri uri)
        {
            foreach (var key in ClipboardService.UriItems.Keys)
                if (key.Equals(uri))
                    return ClipboardService.UriItems[key];
            return null;
        }

        static int _resourceId
        {
            get
            {
                var result = GetResourceId("forms9patch_copy_paste_authority", "string", Settings.Activity.PackageName);
                return result;
            }
        } // = GetResourceId("forms9patch_copy_paste_authority", "string", Settings.Activity.PackageName);
        public static string AUTHORITY
        {
            get
            {
                var result = Settings.Activity.Resources.GetString(_resourceId);
                return result;
            }
        }// = Settings.Activity.Resources.GetString(_resourceId);
        public static Android.Net.Uri CONTENT_URI
        {
            get
            {
                var result = Android.Net.Uri.Parse("content://" + AUTHORITY);
                return result;
            }
        }
        //= Android.Net.Uri.Parse("content://" + AUTHORITY);

        static int _index;
        public static Android.Net.Uri NextItemUri
        {
            get
            {
                var result = Android.Net.Uri.Parse("content://" + AUTHORITY + "/" + _index++);
                return result;
            }
        }

        public override int Delete(Android.Net.Uri uri, string selection, string[] selectionArgs)
        {
            return -1;
        }

        public override string GetType(Android.Net.Uri uri)
        {
            var item = ItemForUri(uri);
            return item?.MimeType;
        }

        public override Android.Net.Uri Insert(Android.Net.Uri uri, ContentValues values)
        {
            return null;
        }

        public override bool OnCreate()
        {
            //_resourceId = GetResourceId("forms9patch_copy_paste_authority", "string", Settings.Activity.PackageName);
            //AUTHORITY = Settings.Activity.Resources.GetString(_resourceId);
            //CONTENT_URI = Android.Net.Uri.Parse("content://" + AUTHORITY);
            return true;
        }

        public override ICursor Query(Android.Net.Uri uri, string[] projection, string selection, string[] selectionArgs, string sortOrder)
        {
            var item = ItemForUri(uri);
            if (item == null)
                return null;
            if (item.Type.ToAndroidFieldType() != FieldType.Null)
                return new PrimativeCursor(item.MimeType, item.Value);
            if (item.Value is IList list && item.Type.IsGenericType)
            {
                var elementType = item.Type.GetElementType();
                var fieldType = elementType.ToAndroidFieldType();
                if (fieldType != FieldType.Null)
                    return new IListCursor(item.MimeType, list, elementType);
            }
            return null;
        }

        public override int Update(Android.Net.Uri uri, ContentValues values, string selection, string[] selectionArgs)
        {
            return -1;
        }


        static int GetResourceId(String pVariableName, String pResourcename, String pPackageName)
        {
            try
            {
                var result = Settings.Activity.Resources.GetIdentifier(pVariableName, pResourcename, pPackageName);
                return result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("stack: " + e.StackTrace);
                return -1;
            }
        }
    }

}
