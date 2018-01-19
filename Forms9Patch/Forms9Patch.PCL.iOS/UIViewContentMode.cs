using System;
using UIKit;

namespace Forms9Patch.iOS
{
	/// <summary>
	/// Image extensions.
	/// </summary>
	public static class ImageExtensions
	{
		/// <summary>
		/// Transforms Forms9Patch.Fill to UIViewContentMode.
		/// </summary>
		/// <returns>UIViewContentMode</returns>
		/// <param name="aspect">Aspect.</param>
		public static UIViewContentMode ToUIViewContentMode (this Fill aspect)
		{
			switch (aspect) {
			case Fill.AspectFill:
				return UIViewContentMode.ScaleAspectFill;
			case Fill.Fill:
				return UIViewContentMode.ScaleToFill;
			}
			return UIViewContentMode.ScaleAspectFit;
		}
	}
}

