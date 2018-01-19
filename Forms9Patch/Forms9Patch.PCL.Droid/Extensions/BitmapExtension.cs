//using System;
using Android.Graphics;

namespace Forms9Patch.Droid
{
	internal static class BitmapExtension
	{
		/*
		const int _black = (int)0xFF000000;
		public static bool IsBlackAt(this Bitmap bitmap, int x, int y) {
			return bitmap.GetPixel (x, y) == _black;
		}

		const int _transparent = 0x00000000;
		public static bool IsTransparentAt(this Bitmap bitmap, int x, int y) {
			return bitmap.GetPixel(x,y)==_transparent;
		}
		*/

		public static NinePatch NinePatch(this Bitmap bitmap) {
			var result = new NinePatch (bitmap);
			return result.Ranges == null ? null : result;
		}
	}
}

