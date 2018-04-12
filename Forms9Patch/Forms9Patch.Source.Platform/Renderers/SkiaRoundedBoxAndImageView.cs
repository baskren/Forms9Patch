using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using System.ComponentModel;
using System.Diagnostics;

#if __IOS__
using SkiaSharp.Views.iOS;
using UIKit;
using Foundation;
using System.Runtime.CompilerServices;
using CoreGraphics;
using P42.Utils;
using SkiaSharp.Extended.Svg;
using Xamarin.Forms;

namespace Forms9Patch.iOS
{
    public class SkiaRoundedBoxAndImageView : SKCanvasView, IDisposable

#elif __DROID__
using Android.Runtime;
using Android.Views;
using SkiaSharp.Views.Android;
using Android.Sax;
using Xamarin.Forms;

namespace Forms9Patch.Droid
{
    public class SkiaRoundedBoxAndImageView : SKCanvasView, IDisposable

#elif WINDOWS_UWP

using SkiaSharp.Views.UWP;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
namespace Forms9Patch.UWP
{
    public class SkiaRoundedBoxAndImageView : SKXamlCanvas, IDisposable

#endif
    {

#pragma warning disable CS0649
        bool _debugMessages;
#pragma warning restore CS0649

        #region Constructor
#if __DROID__  // needed for NativeGestureHandler?
        public SkiaRoundedBoxAndImageView() : base(Settings.Context) { }

        public SkiaRoundedBoxAndImageView(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer) { }

        public SkiaRoundedBoxAndImageView(Android.Content.Context context, Android.Util.IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

        public SkiaRoundedBoxAndImageView(Android.Content.Context context, Android.Util.IAttributeSet attrs) : base(context, attrs) { }
#endif

        internal SkiaRoundedBoxAndImageView(IShape roundedBoxElement)
#if __DROID__
            : base(Settings.Context)
#endif
        {
            _instanceId = roundedBoxElement.InstanceId;
            _roundedBoxElement = roundedBoxElement;
#if __IOS__ || __DROID__
            PaintSurface += OnPaintSurface;
#endif
#if __IOS__
            BackgroundColor = UIColor.Clear;
#endif
            if (_roundedBoxElement is Xamarin.Forms.VisualElement element)
            {
                element.PropertyChanged += OnShapeElementPropertyChanged;
                //element.SizeChanged += OnElementSizeChanged;
            }

            SetImageElement();
        }

        void SetImageElement()
        {
            if (_roundedBoxElement is ILayout layout)
                ImageElement = layout.BackgroundImage;
            else if (_roundedBoxElement is Image image)
                ImageElement = image;
        }
        #endregion


        #region IDisposable Support
        private bool _disposed; // To detect redundant calls

#if __IOS__ || __DROID__
        protected override void Dispose(bool disposing)
#else
protected virtual void Dispose(bool disposing)
#endif
        {
            if (!_disposed)
            {
                if (disposing)
                {
#if __IOS__ || __DROID__
                    PaintSurface -= OnPaintSurface;
#endif

                    _xfImageSource?.ReleaseF9PBitmap(this);
                    _f9pImageData = null;
                    if (_roundedBoxElement is Xamarin.Forms.VisualElement element)
                    {
                        element.PropertyChanged -= OnShapeElementPropertyChanged;
                        //element.SizeChanged -= OnElementSizeChanged;
                    }
                }
                _disposed = true;
            }
#if __IOS__
            base.Dispose(disposing);
#endif
        }

#if WINDOWS_UWP
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
#endif

        #endregion


        #region Properties
        //Button materialButton => _roundedBoxElement as Button;

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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    SetImageSourceAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }

        Xamarin.Forms.BindableObject BindableObject => _roundedBoxElement as Xamarin.Forms.BindableObject;

        double HeightRequest => ((Xamarin.Forms.VisualElement)_roundedBoxElement).HeightRequest;

        double WidthRequest => ((Xamarin.Forms.VisualElement)_roundedBoxElement).WidthRequest;

        Xamarin.Forms.Color ElementBackgroundColor => (Xamarin.Forms.Color)BindableObject.GetValue(ShapeBase.BackgroundColorProperty);

        internal async Task SetImageSourceAsync()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "]ImageView.SetSourceAsync ENTER");
            if (_imageElement?.Source != _xfImageSource)
            {
                // release the previous
                _xfImageSource?.ReleaseF9PBitmap(this);
                _f9pImageData = null;
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
                if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "][" + GetType() + "][" + P42.Utils.ReflectionExtensions.CallerMemberName() + "] A[" + stopWatch.ElapsedMilliseconds + "]");
                stopWatch.Reset();
                stopWatch.Start();

