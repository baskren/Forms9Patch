using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Forms9Patch.UWP
{
    static class ImageViewManagerExtensions
    {
        //static readonly Dictionary<string, >
        static readonly Dictionary<string, BitmapImage> Cache = new Dictionary<string, BitmapImage>();
        static readonly Dictionary<string, List<ImageView>> ImageViews = new Dictionary<string, List<ImageView>>();
        static readonly object _constructorLock = new object();
        //static readonly Dictionary<string, object> ConstructionLocks = new Dictionary<string, object>();

#pragma warning disable 1998
        internal static async Task<BitmapImage> FetchResourceData(this ImageView imageViewManager, Xamarin.Forms.ImageSource streamSource, int _i)
#pragma warning restore 1998
        {
            string path = (string)streamSource?.GetValue(ImageSource.PathProperty);
            if (path == null)
                return null;

            if (Cache.ContainsKey(path))
            {
                ImageViews[path].Add(imageViewManager);
                var result = Cache[path];
                if (result == null)
                    throw new Exception();
                return Cache[path];
            }

            //lock (_constructorLock)
            //{
            BitmapImage bitmap = null;
            var assembly = (Assembly)streamSource.GetValue(ImageSource.AssemblyProperty);
            using (var stream = assembly.GetManifestResourceStream(path).AsRandomAccessStream())
            {
                if (stream == null)
                    return null;
                bitmap = new BitmapImage();
                stream.Seek(0);
                Cache[path] = bitmap;
                if (!ImageViews.ContainsKey(path))
                    ImageViews[path] = new List<ImageView>();
                ImageViews[path].Add(imageViewManager);
                await bitmap.SetSourceAsync(stream);
            }
            if (bitmap==null)
                System.Diagnostics.Debug.WriteLine("");
            return bitmap;
                //}
        }



        internal static void ReleaseStreamData(this ImageView imageViewManager, Xamarin.Forms.BindableObject streamSource)
        {
            var path = (string)streamSource?.GetValue(ImageSource.PathProperty);
            if (path == null)
                return;
            lock (_constructorLock)
            {
                //System.Diagnostics.Debug.WriteLine ("\t\tR0");
                var clients = ImageViews[path];
                //System.Diagnostics.Debug.WriteLine ("\t\tR1");
                clients?.Remove(imageViewManager);
                //System.Diagnostics.Debug.WriteLine ("\t\tR2");
                if (clients.Count > 0)
                {
                    //System.Diagnostics.Debug.WriteLine ("\t\tR2.1");
                    return;
                }
                //System.Diagnostics.Debug.WriteLine ("\t\tR3");
                ImageViews.Remove(path);
                //System.Diagnostics.Debug.WriteLine ("\t\tR4");
                Cache.Remove(path);
                //System.Diagnostics.Debug.WriteLine ("\t\tR5");
            }
        }

    }
}
