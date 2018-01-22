using UIKit;
using CoreGraphics;

namespace Forms9Patch.iOS
{
	static class UIImageExtension
	{
		public static UIImage Crop(this UIImage srcImage, CGRect rect) {
			using (CGImage cgCroppedImage = srcImage.CGImage.WithImageInRect (rect)) {
				return UIImage.FromImage (cgCroppedImage);
			}
		}

		public static NinePatch NinePatch(this UIImage image) {
			var result = new NinePatch (image);
			return result.Ranges == null ? null : result;
		}

		public static UIImageView UIImageView(this UIImage image, CGRect rect) {
			var patch = new UIImageView(image.Crop(rect));
			patch.Frame = rect;
			patch.ContentMode = UIViewContentMode.ScaleToFill;
			return patch;
		}
	}
}

