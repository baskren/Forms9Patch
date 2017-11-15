#define Forms9Patch_Image

using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Image element.
    /// </summary>
    public class Image : Xamarin.Forms.Image, IImage
    {
        #region IImage Properties

        #region Fill
        /// <summary>
        /// UNSUPPORTED INHERITED PROPERTY. See <see cref="Forms9Patch.Fill"/>.
        /// </summary>
        public new static readonly BindableProperty AspectProperty = BindableProperty.Create("Aspect", typeof(Aspect), typeof(Image), Aspect.Fill, BindingMode.OneWay, null, null, null, null, null);
        /// <summary>
        /// UNSUPPORTED INHERITED PROPERTY. See <see cref="Forms9Patch.Fill"/>.
        /// </summary>
        /// <value>The scaling method</value>
        public new Aspect Aspect
        {
            get { throw new NotSupportedException("[Forms9Patch.Image]Aspect property is not supported"); }
            set { throw new NotSupportedException("[Forms9Patch.Image]Aspect property is not supported"); }
        }

        /// <summary>
        /// Backing store for the Fill bindable property.
        /// </summary>
        public static readonly BindableProperty FillProperty = BindableProperty.Create("Fill", typeof(Fill), typeof(Image), Fill.None);
        /// <summary>
        /// Fill behavior for nonscalable (not NinePatch or CapInsets not set) image. 
        /// </summary>
        /// <value>The fill method (AspectFill, AspectFit, Fill, Tile)</value>
        public Fill Fill
        {
            get { return (Fill)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
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
            get
            {
                return (Thickness)GetValue(CapInsetsProperty);
            }
            set
            {
                SetValue(CapInsetsProperty, value);
            }
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
            get { return (Thickness)GetValue(ContentPaddingProperty); }
            internal set { SetValue (ContentPaddingProperty, value); }
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
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }
        #endregion TintColor

        #region SourceImageSize
        internal static readonly BindableProperty BaseImageSizeProperty = BindableProperty.Create("BaseImageSize", typeof(Size), typeof(Image), default(Size));
        /// <summary>
        /// The size of the source image
        /// </summary>
        public Size SourceImageSize
        {
            get { return (Size)GetValue(BaseImageSizeProperty); }
            internal set { SetValue(BaseImageSizeProperty, value); }
        }
        #endregion SourceImageSize

        #region IShape

        // BackgroundColor inherited from VisualElement

        #region HasShadow property
        /// <summary>
        /// Backing store for HasShadow property
        /// </summary>
        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create("HasShadow", typeof(bool), typeof(Image), default(bool));
        /// <summary>
        /// Get/sets if the shape (if any) behind the image has a shadow
        /// </summary>
        public bool HasShadow
        {
            get { return (bool)GetValue(HasShadowProperty); }
            set { SetValue(HasShadowProperty, value); }
        }
        #endregion HasShadow property

        #region ShadowInverted property
        /// <summary>
        /// backing store for ShadowInverted property
        /// </summary>
        public static readonly BindableProperty ShadowInvertedProperty = BindableProperty.Create("ShadowInverted", typeof(bool), typeof(Image), default(bool));
        /// <summary>
        /// Gets/Sets the ShadowInverted property
        /// </summary>
        public bool ShadowInverted
        {
            get { return (bool)GetValue(ShadowInvertedProperty); }
            set { SetValue(ShadowInvertedProperty, value); }
        }
        #endregion ShadowInverted property

        #region OutlineColor property
        /// <summary>
        /// backing store for OutlineColor property
        /// </summary>
        public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create("OutlineColor", typeof(Color), typeof(Image), default(Color));
        /// <summary>
        /// Gets/Sets the OutlineColor property
        /// </summary>
        public Color OutlineColor
        {
            get { return (Color)GetValue(OutlineColorProperty); }
            set { SetValue(OutlineColorProperty, value); }
        }
        #endregion OutlineColor property

        #region OutlineRadius property
        /// <summary>
        /// backing store for OutlineRadius property
        /// </summary>
        public static readonly BindableProperty OutlineRadiusProperty = BindableProperty.Create("OutlineRadius", typeof(float), typeof(Image), default(float));
        /// <summary>
        /// Gets/Sets the OutlineRadius property
        /// </summary>
        public float OutlineRadius
        {
            get { return (float)GetValue(OutlineRadiusProperty); }
            set { SetValue(OutlineRadiusProperty, value); }
        }
        #endregion OutlineRadius property

        #region OutlineWidth property
        /// <summary>
        /// backing store for OutlineWidth property
        /// </summary>
        public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("OutlineWidth", typeof(float), typeof(Image), -1f);
        /// <summary>
        /// Gets/Sets the OutlineWidth property
        /// </summary>
        public float OutlineWidth
        {
            get { return (float)GetValue(OutlineWidthProperty); }
            set { SetValue(OutlineWidthProperty, value); }
        }
        #endregion OutlineWidth property

        #region ElementShape property
        /// <summary>
        /// backing store for ElementShape property
        /// </summary>
        public static readonly BindableProperty ElementShapeProperty = BindableProperty.Create("ElementShape", typeof(ElementShape), typeof(Image), default(ElementShape));
        /// <summary>
        /// Gets/Sets the ElementShape property
        /// </summary>
        ElementShape IShape.ElementShape
        {
            get { return (ElementShape)GetValue(ElementShapeProperty); }
            set { SetValue(ElementShapeProperty, value); }
        }
        #endregion ElementShape property

        #region IgnoreShapePropertiesChanges
        /// <summary>
        /// Backging store for the IgnoreShapePropertiesChanges property
        /// </summary>
        //public static BindableProperty IgnoreShapePropertiesChangesProperty = ShapeBase.IgnoreShapePropertiesChangesProperty;
        /// <summary>
        /// Prevent shape updates (to optimize performace)
        /// </summary>
        //public bool IgnoreShapePropertiesChanges
       // {
       //     get { return (bool)GetValue(ShapeBase.IgnoreShapePropertiesChangesProperty); }
       //     set { SetValue(ShapeBase.IgnoreShapePropertiesChangesProperty, value); }
       // }
        #endregion IgnoreShapePropertyChanges

        #region IElement

        public int InstanceId => _f9pId;

        #endregion IElement

        #endregion IShape

        #endregion IImage


        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image()
        {
            //base.Fill = Xamarin.Forms.Aspect.Fill;
            _instances = 0;
        }

        int _instances;
        int _f9pId;


        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="image">Image.</param>
        public Image(Xamarin.Forms.Image image)
        {
            _f9pId = _instances++;
            Fill = image.Aspect.ToF9pFill();
            IsOpaque = image.IsOpaque;
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
        #endregion


        #region Change management

        /// <summary>
        /// Addresses a size request
        /// </summary>
        /// <returns>The size request.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        [Obsolete("Use OnMeasure")]
        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            /*
            var result =  base.OnSizeRequest(widthConstraint,heightConstraint);
            return result;
            */

            
            SizeRequest sizeRequest = base.OnSizeRequest(double.PositiveInfinity, double.PositiveInfinity);
            double requestAspectRatio = sizeRequest.Request.Width / sizeRequest.Request.Height;
            double constraintAspectRatio = widthConstraint / heightConstraint;
            double width = sizeRequest.Request.Width;
            double height = sizeRequest.Request.Height;
            if (Math.Abs(width) < float.Epsilon * 5 || Math.Abs(height) < float.Epsilon * 5)
                //return new SizeRequest(new Size(40.0, 40.0));
                return new SizeRequest(SourceImageSize);
            if (constraintAspectRatio > requestAspectRatio)
            {
                switch (Fill)
                {
                    case Fill.AspectFit:
                    case Fill.AspectFill:
                        height = Math.Min(height, heightConstraint);
                        width = width * (height / height);
                        break;
                    case Fill.Fill:
                    case Fill.Tile:
                        width = Math.Min(width, widthConstraint);
                        height = height * (width / width);
                        break;
                }
            }
            else if (constraintAspectRatio < requestAspectRatio)
            {
                switch (Fill)
                {
                    case Fill.AspectFit:
                    case Fill.AspectFill:
                        width = Math.Min(width, widthConstraint);
                        height = height * (width / width);
                        break;
                    case Fill.Fill:
                    case Fill.Tile:
                        height = Math.Min(height, heightConstraint);
                        width = width * (height / height);
                        break;
                }
            }
            else
            {
                width = Math.Min(width, widthConstraint);
                height = height * (width / width);
            }
            return new SizeRequest(new Size(width, height));
            

            /*
            SizeRequest sizeRequest = base.OnSizeRequest(double.PositiveInfinity, double.PositiveInfinity);
            double width = sizeRequest.Request.Width;
            double height = sizeRequest.Request.Height;
            if (Math.Abs(width) < float.Epsilon * 5 || Math.Abs(height) < float.Epsilon * 5)
                //return new SizeRequest(new Size(40.0, 40.0));
                return new SizeRequest(BaseImageSize);
            Size result = new Size { Width = width, Height = height };
            if (Fill != Fill.None)
            {
                if (!Double.IsInfinity(widthConstraint))
                    result.Width = Math.Max(width, widthConstraint);
                if (!Double.IsInfinity(heightConstraint))
                    result.Height = Math.Max(height, heightConstraint);
            }
            return new SizeRequest(result);
            */

        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == HasShadowProperty.PropertyName)
                InvalidateMeasure();
            base.OnPropertyChanged(propertyName);
        }
        #endregion


        #region Layout
        Thickness IShape.ShadowPadding() => ShapeBase.ShadowPadding(this, HasShadow);

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var result = base.OnMeasure(widthConstraint, heightConstraint);
            if (HasShadow)
            {
                var shadowPadding = ((IShape)this).ShadowPadding();
                result = new SizeRequest(new Size(result.Request.Width + shadowPadding.HorizontalThickness, result.Request.Height + shadowPadding.VerticalThickness),
                    new Size(result.Minimum.Width + shadowPadding.HorizontalThickness, result.Minimum.Height + shadowPadding.VerticalThickness));
            }
            return result;
        }

#if Forms9Patch_Image
#else
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            var shadowPadding = ((IShape)this).ShadowPadding();
            x += shadowPadding.Left;
            y += shadowPadding.Top;
            width -= shadowPadding.HorizontalThickness;
            height -= shadowPadding.VerticalThickness;
            base.LayoutChildren(x, y, width, height);
        }
#endif
        #endregion

    }
}

