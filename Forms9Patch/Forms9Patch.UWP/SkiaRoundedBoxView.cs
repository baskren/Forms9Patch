using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.UWP;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using System.ComponentModel;

namespace Forms9Patch.UWP
{
    public class SkiaRoundedBoxView :SKXamlCanvas, IDisposable
    {

        #region Constructor
        public SkiaRoundedBoxView(IRoundedBox roundedBoxElement)
        {
            _roundedBoxElement = roundedBoxElement;
            if (_roundedBoxElement is Xamarin.Forms.VisualElement element)
            {
                element.PropertyChanged += OnElementPropertyChanged;
            }
        }

        private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!ValidLayout(CanvasSize))
                Invalidate();
        }
        #endregion


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SkiaRoundedBoxView() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


        #region local fields
        Forms9Patch.IRoundedBox _roundedBoxElement = null;
        #endregion


        #region Layout State

        #region Layout State Fields
        SKSize _lastCanvasSize = default(SKSize);

        Xamarin.Forms.Color _lastBackgroundColor = default(Xamarin.Forms.Color);
        bool _lastHasShadow = false;
        bool _lastShadowInverted = false;
        Xamarin.Forms.Color _lastOutlineColor = default(Xamarin.Forms.Color);
        double _lastRadius = -1;
        double _lastOutlineWidth = -1;
        Xamarin.Forms.Thickness _lastPadding = default(Xamarin.Forms.Thickness);
        //Forms9Patch.ButtonShape _lastButtonShape = ButtonShape.Rectangle;
        #endregion

        bool ValidLayout(SKSize canvasSize)
        {
            if (canvasSize != _lastCanvasSize)
                return false;
            if (_lastBackgroundColor != _roundedBoxElement.BackgroundColor)
                return false;
            if (_lastHasShadow != _roundedBoxElement.HasShadow)
                return false;
            if (_lastShadowInverted != _roundedBoxElement.ShadowInverted)
                return false;
            if (_lastOutlineColor != _roundedBoxElement.OutlineColor)
                return false;
            if (_lastRadius != _roundedBoxElement.OutlineRadius)
                return false;
            if (_lastOutlineWidth != _roundedBoxElement.OutlineWidth)
                return false;
            if (_lastPadding != _roundedBoxElement.Padding)
                return false;
            //if (_lastButtonShape != _roundedBoxElement.ButtonShape)
            //    return false;
            return true;
        }

        void StoreLayoutProperties()
        {
            _lastCanvasSize = CanvasSize;
            _lastBackgroundColor = _roundedBoxElement.BackgroundColor;
            _lastHasShadow = _roundedBoxElement.HasShadow;
            _lastShadowInverted = _roundedBoxElement.ShadowInverted;
            _lastOutlineColor = _roundedBoxElement.OutlineColor;
            _lastRadius = _roundedBoxElement.OutlineRadius;
            _lastOutlineWidth = _roundedBoxElement.OutlineWidth;
            _lastPadding = _roundedBoxElement.Padding;
            //_lastButtonShape = _roundedBoxElement.ButtonShape;
        }
        #endregion


        #region Layout
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            var surfaceInfo = e.Info;
            var surface = e.Surface;
            var canvas = e.Surface.Canvas;


            if (ValidLayout(CanvasSize))
                return;

            canvas.Clear();

            //Background = new Windows.UI.Xaml.Media.SolidColorBrush(Xamarin.Forms.Color.Black.WithAlpha(0.25).ToWindowsColor());

            if (_roundedBoxElement is MaterialSegmentedControl materialSegmentedControl)
            {
                // do nothing
            }
            else
            {
                var radius = _roundedBoxElement.OutlineRadius * Display.Scale;
                var outlineWidth = _roundedBoxElement.OutlineWidth * Display.Scale;
                bool drawOutline = outlineWidth > 0.05 && _roundedBoxElement.OutlineColor.A > 0.01;
                bool drawFill = _roundedBoxElement.BackgroundColor.A > 0.01;

                if (drawFill || drawOutline)
                {

                    var visualElement = _roundedBoxElement as Xamarin.Forms.VisualElement;

                    if (CanvasSize != default(SKSize))
                    {

                        var rect = new SKRect(0, 0, (float)(((FrameworkElement)Parent).ActualWidth * Display.Scale), (float)(((FrameworkElement)Parent).ActualHeight * Display.Scale));

                        ButtonShape buttonShape = ButtonShape.Rectangle;
                        var hz = true;
                        if (_roundedBoxElement is MaterialButton materialButton)
                        {
                            buttonShape = materialButton.SegmentType;
                            hz = materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Horizontal;
                        }
                        else
                            materialButton = null;

                        var vt = !hz;


                        double separatorWidth = materialButton == null || buttonShape == ButtonShape.Rectangle ? 0 : materialButton.SeparatorWidth < 0 ? outlineWidth : Math.Max(0, materialButton.SeparatorWidth);
                        if (_roundedBoxElement.BackgroundColor.A < 0.01 && (_roundedBoxElement.OutlineColor.A < 0.01 || (outlineWidth < 0.01 && separatorWidth < 0.01)))
                        {

                        }
                        else
                        {
                            var makeRoomForShadow = _roundedBoxElement.HasShadow && _roundedBoxElement.BackgroundColor.A > 0.01 && !_roundedBoxElement.ShadowInverted;
                            var shadowX = (float)(Forms9Patch.Settings.ShadowOffset.X * Display.Scale);
                            var shadowY = (float)(Forms9Patch.Settings.ShadowOffset.Y * Display.Scale);
                            var shadowR = (float)(Forms9Patch.Settings.ShadowRadius);
                            var shadowColor = Xamarin.Forms.Color.FromRgba(0.5, 0.5, 0.5, 1.0).ToWindowsColor().ToSKColor();
                            var shadowPadding = RoundedBoxBase.ShadowPadding(_roundedBoxElement as Xamarin.Forms.Layout);

                            var perimeter = rect;
                            if (makeRoomForShadow)
                            {
                                // what additional padding was allocated to cast the button's shadow?
                                switch(buttonShape)
                                {
                                    case ButtonShape.SegmentStart:
                                        perimeter = new SKRect(rect.Left + (float)shadowPadding.Left, rect.Top + (float)shadowPadding.Top, rect.Right - (hz ? 0 :  (float)shadowPadding.Right), rect.Bottom - (vt ? 0 :  (float)shadowPadding.Bottom));
                                        break;
                                    case ButtonShape.SegmentMid:
                                        perimeter = new SKRect(rect.Left + (hz ? 0 : (float)shadowPadding.Left), rect.Top + (vt ? 0 : (float)shadowPadding.Top), rect.Right - (hz ? 0 : (float)shadowPadding.Right), rect.Bottom - (vt ? 0 : (float)shadowPadding.Bottom));
                                        break;
                                    case ButtonShape.SegmentEnd:
                                        perimeter = new SKRect(rect.Left + (hz ? 0 : (float)shadowPadding.Left), rect.Top + (vt ? 0 : (float)shadowPadding.Top), rect.Right - (float)shadowPadding.Right, rect.Bottom - (float)shadowPadding.Bottom);
                                        break;
                                    default:
                                        perimeter = new SKRect(rect.Left + (float)shadowPadding.Left, rect.Top + (float)shadowPadding.Top, rect.Width - (float)shadowPadding.HorizontalThickness, rect.Height - (float)shadowPadding.VerticalThickness);
                                        break;
                                }

                                if (!_roundedBoxElement.ShadowInverted)
                                {
                                        // if it is a segment, cast the shadow beyond the button's parimeter and clip it (so no overlaps or gaps)
                                    float allowance = Math.Abs(shadowX) + Math.Abs(shadowY) + Math.Abs(shadowR);
                                    SKRect shadowRect = perimeter;
                                    
                                    if (buttonShape == ButtonShape.SegmentStart)
                                        shadowRect = new SKRect(perimeter.Left, perimeter.Top, perimeter.Right + allowance * (vt?0:1), perimeter.Bottom + allowance * (hz?0:1));
                                    else if (buttonShape == ButtonShape.SegmentMid)
                                        shadowRect = new SKRect(perimeter.Left - allowance * (vt?0:1), perimeter.Top - allowance * (hz?0:1), perimeter.Right + allowance * (vt?0:1), perimeter.Bottom + allowance * (hz?0:1));
                                    else if (buttonShape == ButtonShape.SegmentEnd)
                                        shadowRect = new SKRect(perimeter.Left - allowance * (vt?0:1), perimeter.Top - allowance * (hz?0:1), perimeter.Right, perimeter.Bottom);

                                    Clip = new Windows.UI.Xaml.Media.RectangleGeometry
                                    {
                                        Rect = new Rect(0, 0, Width-1, Height-1)
                                    };
                                    

                                    SKPaint shadowPaint = new SKPaint
                                    {
                                        Style = SKPaintStyle.Fill,
                                        Color = shadowColor,
                                    };

                                    var filter = SkiaSharp.SKImageFilter.CreateDropShadow(shadowX, shadowY, shadowR/2, shadowR/2, shadowColor, SKDropShadowImageFilterShadowMode.DrawShadowOnly);
                                    shadowPaint.ImageFilter = filter;
                                    //var filter = SkiaSharp.SKMaskFilter.CreateBlur(SKBlurStyle.Outer, 0.5f);
                                    //shadowPaint.MaskFilter = filter;

                                    switch (buttonShape)
                                    {
                                        case ButtonShape.Elliptical:
                                            canvas.DrawOval(shadowRect, shadowPaint);
                                            break;
                                        case ButtonShape.Circle:
                                            canvas.DrawCircle(shadowRect.MidX, shadowRect.MidY, (float)(Math.Min(shadowRect.MidX, shadowRect.MidY) / 2.0), shadowPaint);
                                            break;
                                        default:
                                            {
                                                SKPath path = PerimeterPath(_roundedBoxElement, shadowRect, radius - (drawOutline ? outlineWidth : 0));
                                                canvas.DrawPath(path, shadowPaint);
                                            }
                                            break;
                                    }
                                }
                            }


                            if (drawFill)
                            {
                                SKPaint fillPaint = new SKPaint
                                {
                                    Style = SKPaintStyle.Fill,
                                    Color = _roundedBoxElement.BackgroundColor.ToWindowsColor().ToSKColor(),
                                    //StrokeJoin = SKStrokeJoin.Round,
                                    IsAntialias = true,
                                };

                                var fillRect = perimeter;
                                switch (buttonShape)
                                {
                                    case ButtonShape.SegmentStart:
                                        fillRect = RectInset(perimeter, outlineWidth, outlineWidth, vt ? outlineWidth : 0, hz ? outlineWidth : 0);
                                        break;
                                    case ButtonShape.SegmentMid:
                                        fillRect = RectInset(perimeter, outlineWidth, outlineWidth, vt ? outlineWidth : 0, hz ? outlineWidth : 0);
                                        break;
                                    case ButtonShape.SegmentEnd:
                                        fillRect = RectInset(perimeter, outlineWidth);
                                        break;
                                    default:
                                        fillRect = RectInset(perimeter, outlineWidth);
                                        break;
                                }
                                switch (buttonShape)
                                {
                                    case ButtonShape.Elliptical:
                                        canvas.DrawOval(fillRect, fillPaint);
                                        break;
                                    case ButtonShape.Circle:
                                        canvas.DrawCircle(fillRect.MidX, fillRect.MidY, (float)(Math.Min(fillRect.MidX, fillRect.MidY) / 2.0), fillPaint);
                                        break;
                                    default:
                                        {
                                            SKPath path = PerimeterPath(_roundedBoxElement, fillRect, radius - (drawOutline ? outlineWidth : 0));
                                            canvas.DrawPath(path, fillPaint);
                                        }
                                        break;
                                }
                            }

                            if (drawOutline)
                            {
                                SKPaint outlinePaint = new SKPaint
                                {
                                    Style = SKPaintStyle.Stroke,
                                    Color = _roundedBoxElement.OutlineColor.ToWindowsColor().ToSKColor(),
                                    StrokeWidth = outlineWidth,
                                    //StrokeJoin = SKStrokeJoin.Round,
                                    IsAntialias = true,
                                    //PathEffect = SKPathEffect.CreateDash(new float[] { 20,20 }, 0)
                                };
                                var outlineRect = perimeter;
                                switch (buttonShape)
                                {
                                    case ButtonShape.SegmentStart:
                                        outlineRect = RectInset(perimeter, outlineWidth / 2, outlineWidth / 2, vt ? outlineWidth / 2 : 0, hz ? outlineWidth / 2 : 0);
                                        break;
                                    case ButtonShape.SegmentMid:
                                        outlineRect = RectInset(perimeter, outlineWidth / 2, outlineWidth / 2, vt ? outlineWidth / 2 : 0, hz ? outlineWidth / 2 : 0);
                                        break;
                                    case ButtonShape.SegmentEnd:
                                        outlineRect = RectInset(perimeter, outlineWidth / 2);
                                        break;
                                    default:
                                        outlineRect = RectInset(perimeter, outlineWidth / 2);
                                        break;
                                }
                                switch (buttonShape)
                                {
                                    case ButtonShape.Elliptical:
                                        canvas.DrawOval(outlineRect, outlinePaint);
                                        break;
                                    case ButtonShape.Circle:
                                        canvas.DrawCircle(outlineRect.MidX, outlineRect.MidY, (float)(Math.Min(outlineRect.MidX, outlineRect.MidY) / 2.0), outlinePaint);
                                        break;
                                    default:
                                        {
                                            SKPath path = PerimeterPath(_roundedBoxElement, outlineRect, radius - (drawOutline ? outlineWidth : 0));
                                            canvas.DrawPath(path, outlinePaint);
                                        }
                                        break;
                                }
                            }

                            if (makeRoomForShadow && _roundedBoxElement.ShadowInverted)
                            {
                                
                            }

                            StoreLayoutProperties();
                        }
                    }
                }
            }
            base.OnPaintSurface(e);




        }
        #endregion


        #region layout support 
        static SKRect RoundRect(SKRect rect, Xamarin.Forms.StackOrientation orientation, Forms9Patch.ButtonShape type)
        {
            return rect;
        }

        static SKRect SegmentAllowanceRect(SKRect rect, double allowance, Xamarin.Forms.StackOrientation orientation, Forms9Patch.ButtonShape type)
        {
            SKRect result;
            switch (type)
            {
                case ButtonShape.SegmentStart:
                    result = new Rect(rect.Left, rect.Top, rect.Width + (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Height + (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0)).ToSKRect();
                    break;
                case ButtonShape.SegmentMid:
                    result = new Rect(rect.Left - (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0), rect.Width + (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance * 2 : 0), rect.Height + (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance * 2 : 0)).ToSKRect();
                    break;
                case ButtonShape.SegmentEnd:
                    result = new Rect(rect.Left - (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0), rect.Width + (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Height + (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0)).ToSKRect();
                    break;
                default:
                    result = rect;
                    break;
            }
            //if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.SegmentAllowanceRect result=[" + result + "]");
            return result;
        }

        static SKRect RectInset(SKRect rect, double inset) => RectInset(rect, (float)inset);

        static SKRect RectInset(SKRect rect, float inset)
        {
            return RectInset(rect, inset, inset, inset, inset);
        }

        static SKRect RectInset(SKRect rect, double left, double top, double right, double bottom) => RectInset(rect, (float)left, (float)top, (float)right, (float)bottom);

        static SKRect RectInset(SKRect rect, float left, float top, float right, float bottom) => new SKRect(rect.Left + left, rect.Top + top, rect.Right - right, rect.Bottom - bottom);

        internal static SKPath PerimeterPath(Forms9Patch.IRoundedBox element, SKRect rect, float radius)
        {
            radius = Math.Max(radius, 0);

            var materialButton = element as MaterialButton;
            ButtonShape type = materialButton == null ? ButtonShape.Rectangle : materialButton.SegmentType;
            Xamarin.Forms.StackOrientation orientation = materialButton == null ? Xamarin.Forms.StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

            var path = new SKPath();
            //path.FillType = SKPathFillType.InverseWinding;

            //System.Diagnostics.Debug.WriteLine("RectangularPerimeterPath rect[" + rect + "] type=["+type+"] ");
            //rect = RoundRect(rect, orientation, type);
            //System.Diagnostics.Debug.WriteLine("RectangularPerimeterPath rect[" + rect + "] type=[" + type + "] ");
            //System.Diagnostics.Debug.WriteLine("");
            var diameter = radius * 2;
            var topLeft = new SKRect(rect.Left, rect.Top, rect.Left + diameter, rect.Top + diameter);
            var bottomLeft = new SKRect(rect.Left, rect.Bottom - diameter, rect.Left + diameter, rect.Bottom);
            var bottomRight = new SKRect(rect.Right - diameter, rect.Bottom - diameter, rect.Right, rect.Bottom);
            var topRight = new SKRect(rect.Right - diameter, rect.Top, rect.Right, rect.Top + diameter);


            if (type == ButtonShape.Rectangle)
            {
                path.MoveTo((rect.Left + rect.Right) / 2, rect.Top);
                path.LineTo(rect.Left + radius, rect.Top);
                if (radius > 0)
                    //path.ArcTo(rect.Left + radius, rect.Top + radius, rect.Left, rect.Top + radius, radius);
                    path.ArcTo(topLeft, 270, -90, false);
                path.LineTo(rect.Left, rect.Bottom - radius);
                if (radius > 0)
                    //path.ArcTo(rect.Left + radius, rect.Bottom - radius, rect.Left + radius, rect.Bottom, radius);
                    path.ArcTo(bottomLeft, 180, -90, false);
                path.LineTo(rect.Right - radius, rect.Bottom);
                if (radius > 0)
                    //path.ArcTo(rect.Right - radius, rect.Bottom - radius, rect.Right, rect.Bottom - radius, radius);
                    path.ArcTo(bottomRight, 90, -90, false);
                path.LineTo(rect.Right, rect.Top + radius);
                if (radius > 0)
                    //path.ArcTo(rect.Right - radius, rect.Top + radius, rect.Right - radius, rect.Top, radius);
                    path.ArcTo(topRight, 0, -90, false);
                path.LineTo((rect.Left + rect.Right) / 2, rect.Top);
            }
            else if (type == ButtonShape.SegmentStart)
            {
                if (orientation == Xamarin.Forms.StackOrientation.Horizontal)
                {
                    path.MoveTo(rect.Right, rect.Top);
                    path.LineTo(rect.Left + radius, rect.Top);
                    if (radius > 0)
                        path.ArcTo(topLeft, 270, -90, false);
                    path.LineTo(rect.Left, rect.Bottom - radius);
                    if (radius > 0)
                        path.ArcTo(bottomLeft, 180, -90, false);
                    path.LineTo(rect.Right, rect.Bottom);
                }
                else
                {
                    path.MoveTo(rect.Right, rect.Bottom);
                    path.LineTo(rect.Right, rect.Top + radius);
                    if (radius > 0)
                        path.ArcTo(topRight, 0, -90, false);
                    path.LineTo(rect.Left + radius, rect.Top);
                    if (radius > 0)
                        path.ArcTo(topLeft, 270, -90, false);
                    path.LineTo(rect.Left, rect.Bottom);
                }
            }
            else if (type == ButtonShape.SegmentMid)
            {
                if (orientation == Xamarin.Forms.StackOrientation.Horizontal)
                {
                    path.MoveTo(rect.Right, rect.Top);
                    path.LineTo(rect.Left, rect.Top);
                    path.LineTo(rect.Left, rect.Bottom);
                    path.LineTo(rect.Right, rect.Bottom);
                }
                else
                {
                    path.MoveTo(rect.Right, rect.Bottom);
                    path.LineTo(rect.Right, rect.Top);
                    path.LineTo(rect.Left, rect.Top);
                    path.LineTo(rect.Left, rect.Bottom);
                }
            }
            else if (type == ButtonShape.SegmentEnd)
            {
                if (orientation == Xamarin.Forms.StackOrientation.Horizontal)
                {
                    path.MoveTo((rect.Left + rect.Right) / 2, rect.Top);
                    path.LineTo(rect.Left, rect.Top);
                    path.LineTo(rect.Left, rect.Bottom);
                    path.LineTo(rect.Right - radius, rect.Bottom);
                    if (radius > 0)
                        path.ArcTo(bottomRight, 90, -90, false);
                    path.LineTo(rect.Right, rect.Top + radius);
                    if (radius > 0)
                        path.ArcTo(topRight, 0, -90, false);
                    path.LineTo((rect.Left + rect.Right) / 2, rect.Top);
                }
                else
                {
                    path.MoveTo((rect.Left + rect.Right) / 2, rect.Top);
                    path.LineTo(rect.Left, rect.Top);
                    path.LineTo(rect.Left, rect.Bottom - radius);
                    if (radius > 0)
                        path.ArcTo(bottomLeft, 180, -90, false);
                    path.LineTo(rect.Right - radius, rect.Bottom);
                    if (radius > 0)
                        path.ArcTo(bottomRight, 90, -90, false);
                    path.LineTo(rect.Right, rect.Top);
                    path.LineTo((rect.Left + rect.Right) / 2, rect.Top);
                }
            }
            else if (type == ButtonShape.Obround)
            {
                if (rect.Width > rect.Height)
                {
                    radius = (float) (rect.Height / 2.0);
                    var left = new SKRect(0, 0, rect.Height, rect.Height);
                    var right = new SKRect(rect.Width - rect.Height, 0, rect.Height, rect.Height);
                    path.MoveTo(rect.Right - radius, rect.Top);
                    path.LineTo(rect.Left + radius, rect.Top);
                    path.ArcTo(left, -90, -180, false);
                    path.LineTo(rect.Right - radius, rect.Bottom);
                    path.ArcTo(right, 90, -180, false);
                }
                else
                {
                    radius = (float)(rect.Width / 2.0);
                    var top = new SKRect(0, 0, rect.Width, rect.Width);
                    var bottom = new SKRect(0, rect.Height - rect.Width, rect.Width, rect.Width);
                    path.MoveTo(rect.Left, rect.Top + radius);
                    path.LineTo(rect.Left, rect.Bottom - radius);
                    path.ArcTo(bottom, 180, -180, false);
                    path.LineTo(rect.Right, rect.Top + radius);
                    path.ArcTo(top, 0, -180, false);
                }
            }
            return path;
        }

        #endregion
    }
}
