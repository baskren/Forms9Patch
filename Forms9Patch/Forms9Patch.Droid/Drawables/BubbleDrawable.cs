using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Forms9Patch;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

namespace Forms9Patch.Droid
{
    class BubbleDrawable : Drawable
    {
        readonly BubbleLayout _element;
        Paint p;

        public override int Opacity
        {
            get { return 0; }
        }

        internal BubbleDrawable(BubbleLayout element)
        {
            _element = element;
            p = new Paint
            {
                AntiAlias = true,
            };
        }

        public override void Draw(Canvas canvas)
        {
            //System.Diagnostics.Debug.WriteLine ("\t\tBubbleDrawable.Draw.Bounds=[{0}, {1}, {2}, {3}]", Bounds.Left, Bounds.Top, Bounds.Width(), Bounds.Height());

            //System.Diagnostics.Debug.WriteLine ("Draw");
            var backgroundColor = (Xamarin.Forms.Color)_element.GetValue(BubbleLayout.BackgroundColorProperty);
            var outlineColor = (Xamarin.Forms.Color)_element.GetValue(RoundedBoxBase.OutlineColorProperty);
            var outlineWidth = Math.Max(0, (float)_element.GetValue(RoundedBoxBase.OutlineWidthProperty) * Display.Scale);







            if (Bounds.Width() <= 0 || Bounds.Height() <= 0)
                return;


            if (backgroundColor.A < 0.01 && (outlineColor.A < 0.01 || outlineWidth < 0.01))
            {
                //base.Draw (canvas);
                return;
            }

            var outlineRadius = Math.Max(0f, (float)_element.GetValue(RoundedBoxBase.OutlineRadiusProperty) * Display.Scale);
            var hasShadow = (bool)_element.GetValue(BubbleLayout.HasShadowProperty);





            var shadowInverted = ((bool)_element.GetValue(RoundedBoxBase.ShadowInvertedProperty));

            int shadowX = (int)(Forms9Patch.Settings.ShadowOffset.X * Display.Scale);
            int shadowY = (int)((Forms9Patch.Settings.ShadowOffset.Y) * Display.Scale);
            int shadowR = (int)((Forms9Patch.Settings.ShadowRadius) * Display.Scale);

            var perimeter = new CGRect(Bounds);
            //System.Diagnostics.Debug.WriteLine ("bounds=["+Bounds+"]");
            if (hasShadow)
            {
                // what additional padding was allocated to cast the button's shadow?
                var shadowPad = BubbleLayout.ShadowPadding(_element);
                // shrink the button's perimeter by that extra padding (so it is the size it was originally intended to be)
                perimeter = new CGRect((float)(shadowPad.Left * Display.Scale + Bounds.Left), (float)(shadowPad.Top * Display.Scale + Bounds.Top), (float)(Bounds.Right - shadowPad.Right * Display.Scale), (float)(Bounds.Bottom - shadowPad.Bottom * Display.Scale));

                // set for a raised shadow
                if (!shadowInverted && hasShadow && backgroundColor.A > 0)
                {
                    var shadow = Bitmap.CreateBitmap(Bounds.Width(), Bounds.Height(), Bitmap.Config.Argb8888);// (bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);
                    var c = new Canvas(shadow);
                    var boxShadow = new CGRect(perimeter);
                    boxShadow.Offset(shadowX, shadowY);
                    boxShadow.Inset(-shadowR, -shadowR);
                    p.Color = Android.Graphics.Color.Black;
                    p.StrokeWidth = (float)shadowR;
                    p.Alpha = 255 / (shadowR * 4);
                    p.SetStyle(Paint.Style.Fill);
                    int r = (int)(outlineRadius + shadowR);
                    for (int i = 0; i <= 2 * shadowR; i++)
                    {
                        var path = PerimeterPath(_element, boxShadow, r);
                        c.DrawPath(path, p);
                        boxShadow.Inset(1, 1);
                        //p.StrokeWidth -= 1;
                        //if (p.StrokeWidth < 0)
                        //	p.StrokeWidth = 0;
                        p.Alpha += 14 * (i + 1) / shadowR;
                        r -= 1;
                        if (r < 0)
                            r = 0;
                    }
                    canvas.DrawBitmap(shadow, 0, 0, p);
                }
            }
            var insetShadowBounds = new CGRect(perimeter);

            Path clipPath = null;
            // generate background
            if (backgroundColor.A > 0)
            {
                p.Color = backgroundColor.ToAndroid();
                p.SetStyle(Paint.Style.Fill);













                var insetPerimeter = new CGRect(perimeter);
                insetPerimeter.Inset(outlineWidth / 2.0f, outlineWidth / 2.0f);



















                Path perimeterPath = PerimeterPath(_element, insetPerimeter, outlineRadius + (outlineColor.A > 0 ? outlineWidth / 2.0f : 0));
                canvas.DrawPath(perimeterPath, p);
                clipPath = perimeterPath;
            }

            // shrink the perimeter by the outline width
            if (outlineColor.A > 0)
                perimeter.Inset(outlineWidth / 2.0f, outlineWidth / 2.0f);

            // outline
            if (outlineWidth > 0 && outlineColor.A > 0)
            {
                p.StrokeWidth = (outlineWidth);
                p.Color = outlineColor.ToAndroid();
                p.SetStyle(Paint.Style.Stroke);
                var outlinePath = PerimeterPath(_element, perimeter, outlineRadius);
                canvas.DrawPath(outlinePath, p);
            }






























            /*
			// interior "inset" shadow
			if (hasShadow && backgroundColor.A > 0 && shadowInverted) {
				var shadow = Bitmap.CreateBitmap (Bounds.Width(), Bounds.Height(), Bitmap.Config.Argb8888);// (bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);
				var c = new Canvas (shadow);

				c.ClipPath (PerimeterPath(_element, perimeter, outlineRadius));

				insetShadowBounds.Offset (shadowX, shadowY);//+1);
				insetShadowBounds.Inset (shadowR/2, shadowR/2);//(shadowR-1)/2);
				p.Color = Android.Graphics.Color.Black;
				p.StrokeWidth = (float)shadowR;
				p.Alpha = 255 / (shadowR * 3);
				p.SetStyle (Paint.Style.Stroke);
				for (int i = 0; i <= 2*shadowR; i++) {
					var r = Math.Max (0, outlineRadius);// - shadowR);
					c.DrawPath(PerimeterPath(_element, insetShadowBounds, r), p);
					insetShadowBounds.Inset (-1, -1);
					p.StrokeWidth -= 1;
					if (p.StrokeWidth < 0)
						p.StrokeWidth = 0;
					p.Alpha += (int)(1.5 * 7 * (i+1) / shadowR);
				}
				p.Alpha = 255;
				canvas.DrawBitmap(shadow, 0, 0, p);
			}
			*/

            // interior "inset" shadow
            if (hasShadow && backgroundColor.A > 0 && shadowInverted)
            {

                var maskBitmap = Bitmap.CreateBitmap(Bounds.Width(), Bounds.Height(), Bitmap.Config.Argb8888);
                var maskCanvas = new Canvas(maskBitmap);
                var maskPaint = new Paint(PaintFlags.AntiAlias);
                maskPaint.Color = Android.Graphics.Color.Black;
                maskPaint.SetStyle(Paint.Style.Fill);
                maskCanvas.DrawPath(clipPath, maskPaint);

                var shadowBitmap = Bitmap.CreateBitmap(Bounds.Width(), Bounds.Height(), Bitmap.Config.Argb8888);// (bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);
                var shadowCanvas = new Canvas(shadowBitmap);
                var shadowPaint = new Paint(PaintFlags.AntiAlias);

                //shadowCanvas.ClipPath (PerimeterPath(_element, perimeter, outlineRadius));
                //shadowCanvas.ClipPath(clipPath);

                insetShadowBounds.Offset(shadowX, shadowY);//+1);
                insetShadowBounds.Inset(shadowR / 2, shadowR / 2);//(shadowR-1)/2);
                shadowPaint.Color = Android.Graphics.Color.Black;
                shadowPaint.StrokeWidth = (float)shadowR;
                shadowPaint.Alpha = (int)(255 * 0.05 / shadowR);
                shadowPaint.SetStyle(Paint.Style.Stroke);
                for (int i = 0; i <= 2 * shadowR + 2 * Display.Scale; i++)
                {
                    var r = Math.Max(0, outlineRadius);// - shadowR);
                    shadowCanvas.DrawPath(PerimeterPath(_element, insetShadowBounds, r), shadowPaint);
                    insetShadowBounds.Inset(-1, -1);
                    //p.StrokeWidth -= 1;
                    //if (p.StrokeWidth < 0)
                    //	p.StrokeWidth = 0;
                    shadowPaint.Alpha += (int)(255 * (0.125 * (i + 1) / shadowR));
                }
                shadowPaint.Alpha = 255;

                //p.SetXfermode(
                Bitmap result = Bitmap.CreateBitmap(Bounds.Width(), Bounds.Height(), Bitmap.Config.Argb8888);
                Canvas resultCanvas = new Canvas(result);
                Paint resultPaint = new Paint(PaintFlags.AntiAlias);
                resultPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.DstIn));
                resultCanvas.DrawBitmap(shadowBitmap, 0, 0, null);
                resultCanvas.DrawBitmap(maskBitmap, 0, 0, resultPaint);
                resultPaint.SetXfermode(null);

