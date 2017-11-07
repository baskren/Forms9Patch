using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    interface IImage : IRoundedBox
    {
        /// <summary>
        /// 
        /// </summary>
        Xamarin.Forms.ImageSource Source { get; set; }
    }
}
