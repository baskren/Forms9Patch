#define _Forms9Patch_BubbleLayout_

using System;
using Xamarin.Forms;


namespace Forms9Patch
{
    /// <summary>
    /// Bubble layout.
    /// </summary>
    internal class BubbleLayout : Xamarin.Forms.ContentView, ILayout
    {
        static BubbleLayout()
        {
            Settings.ConfirmInitialization();
        }

        #region IBubbleLayout Properties

        #region PoinerLength property
        /// <summary>
        /// Backing store for pointer length property.
        /// </summary>
        public static readonly BindableProperty PointerLengthProperty = BindableProperty.Create("PointerLength", typeof(float), typeof(BubbleLayout), 4.0f);//, propertyChanged: UpdateBasePadding);
        /// <summary>
        /// Gets or sets the length of the bubble layout's pointer.
        /// </summary>
        /// <value>The length of the pointer.</value>
        public float PointerLength
        {
            get => (float)GetValue(PointerLengthProperty); 
            set => SetValue(PointerLengthProperty, value); 
        }
        #endregion PoinerLength property

        #region PointerTipRadius property
        /// <summary>
        /// Backing store for pointer tip radius property.
        /// </summary>
        public static readonly BindableProperty PointerTipRadiusProperty = BindableProperty.Create("PointerTipRadius", typeof(float), typeof(BubbleLayout), 2.0f);//, propertyChanged: UpdateBasePadding);
        /// <summary>
        /// Gets or sets the radius of the bubble's pointer tip.
        /// </summary>
        /// <value>The pointer tip radius.</value>
        public float PointerTipRadius
        {
            get => (float)GetValue(PointerTipRadiusProperty); 
            set => SetValue(PointerTipRadiusProperty, value); 
        }
        #endregion PointerTipRadius property

        #region PointerAngle property
        /// <summary>
        /// backing store for PointerAngle property
        /// </summary>
        internal static readonly BindableProperty PointerAngleProperty = BindableProperty.Create("PointerAngle", typeof(float), typeof(BubbleLayout), 60f);
        /// <summary>
        /// Gets/Sets the PointerAngle property
        /// </summary>
        internal float PointerAngle
        {
            get => (float)GetValue(PointerAngleProperty); 
            set => SetValue(PointerAngleProperty, value); 
        }
        #endregion PointerAngle property

        #region PointerAxialPosition property
        /// <summary>
        /// Backing store for pointer axial position property.
        /// </summary>
        public static readonly BindableProperty PointerAxialPositionProperty = BindableProperty.Create("PointerAxialPosition", typeof(float), typeof(BubbleLayout), 0.5f);
        /// <summary>
        /// Gets or sets the position of the bubble's pointer along the face it's on.
        /// </summary>
        /// <value>The pointer axial position (left/top is zero).</value>
        public float PointerAxialPosition
        {
            get => (float)GetValue(PointerAxialPositionProperty); 
            set => SetValue(PointerAxialPositionProperty, value); 
        }
        #endregion PointerAxialPosition property

        #region PointerDirection property
        /// <summary>
        /// Backing store for pointer direction property.
        /// </summary>
        public static readonly BindableProperty PointerDirectionProperty = BindableProperty.Create("PointerDirection", typeof(PointerDirection), typeof(BubbleLayout), PointerDirection.Any);//, propertyChanged: UpdateBasePadding);
        /// <summary>
        /// Gets or sets the direction in which the pointer pointing.
        /// </summary>
        /// <value>The pointer direction.</value>
        public PointerDirection PointerDirection
        {
            get => (PointerDirection)GetValue(PointerDirectionProperty); 
            set => SetValue(PointerDirectionProperty, value); 
        }
        #endregion PointerDirection property

        #region PointerCornerRadius property
        /// <summary>
        /// The pointer corner radius property.
        /// </summary>
        public static readonly BindableProperty PointerCornerRadiusProperty = BindableProperty.Create("PointerCornerRadius", typeof(float), typeof(BubbleLayout), 0f);
        /// <summary>
        /// Gets or sets the pointer corner radius.
        /// </summary>
        /// <value>The pointer corner radius.</value>
        public float PointerCornerRadius
        {
            get => (float)GetValue(PointerCornerRadiusProperty); 
            set => SetValue(PointerCornerRadiusProperty, value); 
        }
        #endregion PointerCornerRadius property

        #region ILayout properties

        #region IgnoreChildren property
        /// <summary>
        /// backing store for IgnoreChildren property
        /// </summary>
        public static readonly BindableProperty IgnoreChildrenProperty = BindableProperty.Create("IgnoreChildren", typeof(bool), typeof(BubbleLayout), default(bool));
        /// <summary>
        /// Gets/Sets the IgnoreChildren property
        /// </summary>
        public bool IgnoreChildren
        {
            get => (bool)GetValue(IgnoreChildrenProperty); 
            set => SetValue(IgnoreChildrenProperty, value); 
        }
        #endregion IgnoreChildren properties

