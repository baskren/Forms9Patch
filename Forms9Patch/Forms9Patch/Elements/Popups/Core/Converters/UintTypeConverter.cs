using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch.Elements.Popups.Core.Converters.TypeConverters
{
    /// <summary>
    /// Popup unit type converter
    /// </summary>
    public class UintTypeConverter : TypeConverter
    {
        /// <summary>
        /// String to unit type converter
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFromInvariantString(string value)
        {
            try
            {
                return Convert.ToUInt32(value);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Cannot convert {value} into {typeof(uint)}");
            }
        }
    }
}
