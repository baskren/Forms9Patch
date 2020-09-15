#define _Forms9Patch_BubbleLayout_

using System.ComponentModel;
using Xamarin.Forms;


namespace Forms9Patch
{
    /// <summary>
    /// Bubble layout.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    class BubbleLayout : Forms9Patch.ContentView, IBubbleLayout
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
        public static readonly BindableProperty PointerLengthProperty = BindableProperty.Create("Forms9Patch.BubbleLayout.PointerLength", typeof(float), typeof(BubbleLayout), 10.0f,
        propertyChanged: (b, o, n) =>
         {
             if (b is BubbleLayout layout)
                 ((IBubbleShape)layout.CurrentBackgroundImage).PointerLength = ((IBubbleShape)layout._fallbackBackgroundImage).PointerLength = layout.PointerLength;
         });//, propertyChanged: UpdateBasePadding);
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
        public static readonly BindableProperty PointerTipRadiusProperty = BindableProperty.Create("Forms9Patch.BubbleLayout.PointerTipRadius", typeof(float), typeof(BubbleLayout), 1.0f,
        propertyChanged: (b, o, n) =>
        {
            if (b is BubbleLayout layout)
                ((IBubbleShape)layout.CurrentBackgroundImage).PointerTipRadius = ((IBubbleShape)layout._fallbackBackgroundImage).PointerTipRadius = layout.PointerTipRadius;
        });//, propertyChanged: UpdateBasePadding);
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

        #region PointerAxialPosition property
        /// <summary>
        /// Backing store for pointer axial position property.
        /// </summary>
        public static readonly BindableProperty PointerAxialPositionProperty = BindableProperty.Create("Forms9Patch.BubbleLayout.PointerAxialPosition", typeof(float), typeof(BubbleLayout), 0.5f,
        propertyChanged: (b, o, n) =>
        {
            if (b is BubbleLayout layout)
                ((IBubbleShape)layout.CurrentBackgroundImage).PointerAxialPosition = ((IBubbleShape)layout._fallbackBackgroundImage).PointerAxialPosition = layout.PointerAxialPosition;
        });
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
        public static readonly BindableProperty PointerDirectionProperty = BindableProperty.Create("Forms9Patch.BubbleLayout.PointerDirection", typeof(PointerDirection), typeof(BubbleLayout), PointerDirection.Any,
        propertyChanged: (b, o, n) =>
        {
            if (b is BubbleLayout layout)
                ((IBubbleShape)layout.CurrentBackgroundImage).PointerDirection = ((IBubbleShape)layout._fallbackBackgroundImage).PointerDirection = layout.PointerDirection;
        });
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
        public static readonly BindableProperty PointerCornerRadiusProperty = BindableProperty.Create("Forms9Patch.BubbleLayout.PointerCornerRadius", typeof(float), typeof(BubbleLayout), 0f,
        propertyChanged: (b, o, n) =>
        {
            if (b is BubbleLayout layout)
                ((IBubbleShape)layout.CurrentBackgroundImage).PointerCornerRadius = ((IBubbleShape)layout._fallbackBackgroundImage).PointerCornerRadius = layout.PointerCornerRadius;
        });

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

        #endregion IBubbleLayout properties


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Forms9Patch.BubbleLayout"/> class.
        /// </summary>
        public BubbleLayout()
        {
            _fallbackBackgroundImage.IsBubble = true;
        }
        #endregion Constructor


        #region Description
        public override string ToString() => Description();
        #endregion


        #region OnPropertyChanged
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <summary>
        /// Call this method from a child class to notify that a change happened on a property.
        /// </summary>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                base.OnPropertyChanged(propertyName);

                if (propertyName == BackgroundImageProperty.PropertyName && BackgroundImage != null)
                    BackgroundImage.IsBubble = true;

                if (propertyName == PointerLengthProperty.PropertyName
                    || propertyName == HasShadowProperty.PropertyName
                    || propertyName == IsVisibleProperty.PropertyName)
                    InvalidateMeasure();
            });
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
            var padL = (PointerDirection == PointerDirection.Left ? PointerLength : 0);
            var padT = (PointerDirection == PointerDirection.Up ? PointerLength : 0);
            var padR = (PointerDirection == PointerDirection.Right ? PointerLength : 0);
            var padB = (PointerDirection == PointerDirection.Down ? PointerLength : 0);

            return new Thickness(padL, padT, padR, padB);
        }


        //Thickness IShape.ShadowPadding() => ShapeBase.ShadowPadding(this, HasShadow);


#pragma warning disable CS0672 // Member overrides obsolete member
        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            if (double.IsInfinity(heightConstraint) || double.IsNaN(heightConstraint))
                heightConstraint = Forms9Patch.Display.Height;
            if (double.IsInfinity(widthConstraint) || double.IsNaN(widthConstraint))
                widthConstraint = Forms9Patch.Display.Width;

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
            var decorativePadding = DecorativePadding();
            x += decorativePadding.Left;
            y += decorativePadding.Top;
            width -= decorativePadding.HorizontalThickness;
            height -= decorativePadding.VerticalThickness;
            base.LayoutChildren(x, y, width, height);
        }

        internal void InternalLayout(Rectangle rect)
        {
            Layout(rect);
            var decorativePadding = DecorativePadding();
            var shadowPadding = HasShadow ? ShapeBase.ShadowPadding(this) : new Thickness();
            rect.X = decorativePadding.Left + Padding.Left + shadowPadding.Left;
            rect.Y = decorativePadding.Top + Padding.Top + shadowPadding.Top;
            rect.Width -= decorativePadding.HorizontalThickness + Padding.HorizontalThickness + shadowPadding.HorizontalThickness;
            rect.Height -= decorativePadding.VerticalThickness + Padding.VerticalThickness + shadowPadding.VerticalThickness;
            Content.Layout(rect);
        }

        #endregion Layout management


    }
}

