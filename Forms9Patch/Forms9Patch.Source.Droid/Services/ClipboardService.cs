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


        #region IClipboardService
        public ClipboardEntry Entry
        {
            get
            {
                if (!Clipboard.HasPrimaryClip)
                    return null;
                var result = new ClipboardEntry();
                var description = Clipboard.PrimaryClipDescription;
                var clipData = Clipboard.PrimaryClip;

                result.Description = description.Label;

                for (int i = 0; i < clipData.ItemCount; i++)
                {
                    var item = clipData.GetItemAt(i);
                    if (!string.IsNullOrEmpty(item.HtmlText))
                        result.HtmlText = item.HtmlText;
                    if (!string.IsNullOrEmpty(item.Text))
                        result.PlainText = item.Text;

                }
                return result;
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
                if (value.AdditionalItems != null && value.AdditionalItems.Count > 0 && value is Forms9Patch.IPlatformKey platform)
                {
                    foreach (var item in value.AdditionalItems)
                    {
                        // here is where we would detect if the item is a FilePathEntryItem or the item.Type is a System.IO.File and then setup things to use a android.support.v4.content.FileProvider
                        var uri = ClipboardContentProvider.NextItemUri;
                        UriItems[uri] = item;
                        //}
                        var androidClipItem = new ClipData.Item(uri);
                        clipData.AddItem(androidClipItem);
                    }
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
            _fieldType = itemsType.GetAndroidFieldType();
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
            if (primative.GetType().GetAndroidFieldType() == FieldType.Null && primative != null)
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
            var fieldType = _primative.GetType().GetAndroidFieldType();
            if (fieldType != FieldType.Null)
                return fieldType;
            return FieldType.String;
        }

    }

    [ContentProvider(new string[] { "@string/forms9patch_copy_paste_authority" })]
    public class ClipboardContentProvider : ContentProvider
    {

        static readonly int _resourceId = GetResourceId("forms9patch_copy_paste_authority", "string", Settings.Activity.PackageName);
        public static readonly string AUTHORITY = Settings.Activity.Resources.GetString(_resourceId);
        public static readonly Android.Net.Uri CONTENT_URI = Android.Net.Uri.Parse("content://" + AUTHORITY);

        static int _index;
        public static Android.Net.Uri NextItemUri => Android.Net.Uri.Parse("content://" + AUTHORITY + "/" + _index++);

        public override int Delete(Android.Net.Uri uri, string selection, string[] selectionArgs)
        {
            return -1;
        }

        public override string GetType(Android.Net.Uri uri)
        {
            if (!ClipboardService.UriItems.ContainsKey(uri))
                return null;
            return ClipboardService.UriItems[uri].MimeType;
        }

        public override Android.Net.Uri Insert(Android.Net.Uri uri, ContentValues values)
        {
            return null;
        }

        public override bool OnCreate()
        {
            return true;
        }

        public override ICursor Query(Android.Net.Uri uri, string[] projection, string selection, string[] selectionArgs, string sortOrder)
        {
            if (!ClipboardService.UriItems.ContainsKey(uri))
                return null;
            var item = ClipboardService.UriItems[uri];
            if (item.Type.GetAndroidFieldType() != FieldType.Null)
                return new PrimativeCursor(item.MimeType, item.Item);
            if (item.Type is IList list && item.Type.IsGenericType)
            {
                var elementType = item.Type.GetElementType();
                var fieldType = elementType.GetAndroidFieldType();
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
