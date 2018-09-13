#define Forms9Patch_Image

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Image element.
    /// </summary>
    public class Image : View, IImage, IImageController //, IElementConfiguration<Image> //Xamarin.Forms.Image, IImage
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
        internal static readonly BindableProperty BaseImageSizeProperty = BindableProperty.Create("BaseImageSize", typeof(Size), typeof(Image), default(Size));
        /// <summary>
        /// The size of the source image
        /// </summary>
        public Size SourceImageSize
        {
            get => (Size)GetValue(BaseImageSizeProperty);
            internal set => SetValue(BaseImageSizeProperty, value);
        }
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
        public static readonly BindableProperty HasShadowProperty = ShapeBase.HasShadowProperty; // = BindableProperty.Create("HasShadow", typeof(bool), typeof(Image), default(bool));
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
        public static readonly BindableProperty ShadowInvertedProperty = ShapeBase.ShadowInvertedProperty; // = BindableProperty.Create("ShadowInverted", typeof(bool), typeof(Image), default(bool));
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
        public static readonly BindableProperty OutlineColorProperty = ShapeBase.OutlineColorProperty; // = BindableProperty.Create("OutlineColor", typeof(Color), typeof(Image), default(Color));
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
        public static readonly BindableProperty OutlineRadiusProperty = ShapeBase.OutlineRadiusProperty; // = BindableProperty.Create("OutlineRadius", typeof(float), typeof(Image), default(float));
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
        public static readonly BindableProperty OutlineWidthProperty = ShapeBase.OutlineWidthProperty;// = BindableProperty.Create("OutlineWidth", typeof(float), typeof(Image), -1f);
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

        /*
        #region ExtendedElementShape property
        /// <summary>
        /// backing store for ExtendedElementShape property
        /// </summary>
        public static readonly BindableProperty ExtendedElementShapeProperty = ShapeBase.ExtendedElementShapeProperty;// = BindableProperty.Create("ExtendedElementShape", typeof(ExtendedElementShape), typeof(Image), default(ExtendedElementShape));
        /// <summary>
        /// Gets/Sets the ExtendedElementShape property
        /// </summary>
        ExtendedElementShape IShape.ExtendedElementShape
        {
            get => (ExtendedElementShape)GetValue(ExtendedElementShapeProperty); 
            set => SetValue(ExtendedElementShapeProperty, value); 
        }
        #endregion ExtendedElementShape property
        */

        #region IElement

        /// <summary>
        /// Returns index instance ID for this class (starts at 0)
        /// </summary>
        public int InstanceId => _f9pId;

        #endregion IElement

        #endregion IShape

        #endregion IImage

        #endregion Properties

        internal bool FillOrLayoutSet;


        #region Fields
        int _instances;
        int _f9pId;

        #endregion


        #region Constructors
        static Image()
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Instantiates a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image()
        {
            _instances = 0;
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


        #region Change management
        /// <summary>
        /// PropertyChanged event handler
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == HasShadowProperty.PropertyName
                || propertyName == SourceProperty.PropertyName
                || propertyName == OutlineWidthProperty.PropertyName
                || propertyName == OutlineColorProperty.PropertyName)
                InvalidateMeasure();
            else if (propertyName == FillProperty.PropertyName
                || propertyName == HorizontalOptionsProperty.PropertyName
                || propertyName == VerticalOptionsProperty.PropertyName
                )
                FillOrLayoutSet = true;
            base.OnPropertyChanged(propertyName);
        }
        #endregion


        #region Layout
        Thickness IShape.ShadowPadding() => ShapeBase.ShadowPadding(this, HasShadow);
        #endregion

    }
}

