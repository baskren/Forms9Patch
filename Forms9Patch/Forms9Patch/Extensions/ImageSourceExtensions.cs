
namespace Forms9Patch
{
    /// <summary>
    /// ImageSource extension methods
    /// </summary>
    public static class ImageSourceExtensions
    {
        /// <summary>
        /// Determins if two ImageSources are the same
        /// </summary>
        /// <param name="thisSource"></param>
        /// <param name="otherSource"></param>
        /// <returns></returns>
        public static bool SameAs(this Xamarin.Forms.ImageSource thisSource, Xamarin.Forms.ImageSource otherSource)
        {
            if (thisSource == otherSource)
                return true;
            if (thisSource is Xamarin.Forms.StreamImageSource thisStreamSource && otherSource is Xamarin.Forms.StreamImageSource otherStreamSource)
            {
                if (thisStreamSource.GetValue(ImageSource.AssemblyProperty) != otherStreamSource.GetValue(ImageSource.AssemblyProperty))
                    return false;
                return thisStreamSource.GetValue(ImageSource.EmbeddedResourceIdProperty) != otherStreamSource.GetValue(ImageSource.EmbeddedResourceIdProperty) ? false : true;
            }
            return thisSource is Xamarin.Forms.FileImageSource thisFileSource && otherSource is Xamarin.Forms.FileImageSource otherFileSource && thisFileSource.File == otherFileSource.File;
        }

        internal static string ImageSourceKey(this Xamarin.Forms.ImageSource imageSource)
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
