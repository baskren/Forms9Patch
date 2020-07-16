using System;
using System.Collections.Generic;
using System.Text;

namespace Forms9Patch
{
    /// <summary>
    /// Attribute that makes sure classes are preserved by the linker
    /// </summary>
    internal sealed class PreserveAttribute : System.Attribute
    {
        public bool AllMembers;
        public bool Conditional;
    }
}
