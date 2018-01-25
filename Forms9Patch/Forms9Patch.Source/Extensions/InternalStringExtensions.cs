using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// String extensions.
    /// </summary>
    static class InternalStringExtensions
    {
        internal static IEnumerable<int> GetUnicodeCodePoints(this string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                int unicodeCodePoint = char.ConvertToUtf32(s, i);
                if (unicodeCodePoint > 0xffff)
                    i++;
                yield return unicodeCodePoint;
            }
        }

        internal static double ToFontSize(this string sizeString)
        {
            var s = sizeString;
            double size;
            var element = new Xamarin.Forms.Label();
            if (s.EndsWith("px", StringComparison.Ordinal))
            {
                var subString = s.Substring(0, s.Length - 2);
                if (double.TryParse(subString, NumberStyles.Float, CultureInfo.InvariantCulture, out size))
                    return size;
                throw new FormatException("Cannot parse [" + s + "][" + subString + "] [" + size + "] px font size");
            }
            if (s.EndsWith("em", StringComparison.Ordinal))
            {
                var subString = s.Substring(0, s.Length - 2);
                if (double.TryParse(subString, NumberStyles.Float, CultureInfo.InvariantCulture, out size))
                    return -size;
                throw new FormatException("Cannot parse [" + s + "][" + subString + "] [" + size + "] em font size");
            }
            if (s.EndsWith("%", StringComparison.Ordinal))
            {
                var subString = s.Substring(0, s.Length - 1);
                if (double.TryParse(subString, NumberStyles.Float, CultureInfo.InvariantCulture, out size))
                    return -size / 100.0;
                throw new FormatException("Cannot parse [" + s + "][" + subString + "] [" + size + "] % font size");
            }
            switch (s.ToLower())
            {
                case "xx-small":
                    //return Device.GetNamedSize (NamedSize.Micro, element) - (Device.GetNamedSize (NamedSize.Small, element) - Device.GetNamedSize (NamedSize.Micro, element)) ;
                    return -10.0 / 17.0;
                case "x-small":
                case "1":
                case "-2":
                    //return Device.GetNamedSize (NamedSize.Micro, element);
                    return -12.0 / 17.0;
                case "small":
                case "2":
                case "-1":
                    //return Device.GetNamedSize (NamedSize.Small, element);
                    return -14.0 / 17.0;
                case "medium":
                case "3":
                    //return Device.GetNamedSize (NamedSize.Medium, element);
                    return -1.0;
                case "large":
                case "4":
                case "+1":
                    //return Device.GetNamedSize (NamedSize.Large, element);
                    return -22.0 / 17.0;
                case "x-large":
                case "5":
                case "+2":
                    //return Device.GetNamedSize (NamedSize.Large, element) + (Device.GetNamedSize (NamedSize.Large, element) - Device.GetNamedSize (NamedSize.Medium, element));
                    return -27.0 / 17.0;
                case "xx-large":
                case "6":
                case "+3":
                    //return Device.GetNamedSize (NamedSize.Large, element) + (Device.GetNamedSize (NamedSize.Large, element) - Device.GetNamedSize (NamedSize.Medium, element)) * 2.0;
                    return -32.0 / 17.0;
                case "7":
                case "+4":
                    //return Device.GetNamedSize (NamedSize.Large, element) + (Device.GetNamedSize (NamedSize.Large, element) - Device.GetNamedSize (NamedSize.Medium, element)) * 3.0;
                    return -37.0 / 17.0;
                case "initial":
                    return -1;
            }
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out size))
            {
                if (size < 1)
                    return Device.GetNamedSize(NamedSize.Micro, element);
                if (size > 7)
                    return Device.GetNamedSize(NamedSize.Large, element) + (Device.GetNamedSize(NamedSize.Large, element) - Device.GetNamedSize(NamedSize.Medium, element)) * 3.0;
            }
            return 0;
        }


    }
}

