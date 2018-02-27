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
                    var typeString = Cursor.Extras.GetString("CSharpType");
                    Type type = null;
                    if (!string.IsNullOrWhiteSpace(typeString))
                        type = Type.GetType(typeString);
                    if (Cursor.Count == 1)
                        return GetCursorItem(0, type);
                    if (type != null)
                    {
                        var ilist = (IList)Activator.CreateInstance(type);
                        var itemsTypeString = Cursor.Extras.GetString("CSharpItemsType");
                        var itemsType = Type.GetType(itemsTypeString);
                        for (int i = 0; i < Cursor.Count; i++)
                            ilist.Add(GetCursorItem(i, itemsType));
                        return ilist;
                    }
                    try
                    {
                        var ilist = new List<object>();
                        var fieldTypes = new List<FieldType>();
                        for (int i = 0; i < Cursor.Count; i++)
                        {
                            ilist.Add(GetCursorItem(i));
                            fieldTypes.Add(Cursor.GetType(i));
                        }
                        bool allSame = true;
                        FieldType fieldType = fieldTypes[0];
                        for (int i = 1; i < Cursor.Count; i++)
                            if (fieldTypes[i] != fieldType)
                            {
                                allSame = false;
                                break;
                            }
                        if (!allSame)
                            return ilist;
                        var itemType = fieldType.ToCSharpType();
                        if (itemType.IsGenericType)
                            return ilist;
                        var listType = typeof(List<>);
                        var constructedListType = listType.MakeGenericType(itemType);
                        var result = (IList)Activator.CreateInstance(constructedListType);
                        foreach (var item in ilist)
                            result.Add(item);
                        return result;
                    }
                    catch (Exception)
                    {

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
                var typeString = Cursor.Extras.GetString("CSharpType");
                if (!string.IsNullOrWhiteSpace(typeString))
                    return Type.GetType(typeString);

                if (Cursor.ColumnCount == 0)
                    return null;

                if (Cursor.ColumnCount == 1)
                {
                    if (Cursor.Count == 0)
                        return null;
                    if (Cursor.Count == 1)
                        return Cursor.GetType(0).ToCSharpType();
                    var fieldTypes = new List<FieldType>();
                    for (int i = 0; i < Cursor.Count; i++)
                        fieldTypes.Add(Cursor.GetType(i));
                    bool allSame = true;
                    FieldType fieldType = fieldTypes[0];
                    for (int i = 1; i < Cursor.Count; i++)
                        if (fieldTypes[i] != fieldType)
                        {
                            allSame = false;
                            break;
                        }
                    if (!allSame)
                        return typeof(object);
                    return fieldType.ToCSharpType();
                }

                return null;
            }

            object GetCursorItem(int index, Type type = null)
            {
                Cursor.MoveToPosition(index);
                if (type == typeof(bool))
                    return Cursor.GetInt(index) == 1;
                if (type == typeof(char))
                    return (char)Cursor.GetShort(index);
                if (type == typeof(short))
                    return Cursor.GetShort(index);
                if (type == typeof(long))
                    return (long)Cursor.GetLong(index);
                switch (Cursor.GetType(index))
                {
                    case FieldType.Blob:
                        var blob = Cursor.GetBlob(index);
                        if (blob.Length == 1)
                        {
                            if (type == typeof(sbyte))
                                return (sbyte)blob[0];
                            return blob[0];
                        }
                        return blob;
                    case FieldType.Float:
                        return Cursor.GetDouble(index);
                    case FieldType.Integer:
                        return Cursor.GetInt(index);
                    case FieldType.Null:
                        return null;
                    case FieldType.String:
                        return Cursor.GetString(index);
                }
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
            Bundle bundle = new Bundle();
            bundle.PutString("CSharpType", list.GetType().FullName);
            bundle.PutString("CSharpItemsType", itemsType.FullName);
            Extras = bundle;
            if (itemsType == typeof(byte) || itemsType == typeof(sbyte))
            {
                _list = new List<byte[]>();
                foreach (byte item in list)
                    _list.Add(new byte[] { (byte)item });
            }
        }

        public override int Count => _list.Count;

        public override string[] GetColumnNames()
        {
            return new string[] { _itemsType.FullName };
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
            if (_fieldType == FieldType.Integer)
                return (int)_list[Position];
            throw new InvalidCastException("itemsType [" + _itemsType + "] cannot be cast to int");
        }

        public override long GetLong(int column)
        {
            return (long)_list[Position];
        }

        public override short GetShort(int column)
        {
            return (short)_list[Position];
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

        public override FieldType GetType(int column)
        {
            if (_itemsType == null)
                return FieldType.Null;
            var fieldType = _itemsType.ToAndroidFieldType();
            if (fieldType != FieldType.Null)
                return fieldType;
            return FieldType.String;
        }
    }

    class PrimativeCursor : AbstractCursor
    {
        readonly object _primative;
        readonly Type _primativeType;
        readonly string _mimeType;

        public PrimativeCursor(string mimeType, object primative)
        {
            _primative = primative;
            _primativeType = primative.GetType();
            Bundle bundle = new Bundle();
            bundle.PutString("CSharpType", _primativeType.FullName);
            Extras = bundle;
            /*
            if (_primativeType == typeof(bool))
            {
                var byteValue = (byte)(((bool)_primative) ? 0x1 : 0x0);
                _primative = new byte[] { (byte)primative };
            }
            */
            if (_primativeType == typeof(byte) || _primativeType == typeof(sbyte))
            {
                var byteValue = (byte)primative;
                _primative = new byte[] { (byte)primative };
            }
            /*
            else if (_primativeType == typeof(char))
            {
                var charValue = (char)primative;
                _primative = BitConverter.GetBytes(charValue);
            }
            else if (_primativeType == typeof(short) || _primativeType == typeof(ushort))
            {
                var shortValue = (short)primative;
                _primative = BitConverter.GetBytes(shortValue);
            }
            */
            if (primative.GetType().ToAndroidFieldType() == FieldType.Null && primative != null)
            {
                throw new InvalidDataException("PrimativeCursor does not work with [" + primative.GetType() + "]");
            }
        }

        public override int Count => 1;

        public override int ColumnCount => 1;


        public override string[] GetColumnNames()
        {
            return new string[] { _primativeType.FullName };
        }

        public override double GetDouble(int column)
        {
            return Convert.ToDouble(_primative);
        }

        public override float GetFloat(int column)
        {
            return Convert.ToSingle(_primative);
        }

        public override int GetInt(int column)
        {
            var result = Convert.ToInt32(_primative);
            return Convert.ToInt32(_primative);
        }

        public override long GetLong(int column)
        {
            return Convert.ToInt64(_primative);
        }

        public override short GetShort(int column)
        {
            return Convert.ToInt16(_primative);
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
                var elementType = item.Type.GenericTypeArguments[0];
                //var fieldType = elementType.ToAndroidFieldType();
                //if (fieldType != FieldType.Null)
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
