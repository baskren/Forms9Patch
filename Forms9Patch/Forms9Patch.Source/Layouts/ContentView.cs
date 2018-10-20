using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch ContentView.  
    /// </summary>
    public class ContentView : Xamarin.Forms.ContentView, ILayout
    {
        #region Properties

        #region Xamarin.Forms.ContentView property override
        /// <summary>
        /// Backing store key for Content
        /// </summary>
        public static readonly new BindableProperty ContentProperty = BindableProperty.Create("Forms9Patch.ContentView.Content", typeof(View), typeof(ContentView), null,
                                                                                            propertyChanging: (bindable, oldValue, newValue) =>
                                                                                            {
                                                                                                if (oldValue is View element)
                                                                                                    ((ContentView)bindable).BaseInternalChildren.Remove(element);
                                                                                            }, propertyChanged: (bindable, oldValue, newValue) =>
                                                                                            {
                                                                                                if (newValue is View element)
                                                                                                    ((ContentView)bindable).BaseInternalChildren.Add(element);
                                                                                            });

        /// <summary>
        /// Content of Layout
        /// </summary>
        public new View Content
        {
            get => (View)GetValue(Forms9Patch.ContentView.ContentProperty);
            set => SetValue(Forms9Patch.ContentView.ContentProperty, value);
        }
        #endregion

        #region ILayout Properties

        #region IgnoreChildren
        /// <summary>
        /// Backing store for the ignore children property.
        /// </summary>
        public static readonly BindableProperty IgnoreChildrenProperty = BindableProperty.Create("IgnoreChildren", typeof(bool), typeof(ContentView), default(bool));
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.ContentView"/> ignore children.
        /// </summary>
        /// <value><c>true</c> if ignore children; otherwise, <c>false</c>.</value>
        public bool IgnoreChildren
        {
            get => (bool)GetValue(IgnoreChildrenProperty);
            set => SetValue(IgnoreChildrenProperty, value);
        }
        #endregion IgnoreChildren

        #region IBackground

        #region BackgroundImage property
        /// <summary>
        /// Backing store for the background image property.
        /// </summary>
        public static readonly BindableProperty BackgroundImageProperty = BindableProperty.Create("Forms9Patch.ContentView.BackgroundImage", typeof(Image), typeof(ContentView), null,
                                                                                              propertyChanging: (bindable, oldValue, newValue) =>
                                                                                              {
                                                                                                  if (bindable is ContentView contentView)
                                                                                                      contentView.BaseInternalChildren.Remove(contentView.CurrentBackgroundImage);
                                                                                              }, propertyChanged: (bindable, oldValue, newValue) =>
                                                                                              {
                                                                                                  if (bindable is ContentView contentView)
                                                                                                      contentView.BaseInternalChildren.Insert(0, contentView.CurrentBackgroundImage);
                                                                                              });
        /// <summary>
        /// Gets or sets the background image.
        /// </summary>
        /// <value>The background image.</value>
        public Image BackgroundImage
        {
            get => (Image)GetValue(BackgroundImageProperty);
            set => SetValue(BackgroundImageProperty, value);
        }
        #endregion BackgroundImage property

        #region LimitMinSizeToBackgroundImageSize property
        /// <summary>
        /// Backing store for the limit minimum size to background image size property.
        /// </summary>
        public static readonly BindableProperty LimitMinSizeToBackgroundImageSizeProperty = BindableProperty.Create("Forms9Patch.ContentView.LimitMinSizeToBackgroundImageSize", typeof(bool), typeof(ContentView), default(bool));
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.ContentView"/> will limit its minimum size to
        /// background image size.
        /// </summary>
        /// <value><c>true</c> if limit minimum size to background image size; otherwise, <c>false</c>.</value>
        public bool LimitMinSizeToBackgroundImageSize
        {
            get => (bool)GetValue(LimitMinSizeToBackgroundImageSizeProperty);
            set => SetValue(LimitMinSizeToBackgroundImageSizeProperty, value);
        }
        #endregion LimitMinSizeToBackgroundImageSize property

        #region IShape

        #region BackgroundColor property
        /// <summary>
        /// Backing store for the background color property.
        /// </summary>
        public static readonly new BindableProperty BackgroundColorProperty = ShapeBase.BackgroundColorProperty;
        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        #endregion BackgroundColor property

        #region HasShadow property
        /// <summary>
        /// Backing store for the has shadow property.
        /// </summary>
        public static readonly BindableProperty HasShadowProperty = ShapeBase.HasShadowProperty;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.ContentView"/> has a shadow.
        /// </summary>
        /// <value><c>true</c> if has shadow; otherwise, <c>false</c>.</value>
        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }
        #endregion HasShadow property

        #region ShadowInverted property
        /// <summary>
        /// Backing store for the shadow inverted property.
        /// </summary>
        public static readonly BindableProperty ShadowInvertedProperty = ShapeBase.ShadowInvertedProperty;
        /// <summary>
        /// Gets or sets a value indicating whether the shadow is inverted for this <see cref="T:Forms9Patch.ContentView"/>.
        /// </summary>
        /// <value><c>true</c> if shadow inverted; otherwise, <c>false</c>.</value>
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
        #endregion OutlineWidth property

        #region ElementShape property
        /// <summary>
        /// Backing store for the element shape property.
        /// </summary>
        public static readonly BindableProperty ElementShapeProperty = ShapeBase.ElementShapeProperty;
        /// <summary>
        /// Gets or sets the element shape.
        /// </summary>
        /// <value>The element shape.</value>
        public ElementShape ElementShape
        {
            get => (ElementShape)GetValue(ElementShapeProperty);
            set => SetValue(ElementShapeProperty, value);
        }
        #endregion ElementShape property

        #region IElement properties
        /// <summary>
        /// INTERNAL USE ONLY
        /// </summary>
        public int InstanceId => _f9pId;
        #endregion IElement properties

        #endregion IShape properties

        #endregion IBackground properties

        #endregion ILayout properties

        #endregion properties


        #region Private Fields and Properties
        static int _instances;
        /// <summary>
        /// INTERNAL USE ONLY
        /// </summary>
        protected readonly int _f9pId;

        /// <summary>
        /// INTERNAL USE ONLY
        /// </summary>
        internal protected readonly Image _fallbackBackgroundImage = new Image
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
        };

        /// <summary>
        /// INTERNAL USE ONLY
        /// </summary>
        internal protected Image CurrentBackgroundImage => BackgroundImage ?? _fallbackBackgroundImage;

        ObservableCollection<Element> _baseInternalChildren;
        /// <summary>
        /// INTERNAL USE ONLY
        /// </summary>
        internal protected ObservableCollection<Element> BaseInternalChildren
        {
            get
            {
                if (_baseInternalChildren == null)
                {
                    _baseInternalChildren = (ObservableCollection<Element>)P42.Utils.ReflectionExtensions.GetPropertyValue(this, "InternalChildren");
                    _baseInternalChildren?.Insert(0, CurrentBackgroundImage);
                }
                return _baseInternalChildren;
            }
        }
        #endregion


        #region Constructors
        static ContentView()
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ContentView"/> class.  Forms9Patch.ContentView is same as Forms9Patch.Frame - but with different default values.
        /// </summary>
        public ContentView()
        {
            _f9pId = _instances++;
        }
        #endregion


        #region Methods

        #region Property Change Handlers
        /// <summary>
        /// Called when a property has changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            base.OnPropertyChanged(propertyName);
            if (propertyName == BackgroundColorProperty.PropertyName)
                CurrentBackgroundImage.BackgroundColor = _fallbackBackgroundImage.BackgroundColor = BackgroundColor;
            else if (propertyName == HasShadowProperty.PropertyName)
                CurrentBackgroundImage.HasShadow = _fallbackBackgroundImage.HasShadow = HasShadow;
            else if (propertyName == ShadowInvertedProperty.PropertyName)
                CurrentBackgroundImage.ShadowInverted = _fallbackBackgroundImage.ShadowInverted = ShadowInverted;
            else if (propertyName == OutlineColorProperty.PropertyName)
                CurrentBackgroundImage.OutlineColor = _fallbackBackgroundImage.OutlineColor = OutlineColor;
            if (propertyName == OutlineRadiusProperty.PropertyName)
                CurrentBackgroundImage.OutlineRadius = _fallbackBackgroundImage.OutlineRadius = OutlineRadius;
            else if (propertyName == OutlineWidthProperty.PropertyName)
                CurrentBackgroundImage.OutlineWidth = _fallbackBackgroundImage.OutlineWidth = OutlineWidth;
            else if (propertyName == ElementShapeProperty.PropertyName)
                CurrentBackgroundImage.ElementShape = _fallbackBackgroundImage.ElementShape = ElementShape;

            if (propertyName == BackgroundColorProperty.PropertyName ||
                propertyName == HasShadowProperty.PropertyName ||
                propertyName == ShadowInvertedProperty.PropertyName ||
                propertyName == OutlineColorProperty.PropertyName ||
                propertyName == OutlineWidthProperty.PropertyName ||
                propertyName == ElementShapeProperty.PropertyName)
                InvalidateMeasure();
        }
        #endregion


        #region Description
        /// <summary>
        /// Returns a <see cref="System.String"/> that describes the current <see cref="Forms9Patch.Frame"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that describes the current <see cref="Forms9Patch.Frame"/>.</returns>
        public string Description() { return string.Format("[{0}.{1}]", GetType(), _f9pId); }

        /// <summary>
        /// Returns a <see cref="System.String"/> that describes the current <see cref="Forms9Patch.Frame"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Description();
        #endregion


        #region IgnoreChildren handlers
        /// <summary>
        /// Shoulds the invalidate on child added.
        /// </summary>
        /// <returns><c>true</c>, if invalidate on child added was shoulded, <c>false</c> otherwise.</returns>
        /// <param name="child">Child.</param>
        protected override bool ShouldInvalidateOnChildAdded(View child)
        {
            return !IgnoreChildren; // stop pestering me
        }

        /// <summary>
        /// Shoulds the invalidate on child removed.
        /// </summary>
        /// <returns><c>true</c>, if invalidate on child removed was shoulded, <c>false</c> otherwise.</returns>
        /// <param name="child">Child.</param>
        protected override bool ShouldInvalidateOnChildRemoved(View child)
        {
            return !IgnoreChildren; // go away and leave me alone
        }

        /// <summary>
        /// Ons the child measure invalidated.
        /// </summary>
        protected override void OnChildMeasureInvalidated()
        {
            // I'm ignoring you.  You'll take whatever size I want to give
            // you.  And you'll like it.
            if (!IgnoreChildren)
                base.OnChildMeasureInvalidated();
        }
        #endregion IgnoreChildren handlers


        #region Layout overrides

