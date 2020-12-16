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
    /*
    class ReturnMimeItem<T> : IMimeItem<T>
    {
        ReturnMimeItem _source;

        public string MimeType => _source.MimeType;
        public T Value => (T)_source.Value;
        public Type Type => _source.Type;

        object IMimeItem.Value => _source.Value;

        public ReturnMimeItem(ReturnMimeItem source)
        {
            _source = source;
        }
    }
    */

    static class ReturnMimeItem
    {
        public static IMimeItem Parse(ClipData.Item item)
        {
            if (item.Uri != null && new AndroidUriMimeItem(item.Uri) is IMimeItem result && result.MimeType != null)
                return result;
            if (item.HtmlText != null)
                return new AndroidHtmlMimeItem(item.HtmlText);
            if (item.TextFormatted != null && item.CoerceToHtmlText(Forms9Patch.Droid.Settings.Context) is string html)
                return new AndroidHtmlMimeItem(html);
            if (item.Text != null)
                return new AndroidTextMimeItem(item.Text);
            if (item.Intent != null)
                return new AndroidIntentMimeItem(item.Intent);
            return null;
        }
    }

    class AndroidIntentMimeItem : IMimeItem
    {
        readonly Intent _intent;

        public AndroidIntentMimeItem(Intent intent)
        {
            _intent = intent;
            //var extas = _intent.Extras;
        }

        public string MimeType => _intent.Type;

        public object Value
        {
            get
            {
                if (_intent.Extras.Get(Intent.ExtraStream) is Android.Net.Uri uri)
                {
                    if (uri.Scheme == "file")
                        return File.ReadAllBytes(uri.Path);
                    return uri.Path;
                }
                //System.Diagnostics.Debug.WriteLine("_intent.Extras.Get(Itent.ExtraStream)=[" + _intent.Extras.Get(Intent.ExtraStream + "]"));
                return null;
            }
        }
    }

    class AndroidTextMimeItem : IMimeItem
    {
        public string MimeType => "text/plain";

        readonly string _value;
        public object Value => _value;

        public AndroidTextMimeItem(string text)
        {
            _value = text;
        }
    }

    class AndroidHtmlMimeItem : IMimeItem
    {
        public string MimeType => "text/html";

        readonly string _value;
        public object Value => _value;

        public AndroidHtmlMimeItem(string htmlText)
        {
            _value = htmlText;
        }
    }

    class AndroidUriMimeItem : IMimeItem
    {

        public string MimeType { get; internal set; }

        ICursor _cursor;
        ICursor Cursor
        {
            get
            {
                if (_cursor == null)
                {
                    try
                    {
                        
                        if (Android.OS.Build.VERSION.SdkInt <= BuildVersionCodes.P)
                        {
#pragma warning disable CS0618 // Type or member is obsolete (addressed below)
                            var loader = new CursorLoader(Settings.Activity, _uri, null, null, null, null);
#pragma warning restore CS0618 // Type or member is obsolete
                            _cursor = (ICursor)loader.LoadInBackground();
                        }
                        else
                        {
                            var loader = new AndroidX.Loader.Content.CursorLoader(Settings.Context, _uri, null, null, null, null);   
                            _cursor = (ICursor)loader.LoadInBackground();
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception: " + e.Message);
                        return null;
                    }
                }
                return _cursor;
            }
        }

        object _value;
        public object Value
        {
            get
            {
                if (_value != null)
                    return _value;

                if (Cursor == null || Cursor.Count == 0 || Cursor.ColumnCount == 0)
                    return null;

                var typeString = Cursor.Extras.GetString("CSharpType");
                Type type = null;
                if (!string.IsNullOrWhiteSpace(typeString))
                    type = Type.GetType(typeString);
                if (type == null)
                    type = DetermineType();

                if (Cursor.Count == 1)
                {
                    if (Cursor.ColumnCount == 1)
                        return _value = GetCursorItem(0, 0, type);
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
                    return _value = dictionary;
                }

                var list = (IList)Activator.CreateInstance(type);
                if (Cursor.ColumnCount == 1)
                {
                    Type valueType = null;
                    if (type.IsGenericType)
                        valueType = type.GetGenericArguments()[0];
                    for (int i = 0; i < Cursor.Count; i++)
                        list.Add(GetCursorItem(i, 0, valueType));
                    return _value = list;
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
                        return _value = list;
                    }
                }
                throw new Exception("[" + type + "] was not parcable.");
            }
            internal set => _value = value;
        }

        /*
        Type _type;
        public Type Type
        {
            get
            {
                _type = _type ?? DetermineType();
                return _type;
            }
            internal set => _type = value;
        }
        */

        readonly Android.Net.Uri _uri;

        public AndroidUriMimeItem() { }

        public AndroidUriMimeItem(Android.Net.Uri uri)
        {
            _uri = uri;
            MimeType = Settings.Activity.ContentResolver.GetType(uri);
        }

        Type DetermineType()
        {
            var typeString = Cursor.Extras.GetString("CSharpType");
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

}