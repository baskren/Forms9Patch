using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    class MetaFont
    {
        public string Family { get; set; }
        public double Size { get; set; }
        public bool Italic { get; set; }
        public bool Bold { get; set; }
        public FontBaseline Baseline { get; set; }

        public MetaFont(string family, double size, bool bold = false, bool italic = false)
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

        public static bool operator ==(MetaFont f1, MetaFont f2) => f1.Equals(f2);

        public static bool operator !=(MetaFont f1, MetaFont f2) => !f1.Equals(f2);

        public override bool Equals(object obj)
        {
            if (obj is MetaFont other)
            {
                if (Size != other.Size)
                    return false;
                if (Bold != other.Bold)
                    return false;
                if (Italic != other.Italic)
                    return false;
                if (Family != other.Family)
                    return false;
                return Baseline == other.Baseline;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + Family.GetHashCode();
            hash = hash * 23 + Size.GetHashCode();
            hash = hash * 23 + Bold.GetHashCode();
            hash = hash * 23 + Italic.GetHashCode();
            hash = hash * 23 + Baseline.GetHashCode();
            return hash;
        }

    }
}
