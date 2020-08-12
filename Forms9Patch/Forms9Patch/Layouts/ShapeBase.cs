using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Rounded box base.
    /// </summary>
    [Preserve(AllMembers = true)]
    public static class ShapeBase
    {

        #region IBackground backing stores

        /// <summary>
        /// Backing store for the BackgroundImage property
        /// </summary>
        public static readonly BindableProperty BackgroundImageProperty = BindableProperty.Create("BackgroundImage", typeof(Forms9Patch.Image), typeof(ShapeBase), null);

        #region IShape BindableProperties backing stores

        #region BackgroundColor property
        /// <summary>
        /// backing store for BackgroundColor property
        /// </summary>
        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("Forms9Patch.ShapeBase.BackgroundColor", typeof(Color), typeof(ShapeBase), default(Color));
        #endregion BackgroundColor property

        #region HasShadow property
        /// <summary>
        /// backing store for HasShadow property
        /// </summary>
        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create("Forms9Patch.ShapeBase.HasShadow", typeof(bool), typeof(ShapeBase), false); //, BindingMode.OneWay, UpdateBasePadding);
        #endregion HasShadow property

        #region ShadowInverted property
        /// <summary>
        /// backing store for ShadowInverted property
        /// </summary>
        public static readonly BindableProperty ShadowInvertedProperty = BindableProperty.Create("Forms9Patch.ShapeBase.ShadowInverted", typeof(bool), typeof(ShapeBase), default(bool));
        #endregion ShadowInverted property

        #region OutlineColor property
        /// <summary>
        /// backing store for OutlineColor property
        /// </summary>
        public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create("Forms9Patch.ShapeBase.OutlineColor", typeof(Color), typeof(ShapeBase), default(Color));
        /// <summary>
        /// Backing store for the BorderColor property
        /// </summary>
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create("Forms9Patch.ShapeBase.BorderColor", typeof(Color), typeof(ShapeBase), default(Color), propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is IShape shape)
                shape.OutlineColor = (Color)newValue;
        });
        #endregion OutlineColor property

        #region OutlineRadius property
        /// <summary>
        /// backing store for OutlineRadius property
        /// </summary>
        public static readonly BindableProperty OutlineRadiusProperty = BindableProperty.Create("Forms9Patch.ShapeBase.OutlineRadius", typeof(float), typeof(ShapeBase), -1f);
        /// <summary>
        /// Backing store for the BorderRadius property
        /// </summary>
        public static readonly BindableProperty BorderRadiusProperty = BindableProperty.Create("Forms9Patch.ShapeBase.BorderRadius", typeof(float), typeof(ShapeBase), -1f, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is IShape shape)
                shape.OutlineRadius = (float)newValue;
        });
        #endregion OutlineRadius property

        #region OutlineWidth property
        /// <summary>
        /// backing store for OutlineWidth property
        /// </summary>
        public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("Forms9Patch.ShapeBase.OutlineWidth", typeof(float), typeof(ShapeBase), -1f);
        /// <summary>
        /// Backing store for the BorderWidth property
        /// </summary>
        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create("Forms9Patch.ShapeBase.BorderWidth", typeof(float), typeof(ShapeBase), -1f, propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is IShape shape)
                shape.OutlineWidth = (float)newValue;
        });
        #endregion OutlineWidth property

        #region ElementShape property
        /// <summary>
        /// backing store for ExtendedElementShape property
        /// </summary>
        public static readonly BindableProperty ElementShapeProperty = BindableProperty.Create(nameof(ElementShape), typeof(ElementShape), typeof(ShapeBase), default(ElementShape),
            propertyChanged: ((BindableObject bindable, object oldValue, object newValue) =>
            {
                if (bindable is IExtendedShape extendedShape)
                    extendedShape.ExtendedElementShape = ((ElementShape)newValue).ToExtendedElementShape();
            })
            );
        #endregion ElementShape property

        #region ExtendedElementShapeOrientationProperty
        /// <summary>
        /// Backing store for ExtendedElementShapeOrientaiton property
        /// </summary>
        public static readonly BindableProperty ExtendedElementShapeOrientationProperty = BindableProperty.Create("Forms9Patch.ShapeBase.ExtendedElementShapeOrientation", typeof(Xamarin.Forms.StackOrientation), typeof(ShapeBase), default(Xamarin.Forms.StackOrientation));
        #endregion

        #region ExtendedElementSeparatorWidth
        /// <summary>
        /// Backind store for the extended element separator width property.
        /// </summary>
        public static readonly BindableProperty ExtendedElementSeparatorWidthProperty = BindableProperty.Create("Forms9Patch.ShapeBase.ExtendedElementSeparatorWidth", typeof(float), typeof(ShapeBase), 1.0f);
        #endregion

        #region ExtendedElementShape property
        /// <summary>
        /// backing store for ExtendedElementShape property
        /// </summary>
        public static readonly BindableProperty ExtendedElementShapeProperty = BindableProperty.Create("Forms9Patch.ShapeBase.ExtendedElementShape", typeof(ExtendedElementShape), typeof(ShapeBase), default(ExtendedElementShape));
        #endregion ExtendedElementShape property

        #region ParentSegmentsOrientation property
        /// <summary>
        /// Backing store for the SegmentOrientation of the ExtendedShape's Parent
        /// </summary>
        public static readonly BindableProperty ParentSegmentsOrientationProperty = BindableProperty.Create("Forms9Patch.ShapeBase.ParentSegmentsOrientation", typeof(StackOrientation), typeof(ShapeBase), default(StackOrientation));
        #endregion ParentSegmentsOrientation property

        #endregion IShape backing stores

        #endregion IBackground backing stores

        static ShapeBase()
        {
            Settings.ConfirmInitialization();
        }

        internal static Thickness ShadowPadding(IShape shape, bool scaleForDisplay = false, bool force = false)
        {
            if (force || shape.HasShadow || (shape is StateButton && shape is ContentView contentView && contentView.HasShadow))
            {
                var shadowX = Settings.ShadowOffset.X;
                var shadowY = Settings.ShadowOffset.Y;
                var shadowR = Settings.ShadowRadius;

                var padL = Math.Max(0, shadowR - shadowX);
                var padR = Math.Max(0, shadowR + shadowX);
                var padT = Math.Max(0, shadowR - shadowY);
                var padB = Math.Max(0, shadowR + shadowY);

                if (shape is IExtendedShape extendedShape)
                {
                    var orientation = extendedShape.ExtendedElementShapeOrientation;
                    if (orientation == StackOrientation.Horizontal && (extendedShape.ExtendedElementShape == ExtendedElementShape.SegmentMid || extendedShape.ExtendedElementShape == ExtendedElementShape.SegmentEnd))
                        padL = 0;
                    if (orientation == StackOrientation.Horizontal && (extendedShape.ExtendedElementShape == ExtendedElementShape.SegmentStart || extendedShape.ExtendedElementShape == ExtendedElementShape.SegmentMid))
                        padR = 0;
                    if (orientation == StackOrientation.Vertical && (extendedShape.ExtendedElementShape == ExtendedElementShape.SegmentMid || extendedShape.ExtendedElementShape == ExtendedElementShape.SegmentEnd))
                        padT = 0;
                    if (orientation == StackOrientation.Vertical && (extendedShape.ExtendedElementShape == ExtendedElementShape.SegmentStart || extendedShape.ExtendedElementShape == ExtendedElementShape.SegmentMid))
                        padB = 0;
                }

                if (scaleForDisplay)
                {
                    padL *= Display.Scale;
                    padR *= Display.Scale;
                    padT *= Display.Scale;
                    padB *= Display.Scale;
                }

                return new Thickness(padL, padT, padR, padB);
            }
            else
                return new Thickness(0);
        }

    }
}

