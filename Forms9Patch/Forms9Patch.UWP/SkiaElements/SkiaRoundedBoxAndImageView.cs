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
using System.Diagnostics;

namespace Forms9Patch.UWP
{
    public class SkiaRoundedBoxAndImageView : SKXamlCanvas, IDisposable
    {

        bool _debugMessages = true;

        #region Constructor
        internal SkiaRoundedBoxAndImageView(IShape roundedBoxElement)
        {
            _instanceId = roundedBoxElement.InstanceId;
            _roundedBoxElement = roundedBoxElement;
            if (_roundedBoxElement is Xamarin.Forms.VisualElement element)
            {
                element.PropertyChanged += OnElementPropertyChanged;
                element.SizeChanged += OnElementSizeChanged;
            }
            SetImageElement();
            SizeChanged += OnSizeChanged;
        }

        private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (MaterialButton!=null && MaterialButton.ParentSegmentsOrientation==Xamarin.Forms.StackOrientation.Vertical)
                System.Diagnostics.Debug.WriteLine("["+_roundedBoxElement.InstanceId+"]["+_roundedBoxElement.InstanceId+"][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName()+"] e.PropertyName=["+e.PropertyName+"]");
            //if (e.PropertyName==ShapeBase.IgnoreShapePropertiesChangesProperty.PropertyName)
            //    System.Diagnostics.Debug.WriteLine("            IgnoreChanges=["+IgnoreChanges+"]");
            if (!ValidLayout(CanvasSize))
                Invalidate();
            if (e.PropertyName == ShapeBase.BackgroundImageProperty.PropertyName)
                SetImageElement();
        }

        void SetImageElement()
        {
            if (_roundedBoxElement is ILayout background)
                ImageElement = background.BackgroundImage;
            else if (_roundedBoxElement is Image image)
                ImageElement = image;
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
                    _xfImageSource?.ReleaseF9PBitmap(this);
                    _sourceBitmap = null;
                    if (_roundedBoxElement is Xamarin.Forms.VisualElement element)
                        element.PropertyChanged -= OnElementPropertyChanged;
                    SizeChanged -= OnSizeChanged;
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


        #region Properties
        MaterialButton MaterialButton => _roundedBoxElement as MaterialButton;

        internal Forms9Patch.Image ImageElement
        {
            get { return _imageElement; }
            set
            {
                if (value != _imageElement)
                {
                    if (_imageElement != null)
                        _imageElement.PropertyChanged -= OnImageElementPropertyChanged;
                    _imageElement = value;
                    if (_imageElement != null)
                        _imageElement.PropertyChanged += OnImageElementPropertyChanged;
                    SetSourceAsync();
                }
            }
        }

        Xamarin.Forms.BindableObject BindableObject => _roundedBoxElement as Xamarin.Forms.BindableObject;

        Xamarin.Forms.Color BackgroundColor => (Xamarin.Forms.Color)BindableObject.GetValue(ShapeBase.BackgroundColorProperty);

        internal async Task SetSourceAsync()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "]ImageView.SetSourceAsync ENTER");
            if (_imageElement.Source != _xfImageSource)
            {
                // release the previous
                _xfImageSource?.ReleaseF9PBitmap(this);
                _sourceBitmap = null;
                _sourceRangeLists = null;

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                ((Xamarin.Forms.IImageController)_imageElement)?.SetIsLoading(true);
                if (Settings.IsLicenseValid || _instanceId < 4)
                    _xfImageSource = _imageElement?.Source;
                else if (_xfImageSource != null)
                    _xfImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9Patch.Resources.unlicensedcopy");
                else
                    _xfImageSource = null;

                stopWatch.Stop();
                if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] A[" + stopWatch.ElapsedMilliseconds + "]");
                stopWatch.Reset();
                stopWatch.Start();

                if (_xfImageSource != null)
                {
                    _sourceBitmap = await _xfImageSource.FetchF9PBitmap(this);
                    _sourceRangeLists = _sourceBitmap.RangeLists;
                }

                if (_sourceBitmap == null || _sourceBitmap.SKBitmap == null)
                    _imageElement.SourceImageSize = Xamarin.Forms.Size.Zero;
                else
                    _imageElement.SourceImageSize = new Xamarin.Forms.Size(_sourceBitmap.Width, _sourceBitmap.Height);

