using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch.UWP
{
    struct SizeI
    {
        public int Width;
        public int Height;

        public SizeI(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return "[" + Width + "," + Height + "]";
        }
    }
}
