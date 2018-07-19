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
        #endregion

        #region ReturnClipboardEntryItem

        class ReturnClipboardEntryItem<T> : IMimeItem<T>
        {
            ReturnClipboardEntryItem _source;

            public string MimeType => _source.MimeType;
            public T Value => (T)_source.Value;
            public Type Type => _source.Type;

            object IMimeItem.Value => _source.Value;

            public ReturnClipboardEntryItem(ReturnClipboardEntryItem source)
            {
                _source = source;
            }
        }

        class ReturnClipboardEntryItem : IMimeItem
        {
            public string MimeType { get; protected set; }

            ICursor _cursor;
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
                    if (Cursor.Count == 0 || Cursor.ColumnCount == 0)
                        return null;

                    var typeString = _entryItemTypeCaching ? Cursor.Extras.GetString("CSharpType") : null;
                    Type type = null;
                    if (!string.IsNullOrWhiteSpace(typeString))
                        type = Type.GetType(typeString);
                    if (type == null)
                        type = DetermineType();

                    if (Cursor.Count == 1)
                    {
                        if (Cursor.ColumnCount == 1)
                            return GetCursorItem(0, 0, type);
                        var dictionary = (IDictionary)Activator.CreateInstance(type);
                        Type valueType = null;
                        if (type.IsGenericType)
                            valueType = type.GetGenericArguments()[1];
                        var columnNames = Cursor.GetColumnNames();
                        for (int c = 0; c < Cursor.ColumnCount; c++)
                        {
                            var key = columnNames[c];
                            var value = GetCursorItem(0, c, valueType);
                            dictionary.Add(key, value);
                        }
                        return dictionary;
                    }

                    var list = (IList)Activator.CreateInstance(type);
                    if (Cursor.ColumnCount == 1)
                    {
                        Type valueType = null;
                        if (type.IsGenericType)
                            valueType = type.GetGenericArguments()[0];
                        for (int i = 0; i < Cursor.Count; i++)
                            list.Add(GetCursorItem(i, 0, valueType));
                        return list;
                    }

                    if (type.IsGenericType)
                    {
                        // and it should be because type should be an IList<>
                        var itemType = type.GetGenericArguments()[0];
                        var columnNames = Cursor.GetColumnNames();
                        if (itemType.IsGenericType)
                        {
                            var valueType = itemType.GetGenericArguments()[1];
                            for (int i = 0; i < Cursor.Count; i++)
                            {
                                var dictionary = (IDictionary)Activator.CreateInstance(itemType);
                                for (int c = 0; c < Cursor.ColumnCount; c++)
                                {
                                    var key = columnNames[c];
                                    var value = GetCursorItem(i, c, valueType);
                                    dictionary.Add(key, value);
                                }
                                list.Add(dictionary);
                            }
                            return list;
                        }
                    }
                    throw new Exception("[" + type + "] was not parcable.");
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
                var typeString = _entryItemTypeCaching ? Cursor.Extras.GetString("CSharpType") : null;
                if (!string.IsNullOrWhiteSpace(typeString))
                    return Type.GetType(typeString);

                if (Cursor.ColumnCount == 0 || Cursor.Count == 0)
                    return null;

                if (Cursor.ColumnCount == 1)
                    return Cursor.GetType(0).ToCSharpType();

                Cursor.MoveToPosition(0);
                var fieldTypes = new List<FieldType>();
                for (int i = 0; i < Cursor.ColumnCount; i++)
                    fieldTypes.Add(Cursor.GetType(i));
                bool allSame = true;
                FieldType fieldType = fieldTypes[0];
                for (int i = 1; i < fieldTypes.Count; i++)
                    if (fieldTypes[i] != fieldType)
                    {
                        allSame = false;
                        break;
                    }
                var dictionaryType = typeof(Dictionary<,>);
                var constructedDictionaryType = dictionaryType.MakeGenericType(new Type[] { typeof(string), (allSame ? fieldType.ToCSharpType() : typeof(object)) });
                if (Cursor.Count == 1)
                    return constructedDictionaryType;
                var listType = typeof(List<>).MakeGenericType(constructedDictionaryType);
                return listType;
            }

            object GetCursorItem(int index, int column, Type type = null)
            {
                Cursor.MoveToPosition(index);
                if (type == typeof(bool))
                    return Cursor.GetInt(column) == 1;
                if (type == typeof(char))
                    return (char)Cursor.GetShort(column);
                if (type == typeof(short))
                    return Cursor.GetShort(column);
                if (type == typeof(long))
                    return (long)Cursor.GetLong(column);
                switch (Cursor.GetType(column))
                {
                    case FieldType.Blob:
                        var blob = Cursor.GetBlob(column);
                        if (blob.Length == 1)
                        {
                            if (type == typeof(sbyte))
                                return (sbyte)blob[0];
                            return blob[0];
                        }
                        return blob;
                    case FieldType.Float:
                        return Cursor.GetDouble(column);
                    case FieldType.Integer:
                        return Cursor.GetInt(column);
                    case FieldType.Null:
                        return null;
                    case FieldType.String:
                        return Cursor.GetString(column);
                }
                return null;
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
        ClipboardEntry _lastEntry;
        bool _lastChangedByThis;
        public ClipboardEntry Entry
        {
            get
            {
                if (EntryCaching && _lastEntry != null)
                    return _lastEntry;

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
                        var entryItemType = typeof(ReturnClipboardEntryItem<>).MakeGenericType(entryItem.Type);
                        var typedEntryItem = (IMimeItem)Activator.CreateInstance(entryItemType, new object[] { entryItem });
                        entry._item.Add(typedEntryItem);
                    }
                }

                _lastEntry = entry;
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
                foreach (var item in value._item)
                {
                    // here is where we would detect if the item is a FilePathEntryItem or the item.Type is a System.IO.File and then setup things to use a android.support.v4.content.FileProvider
                    var uri = ClipboardContentProvider.NextItemUri;
                    UriItems[uri] = item;
                    //}
                    var androidClipItem = new ClipData.Item(uri);
                    clipData.AddItem(androidClipItem);
                }

                _lastEntry = value;
                _lastChangedByThis = true;
                Clipboard.PrimaryClip = clipData;
            }
        }
        #endregion
    }



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

    #region ContentProvider
    [ContentProvider(new string[] { "@string/forms9patch_copy_paste_authority" })]
    public class ClipboardContentProvider : ContentProvider
    {
        static Forms9Patch.IMimeItem ItemForUri(Android.Net.Uri uri)
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
}
