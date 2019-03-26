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

namespace Forms9Patch
{
    static class SKBitmapExtensions
    {
        #region Bitmap Caching
        static readonly Dictionary<string, F9PImageData> _cache = new Dictionary<string, F9PImageData>();
        static readonly Dictionary<string, List<Image>> _views = new Dictionary<string, List<Image>>();
        static readonly object _constructorLock = new object();
        //static readonly Dictionary<string, object> _locks = new Dictionary<string, object>();
        static readonly Dictionary<string, SemaphoreSlim> _locks = new Dictionary<string, SemaphoreSlim>();

#pragma warning disable 1998
        internal static async Task<F9PImageData> FetchF9pImageData(this Xamarin.Forms.ImageSource imageSource, Image view, CancellationToken cancellationToken = new CancellationToken())
#pragma warning restore 1998
        {
            F9PImageData f9pImageData = null;

            var key = imageSource.ImageSourceKey();
            if (key != null)
            {
                if (_cache.ContainsKey(key))
                {
                    _views[key].Add(view);
                    f9pImageData = _cache[key];
                    if (f9pImageData == null)
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
                        //SKBitmap skBitmap = null;
                        string path = null;
                        if (key.StartsWith("eri:", StringComparison.Ordinal))
                        {
                            var assembly = (Assembly)imageSource.GetValue(ImageSource.AssemblyProperty);
                            var resourceId = key.Substring(4);

                            using (var stream = await P42.Utils.EmbeddedResourceCache.GetStreamAsync(resourceId, assembly, Forms9Patch.Image.EmbeddedResourceImageCacheFolderName))
                            {
                                if (stream == null)
                                {
                                    Toast.Create("Cannot find EmbeddedResource", "Cannot find EmbeddedResource with Id of [" + resourceId + "] in Assembly [" + assembly + "]");
                                    return null;
                                }
                                /*    
                                using (var skStream = new SKManagedStream(stream))
                                    skBitmap = SKBitmap.Decode(skStream);
                                */
                                f9pImageData = F9PImageData.Create(stream, key);
                            }
                        }
                        else if (key.StartsWith("uri:", StringComparison.Ordinal))
                        {
                            path = await P42.Utils.DownloadCache.DownloadAsync(key.Substring(4), Forms9Patch.Image.UriImageCacheFolderName);
                            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                                f9pImageData = F9PImageData.Create(stream, key);
                        }
                        else if (key.StartsWith("file:", StringComparison.Ordinal))  // does this work for Windows or iOS?
                        {
                            //path = key.Substring(5);
                            string filePath = ((FileImageSource)imageSource).File;
                            /*
                            if (File.Exists(filePath))
                                skBitmap = SKBitmap.Decode(filePath);
#if __DROID__
                            else
                            {
                                var nativeBitmap = Settings.Context.Resources.GetBitmap(filePath);
                                skBitmap = nativeBitmap.ToSKBitmap();
                            }
#endif
                            */
                            f9pImageData = F9PImageData.Create(filePath, key);
                        }
                        else
                            throw new InvalidDataContractException();
                        /*
                        if (skBitmap != null)
                        {
                            f9pImageData = F9PImageData.Create(skBitmap, key);

                            _cache[key] = f9pImageData;
                            if (!_views.ContainsKey(key))
                                _views[key] = new List<SkiaRoundedBoxAndImageView>();
                            _views[key].Add(view);
                        }
                        */
                        if (f9pImageData != null)
                        {
                            _cache[key] = f9pImageData;
                            if (!_views.ContainsKey(key))
                                _views[key] = new List<Image>();
                            _views[key].Add(view);
                        }
                    }
                    finally
                    {
                        _locks[key].Release();
                        if ((f9pImageData?.SKBitmap == null || f9pImageData?.SKSvg == null))
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
                            f9pImageData = F9PImageData.Create(skBitmap, null);
                        }
                    }
                }
                else
                    throw new InvalidDataContractException();
            }

