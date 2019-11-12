using System;
using SkiaSharp;
using System.IO;
using Xamarin.Forms;
using P42.Utils;

namespace Forms9Patch
{
    class F9PImageData : IDisposable
    {
        #region Properties
        public RangeLists RangeLists { get; private set; }

        public SKBitmap SKBitmap { get; private set; }

        public SkiaSharp.Extended.Svg.SKSvg SKSvg { get; private set; }

        readonly double _width;
        public double Width => _width;

        readonly double _height;
        public double Height => _height;

        public bool ValidBitmap => SKBitmap != null && _width > 0 && _height > 0;

        public bool ValidSVG => SKSvg != null;

        public bool ValidImage => ValidSVG || ValidBitmap;

        public string Key { get; set; }
        #endregion

        #region Factory / Constructor
        static public F9PImageData Create(SKBitmap skBitmap, string key)
        {
            if (skBitmap == null)
                return null;
            return new F9PImageData(skBitmap, key);
        }

        F9PImageData(SKBitmap skBitamp, string key)
        {
            P42.Utils.Debug.AddToCensus(this);

            RangeLists = skBitamp.PatchRanges();
            if (RangeLists?.PatchesX != null && RangeLists.PatchesX.Count > 0 && RangeLists.PatchesY != null && RangeLists.PatchesY.Count > 0)
            {
                var unmarkedBitmap = new SKBitmap(skBitamp.Width - 1, skBitamp.Height - 1, SKColorType.Rgba8888, SKAlphaType.Unpremul);
                skBitamp.ExtractSubset(unmarkedBitmap, SKRectI.Create(1, 1, skBitamp.Width - 2, skBitamp.Height - 2));
                skBitamp.Dispose();
                skBitamp = unmarkedBitmap.Copy();
            }
            _width = skBitamp.Width;
            _height = skBitamp.Height;
            SKBitmap = skBitamp;
            Key = key;
        }

        F9PImageData(SkiaSharp.Extended.Svg.SKSvg skSvg, string key)
        {
            P42.Utils.Debug.AddToCensus(this);

            _width = skSvg.CanvasSize.Width;
            _height = skSvg.CanvasSize.Height;
            SKSvg = skSvg;
            Key = key;
        }

        static public F9PImageData Create(System.IO.Stream stream, string key)
        {
            // necessary if we can't random access the stream - which should be the case with url streams
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);

                //TODO: Try opening SVG with SKBitmap.Decode.  Could be viable alternative to the following?
                using (var sr = new StreamReader(memoryStream))
                {
                    if (IsSvg(sr))
                    {
                        var skSvg = new SkiaSharp.Extended.Svg.SKSvg();
                        try
                        {
                            skSvg.Load(memoryStream);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Failed to load SkiaSvg memory stream.  Key=[" + key + "]");
                            return null;
                        }
                        return new F9PImageData(skSvg, key);
                    }
                    var skBitmap = SKBitmap.Decode(memoryStream);
                    if (skBitmap != null)
                        return new F9PImageData(skBitmap, key);
                }

            }
            return null;
        }

        static bool IsSvg(StreamReader sr)
        {
            sr.BaseStream.Position = 0;
            string str;
            while (!sr.EndOfStream)
            {
                str = sr.ReadLine();
                if (str == "<svg" || str.Contains("<svg ") || str == "<SVG" || str.Contains("<SVG "))
                {
                    sr.BaseStream.Position = 0;
                    return true;
                }
            }
            sr.BaseStream.Position = 0;
            return false;
        }

        static public F9PImageData Create(string path, string key)
        {
            if (File.Exists(path))
            {
                if (Device.RuntimePlatform == Device.UWP)
                {
                    var skBitmap = SKBitmap.Decode(path);
                    if (skBitmap != null && skBitmap.Width > 0 && skBitmap.Height > 0)
                        return new F9PImageData(skBitmap, key);
                    var skSvg = new SkiaSharp.Extended.Svg.SKSvg();
                    skSvg.Load(path);
                    if (skSvg.CanvasSize.Width > 0 && skSvg.CanvasSize.Height > 0)
                        return new F9PImageData(skSvg, key);
                    return null;
                }
                else
                {
                    if (File.Exists(path))
                        using (var fileStream = new FileStream(path, FileMode.Open))
                            return Create(fileStream, key);
                }
            }
#if __DROID__
            if (Device.RuntimePlatform == Device.Android)
            {
                if (path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                {
                    /*
                    var resources = Settings.Context.Resources;
                    int resId = resources.GetIdentifier(path.ToLower().Substring(0, path.Length - 4), "drawable", Settings.Context.PackageName);
                    var str = Settings.Context.Resources.GetString(resId);

                    var root = Forms9Patch.Droid.Settings.Activity.FileList();
                    var dir = Forms9Patch.Droid.Settings.Activity.FilesDir;

                    var x = System.IO.Directory.EnumerateDirectories()
                    */

                    //string data = null;
                    var assets = Settings.Context.Assets.List("");
                    if (assets.Contains(path))
                        using (var fileStream = Settings.Context.Assets.Open(path))
                            return Create(fileStream, key);
                    assets = Settings.Context.Assets.List("images");
                    if (assets.Contains(path))
                        using (var fileStream = Settings.Context.Assets.Open(path))
                            return Create(fileStream, key);
                }
                else
                {
                    var nativeBitmap = Settings.Context.Resources.GetBitmap(path);
                    var skBitmap = nativeBitmap.ToSKBitmap();
                    return Create(skBitmap, key);
                }
            }
#endif
            return null;
        }

        #endregion

        #region IDisposable Support
        private bool _disposed; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                SKBitmap?.Dispose();
                SKBitmap = null;
                P42.Utils.Debug.RemoveFromCensus(this);
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}

