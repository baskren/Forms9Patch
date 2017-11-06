using System.ComponentModel;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System;
using Forms9Patch.Enums;

namespace Forms9Patch.iOS
{
    /// <summary>
    /// Forms9Patch Layout renderer.
    /// </summary>
    public class LayoutRenderer<TElement> : VisualElementRenderer<TElement> where TElement : View, IBackgroundImage // VisualElement, IBackgroundImage
    {
        #region Element Fields
        Image _oldImage;
        ImageViewManager _imageViewManager;
        #endregion


        #region Change listeners
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
        #endregion


        #region Layout Fields
        bool _drawOutline;
        bool _drawFill;
        bool _hz, _vt;
        ButtonShape _segmentType;
        IRoundedBox _roundedBoxElement;
        #endregion


        #region Layout
        /// <summary>
        /// Draw the specified rect.
        /// </summary>
        /// <param name="rect">Rect.</param>
        public override void Draw(CGRect rect)
        {
            //System.Diagnostics.Debug.WriteLine("rect=[" + rect + "]");

            if (_imageViewManager.HasBackgroundImage)
            {
                _imageViewManager.Draw(rect);
                base.Draw(rect);
                return;
            }

            if (Element is MaterialSegmentedControl)
            {
                base.Draw(rect);
                return;
            }

            _roundedBoxElement = Element as IRoundedBox;
            if (_roundedBoxElement == null)
            {
                base.Draw(rect);
                return;
            }

            ContentMode = UIViewContentMode.Redraw;

            _drawOutline = _roundedBoxElement.OutlineWidth > 0.05 && _roundedBoxElement.OutlineColor.A > 0.01;
            _drawFill = _roundedBoxElement.BackgroundColor.A > 0.01;

            var visualElement = _roundedBoxElement as VisualElement;

            if (rect.Width <= 0 || rect.Height <= 0)
            {
                base.Draw(rect);
                return;
            }


            _segmentType = ButtonShape.Rectangle;
            _hz = true;
            if (_roundedBoxElement is MaterialButton materialButton)
            {
                _segmentType = materialButton.SegmentType;
                _hz = materialButton.ParentSegmentsOrientation == StackOrientation.Horizontal;
            }
            else
                materialButton = null;
            _vt = !_hz;

            nfloat separatorWidth = materialButton == null || _segmentType == ButtonShape.Rectangle ? 0 : materialButton.SeparatorWidth < 0 ? _roundedBoxElement.OutlineWidth : Math.Max(0, materialButton.SeparatorWidth);
            if (_roundedBoxElement.BackgroundColor.A < 0.01 && (_roundedBoxElement.OutlineColor.A < 0.01 || (_roundedBoxElement.OutlineWidth < 0.01 && separatorWidth < 0.01)))
            {
                base.Draw(rect);
                return;
            }

            var makeRoomForShadow = _roundedBoxElement.HasShadow && _roundedBoxElement.BackgroundColor.A > 0.01;// && !RoundedBoxElement.ShadowInverted;
            var shadowX = (float)Forms9Patch.Settings.ShadowOffset.X * Display.Scale;
            var shadowY = (float)Forms9Patch.Settings.ShadowOffset.Y * Display.Scale;
            var shadowR = (float)Forms9Patch.Settings.ShadowRadius * Display.Scale;
            var shadowColor = Color.FromRgba(0.0, 0.0, 0.0, 0.55).ToCGColor();
            var shadowPadding = RoundedBoxBase.ShadowPadding(Element as Layout);

            var perimeter = rect;

            using (var g = UIGraphics.GetCurrentContext())
            {
                g.SaveState();


                if (makeRoomForShadow)
                {
                    perimeter = new CGRect(rect.Left + shadowPadding.Left, rect.Top + shadowPadding.Top, rect.Width - shadowPadding.HorizontalThickness, rect.Height - shadowPadding.VerticalThickness);
                    if (!_roundedBoxElement.ShadowInverted)
                    {
                        if (_segmentType != ButtonShape.Rectangle)
                        {
                            // it is a segment, cast the shadow beyond the button's parimeter and clip it (so no overlaps or gaps)
                            double allowance = Math.Abs(shadowX) + Math.Abs(shadowY) + Math.Abs(shadowR);
                            CGRect result;
                            if (_segmentType == ButtonShape.SegmentStart)
                                result = new CGRect(perimeter.Left - allowance, perimeter.Top - allowance, perimeter.Width + allowance * (_hz ? 1 : 2), perimeter.Height + allowance * (_vt ? 1 : 2));
                            else if (_segmentType == ButtonShape.SegmentMid)
                                result = new CGRect(perimeter.Left - (_hz ? 0 : allowance), perimeter.Top - (_vt ? 0 : allowance), perimeter.Width + (_hz ? 0 : 2 * allowance), perimeter.Height + (_vt ? 0 : 2 * allowance));
                            else
                                result = new CGRect(perimeter.Left - (_hz ? 0 : allowance), perimeter.Top - (_vt ? 0 : allowance), perimeter.Width + allowance * (_hz ? 1 : 2), perimeter.Height + allowance * (_vt ? 1 : 2));
                            //Console.WriteLine ("clipRect:["+result.Left+", "+result.Top+", "+result.Width+", "+result.Height+"]");
                            var clipPath = CGPath.FromRect(result);
                            g.AddPath(clipPath);
                            g.Clip();
                        }
                        g.SetShadow(new CGSize(shadowX, shadowY), shadowR, shadowColor);
                    }
                }

                // generate background
                if (_drawFill)
                {
                    var fillPerimeter = makeRoomForShadow ? SegmentBoundaryEnlarge(perimeter) : perimeter;

                    CGPath fillPath = RoundedBoxPath(fillPerimeter, RoundRectPath.Fill);
                    g.SetFillColor(_roundedBoxElement.BackgroundColor.ToCGColor());
                    g.AddPath(fillPath);
                    g.FillPath();
                }

                if (_drawOutline)
                {
                    CGPath outlinePath = RoundedBoxPath(perimeter, RoundRectPath.Outline);
                    g.SetLineWidth(_roundedBoxElement.OutlineWidth);
                    g.SetStrokeColor(_roundedBoxElement.OutlineColor.ToCGColor());
                    g.AddPath(outlinePath);
                    g.StrokePath();
                }

                // separators
                if (materialButton!=null && _roundedBoxElement.OutlineWidth < 0.05 && separatorWidth > 0 && !_roundedBoxElement.IsElliptical)
                {
                    g.SetShadow(new CGSize(0, 0), 0, Color.Transparent.ToCGColor());
         
                    nfloat inset = materialButton.OutlineColor.A > 0 ? separatorWidth / 2.0f : 0;
                    g.SetStrokeColor(materialButton.OutlineColor.ToCGColor());
                    g.SetLineWidth(separatorWidth);
                    if (_segmentType == ButtonShape.SegmentStart || _segmentType == ButtonShape.SegmentMid)
                    {
                        if (_hz)
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
                    if (_segmentType == ButtonShape.SegmentMid || _segmentType == ButtonShape.SegmentEnd)
                    {
                        if (_hz)
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
                if (_roundedBoxElement.HasShadow && _roundedBoxElement.BackgroundColor.A > 0 && _roundedBoxElement.ShadowInverted)
                {
                    var orientation = materialButton.ParentSegmentsOrientation;

                    var insetShadowPath = CGPath.FromRect(rect.Inset(-40, -40));
                    CGRect newPerimenter = SegmentAllowanceRect(perimeter, 20);
                    var interiorShadowPath = RoundedBoxPath(newPerimenter, 0);
                    insetShadowPath.AddPath(interiorShadowPath);
                    insetShadowPath.CloseSubpath();
                    g.AddPath(interiorShadowPath);
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

        CGRect SegmentBoundaryEnlarge(CGRect rect)
        {
            switch (_segmentType)
            {
                case ButtonShape.SegmentStart:
                    return new CGRect(rect.X, rect.Y, rect.Width + (_hz ? 20 : 0), rect.Height + (_hz ? 0 : 20));
                case ButtonShape.SegmentMid:
                    return new CGRect(rect.X + (_hz ? -20 : 0), rect.Y + (_hz ? 0 : -20), rect.Width + (_hz ? 40 : 0), rect.Height + (_hz ? 0 : 40));
                case ButtonShape.SegmentEnd:
                    return new CGRect(rect.X + (_hz ? -20 : 0), rect.Y + (_hz ? 0 : -20), rect.Width + (_hz ? 20 : 0), rect.Height + (_hz ? 0 : 20));
            }
            return rect;
        }

        CGRect SegmentAllowanceRect(CGRect rect, nfloat allowance)
        {
            CGRect result;
            switch (_segmentType)
            {
                case ButtonShape.SegmentStart:
                    result = new CGRect(rect.Left, rect.Top, rect.Width + (_hz ? allowance : 0), rect.Height + (_vt ? allowance : 0));
                    break;
                case ButtonShape.SegmentMid:
                    result = new CGRect(rect.Left - (_hz ? allowance : 0), rect.Top - (_vt ? allowance : 0), rect.Width + (_hz ? allowance * 2 : 0), rect.Height + (_vt ? allowance * 2 : 0));
                    break;
                case ButtonShape.SegmentEnd:
                    result = new CGRect(rect.Left - (_hz ? allowance : 0), rect.Top - (_vt ? allowance : 0), rect.Width + (_hz ? allowance : 0), rect.Height + (_vt ? allowance : 0));
                    break;
                default:
                    result = rect;
                    break;
            }
            //System.Diagnostics.Debug.WriteLine("SegmentAllowanceRect result=[" + result + "]");
            return result;
        }


        CGRect RectInset(CGRect rect, float inset)
        {
            return RectInset(rect, inset, inset, inset, inset);
        }

        CGRect RectInset(CGRect rect, double left, double top, double right, double bottom)
        {
            var newLeft = (nfloat)Math.Round((rect.X + left) * Display.Scale) / Display.Scale;
            var newTop = (nfloat)Math.Round((rect.Y + top) * Display.Scale) / Display.Scale;
            var newRight = (nfloat)Math.Round((rect.Right - right) * Display.Scale) / Display.Scale;
            var newBottom = (nfloat)Math.Round((rect.Bottom - bottom) * Display.Scale) / Display.Scale;

            var newWidth = newRight - newLeft;
            var newHeight = newBottom - newTop;

            return new CGRect(newLeft, newTop, newWidth, newHeight);
        }


        static readonly nfloat a90 = (nfloat)(Math.PI / 2.0);
        static readonly nfloat a180 = (nfloat)Math.PI;
        static readonly nfloat a270 = (nfloat)(a90 * 3);

        CGPath RoundedBoxPath(CGRect perimeter, RoundRectPath pathType)
        {
            var offset = _roundedBoxElement.OutlineWidth;
            if (pathType == RoundRectPath.Outline)
                offset /= 2;
            bool drawOutline = _roundedBoxElement.OutlineWidth > 0.05 && _roundedBoxElement.OutlineColor.A > 0.01;
            CGPath result = default(CGPath);
            CGRect boundary = default(CGRect);
            var materialButton = _roundedBoxElement as MaterialButton;
            //var _roundedBoxElement.OutlineRadius = roundedBox.OutlineRadius; // * Display.Scale;
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
                if (_roundedBoxElement.IsElliptical)
                    result = Ellipse(boundary);
                else
                    result = RectangularPerimeterPath(_roundedBoxElement, boundary, _roundedBoxElement.OutlineRadius - (drawOutline ? offset : 0));
            }
            else
                result = RectangularPerimeterPath(_roundedBoxElement, boundary, _roundedBoxElement.OutlineRadius - (drawOutline ? offset : 0));
            return result;
        }

        CGPath Ellipse(CGRect rect)
        {
            return CGPath.EllipseFromRect(rect);
        }

        internal static CGPath RectangularPerimeterPath(IRoundedBox element, CGRect rect, float radius)
        {
            radius = Math.Max(radius, 0);

            var materialButton = element as MaterialButton;
            ButtonShape type = materialButton == null ? ButtonShape.Rectangle : materialButton.SegmentType;
            StackOrientation orientation = materialButton == null ? StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

            var path = new CGPath();

            if (type == ButtonShape.Rectangle)
            {
                path.MoveToPoint((rect.Left + rect.Right) / 2, rect.Top);
                path.AddLineToPoint(rect.Left + radius, rect.Top);
                if (radius > 0)
                    path.AddRelativeArc(rect.Left + radius, rect.Top + radius, radius, a270, -a90);
                path.AddLineToPoint(rect.Left, rect.Bottom - radius);
                if (radius > 0)
                    path.AddRelativeArc(rect.Left + radius, rect.Bottom - radius, radius, a180, -a90);
                path.AddLineToPoint(rect.Right - radius, rect.Bottom);
                if (radius > 0)
                    path.AddRelativeArc(rect.Right - radius, rect.Bottom - radius, radius, a90, -a90);
                path.AddLineToPoint(rect.Right, rect.Top + radius);
                if (radius > 0)
                    path.AddRelativeArc(rect.Right - radius, rect.Top + radius, radius, 0, -a90);
                path.AddLineToPoint((rect.Left + rect.Right) / 2, rect.Top);
            }
            else if (type == ButtonShape.SegmentStart)
            {
                if (orientation == StackOrientation.Horizontal)
                {
                    path.MoveToPoint(rect.Right, rect.Top);
                    path.AddLineToPoint(rect.Left + radius, rect.Top);
                    if (radius > 0)
                        path.AddRelativeArc(rect.Left + radius, rect.Top + radius, radius, a270, -a90);
                    path.AddLineToPoint(rect.Left, rect.Bottom - radius);
                    if (radius > 0)
                        path.AddRelativeArc(rect.Left + radius, rect.Bottom - radius, radius, a180, -a90);
                    path.AddLineToPoint(rect.Right, rect.Bottom);
                }
                else
                {
                    path.MoveToPoint(rect.Right, rect.Bottom);
                    path.AddLineToPoint(rect.Right, rect.Top + radius);
                    if (radius > 0)
                        path.AddRelativeArc(rect.Right - radius, rect.Top + radius, radius, 0, -a90);
                    path.AddLineToPoint(rect.Left + radius, rect.Top);
                    if (radius > 0)
                        path.AddRelativeArc(rect.Left + radius, rect.Top + radius, radius, a270, -a90);
                    path.AddLineToPoint(rect.Left, rect.Bottom);
                }
            }
            else if (type == ButtonShape.SegmentMid)
            {
                if (orientation == StackOrientation.Horizontal)
                {
                    path.MoveToPoint(rect.Right, rect.Top);
                    path.AddLineToPoint(rect.Left, rect.Top);
                    path.AddLineToPoint(rect.Left, rect.Bottom);
                    path.AddLineToPoint(rect.Right, rect.Bottom);
                }
                else
                {
                    path.MoveToPoint(rect.Right, rect.Bottom);
                    path.AddLineToPoint(rect.Right, rect.Top);
                    path.AddLineToPoint(rect.Left, rect.Top);
                    path.AddLineToPoint(rect.Left, rect.Bottom);
                }
            }
            else if (type == ButtonShape.SegmentEnd)
            {
                if (orientation == StackOrientation.Horizontal)
                {
                    path.MoveToPoint((rect.Left + rect.Right) / 2, rect.Top);
                    path.AddLineToPoint(rect.Left, rect.Top);
                    path.AddLineToPoint(rect.Left, rect.Bottom);
                    path.AddLineToPoint(rect.Right - radius, rect.Bottom);
                    if (radius > 0)
                        path.AddRelativeArc(rect.Right - radius, rect.Bottom - radius, radius, a90, -a90);
                    path.AddLineToPoint(rect.Right, rect.Top + radius);
                    if (radius > 0)
                        path.AddRelativeArc(rect.Right - radius, rect.Top + radius, radius, 0, -a90);
                    path.AddLineToPoint((rect.Left + rect.Right) / 2, rect.Top);
                }
                else
                {
                    path.MoveToPoint((rect.Left + rect.Right) / 2, rect.Top);
                    path.AddLineToPoint(rect.Left, rect.Top);
                    path.AddLineToPoint(rect.Left, rect.Bottom - radius);
                    if (radius > 0)
                        path.AddRelativeArc(rect.Left + radius, rect.Bottom - radius, radius, a180, -a90);
                    path.AddLineToPoint(rect.Right - radius, rect.Bottom);
                    if (radius > 0)
                        path.AddRelativeArc(rect.Right - radius, rect.Bottom - radius, radius, a90, -a90);
                    path.AddLineToPoint(rect.Right, rect.Top);
                    path.AddLineToPoint((rect.Left + rect.Right) / 2, rect.Top);
                }
            }

            return path;
        }
        #endregion
    }
}