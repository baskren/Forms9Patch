using System;
using System.ComponentModel;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using SkiaSharp;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch.Image element
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class Image : SKCanvasView, IImage, IImageController, IExtendedShape, IBubbleShape, IDisposable
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
        /// <param name="resourceId"></param>
        /// <param name="assembly"></param>
        public static void ClearEmbeddedResourceCache(string resourceId = null, Assembly assembly = null) => P42.Utils.EmbeddedResourceCache.Clear(resourceId, assembly, EmbeddedResourceImageCacheFolderName);
        #endregion

        #endregion


        #region Properties

        #region IImageController
        internal static readonly BindablePropertyKey IsLoadingPropertyKey = BindableProperty.CreateReadOnly(nameof(IsLoading), typeof(bool), typeof(Image), default(bool));

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
        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(Xamarin.Forms.ImageSource), typeof(Image), default(Xamarin.Forms.ImageSource));
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
        public static readonly BindableProperty FillProperty = BindableProperty.Create(nameof(Fill), typeof(Fill), typeof(Image), Fill.Fill);
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
        public static readonly BindableProperty CapInsetsProperty = BindableProperty.Create(nameof(CapInsets), typeof(Thickness), typeof(Image), new Thickness(-1));
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
        public static readonly BindableProperty ContentPaddingProperty = BindableProperty.Create(nameof(ContentPadding), typeof(Thickness), typeof(Image), new Thickness(-1));
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
        public static readonly BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(Image), Color.Default);
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
        /// <summary>
        /// Size of source image
        /// </summary>
        public Size SourceImageSize
        {
            get
            {
                return _f9pImageData != null && _f9pImageData.ValidImage
                    ? new Size(SourceImageWidth, SourceImageHeight)
                    : Size.Zero;
            }
        }

        /// <summary>
        /// Width of source image
        /// </summary>
        public double SourceImageWidth => _f9pImageData != null ? _f9pImageData.Width /* / FormsGestures.Display.Scale */: 0;
        /// <summary>
        /// Height of source Image
        /// </summary>
        public double SourceImageHeight => _f9pImageData != null ? _f9pImageData.Height /* / FormsGestures.Display.Scale */: 0;
        double SourceImageAspect => SourceImageHeight > 0 ? SourceImageWidth / SourceImageHeight : 1;
        #endregion SourceImageSize

        #region AntiAlias property
        /// <summary>
        /// backing store for AntiAlias property
        /// </summary>
        public static readonly BindableProperty AntiAliasProperty = BindableProperty.Create(nameof(AntiAlias), typeof(bool), typeof(Image), true);
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
        StackOrientation IExtendedShape.ExtendedElementShapeOrientation
        {
            get => (StackOrientation)GetValue(ExtendedElementShapeOrientationProperty);
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
        /// <summary>
        /// Backing store key for INTERNAL ExtendedElementSeparatorWidth property
        /// </summary>
        public static readonly BindableProperty ExtendedElementSeparatorWidthProperty = ShapeBase.ExtendedElementSeparatorWidthProperty;
        float IExtendedShape.ExtendedElementSeparatorWidth
        {
            get => (float)GetValue(ExtendedElementSeparatorWidthProperty);
            set => SetValue(ExtendedElementSeparatorWidthProperty, value);
        }
        #endregion ExtendedElementSeparatorWidth

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


        #region MakeRoomForShadow
        /// <summary>
        /// Backing store for ContentView.MakeRoomForShadow property
        /// </summary>
        internal static readonly BindableProperty InvisibleShadowProperty = BindableProperty.Create(nameof(InvisibleShadow), typeof(bool), typeof(Image), default);
        /// <summary>
        /// controls value of .MakeRoomForShadow property
        /// </summary>
        internal bool InvisibleShadow
        {
            get => (bool)GetValue(InvisibleShadowProperty);
            set => SetValue(InvisibleShadowProperty, value);
        }
        #endregion


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
        /// Backing store for the outline color property.
        /// </summary>
        public static readonly BindableProperty OutlineColorProperty = ShapeBase.OutlineColorProperty;
        /// <summary>
        /// Gets or sets the color of the outline.
        /// </summary>
        /// <value>The color of the outline.</value>
        public Color OutlineColor
        {
            get => (Color)GetValue(OutlineColorProperty);
            set => SetValue(OutlineColorProperty, value);
        }
        /// <summary>
        /// The boarder color property.
        /// </summary>
        public static readonly BindableProperty BorderColorProperty = ShapeBase.BorderColorProperty;
        /// <summary>
        /// Gets or sets the color of the boarder.
        /// </summary>
        /// <value>The color of the boarder.</value>
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }
        #endregion OutlineColor property

        #region OutlineRadius property
        /// <summary>
        /// Backing store for the outline radius property.
        /// </summary>
        public static readonly BindableProperty OutlineRadiusProperty = ShapeBase.OutlineRadiusProperty;
        /// <summary>
        /// Gets or sets the outline radius.
        /// </summary>
        /// <value>The outline radius.</value>
        public float OutlineRadius
        {
            get => (float)GetValue(OutlineRadiusProperty);
            set => SetValue(OutlineRadiusProperty, value);
        }
        /// <summary>
        /// The boarder radius property.
        /// </summary>
        public static readonly BindableProperty BorderRadiusProperty = ShapeBase.BorderRadiusProperty;
        /// <summary>
        /// Gets or sets the boarder radius.
        /// </summary>
        /// <value>The boarder radius.</value>
        public float BorderRadius
        {
            get => (float)GetValue(BorderRadiusProperty);
            set => SetValue(BorderRadiusProperty, value);
        }
        #endregion OutlineRadius property

        #region OutlineWidth property
        /// <summary>
        /// Backing store for the outline width property.
        /// </summary>
        public static readonly BindableProperty OutlineWidthProperty = ShapeBase.OutlineWidthProperty;
        /// <summary>
        /// Gets or sets the width of the outline.
        /// </summary>
        /// <value>The width of the outline.</value>
        public float OutlineWidth
        {
            get => (float)GetValue(OutlineWidthProperty);
            set => SetValue(OutlineWidthProperty, value);
        }
        /// <summary>
        /// The boarder width property.
        /// </summary>
        public static readonly BindableProperty BorderWidthProperty = ShapeBase.BorderWidthProperty;
        /// <summary>
        /// Gets or sets the width of the boarder.
        /// </summary>
        /// <value>The width of the boarder.</value>
        public float BorderWidth
        {
            get => (float)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
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


        #region IElement

        /// <summary>
        /// Returns index instance ID for this class (starts at 0)
        /// </summary>
        public int InstanceId { get; private set; }

        #endregion IElement

        #endregion IShape

        #endregion IExtendedShape

        #endregion IImage

        #region IBubbleShape Properties

        internal bool IsBubble { get; set; }

        #region PoinerLength property
        /// <summary>
        /// Backing store for pointer length property.
        /// </summary>
        public static readonly BindableProperty PointerLengthProperty = BubbleLayout.PointerLengthProperty; //  BindableProperty.Create("PointerLength", typeof(float), typeof(BubbleLayout), 4.0f);//, propertyChanged: UpdateBasePadding);
        /// <summary>
        /// Gets or sets the length of the bubble layout's pointer.
        /// </summary>
        /// <value>The length of the pointer.</value>
        float IBubbleShape.PointerLength
        {
            get => (float)GetValue(PointerLengthProperty);
            set => SetValue(PointerLengthProperty, value);
        }
        #endregion PoinerLength property

        #region PointerTipRadius property
        /// <summary>
        /// Backing store for pointer tip radius property.
        /// </summary>
        public static readonly BindableProperty PointerTipRadiusProperty = BubbleLayout.PointerTipRadiusProperty;
        /// <summary>
        /// Gets or sets the radius of the bubble's pointer tip.
        /// </summary>
        /// <value>The pointer tip radius.</value>
        float IBubbleShape.PointerTipRadius
        {
            get => (float)GetValue(PointerTipRadiusProperty);
            set => SetValue(PointerTipRadiusProperty, value);
        }
        #endregion PointerTipRadius property

        #region PointerAxialPosition property
        /// <summary>
        /// Backing store for pointer axial position property.
        /// </summary>
        public static readonly BindableProperty PointerAxialPositionProperty = BubbleLayout.PointerAxialPositionProperty;
        /// <summary>
        /// Gets or sets the position of the bubble's pointer along the face it's on.
        /// </summary>
        /// <value>The pointer axial position (left/top is zero).</value>
        float IBubbleShape.PointerAxialPosition
        {
            get => (float)GetValue(PointerAxialPositionProperty);
            set => SetValue(PointerAxialPositionProperty, value);
        }
        #endregion PointerAxialPosition property

        #region PointerDirection property
        /// <summary>
        /// Backing store for pointer direction property.
        /// </summary>
        public static readonly BindableProperty PointerDirectionProperty = BubbleLayout.PointerDirectionProperty;
        /// <summary>
        /// Gets or sets the direction in which the pointer pointing.
        /// </summary>
        /// <value>The pointer direction.</value>
        PointerDirection IBubbleShape.PointerDirection
        {
            get => (PointerDirection)GetValue(PointerDirectionProperty);
            set => SetValue(PointerDirectionProperty, value);
        }
        #endregion PointerDirection property

        #region PointerCornerRadius property
        /// <summary>
        /// The pointer corner radius property.
        /// </summary>
        public static readonly BindableProperty PointerCornerRadiusProperty = BubbleLayout.PointerCornerRadiusProperty;
        /// <summary>
        /// Gets or sets the pointer corner radius.
        /// </summary>
        /// <value>The pointer corner radius.</value>
        float IBubbleShape.PointerCornerRadius
        {
            get => (float)GetValue(PointerCornerRadiusProperty);
            set => SetValue(PointerCornerRadiusProperty, value);
        }
        #endregion PointerCornerRadius property

        #endregion IBubbleShape properties

        #endregion Properties


        #region Fields and Private Properties
        internal bool FillOrLayoutSet;
        static int _instances;
        Xamarin.Forms.ImageSource _xfImageSource;

        F9PImageData _f9pImageData;
        RangeLists _sourceRangeLists;
        DateTime _lastPaint = DateTime.MaxValue;
        bool _repainting;


        bool DrawImage => (Opacity > 0 && _f9pImageData != null && _f9pImageData.ValidImage);

        bool DrawOutline => (OutlineColor.A > 0.01 && OutlineWidth > 0.05);

        bool DrawFill => BackgroundColor.A > 0.01;

        bool IsSegment => ((IExtendedShape)this).ExtendedElementShape == ExtendedElementShape.SegmentEnd || ((IExtendedShape)this).ExtendedElementShape == ExtendedElementShape.SegmentMid || ((IExtendedShape)this).ExtendedElementShape == ExtendedElementShape.SegmentStart;

        #region FailAction
        /// <summary>
        /// Backing store for Image FailAction property
        /// </summary>
        public static readonly BindableProperty FailActionProperty = BindableProperty.Create(nameof(FailAction), typeof(FailAction), typeof(Image), FailAction.ShowAlert);
        /// <summary>
        /// controls value of Image FailAction property
        /// </summary>
        public FailAction FailAction
        {
            get => (FailAction)GetValue(FailActionProperty);
            set => SetValue(FailActionProperty, value);
        }
        #endregion

        #endregion


        #region Constructors
        static Image()
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Image()
        {
            P42.Utils.DebugExtensions.AddToCensus(this);

            InstanceId = _instances++;
            if (Device.RuntimePlatform != Device.iOS)
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
                {
                    Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if ((DateTime.Now - _lastPaint) is TimeSpan elapsed && elapsed > TimeSpan.FromMilliseconds(100))
                        {
                            _repainting = true;
                            _lastPaint = DateTime.MaxValue;
                            Invalidate();
                        }
                    });
                    return false;
                });
            }
        }

        /// <summary>
        /// Instantiates a new instance of Image from an MultiResource embedded resource
        /// </summary>
        /// <param name="embeddedResourceId">EmbeddedResourceId for image</param>
        /// <param name="assembly">Assembly in which embedded resource is embedded</param>
        public Image(string embeddedResourceId, Assembly assembly = null) : this()
        {
            if (assembly == null && Device.RuntimePlatform != Device.UWP)
                assembly = assembly ?? Assembly.GetCallingAssembly();
            Source = ImageSource.FromMultiResource(embeddedResourceId, assembly);
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
            P42.Utils.DebugExtensions.AddToCensus(this);

            InstanceId = _instances++;
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
            P42.Utils.DebugExtensions.AddToCensus(this);

            InstanceId = _instances++;
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

        private bool _disposed;
        /// <summary>
        /// Disposed the image
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                Source = null;
                _f9pImageData?.Dispose();
                _f9pImageData = null;
                _sourceRangeLists = null;

                P42.Utils.DebugExtensions.RemoveFromCensus(this);
            }
        }

        /// <summary>
        /// Dispose the image
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


        #region Conversion Operators
        /// <summary>
        /// Converts Xamarin.Forms.Image to Forms9Patch.Image
        /// </summary>
        /// <param name="xamarinFormsImage"></param>
        public static implicit operator Image(Xamarin.Forms.Image xamarinFormsImage)
        {
            return new Image(xamarinFormsImage);
        }

        /// <summary>
        /// Converts string (embedded resource id) into a Forms9Patch.Image
        /// </summary>
        /// <param name="embeddedResourceId"></param>
        public static implicit operator Image(string embeddedResourceId)
        {
            var assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, Array.Empty<object>());
            var result = new Image(embeddedResourceId, assembly);
            return result;
        }
        #endregion


        #region Property Change Handlers
        bool invalidating = false;
        bool pendingInvalidate = false;
        void Invalidate()
        {
            if (invalidating)
            {
                pendingInvalidate = true;
                System.Diagnostics.Debug.WriteLine("pending");
                return; 
            }
            invalidating = true;
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                InvalidateMeasure();
                InvalidateSurface();
                invalidating = false;
                if (pendingInvalidate)
                {
                    pendingInvalidate = false;
                    Invalidate();
                }
            });
        }

        async Task SetImageSourceAsync()
        {
            if (Source != _xfImageSource)
            {
                // release the previous
                if (_xfImageSource != null)
                {
                    _xfImageSource.ReleaseF9PBitmap(this);
                    _xfImageSource.PropertyChanged -= OnImageSourcePropertyChanged;
                }

                _f9pImageData = null;
                _sourceRangeLists = null;

                ((IImageController)this)?.SetIsLoading(true);
                _xfImageSource = Source;

                if (_xfImageSource != null)
                {
                    _xfImageSource.PropertyChanged += OnImageSourcePropertyChanged;
                    _f9pImageData = await _xfImageSource.FetchF9pImageData(this, failAction: FailAction);
                    _sourceRangeLists = _f9pImageData?.RangeLists;
                }
                ((IImageController)this)?.SetIsLoading(false);
                Invalidate();
            }
        }

        private async void OnImageSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == UriImageSource.UriProperty.PropertyName)
            {
                ((IImageController)this)?.SetIsLoading(true);

                _f9pImageData = await _xfImageSource.FetchF9pImageData(this, failAction: FailAction);
                _sourceRangeLists = _f9pImageData?.RangeLists;
                ((IImageController)this)?.SetIsLoading(false);
                Invalidate();
            }
        }

        /// <summary>
        /// Called when the binding contex has changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (_xfImageSource != null)
            {
                _xfImageSource.BindingContext = BindingContext;
                if (Device.RuntimePlatform == Device.Android)
                    Invalidate();
            }
        }


        /// <summary>
        /// Called when a property has changed
        /// </summary>
        /// <param name="propertyName"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0022:A catch clause that catches System.Exception and has an empty body", Justification = "<Pending>")]
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    base.OnPropertyChanged(propertyName);
                }
                catch (Exception) { }

                if (propertyName == ElementShapeProperty.PropertyName)
                    ((IExtendedShape)this).ExtendedElementShape = ElementShape.ToExtendedElementShape();
                if (propertyName == SourceProperty.PropertyName)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    SetImageSourceAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                if (
                    // VisualElement
                    propertyName == BackgroundColorProperty.PropertyName ||
                    propertyName == OpacityProperty.PropertyName ||

                    // ShapeBase
                    propertyName == ElementShapeProperty.PropertyName ||
                    propertyName == HasShadowProperty.PropertyName ||
                    propertyName == InvisibleShadowProperty.PropertyName ||
                    propertyName == OutlineColorProperty.PropertyName ||
                    propertyName == OutlineRadiusProperty.PropertyName ||
                    propertyName == OutlineWidthProperty.PropertyName ||
                    propertyName == ShadowInvertedProperty.PropertyName ||

                    // BubbleLayout
                    propertyName == PointerAxialPositionProperty.PropertyName ||
                    propertyName == PointerCornerRadiusProperty.PropertyName ||
                    propertyName == PointerDirectionProperty.PropertyName ||
                    propertyName == PointerLengthProperty.PropertyName ||
                    propertyName == PointerTipRadiusProperty.PropertyName ||


                    // Image
                    //propertyName == SourceProperty.PropertyName ||  // Handled by SetImageSource()
                    propertyName == TintColorProperty.PropertyName ||
                    propertyName == FillProperty.PropertyName ||
                    propertyName == CapInsetsProperty.PropertyName ||
                    propertyName == AntiAliasProperty.PropertyName ||

                    // IExtendedElementShape
                    propertyName == ExtendedElementShapeProperty.PropertyName ||
                    propertyName == ExtendedElementShapeOrientationProperty.PropertyName ||
                    propertyName == ExtendedElementSeparatorWidthProperty.PropertyName
                   )
                {
                    Invalidate();
                }
            });
        }
        #endregion


        #region Measurement
        /// <summary>
        /// Internal use only
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        [Obsolete("Ugh")]
        public override SizeRequest GetSizeRequest(double widthConstraint, double heightConstraint)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "GetSizeRequest(" + widthConstraint + ", " + heightConstraint + ") ENTER");
