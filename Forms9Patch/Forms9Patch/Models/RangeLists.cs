using System.Collections.Generic;

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
    internal class RangeLists
	{
		public List<Range> PatchesX;
		public List<Range> PatchesY;
		public Range MarginX;
		public Range MarginY;

        public RangeLists()
        {

        }


	}

    internal static class RangeListExtensions
    {
        public static RangeLists ToRangeLists(this Xamarin.Forms.Thickness capInsets, int bitmapWidth, int bitmapHeight, Xamarin.Forms.ImageSource imageSource, bool ninePatchSource)
        {
            if (capInsets.Left > 0 || capInsets.Right > 0 || capInsets.Top > 0 || capInsets.Bottom > 0)
            {
                int offset = ninePatchSource ? 1 : 0;
                bool normalized = (capInsets.Left <= 1 && capInsets.Right <= 1);
                var scale = (float)imageSource.GetValue(ImageSource.ImageScaleProperty);
                var capsX = new List<Range>();
                if (capInsets.Left > 0 || capInsets.Right > 0)
                {
                    var start = capInsets.Left * (normalized ? bitmapWidth : scale) + offset;
                    System.Diagnostics.Debug.WriteLine("START: ["+start+"]");
                    var end = bitmapWidth - 1 - capInsets.Right * (normalized ? bitmapWidth : scale) + offset;
                    System.Diagnostics.Debug.WriteLine("END:   ["+end+"]");
                    if (start < 0)
                        start = 0;
                    if (end > bitmapWidth - 2)
                        end = bitmapWidth - 2;
                    if (start >= end)
                        start = end - 1;
                    capsX.Add(new Range {
                        Start = start,
                        End = end
                    });
                }
                normalized = (capInsets.Top <= 1 && capInsets.Bottom <= 1);
                var capsY = new List<Range>();
                if (capInsets.Top > 0 || capInsets.Bottom > 0)
                {
                    var start = capInsets.Top * (normalized ? bitmapHeight : scale) + offset;
                    System.Diagnostics.Debug.WriteLine("START: [" + start + "]");
                    var end = bitmapHeight - 1 - capInsets.Bottom * (normalized ? bitmapHeight : scale) + offset;
                    System.Diagnostics.Debug.WriteLine("END:   [" + end + "]");
                    if (start < 0)
                        start = 0;
                    if (end > bitmapHeight - 2)
                        end = bitmapHeight - 2;
                    if (start >= end)
                        start = end - 1;
                    capsY.Add(new Range
                    {
                        Start = start,
                        End = end,
                    });
                }
                return new RangeLists
                {
                    PatchesX = capsX,
                    PatchesY = capsY
                };
            }
            return null;
        }
    }
}

