using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    interface IElement
    {
        /// <summary>
        /// Incremental instance id (starting at zero, increasing by one for each new instance)
        /// </summary>
        int InstanceId { get; }
    }
}
