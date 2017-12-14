using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    /// <summary>
    /// Shape of Forms9Patch Image, Button, or Layout element
    /// </summary>
    public enum ElementShape
    {
        /// <summary>
        /// Rectangle
        /// </summary>
        Rectangle = 0,
        /// <summary>
        /// Square
        /// </summary>
        Square,
        /// <summary>
        /// Circle
        /// </summary>
        Circle,
        /// <summary>
        /// Ellipse
        /// </summary>
        Elliptical,
        /// <summary>
        /// Obround (rectangle with circular ends)
        /// </summary>
        Obround,
    }
}
