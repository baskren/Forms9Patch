using System;
using SkiaSharp;

namespace Forms9Patch.UWP
{
    class F9PBitmap : IDisposable
    {
        public RangeLists RangeLists { get; private set; }

        public SKBitmap SKBitmap { get; private set; }

        public double Width => SKBitmap.Width;

        public double Height => SKBitmap.Height;

        static public F9PBitmap Create(SKBitmap skBitmap)
        {
            if (skBitmap == null)
                return null;
            return new F9PBitmap(skBitmap);
        }

        private F9PBitmap(SKBitmap skBitamp)
        {
            RangeLists = skBitamp.PatchRanges();
            if (RangeLists?.PatchesX!=null && RangeLists.PatchesX.Count>0 && RangeLists.PatchesY!=null && RangeLists.PatchesY.Count>0)
            {
                var unmarkedBitmap = new SKBitmap(skBitamp.Width - 2, skBitamp.Height - 2, SKColorType.Rgba8888, SKAlphaType.Unpremul);
                //_sourceBitmap.ExtractAlpha(unmarkedBitmap);
                skBitamp.ExtractSubset(unmarkedBitmap, SKRectI.Create(1, 1, skBitamp.Width - 2, skBitamp.Height - 2));
                skBitamp.Dispose();
                skBitamp = unmarkedBitmap.Copy();
            }
            SKBitmap = skBitamp;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

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

