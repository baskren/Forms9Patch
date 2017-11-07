using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    interface IBackground : IRoundedBox
    {
        /// <summary>
        /// Gets or sets the padding.
        /// </summary>
        /// <value>The padding.</value>
        Xamarin.Forms.Thickness Padding { get; set; }


        Forms9Patch.Image BackgroundImage { get; set; }
    }


}
