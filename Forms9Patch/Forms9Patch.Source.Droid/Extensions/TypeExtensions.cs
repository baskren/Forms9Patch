using System;
using Android.Content;
using Android.Database;

namespace Forms9Patch.Droid
{
    static class TypeExtensions
    {
        public static FieldType GetAndroidFieldType(Type type)
        {
            if (type == typeof(byte[]))
                return FieldType.Blob;
            if (type == typeof(double) || type == typeof(float) || type == typeof(decimal))
                return FieldType.Float;
            if (type == typeof(byte) || type == typeof(char) || type == typeof(short) || type == typeof(long) || type == typeof(int) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong))
                return FieldType.Integer;
            if (type == typeof(string))
                return FieldType.String;
            return FieldType.Null;
        }
    }

}