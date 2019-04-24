using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.DirectWrite;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.UWP.InstalledFont))]
namespace Forms9Patch.UWP
{
    public class InstalledFont : IComparable, IFontFamilies
    {
        static List<InstalledFont> fonts = null;
        public string Name { get; set; }
        public string DisplayFont { get; set; }

        public static InstalledFont FindFont(string font)
        {
            foreach (var f in fonts)
            {
                if (f.Name == font)
                    return f;
            }

            return null;
        }


        // Code taken straight from SharpDX\Samples\DirectWrite\FontEnumeration\Program.cs
        public static List<InstalledFont> GetFonts()
        {
            if (fonts != null)
                return fonts;
            var fontList = new List<InstalledFont>();

            using (var factory = new Factory())
            {
                using (var fontCollection = factory.GetSystemFontCollection(false))
                {
                    var familyCount = fontCollection.FontFamilyCount;
                    for (int i = 0; i < familyCount; i++)
                    {
                        try
                        {
                            using (var fontFamily = fontCollection.GetFontFamily(i))
                            {
                                var familyNames = fontFamily.FamilyNames;
                                int index;

                                if (!familyNames.FindLocaleName(System.Globalization.CultureInfo.CurrentCulture.Name, out index))
                                    familyNames.FindLocaleName("en-us", out index);

                                string name = familyNames.GetString(index);
                                string display = name;
                                using (var font = fontFamily.GetFont(index))
                                {
                                    if (font.IsSymbolFont)
                                        display = "Segoe UI";
                                }

                                fontList.Add(new InstalledFont { Name = name, DisplayFont = display });
                            }
                        }
                        catch { }       // Corrupted font files throw an exception - ignore them
                    }
                }
            }

            fontList.Sort();
            fonts = fontList;
            return fontList;
        }

        public int CompareTo(object obj)
        {
            return String.Compare(this.Name, (obj as InstalledFont).Name);
        }

        public List<string> FontFamilies()
        {
            var results = new List<string>();
            var systemFonts = GetFonts();
            foreach (var systemFont in systemFonts)
                    results.Add(systemFont.Name);
            var fontEmbeddedResourceIds = Forms9Patch.ApplicationInfoService.Assembly.GetManifestResourceNames().Where((resourceId) => IsFont(resourceId));
            results.AddRange(fontEmbeddedResourceIds);
            
            var appIncludedAssemblies = Settings.AssembliesToInclude;
            foreach (var asm in appIncludedAssemblies)
            {
                fontEmbeddedResourceIds = asm.GetManifestResourceNames().Where((resourceId) => IsFont(resourceId));
                results.AddRange(fontEmbeddedResourceIds);
            }
            return results;
        }

        bool IsFont(string resourceId)
        {
            var lower = resourceId.ToLower();
            return lower.EndsWith(".ttf") || lower.EndsWith(".otf");
        }
    }
}
