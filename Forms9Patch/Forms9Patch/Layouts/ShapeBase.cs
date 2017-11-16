using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Rounded box base.
	/// </summary>
	public static class ShapeBase 
	{
        #region Overridden BinableProperties backing stores
        /// <summary>
        /// Identifies the Padding bindable property.
        /// </summary>
        /// <remarks></remarks>
        //public static readonly BindableProperty PaddingProperty = BindableProperty.Create("Forms9Patch.ShapeBase.Padding", typeof(Thickness), typeof(ShapeBase), new Thickness(0), BindingMode.OneWay, UpdateBasePadding);

        //internal static readonly BindableProperty IgnoreShapePropertiesChangesProperty = BindableProperty.Create("Forms9Patch.Shapebase.IgnoreShapePropertiesChanges", typeof(bool), typeof(ShapeBase), false);
        #endregion

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

        /*
        internal static bool UpdateBasePadding(BindableObject bindable, object newValue) {
			var layout = bindable as ILayout;
			Thickness layoutPadding;
            var hasShadow = (bool)bindable.GetValue(ShapeBase.HasShadowProperty);
            if (newValue is bool)
            {
                layoutPadding = (Thickness)bindable.GetValue(ShapeBase.PaddingProperty);
                hasShadow = (bool)newValue;
            }
            else
            {
                layoutPadding = (Thickness)newValue;
            }
            //var materialButton = layout as MaterialButton;
            //var makeRoomForShadow = materialButton == null ? hasShadow : (bool)layout.GetValue (MaterialButton.HasShadowProperty);

            //var updating = (bool)bindable.GetValue(IgnoreShapePropertiesChangesProperty);
            //bindable.SetValue(IgnoreShapePropertiesChangesProperty, true);
			if (hasShadow) {
				
				var shadowPadding = ShadowPadding (layout, hasShadow);

				Thickness newPadding;
				double xLeft = layoutPadding.Left + shadowPadding.Left;
				double xTop = layoutPadding.Top + shadowPadding.Top;
				double xRight = layoutPadding.Right + shadowPadding.Right;
				double xBottom = layoutPadding.Bottom + shadowPadding.Bottom;
				newPadding = new Thickness (xLeft, xTop, xRight, xBottom);

                bindable.SetValue (Layout.PaddingProperty, newPadding);
				//System.Diagnostics.Debug.WriteLine ("\tRoundedBoxBase.UpdateBasePadding newPadding: " + newPadding.Description ());
			} else {
                bindable.SetValue (Layout.PaddingProperty, layoutPadding);
				//System.Diagnostics.Debug.WriteLine ("\tRoundedBoxBase.UpdateBasePadding layoutPadding: " + layoutPadding.Description ());
			}
            //var contentView = bindable as ContentView;
            //bindable.SetValue(IgnoreShapePropertiesChangesProperty, updating);
            return true;
		}
        */


		internal static Thickness ShadowPadding(IShape shape, bool hasShadow=true) {
			if (hasShadow) {
                var shadowX = Settings.ShadowOffset.X;
                var shadowY = Settings.ShadowOffset.Y;
                var shadowR = Settings.ShadowRadius;

                var padL = Math.Max(0, shadowR - shadowX);
                var padR = Math.Max(0, shadowR + shadowX);
                var padT = Math.Max(0, shadowR - shadowY);
                var padB = Math.Max(0, shadowR + shadowY);

                if (shape is MaterialButton materialButton)
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
                return new Thickness(padL, padT, padR, padB);
            }
            else
				return new Thickness (0);
		}

	}
}

