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
                var capsX = new List<Range> {new Range {
                        Start = capInsets.Left * (normalized ?  bitmapWidth : scale) + offset,
                        End = bitmapWidth - capInsets.Right * (normalized ? bitmapWidth :  scale) + offset
                    }};
                normalized = (capInsets.Top <= 1 && capInsets.Bottom <= 1);
                var capsY = new List<Range> {new Range {
                        Start = capInsets.Top * (normalized ? bitmapHeight :  scale) + offset,
                        End = bitmapHeight - capInsets.Bottom * (normalized ? bitmapHeight :  scale) + offset,
                    }};
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

