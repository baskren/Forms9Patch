using System;
using Xamarin.Forms;
using System.Reflection;

namespace Forms9Patch
{
    static class IFontElementExtensions
    {
        internal static bool HasDefaultFont(this Xamarin.Forms.Button element)
        {
            if (element.FontFamily != null || element.FontAttributes != FontAttributes.None)
                return false;

            var getNamedSize = typeof(Device).GetRuntimeMethod("GetNamedSize", new Type[] { typeof(NamedSize), typeof(Type), typeof(bool) });
            if (getNamedSize != null)
            {
                var result = (double)getNamedSize.Invoke(null, new object[] { NamedSize.Default, typeof(Label), true });
                return result == element.FontSize;
            }
            return false;
        }

        internal static bool HasDefaultFont(this Xamarin.Forms.Editor element)
        {
            if (element.FontFamily != null || element.FontAttributes != FontAttributes.None)
                return false;

            var getNamedSize = typeof(Device).GetRuntimeMethod("GetNamedSize", new Type[] { typeof(NamedSize), typeof(Type), typeof(bool) });
            if (getNamedSize != null)
            {
                var result = (double)getNamedSize.Invoke(null, new object[] { NamedSize.Default, typeof(Label), true });
                return result == element.FontSize;
            }
            return false;
        }

        internal static bool HasDefaultFont(this Xamarin.Forms.Entry button)
        {
            if (button.FontFamily != null || button.FontAttributes != FontAttributes.None)
                return false;

            var getNamedSize = typeof(Device).GetRuntimeMethod("GetNamedSize", new Type[] { typeof(NamedSize), typeof(Type), typeof(bool) });
            if (getNamedSize != null)
            {
                var result = (double)getNamedSize.Invoke(null, new object[] { NamedSize.Default, typeof(Label), true });
                return result == button.FontSize;
            }
            return false;
        }

        internal static bool HasDefaultFont(this Xamarin.Forms.Label element)
        {
            if (element.FontFamily != null || element.FontAttributes != FontAttributes.None)
                return false;

            var getNamedSize = typeof(Device).GetRuntimeMethod("GetNamedSize", new Type[] { typeof(NamedSize), typeof(Type), typeof(bool) });
            if (getNamedSize != null)
            {
                var result = (double)getNamedSize.Invoke(null, new object[] { NamedSize.Default, typeof(Label), true });
                return result == element.FontSize;
            }
            return false;
        }

        internal static bool HasDefaultFont(this Xamarin.Forms.SearchBar element)
        {
            if (element.FontFamily != null || element.FontAttributes != FontAttributes.None)
                return false;

            var getNamedSize = typeof(Device).GetRuntimeMethod("GetNamedSize", new Type[] { typeof(NamedSize), typeof(Type), typeof(bool) });
            if (getNamedSize != null)
            {
                var result = (double)getNamedSize.Invoke(null, new object[] { NamedSize.Default, typeof(Label), true });
                return result == element.FontSize;
            }
            return false;
        }

        internal static bool HasDefaultFont(this Xamarin.Forms.Span element)
        {
            if (element.FontFamily != null || element.FontAttributes != FontAttributes.None)
                return false;

            var getNamedSize = typeof(Device).GetRuntimeMethod("GetNamedSize", new Type[] { typeof(NamedSize), typeof(Type), typeof(bool) });
            if (getNamedSize != null)
            {
                var result = (double)getNamedSize.Invoke(null, new object[] { NamedSize.Default, typeof(Label), true });
                return result == element.FontSize;
            }
            return false;
        }

    }
}