                stopWatch.Stop();
                if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] B[" + stopWatch.ElapsedMilliseconds + "]");

                _actualSizeValid = false;
                _validLayout = false;
                Invalidate();
                ((Xamarin.Forms.IImageController)_imageElement)?.SetIsLoading(false);
            }
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "]ImageView.SetSourceAsync EXIT");
        }

        bool HasShadow => (bool)BindableObject.GetValue(ShapeBase.HasShadowProperty);

        bool ShadowInverted => (bool)BindableObject.GetValue(ShapeBase.ShadowInvertedProperty);

        Xamarin.Forms.Color OutlineColor => (Xamarin.Forms.Color)BindableObject.GetValue(ShapeBase.OutlineColorProperty);

        float OutlineRadius => Display.Scale * (float)BindableObject.GetValue(ShapeBase.OutlineRadiusProperty);

        float OutlineWidth => Display.Scale * (float)BindableObject.GetValue(ShapeBase.OutlineWidthProperty);

        ExtendedElementShape ExtendedElementShape => (ExtendedElementShape)BindableObject.GetValue(ShapeBase.ExtendedElementShapeProperty);

        Xamarin.Forms.Thickness ShadowPadding => _roundedBoxElement.ShadowPadding();

        internal Xamarin.Forms.Size SourceImageSize()
        {
            if (_sourceBitmap?.SKBitmap != null)
                return new Xamarin.Forms.Size(_sourceBitmap.Width / Display.Scale, _sourceBitmap.Height / Display.Scale);
            return Xamarin.Forms.Size.Zero;
        }

        #endregion


        /*#region Xamarin.Forms Measuring
        //TODO: DELETE THIS?!?!
        internal Xamarin.Forms.SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (ImageElement == null)
                throw new InvalidCastException("DesiredSize is only valid with the Forms9Patch.Image element");
            var size = SourceImageSize();
            if (size.Width < 1 || size.Height < 1)
                return new Xamarin.Forms.SizeRequest(Xamarin.Forms.Size.Zero);
            var sourceAspect = size.Width / size.Height;

            switch (ImageElement.Fill)
            {
                case Fill.AspectFill:
                    if (double.IsInfinity(widthConstraint) && double.IsInfinity(heightConstraint))
                        return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(widthConstraint, heightConstraint), new Xamarin.Forms.Size(1, 1));
                    if (double.IsInfinity(widthConstraint))
                        return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(heightConstraint * sourceAspect, heightConstraint), new Xamarin.Forms.Size(1, 1));
                    if (double.IsInfinity(heightConstraint))
                        return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(widthConstraint, widthConstraint / sourceAspect), new Xamarin.Forms.Size(1, 1));
                    return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(widthConstraint, heightConstraint), new Xamarin.Forms.Size(1, 1));
                case Fill.AspectFit:
                    var minSize = sourceAspect >= 1 ? new Xamarin.Forms.Size(sourceAspect, 1) : new Xamarin.Forms.Size(1, 1 / sourceAspect);
                    if (double.IsInfinity(widthConstraint) && double.IsInfinity(heightConstraint))
                        return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(widthConstraint, heightConstraint), minSize);
                    if (double.IsInfinity(widthConstraint))
                        return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(heightConstraint * sourceAspect, heightConstraint), minSize);
                    if (double.IsInfinity(heightConstraint))
                        return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(widthConstraint, widthConstraint / sourceAspect), minSize);
                    var constraintAspect = widthConstraint / heightConstraint;
                    if (constraintAspect > sourceAspect)
                        return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(heightConstraint * sourceAspect, heightConstraint), minSize);
                    return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(heightConstraint * constraintAspect, heightConstraint), minSize);
                case Fill.None:
                    return new Xamarin.Forms.SizeRequest(size, size);
                default:  // Fill and Tile
                    return new Xamarin.Forms.SizeRequest(new Xamarin.Forms.Size(widthConstraint, heightConstraint), new Xamarin.Forms.Size(1, 1));
            }

        }
        #endregion*/


        #region local fields
        int _instanceId;

        Forms9Patch.IShape _roundedBoxElement = null;

        Forms9Patch.Image _imageElement;
        Xamarin.Forms.ImageSource _xfImageSource;

        F9PBitmap _sourceBitmap;
        RangeLists _sourceRangeLists = null;

        #endregion


        #region Layout State Fields

        SKSize _lastCanvasSize = default(SKSize);

        #region RoundedBox Layout State
        bool _validLayout;
        Xamarin.Forms.Color _lastBackgroundColor = default(Xamarin.Forms.Color);
        bool _lastHasShadow = false;
        bool _lastShadowInverted = false;
        Xamarin.Forms.Color _lastOutlineColor = default(Xamarin.Forms.Color);
        double _lastRadius = -1;
        double _lastOutlineWidth = -1;
        ExtendedElementShape _lastExtendedElementShape = default(ExtendedElementShape);
        //bool _lastIgnoreChanges = false;
        #endregion

        #region Background Layout State
        Xamarin.Forms.Thickness _lastPadding = default(Xamarin.Forms.Thickness);
        #endregion

        #region Image Layout State
        //Forms9Patch.ElementShape _lastElementShape = ElementShape.Rectangle;
        bool _actualSizeValid;
        #endregion

        #endregion


        #region Layout State
        bool ValidLayout(SKSize canvasSize)
        {
            //if (!IgnoreChanges && _lastIgnoreChanges == true)
            //    return false;
            if (!_validLayout)
                return false;
            if (!_actualSizeValid)
                return false;
            if (canvasSize != _lastCanvasSize)
                return false;
            if (_lastBackgroundColor != BackgroundColor)
                return false;
            if (_lastHasShadow != HasShadow)
                return false;
            if (_lastShadowInverted != ShadowInverted)
                return false;
            if (_lastOutlineColor != OutlineColor)
                return false;
            if (_lastRadius != OutlineRadius)
                return false;
            if (_lastOutlineWidth != OutlineWidth)
                return false;
            if (_roundedBoxElement is ILayout backgroundElement && _lastPadding != ShadowPadding)
                return false;
            if (_lastExtendedElementShape != ExtendedElementShape)
                return false;
            return true;
        }

        void StoreLayoutProperties()
        {
            _lastCanvasSize = CanvasSize;
            _lastBackgroundColor = BackgroundColor;
            _lastHasShadow = HasShadow;
            _lastShadowInverted = ShadowInverted;
            _lastOutlineColor = OutlineColor;
            _lastRadius = OutlineRadius;
            _lastOutlineWidth = OutlineWidth;
            if (_roundedBoxElement is ILayout backgroundElement)
                _lastPadding = ShadowPadding;
            _lastExtendedElementShape = ExtendedElementShape;
            _validLayout = true;
            _actualSizeValid = true;
            //_lastIgnoreChanges = IgnoreChanges;
        }

        private void OnElementSizeChanged(object sender, EventArgs e)
        {
            if (MaterialButton != null && MaterialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical)
                if (_debugMessages)
                    System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] element.Size=[" + ((Xamarin.Forms.VisualElement)_roundedBoxElement).Bounds.Size + "] Size=[" + Width + ", " + Height + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "]");
            Invalidate();
        }

        private void OnImageElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Xamarin.Forms.Image.SourceProperty.PropertyName)
                SetSourceAsync();
            else if (e.PropertyName == Forms9Patch.Image.TintColorProperty.PropertyName
                || e.PropertyName == Forms9Patch.Image.FillProperty.PropertyName
                || e.PropertyName == Forms9Patch.Image.CapInsetsProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.VisualElement.WidthRequestProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.VisualElement.HeightRequestProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.View.HorizontalOptionsProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.View.VerticalOptionsProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.View.MarginProperty.PropertyName
                )
            {
                _validLayout = false;
                Invalidate();
            }

        }
        #endregion


        #region Windows Layout Support
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _actualSizeValid = false;
            Invalidate();
        }

        protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
        {
            if (MaterialButton != null && MaterialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical) if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] availableSize=[" + availableSize + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "]");

            var result = base.MeasureOverride(availableSize);

            if (MaterialButton != null && MaterialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical)
            {
                if (_sourceBitmap != null)
                {
                    if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap.Size=[" + _sourceBitmap.Width + ", " + _sourceBitmap.Height + "]");
                }
                else
                {
                    if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap is null");
                }
            }


            if (_roundedBoxElement is Forms9Patch.Image && !_actualSizeValid && _sourceBitmap != null)
            {
                bool constrainedWidth = !double.IsInfinity(availableSize.Width) || ImageElement.WidthRequest > -1;
                bool constrainedHeight = !double.IsInfinity(availableSize.Height) || ImageElement.HeightRequest > -1;

                bool drawOutline = (OutlineColor.A > 0.01 && OutlineWidth > 0.05);

                double constrainedWidthValue = availableSize.Width;// - (drawOutline ? OutlineWidth * 2 : 0);// - ShadowPadding.HorizontalThickness;
                if (ImageElement.WidthRequest > -1)
                    constrainedWidthValue = Math.Min(constrainedWidthValue, ImageElement.WidthRequest);
                double constrainedHeightValue = availableSize.Height;// - (drawOutline ? OutlineWidth * 2 : 0);// - ShadowPadding.VerticalThickness;
                if (ImageElement.HeightRequest > -1)
                    constrainedHeightValue = Math.Min(constrainedHeightValue, ImageElement.HeightRequest);

                if ((!constrainedWidth && !constrainedHeight) || ImageElement.Fill == Fill.None)
                    result = new Windows.Foundation.Size(_sourceBitmap.Width / Display.Scale, _sourceBitmap.Height / Display.Scale);
                else
                {
                    var sourceAspect = _sourceBitmap.Height / _sourceBitmap.Width;

                    if (constrainedWidth && constrainedHeight)
                    {
                        // if single image, SetAspect should do all the heavy lifting.  if stitched together, then it's ImageFill.Fill;
                        result = new Windows.Foundation.Size(constrainedWidthValue, constrainedHeightValue);
                    }
                    else if (constrainedWidth)
                    {
                        if (ImageElement.Fill == Fill.Tile)
                            result = new Windows.Foundation.Size(constrainedWidthValue, availableSize.Height / Display.Scale);
                        else
                            result = new Windows.Foundation.Size(constrainedWidthValue, constrainedWidthValue * sourceAspect);
                    }
                    else if (constrainedHeight)
                    {
                        if (ImageElement.Fill == Fill.Tile)
                            result = new Windows.Foundation.Size(availableSize.Width / Display.Scale, constrainedHeightValue);
                        else
                            result = new Windows.Foundation.Size(constrainedHeightValue / sourceAspect, constrainedHeightValue);
                    }
                }
                if (MaterialButton != null && MaterialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical) if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap.Size=[" + _sourceBitmap.Width + ", " + _sourceBitmap.Height + "]");
            }

            return result;
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            if (MaterialButton != null && MaterialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical) if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] availableSize=[" + finalSize + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "]");

            var result = base.ArrangeOverride(finalSize);

            if (_roundedBoxElement is Forms9Patch.Image && !_actualSizeValid && _sourceBitmap != null && Children.Count > 0)
                Invalidate();

            if (MaterialButton != null && MaterialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical)
            {
                if (_debugMessages)
                {
                    if (_sourceBitmap != null)
                        System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap.Size=[" + _sourceBitmap.Width + ", " + _sourceBitmap.Height + "]");
                    else
                        System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap is null");
                }
            }

            return result;
        }

        #endregion


        #region Layout

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            var surfaceInfo = e.Info;
            var surface = e.Surface;
            var canvas = e.Surface.Canvas;

           // if (IgnoreChanges)
           // {
           //     _lastIgnoreChanges = true;
           //     return;
           // }

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
                var hz = true;
                if (MaterialButton!=null)
                {
                    hz = MaterialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Horizontal;
                }
                var vt = !hz;

                var backgroundColor = BackgroundColor;
                var hasShadow = HasShadow;
                var shadowInverted = ShadowInverted;
                var outlineWidth = OutlineWidth;
                var outlineRadius = OutlineRadius;
                var outlineColor = OutlineColor;
                var elementShape = ExtendedElementShape;

                double separatorWidth = MaterialButton == null || elementShape == ExtendedElementShape.Rectangle ? 0 : MaterialButton.SeparatorWidth < 0 ? outlineWidth : Math.Max(0, MaterialButton.SeparatorWidth);

                bool drawOutline = (outlineColor.A > 0.01 && outlineWidth > 0.05);
                bool drawImage = ( _sourceBitmap?.SKBitmap!=null && _sourceBitmap.Width > 0 && _sourceBitmap.Height > 0);
                bool drawSeparators = outlineColor.A > 0.01 && MaterialButton != null && separatorWidth > 0.01;
                bool drawFill = backgroundColor.A > 0.01; // ||_roundedBoxElement.BackgroundImage?.Source !=null;

                if (drawFill || drawOutline || drawSeparators || drawImage)
                {

                    //var visualElement = _roundedBoxElement as Xamarin.Forms.VisualElement;

                    if (CanvasSize != default(SKSize))
                    {
                        if (MaterialButton != null && MaterialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical)  if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "][" + GetType() + "." + PCL.Utils.ReflectionExtensions.CallerMemberName() + "]  Parent.Size=[" + ((FrameworkElement)Parent).ActualWidth + ","+ ((FrameworkElement)Parent).ActualHeight + "]");
                        var rect = new SKRect(0, 0, (float)(((FrameworkElement)Parent).ActualWidth * Display.Scale), (float)(((FrameworkElement)Parent).ActualHeight * Display.Scale));

                        var makeRoomForShadow = hasShadow && (backgroundColor.A > 0.01 || drawImage); // && !_roundedBoxElement.ShadowInverted;
                        var shadowX = (float)(Forms9Patch.Settings.ShadowOffset.X * Display.Scale);
                        var shadowY = (float)(Forms9Patch.Settings.ShadowOffset.Y * Display.Scale);
                        var shadowR = (float)(Forms9Patch.Settings.ShadowRadius * Display.Scale);
                        var shadowColor = Xamarin.Forms.Color.FromRgba(0.0, 0.0, 0.0, 0.75).ToWindowsColor().ToSKColor();
                        var shadowPadding = ShapeBase.ShadowPadding(_roundedBoxElement, hasShadow);

                        var perimeter = rect;

                        Clip = null;

                        if (makeRoomForShadow)
                        {
                            // what additional padding was allocated to cast the button's shadow?
                            switch (elementShape)
                            {
                                case ExtendedElementShape.SegmentStart:
                                    perimeter = new SKRect(rect.Left + (float)shadowPadding.Left, rect.Top + (float)shadowPadding.Top, rect.Right - (hz ? 0 : (float)shadowPadding.Right), rect.Bottom - (vt ? 0 : (float)shadowPadding.Bottom));
                                    break;
                                case ExtendedElementShape.SegmentMid:
                                    perimeter = new SKRect(rect.Left + (hz ? 0 : (float)shadowPadding.Left), rect.Top + (vt ? 0 : (float)shadowPadding.Top), rect.Right - (hz ? 0 : (float)shadowPadding.Right), rect.Bottom - (vt ? 0 : (float)shadowPadding.Bottom));
                                    break;
                                case ExtendedElementShape.SegmentEnd:
                                    perimeter = new SKRect(rect.Left + (hz ? 0 : (float)shadowPadding.Left), rect.Top + (vt ? 0 : (float)shadowPadding.Top), rect.Right - (float)shadowPadding.Right, rect.Bottom - (float)shadowPadding.Bottom);
                                    break;
                                default:
                                    perimeter = new SKRect(rect.Left + (float)shadowPadding.Left, rect.Top + (float)shadowPadding.Top, rect.Right - (float)shadowPadding.Right, rect.Bottom - (float)shadowPadding.Bottom);
                                    break;
                            }

                            if (!shadowInverted)
                            {
                                // if it is a segment, cast the shadow beyond the button's parimeter and clip it (so no overlaps or gaps)
                                float allowance = Math.Abs(shadowX) + Math.Abs(shadowY) + Math.Abs(shadowR);
                                SKRect shadowRect = perimeter;

                                if (elementShape == ExtendedElementShape.SegmentStart)
                                    shadowRect = new SKRect(perimeter.Left, perimeter.Top, perimeter.Right + allowance * (vt ? 0 : 1), perimeter.Bottom + allowance * (hz ? 0 : 1));
                                else if (elementShape == ExtendedElementShape.SegmentMid)
                                    shadowRect = new SKRect(perimeter.Left - allowance * (vt ? 0 : 1), perimeter.Top - allowance * (hz ? 0 : 1), perimeter.Right + allowance * (vt ? 0 : 1), perimeter.Bottom + allowance * (hz ? 0 : 1));
                                else if (elementShape == ExtendedElementShape.SegmentEnd)
                                    shadowRect = new SKRect(perimeter.Left - allowance * (vt ? 0 : 1), perimeter.Top - allowance * (hz ? 0 : 1), perimeter.Right, perimeter.Bottom);

                                Clip = new Windows.UI.Xaml.Media.RectangleGeometry
                                {
                                    Rect = new Rect(0, 0, Width - 1, Height - 1)
                                };


                                var shadowPaint = new SKPaint
                                {
                                    Style = SKPaintStyle.Fill,
                                    Color = shadowColor,
                                };

                                var filter = SkiaSharp.SKImageFilter.CreateDropShadow(shadowX, shadowY, shadowR / 2, shadowR / 2, shadowColor, SKDropShadowImageFilterShadowMode.DrawShadowOnly);
                                shadowPaint.ImageFilter = filter;
                                //var filter = SkiaSharp.SKMaskFilter.CreateBlur(SKBlurStyle.Outer, 0.5f);
                                //shadowPaint.MaskFilter = filter;

                                var path = PerimeterPath(_roundedBoxElement, shadowRect, outlineRadius - (drawOutline ? outlineWidth : 0));
                                canvas.DrawPath(path, shadowPaint);
                            }
                        }

                        if (drawFill)
                        {
                            var fillRect = RectInsetForShape(perimeter, elementShape, outlineWidth, vt);
                            var path = PerimeterPath(_roundedBoxElement, fillRect, outlineRadius - (drawOutline ? outlineWidth : 0));

                            if (drawFill)
                            {
                                var fillPaint = new SKPaint
                                {
                                    Style = SKPaintStyle.Fill,
                                    Color = backgroundColor.ToWindowsColor().ToSKColor(),
                                    IsAntialias = true,
                                };
                                canvas.DrawPath(path, fillPaint);
                            }
                        }

                        if (drawImage)
                        {
                            //var fillRect = RectInsetForShape(perimeter, elementShape, 0, vt);
                            var clipPath = PerimeterPath(_roundedBoxElement, perimeter, outlineRadius);
                            GenerateImageLayout(canvas, perimeter, clipPath);
                        }

                        if (drawOutline)// && !drawImage)
                        {
                            var outlinePaint = new SKPaint
                            {
                                Style = SKPaintStyle.Stroke,
                                Color = outlineColor.ToWindowsColor().ToSKColor(),
                                StrokeWidth = outlineWidth,
                                IsAntialias = true,
                                //PathEffect = SKPathEffect.CreateDash(new float[] { 20,20 }, 0)
                            };
                            var outlineRect = RectInsetForShape(perimeter, elementShape, outlineWidth / 2, vt);
                            var path = PerimeterPath(_roundedBoxElement, outlineRect, outlineRadius - (drawOutline ? outlineWidth / 2 : 0));
                            canvas.DrawPath(path, outlinePaint);
                        }

                        if (drawSeparators && !drawOutline && (elementShape == ExtendedElementShape.SegmentMid || elementShape == ExtendedElementShape.SegmentEnd))
                        {
                            var separatorPaint = new SKPaint
                            {
                                Style = SKPaintStyle.Stroke,
                                Color = outlineColor.ToWindowsColor().ToSKColor(),
                                StrokeWidth = outlineWidth,
                                IsAntialias = true,
                                //PathEffect = SKPathEffect.CreateDash(new float[] { 20,20 }, 0)
                            };
                            var path = new SKPath();
                            if (vt)
                            {
                                path.MoveTo(perimeter.Left, perimeter.Top + outlineWidth/2.0f);
                                path.LineTo(perimeter.Right, perimeter.Top + outlineWidth / 2.0f);
                            }
                            else
                            {
                                path.MoveTo(perimeter.Left + outlineWidth / 2.0f, perimeter.Top);
                                path.LineTo(perimeter.Left + outlineWidth / 2.0f, perimeter.Bottom);
                            }
                            canvas.DrawPath(path, separatorPaint);
                        }

                        if (makeRoomForShadow && shadowInverted)
                        {
                            canvas.Save();

                            // setup the paint
                            var insetShadowPaint = new SKPaint
                            {
                                Style = SKPaintStyle.Fill,
                                Color = shadowColor,
                            };
                            var filter = SkiaSharp.SKImageFilter.CreateDropShadow(shadowX, shadowY, shadowR / 2, shadowR / 2, shadowColor, SKDropShadowImageFilterShadowMode.DrawShadowOnly);
                            insetShadowPaint.ImageFilter = filter;

                            // what is the mask?
                            var maskPath = PerimeterPath(_roundedBoxElement, perimeter, outlineRadius);
                            canvas.ClipPath(maskPath);

                            // what is the path that will cast the shadow?
                            // a) the button portion (which will be the hole in the larger outline, b)
                            var path = PerimeterPath(_roundedBoxElement, perimeter, outlineRadius);
                            // b) add to it the larger outline 
                            path.AddRect(RectInset(rect, -50));
                            canvas.DrawPath(path, insetShadowPaint);

                            /*
                            // let's display this just to see if I got it right
                            SKPaint fillPaint = new SKPaint
                            {
                                Style = SKPaintStyle.Fill,
                                Color = Xamarin.Forms.Color.Green.ToWindowsColor().ToSKColor(),
                                IsAntialias = true,
                            };
                            canvas.DrawPath(path, fillPaint);
                            */
                            canvas.Restore();
                        }

                        StoreLayoutProperties();
                    }
                }
            }
            base.OnPaintSurface(e);

            _actualSizeValid = true;


        }
        #endregion


        #region Image Layout
        void GenerateImageLayout(SKCanvas canvas, SKRect fillRect, SKPath clipPath)
        {
            if (_imageElement == null || _sourceBitmap?.SKBitmap == null || _sourceBitmap.Width < 1 || _sourceBitmap.Height < 1)
                return;

            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "]["+GetType()+"."+PCL.Utils.ReflectionExtensions.CallerMemberName()+"]  Fill=[" + _imageElement.Fill + "] W,H=[" + Width + "," + Height + "] ActualWH=[" + ActualWidth + "," + ActualHeight + "] ");
            canvas.Save();
            canvas.ClipPath(clipPath);

            var rangeLists = _imageElement.CapInsets.ToRangeLists(_sourceBitmap.SKBitmap.Width, _sourceBitmap.SKBitmap.Height, _xfImageSource, _sourceRangeLists != null);
            if (rangeLists==null)
                rangeLists = _sourceRangeLists;
            if (rangeLists != null)
            {
                if (rangeLists.PatchesX.Count == 1 && rangeLists.PatchesX[0].Start <= 0 && rangeLists.PatchesX[0].End >= _sourceBitmap.SKBitmap.Width-1)
                    rangeLists.PatchesX.Clear();
                if (rangeLists.PatchesY.Count == 1 && rangeLists.PatchesY[0].Start <= 0 && rangeLists.PatchesY[0].End >= _sourceBitmap.SKBitmap.Height-1)
                    rangeLists.PatchesY.Clear();
                if (rangeLists.PatchesX.Count == 0 && rangeLists.PatchesY.Count == 0)
                    rangeLists = null;
            }
                    
            var bitmap = _sourceBitmap;
            SKPaint paint = null;
            if (_imageElement.TintColor!=Xamarin.Forms.Color.Default && _imageElement.TintColor!=Xamarin.Forms.Color.Transparent)
            {
                var mx = new Single[]
                {
                    0, 0, 0, 0, _imageElement.TintColor.ToWindowsColor().R,
                    0, 0, 0, 0, _imageElement.TintColor.ToWindowsColor().G,
                    0, 0, 0, 0, _imageElement.TintColor.ToWindowsColor().B,
                    0, 0, 0, (float)_imageElement.TintColor.A, 0
                };
                var cf = SKColorFilter.CreateColorMatrix(mx);


                paint = new SKPaint()
                {
                    ColorFilter = cf,
                    IsAntialias = true,
                };
            }

            var fillRectAspect = fillRect.Width / fillRect.Height;
            var bitmapAspect = _sourceBitmap.Width / _sourceBitmap.Height;

            if (_imageElement.Fill == Fill.Tile)
            {
                for (float x = fillRect.Left; x < fillRect.Right; x += _sourceBitmap.SKBitmap.Width)
                    for (float y = fillRect.Top; y < fillRect.Bottom; y += _sourceBitmap.SKBitmap.Height)
                        canvas.DrawBitmap(_sourceBitmap.SKBitmap, x, y, paint);
            }
            else if (_imageElement.Fill == Fill.None)
            {
                float left;
                switch (_imageElement.HorizontalOptions.Alignment)
                {
                    case Xamarin.Forms.LayoutAlignment.Start:
                        left = fillRect.Left;
                        break;
                    case Xamarin.Forms.LayoutAlignment.End:
                        left = fillRect.Right - (float)(_sourceBitmap.Width);
                        break;
                    default:
                        left = (fillRect.Right - (float)_sourceBitmap.Width)/2.0f ;
                        break;
                }
                float top;
                switch (_imageElement.VerticalOptions.Alignment)
                {
                    case Xamarin.Forms.LayoutAlignment.Start:
                        top = fillRect.Top;
                        break;
                    case Xamarin.Forms.LayoutAlignment.End:
                        top = fillRect.Bottom - (float)(_sourceBitmap.Height);
                        break;
                    default:
                        top = (fillRect.Bottom - (float)_sourceBitmap.Height) / 2.0f;
                        break;
                }
                fillRect = SKRect.Create(left, top, (float)_sourceBitmap.Width, (float)_sourceBitmap.Height);
                canvas.DrawBitmap(_sourceBitmap.SKBitmap, _sourceBitmap.SKBitmap.Info.Rect, fillRect, paint);
            }
            else if (rangeLists == null)
            {
                if (_imageElement.Fill == Fill.AspectFill)
                {
                    var croppedWidth = bitmapAspect > fillRectAspect ? _sourceBitmap.Height * fillRectAspect : _sourceBitmap.Width;
                    var croppedHeight = bitmapAspect > fillRectAspect ? _sourceBitmap.Height : _sourceBitmap.Width / fillRectAspect;
                    float left;
                    switch (_imageElement.HorizontalOptions.Alignment)
                    {
                        case Xamarin.Forms.LayoutAlignment.Start:
                            left = 0;
                            break;
                        case Xamarin.Forms.LayoutAlignment.End:
                            left = (float)(_sourceBitmap.Width - croppedWidth);
                            break;
                        default:
                            left = (float)(_sourceBitmap.Width - croppedWidth) / 2.0f;
                            break;
                    }
                    float top;
                    switch (_imageElement.VerticalOptions.Alignment)
                    {
                        case Xamarin.Forms.LayoutAlignment.Start:
                            top = 0;
                            break;
                        case Xamarin.Forms.LayoutAlignment.End:
                            top = (float)(_sourceBitmap.Height - croppedHeight);
                            break;
                        default:
                            top = (float)(_sourceBitmap.Height - croppedHeight) / 2.0f;
                            break;
                    }
                    var cropRect = SKRect.Create(left, top, (float)croppedWidth, (float)croppedHeight);
                    canvas.DrawBitmap(_sourceBitmap.SKBitmap, cropRect, fillRect, paint);
                }
                else if (_imageElement.Fill == Fill.AspectFit)
                {
                    var destWidth = bitmapAspect > fillRectAspect ? fillRect.Width : fillRect.Height * bitmapAspect;
                    var destHeight = bitmapAspect > fillRectAspect ? fillRect.Width / bitmapAspect : fillRect.Height;
                    float left;
                    switch (_imageElement.HorizontalOptions.Alignment)
                    {
                        case Xamarin.Forms.LayoutAlignment.Start:
                            left = 0;
                            break;
                        case Xamarin.Forms.LayoutAlignment.End:
                            left = (float)(fillRect.Width - destWidth);
                            break;
                        default:
                            left = (float)(fillRect.Width - destWidth) / 2.0f;
                            break;
                    }
                    float top;
                    switch (_imageElement.VerticalOptions.Alignment)
                    {
                        case Xamarin.Forms.LayoutAlignment.Start:
                            top = 0;
                            break;
                        case Xamarin.Forms.LayoutAlignment.End:
                            top = (float)(fillRect.Height - destHeight);
                            break;
                        default:
                            top = (float)(fillRect.Height - destHeight) / 2.0f;
                            break;
                    }
                    var destRect = SKRect.Create(left, top, (float)destWidth, (float)destHeight);
                    canvas.DrawBitmap(_sourceBitmap.SKBitmap, _sourceBitmap.SKBitmap.Info.Rect, destRect, paint);
                }
                else // Fill==Fill.Fill
                    canvas.DrawBitmap(_sourceBitmap.SKBitmap, _sourceBitmap.SKBitmap.Info.Rect, fillRect, paint);
            }
            else
            {
                var lattice = rangeLists.ToSKLattice(_sourceBitmap.SKBitmap);
                canvas.DrawBitmapLattice(_sourceBitmap.SKBitmap, lattice, fillRect);
                //canvas.DrawBitmap(_sourceBitmap, _sourceBitmap.Info.Rect, fillRect);
            }

            canvas.Restore();
        }


        #endregion


        #region layout support 

        static SKRect RectInsetForShape(SKRect perimeter, ExtendedElementShape buttonShape, float inset, bool vt)
        {
            if (inset < 0)
                inset = 0;
            var result = perimeter;
            var hz = !vt;
            switch (buttonShape)
            {
                case ExtendedElementShape.SegmentStart:
                    result = RectInset(perimeter, inset, inset, vt ? inset : 0, hz ? inset : 0);
                    break;
                case ExtendedElementShape.SegmentMid:
                    result = RectInset(perimeter, inset, inset, vt ? inset : 0, hz ? inset : 0);
                    break;
                case ExtendedElementShape.SegmentEnd:
                    result = RectInset(perimeter, inset);
                    break;
                default:
                    result = RectInset(perimeter, inset);
                    break;
            }
            return result;
        }

        static SKRect RoundRect(SKRect rect, Xamarin.Forms.StackOrientation orientation, Forms9Patch.ExtendedElementShape type)
        {
            return rect;
        }

        static SKRect SegmentAllowanceRect(SKRect rect, double allowance, Xamarin.Forms.StackOrientation orientation, Forms9Patch.ExtendedElementShape type)
        {
            SKRect result;
            switch (type)
            {
                case ExtendedElementShape.SegmentStart:
                    result = new Rect(rect.Left, rect.Top, rect.Width + (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Height + (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0)).ToSKRect();
                    break;
                case ExtendedElementShape.SegmentMid:
                    result = new Rect(rect.Left - (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0), rect.Width + (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance * 2 : 0), rect.Height + (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance * 2 : 0)).ToSKRect();
                    break;
                case ExtendedElementShape.SegmentEnd:
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

        static SKRect RectInset(SKRect rect, float inset) => RectInset(rect, inset, inset, inset, inset);

        static SKRect RectInset(SKRect rect, double left, double top, double right, double bottom) => RectInset(rect, (float)left, (float)top, (float)right, (float)bottom);

        static SKRect RectInset(SKRect rect, float left, float top, float right, float bottom) => new SKRect(rect.Left + left, rect.Top + top, rect.Right - right, rect.Bottom - bottom);

        internal static SKPath PerimeterPath(Forms9Patch.IShape element, SKRect rect, float radius)
        {
            radius = Math.Max(radius, 0);

            var materialButton = element as MaterialButton;
            //ExtendedElementShape type = materialButton == null ? ExtendedElementShape.Rectangle : materialButton.SegmentType;
            Xamarin.Forms.StackOrientation orientation = materialButton == null ? Xamarin.Forms.StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

            var path = new SKPath();

            var length = Math.Min(rect.Width, rect.Height);
            if (element.ExtendedElementShape == ExtendedElementShape.Square || element.ExtendedElementShape == ExtendedElementShape.Circle)
            {
                var location = new SKPoint(rect.MidX - length / 2f, rect.MidY - length / 2f);
                var size = new SKSize(length, length);
                //rect = new SKRect(rect.MidX - width / 2f, rect.MidY - width / 2f, width, width);
                rect = SKRect.Create(location, size);
            }
            else if (element.ExtendedElementShape == ExtendedElementShape.Obround)
                radius = length / 2f;
            radius = Math.Min(radius, length / 2f);

            var diameter = radius * 2;
            var topLeft = new SKRect(rect.Left, rect.Top, rect.Left + diameter, rect.Top + diameter);
            var bottomLeft = new SKRect(rect.Left, rect.Bottom - diameter, rect.Left + diameter, rect.Bottom);
            var bottomRight = new SKRect(rect.Right - diameter, rect.Bottom - diameter, rect.Right, rect.Bottom);
            var topRight = new SKRect(rect.Right - diameter, rect.Top, rect.Right, rect.Top + diameter);


            switch (element.ExtendedElementShape)
            {
                case ExtendedElementShape.Rectangle:
                case ExtendedElementShape.Square:
                case ExtendedElementShape.Obround:
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
                    break;
                case ExtendedElementShape.Elliptical:
                case ExtendedElementShape.Circle:
                    path.AddOval(rect);
                    break;
                /*
                case ExtendedElementShape.Obround:
                    {
                        if (rect.Width > rect.Height)
                        {
                            radius = (float)(rect.Height / 2.0);
                            var left = new SKRect(0, 0, rect.Height, rect.Height);
                            var right = new SKRect(rect.Width - rect.Height -1, 0, rect.Width -1, rect.Height -1);
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
                            var bottom = new SKRect(0, rect.Height - rect.Width - 1, rect.Width - 1, rect.Height - 1);
                            path.MoveTo(rect.Left, rect.Top + radius);
                            path.LineTo(rect.Left, rect.Bottom - radius);
                            path.ArcTo(bottom, 180, -180, false);
                            path.LineTo(rect.Right, rect.Top + radius);
                            path.ArcTo(top, 0, -180, false);
                        }
                    }
                    break;*/
                case ExtendedElementShape.SegmentStart:
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
                    break;
                case ExtendedElementShape.SegmentMid:
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
                    break;
                case ExtendedElementShape.SegmentEnd:
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
                    break;
                default:
                    throw new NotSupportedException("ExtendedElementShape [" + element.ExtendedElementShape + "] is not supported")
;            }
            return path;
        }

        #endregion
    }
}
