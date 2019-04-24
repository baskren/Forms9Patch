using System;
using System.Drawing;
using System.Globalization;
using Foundation;
using Xamarin.Forms.Platform.iOS;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using ObjCRuntime;
using CoreFoundation;
using EventKit;
using System.Runtime.Serialization;
using System.Linq;
using System.IO;

namespace Forms9Patch.iOS
{
    public static class NSObjectExtensions
    {

        public static bool Implements(this Type type, Type requestedInterface)
        {
            var typeInfo = type.GetTypeInfo();
            foreach (var implementedInterface in typeInfo.ImplementedInterfaces)
                if (implementedInterface == requestedInterface)
                    return true;
            return false;
        }


        /*
        public static Tuple<object, Type> ToObject(this NSObject nsO) => nsO.ToObject(null);

        public static Tuple<object, Type> ToObject(this NSObject nsO, Type type)

*/
        public static object ToObject(this NSObject nsO) => nsO.ToObject(null);

        public static object ToObject(this NSObject nsO, Type type)
        {
            if (nsO is NSDictionary nsDictionary)
            {
                if (type == null)
                {
                    var result = nsDictionary.ToDictionary();
                    return result;
                }
                if (type.Implements(typeof(IDictionary)))
                {
                    IDictionary result;
                    Type genericType;
                    if (type.IsGenericType)
                    {
                        result = Activator.CreateInstance(type) as IDictionary;
                        genericType = type.GetTypeInfo().GenericTypeArguments[1];
                    }
                    else
                    {
                        result = new Dictionary<string, object>();
                        genericType = typeof(object);
                    }
                    foreach (var key in nsDictionary.Keys)
                    {
                        try
                        {
                            var nsObj = nsDictionary[key];
                            var obj = nsObj.ToObject();
                            result.Add(key.ToString(), obj);
                        }
                        catch (Exception)
                        {
                            result.Add(key.ToString(), null);
                        }
                    }
                    //return new Tuple<object, Type>(result, type);
                    return result;
                }
                //throw new InvalidDataContractException("Cannot reliablity convert NSDictionary to type [" + type + "].");
                return null;
            }

            if (nsO is NSArray nsArray)
            {
                if (type == null)
                {
                    var result = nsArray.ToList();
                    return result;
                }

                if (type.Implements(typeof(IList)))
                {
                    IList result;
                    Type genericType;
                    if (type.IsGenericType)
                    {
                        result = Activator.CreateInstance(type) as IList;
                        genericType = type.GetTypeInfo().GenericTypeArguments[0];
                    }
                    else
                    {
                        result = new List<object>();
                        genericType = null;
                    }
                    //MethodInfo method = typeof(NSArray).GetMethod("FromArray");
                    //MethodInfo genericMethod = method.MakeGenericMethod(new[] { genericType });
                    //result = (IList)genericMethod.Invoke(null, new object[] { nsArray });
                    for (nuint i = 0; i < nsArray.Count; i++)
                    {
                        try
                        {
                            var nsObj = nsArray.GetItem<NSObject>(i);
                            var obj = nsObj.ToObject(genericType);
                            result.Add(obj);
                        }
                        catch (Exception)
                        {
                            result.Add(null);
                        }
                    }
                    //return new Tuple<object, Type>(result, type);
                    return result;
                }
                //throw new InvalidDataContractException("Cannot reliablity convert NSArray to type [" + type + "].");
                return null;
            }

            if (nsO is NSString nsString)
                //return new Tuple<object, Type>(nsString.ToString(), typeof(string));
                return nsString.ToString();


            if (nsO is NSData nsData && nsData.ToByteArray() is byte[] byteArray)
            {
                if (type.Implements(typeof(Stream)))
                    return new MemoryStream(byteArray);
                if (type == typeof(string))
                    return System.Text.Encoding.Default.GetString(byteArray);
                return byteArray;
            }

            if (nsO is NSDate nsDate)
                //return new Tuple<object, Type>(DateTime.SpecifyKind(nsDate.ToDateTime(), DateTimeKind.Unspecified), typeof(DateTime));
                return DateTime.SpecifyKind(nsDate.ToDateTime(), DateTimeKind.Unspecified);

            if (nsO is NSDecimalNumber nsDecimal)
                //return new Tuple<object, Type>(decimal.Parse(nsDecimal.ToString(), CultureInfo.InvariantCulture), typeof(decimal));
                return decimal.Parse(nsDecimal.ToString(), CultureInfo.InvariantCulture);

