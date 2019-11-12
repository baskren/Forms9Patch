using System;
using System.Collections.Generic;
using UIKit;
using System.Linq;
using Xamarin.Forms;
using Foundation;
using CoreGraphics;
using CoreText;
using Forms9Patch.iOS;
using P42.Utils;
using System.Reflection;

[assembly: Dependency(typeof(FontFamilies))]
namespace Forms9Patch.iOS
{
    class FontFamilies : IFontFamilies
    {
        List<string> IFontFamilies.FontFamilies()
        {
            return FontExtensions._embeddedResourceFonts.Keys.Concat(UIFont.FamilyNames).ToList();
        }

        internal static List<string> FontsForFamily(string family)
        {
            return UIFont.FontNamesForFamilyName(family).ToList();
        }


    }


    static class FontExtensions
    {
        //
        // Static Fields
        //
        readonly static Dictionary<FontKey, UIFont> UiFontCache = new Dictionary<FontKey, UIFont>();

        internal static UIFont ToUIFont(this IFontElement element)
            => BestFont(element.FontFamily, (nfloat)element.FontSize, (element.FontAttributes & FontAttributes.Bold) != 0, (element.FontAttributes & FontAttributes.Italic) != 0);

        public static UIFont ToUIFont(this Font font)
        {
            var size = (float)font.FontSize;
            if (font.UseNamedSize)
            {
                switch (font.NamedSize)
                {
                    case NamedSize.Micro:
                        size = 12;
                        break;
                    case NamedSize.Small:
                        size = 14;
                        break;
                    case NamedSize.Medium:
                        size = 17;
                        break;
                    case NamedSize.Large:
                        size = 22;
                        break;
                    default:
                        size = 17;
                        break;
                }
            }
            bool isBold = font.FontAttributes.HasFlag(FontAttributes.Bold);
            bool isItalic = font.FontAttributes.HasFlag(FontAttributes.Italic);

            return BestFont(font.FontFamily, size, isBold, isItalic);
        }

        internal static UIFont EmbeddedFont(string resourceId, nfloat size, Assembly assembly = null)
        {
            if (resourceId == "STIXGeneral")
                resourceId = "Forms9Patch.Resources.Fonts.STIXGeneral.otf";

            lock (_loadFontLock)
            {
                if (_embeddedResourceFonts.ContainsKey(resourceId))
                {
                    string family = _embeddedResourceFonts[resourceId];
                    return UIFont.FromName(family, size);
                }
                if (EmbeddedResourceExtensions.FindAssemblyForResource(resourceId, assembly) is Assembly resourceAssembly
                    && LoadAndRegisterEmbeddedFont(resourceId, size, resourceAssembly) is UIFont uIFont)
                    return uIFont;
            }
            return null;
        }

