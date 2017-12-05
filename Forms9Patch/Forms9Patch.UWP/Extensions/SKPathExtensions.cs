using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch.UWP
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