                if (_xfImageSource != null)
                {
                    _f9pImageData = await _xfImageSource.FetchF9pImageData(this);
                    _sourceRangeLists = _f9pImageData?.RangeLists;
                }

                if (_imageElement != null)
                {
                    if (_f9pImageData == null || _f9pImageData.SKBitmap == null)
                        _imageElement.SourceImageSize = Xamarin.Forms.Size.Zero;
                    else
                        _imageElement.SourceImageSize = new Xamarin.Forms.Size(_f9pImageData.Width, _f9pImageData.Height);
                }

                stopWatch.Stop();
                if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "][" + GetType() + "][" + P42.Utils.ReflectionExtensions.CallerMemberName() + "] B[" + stopWatch.ElapsedMilliseconds + "]");

                _actualSizeValid = false;
                _validLayout = false;
                InvalidateView();
                ((Xamarin.Forms.IImageController)_imageElement)?.SetIsLoading(false);
            }
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "]ImageView.SetSourceAsync EXIT");
        }

        bool HasShadow => (bool)BindableObject.GetValue(ShapeBase.HasShadowProperty);

        bool ShadowInverted => (bool)BindableObject.GetValue(ShapeBase.ShadowInvertedProperty);

        bool DrawImage => (_imageElement != null && _imageElement.Opacity > 0 && _f9pImageData != null && _f9pImageData.ValidImage);

        bool DrawOutline => (OutlineColor.A > 0.01 && OutlineWidth > 0.05);

        bool DrawFill => ElementBackgroundColor.A > 0.01;

        Xamarin.Forms.Color OutlineColor => (Xamarin.Forms.Color)BindableObject.GetValue(ShapeBase.OutlineColorProperty);

        float OutlineRadius => FormsGestures.Display.Scale * (float)BindableObject.GetValue(ShapeBase.OutlineRadiusProperty);

        float OutlineWidth => FormsGestures.Display.Scale * (float)BindableObject.GetValue(ShapeBase.OutlineWidthProperty);

        ExtendedElementShape ExtendedElementShape => (ExtendedElementShape)BindableObject.GetValue(ShapeBase.ExtendedElementShapeProperty);

        Xamarin.Forms.Thickness ShadowPadding => _roundedBoxElement.ShadowPadding();

        internal Xamarin.Forms.Size SourceImageSize()
        {
            if (_f9pImageData != null && _f9pImageData.ValidImage)
                return new Xamarin.Forms.Size(_f9pImageData.Width / FormsGestures.Display.Scale, _f9pImageData.Height / FormsGestures.Display.Scale);
            return Xamarin.Forms.Size.Zero;
        }

        #endregion


        #region Keyboard

#if __IOS__

#elif __DROID__

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            System.Diagnostics.Debug.WriteLine("\tkeyCode=" + keyCode.ToString(), " \te.Action=" + e.Action + " \te.IsAltPressed=" + e.IsAltPressed + " \te.IsCtrlPressed=" + e.IsCtrlPressed + "\te.IsFunctionPressed=" + e.IsFunctionPressed + " \te.IsPressed" + e.IsShiftPressed + " \te.IsCapsLockOn=" + e.IsCapsLockOn);

            return base.OnKeyUp(keyCode, e);
        }

#elif WINDOWS_UWP


#endif

        #endregion


        #region Platform Specific

#if __IOS__
        void InvalidateView()
        {
            if (!_disposed)
                SetNeedsDisplay();
        }

        double ViewOpacity
        {
            get => Alpha;
            set => Alpha = (nfloat)value;
        }
#elif __DROID__
        double ViewOpacity
        {
            get => Alpha;
            set => Alpha = (float)value;
        }

        void InvalidateView()
        {
            if (!_disposed)
                Invalidate();
        }


#elif WINDOWS_UWP
        double ViewOpacity
        {
            get => Opacity;
            set => Opacity = value; 
        }

        void InvalidateView()
        {
        if (!_disposed)
            Invalidate();
        }
