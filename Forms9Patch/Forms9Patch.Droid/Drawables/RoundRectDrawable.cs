using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Forms9Patch;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Util;

namespace Forms9Patch.Droid
{
    class RoundRectDrawable : Drawable
    {
        readonly IRoundedBox _element;
        Paint p;

        public override int Opacity
        {
            get { return 0; }
        }

        internal RoundRectDrawable(IRoundedBox element)
        {
            _element = element;
            p = new Paint
            {
                AntiAlias = true,
                FilterBitmap = true
            };
        }

        public override void Draw(Canvas canvas)
        {
            /*
			string text = "";
			var contentView = _element as Forms9Patch.ContentView;
			if (contentView != null) {
				var label = contentView.Content as Xamarin.Forms.Label;
				if (label != null)
					text = label.Text;
				System.Diagnostics.Debug.WriteLine ("\t\tDraw txt=["+text+"] w=["+canvas.Width+"] h=["+canvas.Height+"] ");
			}
			*/

            if (Bounds.Width() <= 0 || Bounds.Height() <= 0)
                return;

            var Element = _element as BindableObject;

            var backgroundColor = (Xamarin.Forms.Color)Element.GetValue(VisualElement.BackgroundColorProperty);
            var outlineColor = (Xamarin.Forms.Color)Element.GetValue(RoundedBoxBase.OutlineColorProperty);
            var outlineWidth = Math.Max(0, (float)Element.GetValue(RoundedBoxBase.OutlineWidthProperty) * Display.Scale);
            var isElliptical = (bool)Element.GetValue(RoundedBoxBase.IsEllipticalProperty);

            var materialButton = Element as MaterialButton;
            SegmentType type = materialButton == null ? SegmentType.Not : materialButton.SegmentType;
            StackOrientation orientation = materialButton == null ? StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

            float separatorWidth = materialButton == null || type == SegmentType.Not ?
                0 :
                materialButton.SeparatorWidth < 0 ?
                outlineWidth : Math.Max(0, materialButton.SeparatorWidth * Display.Scale);

            if (backgroundColor.A < 0.01 && (outlineColor.A < 0.01 || (outlineWidth < 0.01 && separatorWidth < 0.01)))
            {
                //base.Draw (canvas);
                return;
            }

            var outlineRadius = Math.Max(0f, (float)Element.GetValue(RoundedBoxBase.OutlineRadiusProperty) * Display.Scale);
            var hasShadow = (bool)Element.GetValue(RoundedBoxBase.HasShadowProperty);

            var hz = orientation == StackOrientation.Horizontal;
            var makeRoomForShadow = (materialButton == null ? hasShadow : (bool)Element.GetValue(MaterialButton.HasShadowProperty)) && hasShadow;


            var shadowInverted = ((bool)Element.GetValue(RoundedBoxBase.ShadowInvertedProperty));

            var shadowX = (int)(Forms9Patch.Settings.ShadowOffset.X * Display.Scale);
            var shadowY = (int)((Forms9Patch.Settings.ShadowOffset.Y) * Display.Scale);
            var shadowR = (int)((Forms9Patch.Settings.ShadowRadius) * Display.Scale);

            var perimeter = new RectF(Bounds);

            // exterior shadow
            if (makeRoomForShadow)
            {
                // what additional padding was allocated to cast the button's shadow?
                var shadowPad = RoundedBoxBase.ShadowPadding(Element as Layout);
                // shrink the button's perimeter by that extra padding (so it is the size it was originally intended to be)
                perimeter = new RectF((float)(shadowPad.Left * Display.Scale + Bounds.Left), (float)(shadowPad.Top * Display.Scale + Bounds.Top), (float)(Bounds.Right - shadowPad.Right * Display.Scale), (float)(Bounds.Bottom - shadowPad.Bottom * Display.Scale));

                // set for a raised shadow
                if (!shadowInverted && hasShadow && backgroundColor.A > 0)
                {
                    var shadow = Bitmap.CreateBitmap(Bounds.Width(), Bounds.Height(), Bitmap.Config.Argb8888);// (bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);
                    var c = new Canvas(shadow);
                    var boxShadow = new RectF(perimeter);
                    boxShadow.Offset(shadowX, shadowY);
                    boxShadow.Inset(-shadowR, -shadowR);
                    p.Color = Android.Graphics.Color.Black;
                    p.StrokeWidth = shadowR;
                    p.Alpha = 255 / (shadowR * 4);
                    p.SetStyle(Paint.Style.Fill);
                    var r = (int)(outlineRadius + shadowR);
                    for (int i = 0; i <= 2 * shadowR; i++)
                    {

                        CGPath path;
                        if (isElliptical)
                            path = EllipticalPath(boxShadow);
                        else
                            path = PerimeterPath(_element, boxShadow, r);
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
            var insetShadowBounds = new RectF(perimeter);

            CGPath clipPath = null;
            // generate background
            if (backgroundColor.A > 0)
            {
                p.Color = backgroundColor.ToAndroid();
                p.SetStyle(Paint.Style.Fill);
                RectF insetPerimeter;
                if (hz)
                {
                    switch (type)
                    {
                        case SegmentType.Start:
                            insetPerimeter = RectInset(perimeter, outlineWidth / 2.0f, outlineWidth / 2.0f, 0, outlineWidth / 2.0f);
                            break;
                        case SegmentType.Mid:
                            insetPerimeter = RectInset(perimeter, 0, outlineWidth / 2.0f, 0, outlineWidth / 2.0f);
                            break;
                        case SegmentType.End:
                            insetPerimeter = RectInset(perimeter, 0, outlineWidth / 2.0f, outlineWidth / 2.0f, outlineWidth / 2.0f);
                            break;
                        default:
                            insetPerimeter = new RectF(perimeter);
                            insetPerimeter.Inset(outlineWidth / 2.0f, outlineWidth / 2.0f);
                            break;
                    }
                }
                else
                {
                    switch (type)
                    {
                        case SegmentType.Start:
                            insetPerimeter = RectInset(perimeter, outlineWidth / 2.0f, outlineWidth / 2.0f, outlineWidth / 2.0f, 0);
                            break;
                        case SegmentType.Mid:
                            insetPerimeter = RectInset(perimeter, outlineWidth / 2.0f, 0, outlineWidth / 2.0f, 0);
                            break;
                        case SegmentType.End:
                            insetPerimeter = RectInset(perimeter, outlineWidth / 2.0f, 0, outlineWidth / 2.0f, outlineWidth / 2.0f);
                            break;
                        default:
                            insetPerimeter = new RectF(perimeter);
                            insetPerimeter.Inset(outlineWidth / 2.0f, outlineWidth / 2.0f);
                            break;
                    }
                }
                CGPath perimeterPath;
                if (isElliptical)
                    perimeterPath = EllipticalPath(insetPerimeter);
                else
                    perimeterPath = PerimeterPath(_element, insetPerimeter, outlineRadius + (outlineColor.A > 0 ? outlineWidth / 2.0f : 0));
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
                CGPath outlinePath;
                if (isElliptical)
                    outlinePath = EllipticalPath(perimeter);
                else
                    outlinePath = OutlinePath(_element, perimeter, outlineRadius, outlineWidth);
                canvas.DrawPath(outlinePath, p);
            }

            // separator
            if (separatorWidth > 0 && !isElliptical)
            {
                var inset = outlineColor.A > 0 ? outlineWidth / 2.0f : 0;
                p.StrokeWidth = separatorWidth / 1.5f;
                p.Color = outlineColor.ToAndroid();
                p.SetStyle(Paint.Style.Stroke);
                var path = new CGPath();
                if (type == SegmentType.Start || type == SegmentType.Mid)
                {
                    if (hz)
                    {
                        //g.MoveTo (perimeter.Right, perimeter.Top + inset);
                        //g.AddLineToPoint (perimeter.Right, perimeter.Bottom - inset);
                        path.MoveTo((float)Math.Round(perimeter.Right + inset / 2.0f), perimeter.Top + inset);
                        path.LineTo((float)Math.Round(perimeter.Right + inset / 2.0f), perimeter.Bottom - inset);
                    }
                    else
                    {
                        path.MoveTo(perimeter.Left + inset, (float)Math.Round(perimeter.Bottom + inset / 2.0f));
                        path.LineTo(perimeter.Right - inset, (float)Math.Round(perimeter.Bottom + inset / 2.0f));
                    }
                }
                if (type == SegmentType.Mid || type == SegmentType.End)
                {
                    if (hz)
                    {
                        path.MoveTo((float)Math.Round(perimeter.Left - inset / 2.0f), perimeter.Top + inset);
                        path.LineTo((float)Math.Round(perimeter.Left - inset / 2.0f), perimeter.Bottom - inset);
                    }
                    else
                    {
                        path.MoveTo(perimeter.Left + inset, (float)Math.Round(perimeter.Top - inset / 2.0f));
                        path.LineTo(perimeter.Right - inset, (float)Math.Round(perimeter.Top - inset / 2.0f));
                    }
                }
                canvas.DrawPath(path, p);
            }

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
                shadowPaint.StrokeWidth = shadowR;
                shadowPaint.Alpha = (int)(255 * 0.05 / shadowR);
                shadowPaint.SetStyle(Paint.Style.Stroke);
                for (int i = 0; i <= 2 * shadowR + 2 * Display.Scale; i++)
                {
                    var r = Math.Max(0, outlineRadius);// - shadowR);
                    if (isElliptical)
                        shadowCanvas.DrawPath(EllipticalPath(insetShadowBounds), shadowPaint);
                    else
                        shadowCanvas.DrawPath(PerimeterPath(_element, insetShadowBounds, r), shadowPaint);
                    insetShadowBounds.Inset(-1, -1);
                    //p.StrokeWidth -= 1;
                    //if (p.StrokeWidth < 0)
                    //	p.StrokeWidth = 0;
                    shadowPaint.Alpha += (int)(255 * (0.125 * (i + 1) / shadowR));
                }
                shadowPaint.Alpha = 255;

                //p.SetXfermode(
                var result = Bitmap.CreateBitmap(Bounds.Width(), Bounds.Height(), Bitmap.Config.Argb8888);
                var resultCanvas = new Canvas(result);
                var resultPaint = new Paint(PaintFlags.AntiAlias);
                resultPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.DstIn));
                resultCanvas.DrawBitmap(shadowBitmap, 0, 0, null);
                resultCanvas.DrawBitmap(maskBitmap, 0, 0, resultPaint);
                resultPaint.SetXfermode(null);

                canvas.DrawBitmap(result, 0, 0, p);
            }

        }


