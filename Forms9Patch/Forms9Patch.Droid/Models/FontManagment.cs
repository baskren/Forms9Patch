using System;
using System.Collections.Generic;
using Java.IO;
using Android.Text.Style;
using Android.Graphics;
using System.Linq;
using Forms9Patch.Droid;
using PCL.Utils;

[assembly: Xamarin.Forms.Dependency(typeof(FontManagment))]
namespace Forms9Patch.Droid
{

    class FontManagment : IFontFamilies
    {

        static Typeface TrySystemFont(string fontFamily)
        {
            string fontFilePath;

            if (FontFiles.TryGetValue(fontFamily, out fontFilePath))
            {
                Typeface typeface = Typeface.CreateFromFile(fontFilePath);
                return typeface;
            }
            return null;
        }

        public static Typeface TypefaceForFontFamily(string fontFamily)
        {
            //string fontFilePath;
            if (fontFamily == null)
                //return Typeface.Default;
                return null;

            Typeface result;

            if (fontFamily.ToLower() == "monospace")
            {
                result = TrySystemFont("monospace");
                if (result != null)
                    return result;
                result = TrySystemFont("Droid Sans Mono");
                if (result != null)
                    return result;

            }
            if (fontFamily.ToLower() == "serif")
            {
                result = TrySystemFont("serif");
                if (result != null)
                    return result;
                result = TrySystemFont("Droid Serif");
                if (result != null)
                    return result;
            }
            if (fontFamily.ToLower() == "sans-serif")
            {
                result = TrySystemFont("sans-serif");
                if (result != null)
                    return result;
                result = TrySystemFont("normal");
                if (result != null)
                    return result;
                result = TrySystemFont("Roboto");
                if (result != null)
                    return result;
                result = TrySystemFont("Droid Sans");
                if (result != null)
                    return result;
            }

            result = TrySystemFont(fontFamily);
            if (result != null)
                return result;

            if (fontFamily.Contains(".Resources.Fonts."))
            {
                // it's an Embedded Resource
                if (!fontFamily.ToLower().EndsWith(".ttf") && !fontFamily.ToLower().EndsWith(".otf"))
                    throw new InvalidObjectException("Embedded Font file names must end with \".ttf\" or \".otf\".");
                // what is the assembly?
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
                // move it to the Application's CacheDir
                var inputStream = assembly.GetManifestResourceStream(fontFamily);
                if (inputStream == null)
                {
                    System.Console.WriteLine("Embedded Resource for FontFamily \"" + fontFamily + "\" not found.");
                    return null;
                }

                var cachedFontDir = new File(Xamarin.Forms.Forms.Context.CacheDir + "/ResourceFonts");
                if (!cachedFontDir.Exists())
                    cachedFontDir.Mkdir();

                var cachedFontFile = new File(cachedFontDir, fontFamily);
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
                Typeface typeface = Typeface.CreateFromFile(cachedFontFile);
                if (typeface == null)
                {
                    System.Console.WriteLine("Embedded Resource font file (" + fontFamily + ") could not be loaded as an Android Typeface.");
                    return null;
                }
                _fontFiles.Add(fontFamily, cachedFontFile.AbsolutePath);
                return typeface;
            }
            return null;
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
                var context = Xamarin.Forms.Forms.Context;
                var fontAssetFileNames = context.Assets.List("Fonts");
                var cachedFontDir = new File(context.CacheDir.AbsolutePath + "/AssetFonts");
                // move any Android Asset Fonts to the Applications CacheDir
                if (fontAssetFileNames != null)
                {
                    foreach (var fontAssetFileName in fontAssetFileNames)
                    {
                        if (!cachedFontDir.Exists())
                            cachedFontDir.Mkdir();
                        var cachedFontFile = new File(cachedFontDir.AbsolutePath, fontAssetFileName);
                        if (!cachedFontFile.Exists())
                        {
                            // copy into CacheDir
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
        int readByte()
        {
            return m_file.Read() & 0xFF;
        }

        int readWord()
        {
            int b1 = readByte();
            int b2 = readByte();
            return b1 << 8 | b2;
        }

        int readDword()
        {
            int b1 = readByte();
            int b2 = readByte();
            int b3 = readByte();
            int b4 = readByte();
            return b1 << 24 | b2 << 16 | b3 << 8 | b4;
        }

        void read(byte[] array)
        {
            if (m_file.Read(array) != array.Length)
                throw new IOException();
        }

        // Helper
        int getWord(byte[] array, int offset)
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
                int version = readDword();

                // The version must be either 'true' (0x74727565) or 0x00010000 or 'OTTO' (0x4f54544f) for CFF style fonts.
                if (version != 0x74727565 && version != 0x00010000 && version != 0x4f54544f)
                    return null;

                // The TTF file consist of several sections called "tables", and we need to know how many of them are there.
                int numTables = readWord();

                // Skip the rest in the header
                readWord(); // skip searchRange
                readWord(); // skip entrySelector
                readWord(); // skip rangeShift

                // Now we can read the tables
                for (int i = 0; i < numTables; i++)
                {
                    // Read the table entry
                    int tag = readDword();
                    readDword(); // skip checksum
                    int offset = readDword();
                    int length = readDword();

                    // Now here' the trick. 'name' field actually contains the textual string name.
                    // So the 'name' string in characters equals to 0x6E616D65
                    if (tag == 0x6E616D65)
                    {
                        // Here's the name section. Read it completely into the allocated buffer
                        var table = new byte[length];

                        m_file.Seek(offset);
                        read(table);

                        // This is also a table. See http://developer.apple.com/fonts/ttrefman/rm06/Chap6name.html
                        // According to Table 36, the total number of table records is stored in the second word, at the offset 2.
                        // Getting the count and string offset - remembering it's big endian.
                        int count = getWord(table, 2);
                        int string_offset = getWord(table, 4);

                        // Record starts from offset 6
                        for (int record = 0; record < count; record++)
                        {
                            // Table 37 tells us that each record is 6 words -> 12 bytes, and that the nameID is 4th word so its offset is 6.
                            // We also need to account for the first 6 bytes of the header above (Table 36), so...
                            int nameid_offset = record * 12 + 6;
                            int platformID = getWord(table, nameid_offset);
                            int nameid_value = getWord(table, nameid_offset + 6);

                            // Table 42 lists the valid name Identifiers. We're interested in 1 (Font Family Name) but not in Unicode encoding (for simplicity).
                            // The encoding is stored as PlatformID and we're interested in Mac encoding
                            if (nameid_value == 1 && platformID == 1)
                            {
                                // We need the string offset and length, which are the word 6 and 5 respectively
                                int name_length = getWord(table, nameid_offset + 8);
                                int name_offset = getWord(table, nameid_offset + 10);

                                // The real name string offset is calculated by adding the string_offset
                                name_offset = name_offset + string_offset;

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
            catch (FileNotFoundException e)
#pragma warning restore 0168
            {
                // Permissions?
                return null;
            }
#pragma warning disable 0168
            catch (IOException e)
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
                int version = readDword();

                // The version must be either 'true' (0x74727565) or 0x00010000 or 'OTTO' (0x4f54544f) for CFF style fonts.
                if (version != 0x74727565 && version != 0x00010000 && version != 0x4f54544f)
                    return null;

                // The TTF file consist of several sections called "tables", and we need to know how many of them are there.
                int numTables = readWord();

                // Skip the rest in the header
                readWord(); // skip searchRange
                readWord(); // skip entrySelector
                readWord(); // skip rangeShift

                // Now we can read the tables
                for (int i = 0; i < numTables; i++)
                {
                    // Read the table entry
                    int tag = readDword();
                    readDword(); // skip checksum
                    int offset = readDword();
                    int length = readDword();

                    // Now here' the trick. 'name' field actually contains the textual string name.
                    // So the 'name' string in characters equals to 0x6E616D65
                    if (tag == 0x6E616D65)
                    {
                        // Here's the name section. Read it completely into the allocated buffer
                        var table = new byte[length];

                        m_file.Seek(offset);
                        read(table);

                        // This is also a table. See http://developer.apple.com/fonts/ttrefman/rm06/Chap6name.html
                        // According to Table 36, the total number of table records is stored in the second word, at the offset 2.
                        // Getting the count and string offset - remembering it's big endian.
                        int count = getWord(table, 2);
                        int string_offset = getWord(table, 4);

                        // Record starts from offset 6
                        for (int record = 0; record < count; record++)
                        {
                            // Table 37 tells us that each record is 6 words -> 12 bytes, and that the nameID is 4th word so its offset is 6.
                            // We also need to account for the first 6 bytes of the header above (Table 36), so...
                            int nameid_offset = record * 12 + 6;
                            int platformID = getWord(table, nameid_offset);
                            int nameid_value = getWord(table, nameid_offset + 6);

                            // Table 42 lists the valid name Identifiers. We're interested in 1 (Font Subfamily Name) but not in Unicode encoding (for simplicity).
                            // The encoding is stored as PlatformID and we're interested in Mac encoding
                            if (nameid_value == 2 && platformID == 1)
                            {
                                // We need the string offset and length, which are the word 6 and 5 respectively
                                int name_length = getWord(table, nameid_offset + 8);
                                int name_offset = getWord(table, nameid_offset + 10);

                                // The real name string offset is calculated by adding the string_offset
                                name_offset = name_offset + string_offset;

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
            catch (FileNotFoundException e)
#pragma warning restore 0168
            {
                // Permissions?
                return null;
            }
#pragma warning disable 0168
            catch (IOException e)
#pragma warning restore 0168
            {
                // Most likely a corrupted font file
                return null;
            }
        }
    }

}

