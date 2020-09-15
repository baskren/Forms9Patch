using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch ImageSource.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class ImageSource : Xamarin.Forms.ImageSource
    {
        internal static readonly BindableProperty ImageScaleProperty = BindableProperty.CreateAttached("ImageScale", typeof(float), typeof(ImageSource), 1.0f);
        internal static readonly BindableProperty EmbeddedResourceIdProperty = BindableProperty.CreateAttached("EmbeddedResourceId", typeof(string), typeof(ImageSource), null);
        internal static readonly BindableProperty AssemblyProperty = BindableProperty.CreateAttached("Assembly", typeof(Assembly), typeof(ImageSource), null);

        #region Constructor
        static ImageSource()
        {
            Settings.ConfirmInitialization();
        }

        ImageSource() //: this (1) 
        {
        }

        /*
		ImageSource(int i) : this() {
		}
		*/

        class ImageSourceContainer
        {
            public string Path;
            public float Scale;
            public ImageSourceContainer(string path, float scale)
            {
                Path = path;
                Scale = scale;
            }
        }
        #endregion


        #region Static Methods
        /// <summary>
        /// Use a SVG string as a image source for a Forms9Patch image
        /// </summary>
        /// <param name="svgText"></param>
        /// <returns></returns>
        public static Xamarin.Forms.ImageSource FromSvgText(string svgText)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(svgText);
            var stream = new MemoryStream(byteArray);
            return FromStream(() => stream);
        }

        /// <summary>
        /// Cached selection of best fit multi-device / multi-resolution image embedded resource 
        /// </summary>
        /// <returns>Xamarin.Forms.ImageSource</returns>
        /// <param name="resourceId">ResourceID without extension, resolution modifier, or device modifier</param>
        /// <param name="assembly">Assembly in which the resource can be found</param> 
        public static Xamarin.Forms.ImageSource FromMultiResource(string resourceId, Assembly assembly = null)
        {
            assembly = EmbeddedResourceExtensions.FindAssemblyForMultiResource(resourceId, assembly);
            /*
            assembly = assembly ?? AssemblyExtensions.AssemblyFromResourceId(resourceId);
            if (assembly == null && Device.RuntimePlatform != Device.UWP)
                assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly")?.Invoke(null, new object[0]);
                */

            if (assembly == null)
                return null;
            var r = BestGuessResource(resourceId, assembly);
            var path = r == null ? resourceId : r.Path;
            var imageSource = Xamarin.Forms.ImageSource.FromResource(path, assembly);
            if (imageSource != null)
            {
                imageSource.SetValue(ImageScaleProperty, r == null ? 1.0f : r.Scale);
                imageSource.SetValue(EmbeddedResourceIdProperty, path);
                imageSource.SetValue(AssemblyProperty, assembly);
            }
            return imageSource;
        }

        /// <summary>
        /// Cached selection of resource (literally - no automated selection of device, resolution, or extension).
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="resourceId">Path.</param>
        /// <param name="assembly">Assembly.</param>
        public static new Xamarin.Forms.ImageSource FromResource(string resourceId, Assembly assembly = null)
        {
            assembly = EmbeddedResourceExtensions.FindAssemblyForResource(resourceId, assembly);
            /*
            assembly = assembly ?? AssemblyExtensions.AssemblyFromResourceId(resourceId);
            if (assembly == null && Device.RuntimePlatform != Device.UWP)
                assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);
                */
            var imageSource = Xamarin.Forms.ImageSource.FromResource(resourceId, assembly);
            if (imageSource != null)
            {
                imageSource.SetValue(ImageScaleProperty, 1.0f);
                imageSource.SetValue(EmbeddedResourceIdProperty, resourceId);
                imageSource.SetValue(AssemblyProperty, assembly);
            }
            return imageSource;
        }

        /// <summary>
        /// Load an EmbeddedResource as a Xamarin.Forms.FileImageSource
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Xamarin.Forms.FileImageSource FromResourceAsFile(string resourceId, Assembly assembly = null)
        {
            assembly = EmbeddedResourceExtensions.FindAssemblyForResource(resourceId, assembly);
            /*
            assembly = assembly ?? AssemblyExtensions.AssemblyFromResourceId(resourceId);
            if (assembly == null && Device.RuntimePlatform != Device.UWP)
                assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly")?.Invoke(null, new object[0]);
                */
            if (assembly == null)
                return null;
            var r = BestGuessResource(resourceId, assembly);
            var path = r == null ? resourceId : r.Path;

            var localPath = P42.Utils.EmbeddedResourceCache.LocalStorageFullPathForEmbeddedResource(path, assembly);
            var imageSource = new FileImageSource { File = localPath };
            if (imageSource != null)
            {
                imageSource.SetValue(ImageScaleProperty, 1.0f);
                imageSource.SetValue(EmbeddedResourceIdProperty, resourceId);
                imageSource.SetValue(AssemblyProperty, assembly);
            }
            return imageSource;
        }

        /*
		/// <summary>
		/// Sets image source to file.
		/// </summary>
		/// <returns>The file.</returns>
		/// <param name="path">Path.</param>
		public static Xamarin.Forms.ImageSource FromFile(string path) {
			var imageSource = Xamarin.Forms.ImageSource.FromFile (path);
			if (imageSource != null) {
				imageSource.SetValue (ImageSource.PathProperty, path);
			}
			return imageSource;
		}

		/// <summary>
		/// Froms the URI.
		/// </summary>
		/// <returns>The URI.</returns>
		public static Xamarin.Forms.ImageSource FromUri(Uri uri) {
			var imageSource = Xamarin.Forms.ImageSource.FromUri (uri);
			if (imageSource != null) {
				imageSource.SetValue (ImageSource.PathProperty, uri.AbsolutePath);
			}
			return imageSource;
		}

		/// <summary>
		/// Froms the stream.
		/// </summary>
		/// <returns>The stream.</returns>
		public static Xamarin.Forms.ImageSource FromStream(Func<System.IO.Stream> stream) {
			var imageSource = Xamarin.Forms.ImageSource.FromStream (stream);
			if (imageSource != null) {
				imageSource.SetValue (ImageSource.PathProperty, stream.ToString());
			}
			return imageSource;
		}
		*/
        #endregion


        #region Path Parsing 
        static Tuple<string, string> GetiOSBasePathAndExt(string pathString)
        {
            if (pathString == null)
                return null;
            var reqResExt = null as string;
            var reqResBasePath = pathString;
            //var reqResSplit = pathString.Split(new char[] { '.', '/', '\\' });
            var reqResSplit = pathString.Split('.');
            if (reqResSplit.Length > 1 && ValidImageExtensions.Contains(reqResSplit[reqResSplit.Length - 1].ToLower()))
            {
                reqResExt = reqResSplit[reqResSplit.Length - 1];
                reqResBasePath = pathString.Substring(0, pathString.Length - reqResExt.Length - 1);
            }
            if (reqResExt == "png" && reqResSplit.Length > 2 && reqResSplit[reqResSplit.Length - 2] == "9")
            {
                reqResExt = "9.png";
                reqResBasePath = reqResBasePath.Substring(0, reqResBasePath.Length - 2);
            }
            return new Tuple<string, string>(reqResBasePath, reqResExt);
        }

        static Tuple<string, string, string> GetDroidBasePathFileAndExt(string pathString)
        {
            if (pathString == null)
                return null;
            //var reqResSplit = pathString.Split(new char[] { '.', '/', '\\' }).ToList();
            var reqResSplit = pathString.Split('.');
            if (reqResSplit.Length > 1)
            {
                //var reqResIndex = reqResSplit.IndexOf("Resources");
                var reqResIndex = -1;
                for (int i = 0; i < reqResSplit.Length; i++)
                    if (reqResSplit[i] == "Resources")
                    {
                        reqResIndex = i;
                        break;
                    }


                if (reqResIndex >= 0)
                {
                    var index = 0;
                    for (int i = 0; i <= reqResIndex; i++)
                        index += reqResSplit[i].Length + 1;
                    if (index < pathString.Length)
                    {
                        var reqResBaseName = pathString.Substring(index);
                        var tuple = GetiOSBasePathAndExt(reqResBaseName);
                        return new Tuple<string, string, string>(pathString.Substring(0, index), tuple.Item1, tuple.Item2);
                    }
                }
            }
            return null;
        }
        #endregion


        #region Resource Resolution

        internal static string BestEmbeddedMultiResourceMatch(string resourceId, Assembly assembly)
        {
            assembly = EmbeddedResourceExtensions.FindAssemblyForMultiResource(resourceId, assembly);
            /*
            assembly = assembly ?? AssemblyExtensions.AssemblyFromResourceId(resourceId);
            if (assembly == null && Device.RuntimePlatform != Device.UWP)
                assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);
                */
            var r = BestGuessResource(resourceId, assembly);
            return r?.Path;
        }

        static ImageSourceContainer BestGuessResource(string pathString, Assembly assembly)
        {
            ImageSourceContainer result;
            result = BestGuessF9PResource(pathString, assembly);
            if (result == null)
            {
                System.Diagnostics.Debug.WriteLine("[Forms9Patch.ImageSource.FromMultiResource] alternative resource not found for: " + pathString);
                System.Console.WriteLine("[Forms9Patch.ImageSource.FromMultiResource] alternative resource not found for: " + pathString);
            }
            return result;
        }

        readonly static Dictionary<Assembly, string[]> SortedAppleResources = new Dictionary<Assembly, string[]>();
        static ImageSourceContainer BestGuessF9PResource(string reqResourcePathString, Assembly assembly)
        {
            if (assembly == null)
                return null;
            var tuple = GetiOSBasePathAndExt(reqResourcePathString);
            if (tuple == null)
                return null;
            var reqResBaseName = tuple.Item1;
            var reqResExt = tuple.Item2;


            string[] resourceNames = null;
            if (SortedAppleResources.ContainsKey(assembly))
                resourceNames = SortedAppleResources[assembly];
            if (resourceNames == null)
            {
                SortedAppleResources[assembly] = assembly.GetManifestResourceNames();
                //SortedAppleResources = SortedAppleResources.Where(arg => !arg.Contains(".drawable")).ToArray();
                Array.Sort<string>(SortedAppleResources[assembly]);
                resourceNames = SortedAppleResources[assembly];
            }

            //resourceNames = resourceNames.Where(arg => arg.Contains(reqResBaseName)).ToArray();
            var matchingResourceNames = new List<string>();
            for (int i = 0; i < resourceNames.Length; i++)
            {
                if (resourceNames[i].Contains(reqResBaseName))
                    matchingResourceNames.Add(resourceNames[i]);
            }

            string resMultiple;
            var attempt = 0;
            do
            {
                var scale = AppleDensities[attempt].Scale;
                resMultiple = AppleDensityMatch(attempt++);
                foreach (var resourceName in matchingResourceNames)
                {
                    tuple = GetiOSBasePathAndExt(resourceName);
                    if (tuple != null)
                    {
                        if (tuple.Item1 == reqResBaseName + resMultiple)
                        {
                            if (reqResExt == null || reqResExt.ToLower() == tuple.Item2.ToLower())
                            {
                                //_LastSuccessfulMode = Approach.iOS;
                                //System.Diagnostics.Debug.WriteLine("Apple attempt = "+attempt + " resourceName="+resourceName);
                                var result = new ImageSourceContainer(resourceName, scale);
                                return result;
                            }
                        }

                    }
                }
            } while (attempt < AppleDensities.Count);
            System.Diagnostics.Debug.WriteLine("No matches found for resource name:" + reqResourcePathString);
            System.Console.WriteLine("No matches found for resource name:" + reqResourcePathString);
            return null;
        }
        #endregion


        #region Path Resolution Support
        readonly static List<string> ValidImageExtensions = new List<string> {
			// these extensions can be turned into Image file on all three platforms
			"jpg", "jpeg", "gif", "png", "bmp", "bmpf", "svg"
        };

        class DeviceDensity
        {
            public string Name;
            public int Min, Max;
            public double Distance = -1;
            public float Scale;
        }


        static List<DeviceDensity> AppleDensities = new List<DeviceDensity> {
            new DeviceDensity { Name = "@2x",   Min=320, Max=399, Scale=2.0f },
            new DeviceDensity { Name = "@3x",   Min=400, Max=559, Scale=3.0f },
            new DeviceDensity { Name = "@1½x",  Min=200, Max=319, Scale=1.5f },
            new DeviceDensity { Name = "",      Min=141, Max=199, Scale=1.0f },
            new DeviceDensity { Name = "@1x",   Min=141, Max=199, Scale=1.0f },
            new DeviceDensity { Name = "@4x",   Min=560, Max=int.MaxValue, Scale=4.0f },
            new DeviceDensity { Name = "@¾x",   Min=  0, Max=140, Scale=0.75f },
        };

        static readonly object _appleLock = new object();
        static string AppleDensityMatch(int order)
        {
            //int dpi = (int)Display.Density;
            double scale = Display.Scale;
            lock (_appleLock)
            {
                if (AppleDensities[0].Distance < 0)
                {
                    foreach (var density in AppleDensities)
                        //density.Distance = Math.Min (Math.Abs (dpi - density.Min), Math.Abs (dpi - density.Max));
                        density.Distance = Math.Abs(scale - density.Scale);
                    AppleDensities.Sort((x, y) => x.Distance.CompareTo(y.Distance));
                    var withDeviceType = new List<DeviceDensity>(AppleDensities.Count * 2);
                    var device = "~" + Device.Idiom.ToString().ToLower();

                    foreach (var density in AppleDensities)
                    {
                        withDeviceType.Add(new DeviceDensity { Name = density.Name + device, Min = density.Min, Max = density.Max, Distance = density.Distance });
                        withDeviceType.Add(density);
                    }
                    AppleDensities = withDeviceType;
                }
            }
            return order < 0 || order >= AppleDensities.Count ? null : AppleDensities[order].Name;
        }
        #endregion



    }
}

