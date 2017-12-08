using System;
using Xamarin.Forms;
using SkiaSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;
using System.Diagnostics;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
using SkiaSharp.Views.iOS;
namespace Forms9Patch.iOS
#elif __DROID__
using Xamarin.Forms.Platform.Android;
using SkiaSharp.Views.Android;
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
using SkiaSharp.Views.UWP;
namespace Forms9Patch.UWP
#else 
namespace Forms9Patch
#endif
{
    static class SKBitmapExtensions
    {
        #region Bitmap Caching
        static readonly Dictionary<string, F9PBitmap> _cache = new Dictionary<string, F9PBitmap>();
        static readonly Dictionary<string, List<SkiaRoundedBoxAndImageView>> _views = new Dictionary<string, List<SkiaRoundedBoxAndImageView>>();
        static readonly object _constructorLock = new object();
        //static readonly Dictionary<string, object> _locks = new Dictionary<string, object>();
        static readonly Dictionary<string, SemaphoreSlim> _locks = new Dictionary<string, SemaphoreSlim>();

#pragma warning disable 1998
        internal static async Task<F9PBitmap> FetchF9PBitmap(this Xamarin.Forms.ImageSource imageSource, SkiaRoundedBoxAndImageView view, CancellationToken cancellationToken = new CancellationToken())
#pragma warning restore 1998
        {
            F9PBitmap f9pBitmap = null;

            var key = imageSource.ImageSourceKey();
            if (key != null)
            {
                if (_cache.ContainsKey(key))
                {
                    _views[key].Add(view);
                    f9pBitmap = _cache[key];
                    if (f9pBitmap == null)
                        throw new InvalidDataContractException();
                }
                else
                {
                    if (!_locks.ContainsKey(key))
                        //_locks[key] = new object();
                        _locks[key] = new SemaphoreSlim(1, 1);
                    //lock (_locks[key])
                    await _locks[key].WaitAsync();
                    try
                    {
                        SKBitmap skBitmap = null;
                        string path = null;
                        if (key.StartsWith("eri:", StringComparison.Ordinal))
                        {
                            var assembly = (Assembly)imageSource.GetValue(ImageSource.AssemblyProperty);
                            var resourceId = key.Substring(4);
                            using (var stream = assembly.GetManifestResourceStream(resourceId))
                            {
                                if (stream == null)
                                {
                                    Toast.Create("Cannot find EmbeddedResource", "Cannot find EmbeddedResource with Id of [" + resourceId + "] in Assembly [" + assembly + "]");

                                    return null;
                                }
                                using (var skStream = new SKManagedStream(stream))
                                    skBitmap = SKBitmap.Decode(skStream);
                            }
                        }
                        else if (key.StartsWith("uri:", StringComparison.Ordinal))
                        {
                            path = await PCL.Utils.DownloadCache.DownloadAsync(key.Substring(4));
                            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))  // works for Windows ... not Android
                            using (var skStream = new SKManagedStream(stream))
                                skBitmap = SKBitmap.Decode(skStream);
                        }
                        else if (key.StartsWith("file:", StringComparison.Ordinal))  // does this work for Windows or iOS?
                        {
                            //path = key.Substring(5);
                            string file = ((FileImageSource)imageSource).File;
                            if (File.Exists(file))
                                skBitmap = SKBitmap.Decode(file);

                        }
                        else
                            throw new InvalidDataContractException();
                        if (skBitmap != null)
                        {
                            f9pBitmap = F9PBitmap.Create(skBitmap, key);

                            _cache[key] = f9pBitmap;
                            if (!_views.ContainsKey(key))
                                _views[key] = new List<SkiaRoundedBoxAndImageView>();
                            _views[key].Add(view);
                        }
                    }
                    finally
                    {
                        _locks[key].Release();
                        if (f9pBitmap?.SKBitmap == null)
                            _locks.Remove(key);
                    }
                }
            }
            else
            {
                // it's a Xamarin.Forms StreamSource that doesn't have any information that could be used for caching
                if (imageSource is StreamImageSource streamsource && streamsource.Stream != null)
                {
                    using (Stream stream = await ((IStreamImageSource)streamsource).GetStreamAsync(cancellationToken))
                    {
                        if (stream == null)
                            return null;
                        using (var skStream = new SKManagedStream(stream))
                        {
                            var skBitmap = SKBitmap.Decode(skStream);
                            f9pBitmap = F9PBitmap.Create(skBitmap, null);
                        }
                    }
                }
                else
                    throw new InvalidDataContractException();
            }

            if (f9pBitmap == null)
                System.Diagnostics.Debug.WriteLine("NO BITMAP FOUND FOR IMAGE SOURCE [" + imageSource + "]");
            return f9pBitmap;
        }

        internal static void ReleaseF9PBitmap(this Xamarin.Forms.ImageSource imageSource, SkiaRoundedBoxAndImageView view)
        {
            var key = imageSource.ImageSourceKey();
            if (key == null)
                return;
            lock (_constructorLock)
            {
                if (!_views.ContainsKey(key))
                    // this happens if there is a failure to load the view
                    return;
                var _clientViews = _views[key];
                _clientViews?.Remove(view);
                if (_clientViews.Count > 0)
                    return;
                _views.Remove(key);
                if (_cache.ContainsKey(key))
                {
                    var bitmap = _cache[key];
                    _cache.Remove(key);
                    bitmap.Dispose();
                    bitmap = null;
                }
                _locks.Remove(key);
            }
        }
        #endregion


        #region NinePatch parcing

        static public SKLattice ToSKLattice(this RangeLists rangelists, SKBitmap bitmap)
        {
            if (rangelists == null)
                throw new InvalidDataContractException("ToSKLattice cannot be applied to null RangeLists");
            var result = new SKLattice();
            result.Bounds = bitmap.Info.Rect;

            var xdivs = new List<int>();
            for (int i = 0; i < rangelists.PatchesX.Count; i++)
            {
                xdivs.Add((int)rangelists.PatchesX[i].Start);
                //xdivs.Add(Math.Min((int)rangelists.PatchesX[i].End+1, bitmap.Width));
                xdivs.Add(Math.Min((int)rangelists.PatchesX[i].End + 1, bitmap.Width - 1));
            }
            result.XDivs = xdivs.ToArray();

            var ydivs = new List<int>();
            for (int i = 0; i < rangelists.PatchesY.Count; i++)
            {
                ydivs.Add((int)rangelists.PatchesY[i].Start);
                ydivs.Add(Math.Min((int)rangelists.PatchesY[i].End + 1, bitmap.Height - 1));
            }
            result.YDivs = ydivs.ToArray();


            return result;
        }

        static public RangeLists PatchRanges(this SKBitmap bitmap)
        {
            if (bitmap.Info.ColorType != SKColorType.Bgra8888 && bitmap.Info.ColorType != SKColorType.Rgba8888)
            {
                System.Diagnostics.Debug.WriteLine("bitmap colortype [" + bitmap.Info.ColorType + "] not Bgra888");
                return null;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            var capsX = new List<Range>();
            int pos = -1;
            for (int i = 0; i < bitmap.Width - 2; i++)
            {
                var pixel = bitmap.GetPixel(i + 1, 0);
                if (pixel == SKColors.Black)
                {
                    if (pos == -1)
                        pos = i;
                }
                else if (pixel == 0)
                {
                    if (pos != -1)
                    {
                        var range = new Range(pos, i - 1);
                        capsX.Add(range);
                        pos = -1;
                    }
                }
                else
                    // this is not a nine-patch;
                    return null;
            }
            if (pos != -1)
            {
                var range = new Range(pos, bitmap.Width - 3);
                capsX.Add(range);
            }

            var capsY = new List<Range>();
            pos = -1;
            for (int i = 0; i < bitmap.Height - 2; i++)
            {
                var pixel = bitmap.GetPixel(0, i + 1);
                if (pixel == SKColors.Black)
                {
                    if (pos == -1)
                        pos = i;
                }
                else if (pixel == 0)
                {
                    if (pos != -1)
                    {
                        var range = new Range(pos, i - 1);
                        capsY.Add(range);
                        pos = -1;
                    }
                }
                else
                    return null;
            }
            if (pos != -1)
            {
                var range = new Range(pos, bitmap.Height - 3);
                capsY.Add(range);
            }

            if (capsX.Count < 1 && capsY.Count < 1)
                return null;


            var margX = null as Range;
            pos = -1;
            for (int i = 0; i < bitmap.Width - 2; i++)
            {
                var pixel = bitmap.GetPixel(i + 1, bitmap.Height - 1);
                if (pixel == SKColors.Black)
                {
                    if (pos == -1)
                    {
                        pos = i - 1;
                        break;
                    }
                }
            }
            if (pos != -1)
            {
                for (int i = bitmap.Width - 2; i > pos; i--)
                {
                    var pixel = bitmap.GetPixel(i + 1, bitmap.Height - 1);
                    if (pixel == SKColors.Black)
                    {
                        margX = new Range(pos, i);
                        break;
                    }
                }
            }

            var margY = null as Range;
            pos = -1;
            for (int i = 0; i < bitmap.Height - 2; i++)
            {
                var pixel = bitmap.GetPixel(bitmap.Width - 1, i + 1);
                if (pixel == SKColors.Black)
                {
                    if (pos == -1)
                    {
                        pos = i - 1;
                        break;
                    }
                }
            }
            if (pos != -1)
            {
                for (int i = bitmap.Height - 2; i > pos; i--)
                {
                    var pixel = bitmap.GetPixel(bitmap.Width - 1, i + 1);
                    if (pixel == SKColors.Black)
                    {
                        margY = new Range(pos, i);
                        break;
                    }
                }
            }


            var rangeLists = new RangeLists
            {
                PatchesX = capsX,
                PatchesY = capsY,
                MarginX = margX,
                MarginY = margY
            };
            stopwatch.Stop();

            return rangeLists;
        }

        #endregion
    }
}