            if (nsO is NSNumber nsNumber)
            {
                if (type == null)
                {
                    var objCType = nsNumber.ObjCType;
                    switch (objCType)
                    {
                        case "c": type = typeof(char); break;
                        case "i": type = typeof(int); break;
                        case "s": type = typeof(short); break;
                        case "l": type = typeof(int); break;
                        case "q": type = typeof(long); break;
                        case "C": type = typeof(byte); break;
                        case "S": type = typeof(ushort); break;
                        case "L": type = typeof(uint); break;
                        case "Q": type = typeof(ulong); break;

                        case "f": type = typeof(float); break;
                        case "d": type = typeof(double); break;
                        case "B": type = typeof(bool); break;
                        default:
                            // could be string(*), void (v), object(@), class object (#), array, structure, union, pointer,  or unknown
                            return null;
                    }
                }

                if (type == typeof(char))
                    //return new Tuple<object, Type>((char)nsNumber.ByteValue, type);
                    return (char)nsNumber.ByteValue;
                if (type == typeof(int))
                    //                    return new Tuple<object, Type>((int)nsNumber.Int32Value, type);
                    return (int)nsNumber.Int32Value;
                if (type == typeof(short))
                    //return new Tuple<object, Type>((short)nsNumber.Int16Value, type);
                    return (short)nsNumber.Int16Value;
                if (type == typeof(long))
                    //return new Tuple<object, Type>((long)nsNumber.Int64Value, type);
                    return (long)nsNumber.Int64Value;
                if (type == typeof(byte))
                    //return new Tuple<object, Type>((byte)nsNumber.ByteValue, type);
                    return (byte)nsNumber.ByteValue;
                if (type == typeof(ushort))
                    //return new Tuple<object, Type>((ushort)nsNumber.UInt16Value, type);
                    return (ushort)nsNumber.UInt16Value;
                if (type == typeof(uint))
                    //return new Tuple<object, Type>((uint)nsNumber.UInt32Value, type);
                    return (uint)nsNumber.UInt32Value;
                if (type == typeof(ulong))
                    //return new Tuple<object, Type>((ulong)nsNumber.UInt64Value, type);
                    return (ulong)nsNumber.UInt64Value;
                if (type == typeof(float))
                    //return new Tuple<object, Type>((float)nsNumber.FloatValue, type);
                    return (float)nsNumber.FloatValue;
                if (type == typeof(double))
                    //return new Tuple<object, Type>((double)nsNumber.DoubleValue, type);
                    return (double)nsNumber.DoubleValue;
                if (type == typeof(bool))
                    //return new Tuple<object, Type>((bool)nsNumber.BoolValue, type);
                    return (bool)nsNumber.BoolValue;

            }

            if (nsO is NSUrl nsUrl)
            {
                var absolutePath = nsUrl.AbsoluteString;
                var uri = new Uri(absolutePath);
                //return new Tuple<object, Type>(uri, typeof(Uri));
                return uri;
            }

            /*
            if (nsO is NSData nsData)
            {
                var byteArray = new byte[nsData.Length];
                System.Runtime.InteropServices.Marshal.Copy(nsData.Bytes, byteArray, 0, Convert.ToInt32(nsData.Length));
                return byteArray;
            }
            */

            return null;
        }

        public static object ToDictionary(this NSDictionary nsDictionary)
        {
            var keyList = new List<string>();
            var valueList = new List<object>();
            var typeList = new List<Type>();

            foreach (var key in nsDictionary.Keys)
            {
                keyList.Add(key.ToString());
                try
                {
                    var nsObj = nsDictionary[key];
                    var obj = nsObj.ToObject();
                    valueList.Add(obj);
                    typeList.Add(obj.GetType());
                }
                catch (Exception)
                {
                    valueList.Add(null);
                    typeList.Add(null);
                }
            }
            bool allSame = true;
            if (typeList.Count > 1)
            {
                for (int i = 1; i < typeList.Count; i++)
                    if (typeList[i] != typeList[0])
                    {
                        allSame = false;
                        break;
                    }
            }
            if (typeList.Count > 0)
            {
                var dictionaryType = typeof(Dictionary<,>);
                var elementType = allSame && typeList[0] != null ? typeList[0] : typeof(object);
                var constructedDictionaryType = dictionaryType.MakeGenericType(typeof(string), elementType);
                var result = (IDictionary)Activator.CreateInstance(constructedDictionaryType);
                //var result = Convert.ChangeType(itemList, constructedListType);  // List is not IConvertable
                for (int i = 0; i < keyList.Count; i++)
                {
                    result.Add(keyList[i].ToString(), valueList[i]);
                }
                //return new Tuple<object, Type>(result, constructedDictionaryType);
                return result;
            }
            //return new Tuple<object, Type>(null, typeof(Dictionary<string, object>));
            return null;
        }

