using PCL.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch.UWP
{
    enum FontBaseline
    {
        Normal = 0,
        Superscript = 1,
        Subscript = 2,
        Numerator = 3,
        Denominator = 4,
    }

    class FontExtensions
    {
        
        internal static string EmbeddedFontFamilyName(string resourceID)
        {
            if (resourceID == "STIXGeneral")
                resourceID = "Forms9Patch.Resources.Fonts.STIXGeneral.otf";

            if (_embeddedResourceFonts.ContainsKey(resourceID))
                return  _embeddedResourceFonts[resourceID];

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
                    var assembly = ReflectionExtensions.GetAssemblyByName(assemblyName);
                    if (assembly == null)
                    {
                        // try using the current application assembly instead (as is the case with Shared Applications)
                        //assembly = ReflectionExtensions.GetAssemblyByName(assemblyName + ".Droid");
                        assembly = Forms9Patch.ApplicationInfoService.Assembly;
                        //Console.WriteLine ("Assembly for Resource ID \"" + resourceID + "\" not found.");
                        //return null;
                    }
                    // get font's name
                    var analyzer = new TTFAnalyzer();
                    var name = analyzer.FontFamily(assembly, resourceID);
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        var uwpPathComponents = resourceID.Split('.');
                        var uwpPath = "";
                        for (int i = 0; i < uwpPathComponents.Count(); i++)
                        {
                            uwpPath += uwpPathComponents[i];
                            if (i < uwpPathComponents.Count() - 2)
                                uwpPath += "/";
                            else if (i < uwpPathComponents.Count() - 1)
                                uwpPath += ".";
                        }
                        var fontFamilyName = "/" + assembly.FullName + ";" + uwpPath + "#" + name;
                    }
                }
                //} else {
                //	Console.WriteLine ("Font [] is assumed not to be an embedded resource because it does not contain \".Resources.Fonts.\" in its Resource ID");
            }
            return null;
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        internal class MetaFont
        {
            public string Family { get; set; }
            public float Size { get; set; }
            public bool Italic { get; set; }
            public bool Bold { get; set; }
            public FontBaseline Baseline { get; set; }

            public MetaFont(string family, float size, bool bold = false, bool italic = false)
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

        struct FontKey
        {
#pragma warning disable 0414
            string Family;
            float Size;
            bool Bold;
            bool Italic;
#pragma warning restore 0414

            internal FontKey(string family, float size, bool bold, bool italic)
            {
                Family = family;
                Size = size;
                Bold = bold;
                Italic = italic;
            }
        }

        class TTFAnalyzer
        {

            // Font file; must be seekable
            // m_file;
            //BinaryReader m_binaryReader;

            /*
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

            int binaryReader.ReadInt32()
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
            */

            // Helper
            int getWord(byte[] array, int offset)
            {
                int b1 = array[offset] & 0xFF;
                int b2 = array[offset + 1] & 0xFF;
                return b1 << 8 | b2;
            }

            public string FontFamily(System.Reflection.Assembly assembly, string resourceID)
            {
                try
                {
                    // Parses the TTF file format.
                    // See http://developer.apple.com/fonts/ttrefman/rm06/Chap6.html
                    //m_file = new RandomAccessFile(fontFilename, "r");
                    using (var stream = assembly?.GetManifestResourceStream(resourceID))
                    {
                        using (var binaryReader = new BinaryReader(stream))
                        {
                            // Read the version first
                            int version = binaryReader.ReadInt32();

                            // The version must be either 'true' (0x74727565) or 0x00010000 or 'OTTO' (0x4f54544f) for CFF style fonts.
                            if (version != 0x74727565 && version != 0x00010000 && version != 0x4f54544f)
                                return null;

                            // The TTF file consist of several sections called "tables", and we need to know how many of them are there.
                            int numTables = binaryReader.ReadInt16();

                            // Skip the rest in the header
                            binaryReader.ReadInt16(); // skip searchRange
                            binaryReader.ReadInt16(); // skip entrySelector
                            binaryReader.ReadInt16(); // skip rangeShift

                            // Now we can read the tables
                            for (int i = 0; i < numTables; i++)
                            {
                                // Read the table entry
                                int tag = binaryReader.ReadInt32();
                                binaryReader.ReadInt32(); // skip checksum
                                int offset = binaryReader.ReadInt32();
                                int length = binaryReader.ReadInt32();

                                // Now here' the trick. 'name' field actually contains the textual string name.
                                // So the 'name' string in characters equals to 0x6E616D65
                                if (tag == 0x6E616D65)
                                {
                                    // Here's the name section. Read it completely into the allocated buffer
                                    var table = new byte[length];

                                    //m_file.Seek(offset);
                                    binaryReader.BaseStream.Seek(offset, SeekOrigin.Current);
                                    //read(table);
                                    binaryReader.Read(table, 0, length);

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
                                                //var str = System.Text.Encoding.Default.GetString(chars);
                                                var str = System.Text.Encoding.UTF8.GetString(chars);
                                                return str;
                                            }
                                        }
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

            public string FontAttributes(System.Reflection.Assembly assembly, string resourceID)
            {
                try
                {
                    // Parses the TTF file format.
                    // See http://developer.apple.com/fonts/ttrefman/rm06/Chap6.html
                    using (var stream = assembly?.GetManifestResourceStream(resourceID))
                    {
                        using (var binaryReader = new BinaryReader(stream))
                        {
                            // Read the version first
                            int version = binaryReader.ReadInt32();

                            // The version must be either 'true' (0x74727565) or 0x00010000 or 'OTTO' (0x4f54544f) for CFF style fonts.
                            if (version != 0x74727565 && version != 0x00010000 && version != 0x4f54544f)
                                return null;

                            // The TTF file consist of several sections called "tables", and we need to know how many of them are there.
                            int numTables = binaryReader.ReadInt16();

                            // Skip the rest in the header
                            binaryReader.ReadInt16(); // skip searchRange
                            binaryReader.ReadInt16(); // skip entrySelector
                            binaryReader.ReadInt16(); // skip rangeShift

                            // Now we can read the tables
                            for (int i = 0; i < numTables; i++)
                            {
                                // Read the table entry
                                int tag = binaryReader.ReadInt32();
                                binaryReader.ReadInt32(); // skip checksum
                                int offset = binaryReader.ReadInt32();
                                int length = binaryReader.ReadInt32();

                                // Now here' the trick. 'name' field actually contains the textual string name.
                                // So the 'name' string in characters equals to 0x6E616D65
                                if (tag == 0x6E616D65)
                                {
                                    // Here's the name section. Read it completely into the allocated buffer
                                    var table = new byte[length];

                                    //m_file.Seek(offset);
                                    binaryReader.BaseStream.Seek(offset, SeekOrigin.Current);
                                    //read(table);
                                    binaryReader.Read(table, 0, length);

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
                                                byte[] chars = new byte[name_length];
                                                System.Buffer.BlockCopy(table, name_offset, chars, 0, name_length);
                                                //var str = new string(chars);
                                                //var str = System.Text.Encoding.Default.GetString(chars);
                                                var str = System.Text.Encoding.UTF8.GetString(chars);
                                                return str;
                                            }
                                        }
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
}
