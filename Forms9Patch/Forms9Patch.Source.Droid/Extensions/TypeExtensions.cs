using System;
using Android.Content;
using Android.Database;
using System.IO;

namespace Forms9Patch.Droid
{
    static class TypeExtensions
    {
        public static FieldType ToAndroidFieldType(this Type type)
        {
            if (type == typeof(byte[]))
                return FieldType.Blob;
            if (type == typeof(double) || type == typeof(float) || type == typeof(decimal))
                return FieldType.Float;
            if (type == typeof(byte) || type == typeof(char) || type == typeof(short) || type == typeof(long) || type == typeof(int) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong))
                return FieldType.Integer;
            if (type == typeof(string))
                return FieldType.String;
            if (type == typeof(FileInfo))
                return FieldType.Blob;
            return FieldType.Null;
        }

        public static Type ToCSharpType(this FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Blob:
                    return typeof(byte[]);
                case FieldType.Float:
                    return typeof(double);
                case FieldType.Integer:
                    return typeof(int);
                case FieldType.String:
                    return typeof(string);
            }
            return null;
        }
    }

}