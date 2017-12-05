using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    interface ILayout : IBackground
    {
        Xamarin.Forms.Thickness Padding { get; set; }
        bool IgnoreChildren { get; set; }
    }
}
