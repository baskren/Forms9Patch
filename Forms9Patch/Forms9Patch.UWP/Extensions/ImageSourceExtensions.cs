using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch.UWP
{
    internal static class ImageSourceExtensions
    {
        public static string ImageSourceKey(this Xamarin.Forms.ImageSource imageSource)
        {
            if (imageSource.GetValue(Forms9Patch.ImageSource.EmbeddedResourceIdProperty) is string resourceId)
                    return "eri:" + resourceId;
            if (imageSource is Xamarin.Forms.FileImageSource fileSource)
                return "file:" + fileSource.File;
            if (imageSource is Xamarin.Forms.UriImageSource uriSource)
                return "uri:" + uriSource.Uri;
            return null;
        }
    }


}
