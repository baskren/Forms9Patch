using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Grid layout.
    /// </summary>
    public class Grid : Xamarin.Forms.Grid, ILayout
    {
        static Grid()
        {
            Settings.ConfirmInitialization();
        }

        #region ILayout Properties

        #region IgnoreChildren
        /// <summary>
        /// The ignore children property.
        /// </summary>
        public static readonly BindableProperty IgnoreChildrenProperty = BindableProperty.Create("IgnoreChildren", typeof(bool), typeof(Grid), default(bool));
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

        #region BackgroundImage
        /// <summary>
        /// BackgroundImage backing store
        /// </summary>
        public static BindableProperty BackgroundImageProperty = ShapeBase.BackgroundImageProperty;
        /// <summary>
        /// Gets or sets the background image.
        /// </summary>
        /// <value>The background image.</value>
        public Image BackgroundImage
        {
            get => (Image)GetValue(BackgroundImageProperty); 
            set => SetValue(BackgroundImageProperty, value); 
        }
        #endregion BackgroundImage

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
#if _Forms9Patch_Frame_
        /// <summary>
        /// override Xamarin.Forms.Frame.HasShadow property backing store in order to correctly compute & store shadow padding
        /// </summary>
        public static new BindableProperty HasShadowProperty = ShapeBase.HasShadowProperty;
        /// <summary>
        /// Gets/Sets the HasShadow property
        /// </summary>
        public new bool HasShadow
#else
        /// <summary>
        /// HasShadow property backing store
        /// </summary>
        public static BindableProperty HasShadowProperty = ShapeBase.HasShadowProperty;
        /// <summary>
        /// Gets/Sets the HasShadow property
        /// </summary>
        public bool HasShadow
#endif
        {
            get => (bool)GetValue(HasShadowProperty); 
            set => SetValue(HasShadowProperty, value); 
        }
        #endregion HasShadow property

        #region ShadowInverted
        /// <summary>
        /// Backing store for the ShadowInverted bindable property.
        /// </summary>
        /// <remarks></remarks>
        public static readonly BindableProperty ShadowInvertedProperty = ShapeBase.ShadowInvertedProperty;
        /// <summary>
        /// Gets or sets a flag indicating if the layout's shadow is inverted. This is a bindable property.
        /// </summary>
        /// <value><c>true</c> if this instance's shadow is inverted; otherwise, <c>false</c>.</value>
        public bool ShadowInverted
        {
            get => (bool)GetValue(ShadowInvertedProperty); 
            set => SetValue(ShadowInvertedProperty, value); 
        }
        #endregion ShadowInverted

        #region OutlineColor property

#if _Forms9Patch_Frame_
        /// <summary>
        /// backing store for OutlineColor property
        /// </summary>
        public static new readonly BindableProperty OutlineColorProperty = ShapeBase.OutlineColorProperty;
        /// <summary>
        /// Gets/Sets the OutlineColor property
        /// </summary>
        public new Color OutlineColor
        {
            get { return (Color)GetValue(OutlineColorProperty); }
            set { SetValue(OutlineColorProperty, value); }
        }
#else
        /// <summary>
        /// backing store for OutlineColor property
        /// </summary>
        public static readonly BindableProperty OutlineColorProperty = ShapeBase.OutlineColorProperty;
        /// <summary>
        /// Gets/Sets the OutlineColor property
        /// </summary>
        public Color OutlineColor
        {
            get => (Color)GetValue(OutlineColorProperty); 
            set => SetValue(OutlineColorProperty, value); 
        }
#endif

        #endregion OutlineColor property

        #region OutlineRadius
        /// <summary>
        /// Backing store for the OutlineRadius bindable property.
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
        #endregion OutlineRadius

        #region OutlineWidth
        /// <summary>
        /// Backing store for the OutlineWidth bindable property.
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
        #endregion OutlineWidth

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

        #region ExtendedElementShape property
        /// <summary>
        /// backing store for ExtendedElementShape property
        /// </summary>
        internal static readonly BindableProperty ExtendedElementShapeProperty = ShapeBase.ExtendedElementShapeProperty;
        /// <summary>
        /// Gets/Sets the ExtendedElementShape property
        /// </summary>
        ExtendedElementShape IShape.ExtendedElementShape
        {
            get => (ExtendedElementShape)GetValue(ExtendedElementShapeProperty); 
            set => SetValue(ExtendedElementShapeProperty, value); 
        }
        #endregion ExtendedElementShape property

        #region IElement

        #region InstanceId
        /// <summary>
        /// The Instance Id (for debugging purposes)
        /// </summary>
        public int InstanceId => _id;
        #endregion InstanceId

        #endregion IElement

        #endregion IShape

        #endregion IBackground

        #endregion ILayout


        #region Fields
        static int _instances = 0;
        int _id;
        #endregion


        #region Constructor
        /// <summary>
        /// Initializes an instance of the <see cref="Forms9Patch.Grid"/> class.
        /// </summary>
        public Grid()
        {
            _id = _instances++;
        }
        #endregion


        #region Description
        /// <summary>
        /// Returns a <see cref="System.String"/> that describes the current <see cref="Forms9Patch.Grid"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that describes the current <see cref="Forms9Patch.Grid"/>.</returns>
        public string Description() { return string.Format("[{0}.{1}]", GetType(), _id); }

        /// <summary>
        /// Returns a <see cref="System.String"/> that describes the current <see cref="Forms9Patch.Grid"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Description();
        #endregion


        #region OnPropertyChanged
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <summary>
        /// Call this method from a child class to notify that a change happened on a property.
        /// </summary>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == HasShadowProperty.PropertyName)
                InvalidateMeasure();
            base.OnPropertyChanged(propertyName);
        }
        #endregion


        #region IgnoreChildren handlers
        bool _child1Visibility;

        /// <summary>
        /// Shoulds the invalidate on child added.
        /// </summary>
        /// <returns><c>true</c>, if invalidate on child added was shoulded, <c>false</c> otherwise.</returns>
        /// <param name="child">Child.</param>
        protected override bool ShouldInvalidateOnChildAdded(View child)
        {
            var count = Children.Count;
            if (Children.Count == 1 && Children.Contains(child))
            {
                _child1Visibility = child.IsVisible;
                child.IsVisible = false;
                Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
                {
                    child.IsVisible = _child1Visibility;
                    return false;
                });
            }
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


        #region Layout
        Thickness IShape.ShadowPadding() => ShapeBase.ShadowPadding(this, HasShadow);

        /// <summary>
        /// Processes the layout measurement
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var result = base.OnMeasure(widthConstraint, heightConstraint);
            if (HasShadow)
            {
                var shadowPadding = ShapeBase.ShadowPadding(this);
                result = new SizeRequest(new Size(result.Request.Width + shadowPadding.HorizontalThickness, result.Request.Height + shadowPadding.VerticalThickness), new Size(result.Minimum.Width + shadowPadding.HorizontalThickness, result.Minimum.Height + shadowPadding.VerticalThickness));
            }
            return result;
        }

        /// <summary>
        /// Processes the layout of the layout's children
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (HasShadow)
            {
                var shadowPadding = ShapeBase.ShadowPadding(this);
                x += shadowPadding.Left;
                y += shadowPadding.Top;
                width -= shadowPadding.HorizontalThickness;
                height -= shadowPadding.VerticalThickness;
            }
            base.LayoutChildren(x, y, width, height);
        }
        #endregion

    }
}