#else
#endif
        #endregion


        #region Property Change Management

        private void OnShapeElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_disposed)
                return;
            //if (Button!=null && Button.ParentSegmentsOrientation==Xamarin.Forms.StackOrientation.Vertical)
            //    System.Diagnostics.Debug.WriteLine("["+_roundedBoxElement.InstanceId+"]["+_roundedBoxElement.InstanceId+"][" + GetType() + "][" + P42.Utils.ReflectionExtensions.CallerMemberName()+"] e.PropertyName=["+e.PropertyName+"]");
            //if (e.PropertyName==ShapeBase.IgnoreShapePropertiesChangesProperty.PropertyName)
            //    System.Diagnostics.Debug.WriteLine("            IgnoreChanges=["+IgnoreChanges+"]");
            if (!ValidLayout(CanvasSize))
                InvalidateView();
            if (e.PropertyName == ShapeBase.BackgroundImageProperty.PropertyName)
                SetImageElement();
            else if (e.PropertyName == Forms9Patch.BubbleLayout.PointerAxialPositionProperty.PropertyName
                || e.PropertyName == Forms9Patch.BubbleLayout.PointerCornerRadiusProperty.PropertyName
                || e.PropertyName == Forms9Patch.BubbleLayout.PointerDirectionProperty.PropertyName
                //|| e.PropertyName == Forms9Patch.BubbleLayout.PointerLengthProperty.PropertyName  // Already handled by the change in location / paddiing
                || e.PropertyName == Forms9Patch.BubbleLayout.PointerTipRadiusProperty.PropertyName
                || e.PropertyName == Forms9Patch.BubbleLayout.PointerAngleProperty.PropertyName
                     || e.PropertyName == Forms9Patch.Button.SeparatorWidthProperty.PropertyName
                     || e.PropertyName == Forms9Patch.Button.HasShadowProperty.PropertyName
                     || e.PropertyName == ShapeBase.BackgroundColorProperty.PropertyName
                     || e.PropertyName == ShapeBase.HasShadowProperty.PropertyName
                     || e.PropertyName == ShapeBase.ShadowInvertedProperty.PropertyName
                     || e.PropertyName == ShapeBase.OutlineColorProperty.PropertyName
                     || e.PropertyName == ShapeBase.OutlineRadiusProperty.PropertyName
                     || e.PropertyName == ShapeBase.OutlineWidthProperty.PropertyName
                     || e.PropertyName == ShapeBase.ExtendedElementShapeProperty.PropertyName
                )
            {
                _validLayout = false;
                InvalidateView();
            }
            else if (e.PropertyName == Xamarin.Forms.VisualElement.OpacityProperty.PropertyName && _roundedBoxElement != null)
                ViewOpacity = View.Opacity;

        }

        private void OnImageElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_disposed)
                return;

            if (e.PropertyName == Xamarin.Forms.Image.SourceProperty.PropertyName)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                SetImageSourceAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
                InvalidateView();
            }
            else if (e.PropertyName == Xamarin.Forms.VisualElement.OpacityProperty.PropertyName && ImageElement != null)
            {
                _validLayout = false;
                InvalidateView();
            }
        }

        #endregion


        #region Xamarin.Forms Measuring
        internal Xamarin.Forms.SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (_disposed)
                return new Xamarin.Forms.SizeRequest(Xamarin.Forms.Size.Zero);
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

#if __DROID__
            reqWidth *= Forms9Patch.Display.Scale;
            reqHeight *= Forms9Patch.Display.Scale;
            minWidth *= Forms9Patch.Display.Scale;
            minHeight *= Forms9Patch.Display.Scale;
