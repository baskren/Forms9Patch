using System;
using System.Collections.Generic;
using Foundation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace Forms9Patch.iOS
{
    static class ImageViewManagerExtension
    {
        static readonly Dictionary<string, NSData> Cache = new Dictionary<string, NSData>();
        static readonly Dictionary<string, List<ImageViewManager>> Clients = new Dictionary<string, List<ImageViewManager>>();

        static readonly object _constructorLock = new object();

        internal static bool Same(Xamarin.Forms.ImageSource thisSource, Xamarin.Forms.ImageSource otherSource)
        {

            if (thisSource == otherSource)
                return true;

            var thisStreamSource = thisSource as Xamarin.Forms.StreamImageSource;
            var otherStreamSource = otherSource as Xamarin.Forms.StreamImageSource;
            if (thisStreamSource != null && otherStreamSource != null)
            {
                if (thisStreamSource.GetValue(ImageSource.AssemblyProperty) != otherStreamSource.GetValue(ImageSource.AssemblyProperty))
                    return false;
                return thisStreamSource.GetValue(ImageSource.PathProperty) != otherStreamSource.GetValue(ImageSource.PathProperty) ? false : true;
            }

            var thisFileSource = thisSource as Xamarin.Forms.FileImageSource;
            var otherFileSource = otherSource as Xamarin.Forms.FileImageSource;
            return thisFileSource != null && otherFileSource != null && thisFileSource.File == otherFileSource.File;
        }

#pragma warning disable 1998
        internal static async Task<NSData> FetchResourceData(this ImageViewManager client, Xamarin.Forms.ImageSource streamSource, int _i)
#pragma warning restore 1998
        {
            string path = (string)streamSource?.GetValue(ImageSource.PathProperty);
            if (path == null)
            {
                //System.Diagnostics.Debug.WriteLine ("{0} Fetch.path==null", _i);
                return null;
            }
            //System.Diagnostics.Debug.WriteLine ("{0} Fetch.path={1}", _i,path);

            lock (_constructorLock)
            {
                //System.Diagnostics.Debug.WriteLine ("\t\tF0");
                NSData result;
                if (Cache.ContainsKey(path))
                {
                    //System.Diagnostics.Debug.WriteLine ("\t\tF0.1");
                    Clients[path].Add(client);
                    //System.Diagnostics.Debug.WriteLine ("{0} Fetch.SUCCESS CACHE", _i);
                    //System.Diagnostics.Debug.WriteLine ("\t\tF0.2");
                    return Cache[path];
                }
                //System.Diagnostics.Debug.WriteLine ("\t\tF1");
                //var stream = streamSource.Stream (default(CancellationToken));
                var assembly = (Assembly)streamSource.GetValue(ImageSource.AssemblyProperty);
                //System.Diagnostics.Debug.WriteLine ("\t\tF2");
                var stream = assembly.GetManifestResourceStream(path);
                //System.Diagnostics.Debug.WriteLine ("\t\tF3");
                if (stream == null)
                {
                    //System.Diagnostics.Debug.WriteLine ("\t\tF4.1");
                    //System.Diagnostics.Debug.WriteLine ("{0} Fetch.stream==null", _i);
                    return null;
                }
                //System.Diagnostics.Debug.WriteLine ("\t\tF5");
                result = NSData.FromStream(stream);
                stream.Close();
                //System.Diagnostics.Debug.WriteLine ("\t\tF6");
                if (result == null)
                {
                    //System.Diagnostics.Debug.WriteLine ("\t\tF6.1");
                    //System.Diagnostics.Debug.WriteLine ("{0} Fetch.result==null", _i);
                    return null;
                }
                //System.Diagnostics.Debug.WriteLine ("\t\tF7");
                Cache[path] = result;
                //System.Diagnostics.Debug.WriteLine ("\t\tF8");
                if (!Clients.ContainsKey(path))
                {
                    //System.Diagnostics.Debug.WriteLine ("\t\tF8.1");
                    Clients[path] = new List<ImageViewManager>();
                }
                //System.Diagnostics.Debug.WriteLine ("\t\tF9");
                Clients[path].Add(client);
                //System.Diagnostics.Debug.WriteLine ("\t\tF10");
                //System.Diagnostics.Debug.WriteLine ("{0} Fetch.SUCCESS NEW", _i);

                return result;
            }
        }



        internal static void ReleaseStreamData(this ImageViewManager client, Xamarin.Forms.BindableObject streamSource)
        {
            var path = (string)streamSource?.GetValue(ImageSource.PathProperty);
            if (path == null)
                return;
            lock (_constructorLock)
            {
                //System.Diagnostics.Debug.WriteLine ("\t\tR0");
                if (Clients.ContainsKey(path))
                {
                    var clients = Clients[path];
                    //System.Diagnostics.Debug.WriteLine ("\t\tR1");
                    clients?.Remove(client);
                    //System.Diagnostics.Debug.WriteLine ("\t\tR2");
                    if (clients.Count > 0)
                    {
                        //System.Diagnostics.Debug.WriteLine ("\t\tR2.1");
                        return;
                    }
                    //System.Diagnostics.Debug.WriteLine ("\t\tR3");
                    Clients.Remove(path);
                }

                //System.Diagnostics.Debug.WriteLine ("\t\tR4");
                if (Cache.ContainsKey(path))
                    Cache.Remove(path);
                //System.Diagnostics.Debug.WriteLine ("\t\tR5");
            }
        }
    }
}

