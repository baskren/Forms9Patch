using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    public class FontSource
    {
        public string FamilyName { get; set; }

        internal string XamarinFormsFontFamily { get; set; }

        public static implicit operator string(FontSource source)
        {
            return source.XamarinFormsFontFamily;
        }

        public static FontSource FromEmbeddedResource(string embeddedResourceId, Assembly assembly = null)
        {
            if (string.IsNullOrWhiteSpace(Settings.LicenseKey) && Xamarin.Forms.Application.Current != null)
                throw new Exception("Forms9Patch is has not been initialized. Xamarin.Forms.Application.Current=[" + Xamarin.Forms.Application.Current + "]");
            if (assembly == null)
                assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);
            var result = Task.Run(async () => await FontExtensions.GetEmbeddedResourceFontSourceAsync(embeddedResourceId, assembly)).Result;
            return result;
        }

        public static FontSource FromFileSource(PCLStorage.IFile file)
        {
            var result = Task.Run(async () => await FontExtensions.GetFileFontSourceAsync(file)).Result;
            return result;
        }

        public static FontSource FromUrlFontSource(string url)
        {
            var result = Task.Run(async () => await FontExtensions.GetUrlFontSource(url)).Result;
            return result;
        }

        public static FontSource FromRuntimePlatform(string iOS_path=null, string androidPath=null, string uwpPath=null, string otherPath=null)
        {
            string path = null;
            switch(Xamarin.Forms.Device.OS)
            {
                case Xamarin.Forms.TargetPlatform.Android:
                    path = androidPath;
                    break;
                case Xamarin.Forms.TargetPlatform.iOS:
                    path = iOS_path;
                    break;
                case Xamarin.Forms.TargetPlatform.Windows:
                    path = uwpPath;
                    break;
                case Xamarin.Forms.TargetPlatform.Other:
                    path = otherPath;
                    break;
            }
            var result = new FontSource { XamarinFormsFontFamily = path };
            //TOOD: get the FamilyName!
            return result;
        }
    }

    internal class EmbeddedResourceFontSource : FontSource
    {
        public string EmbeddedResourceId { get; set; }

        public Assembly Assembly { get; set; }
    }

    internal class UrlFontSource : FontSource
    {
        public string Url { get; set; }
    }

    internal class FileFontSource : FontSource
    {
        public PCLStorage.IFile File { get; set; }
    }

    internal class RuntimePlatformFontSource
    {
        public string Path { get; set; }


    }


    // StreamFontSource not implemented because converted streams cannot be identified as redundant - thus preventing caching.  
    // Unlike images, native systems do not have provisions for stream source fonts, so this is a problem
}