            if (f9pImageData == null)
            {
                System.Diagnostics.Debug.WriteLine("NO BITMAP FOUND FOR IMAGE SOURCE [" + imageSource + "]");
                System.Console.WriteLine("FORMS9PATCH: NO BITMAP FOUND FOR IMAGE SOURCE [" + imageSource + "]");
            }
            return f9pImageData;
        }


        internal static void ReleaseF9PBitmap(this Xamarin.Forms.ImageSource imageSource, Image view)
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
        /*
        static public SKLattice ToSKLattice(this RangeLists rangelists, SKBitmap bitmap)
        {
            if (rangelists == null)
                throw new InvalidDataContractException("ToSKLattice cannot be applied to null RangeLists");
            var result = new SKLattice();
            result.Bounds = bitmap.Info.Rect;
            result.Flags = new SKLatticeFlags[rangelists.PatchesX.Count * rangelists.PatchesY.Count];
            for (int i = 0; i < rangelists.PatchesX.Count * rangelists.PatchesY.Count; i++)
                result.Flags[i] = SKLatticeFlags.Default;

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
        */

        static public RangeLists PatchRanges(this SKBitmap bitmap)
        {
            if (bitmap.Info.ColorType != SKColorType.Bgra8888 && bitmap.Info.ColorType != SKColorType.Rgba8888)
            {
                System.Diagnostics.Debug.WriteLine("bitmap colortype [" + bitmap.Info.ColorType + "] not Bgra888");
                System.Console.WriteLine("bitmap colortype [" + bitmap.Info.ColorType + "] not Bgra888.  Just can't deal with it for 9Patch Images.");
                return null;
            }
            if (bitmap.Width < 3 || bitmap.Height < 3)
                return null;

            int lastPos = 0;
            SKColor lastPixel = bitmap.GetPixel(lastPos + 1, 0);
            if (lastPixel != 0 && lastPixel != SKColors.Black)
                return null;
            var capsX = new List<Range>();
            if (bitmap.Width == 3)
                capsX.Add(new Range(0, 1, lastPixel == SKColors.Black));
            else
            {
                for (int i = 1; i < bitmap.Width - 2; i++)
                {
                    var pixel = bitmap.GetPixel(i + 1, 0);
                    if (pixel != 0 && pixel != SKColors.Black)
                        return null;
                    if (pixel != lastPixel)
                    {
                        capsX.Add(new Range(lastPos, i - 1, lastPixel == SKColors.Black));
                        lastPixel = pixel;
                        lastPos = i;
                    }
                }
                capsX.Add(new Range(lastPos, bitmap.Width - 3, lastPixel == SKColors.Black));
            }

            lastPos = 0;
            lastPixel = bitmap.GetPixel(lastPos + 1, 0);
            if (lastPixel != 0 && lastPixel != SKColors.Black)
                return null;
            var capsY = new List<Range>();
            if (bitmap.Height == 3)
                capsY.Add(new Range(0, 1, lastPixel == SKColors.Black));
            else
            {
                for (int i = 1; i < bitmap.Height - 2; i++)
                {
                    var pixel = bitmap.GetPixel(0, i + 1);
                    if (pixel != 0 && pixel != SKColors.Black)
                        return null;
                    if (pixel != lastPixel)
                    {
                        capsY.Add(new Range(lastPos, i - 1, lastPixel == SKColors.Black));
                        lastPixel = pixel;
                        lastPos = i;
                    }
                }
                capsY.Add(new Range(lastPos, bitmap.Height - 3, lastPixel == SKColors.Black));
            }
            /*
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
*/


            var margX = null as Range;
            var pos = -1;
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
            //stopwatch.Stop();

            if (rangeLists.PatchesX != null && rangeLists.PatchesX.Count < 2 && rangeLists.PatchesY != null && rangeLists.PatchesY.Count < 2)
                return null;

            return rangeLists;
        }

        #endregion
    }
}