using System.ComponentModel;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System;

namespace Forms9Patch.iOS
{
    /// <summary>
    /// Forms9Patch Layout renderer.
    /// </summary>
    public class LayoutRenderer<TElement> : VisualElementRenderer<TElement> where TElement : View, IBackgroundImage // VisualElement, IBackgroundImage
    {
        Image _oldImage;
        ImageViewManager _imageViewManager;

        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<TElement> e)
        {
            if (e.OldElement != null)
            {
                _imageViewManager.LayoutComplete -= OnBackgroundImageLayoutComplete;
                _imageViewManager.Dispose();
                _imageViewManager = null;
            }
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (_imageViewManager == null)
                {
                    _imageViewManager = new ImageViewManager(this, e.NewElement);
                    _imageViewManager.LayoutComplete += OnBackgroundImageLayoutComplete;
                }
                _imageViewManager.Element = e.NewElement;
                _oldImage = e.NewElement.BackgroundImage;
#pragma warning disable 4014
                _imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
            }
            else if (_imageViewManager != null)
            {
                _imageViewManager.LayoutComplete -= OnBackgroundImageLayoutComplete;
                _imageViewManager.Dispose();
                _imageViewManager = null;
            }
            if (e.NewElement is IRoundedBox)
                BackgroundColor = Color.Transparent.ToUIColor();
        }

        /// <summary>
        /// Raises the element property changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (
                e.PropertyName == RoundedBoxBase.OutlineColorProperty.PropertyName
                || e.PropertyName == RoundedBoxBase.ShadowInvertedProperty.PropertyName
                || e.PropertyName == RoundedBoxBase.OutlineRadiusProperty.PropertyName
                || e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName
                //) {
                //_imageViewManager.LayoutImage (_oldImage);
                //} else if (
                || e.PropertyName == VisualElement.WidthProperty.PropertyName
                || e.PropertyName == VisualElement.HeightProperty.PropertyName
                || e.PropertyName == VisualElement.XProperty.PropertyName
                || e.PropertyName == VisualElement.YProperty.PropertyName
                || e.PropertyName == RoundedBoxBase.HasShadowProperty.PropertyName
                || e.PropertyName == RoundedBoxBase.OutlineWidthProperty.PropertyName
            )
            {
                SetNeedsDisplay();
            }
            else if (e.PropertyName == ContentView.BackgroundImageProperty.PropertyName)
            {
                if (_oldImage != null)
                    _oldImage.PropertyChanged -= OnBackgroundImagePropertyChanged;
                _oldImage = Element.BackgroundImage;
                if (_oldImage != null)
                    _oldImage.PropertyChanged += OnBackgroundImagePropertyChanged;
#pragma warning disable 4014
                _imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014
            }
            if (Element is IRoundedBox)
                BackgroundColor = Color.Transparent.ToUIColor();
        }



        void OnBackgroundImagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // No Wait or await here because we want RenderBackgroundImage to run in parallel
#pragma warning disable 4014
            _imageViewManager.LayoutImage(_oldImage);
