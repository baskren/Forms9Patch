// /*******************************************************************
//  *
//  * LineBreakModeExtension.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Android.Text;
namespace Forms9Patch.Droid
{
	static class LineBreakModeExtension
	{
		public static TextUtils.TruncateAt ToEllipsize(this Xamarin.Forms.LineBreakMode lineBreakMode)
		{
			switch (lineBreakMode)
			{
				case Xamarin.Forms.LineBreakMode.HeadTruncation:
					return TextUtils.TruncateAt.Start;
				case Xamarin.Forms.LineBreakMode.MiddleTruncation:
					return TextUtils.TruncateAt.Middle;
				case Xamarin.Forms.LineBreakMode.TailTruncation:
					return TextUtils.TruncateAt.End;
			}
			return null;
		}
	}
}

