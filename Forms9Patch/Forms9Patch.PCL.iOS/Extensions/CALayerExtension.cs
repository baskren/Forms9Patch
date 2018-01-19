using System;
using System.Drawing;
using CoreAnimation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Forms9Patch.iOS
{
	static class CALayerExtension
	{
		public static void LayoutRoundedRect(this CALayer layer, IRoundedBox element) {
			if ( element.BackgroundColor.A < 0.01 && (element.OutlineColor.A  < 0.01 || element.OutlineWidth  < 0.01))
				return;

			if (element.HasShadow) {
				layer.ShadowRadius = 2;
				layer.ShadowColor = UIColor.Black.CGColor;
				layer.ShadowOpacity = (float)0.5;
				layer.ShadowOffset = new SizeF (1, 1);
			} else {
				layer.ShadowOpacity = 0;
			}

			if (element.OutlineColor == Color.Default) {
				layer.BorderColor = UIColor.Clear.CGColor;
			} else {
				layer.BorderColor = element.OutlineColor.ToCGColor ();
				layer.BorderWidth = (nfloat)element.OutlineWidth;
			}

			layer.RasterizationScale = UIScreen.MainScreen.Scale;
			layer.ShouldRasterize = true;
			layer.BackgroundColor = element.BackgroundColor == Color.Default ? UIColor.White.CGColor : element.BackgroundColor.ToCGColor ();
			layer.CornerRadius = (nfloat)element.OutlineRadius;
			//layer.MasksToBounds = true;
		}
	}
}

