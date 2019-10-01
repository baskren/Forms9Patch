using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Forms9Patch.Elements.Popups.Core.Converters.TypeConverters
{
    /// <summary>
    /// Popup type converter
    /// </summary>
    public class EasingTypeConverter : TypeConverter
    {
        /// <summary>
        /// String to Easing Type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFromInvariantString(string value)
        {
            if (value != null)
            {
                FieldInfo fieldInfo = typeof(Easing).GetRuntimeFields()?.FirstOrDefault((fi =>
                {
                    if (fi.IsStatic)
                        return fi.Name == value;
                    return false;
                }));
                if (fieldInfo != null)
                    return (Easing)fieldInfo.GetValue(null);
            }
            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Easing)}");
        }
    }
}
