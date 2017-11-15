using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    static class ElementShapeExtensions
    {
        public static bool IsSegment(this IShape element)
        {
            switch(element.ElementShape)
            {
                case ElementShape.SegmentEnd:
                case ElementShape.SegmentMid:
                case ElementShape.SegmentStart:
                    return true;
            }
            return false;
        }
    }
}