        public static object ToList(this NSArray nsArray)
        {
            //bool typeCodeSet = false;
            var itemList = new List<object>();
            var typeList = new List<Type>();
            for (nuint i = 0; i < nsArray.Count; i++)
            {
                try
                {
                    var nsObj = nsArray.GetItem<NSObject>(i);
                    var obj = nsObj.ToObject();
                    itemList.Add(obj);
                    typeList.Add(obj.GetType());
                }
                catch (Exception)
                {
                    itemList.Add(null);
                    typeList.Add(null);
                }
            }

            bool allSame = true;
            if (typeList.Count > 1)
            {
                for (int i = 1; i < typeList.Count; i++)
                    if (typeList[i] != typeList[0])
                    {
                        allSame = false;
                        break;
                    }
            }
            if (allSame && typeList.Count > 0 && typeList[0] != null)
            {
                var listType = typeof(List<>);
                var elementType = typeList[0];
                var constructedListType = listType.MakeGenericType(elementType);
                var result = (IList)Activator.CreateInstance(constructedListType);
                //var result = Convert.ChangeType(itemList, constructedListType);  // List is not IConvertable
                foreach (var item in itemList)
                    result.Add(item);
                //return new Tuple<object, Type>(result, constructedListType);
                return result;
            }
            //return new Tuple<object, Type>(itemList, typeof(List<object>));
            return null;
        }

        public static NSObject ToNSObject(this object obj)
        {
            if (obj == null)
                return null;
            NSObject nsObject = null;
            var type = obj.GetType();
            var typeInfo = type.GetTypeInfo();

            if (obj is byte[] byteArray)
                nsObject = NSData.FromArray(byteArray);
            else if (obj is IList ilist && typeInfo.IsGenericType)
                nsObject = ilist.ToNSArray();
            else if (obj is IDictionary dictionary && typeInfo.IsGenericType)
                nsObject = dictionary.ToNSDictionary();
            /*
            else if (obj is Stream stream)
                nsObject = NSData.FromStream(stream);
            else if (obj is Uri uri)
            {
                //if (uri.IsFile)
                //    nsObject = NSData.FromUrl(uri);
                //else
                //nsObject = new NSUrl(uri.AbsoluteUri);
                nsObject = NSObject.FromObject(uri.AbsoluteUri);
            }
            else if (obj is FileInfo fileInfo)
                //nsObject = NSData.FromFile(fileInfo.FullName);
                nsObject = NSObject.FromObject(fileInfo.FullName);
                */
            /*
            else if (obj is string str)
            {
                if (str.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) || str.StartsWith("file://", StringComparison.InvariantCultureIgnoreCase))
                    nsObject = NSData.FromUrl(new NSUrl(str));
                else if (File.Exists(str))
                    nsObject = NSData.FromFile(str);
            }
*/
            if (nsObject == null)
                nsObject = NSObject.FromObject(obj);

            return nsObject;
        }

        public static NSDictionary ToNSDictionary(this IDictionary dictionary)
        {
            var dictionaryType = dictionary.GetType();
            if (!dictionaryType.IsGenericType)
                throw new Exception("Only works with Generic IDictionary objects");
            var genericArgs = dictionaryType.GetGenericArguments();
            if (genericArgs[0] != typeof(string))
                throw new Exception("Only works with Dictionary<string,T> objects");
            var nsDictionary = new NSMutableDictionary();
            foreach (var key in dictionary.Keys)
            {
                var nsItem = NSObject.FromObject(dictionary[key]);
                nsDictionary.Add(new NSString(key.ToString()), nsItem);
            }
            return nsDictionary;
        }

        public static NSArray ToNSArray(this IList list)
        {
            var nsArray = new NSMutableArray();
            var arrayType = list.GetType();
            bool isArrayOfDictionaries = arrayType.IsGenericType && arrayType.GetGenericArguments()[0].GetTypeInfo().ImplementedInterfaces.Contains(typeof(IDictionary));
            foreach (var item in list)
            {
                NSObject nsItem = null;
                if (isArrayOfDictionaries)
                    nsItem = ((IDictionary)item).ToNSDictionary();
                else
                    nsItem = NSObject.FromObject(item);
                nsArray.Add(nsItem);
            }
            return nsArray;
        }

        public static byte[] ToByteArray(this NSData data)
        {
            var dataBytes = new byte[data.Length];
            System.Runtime.InteropServices.Marshal.Copy(data.Bytes, dataBytes, 0, Convert.ToInt32(data.Length));
            return dataBytes;
        }
    }
}