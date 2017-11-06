using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Forms9Patch;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Util;
using Forms9Patch.Enums;

namespace Forms9Patch.Droid
{
    class RoundRectDrawable : Drawable
    {
        #region ElementFields
        readonly IRoundedBox _element;
        IRoundedBox RoundedBoxElement
        {
            get
            {
                return _element;
            }
        }
        Paint p;

        public override int Opacity
        {
            get { return 0; }
        }
        #endregion


        #region Constructor
        internal RoundRectDrawable(IRoundedBox element)
        {
            _element = element;
            p = new Paint
            {
                AntiAlias = true,
                FilterBitmap = true
            };
        }
        #endregion


        #region Overrides
        public override void SetAlpha(int alpha) { }

        public override void SetColorFilter(ColorFilter colorFilter) { }
        #endregion


        #region Layout
        public override void Draw(Canvas canvas)
        {
            if (RoundedBoxElement is MaterialSegmentedControl)
                return;

            //var radius = RoundedBoxElement.OutlineRadius;// * Display.Scale;
            var radius = RoundedBoxElement.OutlineRadius * Display.Scale;
            var outlineWidth = RoundedBoxElement.OutlineWidth * Display.Scale; // (nfloat)(Math.Max(0, (float)Element.GetValue(RoundedBoxBase.OutlineWidthProperty)));

            bool drawOutline = outlineWidth > 0.05 && RoundedBoxElement.OutlineColor.A > 0.01;
            bool drawFill = RoundedBoxElement.BackgroundColor.A > 0.01;

            if (!drawFill && !drawOutline)
                return;

            var visualElement = RoundedBoxElement as VisualElement;

            if (Bounds.Width() <= 0 || Bounds.Height() <= 0)
                return;

            var rect = new RectF(Bounds.Left, Bounds.Top, Bounds.Right, Bounds.Bottom); //new Rect(0, 0, width, height);

            ButtonShape segmentType = ButtonShape.Rectangle;
            var hz = true;
            if (RoundedBoxElement is MaterialButton materialButton)
            {
                segmentType = materialButton.SegmentType;
                hz = materialButton.ParentSegmentsOrientation == StackOrientation.Horizontal;
            }
            else
                materialButton = null;

            var vt = !hz;

            float separatorWidth = materialButton == null || segmentType == ButtonShape.Rectangle ? 0 : materialButton.SeparatorWidth < 0 ? outlineWidth : Math.Max(0, materialButton.SeparatorWidth);
            if (RoundedBoxElement.BackgroundColor.A < 0.01 && (RoundedBoxElement.OutlineColor.A < 0.01 || (outlineWidth < 0.01 && separatorWidth < 0.01)))
                return;

            var makeRoomForShadow = RoundedBoxElement.HasShadow && RoundedBoxElement.BackgroundColor.A > 0.01;// && !RoundedBoxElement.ShadowInverted;

            var shadowX = (float)Forms9Patch.Settings.ShadowOffset.X * Display.Scale;
            var shadowY = (float)Forms9Patch.Settings.ShadowOffset.Y * Display.Scale;
            var shadowR = (float)Forms9Patch.Settings.ShadowRadius * Display.Scale;

            var shadowPadding = RoundedBoxBase.ShadowPadding(RoundedBoxElement as Layout);

            RectF perimeter = rect;

            System.Diagnostics.Debug.WriteLine("Bounds=[" + Bounds + "]");

            // exterior shadow
            if (makeRoomForShadow)
            {
                // what additional padding was allocated to cast the button's shadow?
                //var shadowPad = RoundedBoxBase.ShadowPadding(Element as Layout);
                // shrink the button's perimeter by that extra padding (so it is the size it was originally intended to be)
                perimeter = new RectF((float)(shadowPadding.Left * Display.Scale + Bounds.Left), (float)(shadowPadding.Top * Display.Scale + Bounds.Top), (float)(Bounds.Right - shadowPadding.Right * Display.Scale), (float)(Bounds.Bottom - shadowPadding.Bottom * Display.Scale));

                // set for a raised shadow
                if (!RoundedBoxElement.ShadowInverted)
                {
                    var shadow = Bitmap.CreateBitmap(Bounds.Width(), Bounds.Height(), Bitmap.Config.Argb8888);// (bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);
                    var c = new Canvas(shadow);
                    var boxShadow = new RectF(perimeter);
                    boxShadow.Offset(shadowX, shadowY);
                    boxShadow.Inset(-shadowR, -shadowR);
                    p.Color = Android.Graphics.Color.Black;
                    p.StrokeWidth = shadowR;
                    p.Alpha = (int)(255 / (shadowR * 4));
                    p.SetStyle(Paint.Style.Fill);
                    var r = (int)(radius + shadowR);
                    for (int i = 0; i <= 2 * shadowR; i++)
                    {

                        CGPath path;
                        if (RoundedBoxElement.IsElliptical)
                            path = Ellipse(boxShadow);
                        else
                            path = PerimeterPath(_element, boxShadow, r);
                        c.DrawPath(path, p);
                        boxShadow.Inset(1, 1);
                        //p.StrokeWidth -= 1;
                        //if (p.StrokeWidth < 0)
                        //	p.StrokeWidth = 0;
                        p.Alpha += (int)(14 * (i + 1) / shadowR);
                        r -= 1;
                        if (r < 0)
                            r = 0;
                    }
                    canvas.DrawBitmap(shadow, 0, 0, p);

                    c.Dispose();
                    shadow.Dispose();
                }
            }

            // generate background
            if (drawFill)
            {
                CGPath fillPath = RoundedBoxPath(perimeter, RoundRectPath.Fill);
                p.Color = RoundedBoxElement.BackgroundColor.ToAndroid();
                //p.Color = Android.Graphics.Color.Blue;
                p.SetStyle(Paint.Style.Fill);
                canvas.DrawPath(fillPath, p);
            }

            if (drawOutline)
            {
                CGPath outlinePath = RoundedBoxPath(perimeter, RoundRectPath.Outline);
                p.StrokeWidth = (outlineWidth);
                p.Color = RoundedBoxElement.OutlineColor.ToAndroid();
                p.SetStyle(Paint.Style.Stroke);
                canvas.DrawPath(outlinePath, p);
            }


            // interior "inset" shadow
            if (RoundedBoxElement.HasShadow && RoundedBoxElement.BackgroundColor.A > 0 && RoundedBoxElement.ShadowInverted)
            {
                var maskBitmap = Bitmap.CreateBitmap(Bounds.Width(), Bounds.Height(), Bitmap.Config.Argb8888);
                var maskCanvas = new Canvas(maskBitmap);
                var maskPaint = new Paint(PaintFlags.AntiAlias);
                maskPaint.Color = Android.Graphics.Color.Black;
                maskPaint.SetStyle(Paint.Style.Fill);
                maskCanvas.DrawPath(RoundedBoxPath(perimeter, 0), maskPaint);

                var shadowBitmap = Bitmap.CreateBitmap(Bounds.Width(), Bounds.Height(), Bitmap.Config.Argb8888);// (bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);
                var shadowCanvas = new Canvas(shadowBitmap);
                var shadowPaint = new Paint(PaintFlags.AntiAlias);

                //shadowCanvas.ClipPath (PerimeterPath(_element, perimeter, outlineRadius));
                //shadowCanvas.ClipPath(clipPath);

                var insetShadowBounds = new RectF(perimeter);
                insetShadowBounds.Offset(shadowX, shadowY);//+1);
                insetShadowBounds.Inset(shadowR / 2, shadowR / 2);//(shadowR-1)/2);
                shadowPaint.Color = Android.Graphics.Color.Black;
                shadowPaint.StrokeWidth = shadowR;
                shadowPaint.Alpha = (int)(255 * 0.05 / shadowR);
                shadowPaint.SetStyle(Paint.Style.Stroke);
                for (int i = 0; i <= 2 * shadowR + 2 * Display.Scale; i++)
                {
                    /*
                    var r = Math.Max(0, RoundedBoxElement.OutlineRadius);// - shadowR);
                    if (RoundedBoxElement.IsElliptical)
                        shadowCanvas.DrawPath(Ellipse(insetShadowBounds), shadowPaint);
                    else
                        shadowCanvas.DrawPath(PerimeterPath(_element, insetShadowBounds, r), shadowPaint);
                        */
                    shadowCanvas.DrawPath(RoundedBoxPath(insetShadowBounds, 0), shadowPaint);
                    insetShadowBounds.Inset(-1, -1);
                    //p.StrokeWidth -= 1;
                    //if (p.StrokeWidth < 0)
                    //  p.StrokeWidth = 0;
                    //shadowPaint.Alpha += (int)(255 * (0.02 * (i + 1) / shadowR));
                    shadowPaint.Alpha += (int)(shadowR / 4.0);
                    //System.Diagnostics.Debug.WriteLine("Alpha=[" + shadowPaint.Alpha + "] shadowR=[" + shadowR + "]");
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


                shadowBitmap.Dispose();
                maskBitmap.Dispose();
                shadowPaint.Dispose();
                maskPaint.Dispose();
                resultPaint.Dispose();
                resultCanvas.Dispose();
                shadowCanvas.Dispose();
                maskCanvas.Dispose();
            }

            GC.Collect();

            /*
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


            */
        }

        CGPath RoundedBoxPath(RectF perimeter, RoundRectPath pathType)
        {
            var roundedBox = RoundedBoxElement;
            var offset = roundedBox.OutlineWidth * Display.Scale;
            if (pathType == RoundRectPath.Outline)
                offset /= 2;
            bool drawOutline = roundedBox.OutlineWidth > 0.05 && roundedBox.OutlineColor.A > 0.01;
            CGPath result = default(CGPath);
            RectF boundary = default(RectF);
            var materialButton = roundedBox as MaterialButton;
            var radius = roundedBox.OutlineRadius * Display.Scale;
            var hz = materialButton == null || materialButton.ParentSegmentsOrientation == StackOrientation.Horizontal;
            var vt = !hz;
            var segmentType = materialButton == null ? ButtonShape.Rectangle : materialButton.SegmentType;
            switch (segmentType)
            {
                case ButtonShape.Rectangle:
                    boundary = RectInset(perimeter, offset);
                    break;
                case ButtonShape.SegmentStart:
                    boundary = RectInset(perimeter, offset, offset, vt ? offset : 0, hz ? offset : 0);
                    break;
                case ButtonShape.SegmentMid:
                    boundary = RectInset(perimeter, offset, offset, vt ? offset : 0, hz ? offset : 0);
                    break;
                case ButtonShape.SegmentEnd:
                    boundary = RectInset(perimeter, offset);
                    break;
            }
            if (segmentType == ButtonShape.Rectangle)
            {
                if (roundedBox.IsElliptical)
                    result = Ellipse(boundary);
                else
                    result = RectangularPerimeterPath(boundary, radius - (drawOutline ? offset : 0));
            }
            else
                result = RectangularPerimeterPath(boundary, radius - (drawOutline ? offset : 0));
            return result;
        }

        RectF RectInset(RectF rect, float inset)
        {
            return RectInset(rect, inset, inset, inset, inset);
        }

        RectF RectInset(RectF rect, float left, float top, float right, float bottom)
        {
            return new RectF(rect.Left + left, rect.Top + top, rect.Right - right, rect.Bottom - bottom);
        }

        internal static CGPath PerimeterPath(IRoundedBox element, RectF rect, float radius, bool counterClockWise = true)
        {
            var materialButton = element as MaterialButton;
            ButtonShape type = materialButton == null ? ButtonShape.Rectangle : materialButton.SegmentType;
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
                if (type == ButtonShape.SegmentStart || type == ButtonShape.Rectangle)
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
                if (type == ButtonShape.SegmentStart && orientation == StackOrientation.Horizontal || type == ButtonShape.SegmentEnd && orientation == StackOrientation.Vertical || type == ButtonShape.Rectangle)
                {
                    //result.LineTo (rect.Left, rect.Bottom - radius);
                    //result.AddRelativeArc(rect.Left + radius, rect.Bottom - radius, radius, (nfloat)(Math.PI), (nfloat)(-Math.PI / 2.0));
                    result.ArcTo(bottomLeft, 180, -90);
                }
                else
                    result.LineTo(rect.Left, rect.Bottom);

                // bottom right
                if (type == ButtonShape.SegmentEnd || type == ButtonShape.Rectangle)
                {
                    //result.LineTo (rect.Right - radius, rect.Bottom);
                    //result.AddRelativeArc (rect.Right - radius, rect.Bottom - radius, radius, (nfloat)(Math.PI/2.0), (nfloat)(-Math.PI / 2.0));
                    result.ArcTo(bottomRight, 90, -90);
                }
                else
                    result.LineTo(rect.Right, rect.Bottom);

                // top right
                if (type == ButtonShape.SegmentEnd && orientation == StackOrientation.Horizontal || type == ButtonShape.SegmentStart && orientation == StackOrientation.Vertical || type == ButtonShape.Rectangle)
                {
                    //result.LineTo (rect.Right, rect.Top + radius);					
                    //result.AddArcToPoint (rect.Right, rect.Top - radius, rect.Right - radius, rect.Top, radius);
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
                if (type == ButtonShape.SegmentStart || type == ButtonShape.Rectangle)
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
                if (type == ButtonShape.SegmentEnd || type == ButtonShape.SegmentStart && orientation == StackOrientation.Vertical || type == ButtonShape.Rectangle)
                {
                    //result.LineTo (rect.Right - radius, rect.Top);
                    //result.AddRelativeArc (rect.Right - radius, rect.Top + radius, radius, (nfloat)(3*Math.PI/2.0), (nfloat)(Math.PI / 2.0));
                    result.ArcTo(topRight, 270, 90);
                }
                else
                    result.LineTo(rect.Right, rect.Top);

                // bottom right
                if (type == ButtonShape.SegmentEnd || type == ButtonShape.Rectangle)
                {
                    //result.LineTo (rect.Right, rect.Bottom - radius);
                    //result.AddRelativeArc (rect.Right - radius, rect.Bottom - radius, radius, 0, (nfloat)(Math.PI / 2.0));
                    result.ArcTo(bottomRight, 0, 90);
                }
                else
                    result.LineTo(rect.Right, rect.Bottom);

                // bottom left
                if (type == ButtonShape.SegmentStart || type == ButtonShape.SegmentEnd && orientation == StackOrientation.Vertical || type == ButtonShape.Rectangle)
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

        CGPath Ellipse(RectF rect, bool counterClockWise = true)
        {
            var path = new CGPath();
            path.AddOval(rect, counterClockWise ? Path.Direction.Ccw : Path.Direction.Cw);
            return path;
        }

        CGPath RectangularPerimeterPath(RectF rect, float radius)
        {
            var element = RoundedBoxElement;
            radius = Math.Max(radius, 0);


            var materialButton = element as MaterialButton;
            ButtonShape type = materialButton == null ? ButtonShape.Rectangle : materialButton.SegmentType;
            StackOrientation orientation = materialButton == null ? StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

            var diameter = radius * 2;
            var topLeft = new RectF(rect.Left, rect.Top, rect.Left + diameter, rect.Top + diameter);
            var bottomLeft = new RectF(rect.Left, rect.Bottom - diameter, rect.Left + diameter, rect.Bottom);
            var bottomRight = new RectF(rect.Right - diameter, rect.Bottom - diameter, rect.Right, rect.Bottom);
            var topRight = new RectF(rect.Right - diameter, rect.Top, rect.Right, rect.Top + diameter);

            var pathFigure = new CGPath();

            if (type == ButtonShape.Rectangle)
            {
                pathFigure.MoveToPoint((rect.Left + rect.Right) / 2, rect.Top);
                pathFigure.AddLineToPoint(rect.Left + radius, rect.Top);
                if (radius > 0)
                    pathFigure.ArcTo(topLeft, 270, -90);
                pathFigure.AddLineToPoint(rect.Left, rect.Bottom - radius);
                if (radius > 0)
                    pathFigure.ArcTo(bottomLeft, 180, -90);
                pathFigure.AddLineToPoint(rect.Right - radius, rect.Bottom);
                if (radius > 0)
                    pathFigure.ArcTo(bottomRight, 90, -90);
                pathFigure.AddLineToPoint(rect.Right, rect.Top + radius);
                if (radius > 0)
                    pathFigure.ArcTo(topRight, 0, -90);
                pathFigure.AddLineToPoint((rect.Left + rect.Right) / 2, rect.Top);
            }
            else if (type == ButtonShape.SegmentStart)
            {
                if (orientation == StackOrientation.Horizontal)
                {
                    pathFigure.MoveToPoint(rect.Right, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left + radius, rect.Top);
                    if (radius > 0)
                        pathFigure.ArcTo(topLeft, 270, -90);
                    pathFigure.AddLineToPoint(rect.Left, rect.Bottom - radius);
                    if (radius > 0)
                        pathFigure.ArcTo(bottomLeft, 180, -90);
                    pathFigure.AddLineToPoint(rect.Right, rect.Bottom);
                }
                else
                {
                    pathFigure.MoveToPoint(rect.Right, rect.Bottom);
                    pathFigure.AddLineToPoint(rect.Right, rect.Top + radius);
                    if (radius > 0)
                        pathFigure.ArcTo(topRight, 0, -90);
                    pathFigure.AddLineToPoint(rect.Left + radius, rect.Top);
                    if (radius > 0)
                        pathFigure.ArcTo(topLeft, 270, -90);
                    pathFigure.AddLineToPoint(rect.Left, rect.Bottom);
                }
            }
            else if (type == ButtonShape.SegmentMid)
            {
                if (orientation == StackOrientation.Horizontal)
                {
                    pathFigure.MoveToPoint(rect.Right, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Bottom);
                    pathFigure.AddLineToPoint(rect.Right, rect.Bottom);
                }
                else
                {
                    pathFigure.MoveToPoint(rect.Right, rect.Bottom);
                    pathFigure.AddLineToPoint(rect.Right, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Bottom);
                }
            }
            else if (type == ButtonShape.SegmentEnd)
            {
                if (orientation == StackOrientation.Horizontal)
                {

                    pathFigure.MoveToPoint((rect.Left + rect.Right) / 2, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Bottom);
                    pathFigure.AddLineToPoint(rect.Right - radius, rect.Bottom);
                    if (radius > 0)
                        pathFigure.ArcTo(bottomRight, 90, -90);
                    pathFigure.AddLineToPoint(rect.Right, rect.Top + radius);
                    if (radius > 0)
                        pathFigure.ArcTo(topRight, 0, -90);
                    pathFigure.AddLineToPoint((rect.Left + rect.Right) / 2, rect.Top);
                }
                else
                {

                    pathFigure.MoveToPoint((rect.Left + rect.Right) / 2, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Bottom - radius);
                    if (radius > 0)
                        pathFigure.ArcTo(bottomLeft, 180, -90);
                    pathFigure.AddLineToPoint(rect.Right - radius, rect.Bottom);
                    if (radius > 0)
                        pathFigure.ArcTo(bottomRight, 90, -90);
                    pathFigure.AddLineToPoint(rect.Right, rect.Top);
                    pathFigure.AddLineToPoint((rect.Left + rect.Right) / 2, rect.Top);
                }
            }

            return pathFigure;
        }
        #endregion
    }
}

