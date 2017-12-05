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

        bool _debugMessages = false;

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
            //if (MaterialButton!=null && MaterialButton.ParentSegmentsOrientation==Xamarin.Forms.StackOrientation.Vertical)
            //    System.Diagnostics.Debug.WriteLine("["+_roundedBoxElement.InstanceId+"]["+_roundedBoxElement.InstanceId+"][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName()+"] e.PropertyName=["+e.PropertyName+"]");
            //if (e.PropertyName==ShapeBase.IgnoreShapePropertiesChangesProperty.PropertyName)
            //    System.Diagnostics.Debug.WriteLine("            IgnoreChanges=["+IgnoreChanges+"]");
            if (!ValidLayout(CanvasSize))
                Invalidate();
            if (e.PropertyName == ShapeBase.BackgroundImageProperty.PropertyName)
                SetImageElement();
            else if (e.PropertyName == Forms9Patch.BubbleLayout.PointerAxialPositionProperty.PropertyName
                || e.PropertyName == Forms9Patch.BubbleLayout.PointerCornerRadiusProperty.PropertyName
                || e.PropertyName == Forms9Patch.BubbleLayout.PointerDirectionProperty.PropertyName
                //|| e.PropertyName == Forms9Patch.BubbleLayout.PointerLengthProperty.PropertyName  // Already handled by the change in location / paddiing
                || e.PropertyName == Forms9Patch.BubbleLayout.PointerTipRadiusProperty.PropertyName
                || e.PropertyName == Forms9Patch.BubbleLayout.PointerAngleProperty.PropertyName
                )
            {
                _validLayout = false;
                Invalidate();
            }

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
        //MaterialButton materialButton => _roundedBoxElement as MaterialButton;

        Xamarin.Forms.View View => _roundedBoxElement as Xamarin.Forms.View;

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
                    SetImageSourceAsync();
                }
            }
        }

        Xamarin.Forms.BindableObject BindableObject => _roundedBoxElement as Xamarin.Forms.BindableObject;

        double HeightRequest => ((Xamarin.Forms.VisualElement)_roundedBoxElement).HeightRequest;

        double WidthRequest => ((Xamarin.Forms.VisualElement)_roundedBoxElement).WidthRequest;

        Xamarin.Forms.Color BackgroundColor => (Xamarin.Forms.Color)BindableObject.GetValue(ShapeBase.BackgroundColorProperty);

        internal async Task SetImageSourceAsync()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "]ImageView.SetSourceAsync ENTER");
            if (_imageElement?.Source != _xfImageSource)
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
                    _sourceRangeLists = _sourceBitmap?.RangeLists;
                }

                if (_imageElement != null)
                {
                    if (_sourceBitmap == null || _sourceBitmap.SKBitmap == null)
                        _imageElement.SourceImageSize = Xamarin.Forms.Size.Zero;
                    else
                        _imageElement.SourceImageSize = new Xamarin.Forms.Size(_sourceBitmap.Width, _sourceBitmap.Height);
                }

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

        bool DrawImage => (_sourceBitmap?.SKBitmap != null && _sourceBitmap.Width > 0 && _sourceBitmap.Height > 0);

        bool DrawOutline => (OutlineColor.A > 0.01 && OutlineWidth > 0.05);

        bool DrawFill => BackgroundColor.A > 0.01;

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


        #region Xamarin.Forms Measuring
        internal Xamarin.Forms.SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (ImageElement == null)
                throw new InvalidCastException("DesiredSize is only valid with the Forms9Patch.Image element");
            var size = SourceImageSize();
            if (size.Width < 1 || size.Height < 1)
                return new Xamarin.Forms.SizeRequest(Xamarin.Forms.Size.Zero);

            var sourceAspect = size.Width / size.Height;

            /*
            if (WidthRequest > 0 && WidthRequest < widthConstraint)
                widthConstraint = WidthRequest;
            if (HeightRequest > 0 && HeightRequest < heightConstraint)
                heightConstraint = HeightRequest;

            bool constrainedWidth = !double.IsInfinity(widthConstraint);
            bool constrainedHeight = !double.IsInfinity(heightConstraint);

            var availWidth = constrainedWidth ? widthConstraint : 10000;
            var availHeight = constrainedHeight ? heightConstraint : 10000;

            var reqWidth = constrainedWidth ? availWidth :  size.Width;
            var reqHeight = constrainedHeight ? availWidth : size.Height;
            var minWidth = 1.0;
            var minHeight = 1.0;


            
            switch (ImageElement.Fill)
            {
                case Fill.AspectFill:
                    if (constrainedWidth && constrainedHeight)
                        reqWidth = availHeight * sourceAspect;
                    else if (constrainedWidth && !constrainedHeight)
                        reqHeight = availWidth / sourceAspect;
                    break;
                case Fill.AspectFit:
                    if (sourceAspect >= 1)
                        minWidth = sourceAspect;
                    else
                        minHeight = 1 / sourceAspect;

                    if (constrainedWidth && constrainedHeight)
                    {
                        var constraintAspect = availWidth / availHeight;
                        if (constraintAspect > sourceAspect)
                            reqWidth = availHeight * sourceAspect;
                        else
                            reqWidth = availHeight * constraintAspect;
                    }
                    else if (constrainedHeight)
                        reqWidth = availHeight * sourceAspect;
                    else if (constrainedWidth)
                        reqHeight = availWidth / sourceAspect;
                    break;
                //case Fill.None:
                //    reqWidth = size.Width;
                //    reqHeight = size.Height;
                //    minWidth = size.Width;
                //    minHeight = size.Height;
                //    break;
            }
            */

            var reqWidth = size.Width;
            var reqHeight = size.Height;
            var minWidth = (sourceAspect > 1 ? sourceAspect : 1);
            var minHeight = (sourceAspect > 1 ? 1 : 1 / sourceAspect);

            if (HasShadow && (DrawFill || DrawImage))
            {
                var shadowPadding = ShapeBase.ShadowPadding(_roundedBoxElement);
                reqWidth += shadowPadding.HorizontalThickness;
                reqHeight += shadowPadding.VerticalThickness;
                minWidth += shadowPadding.HorizontalThickness;
                minHeight += shadowPadding.VerticalThickness;
            }
            if (DrawOutline)
            {
                var outlineWidth = _roundedBoxElement.OutlineWidth;
                reqWidth += outlineWidth;
                reqHeight += outlineWidth;
                minWidth += outlineWidth;
                minHeight += outlineWidth;
            }
            var reqSize = new Xamarin.Forms.Size(reqWidth, reqHeight);
            var minSize = new Xamarin.Forms.Size(minWidth, minHeight);
            return new Xamarin.Forms.SizeRequest(reqSize, minSize);
        }
        #endregion


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
            if (_roundedBoxElement is MaterialButton materialButton && materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical)
                if (_debugMessages)
                    System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] element.Size=[" + ((Xamarin.Forms.VisualElement)_roundedBoxElement).Bounds.Size + "] Size=[" + Width + ", " + Height + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "]");
            Invalidate();
        }

        private void OnImageElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Xamarin.Forms.Image.SourceProperty.PropertyName)
                SetImageSourceAsync();
            else if (e.PropertyName == Forms9Patch.Image.TintColorProperty.PropertyName
                || e.PropertyName == Forms9Patch.Image.FillProperty.PropertyName
                || e.PropertyName == Forms9Patch.Image.CapInsetsProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.VisualElement.WidthRequestProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.VisualElement.HeightRequestProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.View.HorizontalOptionsProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.View.VerticalOptionsProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.View.MarginProperty.PropertyName
                || e.PropertyName == Forms9Patch.Image.AntiAliasProperty.PropertyName
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
            //if (materialButton != null && materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical) if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] availableSize=[" + availableSize + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "]");

            var result = base.MeasureOverride(availableSize);
            /*
            if (materialButton != null && materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical)
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
            */


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

                //if ((!constrainedWidth && !constrainedHeight) || ImageElement.Fill == Fill.None)
                //    result = new Windows.Foundation.Size(_sourceBitmap.Width / Display.Scale, _sourceBitmap.Height / Display.Scale);
                //else
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
                //if (materialButton != null && materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical) if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap.Size=[" + _sourceBitmap.Width + ", " + _sourceBitmap.Height + "]");
            }

            return result;
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            //if (materialButton != null && materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical) if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] availableSize=[" + finalSize + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "]");

            var result = base.ArrangeOverride(finalSize);

            if (_roundedBoxElement is Forms9Patch.Image && !_actualSizeValid && _sourceBitmap != null && Children.Count > 0)
                Invalidate();

            /*
            if (materialButton != null && materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical)
            {
                if (_debugMessages)
                {
                    if (_sourceBitmap != null)
                        System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap.Size=[" + _sourceBitmap.Width + ", " + _sourceBitmap.Height + "]");
                    else
                        System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap is null");
                }
            }
            */

            return result;
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

            if (_roundedBoxElement is MaterialSegmentedControl materialSegmentedControl)
            {
                // do nothing
            }
            else
            {
                var hz = true;
                var materialButton = _roundedBoxElement as MaterialButton;
                if (materialButton!=null)
                    hz = materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Horizontal;
                var vt = !hz;

                var backgroundColor = BackgroundColor;
                var hasShadow = HasShadow;
                var shadowInverted = ShadowInverted;
                var outlineWidth = OutlineWidth;
                var outlineRadius = OutlineRadius;
                var outlineColor = OutlineColor;
                var elementShape = ExtendedElementShape;

                double separatorWidth = materialButton == null || elementShape == ExtendedElementShape.Rectangle ? 0 : materialButton.SeparatorWidth < 0 ? outlineWidth : Math.Max(0, materialButton.SeparatorWidth);

                bool drawOutline =DrawOutline;
                bool drawImage = DrawImage;
                bool drawSeparators = outlineColor.A > 0.01 && materialButton != null && separatorWidth > 0.01;
                bool drawFill = DrawFill;

                if ((drawFill || drawOutline || drawSeparators || drawImage) && CanvasSize != default(SKSize))
                {

                        if (materialButton != null && materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical)  if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "][" + GetType() + "." + PCL.Utils.ReflectionExtensions.CallerMemberName() + "]  Parent.Size=[" + ((FrameworkElement)Parent).ActualWidth + ","+ ((FrameworkElement)Parent).ActualHeight + "]");

                    SKRect rect = new SKRect(0, 0, e.Info.Width, e.Info.Height);
                    if (Parent is FrameworkElement parent)
                    {
                        rect = new SKRect(0, 0, (float)(parent.ActualWidth * Display.Scale), (float)(parent.ActualHeight * Display.Scale));
                    }

                        var makeRoomForShadow = hasShadow && (backgroundColor.A > 0.01 || drawImage); // && !_roundedBoxElement.ShadowInverted;
                        var shadowX = (float)(Forms9Patch.Settings.ShadowOffset.X * Display.Scale);
                        var shadowY = (float)(Forms9Patch.Settings.ShadowOffset.Y * Display.Scale);
                        var shadowR = (float)(Forms9Patch.Settings.ShadowRadius * Display.Scale);
                    var shadowColor = Xamarin.Forms.Color.FromRgba(0.0, 0.0, 0.0, 0.75).ToSKColor(); //  .ToWindowsColor().ToSKColor();
                        var shadowPadding = ShapeBase.ShadowPadding(_roundedBoxElement, hasShadow, true);
                       

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

                                if (DrawFill)// || (DrawImage && ImageElement.Fill!=Fill.AspectFit && ExtendedElementShape!=ExtendedElementShape.Rectangle))
                                {
                                    var path = PerimeterPath(_roundedBoxElement, shadowRect, outlineRadius - (drawOutline ? outlineWidth : 0));
                                    canvas.DrawPath(path, shadowPaint);
                                }
                                else if (DrawImage)
                                {
                                    var path = PerimeterPath(_roundedBoxElement, shadowRect, outlineRadius - (drawOutline ? outlineWidth : 0));
                                    GenerateImageLayout(canvas, perimeter, path, shadowPaint);
                                }
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
                                    //Color = backgroundColor.ToWindowsColor().ToSKColor(),
                                    Color = backgroundColor.ToSKColor(),
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
                                //Color = outlineColor.ToWindowsColor().ToSKColor(),
                                Color = outlineColor.ToSKColor(),
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
                                //Color = outlineColor.ToWindowsColor().ToSKColor(),
                                Color = outlineColor.ToSKColor(),
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
            base.OnPaintSurface(e);

            _actualSizeValid = true;


        }
        #endregion


        #region Image Layout
        void GenerateImageLayout(SKCanvas canvas, SKRect fillRect, SKPath clipPath, SKPaint shadowPaint=null)
        {
            SKBitmap shadowBitmap = null;
            SKCanvas workingCanvas = canvas;

            if (shadowPaint != null)
            {
                var x = canvas.DeviceClipBounds;
                shadowBitmap = new SKBitmap((int)x.Width, (int)x.Height);
                workingCanvas = new SKCanvas(shadowBitmap);
                workingCanvas.Clear();
            }
            
            if (_imageElement == null || _sourceBitmap?.SKBitmap == null || _sourceBitmap.Width < 1 || _sourceBitmap.Height < 1)
                return;

            //if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "]["+GetType()+"."+PCL.Utils.ReflectionExtensions.CallerMemberName()+"]  Fill=[" + _imageElement.Fill + "] W,H=[" + Width + "," + Height + "] ActualWH=[" + ActualWidth + "," + ActualHeight + "] ");
            workingCanvas.Save();
            if (clipPath!=null)
                workingCanvas.ClipPath(clipPath);

            var rangeLists = _imageElement.CapInsets.ToRangeLists(_sourceBitmap.SKBitmap.Width, _sourceBitmap.SKBitmap.Height, _xfImageSource, _sourceRangeLists != null);
            if (rangeLists==null)
                rangeLists = _sourceRangeLists;
            /*
            if (rangeLists != null)
            {
                if (rangeLists.PatchesX.Count == 1 && rangeLists.PatchesX[0].Start <= 0 && rangeLists.PatchesX[0].End >= _sourceBitmap.SKBitmap.Width-1)
                    rangeLists.PatchesX.Clear();
                if (rangeLists.PatchesY.Count == 1 && rangeLists.PatchesY[0].Start <= 0 && rangeLists.PatchesY[0].End >= _sourceBitmap.SKBitmap.Height-1)
                    rangeLists.PatchesY.Clear();
                if (rangeLists.PatchesX.Count == 0 && rangeLists.PatchesY.Count == 0)
                    rangeLists = null;
            }
            */
                    
            var bitmap = _sourceBitmap;
            SKPaint paint = null;
            if (shadowPaint==null && _imageElement.TintColor!=Xamarin.Forms.Color.Default && _imageElement.TintColor!=Xamarin.Forms.Color.Transparent)
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
                        workingCanvas.DrawBitmap(_sourceBitmap.SKBitmap, x, y, paint);
            }
            else if (rangeLists == null)
            {
                var sourceRect = new SKRect(_sourceBitmap.SKBitmap.Info.Rect.Left, _sourceBitmap.SKBitmap.Info.Rect.Top, _sourceBitmap.SKBitmap.Info.Rect.Right, _sourceBitmap.SKBitmap.Info.Rect.Bottom);
                var destRect = fillRect;
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
                    sourceRect = SKRect.Create(left, top, (float)croppedWidth, (float)croppedHeight);
                }
                else if (_imageElement.Fill == Fill.AspectFit)
                {

                    //var destWidth = _imageElement.HorizontalOptions.Alignment != Xamarin.Forms.LayoutAlignment.Fill ? (bitmapAspect > fillRectAspect ? fillRect.Width : fillRect.Height * bitmapAspect) : fillRect.Width;
                    var destWidth = (bitmapAspect > fillRectAspect ? fillRect.Width : fillRect.Height * bitmapAspect);
                    //var destHeight = _imageElement.VerticalOptions.Alignment != Xamarin.Forms.LayoutAlignment.Fill ? (bitmapAspect > fillRectAspect ? fillRect.Width / bitmapAspect : fillRect.Height) : fillRect.Height;
                    var destHeight = (bitmapAspect > fillRectAspect ? fillRect.Width / bitmapAspect : fillRect.Height);

                    float left = fillRect.MidX - (float)destWidth / 2f, top = fillRect.MidY - (float)destHeight / 2f;
                    destRect = SKRect.Create(left, top, (float)destWidth, (float)destHeight);
                }
                //else Fill==Fill.Fill

                if (_imageElement.AntiAlias && (destRect.Width > sourceRect.Width || destRect.Height > sourceRect.Height))
                {
                    var croppedBitmap = new SKBitmap((int)sourceRect.Width, (int)sourceRect.Height);
                    var destBitmap = new SKBitmap((int)destRect.Width, (int)destRect.Height);
                    _sourceBitmap.SKBitmap.ExtractSubset(croppedBitmap, new SKRectI((int)sourceRect.Left, (int)sourceRect.Top, (int)sourceRect.Right, (int)sourceRect.Bottom));
                    SKBitmap.Resize(destBitmap, croppedBitmap, SKBitmapResizeMethod.Lanczos3);
                    workingCanvas.DrawBitmap(destBitmap, destBitmap.Info.Rect, destRect, paint);
                }
                else
                {
                    workingCanvas.DrawBitmap(_sourceBitmap.SKBitmap, sourceRect, destRect, paint);
                }
            }
            else
            {
                var lattice = rangeLists.ToSKLattice(_sourceBitmap.SKBitmap);
                //System.Diagnostics.Debug.WriteLine("lattice.x: ["+lattice.XDivs[0]+","+lattice.XDivs[1]+"] lattice.y: ["+lattice.YDivs[0]+","+lattice.YDivs[1]+"] lattice.Bounds=["+lattice.Bounds+"] lattice.Flags:" + lattice.Flags);
                workingCanvas.DrawBitmapLattice(_sourceBitmap.SKBitmap, lattice, fillRect);
            }

            workingCanvas.Restore();

            if (shadowPaint != null)
            {
                paint = shadowPaint;
                canvas.DrawBitmap(shadowBitmap, shadowBitmap.Info.Rect, paint);
            }
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

            if (element is BubbleLayout bubble && bubble.PointerDirection != PointerDirection.None)
                return BubblePerimeterPath(bubble, rect, radius);
            
            var materialButton = element as MaterialButton;
            Xamarin.Forms.StackOrientation orientation = materialButton == null ? Xamarin.Forms.StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

            var path = new SKPath();

            var length = Math.Min(rect.Width, rect.Height);
            if (element.ExtendedElementShape == ExtendedElementShape.Square || element.ExtendedElementShape == ExtendedElementShape.Circle)
            {
                var location = new SKPoint(rect.MidX - length / 2f, rect.MidY - length / 2f);
                var size = new SKSize(length, length);
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
                            path.ArcTo(topLeft, 270, -90, false);
                        path.LineTo(rect.Left, rect.Bottom - radius);
                        if (radius > 0)
                            path.ArcTo(bottomLeft, 180, -90, false);
                        path.LineTo(rect.Right - radius, rect.Bottom);
                        if (radius > 0)
                            path.ArcTo(bottomRight, 90, -90, false);
                        path.LineTo(rect.Right, rect.Top + radius);
                        if (radius > 0)
                            path.ArcTo(topRight, 0, -90, false);
                        path.LineTo((rect.Left + rect.Right) / 2, rect.Top);
                    }
                    break;
                case ExtendedElementShape.Elliptical:
                case ExtendedElementShape.Circle:
                    //path.AddOval(rect);
                    path.AddOval(rect, SKPathDirection.CounterClockwise);
                    break;
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

        internal static SKPath BubblePerimeterPath(BubbleLayout bubble, SKRect rect, float radius)
        {
            var length = bubble.PointerLength * Display.Scale;

            if (radius * 2 > rect.Height - (bubble.PointerDirection.IsVertical() ? length : 0))
                radius = (float)((rect.Height - (bubble.PointerDirection.IsVertical() ? length : 0)) / 2.0);
            if (radius * 2 > rect.Width - (bubble.PointerDirection.IsHorizontal() ? length : 0))
                radius = (float)((rect.Width - (bubble.PointerDirection.IsHorizontal() ? length : 0)) / 2.0);

            var filetRadius = bubble.PointerCornerRadius;
            var tipRadius = bubble.PointerTipRadius * Display.Scale;

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



            var result = new SKPath();
            var position = bubble.PointerAxialPosition;
            if (position <= 1.0)
            {
                if (bubble.PointerDirection == PointerDirection.Down || bubble.PointerDirection == PointerDirection.Up)
                    position = rect.Width * position;
                else
                    position = rect.Height * position;
            }
            var left = rect.Left;
            var right = rect.Right;
            var top = rect.Top;
            var bottom = rect.Bottom;

            const float sqrt3 = (float)1.732050807568877;
            const float sqrt3d2 = (float)0.86602540378444;
            //const float rad60 = (float)(Math.PI / 3.0);
            //const float rad90 = (float)(Math.PI / 2.0);
            //const float rad30 = (float)(Math.PI / 6.0);

           /*
            var pointerAngle = bubble.PointerAngle * Math.PI / 180;
            var tipRadiusWidth = 2 * tipRadius * Math.Sin((Math.PI - pointerAngle) / 2);
            var tipRadiusHeight = tipRadius * (1 - Math.Cos((Math.PI - pointerAngle) / 2) );
            var tipC1 = tipRadiusHeight / Math.Tan(pointerAngle / 2);
            var tipC0 = tipRadiusWidth / 2 - tipC1;
            var tipProjection = tipC0 * Math.Tan(pointerAngle / 2);
            */

            float tipCornerHalfWidth = tipRadius * sqrt3d2;
            float pointerToCornerIntercept = (float)Math.Sqrt((2 * radius * Math.Sin(Math.PI / 12.0)) * (2 * radius * Math.Sin(Math.PI / 12.0)) - (radius * radius / 4.0));

            float pointerAtLimitSansTipHalfWidth = (float)(pointerToCornerIntercept + radius / (2.0 * sqrt3) + (length - tipRadius / 2.0) / sqrt3);
            float pointerAtLimitHalfWidth = pointerAtLimitSansTipHalfWidth + tipRadius * sqrt3d2;

            float pointerSansFiletHalfWidth = (float)(tipCornerHalfWidth + (length - filetRadius / 2.0 - tipRadius / 2.0) / sqrt3);
            float pointerFiletWidth = filetRadius * sqrt3d2;
            float pointerAndFiletHalfWidth = pointerSansFiletHalfWidth + pointerFiletWidth;

            int dir = 1;

            if (bubble.PointerDirection.IsHorizontal())
            {
                float start = left;
                float end = right;
                if (bubble.PointerDirection == PointerDirection.Right)
                {
                    dir = -1;
                    start = right;
                    end = left;
                }
                float baseX = start + dir * length;

                float tipY = position;
                if (tipY > rect.Height - pointerAtLimitHalfWidth)
                    tipY = rect.Height - pointerAtLimitHalfWidth;
                if (tipY < pointerAtLimitHalfWidth)
                    tipY = pointerAtLimitHalfWidth;
                if (rect.Height <= 2 * pointerAtLimitHalfWidth)
                    tipY = (float)(rect.Height / 2.0);

                result.MoveTo(start + dir * (length + radius), top);
                result.ArcTo(end, top, end, bottom, radius);
                result.ArcTo(end, bottom, start, bottom, radius);

                // bottom half
                if (tipY >= rect.Height - pointerAndFiletHalfWidth - radius)
                {
                    result.LineTo(start + dir * (length + radius), bottom);
                    float endRatio = (rect.Height - tipY) / (pointerAndFiletHalfWidth + radius);
                    //System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "A endRatio=[" + endRatio + "]");
                    result.CubicTo(
                        start + dir * (length + radius - endRatio * 4 * radius / 3.0f), bottom,
                        start + dir * (length - filetRadius / 2.0f + filetRadius * sqrt3d2), top + tipY + pointerSansFiletHalfWidth + filetRadius / 2.0f,
                        start + dir * (length - filetRadius / 2.0f), top + tipY + pointerSansFiletHalfWidth);
                }
                else
                {
                    //result.ArcWithCenterTo(start + dir * (length + radius), bottom - radius, radius, 90, dir * 90);
                    result.ArcTo(baseX, bottom, baseX, top, radius);

                    //result.LineTo(
                    //    start + dir * length,
                    //    top + tip + pointerAndFiletHalfWidth
                    //);
                    //result.AddRelativeArc(
                    //    start + dir * (length - filetRadius),
                    //    top + tip + pointerAndFiletHalfWidth,
                    //    filetRadius, rad90 - dir * rad90, dir * -rad60);
                    result.ArcWithCenterTo(start + dir * (length - filetRadius), top + tipY + pointerAndFiletHalfWidth, filetRadius, 90 - 90 * dir, dir * -60);
                    //result.ArcTo(baseX, top + tipY + pointerAndFiletHalfWidth, start, top + tipY, filetRadius);
                }

                //tip

                //result.AddLineToPoint(
                //    (float)(start + dir * tipRadius / 2.0),
                //    top + tip + tipCornerHalfWidth
                //);
                //result.AddRelativeArc(
                //    start + dir * tipRadius,
                //    top + tip,
                //    tipRadius,
                //    rad90 + dir * rad30,
                //    dir * 2 * rad60);
                result.ArcWithCenterTo(start + dir * tipRadius, top + tipY, tipRadius, 90 + dir * 30, dir * 2 * 60);


                // top half
                if (tipY <= pointerAndFiletHalfWidth + radius)
                {
                    var startRatio = tipY / (pointerAndFiletHalfWidth + radius);
                    //result.AddLineToPoint(
                    //    (float)(start + dir * (length - filetRadius / 2.0)),
                    //    top + tip - pointerSansFiletHalfWidth
                    //);
                    //System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "C startRatio=[" + startRatio + "]");
                    //result.AddCurveToPoint(
                    //    new CGPoint(start + dir * (length - filetRadius / 2.0 + filetRadius * sqrt3d2), top + tip - pointerSansFiletHalfWidth - filetRadius / 2.0),
                    //    new CGPoint(start + dir * (length + radius - startRatio * 4 * radius / 3.0), top),
                    //    new CGPoint(start + dir * (length + radius), top)
                    //);
                    result.CubicTo(
                        start + dir * (length - filetRadius / 2.0f + filetRadius * sqrt3d2), top + tipY - pointerSansFiletHalfWidth - filetRadius / 2.0f, 
                        start + dir * (length + radius - startRatio * 4 * radius / 3.0f), top, 
                        start + dir * (length + radius), top);
                }
                else
                {
                    //result.AddLineToPoint(
                    //    (float)(start + dir * (length - filetRadius / 2.0)),
                    //    top + tip - pointerSansFiletHalfWidth
                    //);
                    //System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "D");
                    //result.AddRelativeArc(
                    //    start + dir * (length - filetRadius),
                    //    top + tip - pointerAndFiletHalfWidth,
                    //    filetRadius,
                    //    rad90 - dir * rad30,
                    //    dir * -rad60);
                    result.ArcWithCenterTo(start + dir * (length - filetRadius), top + tipY - pointerAndFiletHalfWidth, filetRadius, 90 - dir * 30, dir * -60);

                    //result.AddLineToPoint(
                    //    start + dir * length,
                    //    top + radius
                    //);
                    //result.AddRelativeArc(
                    //    start + dir * (length + radius),
                    //    top + radius,
                    //    radius,
                    //    rad90 + dir * rad90,
                    //    dir * rad90);
                    result.ArcWithCenterTo(start + dir * (length + radius), top + radius, radius, 90 + dir * 90, dir * 90);
                }
                if (dir > 0)
                {
                    var reverse = new SKPath();
                    reverse.AddPathReverse(result);
                    return reverse;
                }
            }
            else
            {
                float start = top;
                float end = bottom;
                if (bubble.PointerDirection == PointerDirection.Down)
                {
                    dir = -1;
                    start = bottom;
                    end = top;
                }
                float tip = position;
                if (tip > rect.Width - pointerAtLimitHalfWidth)
                    tip = rect.Width - pointerAtLimitHalfWidth;
                if (tip < pointerAtLimitHalfWidth)
                    tip = pointerAtLimitHalfWidth;
                if (rect.Width <= 2 * pointerAtLimitHalfWidth)
                    tip = (float)(rect.Width / 2.0);
                result.MoveTo(left, start + dir * (length + radius));
                result.ArcTo(left, end, right, end, radius);
                result.ArcTo(right, end, right, start, radius);

                // right half
                if (tip > rect.Width - pointerAndFiletHalfWidth - radius)
                {
                    var endRatio = (rect.Width - tip) / (pointerAndFiletHalfWidth + radius);
                    //result.AddLineToPoint(right, start + dir * (radius + length));
                    //result.AddCurveToPoint(
                    //    new CGPoint(right, start + dir * (length + radius - endRatio * 4 * radius / 3.0)),
                    //    new CGPoint(left + tip + pointerSansFiletHalfWidth + filetRadius / 2.0, start + dir * (length - filetRadius / 2.0 + filetRadius * sqrt3d2)),
                    //    new CGPoint(left + tip + pointerSansFiletHalfWidth, start + dir * (length - filetRadius / 2.0))
                    //);
                    result.CubicTo(
                        right, start + dir * (length + radius - endRatio * 4 * radius / 3.0f),
                        left + tip + pointerSansFiletHalfWidth + filetRadius / 2.0f, start + dir * (length - filetRadius / 2.0f + filetRadius * sqrt3d2),
                        left + tip + pointerSansFiletHalfWidth, start + dir * (length - filetRadius / 2.0f)
                        );
                }
                else
                {
                    //result.AddLineToPoint(right, start + dir * (radius + length));
                    //result.AddRelativeArc(
                    //    right - radius,
                    //    start + dir * (length + radius),
                    //    radius, 0, dir * -rad90);
                    result.ArcWithCenterTo(
                        right - radius,
                        start + dir * (length + radius),
                        radius, 0, dir * -90
                        );
                    //result.AddLineToPoint(
                    //    left + tip + pointerAndFiletHalfWidth,
                    //    start + dir * length
                    //);
                    //result.AddRelativeArc(
                    //    left + tip + pointerAndFiletHalfWidth,
                    //    start + dir * (length - filetRadius),
                    //    filetRadius, dir * rad90, dir * rad60);
                    result.ArcWithCenterTo(
                        left + tip + pointerAndFiletHalfWidth,
                        start + dir * (length - filetRadius),
                        filetRadius, dir * 90, dir * 60
                        );
                }

                //tip
                /*
                result.AddLineToPoint(
                    left + tip + tipCornerHalfWidth,
                    (float)(start + dir * tipRadius / 2.0)
                );
                result.AddRelativeArc(
                    left + tip,
                    start + dir * tipRadius,
                    tipRadius,
                    dir * -rad30,
                    dir * -2 * rad60
                    );
                    */
                result.ArcWithCenterTo(
                    left + tip,
                    start + dir * tipRadius,
                    tipRadius,
                    dir * -30,
                    dir * -2 * 60
                    );


                // left half
                if (tip < pointerAndFiletHalfWidth + radius)
                {
                    var startRatio = tip / (pointerAndFiletHalfWidth + radius);
                    /*
                    result.AddLineToPoint(
                        left + tip - pointerSansFiletHalfWidth,
                        (float)(start + dir * (length - filetRadius / 2.0))
                    );
                    result.AddCurveToPoint(
                        new CGPoint(
                            left + tip - pointerSansFiletHalfWidth - filetRadius / 2.0,
                            start + dir * (length - filetRadius / 2.0 + filetRadius * sqrt3d2)
                        ),
                        new CGPoint(
                            left,
                            start + dir * (length + radius - startRatio * 4 * radius / 3.0)
                        ),
                        new CGPoint(
                            left,
                            start + dir * (length + radius)
                        )
                    );
                    */
                    result.CubicTo(
                            left + tip - pointerSansFiletHalfWidth - filetRadius / 2.0f,
                            start + dir * (length - filetRadius / 2.0f + filetRadius * sqrt3d2),
                            left,
                            start + dir * (length + radius - startRatio * 4 * radius / 3.0f),
                            left,
                            start + dir * (length + radius)
                        );
                }
                else
                {
                    /*
                    result.AddLineToPoint(
                        left + tip - pointerSansFiletHalfWidth,
                        (float)(start + dir * (length - filetRadius / 2.0))
                    );
                    result.AddRelativeArc(
                        left + tip - pointerAndFiletHalfWidth,
                        start + dir * (length - filetRadius),
                        filetRadius,
                        dir * rad30,
                        dir * rad60
                        );
                        */
                    result.ArcWithCenterTo(
                        left + tip - pointerAndFiletHalfWidth,
                        start + dir * (length - filetRadius),
                        filetRadius,
                        dir * 30,
                        dir * 60
                        );
                    /*
                    result.AddLineToPoint(
                        left + radius,
                        start + dir * length
                    );
                    result.AddRelativeArc(
                        left + radius,
                        start + dir * (length + radius),
                        radius,
                        dir * -rad90,
                        dir * -rad90
                        );
                        */
                    result.ArcWithCenterTo(
                        left + radius,
                        start + dir * (length + radius),
                        radius,
                        dir * -90,
                        dir * -90
                        );
                }
                if (dir < 0)
                {
                    var reverse = new SKPath();
                    reverse.AddPathReverse(result);
                    return reverse;
                }
            }
            return result;
        }
        #endregion
    }
}
