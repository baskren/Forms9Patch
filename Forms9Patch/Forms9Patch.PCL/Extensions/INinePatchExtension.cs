//using System;
using System.Collections.Generic;

#if __IOS__
namespace Forms9Patch.iOS
#elif __DROID__
namespace Forms9Patch.Droid
#else
namespace Forms9Patch
#endif
{
	internal static class INinePatchExtension
	{

		static public RangeLists NinePatchRanges(this INinePatch bitmap) {
			int width = bitmap.SourceWidth;
			int height = bitmap.SourceHeight;

			var capsX = new List<Range>();
			int pos = -1;
			for (int i=1;i<width-1;i++) {
				if (bitmap.IsBlackAt (i, 0)) {
					if (pos == -1) {
						pos = i - 1;
					}
				} else if (bitmap.IsTransparentAt (i, 0)) {
					if (pos != -1) {
						var range = new Range ();
						range.Start = pos;
						range.End = i - 1; 
						capsX.Add (range);
						pos = -1;
					}
				} else {
					// this is not a nine-patch;
					return null;
				}
			}
			if (pos != -1) {
				var range = new Range();
				range.Start = pos;
				range.End = width-2;
				capsX.Add( range );
			}

			var capsY = new List<Range>();
			pos = -1;
			for (int i=1;i<height-1;i++) {
				if (bitmap.IsBlackAt (0, i)) {
					if (pos == -1) {
						pos = i - 1;
					}
				} else if (bitmap.IsTransparentAt (0, i)) {
					if (pos != -1) {
						var range = new Range ();
						range.Start = pos;
						range.End = i - 1;
						capsY.Add (range);
						pos = -1;
					}
				} else {
					return null;
				}
			}
			if (pos != -1) {
				var range = new Range();
				range.Start = pos;
				range.End = height-2;
				capsY.Add( range );
			}

			var margX = null as Range;
			pos = -1;
			for (int i = 1; i < width - 1; i++) {
				if (bitmap.IsBlackAt (i, height - 1)) {
					if (pos == -1) {
						pos = i - 1;
						break;
					}
				}
			}
			if (pos != -1) {
				for (int i = width - 1; i > pos; i--) {
					if (bitmap.IsBlackAt (i, height - 1)) {
						margX = new Range();
						margX.Start = pos;
						margX.End = i-1; 
						break;
					}
				}
			}

			var margY = null as Range;
			pos = -1;
			for (int i = 1; i < height - 1; i++) {
				if (bitmap.IsBlackAt (width-1, i)) {
					if (pos == -1) {
						pos = i - 1;
						break;
					}
				}
			}
			if (pos != -1) {
				for (int i = height - 1; i > pos; i--) {
					if (bitmap.IsBlackAt (width-1, i)) {
						margY = new Range();
						margY.Start = pos;
						margY.End = i-1;
						break;
					}
				}
			}

			var rangeLists = new RangeLists();
			rangeLists.PatchesX = capsX;
			rangeLists.PatchesY = capsY;
			rangeLists.MarginX = margX;
			rangeLists.MarginY = margY;

			return rangeLists;
		}

	}
}

