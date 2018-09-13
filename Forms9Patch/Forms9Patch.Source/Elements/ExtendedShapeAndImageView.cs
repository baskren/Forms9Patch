using System;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using SkiaSharp;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace Forms9Patch
{
    public class Image : SkiaSharp.Views.Forms.SKCanvasView, IImage, IImageController, IExtendedShape
    {
        #region Static Implementation


        #region Static Fields
        internal static string EmbeddedResourceImageCacheFolderName = "EmbeddedResourceImageCache";
        internal static string UriImageCacheFolderName = "UriResourceImageCache";
        #endregion

        #region Static Methods
        /// <summary>
        /// Clears the cache of Forms9Patch images sourced using Xamarin.Forms.ImageSource.FromUri
        /// </summary>
        /// <param name="uri">The URI of the image.  If null, all cached images are cleared.</param>
        public static void ClearDownloadCache(string uri = null) => P42.Utils.DownloadCache.Clear(uri, UriImageCacheFolderName);

        /// <summary>
        /// Apps run faster when Embedded Resources image don't have to be extracted EVERY SINGLE TIME.  This clears the cache of these images.
        /// </summary>
        /// <param name="resourceId">The ResourceId of the image.  If null, all cahced images are cleared.</param>
        public static void ClearEmbeddedResourceCache(string resourceId = null) => P42.Utils.EmbeddedResourceCache.Clear(resourceId, EmbeddedResourceImageCacheFolderName);
        #endregion

        #endregion


        #region Properties

        #region IImageController
        internal static readonly BindablePropertyKey IsLoadingPropertyKey = BindableProperty.CreateReadOnly("IsLoading", typeof(bool), typeof(Image), default(bool));

        void IImageController.SetIsLoading(bool isLoading)
        {
            SetValue(IsLoadingPropertyKey, isLoading);
        }
        #endregion

        #region IImage Properties

        #region Source property
        /// <summary>
        /// backing store for Source property
        /// </summary>
        public static readonly BindableProperty SourceProperty = BindableProperty.Create("Source", typeof(Xamarin.Forms.ImageSource), typeof(Image), default(Xamarin.Forms.ImageSource));
        /// <summary>
        /// Gets/Sets the Source property
        /// </summary>
        public Xamarin.Forms.ImageSource Source
        {
            get => (Xamarin.Forms.ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        #endregion Source property

        #region IsLoading property
        /// <summary>
        /// backing store for IsLoading property
        /// </summary>
        public static readonly BindableProperty IsLoadingProperty = IsLoadingPropertyKey.BindableProperty;
        /// <summary>
        /// Gets/Sets the IsLoading property
        /// </summary>
        public bool IsLoading => (bool)GetValue(IsLoadingProperty);

        #endregion IsLoading property

        #region Fill

        /// <summary>
        /// Backing store for the Fill bindable property.
        /// </summary>
        public static readonly BindableProperty FillProperty = BindableProperty.Create("Fill", typeof(Fill), typeof(Image), Fill.Fill);
        /// <summary>
        /// Fill behavior for nonscalable (not NinePatch or CapInsets not set) image. 
        /// </summary>
        /// <value>The fill method (AspectFill, AspectFit, Fill, Tile)</value>
        public Fill Fill
        {
            get => (Fill)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
        #endregion Fill

        #region CapInsets
        /// <summary>
        /// Backing store for the CapsInset bindable property.
        /// </summary>
        /// <remarks>
        /// End caps specify the portion of an image that should not be resized when an image is stretched. This technique is used to implement buttons and other resizable image-based interface elements. 
        /// When a button with end caps is resized, the resizing occurs only in the middle of the button, in the region between the end caps. The end caps themselves keep their original size and appearance.
        /// </remarks>
        /// <value>The end-cap insets (double or int)</value>
        public static readonly BindableProperty CapInsetsProperty = BindableProperty.Create("CapInsets", typeof(Thickness), typeof(Image), new Thickness(-1));
        /// <summary>
        /// Gets or sets the end-cap insets.  This is a bindable property.
        /// </summary>
        /// <value>The end-cap insets.</value>
        public Thickness CapInsets
        {
            get => (Thickness)GetValue(CapInsetsProperty);
            set => SetValue(CapInsetsProperty, value);
        }
        #endregion CapInsets

        #region ContentPadding
        /// <summary>
        /// Backing store for the ContentPadding bindable property.
        /// </summary>
        public static readonly BindableProperty ContentPaddingProperty = BindableProperty.Create("ContentPadding", typeof(Thickness), typeof(Image), new Thickness(-1));
        /// <summary>
        /// Gets content padding if Source is NinePatch image.
        /// </summary>
        /// <value>The content padding.</value>
        public Thickness ContentPadding
        {
            get => (Thickness)GetValue(ContentPaddingProperty);
            internal set => SetValue(ContentPaddingProperty, value);
        }
        #endregion ContentPadding

        #region TintColor
        /// <summary>
        /// The tint property.
        /// </summary>
        public static readonly BindableProperty TintColorProperty = BindableProperty.Create("TintColor", typeof(Color), typeof(Image), Color.Default);
        /// <summary>
        /// Gets or sets the image's tint.
        /// </summary>
        /// <value>The tint.  Default is not to tint the image</value>
        public Color TintColor
        {
            get => (Color)GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }
        #endregion TintColor

        #region SourceImageSize
        public Size SourceImageSize
        {
            get
            {
                return _f9pImageData != null && _f9pImageData.ValidImage
                                                             ? new Xamarin.Forms.Size(SourceImageWidth, SourceImageHeight)
                    : Xamarin.Forms.Size.Zero;
            }
        }

        public double SourceImageWidth => _f9pImageData != null ? _f9pImageData.Width /* / FormsGestures.Display.Scale */: 0;
        public double SourceImageHeight => _f9pImageData != null ? _f9pImageData.Height /* / FormsGestures.Display.Scale */: 0;
        double SourceImageAspect => SourceImageHeight > 0 ? SourceImageWidth / SourceImageHeight : 1;
        #endregion SourceImageSize

        #region AntiAlias property
        /// <summary>
        /// backing store for AntiAlias property
        /// </summary>
        public static readonly BindableProperty AntiAliasProperty = BindableProperty.Create("AntiAlias", typeof(bool), typeof(Image), true);
        /// <summary>
        /// Gets/Sets the AntiAlias property
        /// </summary>
        public bool AntiAlias
        {
            get => (bool)GetValue(AntiAliasProperty);
            set => SetValue(AntiAliasProperty, value);
        }
        #endregion AntiAlias property

        #region IExtendedShape

        #region ExtendedElementShapeOrientation property
        /// <summary>
        /// Backing store for the extended element shape orientation property.
        /// </summary>
        public static readonly BindableProperty ExtendedElementShapeOrientationProperty = ShapeBase.ExtendedElementShapeOrientationProperty;
        /// <summary>
        /// Gets or sets the orientation of the shape if it's an extended element shape
        /// </summary>
        /// <value>The forms9 patch. IS hape. extended element shape orientation.</value>
        ExtendedElementShapeOrientation IExtendedShape.ExtendedElementShapeOrientation
        {
            get => (ExtendedElementShapeOrientation)GetValue(ExtendedElementShapeOrientationProperty);
            set => SetValue(ExtendedElementShapeOrientationProperty, value);
        }
        #endregion

        #region ExtendedElementShape property
        /// <summary>
        /// backing store for ExtendedElementShape property
        /// </summary>
        public static readonly BindableProperty ExtendedElementShapeProperty = ShapeBase.ExtendedElementShapeProperty;// = BindableProperty.Create("ExtendedElementShape", typeof(ExtendedElementShape), typeof(ShapeAndImageView), default(ExtendedElementShape));
        /// <summary>
        /// Gets/Sets the ExtendedElementShape property
        /// </summary>
        ExtendedElementShape IExtendedShape.ExtendedElementShape
        {
            get => (ExtendedElementShape)GetValue(ExtendedElementShapeProperty);
            set => SetValue(ExtendedElementShapeProperty, value);
        }
        #endregion ExtendedElementShape property

        #region ExtendedElementSeparatorWidth
        public static readonly BindableProperty ExtendedElementSeparatorWidthProperty = ShapeBase.ExtendedElementSeparatorWidthProperty;
        float IExtendedShape.ExtendedElementSeparatorWidth
        {
            get => (float)GetValue(ExtendedElementSeparatorWidthProperty);
            set => SetValue(ExtendedElementSeparatorWidthProperty, value);
        }
        #endregion ExtendedElementSeparatorWidth

        #region ParentSegmentsOrientation
        public static readonly BindableProperty ParentSegmentsOrientationProperty = ShapeBase.ParentSegmentsOrientationProperty;
        public StackOrientation ParentSegmentsOrientation
        {
            get => (StackOrientation)GetValue(ParentSegmentsOrientationProperty);
            set => SetValue(ParentSegmentsOrientationProperty, value);
        }
        #endregion ParentSegmentsOrientation

        #region IShape

        #region BackgroundColor property
        /// <summary>
        /// backing store for BackgroundColor property
        /// </summary>
        public static new readonly BindableProperty BackgroundColorProperty = ShapeBase.BackgroundColorProperty;
        /// <summary>
        /// Gets/Sets the BackgroundColor property
        /// </summary>
        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        #endregion BackgroundColor property

        #region HasShadow property
        /// <summary>
        /// Backing store for HasShadow property
        /// </summary>
        public static readonly BindableProperty HasShadowProperty = ShapeBase.HasShadowProperty; // = BindableProperty.Create("HasShadow", typeof(bool), typeof(ShapeAndImageView), default(bool));
        /// <summary>
        /// Get/sets if the shape (if any) behind the image has a shadow
        /// </summary>
        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }
        #endregion HasShadow property

        #region ShadowInverted property
        /// <summary>
        /// backing store for ShadowInverted property
        /// </summary>
        public static readonly BindableProperty ShadowInvertedProperty = ShapeBase.ShadowInvertedProperty; // = BindableProperty.Create("ShadowInverted", typeof(bool), typeof(ShapeAndImageView), default(bool));
        /// <summary>
        /// Gets/Sets the ShadowInverted property
        /// </summary>
        public bool ShadowInverted
        {
            get => (bool)GetValue(ShadowInvertedProperty);
            set => SetValue(ShadowInvertedProperty, value);
        }
        #endregion ShadowInverted property

        #region OutlineColor property
        /// <summary>
        /// backing store for OutlineColor property
        /// </summary>
        public static readonly BindableProperty OutlineColorProperty = ShapeBase.OutlineColorProperty; // = BindableProperty.Create("OutlineColor", typeof(Color), typeof(ShapeAndImageView), default(Color));
        /// <summary>
        /// Gets/Sets the OutlineColor property
        /// </summary>
        public Color OutlineColor
        {
            get => (Color)GetValue(OutlineColorProperty);
            set => SetValue(OutlineColorProperty, value);
        }
        #endregion OutlineColor property

        #region OutlineRadius property
        /// <summary>
        /// backing store for OutlineRadius property
        /// </summary>
        public static readonly BindableProperty OutlineRadiusProperty = ShapeBase.OutlineRadiusProperty; // = BindableProperty.Create("OutlineRadius", typeof(float), typeof(ShapeAndImageView), default(float));
        /// <summary>
        /// Gets/Sets the OutlineRadius property
        /// </summary>
        public float OutlineRadius
        {
            get => (float)GetValue(OutlineRadiusProperty);
            set => SetValue(OutlineRadiusProperty, value);
        }
        #endregion OutlineRadius property

        #region OutlineWidth property
        /// <summary>
        /// backing store for OutlineWidth property
        /// </summary>
        public static readonly BindableProperty OutlineWidthProperty = ShapeBase.OutlineWidthProperty;// = BindableProperty.Create("OutlineWidth", typeof(float), typeof(ShapeAndImageView), -1f);
        /// <summary>
        /// Gets/Sets the OutlineWidth property
        /// </summary>
        public float OutlineWidth
        {
            get => (float)GetValue(OutlineWidthProperty);
            set => SetValue(OutlineWidthProperty, value);
        }
        #endregion OutlineWidth property

        #region ElementShape property
        /// <summary>
        /// Backing store for the ElementShape property
        /// </summary>
        public static readonly BindableProperty ElementShapeProperty = ShapeBase.ElementShapeProperty;
        /// <summary>
        /// Gets/sets the geometry of the element
        /// </summary>
        public ElementShape ElementShape
        {
            get => (ElementShape)GetValue(ElementShapeProperty);
            set => SetValue(ElementShapeProperty, value);
        }
        #endregion ElementShape property

        Thickness IShape.ShadowPadding() => ShapeBase.ShadowPadding(this, HasShadow, true);

        #region IElement

        /// <summary>
        /// Returns index instance ID for this class (starts at 0)
        /// </summary>
        public int InstanceId => _f9pId;

        #endregion IElement

        #endregion IShape

        #endregion IExtendedShape

        #endregion IImage

        #endregion Properties




        #region Fields
        internal bool FillOrLayoutSet;
        static int _instances;
        readonly int _f9pId;
        Xamarin.Forms.ImageSource _xfImageSource;

        F9PImageData _f9pImageData;
        RangeLists _sourceRangeLists;

        bool DrawImage => (Opacity > 0 && _f9pImageData != null && _f9pImageData.ValidImage);

        bool DrawOutline => (OutlineColor.A > 0.01 && OutlineWidth > 0.05);

        bool DrawFill => BackgroundColor.A > 0.01;

        bool IsSegment => ((IExtendedShape)this).ExtendedElementShape == ExtendedElementShape.SegmentEnd || ((IExtendedShape)this).ExtendedElementShape == ExtendedElementShape.SegmentEnd || ((IExtendedShape)this).ExtendedElementShape == ExtendedElementShape.SegmentStart;


        #endregion


        #region Constructors
        static Image()
        {
            Settings.ConfirmInitialization();
        }

        public Image()
        {
            _f9pId = _instances++;
        }

        /// <summary>
        /// Instantiates a new instance of Image from an MultiResource embedded resource
        /// </summary>
        /// <param name="embeddedResourceId">EmbeddedResourceId for image</param>
        /// <param name="assembly">Assembly in which embedded resource is embedded</param>
        public Image(string embeddedResourceId, Assembly assembly = null) : this()
        {
            if (assembly == null && Device.RuntimePlatform != Device.UWP)
                assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);
            Source = Forms9Patch.ImageSource.FromMultiResource(embeddedResourceId, assembly);
        }

        /// <summary>
        /// Instantiates a new instance of Image from an imageSource
        /// </summary>
        /// <param name="imageSource"></param>
        public Image(Xamarin.Forms.ImageSource imageSource) : this()
        {
            Source = imageSource;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="image">Image.</param>
        public Image(Xamarin.Forms.Image image)
        {
            _f9pId = _instances++;
            Fill = image.Aspect.ToF9pFill();
            FillOrLayoutSet = !image.HasDefaultAspectAndLayoutOptions();
            //IsOpaque = image.IsOpaque;
            HorizontalOptions = image.HorizontalOptions;
            VerticalOptions = image.VerticalOptions;
            AnchorX = image.AnchorX;
            AnchorY = image.AnchorY;
            BackgroundColor = image.BackgroundColor;
            HeightRequest = image.HeightRequest;
            InputTransparent = image.InputTransparent;
            IsEnabled = image.IsEnabled;
            IsVisible = image.IsVisible;
            MinimumHeightRequest = image.MinimumHeightRequest;
            MinimumWidthRequest = image.MinimumWidthRequest;
            Opacity = image.Opacity;
            Resources = image.Resources;
            Rotation = image.Rotation;
            RotationX = image.RotationX;
            RotationY = image.RotationY;
            Scale = image.Scale;
            Style = image.Style;
            TranslationX = image.TranslationX;
            TranslationY = image.TranslationY;
            WidthRequest = image.WidthRequest;
            Source = image.Source;
        }

        /// <summary>
        /// Convenience constructor for Forms9Patch.Image
        /// </summary>
        /// <param name="image"></param>
        public Image(Image image)
        {
            _f9pId = _instances++;
            if (image != null)
            {
                //IsOpaque = image.IsOpaque;
                HorizontalOptions = image.HorizontalOptions;
                VerticalOptions = image.VerticalOptions;
                AnchorX = image.AnchorX;
                AnchorY = image.AnchorY;
                BackgroundColor = image.BackgroundColor;
                HeightRequest = image.HeightRequest;
                InputTransparent = image.InputTransparent;
                IsEnabled = image.IsEnabled;
                IsVisible = image.IsVisible;
                MinimumHeightRequest = image.MinimumHeightRequest;
                MinimumWidthRequest = image.MinimumWidthRequest;
                Opacity = image.Opacity;
                Resources = image.Resources;
                Rotation = image.Rotation;
                RotationX = image.RotationX;
                RotationY = image.RotationY;
                Scale = image.Scale;
                Style = image.Style;
                TranslationX = image.TranslationX;
                TranslationY = image.TranslationY;
                WidthRequest = image.WidthRequest;

                //((IShape)this).ExtendedElementShape = ((IShape)image).ExtendedElementShape;
                ElementShape = image.ElementShape;
                OutlineWidth = image.OutlineWidth;
                OutlineRadius = image.OutlineRadius;
                OutlineColor = image.OutlineColor;
                ShadowInverted = image.ShadowInverted;
                HasShadow = image.HasShadow;

                TintColor = image.TintColor;
                ContentPadding = image.ContentPadding;
                CapInsets = image.CapInsets;
                Fill = image.Fill;
                FillOrLayoutSet = image.FillOrLayoutSet;
                Source = image.Source;
            }
        }
        #endregion


        #region Property Change Handlers
        async Task SetImageSourceAsync()
        {
            if (Source != _xfImageSource)
            {
                // release the previous
                _xfImageSource?.ReleaseF9PBitmap(this);
                _f9pImageData = null;
                _sourceRangeLists = null;

                ((Xamarin.Forms.IImageController)this)?.SetIsLoading(true);
                _xfImageSource = Source;

                if (_xfImageSource != null)
                {
                    _f9pImageData = await _xfImageSource.FetchF9pImageData(this);
                    _sourceRangeLists = _f9pImageData?.RangeLists;
                }

                ((Xamarin.Forms.IImageController)this)?.SetIsLoading(false);
                InvalidateMeasure();
                InvalidateSurface();
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == ElementShapeProperty.PropertyName)
                ((IExtendedShape)this).ExtendedElementShape = ElementShape.ToExtendedElementShape();
            if (propertyName == SourceProperty.PropertyName)
                SetImageSourceAsync().ConfigureAwait(false);
            if (
                // VisualElement
                propertyName == BackgroundColorProperty.PropertyName ||
                propertyName == Xamarin.Forms.VisualElement.OpacityProperty.PropertyName ||

                // ShapeBase
                propertyName == ElementShapeProperty.PropertyName ||
                propertyName == HasShadowProperty.PropertyName ||
                propertyName == OutlineColorProperty.PropertyName ||
                propertyName == OutlineRadiusProperty.PropertyName ||
                propertyName == OutlineWidthProperty.PropertyName ||
                propertyName == ShadowInvertedProperty.PropertyName ||

                // BubbleLayout
                propertyName == Forms9Patch.BubbleLayout.PointerAngleProperty.PropertyName ||
                propertyName == Forms9Patch.BubbleLayout.PointerAxialPositionProperty.PropertyName ||
                propertyName == Forms9Patch.BubbleLayout.PointerCornerRadiusProperty.PropertyName ||
                propertyName == Forms9Patch.BubbleLayout.PointerDirectionProperty.PropertyName ||
                propertyName == Forms9Patch.BubbleLayout.PointerLengthProperty.PropertyName ||
                propertyName == Forms9Patch.BubbleLayout.PointerTipRadiusProperty.PropertyName ||

                // Button
                propertyName == Forms9Patch.Button.SeparatorWidthProperty.PropertyName ||

                // Image
                //propertyName == SourceProperty.PropertyName ||  // Handled by SetImageSource()
                propertyName == TintColorProperty.PropertyName ||
                propertyName == FillProperty.PropertyName ||
                propertyName == CapInsetsProperty.PropertyName ||
                propertyName == AntiAliasProperty.PropertyName

               )

                InvalidateSurface();
        }
        #endregion


        #region Measurement
        /*
        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            System.Diagnostics.Debug.WriteLine("OnSizeRequest ENTER :" + widthConstraint + ", " + heightConstraint);

            // What do we have to deal with?
            // 1) LayoutOptions.Expand?
            // 2) Height/Width Request?
            // 3) Bitmap pixel size
            // 4) Available Space vs. ImageFill?

            // added this an now everything is working!?
            var platformSize = base.OnSizeRequest(widthConstraint, heightConstraint);

            System.Diagnostics.Debug.WriteLine("base.OnSizeRequest=" + platformSize);

            var width = double.IsNaN(widthConstraint) || double.IsInfinity(widthConstraint) ? Display.Width : widthConstraint;
            var height = double.IsNaN(heightConstraint) || double.IsInfinity(heightConstraint) ? Display.Height : heightConstraint;

            SizeRequest result = new SizeRequest();

            if (_f9pImageData != null)
            {
                if (HorizontalOptions.Alignment != LayoutAlignment.Fill)
                    width = SourceImageWidth;
                if (VerticalOptions.Alignment != LayoutAlignment.Fill)
                    height = SourceImageHeight;


                //var result = base.OnMeasure(widthConstraint, heightConstraint);
                System.Diagnostics.Debug.WriteLine("");
                result = new SizeRequest(new Size(width, height), new Size(SourceImageWidth, SourceImageHeight));
            }
            else
            {

                if (HorizontalOptions.Alignment != LayoutAlignment.Fill)
                    width = 40;
                if (VerticalOptions.Alignment != LayoutAlignment.Fill)
                    height = 40;

                result = new SizeRequest(new Size(width, height), new Size(5, 5));
            }
            System.Diagnostics.Debug.WriteLine("OnMeasure EXIT: " + result);
            return result;
        }
        */

        /*
        public override SizeRequest GetSizeRequest(double widthConstraint, double heightConstraint)
        {
            System.Diagnostics.Debug.WriteLine("GetSizeRequest ENTER");
            var result = base.GetSizeRequest(widthConstraint, heightConstraint);
            System.Diagnostics.Debug.WriteLine("GetSizeRequest EXIT");
            return result;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            System.Diagnostics.Debug.WriteLine("OnSizeAllocated ENTER");
            base.OnSizeAllocated(width, height);
            System.Diagnostics.Debug.WriteLine("OnSizeAllocated EXIT");
        }

        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            System.Diagnostics.Debug.WriteLine("OnSizeRequest ENTER");
            var result = base.OnSizeRequest(widthConstraint, heightConstraint);
            System.Diagnostics.Debug.WriteLine("OnSizeRequest EXIT");
            return result;
        }
*/
        #endregion


        #region Painting
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            //base.OnPaintSurface(e);
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface?.Canvas;

            if (canvas == null)
                return;

            canvas.Clear();

            var hz = ((IExtendedShape)this).ExtendedElementShapeOrientation == ExtendedElementShapeOrientation.Horizontal;
            var vt = !hz;

            var backgroundColor = BackgroundColor;
            var hasShadow = HasShadow;
            var shadowInverted = ShadowInverted;
            var outlineWidth = OutlineWidth;
            var outlineRadius = OutlineRadius;
            var outlineColor = OutlineColor;
            var elementShape = ((IExtendedShape)this).ExtendedElementShape;

            float separatorWidth = FormsGestures.Display.Scale * (IsSegment || elementShape == ExtendedElementShape.Rectangle ? 0 : ((IExtendedShape)this).ExtendedElementSeparatorWidth < 0 ? outlineWidth : Math.Max(0, ((IExtendedShape)this).ExtendedElementSeparatorWidth));

            bool drawOutline = DrawOutline;
            bool drawImage = DrawImage;
            bool drawSeparators = outlineColor.A > 0.01 && IsSegment && separatorWidth > 0.01;
            bool drawFill = DrawFill;

            if ((drawFill || drawOutline || drawSeparators || drawImage) && CanvasSize != default(SKSize))
            {

                //if (materialButton != null && materialButton.ParentSegmentsOrientation == Xamarin.Forms.StackOrientation.Vertical) if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "][" + GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + "]  Parent.Size=[" + ((FrameworkElement)Parent).ActualWidth + "," + ((FrameworkElement)Parent).ActualHeight + "]");

                SKRect rect = new SKRect(0, 0, info.Width, info.Height);
                System.Diagnostics.Debug.WriteLine("Image.OnPaintSurface rect=" + rect);

                var makeRoomForShadow = hasShadow && (backgroundColor.A > 0.01 || drawImage); // && !ShapeElement.ShadowInverted;
                var shadowX = (float)(Forms9Patch.Settings.ShadowOffset.X * FormsGestures.Display.Scale);
                var shadowY = (float)(Forms9Patch.Settings.ShadowOffset.Y * FormsGestures.Display.Scale);
                var shadowR = (float)(Forms9Patch.Settings.ShadowRadius * FormsGestures.Display.Scale);
                var shadowColor = Xamarin.Forms.Color.FromRgba(0.0, 0.0, 0.0, 0.75).ToSKColor(); //  .ToWindowsColor().ToSKColor();
                var shadowPadding = ShapeBase.ShadowPadding(this, hasShadow, true);


                var perimeter = rect;


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
                        //Clips;
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
                            var path = PerimeterPath(shadowRect, outlineRadius - (drawOutline ? outlineWidth : 0));
                            canvas.DrawPath(path, shadowPaint);
                        }
                        else if (DrawImage)
                        {
                            var path = PerimeterPath(shadowRect, outlineRadius - (drawOutline ? outlineWidth : 0));
                            GenerateImageLayout(canvas, perimeter, path, shadowPaint);
                        }
                    }
                }

                if (drawFill)
                {
                    var fillRect = RectInsetForShape(perimeter, elementShape, outlineWidth, vt);
                    var path = PerimeterPath(fillRect, outlineRadius - (drawOutline ? outlineWidth : 0));

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
                    var clipPath = PerimeterPath(perimeter, outlineRadius);
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
                    var path = PerimeterPath(outlineRect, outlineRadius - (drawOutline ? outlineWidth / 2 : 0));
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
                    var maskPath = PerimeterPath(perimeter, outlineRadius);
                    canvas.ClipPath(maskPath);

                    // what is the path that will cast the shadow?
                    // a) the button portion (which will be the hole in the larger outline, b)
                    var path = PerimeterPath(perimeter, outlineRadius);
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

                //StoreLayoutProperties();
            }

        }
        #endregion


        #region Image Layout
        void GenerateImageLayout(SKCanvas canvas, SKRect fillRect, SKPath clipPath, SKPaint shadowPaint = null)
        {
            System.Diagnostics.Debug.WriteLine("Image.GenerateImageLayout fillRect:" + fillRect);

            SKBitmap shadowBitmap = null;
            SKCanvas workingCanvas = canvas;

            if (shadowPaint != null)
            {
                var x = canvas.DeviceClipBounds;
                shadowBitmap = new SKBitmap((int)x.Width, (int)x.Height);
                workingCanvas = new SKCanvas(shadowBitmap);
                workingCanvas.Clear();
            }

            if (_f9pImageData == null || !_f9pImageData.ValidImage)
                return;
            if (_f9pImageData.ValidSVG)
            {
                workingCanvas.Save();
                if (clipPath != null)
                    workingCanvas.ClipPath(clipPath);

                if (Fill == Fill.Tile)
                {
                    for (float x = 0; x < fillRect.Width; x += (float)SourceImageWidth)
                        for (float y = 0; y < fillRect.Height; y += (float)SourceImageHeight)
                        {
                            workingCanvas.ResetMatrix();
                            workingCanvas.Translate(x, y);
                            workingCanvas.DrawPicture(_f9pImageData.SKSvg.Picture);
                        }
                    workingCanvas.Restore();
                    return;
                }

                var fillRectAspect = fillRect.Width / fillRect.Height;
                var imageAspect = SourceImageWidth / SourceImageHeight;
                double scaleX = 1;
                double scaleY = 1;
                if (Fill == Fill.AspectFill)
                {
                    scaleX = imageAspect > fillRectAspect ? fillRect.Height / SourceImageHeight : fillRect.Width / SourceImageWidth;
                    scaleY = scaleX;
                }
                else if (Fill == Fill.AspectFit)
                {
                    scaleX = imageAspect > fillRectAspect ? fillRect.Width / SourceImageWidth : fillRect.Height / SourceImageHeight;
                    scaleY = scaleX;
                }
                else
                {
                    scaleX = fillRect.Width / SourceImageWidth;
                    scaleY = fillRect.Height / SourceImageHeight;
                }

                var scaledWidth = SourceImageWidth * scaleX;
                var scaledHeight = SourceImageHeight * scaleY;

                System.Diagnostics.Debug.WriteLine("Image.GenerateImageLayout scaleX:" + scaleX + " scaleY:" + scaleY + " scaledWidth:" + scaledWidth + " scaledHeight:" + scaledHeight);

                float left;
                switch (HorizontalOptions.Alignment)
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
                switch (VerticalOptions.Alignment)
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
                workingCanvas.Scale((float)scaleX, (float)scaleY);
                workingCanvas.DrawPicture(_f9pImageData.SKSvg.Picture);
                workingCanvas.Restore();
            }
            else if (_f9pImageData.ValidBitmap)
            {
                //if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instanceId + "]["+GetType()+"."+P42.Utils.ReflectionExtensions.CallerMemberName()+"]  Fill=[" + _imageElement.Fill + "] W,H=[" + Width + "," + Height + "] ActualWH=[" + ActualWidth + "," + ActualHeight + "] ");
                workingCanvas.Save();
                if (clipPath != null)
                    workingCanvas.ClipPath(clipPath);

                var rangeLists = CapInsets.ToRangeLists(_f9pImageData.SKBitmap.Width, _f9pImageData.SKBitmap.Height, _xfImageSource, _sourceRangeLists != null);
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
                if (shadowPaint == null && TintColor != Xamarin.Forms.Color.Default && TintColor != Xamarin.Forms.Color.Transparent)
                {
                    var mx = new Single[]
                    {
                    0, 0, 0, 0, TintColor.ByteR(),
                    0, 0, 0, 0, TintColor.ByteG(),
                    0, 0, 0, 0, TintColor.ByteB(),
                    0, 0, 0, (float)TintColor.A, 0
                    };
                    var cf = SKColorFilter.CreateColorMatrix(mx);


                    paint = new SKPaint()
                    {
                        ColorFilter = cf,
                        IsAntialias = true,
                    };
                }
                else if (Opacity < 1.0)
                {
                    var transparency = SKColors.White.WithAlpha((byte)(Opacity * 255)); // 127 => 50%
                    paint = new SKPaint
                    {
                        ColorFilter = SKColorFilter.CreateBlendMode(transparency, SKBlendMode.DstIn)
                    };
                }
                //else
                //    paint = new SKPaint();

                var fillRectAspect = fillRect.Width / fillRect.Height;
                var bitmapAspect = SourceImageAspect;

                if (Fill == Fill.Tile)
                {
                    for (float x = fillRect.Left; x < fillRect.Right; x += _f9pImageData.SKBitmap.Width)
                        for (float y = fillRect.Top; y < fillRect.Bottom; y += _f9pImageData.SKBitmap.Height)
                            workingCanvas.DrawBitmap(_f9pImageData.SKBitmap, x, y, paint);
                }
                else if (rangeLists == null)
                {
                    var sourceRect = new SKRect(_f9pImageData.SKBitmap.Info.Rect.Left, _f9pImageData.SKBitmap.Info.Rect.Top, _f9pImageData.SKBitmap.Info.Rect.Right, _f9pImageData.SKBitmap.Info.Rect.Bottom);
                    var destRect = fillRect;
                    if (Fill == Fill.AspectFill)
                    {
                        var croppedWidth = bitmapAspect > fillRectAspect ? SourceImageHeight * fillRectAspect : SourceImageWidth;
                        var croppedHeight = bitmapAspect > fillRectAspect ? SourceImageHeight : SourceImageWidth / fillRectAspect;
                        float left;
                        switch (HorizontalOptions.Alignment)
                        {
                            case Xamarin.Forms.LayoutAlignment.Start:
                                left = 0;
                                break;
                            case Xamarin.Forms.LayoutAlignment.End:
                                left = (float)(SourceImageWidth - croppedWidth);
                                break;
                            default:
                                left = (float)(SourceImageWidth - croppedWidth) / 2.0f;
                                break;
                        }
                        float top;
                        switch (VerticalOptions.Alignment)
                        {
                            case Xamarin.Forms.LayoutAlignment.Start:
                                top = 0;
                                break;
                            case Xamarin.Forms.LayoutAlignment.End:
                                top = (float)(SourceImageHeight - croppedHeight);
                                break;
                            default:
                                top = (float)(SourceImageHeight - croppedHeight) / 2.0f;
                                break;
                        }
                        sourceRect = SKRect.Create(left, top, (float)croppedWidth, (float)croppedHeight);
                    }
                    else if (Fill == Fill.AspectFit)
                    {

                        //var destWidth = _imageElement.HorizontalOptions.Alignment != Xamarin.Forms.LayoutAlignment.Fill ? (bitmapAspect > fillRectAspect ? fillRect.Width : fillRect.Height * bitmapAspect) : fillRect.Width;
                        var destWidth = (bitmapAspect > fillRectAspect ? fillRect.Width : fillRect.Height * bitmapAspect);
                        //var destHeight = _imageElement.VerticalOptions.Alignment != Xamarin.Forms.LayoutAlignment.Fill ? (bitmapAspect > fillRectAspect ? fillRect.Width / bitmapAspect : fillRect.Height) : fillRect.Height;
                        var destHeight = (bitmapAspect > fillRectAspect ? fillRect.Width / bitmapAspect : fillRect.Height);

                        float left = fillRect.MidX - (float)destWidth / 2f, top = fillRect.MidY - (float)destHeight / 2f;
                        destRect = SKRect.Create(left, top, (float)destWidth, (float)destHeight);
                    }
                    //else Fill==Fill.Fill

                    if (AntiAlias && (destRect.Width > sourceRect.Width || destRect.Height > sourceRect.Height))
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
#if __IOS__
                    //System.Diagnostics.Debug.WriteLine("fillRect=[" + fillRect + "] size=[" + this.Frame.Width + ", " + this.Frame.Height + " ]");
#elif __DROID__
                    //System.Diagnostics.Debug.WriteLine("fillRect=[" + fillRect + "] size=[" + this.Width + ", " + this.Height + " ]");
#else
#endif
                    float xStretchable = 0;
                    foreach (var xpatch in rangeLists.PatchesX)
                        if (xpatch.Stretchable)
                            xStretchable += xpatch.Width;
                    var xExtra = fillRect.Width - ((float)SourceImageWidth - xStretchable);
                    float xScale = xExtra >= 0
                        ? xStretchable > 0 ? xExtra / xStretchable : fillRect.Width / (float)SourceImageWidth
                        : fillRect.Width / ((float)SourceImageWidth - xStretchable);
                    float yStretchable = 0;
                    foreach (var ypatch in rangeLists.PatchesY)
                        if (ypatch.Stretchable)
                            yStretchable += ypatch.Width;
                    var yExtra = fillRect.Height - ((float)SourceImageHeight - yStretchable);
                    float yScale = yExtra >= 0
                        ? yStretchable > 0 ? yExtra / yStretchable : fillRect.Height / (float)SourceImageHeight
                        : fillRect.Height / ((float)SourceImageHeight - yStretchable);
                    float patchX = 0, xPatchWidth;
                    foreach (var xpatch in rangeLists.PatchesX)
                    {
                        xPatchWidth = xExtra >= 0
                            ? xStretchable > 0 ? xpatch.Width * (xpatch.Stretchable ? xScale : 1) : xpatch.Width * xScale
                            : xpatch.Width * (xpatch.Stretchable ? 0 : xScale);
                        float patchY = 0, yPatchWidth;
                        foreach (var ypatch in rangeLists.PatchesY)
                        {
                            yPatchWidth = yExtra >= 0
                                ? yStretchable > 0 ? ypatch.Width * (ypatch.Stretchable ? yScale : 1) : ypatch.Width * yScale
                                : ypatch.Width * (ypatch.Stretchable ? 0 : yScale);
                            if (xPatchWidth > 0 && yPatchWidth > 0)
                            {
                                var sourceRect = new SKRect((float)System.Math.Max(0, xpatch.Start), (float)System.Math.Max(0, ypatch.Start), (float)System.Math.Min(xpatch.Start + xpatch.Width, SourceImageWidth), (float)System.Math.Min(ypatch.Start + ypatch.Width, SourceImageHeight));
                                var destRect = new SKRect((float)System.Math.Max(0, patchX), (float)System.Math.Max(0, patchY), Math.Min(patchX + xPatchWidth, fillRect.Width), Math.Min(patchY + yPatchWidth, fillRect.Height));
                                workingCanvas.DrawBitmap(_f9pImageData.SKBitmap, sourceRect, destRect, paint);
                            }
                            patchY += yPatchWidth;
                        }
                        patchX += xPatchWidth;
                    }

                    //var lattice = rangeLists.ToSKLattice(_f9pImageData.SKBitmap);
                    //System.Diagnostics.Debug.WriteLine("lattice.x: ["+lattice.XDivs[0]+","+lattice.XDivs[1]+"] lattice.y: ["+lattice.YDivs[0]+","+lattice.YDivs[1]+"] lattice.Bounds=["+lattice.Bounds+"] lattice.Flags:" + lattice.Flags);
                    //workingCanvas.DrawBitmapLattice(_f9pImageData.SKBitmap, lattice, fillRect);
                    //workingCanvas.DrawBitmapLattice(_f9pImageData.SKBitmap, new Int32[2] { 0, 1 }, new Int32[2] { 0, 1 }, fillRect, new SKPaint());
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

        protected virtual SKPath PerimeterPath(SKRect rect, float radius)
        {
            radius = Math.Max(radius, 0);

            //if (element is BubbleLayout bubble && bubble.PointerDirection != PointerDirection.None)
            //    return BubblePerimeterPath(bubble, rect, radius);

            Xamarin.Forms.StackOrientation orientation = !IsSegment ? Xamarin.Forms.StackOrientation.Horizontal : ((IExtendedShape)this).ParentSegmentsOrientation;

            var path = new SKPath();

            var length = Math.Min(rect.Width, rect.Height);
            if (((IExtendedShape)this).ExtendedElementShape == ExtendedElementShape.Square || ((IExtendedShape)this).ExtendedElementShape == ExtendedElementShape.Circle)
            {
                var location = new SKPoint(rect.MidX - length / 2f, rect.MidY - length / 2f);
                var size = new SKSize(length, length);
                rect = SKRect.Create(location, size);
            }
            else if (((IExtendedShape)this).ExtendedElementShape == ExtendedElementShape.Obround)
                radius = length / 2f;
            radius = Math.Min(radius, length / 2f);

            var diameter = radius * 2;
            var topLeft = new SKRect(rect.Left, rect.Top, rect.Left + diameter, rect.Top + diameter);
            var bottomLeft = new SKRect(rect.Left, rect.Bottom - diameter, rect.Left + diameter, rect.Bottom);
            var bottomRight = new SKRect(rect.Right - diameter, rect.Bottom - diameter, rect.Right, rect.Bottom);
            var topRight = new SKRect(rect.Right - diameter, rect.Top, rect.Right, rect.Top + diameter);


            switch (((IExtendedShape)this).ExtendedElementShape)
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
                    throw new NotSupportedException("ExtendedElementShape [" + ((IExtendedShape)this).ExtendedElementShape + "] is not supported")
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
                position = bubble.PointerDirection == PointerDirection.Down || bubble.PointerDirection == PointerDirection.Up
                    ? rect.Width * position
                    : rect.Height * position;
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
