using System;
using UIKit;
using Xamarin.Forms;

namespace Forms9Patch.iOS
{
	static class AlignmentExtensions
	{
		internal static UITextAlignment ToNativeTextAlignment (this TextAlignment alignment) {
			if (alignment == TextAlignment.Center) 
				return UITextAlignment.Center;
			return alignment != TextAlignment.End ? UITextAlignment.Left : UITextAlignment.Right;
		}
	}
}

