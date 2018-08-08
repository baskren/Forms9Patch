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

[assembly: Dependency(typeof(Forms9Patch.Droid.ClipboardService))]
namespace Forms9Patch.Droid
{
    public class ClipboardService : Forms9Patch.IClipboardService
    {

        #region Static implementation
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

        public ClipboardService()
        {
            Clipboard.PrimaryClipChanged += (sender, e) =>
            {
                if (!_lastChangedByThis)
                    _lastEntry = null;
                _lastChangedByThis = false;
                Forms9Patch.Clipboard.OnContentChanged(this, EventArgs.Empty);
            };
        }

        public bool EntryCaching { get; set; } = true;
        static bool _entryItemTypeCaching = true;
        public bool EntryItemTypeCaching
        {
            get => _entryItemTypeCaching;
            set => _entryItemTypeCaching = value;
        }

        #region Entry
        IClipboardEntry _lastEntry;
        bool _lastChangedByThis;
        public IClipboardEntry Entry
        {
            get
            {
                if (!Clipboard.HasPrimaryClip)
                    return null;

                return new ClipboardEntry();
            }
            set
            {
                ClipboardContentProvider.Clear();
                ClipData clipData = null;
                if (value is Forms9Patch.ClipboardEntry entry)
                {
                    foreach (var item in entry.Items)
                    {
                        var androidClipItem = ClipboardContentProvider.Add(item.Value);
                        if (clipData == null)
                            clipData = new ClipData(value.Description, entry.MimeTypes.ToArray(), androidClipItem);
                        else
                            clipData.AddItem(androidClipItem);
                    }
                }
                _lastEntry = value;
                _lastChangedByThis = true;
                Clipboard.PrimaryClip = clipData;
            }
        }
        #endregion
    }



    #region ContentProvider

    [ContentProvider(new string[] { "@string/forms9patch_copy_paste_authority" })]
    public class ClipboardContentProvider : ContentProvider
    {
        // Here is where we hold our items, waiting for someone to retrieve them.
        readonly internal static Dictionary<Android.Net.Uri, Forms9Patch.IMimeItem> UriItems = new Dictionary<Android.Net.Uri, IMimeItem>();


        public static void Clear() => UriItems.Clear();

        public static ClipData.Item Add(IMimeItem mimeItem)
        {
            var uri = NextItemUri;
            UriItems[uri] = mimeItem;
            return new ClipData.Item(uri);
        }


        static Forms9Patch.IMimeItem ItemForUri(Android.Net.Uri uri)
        {
            foreach (var key in UriItems.Keys)
                if (key.Equals(uri))
                    return UriItems[key];
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
                return new PrimativeCursor(item.Value);
            if (item.Value is IList list && item.Type.IsGenericType)
            {
                var elementType = item.Type.GenericTypeArguments[0];
                //var fieldType = elementType.ToAndroidFieldType();
                //if (fieldType != FieldType.Null)
                return new IListCursor(item.MimeType, list, elementType);
            }
            if (item.Value is IDictionary dictionary && item.Type.IsGenericType)
                return new PrimativeCursor(item.Value);
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
    #endregion

    #region Cursors
    class IListCursor : AbstractCursor
    {
        readonly IList _list;
        string _mimeType;
        FieldType _fieldType;
        readonly Type _itemsType;
        readonly Type _keyType;
        readonly Type _valueType;

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
            else if (itemsType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IDictionary)))
            {
                if (!itemsType.IsGenericType)
                    throw new Exception("Generic IDictionary is the only allowed IDictionary to be items in IListCursor");
                _keyType = itemsType.GetGenericArguments()[0];
                _valueType = itemsType.GetGenericArguments()[1];
                _fieldType = _valueType.ToAndroidFieldType();
                if (_keyType != typeof(string))
                    throw new Exception("Generic IDictionary with typeof(string) keys are the only allowed IDictionary to be the items in IListCursor");
            }
        }

        public override int Count => _list.Count;

        public override int ColumnCount => _keyType == null ? 1 : ((IDictionary)_list[0]).Keys.Count;

        public override string[] GetColumnNames()
        {
            if (_keyType == null)
                return new string[] { _itemsType.FullName };
            var result = new List<string>();
            foreach (var key in ((IDictionary)_list[0]).Keys)
                result.Add(key.ToString());
            return result.ToArray();
        }

