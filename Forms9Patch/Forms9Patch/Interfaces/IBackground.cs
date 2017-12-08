using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    public interface IBackground : IShape
    {
        Forms9Patch.Image BackgroundImage { get; set; }
    }


}
