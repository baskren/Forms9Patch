using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch.UWP.Extensions
{
    class FontFamilies : IFontFamilies
    {
        /*
        List<string> IFontFamilies.FontFamilies()
        {
            return FontExtensions._embeddedResourceFonts.Keys.Concat(UIFont.FamilyNames).ToList();
        }

        internal static List<string> FontsForFamily(string family)
        {
            return UIFont.FontNamesForFamilyName(family).ToList();
        }
        */
        List<string> IFontFamilies.FontFamilies()
        {
            throw new NotImplementedException();
        }
    }


    static class TextBlockExtensions
    {
            internal static readonly Dictionary<string, string> _embeddedResourceFonts = new Dictionary<string, string>();
    static readonly object _loadFontLock = new object();

    }



}
