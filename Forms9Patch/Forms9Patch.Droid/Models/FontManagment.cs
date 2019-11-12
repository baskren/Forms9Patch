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

    class FontManagment : IFontFamilies
    {

        //static string CustomFontDirRoot => Settings.Context.CacheDir.AbsolutePath;
        //static string CustomFontDirRoot => Settings.Context.DataDir.AbsolutePath;
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

            if (FontFiles.TryGetValue(fontFamily, out string fontFilePath))
            {
                Typeface typeface = Typeface.CreateFromFile(fontFilePath);
                return typeface;
            }
            return null;
        }

        //static Typeface _droidSans;
        static Typeface DroidSans => Typeface.SansSerif;
        /*
    {
        get
        {
            if (_droidSans != null)
                return _droidSans;
            _droidSans = Typeface.SansSerif;
            if (_droidSans!=null)
                return _droidSans;
            _droidSans = TrySystemFont("Roboto");
            if (_droidSans != null)
                return _droidSans;
            _droidSans = TrySystemFont("Droid Sans");
            if (_droidSans != null)
                return _droidSans;
            _droidSans = TrySystemFont("DroidSans");
            if (_droidSans != null)
                return _droidSans;
            _droidSans = TrySystemFont("Roboto");
            if (_droidSans != null)
                return _droidSans;
            _droidSans = TrySystemFont("sans-serif");
            if (_droidSans != null)
                return _droidSans;
            _droidSans = TrySystemFont("normal");
            if (_droidSans != null)
                return _droidSans;
            return _droidSans;
        }
    }
    */

        //static Typeface _monoSpace;
        static Typeface MonoSpace => Typeface.Monospace;
        /*
        {
            get
            {
                if (_monoSpace != null)
                    return _monoSpace;
                _monoSpace = TrySystemFont("monospace");
                if (_monoSpace != null)
                    return _monoSpace;
                _monoSpace = TrySystemFont("Droid Sans Mono");
                if (_monoSpace != null)
                    return _monoSpace;
                return _monoSpace;
            }
        }
        */

        //static Typeface _serif;
        static Typeface Serif => Typeface.Serif;
        /*
        {
            get
            {
                if (_serif != null)
                    return _serif;
                _serif = TrySystemFont("serif");
                if (_serif != null)
                    return _serif;
                _serif = TrySystemFont("Droid Serif");
                if (_serif != null)
                    return _serif;
                return _serif;
            }
        }
        */

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

                    Typeface result;

                    if (fontFamily.ToLower() == "monospace" && MonoSpace != null)
                        return MonoSpace;
                    if (fontFamily.ToLower() == "serif" && Serif != null)
                        return Serif;
                    if (fontFamily.ToLower() == "sans-serif" && DroidSans != null)
                        return DroidSans;

                    result = TrySystemFont(fontFamily);
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
            return null;
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
                _fontFiles.Add(resouceId, cachedFontFile.AbsolutePath);
                return typeface;
            }
        }

        public List<string> FontFamilies()
        {
            return FontFiles.Keys.OrderBy((arg) =>
            {
                if (arg.ToLower().Contains(".ttf") || arg.ToLower().Contains(".otf"))
                    return " " + arg;
                return arg;
            }).ToList();
        }

        static Dictionary<string, string> _fontFiles;
        public static Dictionary<string, string> FontFiles
        {
            get
            {
                if (_fontFiles != null)
                    return _fontFiles;
                var context = Settings.Context;
                var fontAssetFileNames = context.Assets.List("Fonts");
                var cachedFontDir = new File(CustomFontDirRoot + "/AssetFonts");
                // move any Android Asset Fonts to the Applications CustomFontDirRoot
                if (fontAssetFileNames != null)
                {
                    foreach (var fontAssetFileName in fontAssetFileNames)
                    {
                        if (!cachedFontDir.Exists())
                            cachedFontDir.Mkdir();
                        var cachedFontFile = new File(cachedFontDir.AbsolutePath, fontAssetFileName);
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
                        cachedFontDir?.Dispose();
                    }
                }
                var fontdirs = new string[] { "/system/fonts", "/system/font", "/data/fonts", cachedFontDir.AbsolutePath };
                _fontFiles = new Dictionary<string, string>();
                var analyzer = new TTFAnalyzer();

                foreach (var fontdir in fontdirs)
                {
                    var dir = new File(fontdir);
                    if (!dir.Exists())
                        continue;

                    File[] files = dir.ListFiles();

                    if (files == null)
                        continue;

                    foreach (var file in files)
                    {
                        if (analyzer.FontAttributes(file.AbsolutePath) == "Regular")
                        {
                            String fontFamily = analyzer.FontFamily(file.AbsolutePath);
                            if (fontFamily != null && !_fontFiles.ContainsKey(fontFamily))
                                _fontFiles.Add(fontFamily, file.AbsolutePath);
                        }
                    }
                    dir.Dispose();
                }

                return _fontFiles.Count == 0 ? null : _fontFiles;
            }
        }
    }

    class TTFAnalyzer
    {

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

        public string FontFamily(string fontFilename)
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

                        // This is also a table. See http://developer.apple.com/fonts/ttrefman/rm06/Chap6name.html
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

                            // Table 42 lists the valid name Identifiers. We're interested in 1 (Font Family Name) but not in Unicode encoding (for simplicity).
                            // The encoding is stored as PlatformID and we're interested in Mac encoding
                            if (nameid_value == 1 && platformID == 1)
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

        public string FontAttributes(string fontFilename)
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

                        // This is also a table. See http://developer.apple.com/fonts/ttrefman/rm06/Chap6name.html
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

