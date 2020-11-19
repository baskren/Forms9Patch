using System;
using System.Collections.Generic;

namespace Forms9Patch.Droid
{
    internal class FontRecord
    {
        public string FamilyName { get; }

        public string Style { get; }

        public string Path { get; }

        public FontRecord(string familyName, string style, string path)
        {
            FamilyName = familyName;
            Style = style;
            Path = path;
        }
    }
}
