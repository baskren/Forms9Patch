using System;
using System.Collections.Generic;
using System.Linq;
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