        static RectF SegmentAllowanceRect(RectF rect, int allowance, StackOrientation orientation, SegmentType type)
        {
            RectF result;
            if (type == SegmentType.Start)
                result = new RectF(rect.Left, rect.Top, rect.Right + (orientation == StackOrientation.Horizontal ? allowance : 0), rect.Bottom + (orientation == StackOrientation.Vertical ? allowance : 0));
            else if (type == SegmentType.Mid)
                result = new RectF(rect.Left - (orientation == StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == StackOrientation.Vertical ? allowance : 0), rect.Right + (orientation == StackOrientation.Horizontal ? allowance * 2 : 0), rect.Bottom + (orientation == StackOrientation.Vertical ? allowance * 2 : 0));
            else
                result = new RectF(rect.Left - (orientation == StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == StackOrientation.Vertical ? allowance : 0), rect.Right + (orientation == StackOrientation.Horizontal ? allowance : 0), rect.Bottom + (orientation == StackOrientation.Vertical ? allowance : 0));
            return result;
        }

        static RectF RectInset(RectF rect, float left, float top, float right, float bottom)
        {
            return new RectF(rect.Left + left, rect.Top + top, rect.Right - right, rect.Bottom - bottom);
        }

        internal static CGPath PerimeterPath(IRoundedBox element, RectF rect, float radius, bool counterClockWise = true)
        {
            var materialButton = element as MaterialButton;
            SegmentType type = materialButton == null ? SegmentType.Not : materialButton.SegmentType;
            StackOrientation orientation = materialButton == null ? StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

            var diameter = radius * 2;
            var topLeft = new RectF(rect.Left, rect.Top, rect.Left + diameter, rect.Top + diameter);
            var bottomLeft = new RectF(rect.Left, rect.Bottom - diameter, rect.Left + diameter, rect.Bottom);
            var bottomRight = new RectF(rect.Right - diameter, rect.Bottom - diameter, rect.Right, rect.Bottom);
            var topRight = new RectF(rect.Right - diameter, rect.Top, rect.Right, rect.Top + diameter);

            var result = new CGPath();

            if (counterClockWise)
            {
                // top center
                result.MoveTo((rect.Left + rect.Right) / 2.0f, rect.Top);

                // topLeft
                if (type == SegmentType.Start || type == SegmentType.Not)
                {
                    //result.LineTo (rect.Left + radius, rect.Top);
                    //result.AddRelativeArc (rect.Left + radius, rect.Top + radius, radius, (nfloat)(3 * Math.PI / 2.0), (nfloat)(-Math.PI / 2.0));
                    result.ArcTo(topLeft, 270, -90);
                }
                else
                {
                    result.LineTo(rect.Left, rect.Top);
                }

                // bottom left
                if (type == SegmentType.Start && orientation == StackOrientation.Horizontal || type == SegmentType.End && orientation == StackOrientation.Vertical || type == SegmentType.Not)
                {
                    //result.LineTo (rect.Left, rect.Bottom - radius);
                    //result.AddRelativeArc(rect.Left + radius, rect.Bottom - radius, radius, (nfloat)(Math.PI), (nfloat)(-Math.PI / 2.0));
                    result.ArcTo(bottomLeft, 180, -90);
                }
                else
                    result.LineTo(rect.Left, rect.Bottom);

                // bottom right
                if (type == SegmentType.End || type == SegmentType.Not)
                {
                    //result.LineTo (rect.Right - radius, rect.Bottom);
                    //result.AddRelativeArc (rect.Right - radius, rect.Bottom - radius, radius, (nfloat)(Math.PI/2.0), (nfloat)(-Math.PI / 2.0));
                    result.ArcTo(bottomRight, 90, -90);
                }
                else
                    result.LineTo(rect.Right, rect.Bottom);

                // top right
                if (type == SegmentType.End && orientation == StackOrientation.Horizontal || type == SegmentType.Start && orientation == StackOrientation.Vertical || type == SegmentType.Not)
                {
                    //result.LineTo (rect.Right, rect.Top + radius);					//result.AddArcToPoint (rect.Right, rect.Top - radius, rect.Right - radius, rect.Top, radius);
                    //result.AddRelativeArc (rect.Right - radius, rect.Top + radius, radius, 0, (nfloat)(-Math.PI / 2.0));
                    result.ArcTo(topRight, 0, -90);
                }
                else
                    result.LineTo(rect.Right, rect.Top);

                // finish
                result.LineTo((rect.Left + rect.Right) / 2.0f, rect.Top);
            }
            else
            {
                // Left Center
                result.MoveTo(rect.Left, (rect.Top + rect.Bottom) / 2.0f);

                // topLeft
                if (type == SegmentType.Start || type == SegmentType.Not)
                {
                    //result.LineTo (rect.Left, rect.Top + radius);
                    //result.AddRelativeArc (rect.Left + radius, rect.Top + radius, radius, (nfloat)(Math.PI), (nfloat)(Math.PI / 2.0));
                    result.ArcTo(topLeft, 180, 90);
                }
                else
                {
                    result.LineTo(rect.Left, rect.Top);
                }

                // top right
                if (type == SegmentType.End || type == SegmentType.Start && orientation == StackOrientation.Vertical || type == SegmentType.Not)
                {
                    //result.LineTo (rect.Right - radius, rect.Top);
                    //result.AddRelativeArc (rect.Right - radius, rect.Top + radius, radius, (nfloat)(3*Math.PI/2.0), (nfloat)(Math.PI / 2.0));
                    result.ArcTo(topRight, 270, 90);
                }
                else
                    result.LineTo(rect.Right, rect.Top);

                // bottom right
                if (type == SegmentType.End || type == SegmentType.Not)
                {
                    //result.LineTo (rect.Right, rect.Bottom - radius);
                    //result.AddRelativeArc (rect.Right - radius, rect.Bottom - radius, radius, 0, (nfloat)(Math.PI / 2.0));
                    result.ArcTo(bottomRight, 0, 90);
                }
                else
                    result.LineTo(rect.Right, rect.Bottom);

                // bottom left
                if (type == SegmentType.Start || type == SegmentType.End && orientation == StackOrientation.Vertical || type == SegmentType.Not)
                {
                    //result.LineTo (rect.Left + radius, rect.Bottom);
                    //result.AddRelativeArc(rect.Left + radius, rect.Bottom - radius, radius, (nfloat)(Math.PI / 2.0), (nfloat)(Math.PI / 2.0));
                    result.ArcTo(bottomLeft, 90, 90);
                }
                else
                    result.LineTo(rect.Left, rect.Bottom);

                //finish
                result.LineTo(rect.Left, (rect.Top + rect.Bottom) / 2.0f);
            }

            // finish
            //result.LineTo(startX, startY);
            result.LineTo((rect.Left + rect.Right) / 2.0f, rect.Top);


            return result;
        }