                canvas.DrawBitmap(result, 0, 0, p);

            }

            //GC.Collect ();

        }













        static CGRect RectInset(CGRect rect, float left, float top, float right, float bottom)
        {
            return new CGRect(rect.Left + left, rect.Top + top, rect.Right - right, rect.Bottom - bottom);
        }


        static CGPath PerimeterPath(BubbleLayout element, CGRect rect, float radius)
        {

            if (element.PointerDirection == PointerDirection.None)
                return RoundRectDrawable.PerimeterPath(element, rect, radius);

            var length = element.PointerLength * Display.Scale;

            if (radius * 2 > rect.Height - (element.PointerDirection.IsVertical() ? length : 0))
                radius = (float)((rect.Height - (element.PointerDirection.IsVertical() ? length : 0)) / 2.0);
            if (radius * 2 > rect.Width - (element.PointerDirection.IsHorizontal() ? length : 0))
                radius = (float)((rect.Width - (element.PointerDirection.IsHorizontal() ? length : 0)) / 2.0);

            var filetRadius = element.PointerCornerRadius;
            var tipRadius = element.PointerTipRadius * Display.Scale;

            if (filetRadius / 2.0 + tipRadius / 2.0 > length)
            {
                filetRadius = (float)(2 * (length - tipRadius / 2.0));
                if (filetRadius < 0)
                {
                    filetRadius = 0;
                    tipRadius = 2 * length;
                }
            }

            //System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "L-Fr/2=["+(length-filetRadius/2.0)+"]  Tr/2=[" + (tipRadius/2.0) + "] length=["+length+"]");
            if (length - filetRadius / 2.0 < tipRadius / 2.0)
                tipRadius = (float)(2 * (length - filetRadius / 2.0));
            //System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "L-Fr/2=["+(length-filetRadius/2.0)+"]  Tr=/2[" + (tipRadius/2.0) + "] length=["+length+"]");



            var result = new CGPath();
            var position = element.PointerAxialPosition;
            if (position <= 1.0)
            {
                if (element.PointerDirection == PointerDirection.Down || element.PointerDirection == PointerDirection.Up)
                    position = rect.Width * position;
                else
                    position = rect.Height * position;
            }
            var lft = rect.Left;
            var rht = rect.Right;
            var top = rect.Top;
            var bot = rect.Bottom;

            const float sqrt3 = (float)1.732050807568877;
            const float sqrt3d2 = (float)0.86602540378444;
            const float PId3 = (float)(Math.PI / 3.0);
            const float PId2 = (float)(Math.PI / 2.0);
            const float PId6 = (float)(Math.PI / 6.0);

            float tipCornerHalfWidth = tipRadius * sqrt3d2;
            float pointerToCornerIntercept = (float)Math.Sqrt((2 * radius * Math.Sin(Math.PI / 12.0)) * (2 * radius * Math.Sin(Math.PI / 12.0)) - (radius * radius / 4.0));

            float pointerAtLimitSansTipHalfWidth = (float)(pointerToCornerIntercept + radius / (2.0 * sqrt3) + (length - tipRadius / 2.0) / sqrt3);
            float pointerAtLimitHalfWidth = pointerAtLimitSansTipHalfWidth + tipRadius * sqrt3d2;

            float pointerSansFiletHalfWidth = (float)(tipCornerHalfWidth + (length - filetRadius / 2.0 - tipRadius / 2.0) / sqrt3);
            float pointerFiletWidth = filetRadius * sqrt3d2;
            float pointerAndFiletHalfWidth = pointerSansFiletHalfWidth + pointerFiletWidth;

            if (element.PointerDirection.IsHorizontal())
            {
                int dir = 1;
                float start = lft;
                float end = rht;
                if (element.PointerDirection == PointerDirection.Right)
                {
                    dir = -1;
                    start = rht;
                    end = lft;
                }
                float tip = position;
                if (tip > rect.Height - pointerAtLimitHalfWidth)
                    tip = rect.Height - pointerAtLimitHalfWidth;
                if (tip < pointerAtLimitHalfWidth)
                    tip = pointerAtLimitHalfWidth;
                if (rect.Height <= 2 * pointerAtLimitHalfWidth)
                    tip = (float)(rect.Height / 2.0);
                result.MoveToPoint(start + dir * (length + radius), top);
                result.AddLineToPoint(end - dir * radius, top);
                result.AddRelativeArc(end - dir * radius, top + radius, radius, -PId2, dir * PId2);
                result.AddLineToPoint(end, bot - radius);
                result.AddRelativeArc(end - dir * radius, bot - radius, radius, PId2 - dir * PId2, dir * PId2);
                result.AddLineToPoint(start + dir * (length + radius), bot);
                // bottom half
                if (tip >= rect.Height - pointerAndFiletHalfWidth - radius)
                {
                    double endRatio = (rect.Height - tip) / (pointerAndFiletHalfWidth + radius);
                    //System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "A endRatio=[" + endRatio + "]");
                    result.AddCurveToPoint(
                        new CGPoint(start + dir * (length + radius - endRatio * 4 * radius / 3.0), bot),
                        new CGPoint(start + dir * (length - filetRadius / 2.0 + filetRadius * sqrt3d2), top + tip + pointerSansFiletHalfWidth + filetRadius / 2.0),
                        new CGPoint(start + dir * (length - filetRadius / 2.0), top + tip + pointerSansFiletHalfWidth));
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "B");
                    result.AddRelativeArc(
                        start + dir * (length + radius),
                        bot - radius,
                        radius, PId2, dir * PId2);
                    result.AddLineToPoint(
                        start + dir * length,
                        top + tip + pointerAndFiletHalfWidth
                    );
                    result.AddRelativeArc(
                        start + dir * (length - filetRadius),
                        top + tip + pointerAndFiletHalfWidth,
                        filetRadius, PId2 - dir * PId2, dir * -PId3);
                }

                //tip
                result.AddLineToPoint(
                    (float)(start + dir * tipRadius / 2.0),
                    top + tip + tipCornerHalfWidth
                );
                result.AddRelativeArc(
                    start + dir * tipRadius,
                    top + tip,
                    tipRadius,
                    PId2 + dir * PId6,
                    dir * 2 * PId3);
                result.AddLineToPoint(
                    (float)(start + dir * (length - filetRadius / 2.0)),
                    top + tip - pointerSansFiletHalfWidth
                );

                // top half
                if (tip <= pointerAndFiletHalfWidth + radius)
                {
                    double startRatio = tip / (pointerAndFiletHalfWidth + radius);
                    //System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "C startRatio=[" + startRatio + "]");
                    result.AddCurveToPoint(
                        new CGPoint(start + dir * (length - filetRadius / 2.0 + filetRadius * sqrt3d2), top + tip - pointerSansFiletHalfWidth - filetRadius / 2.0),
                        new CGPoint(start + dir * (length + radius - startRatio * 4 * radius / 3.0), top),
                        new CGPoint(start + dir * (length + radius), top)
                    );
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "D");
                    result.AddRelativeArc(
                        start + dir * (length - filetRadius),
                        top + tip - pointerAndFiletHalfWidth,
                        filetRadius,
                        PId2 - dir * PId6,
                        dir * -PId3);
                    result.AddLineToPoint(
                        start + dir * length,
                        top + radius
                    );
                    result.AddRelativeArc(
                        start + dir * (length + radius),
                        top + radius,
                        radius,
                        PId2 + dir * PId2,
                        dir * PId2);
                }
            }
            else
            {
                int dir = 1;
                float start = top;
                float end = bot;
                if (element.PointerDirection == PointerDirection.Down)
                {
                    dir = -1;
                    start = bot;
                    end = top;
                }
                float tip = position;
                if (tip > rect.Width - pointerAtLimitHalfWidth)
                    tip = rect.Width - pointerAtLimitHalfWidth;
                if (tip < pointerAtLimitHalfWidth)
                    tip = pointerAtLimitHalfWidth;
                if (rect.Width <= 2 * pointerAtLimitHalfWidth)
                    tip = (float)(rect.Width / 2.0);
                result.MoveToPoint(lft, start + dir * (length + radius));
                result.AddLineToPoint(lft, end - dir * radius);
                result.AddRelativeArc(lft + radius, end - dir * radius, radius, (float)(Math.PI), dir * -PId2);
                result.AddLineToPoint(rht - radius, end);
                result.AddRelativeArc(rht - radius, end - dir * radius, radius, dir * PId2, dir * -PId2);
                result.AddLineToPoint(rht, start + dir * (radius + length));
                // right half
                if (tip > rect.Width - pointerAndFiletHalfWidth - radius)
                {
                    double endRatio = (rect.Width - tip) / (pointerAndFiletHalfWidth + radius);
                    result.AddCurveToPoint(
                        new CGPoint(rht, start + dir * (length + radius - endRatio * 4 * radius / 3.0)),
                        new CGPoint(lft + tip + pointerSansFiletHalfWidth + filetRadius / 2.0, start + dir * (length - filetRadius / 2.0 + filetRadius * sqrt3d2)),
                        new CGPoint(lft + tip + pointerSansFiletHalfWidth, start + dir * (length - filetRadius / 2.0))
                    );
                }
                else
                {
                    result.AddRelativeArc(
                        rht - radius,
                        start + dir * (length + radius),
                        radius, 0, dir * -PId2);
                    result.AddLineToPoint(
                        lft + tip + pointerAndFiletHalfWidth,
                        start + dir * length
                    );
                    result.AddRelativeArc(
                        lft + tip + pointerAndFiletHalfWidth,
                        start + dir * (length - filetRadius),
                        filetRadius, dir * PId2, dir * PId3);
                }
                //tip
                result.AddLineToPoint(
                    lft + tip + tipCornerHalfWidth,
                    (float)(start + dir * tipRadius / 2.0)
                );
                result.AddRelativeArc(
                    lft + tip,
                    start + dir * tipRadius,
                    tipRadius,
                    dir * -PId6,
                    dir * -2 * PId3);
                result.AddLineToPoint(
                    lft + tip - pointerSansFiletHalfWidth,
                    (float)(start + dir * (length - filetRadius / 2.0))
                );

                // left half
                if (tip < pointerAndFiletHalfWidth + radius)
                {
                    double startRatio = tip / (pointerAndFiletHalfWidth + radius);
                    result.AddCurveToPoint(
                        new CGPoint(
                            lft + tip - pointerSansFiletHalfWidth - filetRadius / 2.0,
                            start + dir * (length - filetRadius / 2.0 + filetRadius * sqrt3d2)
                        ),
                        new CGPoint(
                            lft,
                            start + dir * (length + radius - startRatio * 4 * radius / 3.0)
                        ),
                        new CGPoint(
                            lft,
                            start + dir * (length + radius)
                        )
                    );
                }
                else
                {
                    result.AddRelativeArc(
                        lft + tip - pointerAndFiletHalfWidth,
                        start + dir * (length - filetRadius),
                        filetRadius,
                        dir * PId6,
                        dir * PId3);
                    result.AddLineToPoint(
                        lft + radius,
                        start + dir * length
                    );
                    result.AddRelativeArc(
                        lft + radius,
                        start + dir * (length + radius),
                        radius,
                        dir * -PId2,
                        dir * -PId2);
                }

            }

            return result;
        }

        public override void SetAlpha(int alpha)
        {
        }

        public override void SetColorFilter(ColorFilter colorFilter)
        {
        }
    }
}