#pragma warning restore 4014


        }

        void OnBackgroundImageLayoutComplete(bool hasImage)
        {
            if (hasImage)
                BackgroundColor = Element.BackgroundColor.ToUIColor();
            SetNeedsDisplay();
        }

        /// <summary>
        /// Draw the specified rect.
        /// </summary>
        /// <param name="rect">Rect.</param>
        public override void Draw(CGRect rect)
        {
            System.Diagnostics.Debug.WriteLine("rect=[" + rect + "]");
            if (_imageViewManager.HasBackgroundImage)
            {
                _imageViewManager.Draw(rect);
                base.Draw(rect);
                return;
            }
            var iRoundedBox = Element as IRoundedBox;
            if (iRoundedBox == null)
            {
                base.Draw(rect);
                return;
            }

            ContentMode = UIViewContentMode.Redraw;

            var backgroundColor = (Color)Element.GetValue(VisualElement.BackgroundColorProperty);
            var outlineColor = (Color)Element.GetValue(RoundedBoxBase.OutlineColorProperty);
            var outlineWidth = (nfloat)(Math.Max(0, (float)Element.GetValue(RoundedBoxBase.OutlineWidthProperty)));
            var isElliptical = (bool)Element.GetValue(RoundedBoxBase.IsEllipticalProperty);

            var materialButton = Element as MaterialButton;
            SegmentType type = materialButton == null ? SegmentType.Not : materialButton.SegmentType;
            StackOrientation orientation = materialButton == null ? StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

            nfloat separatorWidth = materialButton == null || type == SegmentType.Not ?
                0 :
                materialButton.SeparatorWidth < 0 ?
                outlineWidth : (nfloat)Math.Max(0, materialButton.SeparatorWidth);

            if (backgroundColor.A < 0.01 && (outlineColor.A < 0.01 || (outlineWidth < 0.01 && separatorWidth < 0.01)))
            {
                base.Draw(rect);
                return;
            }

            var outlineRadius = Math.Max(0, (float)Element.GetValue(RoundedBoxBase.OutlineRadiusProperty));
            var hasShadow = (bool)Element.GetValue(RoundedBoxBase.HasShadowProperty);
            var shadowInverted = ((bool)Element.GetValue(RoundedBoxBase.ShadowInvertedProperty));


            var hz = orientation == StackOrientation.Horizontal;
            var vt = !hz;
            var makeRoomForShadow = (materialButton == null ? hasShadow : (bool)Element.GetValue(MaterialButton.HasShadowProperty)) && !shadowInverted;

            var shadowX = (nfloat)Forms9Patch.Settings.ShadowOffset.X;//* Display.Scale);
            var shadowY = (nfloat)Forms9Patch.Settings.ShadowOffset.Y;// * Display.Scale);
            var shadowR = (nfloat)Forms9Patch.Settings.ShadowRadius;// * Display.Scale);
            using (var g = UIGraphics.GetCurrentContext())
            {
                g.SaveState();
                var shadowColor = Color.FromRgba(0.0, 0.0, 0.0, 0.55).ToCGColor();
                CGRect perimeter = rect;
                if (makeRoomForShadow)
                {

                    // what additional padding was allocated to cast the button's shadow?
                    var shadowPad = Forms9Patch.RoundedBoxBase.ShadowPadding(Element as Layout);
                    // shrink the button's perimeter by that extra padding (so it is the size it was originally intended to be)
                    perimeter = new CGRect(rect.Left + shadowPad.Left, rect.Top + shadowPad.Top, rect.Width - shadowPad.HorizontalThickness, rect.Height - shadowPad.VerticalThickness);
                    // setup for a "raised" shadow
                    if (!shadowInverted && hasShadow && backgroundColor.A > 0)
                    {
                        if (type != SegmentType.Not)
                        {
                            // if it is a segment, cast the shadow beyond the button's parimeter and clip it (so no overlaps or gaps)
                            //var clipRect = SegmentAllowanceRect (perimeter, 20, orientation, type);
                            double allowance = Math.Abs(shadowX) + Math.Abs(shadowY) + Math.Abs(shadowR);
                            CGRect result;
                            if (type == SegmentType.Start)
                                result = new CGRect(perimeter.Left - allowance, perimeter.Top - allowance, perimeter.Width + allowance * (hz ? 1 : 2), perimeter.Height + allowance * (vt ? 1 : 2));
                            else if (type == SegmentType.Mid)
                                result = new CGRect(perimeter.Left - (hz ? 0 : allowance), perimeter.Top - (vt ? 0 : allowance), perimeter.Width + (hz ? 0 : 2 * allowance), perimeter.Height + (vt ? 0 : 2 * allowance));
                            else
                                result = new CGRect(perimeter.Left - (hz ? 0 : allowance), perimeter.Top - (vt ? 0 : allowance), perimeter.Width + allowance * (hz ? 1 : 2), perimeter.Height + allowance * (vt ? 1 : 2));
                            //Console.WriteLine ("clipRect:["+result.Left+", "+result.Top+", "+result.Width+", "+result.Height+"]");
                            var clipPath = CGPath.FromRect(result);
                            g.AddPath(clipPath);
                            g.Clip();
                        }
                        g.SetShadow(new CGSize(shadowX, shadowY), shadowR, shadowColor);
                    }
                }


                // generate background
                CGPath perimeterPath;
                if (backgroundColor.A > 0)
                {
                    g.SetFillColor(backgroundColor.ToCGColor());
                    if (type == SegmentType.Not)
                    {
                        if (isElliptical)
                            perimeterPath = CGPath.EllipseFromRect(perimeter);
                        else
                            perimeterPath = RectangularPerimeterPath(iRoundedBox, perimeter, (float)(outlineRadius + (outlineColor.A > 0 ? outlineWidth / 2.0f : 0)));
                        //Console.WriteLine ("periPath: [" + perimeter.Left + ", " + perimeter.Top + ", " + perimeter.Width + ", " + perimeter.Height + "]");
                    }
                    else
                    {
                        // make the button bigger on the overlap sides so the mask can trim off excess, including shadow
                        CGRect newPerimenter = SegmentAllowanceRect(perimeter, 20, orientation, type);
                        perimeterPath = RectangularPerimeterPath(iRoundedBox, newPerimenter, (float)(outlineRadius + (outlineColor.A > 0 ? outlineWidth / 2.0f : 0)));
                        //Console.WriteLine ("periPath: ["+newPerimenter.Left+", "+newPerimenter.Top+", "+newPerimenter.Width+", "+newPerimenter.Height+"]");
                    }
                    g.AddPath(perimeterPath);
                    g.FillPath();
                }


                // shrink the perimeter by the outlinewidth so it's not clipped by the view's bounds
                if (outlineWidth > 0)
                {
                    perimeter = perimeter.Inset(outlineWidth / 2.0f, outlineWidth / 2.0f);
                    //Console.WriteLine ("perimeter:["+perimeter.Left+", "+perimeter.Top+", "+perimeter.Width+", "+perimeter.Height+"]");
                }


                // outline
                if (outlineWidth > 0 && outlineColor.A > 0)
                {
                    g.SetShadow(new CGSize(0, 0), 0, Color.Transparent.ToCGColor());
                    g.SetLineWidth(outlineWidth);
                    g.SetStrokeColor(outlineColor.ToCGColor());
                    CGPath outlinePath;
                    if (isElliptical)
                        outlinePath = CGPath.EllipseFromRect(perimeter);
                    else
                        outlinePath = OutlinePath(iRoundedBox, perimeter, outlineRadius, outlineWidth);
                    g.AddPath(outlinePath);
                    g.StrokePath();
                }

                // separators
                if (separatorWidth > 0 && !isElliptical)
                {
                    g.SetShadow(new CGSize(0, 0), 0, Color.Transparent.ToCGColor());
                    nfloat inset = outlineColor.A > 0 ? outlineWidth / 2.0f : 0;
                    g.SetStrokeColor(outlineColor.ToCGColor());
                    g.SetLineWidth(separatorWidth);
                    if (type == SegmentType.Start || type == SegmentType.Mid)
                    {
                        if (hz)
                        {
                            //g.MoveTo (perimeter.Right, perimeter.Top + inset);
                            //g.AddLineToPoint (perimeter.Right, perimeter.Bottom - inset);
                            g.MoveTo((nfloat)Math.Ceiling(perimeter.Right), perimeter.Top + inset);
                            g.AddLineToPoint((nfloat)Math.Ceiling(perimeter.Right), perimeter.Bottom - inset);
                        }
                        else
                        {
                            g.MoveTo(perimeter.Left + inset, (nfloat)Math.Ceiling(perimeter.Bottom));
                            g.AddLineToPoint(perimeter.Right - inset, (nfloat)Math.Ceiling(perimeter.Bottom));
                        }
                    }
                    if (type == SegmentType.Mid || type == SegmentType.End)
                    {
                        if (hz)
                        {
                            g.MoveTo((nfloat)Math.Round(perimeter.Left), perimeter.Top + inset);
                            g.AddLineToPoint((nfloat)Math.Round(perimeter.Left), perimeter.Bottom - inset);
                        }
                        else
                        {
                            g.MoveTo(perimeter.Left + inset, (nfloat)Math.Round(perimeter.Top));
                            g.AddLineToPoint(perimeter.Right - inset, (nfloat)Math.Round(perimeter.Top));
                        }
                    }

                    g.StrokePath();
                }

                // interior shadow
                if (hasShadow && backgroundColor.A > 0 && shadowInverted)
                {
                    var insetShadowPath = CGPath.FromRect(rect.Inset(-40, -40));
                    CGRect newPerimenter = SegmentAllowanceRect(perimeter, 20, orientation, type);
                    if (isElliptical)
                        perimeterPath = CGPath.EllipseFromRect(newPerimenter);
                    else
                        perimeterPath = RectangularPerimeterPath(iRoundedBox, newPerimenter, outlineRadius);
                    insetShadowPath.AddPath(perimeterPath);
                    insetShadowPath.CloseSubpath();
                    g.AddPath(perimeterPath);
                    g.Clip();
                    shadowColor = Color.FromRgba(0.0, 0.0, 0.0, 0.75).ToCGColor();
                    g.SetShadow(new CGSize(shadowX, shadowY), shadowR, shadowColor);
                    g.SetFillColor(shadowColor);
                    g.SaveState();
                    g.AddPath(insetShadowPath);
                    g.EOFillPath();
                    g.RestoreState();
                }

                g.RestoreState();
            }
            base.Draw(rect);
        }

        static CGRect SegmentAllowanceRect(CGRect rect, double allowance, StackOrientation orientation, SegmentType type)
        {
            CGRect result;
            switch (type)
            {
                case SegmentType.Start:
                    result = new CGRect(rect.Left, rect.Top, rect.Width + (orientation == StackOrientation.Horizontal ? allowance : 0), rect.Height + (orientation == StackOrientation.Vertical ? allowance : 0));
                    break;
                case SegmentType.Mid:
                    result = new CGRect(rect.Left - (orientation == StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == StackOrientation.Vertical ? allowance : 0), rect.Width + (orientation == StackOrientation.Horizontal ? allowance * 2 : 0), rect.Height + (orientation == StackOrientation.Vertical ? allowance * 2 : 0));
                    break;
                case SegmentType.End:
                    result = new CGRect(rect.Left - (orientation == StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == StackOrientation.Vertical ? allowance : 0), rect.Width + (orientation == StackOrientation.Horizontal ? allowance : 0), rect.Height + (orientation == StackOrientation.Vertical ? allowance : 0));
                    break;
                default:
                    result = rect;
                    break;
            }
            System.Diagnostics.Debug.WriteLine("SegmentAllowanceRect result=[" + result + "]");
            return result;
        }

        static CGRect RectInset(CGRect rect, double left, double top, double right, double bottom)
        {
            return new CGRect(rect.X + left, rect.Y + top, rect.Width - left - right, rect.Height - top - bottom);
        }

        internal static CGPath RectangularPerimeterPath(IRoundedBox element, CGRect rect, float radius, bool counterClockWise = true)
        {
            var materialButton = element as MaterialButton;
            SegmentType type = materialButton == null ? SegmentType.Not : materialButton.SegmentType;
            StackOrientation orientation = materialButton == null ? StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

            //var diameter = radius * 2;
            //var topLeft = new CGRect (rect.Left, rect.Top, diameter, diameter);
            //var bottomLeft = new CGRect (rect.Left, rect.Bottom - diameter, diameter, rect.Bottom);
            //var bottomRight = new CGRect (rect.Right - diameter, rect.Bottom - diameter, diameter, diameter);
            //var topRight = new CGRect (rect.Right - diameter, rect.Top, diameter, diameter);

            var result = new CGPath();

            /*
			var startX = rect.Left;
			var startY = rect.Top;
			if (type == SegmentType.Start || type == SegmentType.Not) {
				if (counterClockWise)
					startX += radius;
				else
					startY -= radius;
			}
			//result.MoveToPoint (startX, startY);
			*/

            if (counterClockWise)
            {
                // top center
                result.MoveToPoint((rect.Left + rect.Right) / 2.0f, rect.Top);

                // topLeft
                if (type == SegmentType.Start || type == SegmentType.Not)
                {
                    result.AddLineToPoint(rect.Left + radius, rect.Top);
                    result.AddRelativeArc(rect.Left + radius, rect.Top + radius, radius, (nfloat)(3 * Math.PI / 2.0), (nfloat)(-Math.PI / 2.0));
                }
                else
                {
                    result.AddLineToPoint(rect.Left, rect.Top);
                }

                // bottom left
                if (type == SegmentType.Start && orientation == StackOrientation.Horizontal || type == SegmentType.End && orientation == StackOrientation.Vertical || type == SegmentType.Not)
                {
                    result.AddLineToPoint(rect.Left, rect.Bottom - radius);
                    result.AddRelativeArc(rect.Left + radius, rect.Bottom - radius, radius, (nfloat)(Math.PI), (nfloat)(-Math.PI / 2.0));
                }
                else
                    result.AddLineToPoint(rect.Left, rect.Bottom);

                // bottom right
                if (type == SegmentType.End || type == SegmentType.Not)
                {
                    result.AddLineToPoint(rect.Right - radius, rect.Bottom);
                    result.AddRelativeArc(rect.Right - radius, rect.Bottom - radius, radius, (nfloat)(Math.PI / 2.0), (nfloat)(-Math.PI / 2.0));
                }
                else
                    result.AddLineToPoint(rect.Right, rect.Bottom);

                // top right
                if (type == SegmentType.End && orientation == StackOrientation.Horizontal || type == SegmentType.Start && orientation == StackOrientation.Vertical || type == SegmentType.Not)
                {
                    result.AddLineToPoint(rect.Right, rect.Top + radius);                   //result.AddArcToPoint (rect.Right, rect.Top - radius, rect.Right - radius, rect.Top, radius);
                    result.AddRelativeArc(rect.Right - radius, rect.Top + radius, radius, 0, (nfloat)(-Math.PI / 2.0));
                }
                else
                    result.AddLineToPoint(rect.Right, rect.Top);

                // finish
                result.AddLineToPoint((rect.Left + rect.Right) / 2.0f, rect.Top);
            }
            else
            {
                // leftCenter
                result.MoveToPoint(rect.Left, (rect.Top + rect.Bottom) / 2.0f);

                // topLeft
                if (type == SegmentType.Start || type == SegmentType.Not)
                {
                    result.AddLineToPoint(rect.Left, rect.Top + radius);
                    result.AddRelativeArc(rect.Left + radius, rect.Top + radius, radius, (nfloat)(Math.PI), (nfloat)(Math.PI / 2.0));
                }
                else
                {
                    result.AddLineToPoint(rect.Left, rect.Top);
                }

                // top right
                if (type == SegmentType.End || type == SegmentType.Start && orientation == StackOrientation.Vertical || type == SegmentType.Not)
                {
                    result.AddLineToPoint(rect.Right - radius, rect.Top);
                    result.AddRelativeArc(rect.Right - radius, rect.Top + radius, radius, (nfloat)(3 * Math.PI / 2.0), (nfloat)(Math.PI / 2.0));
                }
                else
                    result.AddLineToPoint(rect.Right, rect.Top);

                // bottom right
                if (type == SegmentType.End || type == SegmentType.Not)
                {
                    result.AddLineToPoint(rect.Right, rect.Bottom - radius);
                    result.AddRelativeArc(rect.Right - radius, rect.Bottom - radius, radius, 0, (nfloat)(Math.PI / 2.0));
                }
                else
                    result.AddLineToPoint(rect.Right, rect.Bottom);

                // bottom left
                if (type == SegmentType.Start || type == SegmentType.End && orientation == StackOrientation.Vertical || type == SegmentType.Not)
                {
                    result.AddLineToPoint(rect.Left + radius, rect.Bottom);
                    result.AddRelativeArc(rect.Left + radius, rect.Bottom - radius, radius, (nfloat)(Math.PI / 2.0), (nfloat)(Math.PI / 2.0));
                }
                else
                    result.AddLineToPoint(rect.Left, rect.Bottom);

                //finish
                result.AddLineToPoint(rect.Left, (rect.Top + rect.Bottom) / 2.0f);
            }

            // finish
            //result.AddLineToPoint(startX, startY);
            result.AddLineToPoint((rect.Left + rect.Right) / 2.0f, rect.Top);


            return result;
        }

        static CGPath OutlinePath(IRoundedBox element, CGRect rect, float radius, nfloat lineWidth)
        {

            //System.Diagnostics.Debug.WriteLine("OutlinePath(rect: " + rect + ", radius: " + radius + ", lineWidth: " + lineWidth + ")");

            lineWidth /= 2.0f;
            var materialButton = element as MaterialButton;
            SegmentType type = materialButton == null ? SegmentType.Not : materialButton.SegmentType;
            StackOrientation orientation = materialButton == null ? StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

            if (type == SegmentType.Not)
                return RectangularPerimeterPath(element, rect, radius);

            //var diameter = radius * 2;
            //var topLeft = new RectF (rect.Left, rect.Top, rect.Left + diameter, rect.Top + diameter);
            //var bottomLeft = new RectF (rect.Left, rect.Bottom - diameter, rect.Left + diameter, rect.Bottom);
            //var bottomRight = new RectF (rect.Right - diameter, rect.Bottom - diameter, rect.Right, rect.Bottom);
            //var topRight = new RectF (rect.Right - diameter, rect.Top, rect.Right, rect.Top + diameter);

            var result = new CGPath();

            if (orientation == StackOrientation.Horizontal)
            {
                if (type == SegmentType.Start)
                {
                    result.MoveToPoint(rect.Right + lineWidth, rect.Bottom);
                    result.AddLineToPoint(rect.Left + radius, rect.Bottom);
                    result.AddRelativeArc(rect.Left + radius, rect.Bottom - radius, radius, (nfloat)(1 * Math.PI / 2.0), (nfloat)(Math.PI / 2.0));
                    result.AddLineToPoint(rect.Left, rect.Top + radius);
                    result.AddRelativeArc(rect.Left + radius, rect.Top + radius, radius, (nfloat)(2 * Math.PI / 2.0), (nfloat)(Math.PI / 2.0));
                    result.AddLineToPoint(rect.Right + lineWidth, rect.Top);
                }
                else if (type == SegmentType.Mid)
                { // mid
                    result.MoveToPoint(rect.Right + lineWidth, rect.Bottom);
                    result.AddLineToPoint(rect.Left - lineWidth, rect.Bottom);
                    result.MoveToPoint(rect.Left - lineWidth, rect.Top);
                    result.AddLineToPoint(rect.Right + lineWidth, rect.Top);
                }
                else
                { // end
                    result.MoveToPoint(rect.Left - lineWidth, rect.Top);
                    result.AddLineToPoint(rect.Right - radius, rect.Top);
                    result.AddRelativeArc(rect.Right - radius, rect.Top + radius, radius, (nfloat)(3 * Math.PI / 2.0), (nfloat)(Math.PI / 2.0));
                    result.AddLineToPoint(rect.Right, rect.Bottom - radius);
                    result.AddRelativeArc(rect.Right - radius, rect.Bottom - radius, radius, 0, (nfloat)(Math.PI / 2.0));
                    result.AddLineToPoint(rect.Left - lineWidth, rect.Bottom);
                }
            }
            else
            { // vertical
                if (type == SegmentType.Start)
                {
                    result.MoveToPoint(rect.Right, rect.Bottom + lineWidth);
                    result.AddLineToPoint(rect.Right, rect.Top + radius);
                    result.AddRelativeArc(rect.Right - radius, rect.Top + radius, radius, 0, (nfloat)(-Math.PI / 2.0));
                    result.AddLineToPoint(rect.Left + radius, rect.Top);
                    result.AddRelativeArc(rect.Left + radius, rect.Top + radius, radius, (nfloat)(3 * Math.PI / 2.0), (nfloat)(-Math.PI / 2.0));
                    result.AddLineToPoint(rect.Left, rect.Bottom + lineWidth);
                }
                else if (type == SegmentType.Mid)
                {
                    result.MoveToPoint(rect.Right, rect.Bottom + lineWidth);
                    result.AddLineToPoint(rect.Right, rect.Top - lineWidth);
                    result.MoveToPoint(rect.Left, rect.Top - lineWidth);
                    result.AddLineToPoint(rect.Left, rect.Bottom + lineWidth);
                }
                else
                { // end
                    result.MoveToPoint(rect.Left, rect.Top - lineWidth);
                    result.AddLineToPoint(rect.Left, rect.Bottom - radius);
                    result.AddRelativeArc(rect.Left + radius, rect.Bottom - radius, radius, (nfloat)(2 * Math.PI / 2.0), (nfloat)(-Math.PI / 2.0));
                    result.AddLineToPoint(rect.Right - radius, rect.Bottom);
                    result.AddRelativeArc(rect.Right - radius, rect.Bottom - radius, radius, (nfloat)(1 * Math.PI / 2.0), (nfloat)(-Math.PI / 2.0));
                    result.AddLineToPoint(rect.Right, rect.Top - lineWidth);
                }
            }
            return result;
        }
    }
}