#endif

            reqWidth = Math.Max(reqWidth, ((Xamarin.Forms.VisualElement)_roundedBoxElement).WidthRequest);
            reqHeight = Math.Max(reqHeight, ((Xamarin.Forms.VisualElement)_roundedBoxElement).HeightRequest);

            minWidth = Math.Max(minWidth, ((Xamarin.Forms.VisualElement)_roundedBoxElement).WidthRequest);
            minHeight = Math.Max(minHeight, ((Xamarin.Forms.VisualElement)_roundedBoxElement).HeightRequest);


            var reqSize = new Xamarin.Forms.Size(reqWidth, reqHeight);
            var minSize = new Xamarin.Forms.Size(minWidth, minHeight);

            //System.Diagnostics.Debug.WriteLine("GetDesiredSize(" + widthConstraint + "," + heightConstraint + ")=[" + reqSize + "][" + minSize + "]");
            return new Xamarin.Forms.SizeRequest(reqSize, minSize);
        }
        #endregion


        #region local fields
        int _instanceId;

        Forms9Patch.IShape _roundedBoxElement;

        Forms9Patch.Image _imageElement;
        Xamarin.Forms.ImageSource _xfImageSource;

        F9PImageData _f9pImageData;
        RangeLists _sourceRangeLists;

        #endregion


        #region Layout State Fields

        SKSize _lastCanvasSize;

        #region RoundedBox Layout State
        bool _validLayout;
        Xamarin.Forms.Color _lastBackgroundColor;
        bool _lastHasShadow;
        bool _lastShadowInverted;
        Xamarin.Forms.Color _lastOutlineColor;
        double _lastRadius = -1;
        double _lastOutlineWidth = -1;
        ExtendedElementShape _lastExtendedElementShape;
        //bool _lastIgnoreChanges = false;
        #endregion

        #region Background Layout State
        Xamarin.Forms.Thickness _lastPadding;
        #endregion

        #region Image Layout State
        //Forms9Patch.ElementShape _lastElementShape = ElementShape.Rectangle;
        bool _actualSizeValid;
        #endregion

        #endregion


        #region Layout State
        bool ValidLayout(SKSize canvasSize)
        {
            if (_disposed)
                return true;
            //if (!IgnoreChanges && _lastIgnoreChanges == true)
            //    return false;
            if (!_validLayout)
                return false;
            if (!_actualSizeValid)
                return false;
            if (canvasSize != _lastCanvasSize)
                return false;
            if (_lastBackgroundColor != ElementBackgroundColor)
                return false;
            if (_lastHasShadow != HasShadow)
                return false;
            if (_lastShadowInverted != ShadowInverted)
                return false;
            if (_lastOutlineColor != OutlineColor)
                return false;
            if (Math.Abs(_lastRadius - OutlineRadius) > 0.1)
                return false;
            if (Math.Abs(_lastOutlineWidth - OutlineWidth) > 0.1)
                return false;
            if (_roundedBoxElement is ILayout backgroundElement && _lastPadding != ShadowPadding)
                return false;
            if (_lastExtendedElementShape != ExtendedElementShape)
                return false;
            return true;
        }

        void StoreLayoutProperties()
        {
            if (_disposed)
                return;
            _lastCanvasSize = CanvasSize;
            _lastBackgroundColor = ElementBackgroundColor;
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
        #endregion


        #region Windows Layout Support
#if WINDOWS_UWP

        protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
        {
            if (_disposed)
                return new Windows.Foundation.Size();
            var result = base.MeasureOverride(availableSize);

            if (_roundedBoxElement is Forms9Patch.Image && !_actualSizeValid && _f9pImageData != null)
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
                //    result = new Windows.Foundation.Size(_sourceBitmap.Width / FormsGestures.Display.Scale, _sourceBitmap.Height / FormsGestures.Display.Scale);
                //else
                {
                    var sourceAspect = _f9pImageData.Height / _f9pImageData.Width;

                    if (constrainedWidth && constrainedHeight)
                    {
                        // if single image, SetAspect should do all the heavy lifting.  if stitched together, then it's ImageFill.Fill;
                        result = new Windows.Foundation.Size(constrainedWidthValue, constrainedHeightValue);
                    }
                    else if (constrainedWidth)
                    {
                        if (ImageElement.Fill == Fill.Tile)
                            result = new Windows.Foundation.Size(constrainedWidthValue, availableSize.Height / FormsGestures.Display.Scale);
                        else
                            result = new Windows.Foundation.Size(constrainedWidthValue, constrainedWidthValue * sourceAspect);
                    }
                    else if (constrainedHeight)
                    {
                        if (ImageElement.Fill == Fill.Tile)
                            result = new Windows.Foundation.Size(availableSize.Width / FormsGestures.Display.Scale, constrainedHeightValue);
                        else
                            result = new Windows.Foundation.Size(constrainedHeightValue / sourceAspect, constrainedHeightValue);
                    }
                }
                //if (materialButton != null && materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical) if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + P42.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap.Size=[" + _sourceBitmap.Width + ", " + _sourceBitmap.Height + "]");
            }
            return result;
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            //if (materialButton != null && materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical) if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _roundedBoxElement.InstanceId + "][" + GetType() + "][" + P42.Utils.ReflectionExtensions.CallerMemberName() + "] availableSize=[" + finalSize + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "]");

            var result = base.ArrangeOverride(finalSize);

            if (_roundedBoxElement is Forms9Patch.Image && !_actualSizeValid && _f9pImageData != null && Children.Count > 0)
                Invalidate();

            return result;
        }

#endif
        #endregion


        #region Layout

#if WINDOWS_UWP
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
#else
        protected void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
