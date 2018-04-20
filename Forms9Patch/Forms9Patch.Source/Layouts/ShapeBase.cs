using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Rounded box base.
    /// </summary>
    public static class ShapeBase
    {

        #region IBackground backing stores

        /// <summary>
        /// Backing store for the BackgroundImage property
        /// </summary>
        public static readonly BindableProperty BackgroundImageProperty = BindableProperty.Create("Forms9Patch.ShapeBase.BackgroundImage", typeof(Forms9Patch.Image), typeof(ShapeBase), null);

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
        #endregion OutlineColor property

        #region OutlineRadius property
        /// <summary>
        /// backing store for OutlineRadius property
        /// </summary>
        public static readonly BindableProperty OutlineRadiusProperty = BindableProperty.Create("Forms9Patch.ShapeBase.OutlineRadius", typeof(float), typeof(ShapeBase), -1f);
        #endregion OutlineRadius property

        #region OutlineWidth property
        /// <summary>
        /// backing store for OutlineWidth property
        /// </summary>
        public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("Forms9Patch.ShapeBase.OutlineWidth", typeof(float), typeof(ShapeBase), -1f);
        #endregion OutlineWidth property

        #region ElementShape property
        /// <summary>
        /// backing store for ExtendedElementShape property
        /// </summary>
        public static readonly BindableProperty ElementShapeProperty = BindableProperty.Create("ElementShape", typeof(ElementShape), typeof(AbsoluteLayout), default(ElementShape),
            propertyChanged: ((BindableObject bindable, object oldValue, object newValue) =>
            {
                ((IShape)bindable).ExtendedElementShape = ((ElementShape)newValue).ToExtendedElementShape();
            })
            );
        #endregion ElementShape property

        #region ExtendedElementShape property
        /// <summary>
        /// backing store for ExtendedElementShape property
        /// </summary>
        public static readonly BindableProperty ExtendedElementShapeProperty = BindableProperty.Create("Forms9Patch.ShapeBase.ExtendedElementShape", typeof(ExtendedElementShape), typeof(ShapeBase), default(ExtendedElementShape));
        #endregion ExtendedElementShape property

        #endregion IShape backing stores

        #endregion IBackground backing stores

        static ShapeBase()
        {
            Settings.ConfirmInitialization();
        }

        internal static Thickness ShadowPadding(IShape shape, bool hasShadow = true, bool scaleForDisplay = false)
        {
            if (hasShadow)
            {
                var shadowX = Settings.ShadowOffset.X;
                var shadowY = Settings.ShadowOffset.Y;
                var shadowR = Settings.ShadowRadius;

                var padL = Math.Max(0, shadowR - shadowX);
                var padR = Math.Max(0, shadowR + shadowX);
                var padT = Math.Max(0, shadowR - shadowY);
                var padB = Math.Max(0, shadowR + shadowY);

                if (shape is Button materialButton)
                {
                    var orientation = materialButton.ParentSegmentsOrientation;
                    if (orientation == StackOrientation.Horizontal && (shape.ExtendedElementShape == ExtendedElementShape.SegmentMid || shape.ExtendedElementShape == ExtendedElementShape.SegmentEnd))
                        padL = 0;
                    if (orientation == StackOrientation.Horizontal && (shape.ExtendedElementShape == ExtendedElementShape.SegmentStart || shape.ExtendedElementShape == ExtendedElementShape.SegmentMid))
                        padR = 0;
                    if (orientation == StackOrientation.Vertical && (shape.ExtendedElementShape == ExtendedElementShape.SegmentMid || shape.ExtendedElementShape == ExtendedElementShape.SegmentEnd))
                        padT = 0;
                    if (orientation == StackOrientation.Vertical && (shape.ExtendedElementShape == ExtendedElementShape.SegmentStart || shape.ExtendedElementShape == ExtendedElementShape.SegmentMid))
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

