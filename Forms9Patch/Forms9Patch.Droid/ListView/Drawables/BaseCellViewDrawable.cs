using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

namespace Forms9Patch.Droid
{
	class BaseCellViewDrawable : Drawable
	{
		readonly BaseCellView _element;
		Paint p;

		public override int Opacity {
			get { return 0; }
		}

		internal BaseCellViewDrawable (BaseCellView element) {
			_element = element;
			p = new Paint { 
				AntiAlias = true, 
				FilterBitmap = true
			};
		}

		public override void Draw (Canvas canvas) {
			//System.Diagnostics.Debug.WriteLine("\tDrawing");
			if (Bounds.Width() <= 0 || Bounds.Height() <= 0)
				return;

			var scale = Forms.Context.Resources.DisplayMetrics.Density;

			var backgroundColor = (Xamarin.Forms.Color) _element.GetValue (VisualElement.BackgroundColorProperty);

			var separatorColor = _element.SeparatorColor;
			if (separatorColor == Xamarin.Forms.Color.Default) {
				separatorColor = Xamarin.Forms.Color.FromRgb(224, 224, 224);
			}
			var separatorHeight = _element.SeparatorHeight<0 ? 1.0f : (float)(Math.Max (0, _element.SeparatorHeight) ) ;

			canvas.DrawColor (backgroundColor.ToAndroid ());
		
			// separator
			if (_element.SeparatorIsVisible && separatorHeight > 0 && separatorColor.A > 0) {
				p.StrokeWidth = separatorHeight * scale;
				p.Color = separatorColor.ToAndroid ();
				p.SetStyle (Paint.Style.Stroke);
				var path = new Path ();
				path.MoveTo ((float)_element.SeparatorLeftIndent * scale, 0);
				path.LineTo ((float)(_element.Width - _element.SeparatorRightIndent) * scale, 0);
				canvas.DrawPath(path, p);
			}
		}
			
		public override void SetAlpha (int alpha) { }

		public override void SetColorFilter (ColorFilter colorFilter) { }	}
}

