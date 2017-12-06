using System;
using SkiaSharp;

#if __IOS__
namespace Forms9Patch.iOS
#elif __DROID__
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
namespace Forms9Patch.UWP
#else 
namespace Forms9Patch
#endif
{
    class F9PBitmap : IDisposable
    {
        #region Properties
        public RangeLists RangeLists { get; private set; }

        public SKBitmap SKBitmap { get; private set; }

        public double Width => SKBitmap.Width;

        public double Height => SKBitmap.Height;
        #endregion

        #region Factory / Constructor
        static public F9PBitmap Create(SKBitmap skBitmap, string key)
        {
            if (skBitmap == null)
                return null;
            return new F9PBitmap(skBitmap, key);
        }

        F9PBitmap(SKBitmap skBitamp, string key)
        {
            RangeLists = skBitamp.PatchRanges();
            if (RangeLists?.PatchesX != null && RangeLists.PatchesX.Count > 0 && RangeLists.PatchesY != null && RangeLists.PatchesY.Count > 0)
            {
                SKBitmap unmarkedBitmap = new SKBitmap(skBitamp.Width - 1, skBitamp.Height - 1, SKColorType.Rgba8888, SKAlphaType.Unpremul);
                skBitamp.ExtractSubset(unmarkedBitmap, SKRectI.Create(1, 1, skBitamp.Width - 2, skBitamp.Height - 2));
                skBitamp.Dispose();
                skBitamp = unmarkedBitmap.Copy();
            }
            SKBitmap = skBitamp;
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SKBitmap?.Dispose();
                    SKBitmap = null;
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() => Dispose(true);
        #endregion


    }
}

