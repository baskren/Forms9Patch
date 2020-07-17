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
using Android.Content.Res;
using Android.Content.PM;
using Java.IO;
using P42.Utils;

[assembly: Dependency(typeof(Forms9Patch.Droid.ClipboardService))]
namespace Forms9Patch.Droid
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
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
                if (!_locallyUpdated)
                    _lastEntry = null;
                _locallyUpdated = false;
                Forms9Patch.Clipboard.OnContentChanged(this, EventArgs.Empty);
            };
        }

        public bool EntryCaching { get; set; } = false;

        // note: 
        // There is a HUGE lag between when Clipboard.PrimaryClip is set and when 
        // Clipboard.PrimaryClipChanged is fired.  Because of this lag, we need to 
        // know when we've updated locally AND captured the new Clipboard 
        // (via new Forms9Patch.Droid.ClipboardEntry())
        // before Clipboard.PrimaryClipChanged is fired so that we don't whipe 
        // _lastEntry and instantiate another Forms9Patch.Droid.ClipboardEntry.
        bool _locallyUpdated;
        /*
        static bool _entryItemTypeCaching = true;
        public bool EntryItemTypeCaching
        {
            get => _entryItemTypeCaching;
            set => _entryItemTypeCaching = value;
        }
        */

        #region Entry
        IMimeItemCollection _lastEntry;
        public IMimeItemCollection Entry
        {
            get
            {
                return _lastEntry = _lastEntry ?? new Forms9Patch.Droid.MimeItemCollection();
            }
            set
            {
                ClipData clipData = null;

                var items = value.AsClipDataItems();

                /*
                string text = null;
                string html = null;
                foreach (var item in value.Items)
                {
                    if (text == null && item.MimeType == "text/plain")
                        text = (string)item.Value;
                    if (html == null && item.MimeType == "text/html")
                        html = (string)item.Value;
                }
*/


                if (items.Count > 0)
                {
                    /*
                    //clipData = new ClipData(value.Description, value.MimeTypes().ToArray(), items[0]);
                    if (html != null)
                        clipData = ClipData.NewHtmlText(value.Description, text ?? html, html);
                    else if (text != null)
                        clipData = ClipData.NewPlainText(value.Description, text);
                    else
                        */
                    clipData = new ClipData(value.Description, value.MimeTypes().ToArray(), items[0]);
                    if (items.Count > 1)
                    {
                        for (int i = 1; i < items.Count; i++)
                            clipData.AddItem(items[i]);
                    }

                }

                _lastEntry = EntryCaching ? value : null;
                _locallyUpdated = true;
                Clipboard.PrimaryClip = clipData ?? ClipData.NewPlainText("", "");
            }
        }
        #endregion
    }


    #region ContentProvider

    //[ContentProvider(new string[] { "${applicationId}.f9pclipboardcontentprovider.authority" }, Enabled = true, Exported = true, GrantUriPermissions = true, Label = "F9P Clipboard", Name = "${applicationId}.f9pclipboardcontentprovider", MultiProcess = true)]
    [ContentProvider(new string[] { "${applicationId}.f9pclipboardcontentprovider.authority" }, Enabled = true, Exported = true, GrantUriPermissions = true)]
    public class ClipboardContentProvider : ContentProvider
    {
        // Here is where we hold our items, waiting for someone to retrieve them.
        readonly internal static Dictionary<Android.Net.Uri, Forms9Patch.IMimeItem> UriItems = new Dictionary<Android.Net.Uri, IMimeItem>();

        public static void Clear() => UriItems.Clear();

        public static ClipData.Item AddAsClipDataItem(IMimeItem mimeItem)
        {
            var uri = NextItemUri;
            UriItems[uri] = mimeItem;
            return new ClipData.Item(uri);
        }

        public static Android.Net.Uri AsAsAndroidUri(IMimeItem mimeItem)
        {
            Android.Net.Uri uri = null;
            if (mimeItem.Value != null)
            {
                uri = NextItemUri;
                UriItems[uri] = mimeItem;
            }
            return uri;
        }


        static Forms9Patch.IMimeItem ItemForUri(Android.Net.Uri uri)
        {
            foreach (var key in UriItems.Keys)
                if (key.Equals(uri))
                    return UriItems[key];
            return null;
        }


        public static string AUTHORITY => Settings.Activity.PackageName + ".f9pclipboardcontentprovider.authority";

        public static Android.Net.Uri CONTENT_URI => Android.Net.Uri.Parse("content://" + AUTHORITY);

        static int _index;
        public static Android.Net.Uri NextItemUri => Android.Net.Uri.Parse("content://" + AUTHORITY + "/" + _index++);

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
            if (item == null || item.Value == null)
                return null;
            if (item.Value is FileInfo)
                return new PrimativeCursor(item.Value);
            var type = item.Value.GetType();

            if (type.ToAndroidFieldType() != FieldType.Null)
                return new PrimativeCursor(item.Value);
            if (item.Value is IList list && type.IsGenericType)
            {
                var elementType = type.GenericTypeArguments[0];
                //var fieldType = elementType.ToAndroidFieldType();
                //if (fieldType != FieldType.Null)
                return new IListCursor(item.MimeType, list, elementType);
            }
            return item.Value is IDictionary && type.IsGenericType ? new PrimativeCursor(item.Value) : null;
        }

        public override ICursor Query(Android.Net.Uri uri, string[] projection, Bundle queryArgs, CancellationSignal cancellationSignal)
        {
            var result = base.Query(uri, projection, queryArgs, cancellationSignal);
            return result;
        }

        public override ICursor Query(Android.Net.Uri uri, string[] projection, string selection, string[] selectionArgs, string sortOrder, CancellationSignal cancellationSignal)
        {
            var result = base.Query(uri, projection, selection, selectionArgs, sortOrder, cancellationSignal);
            return result;
        }

        public override int Update(Android.Net.Uri uri, ContentValues values, string selection, string[] selectionArgs)
        {
            return -1;
        }

        public override Android.Net.Uri Canonicalize(Android.Net.Uri url)
        {
            return base.Canonicalize(url);
        }

        public override string[] GetStreamTypes(Android.Net.Uri uri, string mimeTypeFilter)
        {
            //var result = base.GetStreamTypes(uri, mimeTypeFilter);
            var result = new List<string>();
            foreach (var item in UriItems.Values)
                if (!result.Contains(item.MimeType))
                    result.Add(item.MimeType);
            return result.ToArray();
        }

        protected override bool IsTemporary => true;

        public override ContentProviderResult[] ApplyBatch(IList<ContentProviderOperation> operations)
        {
            return base.ApplyBatch(operations);
        }

        public override void AttachInfo(Context context, ProviderInfo info)
        {
            /*
            var appInfo = info.ApplicationInfo;
            var enabled = info.Enabled;
            var exported = info.Exported;
            var uriPermissions = info.GrantUriPermissions;
            var authority = info.Authority;
            var name = info.Name;
            */
            base.AttachInfo(context, info);
        }

        public override int BulkInsert(Android.Net.Uri uri, ContentValues[] values)
        {
            var result = base.BulkInsert(uri, values);
            return result;
        }

        public override Bundle Call(string method, string arg, Bundle extras)
        {
            var result = base.Call(method, arg, extras);
            return result;
        }

        public override void Dump(FileDescriptor fd, PrintWriter writer, string[] args)
        {
            base.Dump(fd, writer, args);
        }

        public override ParcelFileDescriptor OpenPipeHelper(Android.Net.Uri uri, string mimeType, Bundle opts, Java.Lang.Object args, IPipeDataWriter func)
        {
            System.Diagnostics.Debug.WriteLine("OpenPipeHelper");
            var result = base.OpenPipeHelper(uri, mimeType, opts, args, func);
            return result;
        }

        public override void Shutdown()
        {
            System.Diagnostics.Debug.WriteLine("Shutdown");
            base.Shutdown();
        }

        public override AssetFileDescriptor OpenTypedAssetFile(Android.Net.Uri uri, string mimeTypeFilter, Bundle opts)
        {
            System.Diagnostics.Debug.WriteLine("OpenTypedAssetFile A");
            AssetFileDescriptor result = null;
            try
            {
                result = base.OpenTypedAssetFile(uri, mimeTypeFilter, opts);
            }
            catch (Exception) { }

            if (result == null && mimeTypeFilter == "text/*")
            {
                try
                {
                    var item = ItemForUri(uri);
                    result = base.OpenTypedAssetFile(uri, item.MimeType, opts);
                }
                catch (Exception) { }
            }

            return result;
        }

        public override AssetFileDescriptor OpenTypedAssetFile(Android.Net.Uri uri, string mimeTypeFilter, Bundle opts, CancellationSignal signal)
        {
            System.Diagnostics.Debug.WriteLine("OpenTypedAssetFile B");
            var result = base.OpenTypedAssetFile(uri, mimeTypeFilter, opts, signal);
            return result;
        }


        public override AssetFileDescriptor OpenAssetFile(Android.Net.Uri uri, string mode)
        {
            System.Diagnostics.Debug.WriteLine("OpenAssetFile A");
            var result = base.OpenAssetFile(uri, mode);
            return result;
        }

        public override AssetFileDescriptor OpenAssetFile(Android.Net.Uri uri, string mode, CancellationSignal signal)
        {
            System.Diagnostics.Debug.WriteLine("OpenAssetFile B");
            var result = base.OpenAssetFile(uri, mode, signal);
            return result;
        }

        public override ParcelFileDescriptor OpenFile(Android.Net.Uri uri, string mode)
        {

            System.Diagnostics.Debug.WriteLine("OpenFile A");

            var item = ItemForUri(uri);
            if (item == null || item.Value == null)
                return null;

            var parcelFileMode = mode.Equals("rw", StringComparison.OrdinalIgnoreCase) ? ParcelFileMode.ReadWrite : ParcelFileMode.ReadOnly;
            Java.IO.File javaFile;
            if (item.Value is System.Uri itemUri && itemUri.AbsoluteUri.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
                javaFile = new Java.IO.File(itemUri.AbsolutePath);
            else if (item.Value is FileInfo fileInfo)
                javaFile = new Java.IO.File(fileInfo.FullName);
            else
            {
                var tempGuid = Guid.NewGuid().ToString();
                var path = Path.Combine(P42.Utils.Environment.TemporaryStoragePath, tempGuid);
                var type = item.Value.GetType();
                if (type == typeof(byte[]))
                    System.IO.File.WriteAllBytes(path, (byte[])item.Value);
                else if (type.IsSimple())
                    System.IO.File.WriteAllText(path, item.Value.ToString());
                else
                    System.IO.File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(item.Value));
                javaFile = new Java.IO.File(path);
            }

            var pfd = ParcelFileDescriptor.Open(javaFile, parcelFileMode);
            return pfd;
        }

        public override ParcelFileDescriptor OpenFile(Android.Net.Uri uri, string mode, CancellationSignal signal)
        {
            System.Diagnostics.Debug.WriteLine("OpenFile B");
            var result = base.OpenFile(uri, mode, signal);
            return result;
        }

        public override Android.Net.Uri Uncanonicalize(Android.Net.Uri url)
        {
            var result = base.Uncanonicalize(url);
            return result;
        }

        public override bool Refresh(Android.Net.Uri uri, Bundle args, CancellationSignal cancellationSignal)
        {
            var result = base.Refresh(uri, args, cancellationSignal);
            return result;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
        }
        /*
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
        */
    }
    #endregion


    #region Cursors
    class IListCursor : AbstractCursor
    {
        readonly IList _list;
        readonly string _mimeType;
        readonly FieldType _fieldType;
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
                return _fieldType == FieldType.String ? (string)_list[Position] : _list[Position].ToString();
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
                return fieldType != FieldType.Null ? fieldType : FieldType.String;
            }
            int index = 0;
            foreach (var value in ((IDictionary)_list[Position]).Values)
                if (column == index++)
                {
                    var fieldType = value.GetType().ToAndroidFieldType();
                    return fieldType != FieldType.Null ? fieldType : FieldType.String;
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
                //var byteValue = (byte)primative;
                _primative = new byte[] { (byte)primative };
            }
            if (_primative is IDictionary dictionary && _primativeType.IsGenericType)
            {
                _dictionary = dictionary;
                _keyType = _primativeType.GetGenericArguments()[0];
                _valueType = _primativeType.GetGenericArguments()[1];
                if (_valueType.ToAndroidFieldType() == FieldType.Null)
#pragma warning disable RECS0035 // Possible mistaken call to 'object.GetType()'
                    throw new InvalidDataException("PrimativeCursor does not work with [" + _valueType.GetType() + "]");
#pragma warning restore RECS0035 // Possible mistaken call to 'object.GetType()'
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
            if (_primative is FileInfo fileInfo)
                return System.IO.File.ReadAllBytes(fileInfo.FullName);
            if (_primative is byte[] byteArray)
                return byteArray;
            if (_dictionary == null)
                return base.GetBlob(column);
            throw new Exception("Invalid column index (" + column + ") for IDictionary");
        }

        public override FieldType GetType(int column)
        {
            if (_primative is FileInfo)
                return typeof(byte[]).ToAndroidFieldType();
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