#pragma warning disable CS0618 // Type or member is obsolete
            var result = base.GetSizeRequest(widthConstraint, heightConstraint);
#pragma warning restore CS0618 // Type or member is obsolete

            var reqW = result.Request.Width;
            var reqH = result.Request.Height;
            if (_f9pImageData != null && _f9pImageData.Height > 0 && _f9pImageData.Width > 0 && (!HorizontalOptions.Expands || !VerticalOptions.Expands))
            {
                //System.Diagnostics.Debug.WriteLine(GetType() + "GetSizeRequest ImageData.Size=[" + _f9pImageData.Width + ", " + _f9pImageData.Height + "]");
                if (!HorizontalOptions.Expands)
                    reqW = _f9pImageData.Width / Display.Scale;
                if (!VerticalOptions.Expands)
                    reqH = _f9pImageData.Height / Display.Scale;
            }

            var shadowPaddingHz = 0.0;
            var shadowPaddingVt = 0.0;
            if (HasShadow)
            {
                var shadow = ShapeBase.ShadowPadding(this);
                shadowPaddingHz = shadow.HorizontalThickness;
                shadowPaddingVt = shadow.VerticalThickness;
            }


            if (WidthRequest > 0)
                reqW = WidthRequest;
            if (HeightRequest > 0)
                reqH = HeightRequest;
            //result = new SizeRequest(new Size(reqW + shadowPaddingHz, reqH + shadowPaddingVt), new Size(10 + shadowPaddingHz, 10 + shadowPaddingVt));
            var reqSize = new Size(reqW + shadowPaddingHz, reqH + shadowPaddingVt);
            var minSize = new Size(WidthRequest > 0 ? reqSize.Width : 10 + shadowPaddingHz, HeightRequest > 0 ? reqSize.Height : 10 + shadowPaddingVt);
            return new SizeRequest(reqSize, minSize);
        }

        /*
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (Math.Abs(widthConstraint - 768) < 0.00001 && Math.Abs(heightConstraint - 74.9624060150376) < 0.00001)
                System.Diagnostics.Debug.WriteLine("");

            System.Diagnostics.Debug.WriteLine(GetType() + ".OnMeasure(" + widthConstraint + ", " + heightConstraint + ") ENTER");
            var result = base.OnMeasure(widthConstraint, heightConstraint);
            System.Diagnostics.Debug.WriteLine(GetType() + ".OnMeasure(" + widthConstraint + ", " + heightConstraint + ") = [" + result + "]");
            return result;
        }
        */
        #endregion


        #region Painting
        /// <summary>
        /// Internal use only
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            if (e.Surface?.Canvas == null)
                return;

            e.Surface.Canvas.Clear();
            SharedOnPaintSurface(e, e.Info.Rect);
        }

        internal void SharedOnPaintSurface(SKPaintSurfaceEventArgs e, SKRect rect)
        {
            var canvas = e.Surface?.Canvas;
            //canvas.ClipRect(rect, SKClipOperation.Intersect, false);

            var clipBounds = canvas.LocalClipBounds;

            //System.Diagnostics.Debug.WriteLine("SharedOnPaintImage[" + InstanceId + "] rect=[" + rect + "]\t clipBounds=[" + clipBounds + "]");

            if (canvas == null)
                return;

            var hz = ((IExtendedShape)this).ExtendedElementShapeOrientation == StackOrientation.Horizontal;
            var vt = !hz;

            var backgroundColor = BackgroundColor;
            var hasShadow = HasShadow;
            var shadowInverted = ShadowInverted;
            var outlineWidth = OutlineWidth * Display.Scale;
            var outlineRadius = OutlineRadius * Display.Scale;
            var outlineColor = OutlineColor;
            var elementShape = ((IExtendedShape)this).ExtendedElementShape;

            var separatorWidth = IsSegment ? ((IExtendedShape)this).ExtendedElementSeparatorWidth * Display.Scale : 0;
            if (separatorWidth < 0)
                separatorWidth = outlineWidth;
            if (outlineColor.A <= 0.01)
                separatorWidth = 0;

            var drawOutline = DrawOutline;
            var drawImage = DrawImage;
            var drawFill = DrawFill;

            if ((drawFill || drawOutline || separatorWidth > 0 || drawImage))// && CanvasSize != default)
            {

                //SKRect rect = new SKRect(0, 0, info.Width, info.Height);
                //System.Diagnostics.Debug.WriteLine("Image.OnPaintSurface rect=" + rect);

                var makeRoomForShadow = hasShadow && (backgroundColor.A > 0.01 || drawImage); // && !ShapeElement.ShadowInverted;
                var shadowX = (float)(Settings.ShadowOffset.X * FormsGestures.Display.Scale);
                var shadowY = (float)(Settings.ShadowOffset.Y * FormsGestures.Display.Scale);
                var shadowR = (float)(Settings.ShadowRadius * FormsGestures.Display.Scale);
                var shadowColor = Color.FromRgba(0.0, 0.0, 0.0, InvisibleShadow ? 0.0 : 0.75).ToSKColor(); //  .ToWindowsColor().ToSKColor();
                var shadowPadding = ShapeBase.ShadowPadding(this, true);


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
                        canvas.Save();

                        var allowance = Math.Abs(shadowX) + Math.Abs(shadowY) + Math.Abs(shadowR);
                        var shadowRect = perimeter;

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

                        using (var shadowPaint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = shadowColor
                        })
                        {

                            //var filter = SKImageFilter.CreateDropShadow(shadowX, shadowY, shadowR / 2, shadowR / 2, shadowColor, SKDropShadowImageFilterShadowMode.DrawShadowOnly);
                            var filter = SKImageFilter.CreateDropShadowOnly(shadowX, shadowY, shadowR / 2, shadowR / 2, shadowColor);
                            shadowPaint.ImageFilter = filter;
                            //var filter = SkiaSharp.SKMaskFilter.CreateBlur(SKBlurStyle.Outer, 0.5f);
                            //shadowPaint.MaskFilter = filter;

                            try
                            {
                                var region = new SKRegion();
                                region.SetRect(new SKRectI((int)rect.Left, (int)rect.Top, (int)rect.Right, (int)rect.Bottom));
                                canvas.ClipRegion(region);

                                using (var pPath = PerimeterPath(shadowRect, outlineRadius - (drawOutline ? outlineWidth : 0)))
                                {
                                    if (DrawFill)
                                        canvas.DrawPath(pPath, shadowPaint);
                                    else if (DrawImage)
                                        GenerateImageLayout(canvas, perimeter, pPath, shadowPaint);
                                }
                            }
                            catch (Exception exception)
                            {
                                var properties = new Dictionary<string, string>
                            {
                                { nameof(shadowRect), JsonConvert.SerializeObject(shadowRect) },
                                { "class", "Forms9Patch.ExtendedShapeAndImageView" },
                                { "method", "Paint" },
                                { "line", "888" }
                            };
                                //Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, properties);
                                Analytics.TrackException?.Invoke(exception, properties);
                                _repainting = false;
                                return;
                            }
                            canvas.Restore();
                        }
                    }
                }

                if (drawFill)
                {
                    var fillRect = RectInsetForShape(perimeter, outlineWidth, vt, separatorWidth);
                    using (var path = PerimeterPath(fillRect, outlineRadius - (drawOutline ? outlineWidth : 0)))
                    {
                        using (var fillPaint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = backgroundColor.ToSKColor(),
                            IsAntialias = true
                        })
                        {
                            try
                            {
                                canvas.DrawPath(path, fillPaint);
                            }
                            catch (Exception exception)
                            {
                                var properties = new Dictionary<string, string>
                            {
                                { "class", "Forms9Patch.ExtendedShapeAndImageView" },
                                { "method", "Paint" },
                                { "line", "917" }
                            };
                                //Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, properties);
                                Analytics.TrackException?.Invoke(exception, properties);
                                _repainting = false;
                                return;
                            }
                        }
                    }
                }


                if (drawImage)
                {
                    var imagePerimeter = perimeter;
                    if (drawFill)
                        imagePerimeter = RectInsetForShape(perimeter, outlineWidth, vt, separatorWidth);
                    using (var path = PerimeterPath(imagePerimeter, outlineRadius - (drawOutline ? outlineWidth : 0)))
                    {
                        try
                        {
                            GenerateImageLayout(canvas, perimeter, path);
                        }
                        catch (Exception exception)
                        {
                            var properties = new Dictionary<string, string>
                            {
                                { "class", "Forms9Patch.ExtendedShapeAndImageView" },
                                { "method", "Paint" },
                                { "line", "941" }
                            };
                            //Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, properties);
                            Analytics.TrackException?.Invoke(exception, properties);
                            _repainting = false;
                            return;
                        }
                    }
                }

                if (drawOutline)// && !drawImage)
                {
                    using (var outlinePaint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = outlineColor.ToSKColor(),
                        StrokeWidth = outlineWidth,
                        IsAntialias = true
                        //StrokeJoin = SKStrokeJoin.Bevel
                        //PathEffect = SKPathEffect.CreateDash(new float[] { 20,20 }, 0)
                    })
                    {
                        var intPerimeter = new SKRect((int)perimeter.Left, (int)perimeter.Top, (int)perimeter.Right, (int)perimeter.Bottom);
                        //System.Diagnostics.Debug.WriteLine("perimeter=[" + perimeter + "] [" + intPerimeter + "]");
                        var outlineRect = RectInsetForShape(intPerimeter, outlineWidth / 2, vt, separatorWidth);
                        using (var path = PerimeterPath(outlineRect, outlineRadius - (drawOutline ? outlineWidth / 2 : 0), true))
                        {
                            try
                            {
                                canvas.DrawPath(path, outlinePaint);
                            }
                            catch (Exception exception)
                            {
                                var properties = new Dictionary<string, string>
                            {
                                { "class", "Forms9Patch.ExtendedShapeAndImageView" },
                                { "method", "Paint" },
                                { "line", "974" }
                            };
                                //Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, properties);
                                Analytics.TrackException?.Invoke(exception, properties);
                                _repainting = false;
                                return;
                            }
                        }
                    }
                }


                if (separatorWidth > 0 && (elementShape == ExtendedElementShape.SegmentMid || elementShape == ExtendedElementShape.SegmentEnd))
                {
                    //System.Diagnostics.Debug.WriteLine("SeparatorColor: " + outlineColor.R + ", " + outlineColor.G + ", " + outlineColor.B + ", " + outlineColor.A);
                    //System.Diagnostics.Debug.WriteLine("SeparatorWidth: " + separatorWidth);
                    using (var separatorPaint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = outlineColor.ToSKColor(),
                        StrokeWidth = separatorWidth,
                        IsAntialias = true
                        //PathEffect = SKPathEffect.CreateDash(new float[] { 20,20 }, 0)
                    })
                    {
                        using (var path = new SKPath())
                        {
                            if (vt)
                            {
                                path.MoveTo(perimeter.Left, perimeter.Top + outlineWidth / 2);
                                path.LineTo(perimeter.Right, perimeter.Top + outlineWidth / 2);
                            }
                            else
                            {
                                path.MoveTo(perimeter.Left + outlineWidth / 2, perimeter.Top);
                                path.LineTo(perimeter.Left + outlineWidth / 2, perimeter.Bottom);
                            }
                            try
                            {
                                canvas.DrawPath(path, separatorPaint);
                            }
                            catch (Exception exception)
                            {
                                var properties = new Dictionary<string, string>
                            {
                                { "class", "Forms9Patch.ExtendedShapeAndImageView" },
                                { "method", "Paint" },
                                { "line", "1017" }
                            };
                                //Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, properties);
                                Analytics.TrackException?.Invoke(exception, properties);
                                _repainting = false;
                                return;
                            }
                        }
                    }
                }


                if (makeRoomForShadow && shadowInverted)
                {
                    canvas.Save();

                    // setup the paint
                    using (var insetShadowPaint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = shadowColor,
                        IsAntialias = true
                    })
                    {
                        //var filter = SKImageFilter.CreateDropShadow(shadowX, shadowY, shadowR / 2, shadowR / 2, shadowColor, SKDropShadowImageFilterShadowMode.DrawShadowOnly);
                        var filter = SKImageFilter.CreateDropShadowOnly(shadowX, shadowY, shadowR / 2, shadowR / 2, shadowColor);
                        insetShadowPaint.ImageFilter = filter;

                        // what is the mask?
                        using (var maskPath = PerimeterPath(perimeter, outlineRadius))
                        {
                            canvas.ClipPath(maskPath);

                            // what is the path that will cast the shadow?
                            // a) the button portion (which will be the hole in the larger outline, b)
                            var shadowRect = InverseShadowInsetForShape(perimeter, vt);
                            using (var path = PerimeterPath(shadowRect, outlineRadius))
                            {
                                // b) add to it the larger outline 
                                path.AddRect(RectInset(rect, -50));
                                try
                                {
                                    canvas.DrawPath(path, insetShadowPaint);
                                }
                                catch (Exception exception)
                                {
                                    var properties = new Dictionary<string, string>
                            {
                                { "class", "Forms9Patch.ExtendedShapeAndImageView" },
                                { "method", "Paint" },
                                { "line", "917" }
                            };
                                    //Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, properties);
                                    Analytics.TrackException?.Invoke(exception, properties);
                                }
                            }
                        }
                    }
                    canvas.Restore();
                }
            }

            if (!_repainting)
                _lastPaint = DateTime.Now;
            _repainting = false;
        }
        #endregion


        #region Image Layout
        void GenerateImageLayout(SKCanvas canvas, SKRect fillRect, SKPath clipPath, SKPaint shadowPaint = null)
        {
            if (_f9pImageData == null || !_f9pImageData.ValidImage)
                return;

            SKCanvas shadowCanvas = null;
            SKBitmap shadowBitmap = null;
            var workingCanvas = canvas;

            if (shadowPaint != null)
            {
                var x = canvas.DeviceClipBounds;
                shadowBitmap = new SKBitmap(x.Width, x.Height);
                shadowCanvas = new SKCanvas(shadowBitmap);
                workingCanvas = shadowCanvas;
                workingCanvas.Clear();
            }

            if (_f9pImageData.ValidSVG)
            {
                workingCanvas.Save();
                if (clipPath != null)
                    workingCanvas.ClipPath(clipPath);

                if (Fill == Fill.Tile)
                {
                    if (SourceImageWidth > 0 && SourceImageHeight > 0)
                    {
                        for (float x = 0; x < fillRect.Width; x += (float)SourceImageWidth)
                            for (float y = 0; y < fillRect.Height; y += (float)SourceImageHeight)
                            {
                                workingCanvas.ResetMatrix();
                                workingCanvas.Translate(x, y);
                                workingCanvas.DrawPicture(_f9pImageData.SKSvg.Picture);
                            }
                        workingCanvas.Restore();
                        if (shadowCanvas != workingCanvas)
                            shadowCanvas?.Dispose();
                        return;
                    }
                }

                var fillRectAspect = fillRect.Width / fillRect.Height;
                var imageAspect = (SourceImageWidth < 1 || SourceImageHeight < 1) ? 1 : SourceImageWidth / SourceImageHeight;
                double scaleX;
                double scaleY;
                if (SourceImageWidth <= 0 || SourceImageHeight <= 0)
                {
                    scaleX = scaleY = 1;
                    Console.WriteLine("Cannot tile, scale or justify an SVG image with zero or negative Width or Height. Verify, in the SVG source, that the x, y, width, height, and viewBox attributes of the <SVG> tag are present and set correctly.");
                }
                else if (Fill == Fill.AspectFill)
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

                float left = 0;
                float top = 0;
                if (SourceImageWidth > 0 && SourceImageHeight > 0)
                {
                    switch (HorizontalOptions.Alignment)
                    {
                        case LayoutAlignment.Start:
                            left = 0;
                            break;
                        case LayoutAlignment.End:
                            left = (float)(fillRect.Width - scaledWidth);
                            break;
                        default:
                            left = (float)(fillRect.Width - scaledWidth) / 2.0f;
                            break;
                    }
                    switch (VerticalOptions.Alignment)
                    {
                        case LayoutAlignment.Start:
                            top = 0;
                            break;
                        case LayoutAlignment.End:
                            top = (float)(fillRect.Height - scaledHeight);
                            break;
                        default:
                            top = (float)(fillRect.Height - scaledHeight) / 2.0f;
                            break;
                    }
                }
                var shadowPadding = ShapeBase.ShadowPadding(this, true);
                workingCanvas.Translate(left + (float)shadowPadding.Left, top + (float)shadowPadding.Top);
                workingCanvas.Scale((float)scaleX, (float)scaleY);
                SKPaint paint = null;
                if (shadowPaint == null && TintColor != Color.Default && TintColor != Color.Transparent)
                {
                    var color = new SKColor(TintColor.ByteR(), TintColor.ByteG(), TintColor.ByteB(), TintColor.ByteA());
                    var cf = SKColorFilter.CreateBlendMode(color, SKBlendMode.SrcIn);

                    paint = new SKPaint
                    {
                        ColorFilter = cf,
                        IsAntialias = true
                    };
                }
                else if (Opacity < 1.0)
                {
                    var transparency = SKColors.White.WithAlpha((byte)(Opacity * 255)); // 127 => 50%
                    paint = new SKPaint { ColorFilter = SKColorFilter.CreateBlendMode(transparency, SKBlendMode.DstIn) };
                }
                workingCanvas.DrawPicture(_f9pImageData.SKSvg.Picture, paint);
                workingCanvas.Restore();
            }
            else if (_f9pImageData.ValidBitmap)
            {
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

                //var bitmap = _f9pImageData;
                SKPaint paint = null;
                if (shadowPaint == null && TintColor != Color.Default && TintColor != Color.Transparent)
                {
                    var mx = new[]
                    {
                        0, 0, 0, 0, TintColor.ByteR(),
                        0, 0, 0, 0, TintColor.ByteG(),
                        0, 0, 0, 0, TintColor.ByteB(),
                        0, 0, 0, (float)(TintColor.A * Opacity), 0
                    };
                    var cf = SKColorFilter.CreateColorMatrix(mx);


                    paint = new SKPaint
                    {
                        ColorFilter = cf,
                        IsAntialias = true
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

                var shadowPadding = ShapeBase.ShadowPadding(this, true);

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
                            case LayoutAlignment.Start:
                                left = 0;
                                break;
                            case LayoutAlignment.End:
                                left = (float)(SourceImageWidth - croppedWidth);
                                break;
                            default:
                                left = (float)(SourceImageWidth - croppedWidth) / 2.0f;
                                break;
                        }
                        float top;
                        switch (VerticalOptions.Alignment)
                        {
                            case LayoutAlignment.Start:
                                top = 0;
                                break;
                            case LayoutAlignment.End:
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

                        var left = fillRect.MidX - (float)destWidth / 2f;
                        var top = fillRect.MidY - (float)destHeight / 2f;
                        destRect = SKRect.Create(left, top, (float)destWidth, (float)destHeight);
                    }
                    //else Fill==Fill.Fill

                    if (AntiAlias && (destRect.Width > sourceRect.Width || destRect.Height > sourceRect.Height))
                    {
                        var croppedBitmap = new SKBitmap((int)sourceRect.Width, (int)sourceRect.Height);
                        var destBitmap = new SKBitmap((int)destRect.Width, (int)destRect.Height);
                        _f9pImageData.SKBitmap.ExtractSubset(croppedBitmap, new SKRectI((int)sourceRect.Left, (int)sourceRect.Top, (int)sourceRect.Right, (int)sourceRect.Bottom));
                        //SKBitmap.Resize(destBitmap, croppedBitmap, SKBitmapResizeMethod.Lanczos3);
                        croppedBitmap.ScalePixels(destBitmap, SKFilterQuality.Medium);
                        workingCanvas.DrawBitmap(destBitmap, destBitmap.Info.Rect, destRect, paint);
                    }
                    else
                    {
                        workingCanvas.DrawBitmap(_f9pImageData.SKBitmap, sourceRect, destRect, paint);
                    }
                }
                else
                {
                    float xStretchable = 0;
                    foreach (var xpatch in rangeLists.PatchesX)
                        if (xpatch.Stretchable)
                            xStretchable += xpatch.Width;
                    var xExtra = fillRect.Width - ((float)SourceImageWidth - xStretchable);
                    var xScale = xExtra >= 0
                        ? xStretchable > 0
                            ? xExtra / xStretchable
                            : fillRect.Width / (float)SourceImageWidth
                        : fillRect.Width / ((float)SourceImageWidth - xStretchable);
                    float yStretchable = 0;
                    foreach (var ypatch in rangeLists.PatchesY)
                        if (ypatch.Stretchable)
                            yStretchable += ypatch.Width;
                    var yExtra = fillRect.Height - ((float)SourceImageHeight - yStretchable);
                    var yScale = yExtra >= 0
                        ? yStretchable > 0
                            ? yExtra / yStretchable
                            : fillRect.Height / (float)SourceImageHeight
                        : fillRect.Height / ((float)SourceImageHeight - yStretchable);
                    float patchX = (float)shadowPadding.Left, xPatchWidth;
                    foreach (var xpatch in rangeLists.PatchesX)
                    {
                        xPatchWidth = xExtra >= 0
                            ? xStretchable > 0
                                ? xpatch.Width * (xpatch.Stretchable ? xScale : 1)
                                : xpatch.Width * xScale
                            : xpatch.Width * (xpatch.Stretchable ? 0 : xScale);
                        float patchY = (float)shadowPadding.Top, yPatchWidth;
                        foreach (var ypatch in rangeLists.PatchesY)
                        {
                            yPatchWidth = yExtra >= 0
                                ? yStretchable > 0 ? ypatch.Width * (ypatch.Stretchable ? yScale : 1) : ypatch.Width * yScale
                                : ypatch.Width * (ypatch.Stretchable ? 0 : yScale);
                            if (xPatchWidth > 0 && yPatchWidth > 0)
                            {
                                var sourceRect = new SKRect(Math.Max(0, xpatch.Start), Math.Max(0, ypatch.Start), (float)Math.Min(xpatch.Start + xpatch.Width, SourceImageWidth), (float)Math.Min(ypatch.Start + ypatch.Width, SourceImageHeight));
                                var destRect = new SKRect(Math.Max(0, patchX), Math.Max(0, patchY), Math.Min(patchX + xPatchWidth, fillRect.Width + (float)shadowPadding.Left), Math.Min(patchY + yPatchWidth, fillRect.Height + (float)shadowPadding.Top));
                                workingCanvas.DrawBitmap(_f9pImageData.SKBitmap, sourceRect, destRect, paint);
                            }
                            patchY += yPatchWidth;
                        }
                        patchX += xPatchWidth;
                    }

                    //var lattice = rangeLists.ToSKLattice(_f9pImageData.SKBitmap);
                    //workingCanvas.DrawBitmapLattice(_f9pImageData.SKBitmap, lattice, fillRect);
                    //workingCanvas.DrawBitmapLattice(_f9pImageData.SKBitmap, new Int32[2] { 0, 1 }, new Int32[2] { 0, 1 }, fillRect, new SKPaint());
                }

                workingCanvas.Restore();

                if (shadowPaint != null)
                    canvas.DrawBitmap(shadowBitmap, shadowBitmap.Info.Rect, shadowPaint);

                paint?.Dispose();
            }
            else
                Console.WriteLine("Image [" + _f9pImageData.Key + "] is neither a valid SVG or valid Bitmap.");

            if (shadowCanvas != workingCanvas)
                shadowCanvas?.Dispose();
        }
        #endregion


        #region layout support 

        SKRect InverseShadowInsetForShape(SKRect perimeter, bool vt)
        {
            var inset = -20.0f;
            SKRect result;
            var hz = !vt;
            switch (((IExtendedShape)this).ExtendedElementShape)
            {
                case ExtendedElementShape.SegmentStart:
                    result = RectInset(perimeter, 0, 0, (vt ? 0 : inset), (hz ? 0 : inset));
                    break;
                case ExtendedElementShape.SegmentMid:
                    result = RectInset(perimeter, (vt ? 0 : inset), (hz ? 0 : inset), (vt ? 0 : inset), (hz ? 0 : inset));
                    break;
                case ExtendedElementShape.SegmentEnd:
                    result = RectInset(perimeter, (vt ? 0 : inset), (hz ? 0 : inset), 0, 0);
                    break;
                default:
                    result = perimeter;
                    break;
            }
            return result;
        }


        SKRect RectInsetForShape(SKRect perimeter, float inset, bool vt, float separatorWidth)
        {
            if (inset < 0)
                inset = 0;
            SKRect result;
            var hz = !vt;



            switch (((IExtendedShape)this).ExtendedElementShape)
            {
                case ExtendedElementShape.SegmentStart:
                    result = RectInset(perimeter, inset, inset, (hz ? 0 : inset), (vt ? 0 : inset));
                    break;
                case ExtendedElementShape.SegmentMid:
                    result = RectInset(perimeter, (hz ? separatorWidth / 2 : inset), (vt ? separatorWidth / 2 : inset), (hz ? 0 : inset), (vt ? 0 : inset));
                    break;
                case ExtendedElementShape.SegmentEnd:
                    result = RectInset(perimeter, (hz ? separatorWidth / 2 : inset), (vt ? separatorWidth / 2 : inset), inset, inset);
                    break;
                default:
                    result = RectInset(perimeter, inset);
                    break;
            }
            return result;
        }

        /*
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
        */

        //static SKRect RectInset(SKRect rect, double inset) => RectInset(rect, (float)inset);

        static SKRect RectInset(SKRect rect, float inset) => RectInset(rect, inset, inset, inset, inset);

        //static SKRect RectInset(SKRect rect, double left, double top, double right, double bottom) => RectInset(rect, (float)left, (float)top, (float)right, (float)bottom);

        static SKRect RectInset(SKRect rect, float left, float top, float right, float bottom) => new SKRect(rect.Left + left, rect.Top + top, rect.Right - right, rect.Bottom - bottom);


        SKPath PerimeterPath(SKRect rect, float radius, bool isOutline = false)
        {
            radius = Math.Max(radius, 0);

            if (IsBubble && this is IBubbleShape bubble && bubble.PointerDirection != PointerDirection.None)
                return BubblePerimeterPath(bubble, rect, radius);

            var orientation = !IsSegment ? StackOrientation.Horizontal : ((IExtendedShape)this).ExtendedElementShapeOrientation;

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
                        if (orientation == StackOrientation.Horizontal)
                        {
                            path.MoveTo(rect.Right, rect.Top);
                            path.LineTo(rect.Left + radius, rect.Top);
                            if (radius > 0)
                                path.ArcTo(topLeft, 270, -90, false);
                            path.LineTo(rect.Left, rect.Bottom - radius);
                            if (radius > 0)
                                path.ArcTo(bottomLeft, 180, -90, false);
                            path.LineTo(rect.Right, rect.Bottom);
                            if (isOutline)
                                path.MoveTo(rect.Right, rect.Top);
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
                            if (isOutline)
                                path.MoveTo(rect.Right, rect.Bottom);

                        }
                    }
                    break;
                case ExtendedElementShape.SegmentMid:
                    {
                        if (orientation == StackOrientation.Horizontal)
                        {
                            path.MoveTo(rect.Right, rect.Top);
                            path.LineTo(rect.Left, rect.Top);
                            if (isOutline)
                                path.MoveTo(rect.Left, rect.Bottom);
                            else
                                path.LineTo(rect.Left, rect.Bottom);
                            path.LineTo(rect.Right, rect.Bottom);
                            if (isOutline)
                                path.MoveTo(rect.Right, rect.Top);
                        }
                        else
                        {
                            path.MoveTo(rect.Right, rect.Bottom);
                            path.LineTo(rect.Right, rect.Top);
                            if (isOutline)
                                path.MoveTo(rect.Left, rect.Top);
                            else
                                path.LineTo(rect.Left, rect.Top);
                            path.LineTo(rect.Left, rect.Bottom);
                            if (isOutline)
                                path.MoveTo(rect.Right, rect.Bottom);
                        }
                    }
                    break;
                case ExtendedElementShape.SegmentEnd:
                    {
                        if (orientation == StackOrientation.Horizontal)
                        {
                            //path.MoveTo((rect.Left + rect.Right) / 2, rect.Top);
                            //path.LineTo(rect.Left, rect.Top);
                            path.MoveTo(rect.Left, rect.Bottom);
                            path.LineTo(rect.Right - radius, rect.Bottom);
                            if (radius > 0)
                                path.ArcTo(bottomRight, 90, -90, false);
                            path.LineTo(rect.Right, rect.Top + radius);
                            if (radius > 0)
                                path.ArcTo(topRight, 0, -90, false);
                            //path.LineTo((rect.Left + rect.Right) / 2, rect.Top);
                            path.LineTo(rect.Left, rect.Top);
                            if (isOutline)
                                path.MoveTo(rect.Left, rect.Bottom);
                        }
                        else
                        {
                            //path.MoveTo((rect.Left + rect.Right) / 2, rect.Top);
                            path.MoveTo(rect.Left, rect.Top);
                            path.LineTo(rect.Left, rect.Bottom - radius);
                            if (radius > 0)
                                path.ArcTo(bottomLeft, 180, -90, false);
                            path.LineTo(rect.Right - radius, rect.Bottom);
                            if (radius > 0)
                                path.ArcTo(bottomRight, 90, -90, false);
                            path.LineTo(rect.Right, rect.Top);
                            //path.LineTo((rect.Left + rect.Right) / 2, rect.Top);
                            if (isOutline)
                                path.MoveTo(rect.Left, rect.Top);
                        }
                    }
                    break;
                default:
                    throw new NotSupportedException("ExtendedElementShape [" + ((IExtendedShape)this).ExtendedElementShape + "] is not supported");
            }
            return path;
        }

        internal static SKPath BubblePerimeterPath(IBubbleShape bubble, SKRect rect, float radius)
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

            var tipCornerHalfWidth = tipRadius * sqrt3d2;
            var pointerToCornerIntercept = (float)Math.Sqrt((2 * radius * Math.Sin(Math.PI / 12.0)) * (2 * radius * Math.Sin(Math.PI / 12.0)) - (radius * radius / 4.0));

            var pointerAtLimitSansTipHalfWidth = (float)(pointerToCornerIntercept + radius / (2.0 * sqrt3) + (length - tipRadius / 2.0) / sqrt3);
            var pointerAtLimitHalfWidth = pointerAtLimitSansTipHalfWidth + tipRadius * sqrt3d2;

            var pointerSansFiletHalfWidth = (float)(tipCornerHalfWidth + (length - filetRadius / 2.0 - tipRadius / 2.0) / sqrt3);
            var pointerFiletWidth = filetRadius * sqrt3d2;
            var pointerAndFiletHalfWidth = pointerSansFiletHalfWidth + pointerFiletWidth;

            var dir = 1;

            if (bubble.PointerDirection.IsHorizontal())
            {
                var start = left;
                var end = right;
                if (bubble.PointerDirection == PointerDirection.Right)
                {
                    dir = -1;
                    start = right;
                    end = left;
                }
                var baseX = start + dir * length;

                var tipY = position;
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
                    var endRatio = (rect.Height - tipY) / (pointerAndFiletHalfWidth + radius);
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
                var start = top;
                var end = bottom;
                if (bubble.PointerDirection == PointerDirection.Down)
                {
                    dir = -1;
                    start = bottom;
                    end = top;
                }
                var tip = position;
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

        bool IImageController.GetLoadAsAnimation() => false;


        #endregion

    }

}
