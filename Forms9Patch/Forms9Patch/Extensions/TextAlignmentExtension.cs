using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	internal static class TextAlignmentExtension
	{
		internal static LayoutOptions ToLayoutOptions(this TextAlignment alignment) {
			switch (alignment) {
			case TextAlignment.Center:
				return LayoutOptions.Center;
			case TextAlignment.End:
				return LayoutOptions.End;
			default:
				return LayoutOptions.Start;
			}
		}
	}
}

