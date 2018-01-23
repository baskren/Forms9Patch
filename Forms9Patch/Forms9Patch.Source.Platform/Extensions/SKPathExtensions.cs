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
    static class SKPathExtensions
    {
        public static void ArcWithCenterTo(this SKPath path, float x, float y, float radius, float startDegrees, float sweepDegrees)
        {
            var rect = new SKRect(x - radius, y - radius, x + radius, y + radius);
            path.ArcTo(rect, startDegrees, sweepDegrees, false);
        }
    }
}
