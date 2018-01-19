//using System;

#if __IOS__
namespace Forms9Patch.iOS
#elif __DROID__
namespace Forms9Patch.Droid
#else
namespace Forms9Patch
#endif
{
	internal interface INinePatch
	{
		int SourceWidth {
			get;
		}

		int SourceHeight {
			get;
		}

		RangeLists Ranges {
			get;
		}

		bool IsBlackAt(int x, int y);

		bool IsTransparentAt(int x, int y);

		int Pixel(int x, int y);
	}
}