        #region IBackground properties

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
        #endregion

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
        internal static readonly BindableProperty ElementShapeProperty = ShapeBase.ElementShapeProperty;
        /// <summary>
        /// Gets/sets the geometry of the element
        /// </summary>
        ElementShape IShape.ElementShape
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

        #endregion IBackground properties

        #endregion ILayout properties

        #endregion IBubbleLayout properties


        #region Fields
        static int _instances = 0;
        int _id;
        #endregion Fields


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Forms9Patch.BubbleLayout"/> class.
        /// </summary>
        public BubbleLayout()
        {
            Padding = 10;
            _id = _instances++;
            //BackgroundColor = Color.White;
        }
        #endregion Constructor


        #region Description
        public string Description() { return string.Format("[{0}.{1}]", GetType(), _id); }

        public override string ToString() => Description();
        #endregion


        #region OnPropertyChanged
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <summary>
        /// Call this method from a child class to notify that a change happened on a property.
        /// </summary>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == PointerAngleProperty.PropertyName)
            {
                if (PointerAngle < 1)
                {
                    PointerAngle = 1;
                    return;
                }
                if (PointerAngle > 89)
                {
                    PointerAngle = 89;
                    return;
                }
            }

            base.OnPropertyChanged(propertyName);
            if (propertyName == PointerLengthProperty.PropertyName
                || propertyName == HasShadowProperty.PropertyName)
                InvalidateMeasure();
        }
        #endregion PropertyChange management


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


        #region Layout management
        internal Thickness DecorativePadding()
        {
            var result = ShapeBase.ShadowPadding(this, HasShadow);

            var padL = result.Left + (PointerDirection == PointerDirection.Left ? PointerLength : 0);
            var padT = result.Top + (PointerDirection == PointerDirection.Up ? PointerLength : 0);
            var padR = result.Right + (PointerDirection == PointerDirection.Right ? PointerLength : 0);
            var padB = result.Bottom + (PointerDirection == PointerDirection.Down ? PointerLength : 0);

            return result = new Thickness(padL, padT, padR, padB);
        }

        Thickness IShape.ShadowPadding() => ShapeBase.ShadowPadding(this, HasShadow);


        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var result = base.OnMeasure(widthConstraint, heightConstraint);
            var decorativePadding = DecorativePadding();
            result = new SizeRequest(new Size(result.Request.Width + decorativePadding.HorizontalThickness, result.Request.Height + decorativePadding.VerticalThickness), new Size(result.Minimum.Width + decorativePadding.HorizontalThickness, result.Minimum.Height + decorativePadding.VerticalThickness));
            return result;
        }


        /// <param name="x">A value representing the x coordinate of the child region bounding box.</param>
        /// <param name="y">A value representing the y coordinate of the child region bounding box.</param>
        /// <param name="width">A value representing the width of the child region bounding box.</param>
        /// <param name="height">A value representing the height of the child region bounding box.</param>
        /// <summary>
        /// Positions and sizes the children of a Layout.
        /// </summary>
        /// <remarks>Implementors wishing to change the default behavior of a Layout should override this method. It is suggested to
        /// still call the base method and modify its calculated results.</remarks>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            //System.Diagnostics.Debug.WriteLine ("\t\tBubbleLayout.LayoutChildren({0},{1},{2},{3})", x, y, width, height);
            //System.Diagnostics.Debug.WriteLine ("\t\tBubbleLayout.Padding=[{0}, {1}, {2}, {3}]",Padding.Left, Padding.Top, Padding.Right, Padding.Bottom);
            //System.Diagnostics.Debug.WriteLine ($"\t\tBubbleLayout.base.Padding=[{base.Padding.Left}, {base.Padding.Top}, {base.Padding.Right}, {base.Padding.Bottom}]");
            //System.Diagnostics.Debug.WriteLine ("\t\tBubbleLayout.Content.Padding=[{0}, {1}, {2}, {3}]",((StackLayout)Content).Padding.Left, ((StackLayout)Content).Padding.Top, ((StackLayout)Content).Padding.Right, ((StackLayout)Content).Padding.Bottom);
            var decorativePadding = DecorativePadding();
            x += decorativePadding.Left;
            y += decorativePadding.Top;
            width -= decorativePadding.HorizontalThickness;
            height -= decorativePadding.VerticalThickness;
            LayoutChildIntoBoundingRegion(base.Content, new Rectangle(x, y, width, height));
        }
        #endregion Layout management


    }
}