        static UIFont LoadAndRegisterEmbeddedFont(string resourceId, nfloat size, Assembly assembly)
        {
            using (var stream = EmbeddedResourceCache.GetStream(resourceId, assembly))
            {
                if (stream == null)
                {
                    Console.WriteLine("Could not open Embedded Resource [" + resourceId + "]");
                    return null;
                }
                var data = NSData.FromStream(stream);
                if (data == null)
                {
                    Console.WriteLine("Could not retrieve data from Embedded Resource [" + resourceId + "].");
                    return null;
                }
                var provider = new CGDataProvider(data);
                var font = CGFont.CreateFromProvider(provider);
                if (!CTFontManager.RegisterGraphicsFont(font, out NSError error))
                {
                    if (error.Code != 105)
                    {
                        Console.WriteLine("Could load but failed to register [" + resourceId + "] font.  Error messages:");
                        Console.WriteLine("\tdescription: " + error.Description);
                        throw new MissingMemberException("Failed to register [" + resourceId + "] font.  Error messages in console.");
                    }
                    error?.Dispose();
                }
                _embeddedResourceFonts.Add(resourceId, font.PostScriptName);
                return UIFont.FromName(font.PostScriptName, size);
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter
        internal static UIFont BestFont(MetaFont metaFont, UIFont baseFont)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            nfloat size = (nfloat)metaFont.Size / (metaFont.Baseline == FontBaseline.Normal ? 1f : 1.6f);

            return BestFont(metaFont.Family, size, metaFont.Bold, metaFont.Italic);
        }

        internal static UIFont BestFont(string family, nfloat size, bool bold = false, bool italic = false, Assembly assembly=null)
        {

            if (family == null)
                family = "sans-serif";
            if (family.ToLower() == "monospace")
                family = "Menlo";
            if (family.ToLower() == "serif")
                family = "Times New Roman";
            if (family.ToLower() == "sans-serif")
                family = "Arial";

            if (size < 0)
                size = (nfloat)(UIFont.LabelFontSize * Math.Abs(size));


            // is it in the cache?
            var key = new FontKey(family, size, bold, italic);
            Dictionary<FontKey, UIFont> dictionary = UiFontCache;
            lock (dictionary)
            {
                if (dictionary.TryGetValue(key, out UIFont storedUiFont))
                    return storedUiFont;
            }


            UIFont bestAttemptFont = null;
            if (UIFont.FamilyNames.Contains(family))
            {
                // a system font
                var fontNames = FontFamilies.FontsForFamily(family);
                string baseFontName = null;
                string reqFontName = null;
                //string fallbackFontName = null;


                if (fontNames != null && fontNames.Count > 0)
                {
                    if (fontNames.Count == 1)
                    {
                        baseFontName = fontNames[0];
                        if (!bold && !italic)
                            reqFontName = fontNames[0];
                    }
                    else
                    {
                        int shortestMatchLength = int.MaxValue;
                        int shortestBaseLength = int.MaxValue;
                        foreach (var fontName in fontNames)
                        {
                            var lowerName = fontName.ToLower();
                            bool nameHasBold = lowerName.Contains("bold") || lowerName == "avenir-black";
                            bool nameHasItalic = lowerName.Contains("italic") || lowerName.Contains("oblique");
                            // assume the shortest name is the base font name
                            if (lowerName.Length < shortestBaseLength)
                            {
                                baseFontName = fontName;
                                shortestBaseLength = lowerName.Length;
                            }

                            // assume the shortest name with matching attributes is a match
                            if (nameHasBold == bold && nameHasItalic == italic && lowerName.Length < shortestMatchLength)
                            {
                                reqFontName = fontName;
                                shortestMatchLength = lowerName.Length;
                            }

                            if (lowerName.Contains("-regular"))
                            {
                                baseFontName = fontName;
                                shortestBaseLength = -1;
                                if (!bold && !italic)
                                {
                                    reqFontName = fontName;
                                    shortestMatchLength = -1;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (reqFontName != null)
                    bestAttemptFont = UIFont.FromName(reqFontName, size);
                if (bestAttemptFont == null && baseFontName != null && !bold && !italic)
                    bestAttemptFont = UIFont.FromName(baseFontName, size);
            }
            else
            {
                //  an embedded font or a explicitly named system font?
                bestAttemptFont = EmbeddedFont(family, size, assembly);

                if (bestAttemptFont == null)
                    bestAttemptFont = UIFont.FromName(family, size);
            }

            if (bestAttemptFont != null)
            {
                // we have a match but is wasn't cached - so let's cache it for future reference
                lock (dictionary)
                {
                    if (!dictionary.TryGetValue(key, out UIFont storedUiFont))
                        // It could have been added by another thread so only add if we're really sure it's no there!
                        dictionary.Add(key, bestAttemptFont);
                }
                return bestAttemptFont;
            }

            // fall back to a system font
            if (bold && italic)
            {
                UIFont systemFont = UIFont.SystemFontOfSize(size);
                var descriptor = systemFont.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold | UIFontDescriptorSymbolicTraits.Italic);
                bestAttemptFont = UIFont.FromDescriptor(descriptor, size);
            }
            if (bestAttemptFont == null && italic)
                bestAttemptFont = UIFont.ItalicSystemFontOfSize(size);
            if (bestAttemptFont == null && bold)
                bestAttemptFont = UIFont.BoldSystemFontOfSize(size);
            if (bestAttemptFont == null)
                bestAttemptFont = UIFont.SystemFontOfSize(size);
            return bestAttemptFont;
        }


        internal static readonly Dictionary<string, string> _embeddedResourceFonts = new Dictionary<string, string>();
        static readonly object _loadFontLock = new object();

        //
        // Nested Types
        //
        struct FontKey
        {
#pragma warning disable 0414
            readonly string Family;
            readonly nfloat Size;
            readonly bool Bold;
            readonly bool Italic;
#pragma warning restore 0414

            internal FontKey(string family, nfloat size, bool bold, bool italic)
            {
                Family = family;
                Size = size;
                Bold = bold;
                Italic = italic;
            }
        }


    }
}

