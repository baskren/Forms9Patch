using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    interface IImage : IShape

    {
        /// <summary>
        /// 
        /// </summary>
        Xamarin.Forms.ImageSource Source { get; set; }

        bool IsLoading { get; }

        //bool IsOpaque { get; set; }

        Forms9Patch.Fill Fill { get; set; }

        Xamarin.Forms.Thickness CapInsets { get; set; }

        Xamarin.Forms.Thickness ContentPadding { get; }

        Xamarin.Forms.Color TintColor { get; set; }

        Xamarin.Forms.Size SourceImageSize { get; }
    }
}
