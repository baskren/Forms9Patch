
namespace Forms9Patch
{
    static class ExtendedElementShapeExtensions
    {
        public static bool IsSegment(this IShape element)
        {
            switch(element.ExtendedElementShape)
            {
                case ExtendedElementShape.SegmentEnd:
                case ExtendedElementShape.SegmentMid:
                case ExtendedElementShape.SegmentStart:
                    return true;
            }
            return false;
        }
    }
}
