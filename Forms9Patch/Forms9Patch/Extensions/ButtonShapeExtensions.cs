using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    static class ButtonShapeExtensions
    {
        public static ElementShape ToElementShape(this ButtonShape buttonShape)
        {
            switch(buttonShape)
            {
                case ButtonShape.Square:        return ElementShape.Square;
                case ButtonShape.Rectangle:     return ElementShape.Rectangle;
                case ButtonShape.Circle:        return ElementShape.Circle;
                case ButtonShape.Elliptical:    return ElementShape.Elliptical;
                case ButtonShape.Obround:       return ElementShape.Obround;
            }
            throw new NotSupportedException("ButtonShape ["+buttonShape+"] cannot be converted to ElementShape");
        }
    }
}
