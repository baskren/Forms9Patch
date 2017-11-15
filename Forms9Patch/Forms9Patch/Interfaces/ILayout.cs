using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    interface ILayout : IBackground
    {
        // IMPORTANT: Need to override (new) ILayoutElement's Padding property in order to correctly compute & store shadow padding
        //Xamarin.Forms.Thickness Padding { get; set; }
        bool IgnoreChildren { get; set; }
    }
}
