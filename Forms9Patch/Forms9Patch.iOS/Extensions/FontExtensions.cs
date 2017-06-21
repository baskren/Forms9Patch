using System;
using System.Collections.Generic;
using UIKit;
using System.Linq;
using Xamarin.Forms;
using Foundation;
using CoreGraphics;
using CoreText;
using Forms9Patch.iOS;
using PCL.Utils;

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
        static Dictionary<FontKey, UIFont> UiFontCache = new Dictionary<FontKey, UIFont>();

        /*
		//
		// Static Methods
		//
		static UIFont _ToUIFont (string family, float size, FontAttributes attributes)
		{
			bool isBold = (attributes & FontAttributes.Bold) > FontAttributes.None;
			bool isItalic = (attributes & FontAttributes.Italic) > FontAttributes.None;
			if (family != null) {
				try {
					UIFont uIFont;
					if (family.ToLower()=="monospace")
						family="Menlo";
					if (family.ToLower()=="serif")
						family="Times New Roman";
					if (family.ToLower()=="sans-serif")
						family="Arial";
					
					if (UIFont.FamilyNames.Contains (family)) {
						UIFontDescriptor uIFontDescriptor = new UIFontDescriptor ().CreateWithFamily (family);
						if (isBold | isItalic) {
							UIFontDescriptorSymbolicTraits uIFontDescriptorSymbolicTraits = UIFontDescriptorSymbolicTraits.ClassUnknown;
							if (isBold) 
								uIFontDescriptorSymbolicTraits |= UIFontDescriptorSymbolicTraits.Bold;
							if (isItalic) 
								uIFontDescriptorSymbolicTraits |= UIFontDescriptorSymbolicTraits.Italic;
							uIFontDescriptor = uIFontDescriptor.CreateWithTraits (uIFontDescriptorSymbolicTraits);
							uIFont = UIFont.FromDescriptor (uIFontDescriptor, size);
							if (uIFont != null) 
								return uIFont;
						}
					}
					//uIFont = UIFont.FromName (family, size);
					uIFont = UIFont.FromName(family, size) ?? EmbeddedFont(family,size);
					if (uIFont != null) 
						return uIFont;
				}
				catch {
				}
			}
			if (isBold & isItalic) {
				UIFont uIFont2 = UIFont.SystemFontOfSize (size);
				return UIFont.FromDescriptor (uIFont2.FontDescriptor.CreateWithTraits (UIFontDescriptorSymbolicTraits.Italic | UIFontDescriptorSymbolicTraits.Bold), 0);
			} else {
				if (isBold) 
					return UIFont.BoldSystemFontOfSize (size);
				return isItalic ? UIFont.ItalicSystemFontOfSize (size) : UIFont.SystemFontOfSize (size);
			}
		}
		*/

        /*
		internal static bool IsDefault (this Span self)
		{
			return self.FontFamily == null && self.FontSize == Device.GetNamedSize (NamedSize.Default, typeof(Label), true) && self.FontAttributes == FontAttributes.None;
		}
		*/

        /*
		static UIFont ToUIFont (string family, float size, FontAttributes attributes) {
			var key = new FontExtensions.FontKey (family, size, attributes);
			Dictionary<FontExtensions.FontKey, UIFont> dictionary = FontExtensions.UiFontCache;
			UIFont result;
			lock (dictionary) {
				UIFont storedUiFont;
				if (dictionary.TryGetValue (key, out storedUiFont)) {
					result = storedUiFont;
					return result;
				}
			}
			UIFont newUiFont = FontExtensions._ToUIFont (family, size, attributes);
			dictionary = FontExtensions.UiFontCache;
			lock (dictionary) {
				UIFont uIFont3;
				if (!dictionary.TryGetValue (key, out uIFont3)) {
					// only add if we're really sure it's no there!
					dictionary.Add (key, uIFont3 = newUiFont);
				}
				result = uIFont3;
			}
			return result;
		}
		*/
        /*
		internal static UIFont ToUIFont (this IFontElement element)
		{
			return FontExtensions.ToUIFont (element.FontFamily, (float)element.FontSize, element.FontAttributes);
		}
		*/

        internal static UIFont ToUIFont(this IFontElement element)
        {
            //object[] values = label.GetValues (Label.FontFamilyProperty, Label.FontSizeProperty, Label.FontAttributesProperty);
            //var family = (string)label.GetValue (Label.FontFamilyProperty);
            //var size = (double)label.GetValue (Label.FontSizeProperty);
            //if (size < 0)
            //	size = UIFont.LabelFontSize;
            //var attr = (FontAttributes)label.GetValue (Label.FontAttributesProperty);
            //return FontExtensions.ToUIFont (family, (float)size, attr) ?? UIFont.SystemFontOfSize (UIFont.LabelFontSize);
            return BestFont(element.FontFamily, (nfloat)element.FontSize, (element.FontAttributes & FontAttributes.Bold) != 0, (element.FontAttributes & FontAttributes.Italic) != 0);
        }

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

            /*
			if (font.FontFamily != null) {
				try {
					UIFont result;
					if (UIFont.FamilyNames.Contains (font.FontFamily)) {
						UIFontDescriptor uIFontDescriptor = new UIFontDescriptor ().CreateWithFamily (font.FontFamily);
						if (isBold | isItalic) {
							UIFontDescriptorSymbolicTraits uIFontDescriptorSymbolicTraits = UIFontDescriptorSymbolicTraits.ClassUnknown;
							if (isBold) {
								uIFontDescriptorSymbolicTraits |= UIFontDescriptorSymbolicTraits.Bold;
							}
							if (isItalic) {
								uIFontDescriptorSymbolicTraits |= UIFontDescriptorSymbolicTraits.Italic;
							}
							uIFontDescriptor = uIFontDescriptor.CreateWithTraits (uIFontDescriptorSymbolicTraits);
							result = UIFont.FromDescriptor (uIFontDescriptor, size);
							return result;
						}
					}
					result = UIFont.FromName (font.FontFamily, size);
					return result;
				}
				catch {
				}
			}
			if (isBold & isItalic) {
				return UIFont.FromDescriptor (UIFont.SystemFontOfSize (size).FontDescriptor.CreateWithTraits (UIFontDescriptorSymbolicTraits.Italic | UIFontDescriptorSymbolicTraits.Bold), 0);
			} else {
				if (isBold) 
					return UIFont.BoldSystemFontOfSize (size);
				return isItalic ? UIFont.ItalicSystemFontOfSize (size) : UIFont.SystemFontOfSize (size);
			}
			*/

            return BestFont(font.FontFamily, size, isBold, isItalic);
        }



        internal static UIFont EmbeddedFont(string resourceID, nfloat size)
        {
            if (resourceID == "STIXGeneral")
                resourceID = "Forms9Patch.Resources.Fonts.STIXGeneral.otf";

            if (_embeddedResourceFonts.ContainsKey(resourceID))
            {
                string family = _embeddedResourceFonts[resourceID];
                return UIFont.FromName(family, size);
            }
            if (resourceID.Contains(".Resources.Fonts."))
            {
                // it's an Embedded Resource
                if (!resourceID.ToLower().EndsWith(".ttf") && !resourceID.ToLower().EndsWith(".otf"))
                    throw new MissingMemberException("Embedded Font file names must end with \".ttf\" or \".otf\".");
                lock (_loadFontLock)
                {
                    // what is the assembly?
                    var assemblyName = resourceID.Substring(0, resourceID.IndexOf(".Resources.Fonts."));
                    //var assembly = System.Reflection.Assembly.Load (assemblyName);
                    var assembly = ReflectionExtensions.GetAssemblyByName(assemblyName) ?? Forms9Patch.iOS.Settings.ApplicationAssembly;
                    if (assembly == null)
                    {
                        // try using the current application assembly instead (as is the case with Shared Applications)
                        assembly = ReflectionExtensions.GetAssemblyByName(assemblyName + ".Droid");
                        //Console.WriteLine ("Assembly for Resource ID \"" + resourceID + "\" not found.");
                        //return null;
                    }
                    // load it!
                    var stream = assembly.GetManifestResourceStream(resourceID);
                    if (stream != null)
                    {
                        var data = NSData.FromStream(stream);
                        stream.Close();
                        if (data != null)
                        {
                            var provider = new CGDataProvider(data);
                            var font = CGFont.CreateFromProvider(provider);
                            NSError error;
                            if (!CTFontManager.RegisterGraphicsFont(font, out error))
                            {
                                if (error.Code != 105)
                                {
                                    Console.WriteLine("Could load but failed to register [" + resourceID + "] font.  Error messages:");
                                    Console.WriteLine("\tdescription: " + error.Description);
                                    //Console.WriteLine ("\tlocalized description: " + error.LocalizedDescription);
                                    //Console.WriteLine ("\tdebug description: " + error.DebugDescription);
                                    throw new MissingMemberException("Failed to register [" + resourceID + "] font.  Error messages in console.");
                                }
                            }
                            //var psName = font.PostScriptName;
                            _embeddedResourceFonts.Add(resourceID, font.PostScriptName);
                            return UIFont.FromName(font.PostScriptName, size);
                        }
                        else
                        {
                            Console.WriteLine("Could not retrieve data from Embedded Resource [" + resourceID + "].");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Could not open Embedded Resource [" + resourceID + "]");
                    }
                }
                //} else {
                //	Console.WriteLine ("Font [] is assumed not to be an embedded resource because it does not contain \".Resources.Fonts.\" in its Resource ID");
            }
            return null;
        }

        internal static UIFont BestFont(MetaFont metaFont, UIFont baseFont)
        {
            nfloat size = metaFont.Size / (metaFont.Baseline == FontBaseline.Normal ? 1f : 1.6f);

            return BestFont(metaFont.Family, size, metaFont.Bold, metaFont.Italic);
        }

        internal static UIFont BestFont(string family, nfloat size, bool bold = false, bool italic = false)
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
                UIFont storedUiFont;
                if (dictionary.TryGetValue(key, out storedUiFont))
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
                bestAttemptFont = EmbeddedFont(family, size);

                if (bestAttemptFont == null)
                    bestAttemptFont = UIFont.FromName(family, size);
            }

            if (bestAttemptFont != null)
            {
                // we have a match but is wasn't cached - so let's cache it for future reference
                lock (dictionary)
                {
                    UIFont storedUiFont;
                    if (!dictionary.TryGetValue(key, out storedUiFont))
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

        internal class MetaFont
        {
            public string Family { get; set; }
            public nfloat Size { get; set; }
            public bool Italic { get; set; }
            public bool Bold { get; set; }
            public FontBaseline Baseline { get; set; }

            public MetaFont(string family, nfloat size, bool bold = false, bool italic = false)
            {
                Baseline = FontBaseline.Normal;
                Family = family;
                Size = size;
                Bold = bold;
                Italic = italic;
            }

            public MetaFont(MetaFont f)
            {
                Baseline = f.Baseline;
                Family = f.Family;
                Size = f.Size;
                Bold = f.Bold;
                Italic = f.Italic;
            }

            public static bool operator ==(MetaFont f1, MetaFont f2)
            {
                if (((object)f1) == null || ((object)f2) == null)
                    return false;
                if (f1.Size != f2.Size)
                    return false;
                if (f1.Bold != f2.Bold)
                    return false;
                if (f1.Italic != f2.Italic)
                    return false;
                if (f1.Family != f2.Family)
                    return false;
                return f1.Baseline == f2.Baseline;
            }


            public static bool operator !=(MetaFont f1, MetaFont f2)
            {
                return !(f1 == f2);
            }

            public override bool Equals(object obj)
            {
                return this == (MetaFont)obj;
            }

            public override int GetHashCode()
            {
                var familyHash = Family.GetHashCode() << 12;
                var sizeHash = ((int)Size) << 4;
                var boldHash = (Bold ? 0x08 : 0x00);
                var italicHash = (Italic ? 0x04 : 0x00);
                var baselineHash = (int)Baseline;
                return familyHash + sizeHash + boldHash + italicHash + baselineHash;
            }

        }

        internal static readonly Dictionary<string, string> _embeddedResourceFonts = new Dictionary<string, string>();
        static readonly object _loadFontLock = new object();

        //
        // Nested Types
        //
        struct FontKey
        {
#pragma warning disable 0414
            string Family;
            nfloat Size;
            bool Bold;
            bool Italic;
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

