/* 
 * Idea behind image cache

1) Images come from 
	a) Streams (file, memory, embedded resources)
	b) files
	c) URLs (only one cached in local storage)
2) there can be some latency in retrieving the file
3) URLs can be out of date 

All images are cached in memory

1) check memory
2) check local storage
3) retrieve from source

if image source is url:
	a) cache locally
	b) kick off a process to get the latest version.  Need to get the expiration for urls.
	c) if cache is updated, need to update any images that use it



SKBitMap GetBitmap(this ImageSource source)
{
	// what kind of 
}

*/

using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using SkiaSharp.Views.UWP;
using SkiaSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Forms9Patch.UWP
{
    static class SKBitmapExtensions
    {
        #region Bitmap Caching
        public static SKBitmap GetSKBitmap(this Xamarin.Forms.ImageSource source)
        {
            var key = source.ImageSourceKey();


            return null;
        }

        static readonly Dictionary<string, SKBitmap> _cache = new Dictionary<string, SKBitmap>();
        static readonly Dictionary<string, List<SkiaRoundedBoxView>> _views = new Dictionary<string, List<SkiaRoundedBoxView>>();
        static readonly object _constructorLock = new object();
        static readonly Dictionary<string, object> _locks = new Dictionary<string, object>();

        #pragma warning disable 1998
        internal static async Task<SKBitmap> FetchSkBitmap(this Xamarin.Forms.ImageSource imageSource, SkiaRoundedBoxView view, CancellationToken cancellationToken = new CancellationToken())
#pragma warning restore 1998
        {
            SKBitmap bitmap = null;

            var key = imageSource.ImageSourceKey();
            if (key != null)
            {
                if (_cache.ContainsKey(key))
                {
                    _views[key].Add(view);
                    bitmap = _cache[key];
                    if (bitmap == null)
                        throw new InvalidDataContractException();
                }
                else
                {
                    if (!_locks.ContainsKey(key))
                        _locks[key] = new object();
                    lock (_locks[key])
                    {
                        string path = null;
                        if (key.StartsWith("eri:"))
                        {
                            var assembly = (Assembly)imageSource.GetValue(ImageSource.AssemblyProperty);
                            var resourceId = key.Substring(4);
                            using (var stream = assembly.GetManifestResourceStream(resourceId))
                            using (var skStream = new SKManagedStream(stream))
                                bitmap = SKBitmap.Decode(skStream);
                        }
                        else if (key.StartsWith("uri:"))
                            path = PCL.Utils.FileCache.Download(key.Substring(4));
                        else if (key.StartsWith("file:"))
                            path = key.Substring(5);
                        else
                            throw new InvalidDataContractException();
                        if (path != null)
                        {
                            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                            using (var skStream = new SKManagedStream(stream))
                                bitmap = SKBitmap.Decode(skStream);
                        }
                        if (bitmap != null)
                        {
                            _cache[key] = bitmap;
                            if (!_views.ContainsKey(key))
                                _views[key] = new List<SkiaRoundedBoxView>();
                            _views[key].Add(view);
                        }
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
                            bitmap = SKBitmap.Decode(skStream);
                    }
                }
                else
                    throw new InvalidDataContractException();
            }

            if (bitmap == null)
                System.Diagnostics.Debug.WriteLine("NO BITMAP FOUND FOR IMAGE SOURCE ["+imageSource+"]");
            return bitmap;
        }

        internal static void ReleaseSkBitmap(this Xamarin.Forms.ImageSource imageSource, SkiaRoundedBoxView view)
        {
            var key = imageSource.ImageSourceKey();
            if (key == null)
                return;
            lock (_constructorLock)
            {
                if (!_views.ContainsKey(key))
                    throw new InvalidDataContractException();
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
            result.Bounds = SKRectI.Create(0, 0, bitmap.Width-1, bitmap.Height-1);
            var xdivs = new List<int>();
            for (int i = 0; i < rangelists.PatchesX.Count; i++)
            {
                xdivs.Add((int)rangelists.PatchesX[i].Start);
                xdivs.Add((int)rangelists.PatchesX[i].End+1);
            }
            var ydivs = new List<int>();
            for (int i = 0; i < rangelists.PatchesY.Count; i++)
            {
                ydivs.Add((int)rangelists.PatchesY[i].Start);
                ydivs.Add((int)rangelists.PatchesY[i].End+1);
            }
            result.XDivs = xdivs.ToArray();
            result.YDivs = ydivs.ToArray();
            return result;
        }

        static public RangeLists PatchRanges(this SKBitmap bitmap)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            //int width = bitmap.Width;
            //int height = bitmap.Height;

            var capsX = new List<Range>();
            int pos = -1;

            if (bitmap.Info.ColorType != SKColorType.Bgra8888)
                return null;
            //var firstRow = bitmap.ToByteArray(0, bitmap.PixelWidth);
            //SKBitmap rowBitmap = new SKBitmap(bitmap.Width - 2, 1, SKColorType.Bgra8888, SKAlphaType.Unpremul);
            //bitmap.ExtractSubset(rowBitmap, SKRectI.Create(1,0, bitmap.Width - 2, 1));
            //var firstRow = rowBitmap.Bytes;
            //var pixels = bitmap.GetPixels();

            for (int i = 0; i < bitmap.Width - 2; i++)
            {
                //if (bitmap.IsBlackAt(i, 0))
                var pixel = bitmap.GetPixel(i+1, 0);
                if (pixel == SKColors.Black)
                {
                    if (pos == -1)
                        pos = i;
                }
                //else if (bitmap.IsTransparentAt(i, 0))
                else if (pixel == 0)
                {
                    if (pos != -1)
                    {
                        var range = new Range();
                        range.Start = pos;
                        range.End = i - 1;
                        capsX.Add(range);
                        pos = -1;
                    }
                }
                else
                {
                    // this is not a nine-patch;
                    //rowBitmap.Dispose();
                    return null;
                }
            }
            if (pos != -1)
            {
                var range = new Range();
                range.Start = pos;
                range.End = bitmap.Width - 2;
                capsX.Add(range);
            }

            var capsY = new List<Range>();
            pos = -1;

            //var firstColBitmap = bitmap.Crop(0, 0, 1, bitmap.PixelHeight);
            //SKBitmap columnBitmap = new SKBitmap(1, bitmap.Height - 2, SKColorType.Bgra8888, SKAlphaType.Unpremul);
            //bitmap.ExtractSubset(columnBitmap, SKRectI.Create(0, 1, 1, bitmap.Height - 2));
            //var firstCol = columnBitmap.Bytes;

            for (int i = 0; i < bitmap.Height - 2; i++)
            {
                //if (bitmap.IsBlackAt(0, i))
                var pixel = bitmap.GetPixel(0, i+1);
                if (pixel == SKColors.Black)
                {
                    if (pos == -1)
                        pos = i;
                }
                //else if (bitmap.IsTransparentAt(0, i))
                else if (pixel == 0)
                {
                    if (pos != -1)
                    {
                        var range = new Range();
                        range.Start = pos;
                        range.End = i - 1;
                        capsY.Add(range);
                        pos = -1;
                    }
                }
                else
                {
                    //columnBitmap.Dispose();
                    //rowBitmap.Dispose();
                    return null;
                }
            }
            if (pos != -1)
            {
                var range = new Range();
                range.Start = pos;
                range.End = bitmap.Height - 2;
                capsY.Add(range);
            }

            var margX = null as Range;
            pos = -1;
            //var lastRow = bitmap.ToByteArray((bitmap.PixelHeight - 1) * bitmap.PixelWidth, bitmap.PixelWidth);
            //bitmap.ExtractSubset(rowBitmap, SKRectI.Create(1, bitmap.Height - 1, bitmap.Width - 2, 1));
            //var lastRow = rowBitmap.Bytes;

            for (int i = 0; i < bitmap.Width - 2; i++)
            {
                var pixel = bitmap.GetPixel(i+1, bitmap.Height-1);
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
                    var pixel = bitmap.GetPixel(i+1, bitmap.Height - 1);
                    if (pixel == SKColors.Black)
                    {
                        margX = new Range();
                        margX.Start = pos;
                        margX.End = i;
                        break;
                    }
                }
            }

            var margY = null as Range;
            pos = -1;

            //var lastColumnBitmap = bitmap.Crop(bitmap.PixelWidth - 1, 0, 1, bitmap.PixelHeight);
            //var lastCol = lastColumnBitmap.ToByteArray();
            //bitmap.ExtractSubset(columnBitmap, SKRectI.Create(bitmap.Width - 1, 1, 1, bitmap.Height - 2));
            //var lastCol = columnBitmap.Bytes;

            for (int i = 0; i < bitmap.Height - 2; i++)
            {
                var pixel = bitmap.GetPixel(bitmap.Width-1, i + 1);
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
                        margY = new Range();
                        margY.Start = pos;
                        margY.End = i;
                        break;
                    }
                }
            }

            var rangeLists = new RangeLists();
            rangeLists.PatchesX = capsX;
            rangeLists.PatchesY = capsY;
            rangeLists.MarginX = margX;
            rangeLists.MarginY = margY;

            stopwatch.Stop();
            System.Diagnostics.Debug.WriteLine("PatchRanges: "+ stopwatch.ElapsedMilliseconds + "ms");

            System.Diagnostics.Debug.Write    ("      capsX: ");
            foreach (var cap in capsX)
                System.Diagnostics.Debug.Write("["+cap.Start+","+cap.End+","+cap.Width+"]");
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.Write("      capsY: ");
            foreach (var cap in capsY)
                System.Diagnostics.Debug.Write("[" + cap.Start + "," + cap.End + "," + cap.Width + "]");
            System.Diagnostics.Debug.WriteLine("");

            if (margX!=null)
            System.Diagnostics.Debug.WriteLine("      margX: [" + margX.Start + "," + margX.End + "," + margX.Width + "]");
            if (margY!=null)
            System.Diagnostics.Debug.WriteLine("      margY: [" + margY.Start + "," + margY.End + "," + margY.Width + "]");

            //rowBitmap.Dispose();
            //columnBitmap.Dispose();

            return rangeLists;
        }

        #endregion
    }
}