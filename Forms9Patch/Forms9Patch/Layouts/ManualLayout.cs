using System;
using Xamarin.Forms;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Manual layout.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class ManualLayout : Xamarin.Forms.Layout<View>, ILayout
    {
        static ManualLayout()
        {
            Settings.ConfirmInitialization();
        }

        #region ILayout Properties

        #region IgnoreChildren
        /// <summary>
        /// The ignore children property.
        /// </summary>
        public static readonly BindableProperty IgnoreChildrenProperty = BindableProperty.Create("Forms9Patch.ManualLayout.IgnoreChildren", typeof(bool), typeof(ManualLayout), default(bool));
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
        readonly int _id;
        #endregion


        #region Constructor
        /// <summary>
        /// Constructor for ManualLayout
        /// </summary>
        public ManualLayout()
        {
            _id = _instances++;
        }
        #endregion Constructor


        #region Description
        /// <summary>
        /// Returns a <see cref="System.String"/> that describes the current <see cref="Forms9Patch.ManualLayout"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that describes the current <see cref="Forms9Patch.ManualLayout"/>.</returns>
        public virtual string Description() { return string.Format("[{0}.{1}]", GetType(), _id); }

        /// <summary>
        /// Returns a <see cref="System.String"/> that describes the current <see cref="Forms9Patch.ManualLayout"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Description();
        #endregion


        #region Layout
        //Thickness IShape.ShadowPadding() => ShapeBase.ShadowPadding(this, HasShadow);

        /// <summary>
        /// Occurs when layout children event is triggered.
        /// </summary>
        public event EventHandler<ManualLayoutEventArgs> LayoutChildrenEvent;

        /// <summary>
        /// processes measurement of layout
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
        /// processes child layouts
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
            //base.LayoutChildren(x, y, width, height);
            LayoutChildrenEvent?.Invoke(this, new ManualLayoutEventArgs(x, y, width, height));
        }

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
                try
                {
                    base.OnPropertyChanged(propertyName);
                }
                catch (Exception) { }

                if (propertyName == HasShadowProperty.PropertyName)
                    InvalidateMeasure();
            });
        }
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
    }


    #region ManualLayoutEventArgs
    /// <summary>
    /// Manual layout event arguments.
    /// </summary>
    public class ManualLayoutEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the x.
        /// </summary>
        /// <value>The x.</value>
        public double X { get; }

        /// <summary>
        /// Gets the y.
        /// </summary>
        /// <value>The y.</value>
        public double Y { get; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public double Width { get; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public double Height { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ManualLayoutEventArgs"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public ManualLayoutEventArgs(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Formats content for display
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[" + X + "," + Y + "," + Width + "," + Height + "]";
        }
    }
    #endregion
}