#pragma warning disable CS0672 // Member overrides obsolete member
        /// <summary>
        /// Called when Xamarin.Forms requests size of this element
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            if (Content == null)
                return new SizeRequest(Size.Zero, Size.Zero);

            if (double.IsInfinity(heightConstraint) || double.IsNaN(heightConstraint))
                heightConstraint = Forms9Patch.Display.Height;
            if (double.IsInfinity(widthConstraint) || double.IsNaN(widthConstraint))
                widthConstraint = Forms9Patch.Display.Width;

            //System.Diagnostics.Debug.WriteLine("ContentView.OnSizeRequest(" + widthConstraint + ", " + heightConstraint + ")");
            //var result = base.OnSizeRequest(widthConstraint, heightConstraint);
            var contentSizeRequest = Content.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
            //System.Diagnostics.Debug.WriteLine("\t\t contentSizeRequest: " + contentSizeRequest);

            if (LimitMinSizeToBackgroundImageSize && BackgroundImage != null && BackgroundImage.SourceImageSize != Size.Zero)
            {
                var reqW = Math.Max(contentSizeRequest.Request.Width, BackgroundImage.SourceImageSize.Width) + BackgroundImage.Margin.HorizontalThickness;
                var reqH = Math.Max(contentSizeRequest.Request.Height, BackgroundImage.SourceImageSize.Height) + BackgroundImage.Margin.VerticalThickness;
                var minW = Math.Max(contentSizeRequest.Minimum.Width, BackgroundImage.SourceImageSize.Width) + BackgroundImage.Margin.HorizontalThickness;
                var minH = Math.Max(contentSizeRequest.Minimum.Height, BackgroundImage.SourceImageSize.Height) + BackgroundImage.Margin.VerticalThickness;
                contentSizeRequest = new SizeRequest(new Size(reqW, reqH), new Size(minW, minH));
            }
            if (HasShadow)
            {
                var shadowPadding = ShapeBase.ShadowPadding(this);
                contentSizeRequest = new SizeRequest(new Size(contentSizeRequest.Request.Width + shadowPadding.HorizontalThickness, contentSizeRequest.Request.Height + shadowPadding.VerticalThickness), new Size(contentSizeRequest.Minimum.Width + shadowPadding.HorizontalThickness, contentSizeRequest.Minimum.Height + shadowPadding.VerticalThickness));
            }
            //System.Diagnostics.Debug.WriteLine("ContentView.OnSizeRequest: result=" + contentSizeRequest);
            return contentSizeRequest;
        }

        /// <summary>
        /// Layout out children of this element
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + " : ContentView.LayoutChildren(" + x + ", " + y + ", " + width + ", " + height + ")   WIDTH: " + Width + "   HEIGHT: " + Height);

            LayoutChildIntoBoundingRegion(CurrentBackgroundImage, new Rectangle(0, 0, Width, Height));

            var rect = new Rectangle(x, y, width, height);
            if (HasShadow)
            {
                var shadowPadding = ShapeBase.ShadowPadding(this);
                rect.X += shadowPadding.Left;
                rect.Y += shadowPadding.Top;
                rect.Width -= shadowPadding.HorizontalThickness;
                rect.Height -= shadowPadding.VerticalThickness;
            }

            //System.Diagnostics.Debug.WriteLine("\t\t contentRect: " + rect);
            LayoutChildIntoBoundingRegion(Content, rect);
        }


        #endregion

        #endregion


    }
}
