using System;

namespace Forms9Patch
{
    static class ElementShapeExtensions
    {
        public static ExtendedElementShape ToExtendedElementShape(this ElementShape buttonShape)
        {
            switch(buttonShape)
            {
                case ElementShape.Square:        return ExtendedElementShape.Square;
                case ElementShape.Rectangle:     return ExtendedElementShape.Rectangle;
                case ElementShape.Circle:        return ExtendedElementShape.Circle;
                case ElementShape.Elliptical:    return ExtendedElementShape.Elliptical;
                case ElementShape.Obround:       return ExtendedElementShape.Obround;
            }
            throw new NotSupportedException("ElementShape ["+buttonShape+"] cannot be converted to ExtendedElementShape");
        }
    }
}