        public override double GetDouble(int column)
        {
            if (_fieldType == FieldType.Float || _fieldType == FieldType.Integer)
            {
                if (_keyType == null)
                    return (double)_list[Position];
                int index = 0;
                foreach (var value in ((IDictionary)_list[Position]).Values)
                    if (column == index++)
                        return Convert.ToDouble(value);
                throw new Exception("Invalid column index (" + column + ") for IDictionary at Position (" + Position + ")");
            }
            throw new InvalidCastException("itemsType [" + _itemsType + "] cannot be cast to double");
        }

        public override float GetFloat(int column)
        {
            if (_fieldType == FieldType.Float || _fieldType == FieldType.Integer)
            {
                if (_keyType == null)
                    return (float)_list[Position];
                int index = 0;
                foreach (var value in ((IDictionary)_list[Position]).Values)
                    if (column == index++)
                        return Convert.ToSingle(value);
                throw new Exception("Invalid column index (" + column + ") for IDictionary at Position (" + Position + ")");
            }
            throw new InvalidCastException("itemsType [" + _itemsType + "] cannot be cast to float");
        }

        public override int GetInt(int column)
        {
            if (_fieldType == FieldType.Integer)
            {
                if (_keyType == null)
                    return (int)_list[Position];
                int index = 0;
                foreach (var value in ((IDictionary)_list[Position]).Values)
                    if (column == index++)
                        return Convert.ToInt32(value);
                throw new Exception("Invalid column index (" + column + ") for IDictionary at Position (" + Position + ")");
            }
            throw new InvalidCastException("itemsType [" + _itemsType + "] cannot be cast to int");
        }

        public override long GetLong(int column)
        {
            if (_keyType == null)
                return (long)_list[Position];
            int index = 0;
            foreach (var value in ((IDictionary)_list[Position]).Values)
                if (column == index++)
                    return Convert.ToInt64(value);
            throw new Exception("Invalid column index (" + column + ") for IDictionary at Position (" + Position + ")");
        }

        public override short GetShort(int column)
        {
            if (_keyType == null)
                return (short)_list[Position];
            int index = 0;
            foreach (var value in ((IDictionary)_list[Position]).Values)
                if (column == index++)
                    return Convert.ToInt16(value);
            throw new Exception("Invalid column index (" + column + ") for IDictionary at Position (" + Position + ")");
        }

        public override string GetString(int column)
        {
            if (_keyType == null)
            {
                if (_fieldType == FieldType.String)
                    return (string)_list[Position];
                return _list[Position].ToString();
            }
            int index = 0;
            foreach (var value in ((IDictionary)_list[Position]).Values)
                if (column == index++)
                    return value.ToString();
            throw new Exception("Invalid column index (" + column + ") for IDictionary at Position (" + Position + ")");
        }

        public override bool IsNull(int column)
        {
            if (_keyType == null)
                return _list[Position] == null;
            int index = 0;
            foreach (var value in ((IDictionary)_list[Position]).Values)
                if (column == index++)
                    return value == null;
            throw new Exception("Invalid column index (" + column + ") for IDictionary at Position (" + Position + ")");
        }