#endif
            if (_disposed)
                return;

            var info = e.Info;
            var surface = e.Surface;
            var canvas = e.Surface?.Canvas;

            if (canvas == null)
                return;



            if (ValidLayout(CanvasSize) && Xamarin.Forms.Device.RuntimePlatform != Xamarin.Forms.Device.UWP)
                return;

            canvas.Clear();

            if (_roundedBoxElement is SegmentedControl materialSegmentedControl)
            {
                // do nothing
            }
            else
            {
                var hz = true;
                var materialButton = _roundedBoxElement as Button;
                if (materialButton != null)
                    hz = materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Horizontal;
                var vt = !hz;

                var backgroundColor = ElementBackgroundColor;
                var hasShadow = HasShadow;
                var shadowInverted = ShadowInverted;
                var outlineWidth = OutlineWidth;
                var outlineRadius = OutlineRadius;
                var outlineColor = OutlineColor;
                var elementShape = ExtendedElementShape;

                float separatorWidth = FormsGestures.Display.Scale * (materialButton == null || elementShape == ExtendedElementShape.Rectangle ? 0 : materialButton.SeparatorWidth < 0 ? outlineWidth : Math.Max(0, materialButton.SeparatorWidth));

                bool drawOutline = DrawOutline;
                bool drawImage = DrawImage;
                bool drawSeparators = outlineColor.A > 0.01 && materialButton != null && separatorWidth > 0.01;
                bool drawFill = DrawFill;

                if ((drawFill || drawOutline || drawSeparators || drawImage) && CanvasSize != default(SKSize))
                {

                    //if (materialButton != null && materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical) if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "][" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + "]  Parent.Size=[" + ((FrameworkElement)Parent).ActualWidth + "," + ((FrameworkElement)Parent).ActualHeight + "]");

                    SKRect rect = new SKRect(0, 0, info.Width, info.Height);

#if __IOS__
                    //if (Superview is UIView parent)
                    //    rect = new SKRect(0, 0, (float)(parent.Bounds.Width * FormsGestures.Display.Scale), (float)(parent.Bounds.Height * FormsGestures.Display.Scale));
#elif __DROID__
                    //if (Parent is Android.Views.View parent)
                    //    rect = new SKRect(0, 0, (float)(parent.Width), (float)(parent.Height));
#elif WINDOWS_UWP
                    //if (Parent is FrameworkElement parent)
                    //    rect = new SKRect(0, 0, (float)(parent.ActualWidth * FormsGestures.Display.Scale), (float)(parent.ActualHeight * FormsGestures.Display.Scale));
#else
            ParentX;
#endif

                    var makeRoomForShadow = hasShadow && (backgroundColor.A > 0.01 || drawImage); // && !_roundedBoxElement.ShadowInverted;
                    var shadowX = (float)(Forms9Patch.Settings.ShadowOffset.X * FormsGestures.Display.Scale);
                    var shadowY = (float)(Forms9Patch.Settings.ShadowOffset.Y * FormsGestures.Display.Scale);
                    var shadowR = (float)(Forms9Patch.Settings.ShadowRadius * FormsGestures.Display.Scale);
                    var shadowColor = Xamarin.Forms.Color.FromRgba(0.0, 0.0, 0.0, 0.75).ToSKColor(); //  .ToWindowsColor().ToSKColor();
                    var shadowPadding = ShapeBase.ShadowPadding(_roundedBoxElement, hasShadow, true);


                    var perimeter = rect;

#if __IOS__
                    ClipsToBounds = false;
#elif __DROIDv15__
                    hasShadow = false;
                    makeRoomForShadow = false;
                    shadowInverted = false;
#elif __DROID__
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBeanMr2)
                        ClipBounds = null;
                    else if (elementShape == ExtendedElementShape.SegmentEnd || elementShape == ExtendedElementShape.SegmentMid || elementShape == ExtendedElementShape.SegmentStart)
                    {
                        hasShadow = false;
                        makeRoomForShadow = false;
                        shadowInverted = false;
                    }
#elif WINDOWS_UWP
                    Clip = null;
#else
            Clip;
#endif

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

                            switch (elementShape)
                            {
                                case ExtendedElementShape.SegmentStart:
                                    shadowRect = new SKRect(perimeter.Left, perimeter.Top, perimeter.Right + allowance * (vt ? 0 : 1), perimeter.Bottom + allowance * (hz ? 0 : 1));
                                    break;
                                case ExtendedElementShape.SegmentMid:
                                    shadowRect = new SKRect(perimeter.Left - allowance * (vt ? 0 : 1), perimeter.Top - allowance * (hz ? 0 : 1), perimeter.Right + allowance * (vt ? 0 : 1), perimeter.Bottom + allowance * (hz ? 0 : 1));
                                    break;
                                case ExtendedElementShape.SegmentEnd:
                                    shadowRect = new SKRect(perimeter.Left - allowance * (vt ? 0 : 1), perimeter.Top - allowance * (hz ? 0 : 1), perimeter.Right, perimeter.Bottom);
                                    break;
                            }

#if __IOS__
                            ClipsToBounds = true;
#elif __DROIDv15__
#elif __DROID__
                            if (Forms9Patch.OsInfoService.Version >= new Version(4, 3))
                                ClipBounds = new Android.Graphics.Rect(0, 0, Width, Height);
#elif WINDOWS_UWP

                            var w = double.IsNaN(Width) ? e.Info.Width+1 : Width;
                            var h = double.IsNaN(Height) ? e.Info.Height+1 : Height;
                            Clip = new Windows.UI.Xaml.Media.RectangleGeometry { Rect = new Rect(0, 0, w - 1, h - 1) };
#else
                    Clips;
#endif

                            var shadowPaint = new SKPaint
                            {
                                Style = SKPaintStyle.Fill,
                                Color = shadowColor,
                            };

                            var filter = SkiaSharp.SKImageFilter.CreateDropShadow(shadowX, shadowY, shadowR / 2, shadowR / 2, shadowColor, SKDropShadowImageFilterShadowMode.DrawShadowOnly);
                            shadowPaint.ImageFilter = filter;
                            //var filter = SkiaSharp.SKMaskFilter.CreateBlur(SKBlurStyle.Outer, 0.5f);
                            //shadowPaint.MaskFilter = filter;

                            if (DrawFill)
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
                            Color = outlineColor.ToSKColor(),
                            StrokeWidth = outlineWidth,
                            IsAntialias = true,
                            //StrokeJoin = SKStrokeJoin.Bevel
                            //PathEffect = SKPathEffect.CreateDash(new float[] { 20,20 }, 0)
                        };
                        var intPerimeter = new SKRect((int)perimeter.Left, (int)perimeter.Top, (int)perimeter.Right, (int)perimeter.Bottom);
                        //System.Diagnostics.Debug.WriteLine("perimeter=[" + perimeter + "] [" + intPerimeter + "]");
                        var outlineRect = RectInsetForShape(intPerimeter, elementShape, outlineWidth / 2, vt);
                        var path = PerimeterPath(_roundedBoxElement, outlineRect, outlineRadius - (drawOutline ? outlineWidth / 2 : 0));
                        canvas.DrawPath(path, outlinePaint);
                    }
                    else if (drawSeparators && (elementShape == ExtendedElementShape.SegmentMid || elementShape == ExtendedElementShape.SegmentEnd))
                    {
                        var separatorPaint = new SKPaint
                        {
                            Style = SKPaintStyle.Stroke,
                            Color = outlineColor.ToSKColor(),
                            StrokeWidth = separatorWidth,
                            IsAntialias = true,
                            //PathEffect = SKPathEffect.CreateDash(new float[] { 20,20 }, 0)
                        };
                        var path = new SKPath();
                        if (vt)
                        {
                            path.MoveTo(perimeter.Left, perimeter.Top + outlineWidth / 2.0f);
                            path.LineTo(perimeter.Right, perimeter.Top + outlineWidth / 2.0f);
                        }
                        else
                        {
                            path.MoveTo(perimeter.Left + outlineWidth, perimeter.Top);
                            path.LineTo(perimeter.Left + outlineWidth, perimeter.Bottom);
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

#if WINDOWS_UWP
            base.OnPaintSurface(e);
#endif

            _actualSizeValid = true;


        }
        #endregion


        #region Image Layout
        void GenerateImageLayout(SKCanvas canvas, SKRect fillRect, SKPath clipPath, SKPaint shadowPaint = null)
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

            if (_imageElement == null || _f9pImageData == null || !_f9pImageData.ValidImage)
                return;
            if (_f9pImageData.ValidSVG)
            {
                workingCanvas.Save();
                if (clipPath != null)
                    workingCanvas.ClipPath(clipPath);

                if (_imageElement.Fill == Fill.Tile)
                {
                    for (float x = 0; x < fillRect.Width; x += (float)_f9pImageData.Width)
                        for (float y = 0; y < fillRect.Height; y += (float)_f9pImageData.Height)
                        {
                            workingCanvas.ResetMatrix();
                            workingCanvas.Translate(x, y);
                            workingCanvas.DrawPicture(_f9pImageData.SKSvg.Picture);
                        }
                    workingCanvas.Restore();
                    return;
                }

                var fillRectAspect = fillRect.Width / fillRect.Height;
                var imageAspect = _f9pImageData.Width / _f9pImageData.Height;
                float scaleX = 1;
                float scaleY = 1;
                if (_imageElement.Fill == Fill.AspectFill)
                {
                    scaleX = (float)(imageAspect > fillRectAspect ? fillRect.Height / _f9pImageData.Height : fillRect.Width / _f9pImageData.Width);
                    scaleY = scaleX;
                }
                else if (_imageElement.Fill == Fill.AspectFit)
                {
                    scaleX = (float)(imageAspect > fillRectAspect ? fillRect.Width / _f9pImageData.Width : fillRect.Height / _f9pImageData.Height);
                    scaleY = scaleX;
                }
                else
                {
                    scaleX = (float)(fillRect.Width / _f9pImageData.Width);
                    scaleY = (float)(fillRect.Height / _f9pImageData.Height);
                }
                var scaledWidth = _f9pImageData.Width * scaleX;
                var scaledHeight = _f9pImageData.Height * scaleY;
                float left;
                switch (_imageElement.HorizontalOptions.Alignment)
                {
                    case Xamarin.Forms.LayoutAlignment.Start:
                        left = 0;
                        break;
                    case Xamarin.Forms.LayoutAlignment.End:
                        left = (float)(fillRect.Width - scaledWidth);
                        break;
                    default:
                        left = (float)(fillRect.Width - scaledWidth) / 2.0f;
                        break;
                }
                float top;
                switch (_imageElement.VerticalOptions.Alignment)
                {
                    case Xamarin.Forms.LayoutAlignment.Start:
                        top = 0;
                        break;
                    case Xamarin.Forms.LayoutAlignment.End:
                        top = (float)(fillRect.Height - scaledHeight);
                        break;
                    default:
                        top = (float)(fillRect.Height - scaledHeight) / 2.0f;
                        break;
                }
                workingCanvas.Translate(left, top);
                workingCanvas.Scale(scaleX, scaleY);
                workingCanvas.DrawPicture(_f9pImageData.SKSvg.Picture);
                workingCanvas.Restore();
            }
            else if (_f9pImageData.ValidBitmap)
            {
                //if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "]["+GetType()+"."+P42.Utils.ReflectionExtensions.CallerMemberName()+"]  Fill=[" + _imageElement.Fill + "] W,H=[" + Width + "," + Height + "] ActualWH=[" + ActualWidth + "," + ActualHeight + "] ");
                workingCanvas.Save();
                if (clipPath != null)
                    workingCanvas.ClipPath(clipPath);

                var rangeLists = _imageElement.CapInsets.ToRangeLists(_f9pImageData.SKBitmap.Width, _f9pImageData.SKBitmap.Height, _xfImageSource, _sourceRangeLists != null);
                if (rangeLists == null)
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

                var bitmap = _f9pImageData;
                SKPaint paint = null;
                if (shadowPaint == null && _imageElement.TintColor != Xamarin.Forms.Color.Default && _imageElement.TintColor != Xamarin.Forms.Color.Transparent)
                {
                    var mx = new Single[]
                    {
                    0, 0, 0, 0, _imageElement.TintColor.ByteR(),
                    0, 0, 0, 0, _imageElement.TintColor.ByteG(),
                    0, 0, 0, 0, _imageElement.TintColor.ByteB(),
                    0, 0, 0, (float)_imageElement.TintColor.A, 0
                    };
                    var cf = SKColorFilter.CreateColorMatrix(mx);


                    paint = new SKPaint()
                    {
                        ColorFilter = cf,
                        IsAntialias = true,
                    };
                }
                else if (_imageElement.Opacity < 1.0)
                {
                    var transparency = SKColors.White.WithAlpha((byte)(_imageElement.Opacity * 255)); // 127 => 50%
                    paint = new SKPaint
                    {
                        ColorFilter = SKColorFilter.CreateBlendMode(transparency, SKBlendMode.DstIn)
                    };
                }

                var fillRectAspect = fillRect.Width / fillRect.Height;
                var bitmapAspect = _f9pImageData.Width / _f9pImageData.Height;

                if (_imageElement.Fill == Fill.Tile)
                {
                    for (float x = fillRect.Left; x < fillRect.Right; x += _f9pImageData.SKBitmap.Width)
                        for (float y = fillRect.Top; y < fillRect.Bottom; y += _f9pImageData.SKBitmap.Height)
                            workingCanvas.DrawBitmap(_f9pImageData.SKBitmap, x, y, paint);
                }
                else if (rangeLists == null)
                {
                    var sourceRect = new SKRect(_f9pImageData.SKBitmap.Info.Rect.Left, _f9pImageData.SKBitmap.Info.Rect.Top, _f9pImageData.SKBitmap.Info.Rect.Right, _f9pImageData.SKBitmap.Info.Rect.Bottom);
                    var destRect = fillRect;
                    if (_imageElement.Fill == Fill.AspectFill)
                    {
                        var croppedWidth = bitmapAspect > fillRectAspect ? _f9pImageData.Height * fillRectAspect : _f9pImageData.Width;
                        var croppedHeight = bitmapAspect > fillRectAspect ? _f9pImageData.Height : _f9pImageData.Width / fillRectAspect;
                        float left;
                        switch (_imageElement.HorizontalOptions.Alignment)
                        {
                            case Xamarin.Forms.LayoutAlignment.Start:
                                left = 0;
                                break;
                            case Xamarin.Forms.LayoutAlignment.End:
                                left = (float)(_f9pImageData.Width - croppedWidth);
                                break;
                            default:
                                left = (float)(_f9pImageData.Width - croppedWidth) / 2.0f;
                                break;
                        }
                        float top;
                        switch (_imageElement.VerticalOptions.Alignment)
                        {
                            case Xamarin.Forms.LayoutAlignment.Start:
                                top = 0;
                                break;
                            case Xamarin.Forms.LayoutAlignment.End:
                                top = (float)(_f9pImageData.Height - croppedHeight);
                                break;
                            default:
                                top = (float)(_f9pImageData.Height - croppedHeight) / 2.0f;
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
                        _f9pImageData.SKBitmap.ExtractSubset(croppedBitmap, new SKRectI((int)sourceRect.Left, (int)sourceRect.Top, (int)sourceRect.Right, (int)sourceRect.Bottom));
                        SKBitmap.Resize(destBitmap, croppedBitmap, SKBitmapResizeMethod.Lanczos3);
                        workingCanvas.DrawBitmap(destBitmap, destBitmap.Info.Rect, destRect, paint);
                    }
                    else
                    {
                        workingCanvas.DrawBitmap(_f9pImageData.SKBitmap, sourceRect, destRect, paint);
                    }
                }
                else
                {
                    var lattice = rangeLists.ToSKLattice(_f9pImageData.SKBitmap);
                    //System.Diagnostics.Debug.WriteLine("lattice.x: ["+lattice.XDivs[0]+","+lattice.XDivs[1]+"] lattice.y: ["+lattice.YDivs[0]+","+lattice.YDivs[1]+"] lattice.Bounds=["+lattice.Bounds+"] lattice.Flags:" + lattice.Flags);
                    workingCanvas.DrawBitmapLattice(_f9pImageData.SKBitmap, lattice, fillRect);
                }

                workingCanvas.Restore();

                if (shadowPaint != null)
                {
                    paint = shadowPaint;
                    canvas.DrawBitmap(shadowBitmap, shadowBitmap.Info.Rect, paint);
                }
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

        /*
            static SKRect RoundRect(SKRect rect, Xamarin.Forms.StackOrientation orientation, Forms9Patch.ExtendedElementShape type)
            {
                return rect;
            }
            */

        static SKRect SegmentAllowanceRect(SKRect rect, float allowance, Xamarin.Forms.StackOrientation orientation, Forms9Patch.ExtendedElementShape type)
        {
            SKRect result;
            switch (type)
            {
                case ExtendedElementShape.SegmentStart:
                    //result = new Rect(rect.Left, rect.Top, rect.Width + (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Height + (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0)).ToSKRect();
                    result = SKRect.Create(rect.Left, rect.Top, rect.Width + (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Height + (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0));
                    break;
                case ExtendedElementShape.SegmentMid:
                    //result = new Rect(rect.Left - (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0), rect.Width + (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance * 2 : 0), rect.Height + (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance * 2 : 0)).ToSKRect();
                    result = new SKRect(rect.Left - (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0), rect.Width + (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance * 2 : 0), rect.Height + (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance * 2 : 0));
                    break;
                case ExtendedElementShape.SegmentEnd:
                    //result = new Rect(rect.Left - (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0), rect.Width + (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Height + (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0)).ToSKRect();
                    result = new SKRect(rect.Left - (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0), rect.Width + (orientation == Xamarin.Forms.StackOrientation.Horizontal ? allowance : 0), rect.Height + (orientation == Xamarin.Forms.StackOrientation.Vertical ? allowance : 0));
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

            var materialButton = element as Button;
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
        ;
            }
            return path;
        }

        internal static SKPath BubblePerimeterPath(BubbleLayout bubble, SKRect rect, float radius)
        {
            var length = bubble.PointerLength * FormsGestures.Display.Scale;

            if (radius * 2 > rect.Height - (bubble.PointerDirection.IsVertical() ? length : 0))
                radius = (float)((rect.Height - (bubble.PointerDirection.IsVertical() ? length : 0)) / 2.0);
            if (radius * 2 > rect.Width - (bubble.PointerDirection.IsHorizontal() ? length : 0))
                radius = (float)((rect.Width - (bubble.PointerDirection.IsHorizontal() ? length : 0)) / 2.0);

            var filetRadius = bubble.PointerCornerRadius;
            var tipRadius = bubble.PointerTipRadius * FormsGestures.Display.Scale;

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

