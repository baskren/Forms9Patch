using System;
using System.Collections.Generic;
using Java.IO;
using Android.Text.Style;
using Android.Graphics;
using System.Linq;
using Forms9Patch.Droid;
using P42.Utils;
using System.Reflection;
using Android.Content.PM;

[assembly: Xamarin.Forms.Dependency(typeof(FontManagment))]
namespace Forms9Patch.Droid
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    class FontManagment : IFontFamilies
    {
        static string CustomFontDirRoot
        {
            get
            {
                PackageManager m = Settings.Context.PackageManager;
                String s = Settings.Context.PackageName;
                PackageInfo p = m.GetPackageInfo(s, 0);
                return p.ApplicationInfo.DataDir;
            }
        }

        static Typeface TrySystemFont(string fontFamily)
        {
            if (string.IsNullOrWhiteSpace(fontFamily))
                return null;
            var fonts = FontFamiliesAndPaths();

            var candidates = new Dictionary<string, string>();
            foreach (var kvp in fonts)
            {
                var fs = kvp.Key.Split("#");
                var family = fs[0];
                if (family.ToLower() == fontFamily.ToLower())
                {
                    if (fs.Length > 1)
                        candidates.Add(fs[1], kvp.Value);
                    else
                        return Typeface.CreateFromFile(kvp.Value);
                }
            }
            if (candidates.Values.FirstOrDefault() is string path)
               return Typeface.CreateFromFile(path);
            return null;
        }

        static Typeface DroidSans => Typeface.SansSerif;

        static Typeface MonoSpace => Typeface.Monospace;

        static Typeface Serif => Typeface.Serif;

        public static Typeface TypefaceForFontFamily(string fontFamilys, Assembly assembly = null)
        {
            if (string.IsNullOrWhiteSpace(fontFamilys))
                return null;
            var fontFamiliesList = fontFamilys.Split(',');
            if (fontFamiliesList != null && fontFamiliesList.Any())
            {
                foreach (var fontFamilyString in fontFamiliesList)
                {
                    var fontFamily = fontFamilyString?.Trim().Trim(';');
                    if (string.IsNullOrWhiteSpace(fontFamily))
                        continue;

                    var fontPair = fontFamily.Split("#");
                    string fontFile = null;
                    string fontName = fontPair[0];
                    if (fontPair[0].EndsWith(".ttf") || fontPair[0].EndsWith(".otf"))
                    {
                        fontFile = fontPair[0];
                        fontName = null;
                    }
                    if (fontPair.Length > 1)
                    {
                        fontName = fontPair[1];
                    }

                    Typeface result;

                    if (fontName?.ToLower() == "monospace" && MonoSpace != null)
                        return MonoSpace;
                    if (fontName?.ToLower() == "serif" && Serif != null)
                        return Serif;
                    if (fontName?.ToLower() == "sans-serif" && DroidSans != null)
                        return DroidSans;

                    result = TrySystemFont(fontName);
                    if (result != null)
                        return result;

                    result = TrySystemFont(fontFile);
                    if (result != null)
                        return result;

                    if (DroidSans != null && (fontFamily.ToLower() == "arial" || fontFamily.ToLower() == "helvetica"))
                        return DroidSans;

                    if (fontFamily.Contains(".Resources."))
                    {

                        // it's an Embedded Resource
                        if (!fontFamily.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) && !fontFamily.EndsWith(".otf", StringComparison.OrdinalIgnoreCase))
                            throw new Exception("Embedded Font file names must end with \".ttf\" or \".otf\".");
                        // what is the assembly?
                        /*
                        var assemblyName = fontFamily.Substring(0, fontFamily.IndexOf(".Resources.Fonts."));

                        //var assembly = System.Reflection.Assembly.Load (assemblyName);
                        var assembly = ReflectionExtensions.GetAssemblyByName(assemblyName) ?? Forms9Patch.Droid.Settings.ApplicationAssembly;

                        if (assembly == null)
                        {
                            // try using the current application assembly instead (as is the case with Shared Applications)
                            assembly = ReflectionExtensions.GetAssemblyByName(assemblyName + ".Droid");
                            ///System.Console.WriteLine ("Assembly for FontFamily \"" + fontFamily + "\" not found.");
                            //return null;
                        }
                        */
                        // move it to the Application's CustomFontDirRoot
                        if (EmbeddedResourceExtensions.FindAssemblyForResource(fontFamily, assembly) is Assembly asm)
                            return LoadAndRegisterEmbeddedFont(fontFamily, asm);
                    }
                }
            }
            return Typeface.Default;
        }

        static Typeface LoadAndRegisterEmbeddedFont(string resouceId, Assembly assembly)
        {
            using (var inputStream = EmbeddedResourceCache.GetStream(resouceId, assembly))
            {
                if (inputStream == null)
                {
                    System.Console.WriteLine("Embedded Resource for FontFamily \"" + resouceId + "\" not found.");
                    return null;
                }

                var cachedFontDir = new File(CustomFontDirRoot + "/ResourceFonts");
                if (!cachedFontDir.Exists())
                    cachedFontDir.Mkdir();

                var cachedFontFile = new File(cachedFontDir, resouceId);
                using (var outputStream = new FileOutputStream(cachedFontFile))
                {
                    const int bufferSize = 1024;
                    var buffer = new byte[bufferSize];
                    int length = -1;
                    while ((length = inputStream.Read(buffer, 0, bufferSize)) > 0)
                        outputStream.Write(buffer, 0, length);
                }
                Typeface typeface = Typeface.CreateFromFile(cachedFontFile);
                if (typeface == null)
                {
                    System.Console.WriteLine("Embedded Resource font file (" + resouceId + ") could not be loaded as an Android Typeface.");
                    return null;
                }
                _resourceFontFiles.Add(resouceId, cachedFontFile.AbsolutePath);
                return typeface;
            }
        }


        public static Dictionary<string,string> FontFamiliesAndPaths()
        {
            var loadedFonts = new Dictionary<string, string>();
            loadedFonts.AddRange(SystemFontFiles);
            loadedFonts.AddRange(AssetFontFiles);
            loadedFonts.AddRange(ResourceFontFiles);
            return loadedFonts;
        }

        public List<string> FontFamilies()
        {
            var result = new List<string>();
            foreach (var key in FontFamiliesAndPaths().Keys)
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    var family = key.Split("#")[0];
                    if (!result.Contains(family))
                        result.Add(family);
                }
            }
            return result;
        }
        /*
            => FontFamiliesAndPaths().Keys.OrderBy((arg) =>
            {
                if (arg.ToLower().Contains(".ttf") || arg.ToLower().Contains(".otf"))
                    return " " + arg;
                return arg;
            }).ToList();
        */

        static Dictionary<string, string> _systemFontFiles;
        public static Dictionary<string, string> SystemFontFiles
        {
            get
            {
                if (_systemFontFiles != null)
                    return _systemFontFiles;
                var context = Settings.Context;
                var fontAssetFileNames = context.Assets.List("Fonts");
                using (var cachedFontDir = new File(CustomFontDirRoot + "/AssetFonts"))
                {
                    if (!cachedFontDir.Exists())
                        cachedFontDir.Mkdir();
                    // move any Android Asset Fonts to the Applications CustomFontDirRoot
                    if (fontAssetFileNames != null)
                    {
                        foreach (var fontAssetFileName in fontAssetFileNames)
                        {
                            using (var cachedFontFile = new File(cachedFontDir.AbsolutePath, fontAssetFileName))
                            {
                                if (!cachedFontFile.Exists())
                                {
                                    // copy into CustomFontDirRoot
                                    var inputStream = context.Assets.Open("Fonts/" + fontAssetFileName);
                                    var outputStream = new FileOutputStream(cachedFontFile);
                                    const int bufferSize = 1024;
                                    var buffer = new byte[bufferSize];
                                    int length = -1;
                                    while ((length = inputStream.Read(buffer, 0, bufferSize)) > 0)
                                    {
                                        outputStream.Write(buffer, 0, length);
                                    }
                                    inputStream.Close();
                                    outputStream.Close();
                                }
                            }
                        }
                    }
                    var fontdirs = new string[] { "/system/fonts", "/system/font", "/data/fonts" };
                    _systemFontFiles = QueryFontFilesFromFontDirectories(fontdirs);
                }
                return _systemFontFiles.Count == 0 ? null : _systemFontFiles;
            }
        }


        static Dictionary<string, string> _assetFontFiles;
        public static Dictionary<string, string> AssetFontFiles
        {
            get
            {
                if (_assetFontFiles != null)
                    return _assetFontFiles;
                /*
                var context = Settings.Context;
                var names = context.Assets.List("");
                var fontAssetFileNames = context.Assets.List("Fonts");
                using (var cachedFontDir = new File(CustomFontDirRoot + "/AssetFonts"))
                {
                    if (!cachedFontDir.Exists())
                        cachedFontDir.Mkdir();
                    // move any Android Asset Fonts to the Applications CustomFontDirRoot
                    if (fontAssetFileNames != null)
                    {
                        foreach (var fontAssetFileName in fontAssetFileNames)
                        {
                            using (var cachedFontFile = new File(cachedFontDir.AbsolutePath, fontAssetFileName))
                            {
                                if (!cachedFontFile.Exists())
                                {
                                    // copy into CustomFontDirRoot
                                    var inputStream = context.Assets.Open("Fonts/" + fontAssetFileName);
                                    var outputStream = new FileOutputStream(cachedFontFile);
                                    const int bufferSize = 1024;
                                    var buffer = new byte[bufferSize];
                                    int length = -1;
                                    while ((length = inputStream.Read(buffer, 0, bufferSize)) > 0)
                                    {
                                        outputStream.Write(buffer, 0, length);
                                    }
                                    inputStream.Close();
                                    outputStream.Close();
                                }
                            }
                        }
                    }
                
                    var fontdirs = new string[] { cachedFontDir.AbsolutePath };
                    _assetFontFiles = QueryFontFilesFromFontDirectories(fontdirs);
                }
                */
                BuildAssetFontFileCache();
                using (var cachedFontDir = new File(CustomFontDirRoot + "/AssetFonts"))
                {
                    var fontdirs = new string[] { cachedFontDir.AbsolutePath };
                    _assetFontFiles = QueryFontFilesFromFontDirectories(fontdirs);
                }
                return _assetFontFiles.Count == 0 ? null : _assetFontFiles;
            }
        }

        static void BuildAssetFontFileCache(string path = "")
        {
            var assetFileNames = Settings.Context.Assets.List(path);
            using (var cachedFontDir = new File(CustomFontDirRoot + "/AssetFonts"))
            {
                if (!cachedFontDir.Exists())
                    cachedFontDir.Mkdir();
                // move any Android Asset Fonts to the Applications CustomFontDirRoot
                if (assetFileNames != null)
                {
                    foreach (var assetFileName in assetFileNames)
                    {
                        var suffix = System.IO.Path.GetExtension(assetFileName);
                        if (!string.IsNullOrWhiteSpace(suffix) && (suffix == ".ttf" || suffix == ".otf"))
                        {
                            using (var cachedFontFile = new File(cachedFontDir.AbsolutePath, assetFileName))
                            {
                                if (!cachedFontFile.Exists())
                                {
                                    // copy into CustomFontDirRoot
                                    var assetFolder = (string.IsNullOrWhiteSpace(path) ? null : path + "/");
                                    var inputStream = Settings.Context.Assets.Open(assetFolder + assetFileName);
                                    var outputStream = new FileOutputStream(cachedFontFile);
                                    const int bufferSize = 1024;
                                    var buffer = new byte[bufferSize];
                                    int length;
                                    while ((length = inputStream.Read(buffer, 0, bufferSize)) > 0)
                                    {
                                        outputStream.Write(buffer, 0, length);
                                    }
                                    inputStream.Close();
                                    outputStream.Close();
                                }
                            }
                        }
                        else
                        {
                            BuildAssetFontFileCache(assetFileName);
                        }
                    }
                }

            }
        }

        static Dictionary<string, string> _resourceFontFiles = new Dictionary<string, string>();
        public static Dictionary<string, string> ResourceFontFiles
            =>  _resourceFontFiles.Count == 0 ? null : _resourceFontFiles;

        static Dictionary<string, string> QueryFontFilesFromFontDirectories(string[] fontdirs)
        {
            var results = new Dictionary<string, string>();
            var analyzer = new TTFAnalyzer();

            foreach (var fontdir in fontdirs)
            {
                using (var dir = new File(fontdir))
                {
                    if (!dir.Exists())
                        continue;

                    File[] files = dir.ListFiles();

                    if (files == null)
                        continue;

                    foreach (var file in files)
                    {
                        if (analyzer.FontFamiliesAndStyle(file.AbsolutePath) is List<string> fontFamilies)
                        {
                            foreach (var fontFamily in fontFamilies)
                            {
                                AddFont(results, fontFamily, file);
                            }
                        }
                    }
                }
            }
            return results;
        }

        static void AddFont(Dictionary<string, string> results, string fontFamily, File file)
        {
            if (!string.IsNullOrWhiteSpace(fontFamily))
            {
                results[fontFamily] = file.AbsolutePath;
                var fileName = System.IO.Path.GetFileName(file.AbsolutePath);
                results[fileName] = file.AbsolutePath;
                var parts = fontFamily.Split("#");
                if (parts.Length > 1 && parts[1].Trim() is string style && !string.IsNullOrWhiteSpace(style))
                {
                    results[fileName + "#" + style] = file.AbsolutePath;
                }
            }
        }

    }




    class TTFAnalyzer
    {
        // https://developer.apple.com/fonts/TrueType-Reference-Manual/RM06/Chap6name.html


        // Font file; must be seekable
        RandomAccessFile m_file;

        // Helper I/O functions
        int ReadByte()
        {
            return m_file.Read() & 0xFF;
        }

        int ReadWord()
        {
            int b1 = ReadByte();
            int b2 = ReadByte();
            return b1 << 8 | b2;
        }

        int ReadDword()
        {
            int b1 = ReadByte();
            int b2 = ReadByte();
            int b3 = ReadByte();
            int b4 = ReadByte();
            return b1 << 24 | b2 << 16 | b3 << 8 | b4;
        }

        void Read(byte[] array)
        {
            if (m_file.Read(array) != array.Length)
                throw new IOException();
        }

        // Helper
        static int GetWord(byte[] array, int offset)
        {
            int b1 = array[offset] & 0xFF;
            int b2 = array[offset + 1] & 0xFF;
            return b1 << 8 | b2;
        }

        public List<string> FontFamiliesAndStyle(string fontFilename)
        {
            string family = null;
            string style = null;
            try
            {
                // Parses the TTF file format.
                // See http://developer.apple.com/fonts/ttrefman/rm06/Chap6.html
                m_file = new RandomAccessFile(fontFilename, "r");

                // Read the version first
                int version = ReadDword();

                // The version must be either 'true' (0x74727565) or 0x00010000 or 'OTTO' (0x4f54544f) for CFF style fonts.
                if (version != 0x74727565 && version != 0x00010000 && version != 0x4f54544f)
                    return null;

                // The TTF file consist of several sections called "tables", and we need to know how many of them are there.
                int numTables = ReadWord();

                // Skip the rest in the header
                ReadWord(); // skip searchRange
                ReadWord(); // skip entrySelector
                ReadWord(); // skip rangeShift

                // Now we can read the tables
                for (int i = 0; i < numTables; i++)
                {
                    // Read the table entry
                    int tag = ReadDword();
                    ReadDword(); // skip checksum
                    int offset = ReadDword();
                    int length = ReadDword();

                    // Now here' the trick. 'name' field actually contains the textual string name.
                    // So the 'name' string in characters equals to 0x6E616D65
                    if (tag == 0x6E616D65)
                    {
                        // Here's the name section. Read it completely into the allocated buffer
                        var table = new byte[length];

                        m_file.Seek(offset);
                        Read(table);

                        // This is also a table. See http://developer.apple.com/fonts/ttrefman/rm06/Chap6name.html
                        // According to Table 36, the total number of table records is stored in the second word, at the offset 2.
                        // Getting the count and string offset - remembering it's big endian.
                        int count = GetWord(table, 2);
                        int string_offset = GetWord(table, 4);

                        List<string> familyNames = new List<string>();

                        // Record starts from offset 6
                        for (int record = 0; record < count; record++)
                        {
                            // Table 37 tells us that each record is 6 words -> 12 bytes, and that the nameID is 4th word so its offset is 6.
                            // We also need to account for the first 6 bytes of the header above (Table 36), so...
                            int nameid_offset = record * 12 + 6;
                            int platformID = GetWord(table, nameid_offset);
                            int nameid_value = GetWord(table, nameid_offset + 6);

                            // Table 42 lists the valid name Identifiers. We're interested in 1 (Font Family Name) but not in Unicode encoding (for simplicity).
                            // The encoding is stored as PlatformID and we're interested in Mac encoding
                            if (platformID == 1)
                            {
                                // We need the string offset and length, which are the word 6 and 5 respectively
                                int name_length = GetWord(table, nameid_offset + 8);
                                int name_offset = GetWord(table, nameid_offset + 10);

                                // The real name string offset is calculated by adding the string_offset
                                name_offset += string_offset;

                                // Make sure it is inside the array
                                if (name_offset >= 0 && name_offset + name_length < table.Length)
                                {
                                    //return new String( table, name_offset, name_length );
                                    //char[] chars = new char[name_length];
                                    /*
                                    System.Buffer.BlockCopy(table, name_offset, chars, 0, name_length);
                                    */
                                    /*
                                    for(int nameI=0;nameI<name_length;nameI++) {
                                        chars [nameI] = (char)table [name_offset + nameI];
                                    }
                                    */
                                    byte[] chars = new byte[name_length];
                                    System.Buffer.BlockCopy(table, name_offset, chars, 0, name_length);
                                    //var str = new string(chars);
                                    var str = System.Text.Encoding.Default.GetString(chars);
                                    if (nameid_value == 1) // Family
                                    {
                                        //familyNames.Add(str);
                                        family = str;
                                        //System.Diagnostics.Debug.WriteLine(GetType() + ".FAMILY: " + str);
                                    }
                                    else if (nameid_value == 2) // Style
                                    {
                                        //System.Diagnostics.Debug.WriteLine(GetType() + ".STYLE: " + str);
                                        style = str;
                                        if (!string.IsNullOrWhiteSpace(family))
                                            familyNames.Add(family + "#" + style);
                                    }
                                    else if (!string.IsNullOrWhiteSpace(str))
                                    {
                                        if (nameid_value == 18 || nameid_value == 16 || nameid_value == 4 || nameid_value == 6)
                                        {
                                            //familyNames.Add(str);
                                            family = str;
                                            if (string.IsNullOrWhiteSpace(style))
                                                familyNames.Add(family);
                                            else
                                                familyNames.Add(family + "#" + style);
                                        }
                                    }
                                    System.Diagnostics.Debug.WriteLine(GetType() + ".\t\t [" + nameid_value + "]: " + str);
                                }
                            }
                        }

                        return familyNames;
                    }
                }

                return null;
            }
#pragma warning disable 0168
            catch (FileNotFoundException)
#pragma warning restore 0168
            {
                // Permissions?
                return null;
            }
#pragma warning disable 0168
            catch (IOException)
#pragma warning restore 0168
            {
                // Most likely a corrupted font file
                return null;
            }
        }


        public string FontAttributes_DELETEME(string fontFilename)
        {
            try
            {
                // Parses the TTF file format.
                // See http://developer.apple.com/fonts/ttrefman/rm06/Chap6.html
                m_file = new RandomAccessFile(fontFilename, "r");

                // Read the version first
                int version = ReadDword();

                // The version must be either 'true' (0x74727565) or 0x00010000 or 'OTTO' (0x4f54544f) for CFF style fonts.
                if (version != 0x74727565 && version != 0x00010000 && version != 0x4f54544f)
                    return null;

                // The TTF file consist of several sections called "tables", and we need to know how many of them are there.
                int numTables = ReadWord();

                // Skip the rest in the header
                ReadWord(); // skip searchRange
                ReadWord(); // skip entrySelector
                ReadWord(); // skip rangeShift

                // Now we can read the tables
                for (int i = 0; i < numTables; i++)
                {
                    // Read the table entry
                    int tag = ReadDword();
                    ReadDword(); // skip checksum
                    int offset = ReadDword();
                    int length = ReadDword();

                    // Now here' the trick. 'name' field actually contains the textual string name.
                    // So the 'name' string in characters equals to 0x6E616D65
                    if (tag == 0x6E616D65)
                    {
                        // Here's the name section. Read it completely into the allocated buffer
                        var table = new byte[length];

                        m_file.Seek(offset);
                        Read(table);

                        // This is also a table. See https://developer.apple.com/fonts/TrueType-Reference-Manual/RM06/Chap6name.html (was http://developer.apple.com/fonts/ttrefman/rm06/Chap6name). 
                        // According to Table 36, the total number of table records is stored in the second word, at the offset 2.
                        // Getting the count and string offset - remembering it's big endian.
                        int count = GetWord(table, 2);
                        int string_offset = GetWord(table, 4);

                        // Record starts from offset 6
                        for (int record = 0; record < count; record++)
                        {
                            // Table 37 tells us that each record is 6 words -> 12 bytes, and that the nameID is 4th word so its offset is 6.
                            // We also need to account for the first 6 bytes of the header above (Table 36), so...
                            int nameid_offset = record * 12 + 6;
                            int platformID = GetWord(table, nameid_offset);
                            int nameid_value = GetWord(table, nameid_offset + 6);

                            // Table 42 lists the valid name Identifiers. We're interested in 1 (Font Subfamily Name) but not in Unicode encoding (for simplicity).
                            // The encoding is stored as PlatformID and we're interested in Mac encoding
                            if (nameid_value == 2 && platformID == 1)
                            {
                                // We need the string offset and length, which are the word 6 and 5 respectively
                                int name_length = GetWord(table, nameid_offset + 8);
                                int name_offset = GetWord(table, nameid_offset + 10);

                                // The real name string offset is calculated by adding the string_offset
                                name_offset += string_offset;

                                // Make sure it is inside the array
                                if (name_offset >= 0 && name_offset + name_length < table.Length)
                                {
                                    //return new String( table, name_offset, name_length );
                                    //char[] chars = new char[name_length];
                                    /*
                                    System.Buffer.BlockCopy(table, name_offset, chars, 0, name_length);
                                    */
                                    /*
                                    for(int nameI=0;nameI<name_length;nameI++) {
                                        chars [nameI] = (char)table [name_offset + nameI];
                                    }
                                    */
                                    byte[] chars = new byte[name_length];
                                    System.Buffer.BlockCopy(table, name_offset, chars, 0, name_length);
                                    //var str = new string(chars);
                                    var str = System.Text.Encoding.Default.GetString(chars);
                                    System.Diagnostics.Debug.WriteLine("\t\t"+GetType() + ".FontAttributes attr=" + str);
                                    return str;
                                }
                            }
                        }
                    }
                }

                return null;
            }
#pragma warning disable 0168
            catch (FileNotFoundException)
#pragma warning restore 0168
            {
                // Permissions?
                return null;
            }
#pragma warning disable 0168
            catch (IOException)
#pragma warning restore 0168
            {
                // Most likely a corrupted font file
                return null;
            }
        }
    }

}