        public override FieldType GetType(int column)
        {
            if (_keyType == null)
            {
                if (_itemsType == null)
                    return FieldType.Null;
                var fieldType = _itemsType.ToAndroidFieldType();
                if (fieldType != FieldType.Null)
                    return fieldType;
                return FieldType.String;
            }
            int index = 0;
            foreach (var value in ((IDictionary)_list[Position]).Values)
                if (column == index++)
                {
                    var fieldType = value.GetType().ToAndroidFieldType();
                    if (fieldType != FieldType.Null)
                        return fieldType;
                    return FieldType.String;
                }
            throw new Exception("Invalid column index (" + column + ") for IDictionary at Position (" + Position + ")");
        }
    }

    class PrimativeCursor : AbstractCursor
    {
        readonly object _primative;
        readonly Type _primativeType;
        readonly Type _keyType;
        readonly Type _valueType;
        readonly IDictionary _dictionary;

        public PrimativeCursor(object primative)
        {
            _primative = primative;
            _primativeType = primative.GetType();
            Bundle bundle = new Bundle();
            bundle.PutString("CSharpType", _primativeType.FullName);
            Extras = bundle;
            if (_primativeType == typeof(byte) || _primativeType == typeof(sbyte))
            {
                var byteValue = (byte)primative;
                _primative = new byte[] { (byte)primative };
            }
            if (_primative is IDictionary dictionary && _primativeType.IsGenericType)
            {
                _dictionary = dictionary;
                _keyType = _primativeType.GetGenericArguments()[0];
                _valueType = _primativeType.GetGenericArguments()[1];
                if (_valueType.ToAndroidFieldType() == FieldType.Null)
                    throw new InvalidDataException("PrimativeCursor does not work with [" + _valueType.GetType() + "]");
            }
            else if (primative.GetType().ToAndroidFieldType() == FieldType.Null && primative != null)
                throw new InvalidDataException("PrimativeCursor does not work with [" + primative.GetType() + "]");
        }

        public override int Count => 1;

        public override int ColumnCount => _dictionary == null ? 1 : _dictionary.Keys.Count;


        public override string[] GetColumnNames()
        {
            if (_dictionary == null)
                return new string[] { _primativeType.FullName };
            var result = new List<string>();
            foreach (var key in _dictionary.Keys)
                result.Add(key.ToString());
            return result.ToArray();
        }

        public override double GetDouble(int column)
        {
            if (_dictionary == null)
                return Convert.ToDouble(_primative);
            int index = 0;
            foreach (var value in _dictionary.Values)
                if (column == index++)
                    return Convert.ToDouble(value);
            throw new Exception("Invalid column index (" + column + ") for IDictionary");
        }

        public override float GetFloat(int column)
        {
            if (_dictionary == null)
                return Convert.ToSingle(_primative);
            int index = 0;
            foreach (var value in _dictionary.Values)
                if (column == index++)
                    return Convert.ToSingle(value);
            throw new Exception("Invalid column index (" + column + ") for IDictionary");
        }

        public override int GetInt(int column)
        {
            if (_dictionary == null)
                return Convert.ToInt32(_primative);
            int index = 0;
            foreach (var value in _dictionary.Values)
                if (column == index++)
                    return Convert.ToInt32(value);
            throw new Exception("Invalid column index (" + column + ") for IDictionary");
        }

        public override long GetLong(int column)
        {
            if (_dictionary == null)
                return Convert.ToInt64(_primative);
            int index = 0;
            foreach (var value in _dictionary.Values)
                if (column == index++)
                    return Convert.ToInt64(value);
            throw new Exception("Invalid column index (" + column + ") for IDictionary");
        }

        public override short GetShort(int column)
        {
            if (_dictionary == null)
                return Convert.ToInt16(_primative);
            int index = 0;
            foreach (var value in _dictionary.Values)
                if (column == index++)
                    return Convert.ToInt16(value);
            throw new Exception("Invalid column index (" + column + ") for IDictionary");
        }

        public override string GetString(int column)
        {
            if (_primative is string str)
                return str;
            if (_dictionary == null)
                return _primative.ToString();
            int index = 0;
            foreach (var value in _dictionary.Values)
                if (column == index++)
                    return value.ToString();
            throw new Exception("Invalid column index (" + column + ") for IDictionary");
        }

        public override bool IsNull(int column)
        {
            if (_dictionary == null)
                return _primative == null;
            int index = 0;
            foreach (var value in _dictionary.Values)
                if (column == index++)
                    return value == null;
            throw new Exception("Invalid column index (" + column + ") for IDictionary");
        }

        public override byte[] GetBlob(int column)
        {
            if (_primative is byte[] byteArray)
                return byteArray;
            if (_dictionary == null)
                return base.GetBlob(column);
            throw new Exception("Invalid column index (" + column + ") for IDictionary");
        }

        public override FieldType GetType(int column)
        {
            if (_primative == null)
                return FieldType.Null;
            if (_dictionary == null)
            {
                var fieldType = _primative.GetType().ToAndroidFieldType();
                if (fieldType != FieldType.Null)
                    return fieldType;
            }
            int index = 0;
            foreach (var value in _dictionary.Values)
                if (column == index++)
                {
                    var fieldType = value.GetType().ToAndroidFieldType();
                    if (fieldType != FieldType.Null)
                        return fieldType;
                }
            return FieldType.String;
        }

    }
    #endregion


}