        static CGPath OutlinePath(IRoundedBox element, RectF rect, float radius, float lineWidth)
        {
            lineWidth /= 2.0f;
            var materialButton = element as MaterialButton;
            SegmentType type = materialButton == null ? SegmentType.Not : materialButton.SegmentType;
            StackOrientation orientation = materialButton == null ? StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

            if (type == SegmentType.Not)
                return PerimeterPath(element, rect, radius);

            var diameter = radius * 2;
            var topLeft = new RectF(rect.Left, rect.Top, rect.Left + diameter, rect.Top + diameter);
            var bottomLeft = new RectF(rect.Left, rect.Bottom - diameter, rect.Left + diameter, rect.Bottom);
            var bottomRight = new RectF(rect.Right - diameter, rect.Bottom - diameter, rect.Right, rect.Bottom);
            var topRight = new RectF(rect.Right - diameter, rect.Top, rect.Right, rect.Top + diameter);

            var result = new CGPath();

            if (orientation == StackOrientation.Horizontal)
            {
                if (type == SegmentType.Start)
                {
                    result.MoveTo(rect.Right + lineWidth, rect.Bottom);
                    //result.LineTo (rect.Left + radius, rect.Bottom);
                    //result.AddRelativeArc (rect.Left + radius, rect.Bottom - radius, radius, (nfloat)(1*Math.PI/2.0), (nfloat)(Math.PI / 2.0));
                    result.ArcTo(bottomLeft, 90, 90);
                    //result.LineTo (rect.Left,          rect.Top + radius);
                    //result.AddRelativeArc (rect.Left + radius, rect.Top    + radius, radius, (nfloat)(2*Math.PI/2.0), (nfloat)(Math.PI / 2.0));
                    result.ArcTo(topLeft, 180, 90);
                    result.LineTo(rect.Right + lineWidth, rect.Top);
                }
                else if (type == SegmentType.Mid)
                { // mid
                    result.MoveTo(rect.Right + lineWidth, rect.Bottom);
                    result.LineTo(rect.Left - lineWidth, rect.Bottom);
                    result.MoveTo(rect.Left - lineWidth, rect.Top);
                    result.LineTo(rect.Right + lineWidth, rect.Top);
                }
                else
                { // end
                    result.MoveTo(rect.Left - lineWidth, rect.Top);
                    //result.LineTo (rect.Right - radius, rect.Top);
                    //result.AddRelativeArc (rect.Right - radius, rect.Top + radius, radius, (nfloat)(3 * Math.PI / 2.0), (nfloat)(Math.PI / 2.0));
                    result.ArcTo(topRight, 270, 90);
                    //result.LineTo (rect.Right, rect.Bottom - radius);
                    //result.AddRelativeArc (rect.Right - radius, rect.Bottom - radius, radius, 0, (nfloat)(Math.PI / 2.0));
                    result.ArcTo(bottomRight, 0, 90);
                    result.LineTo(rect.Left - lineWidth, rect.Bottom);
                }
            }
            else
            { // vertical
                if (type == SegmentType.Start)
                {
                    result.MoveTo(rect.Right, rect.Bottom + lineWidth);
                    //result.LineTo (rect.Right, rect.Top + radius);
                    //result.AddRelativeArc(rect.Right - radius, rect.Top + radius, radius, 0, (nfloat)(-Math.PI / 2.0));
                    result.ArcTo(topRight, 0, -90);
                    //result.LineTo (rect.Left + radius, rect.Top);
                    //result.AddRelativeArc (rect.Left + radius, rect.Top + radius, radius, (nfloat)(3*Math.PI / 2.0), (nfloat)(-Math.PI / 2.0));
                    result.ArcTo(topLeft, 270, -90);
                    result.LineTo(rect.Left, rect.Bottom + lineWidth);
                }
                else if (type == SegmentType.Mid)
                {
                    result.MoveTo(rect.Right, rect.Bottom + lineWidth);
                    result.LineTo(rect.Right, rect.Top - lineWidth);
                    result.MoveTo(rect.Left, rect.Top - lineWidth);
                    result.LineTo(rect.Left, rect.Bottom + lineWidth);
                }
                else
                { // end
                    result.MoveTo(rect.Left, rect.Top - lineWidth);
                    //result.LineTo (rect.Left, rect.Bottom - radius);
                    //result.AddRelativeArc (rect.Left + radius, rect.Bottom - radius, radius, (nfloat)(2 * Math.PI / 2.0), (nfloat)(-Math.PI / 2.0));
                    result.ArcTo(bottomLeft, 180, -90);
                    //result.LineTo (rect.Right - radius, rect.Bottom);
                    //result.AddRelativeArc (rect.Right - radius, rect.Bottom - radius, radius, (nfloat)(1 * Math.PI / 2.0), (nfloat)(-Math.PI / 2.0));
                    result.ArcTo(bottomRight, 90, -90);
                    result.LineTo(rect.Right, rect.Top - lineWidth);
                }
            }
            return result;
        }

        static CGPath EllipticalPath(RectF rect, bool counterClockWise = true)
        {
            var path = new CGPath();
            path.AddOval(rect, counterClockWise ? Path.Direction.Ccw : Path.Direction.Cw);
            return path;
        }
        public override void SetAlpha(int alpha) { }

        public override void SetColorFilter(ColorFilter colorFilter) { }
    }
}

