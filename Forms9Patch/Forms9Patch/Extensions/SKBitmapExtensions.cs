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
        static readonly Dictionary<string, (DateTime Start, SemaphoreSlim Lock)> _locks = new Dictionary<string, (DateTime Start, SemaphoreSlim Lock)>();

#pragma warning disable 1998
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0068:Use recommended dispose pattern", Justification = "<Pending>")]
        internal static async Task<F9PImageData> FetchF9pImageData(this Xamarin.Forms.ImageSource imageSource, Image view, CancellationToken cancellationToken = new CancellationToken(), FailAction failAction = FailAction.ShowAlert)
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
                        _locks[key] = (DateTime.Now, new SemaphoreSlim(1, 1));
                    else if (DateTime.Now - _locks[key].Start > TimeSpan.FromSeconds(2))
                        _locks[key].Lock.Release();
                    _locks[key] = (DateTime.Now, _locks[key].Lock);
                    await _locks[key].Lock.WaitAsync();
                    try
                    {
                        //SKBitmap skBitmap = null;
                        string path = null;
                        if (key.StartsWith("eri:", StringComparison.Ordinal))
                        {
                            var assembly = (Assembly)imageSource.GetValue(ImageSource.AssemblyProperty);
                            var resourceId = key.Substring(4);

                            using (var stream = await P42.Utils.EmbeddedResourceCache.GetStreamAsync(resourceId, assembly, Image.EmbeddedResourceImageCacheFolderName))
                            {
                                if (stream == null)
                                {
                                    if (failAction == FailAction.ShowAlert)
                                    { 
                                        using (var toast = Toast.Create("Cannot find EmbeddedResource", "Cannot find EmbeddedResource with Id of [" + resourceId + "] in Assembly [" + assembly + "]"))
                                        {
                                            await toast.WaitForPoppedAsync();
                                        }
                                    }
                                    else if (failAction == FailAction.ThrowException)
                                        throw new Exception("Cannot find EmbeddedResource with Id of [" + resourceId + "] in Assembly [" + assembly + "]");
                                    return null;
                                }
                                f9pImageData = F9PImageData.Create(stream, key);
                            }
                        }
                        else if (key.StartsWith("uri:", StringComparison.Ordinal))
                        {
                            Uri uri = null;
                            if (imageSource is Xamarin.Forms.UriImageSource uriSource)
                                uri = uriSource.Uri;
                            else if (new Uri(key.Substring(4)) is Uri newUri)
                                uri = newUri;

                            if (uri != null && uri.IsAbsoluteUri)
                            {
                                path = await P42.Utils.DownloadCache.DownloadAsync(uri.AbsoluteUri, Forms9Patch.Image.UriImageCacheFolderName);
                                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                                    f9pImageData = F9PImageData.Create(stream, key);
                            }
                        }
                        else if (key.StartsWith("file:", StringComparison.Ordinal))  // does this work for Windows or iOS?
                        {
                            var filePath = ((FileImageSource)imageSource).File;
                            f9pImageData = F9PImageData.Create(filePath, key);
                        }
                        else
                        {
                            Console.WriteLine("UNABLE TO ACCESS IMAGESOURCE: " + key);
                        }
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
                        _locks[key].Lock.Release();
                        if ((f9pImageData?.SKBitmap == null || f9pImageData?.SKSvg == null))
                            _locks.Remove(key);
                        else
                            _locks[key] = (DateTime.MaxValue, _locks[key].Lock);
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
                        /*
                        using (var skStream = new SKManagedStream(stream))
                        {
                            var skBitmap = SKBitmap.Decode(skStream);
                            f9pImageData = F9PImageData.Create(skBitmap, null);
                        }
                        */
                        f9pImageData = F9PImageData.Create(stream, null);
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

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        internal static void ReleaseF9PBitmap(this Xamarin.Forms.ImageSource imageSource, Image view)
            => ReleaseF9PBitmap(imageSource?.ImageSourceKey(), view);

        internal static void ReleaseF9PBitmap(this F9PImageData f9PImageData, Image view)
            => ReleaseF9PBitmap(f9PImageData?.Key, view);

#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        static async Task ReleaseF9PBitmap(string key, Image view)
        {
            if (key == null)
                return;

            SemaphoreSlim lck = null;
            if (_locks.ContainsKey(key))
            {
                lck = _locks[key].Lock;
                await lck.WaitAsync();
            }

            int remainingViews = 0;
            if (_views.ContainsKey(key))
            {
                var _clientViews = _views[key];
                _clientViews?.Remove(view);
                remainingViews = _clientViews.Count;
                if (remainingViews <= 0)
                {
                    _views.Remove(key);
                    if (_cache.ContainsKey(key))
                    {
                        var bitmap = _cache[key];
                        _cache.Remove(key);

                        bitmap.Dispose();
                        bitmap = null;
                    }
                }
            }

            if (remainingViews <= 0)
                _locks.Remove(key);
            lck?.Release();

        }
        #endregion


        #region NinePatch parcing
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

            var lastPos = 0;
            var lastPixel = bitmap.GetPixel(lastPos + 1, 0);
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