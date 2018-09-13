#define _Forms9Patch_Frame_

using Xamarin.Forms;
using System;


namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Frame layout.
    /// </summary>
    public class Frame : Forms9Patch.ContentView
    {
        #region Constructor
        static Frame()
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Forms9Patch.Frame"/> class.
        /// </summary>
        public Frame()
        {
            Padding = new Thickness(20);
        }
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


        #region Layout
        //Thickness IExtendedShape.ShadowPadding() => ShapeBase.ShadowPadding(this, HasShadow);

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
        /// processes child layout
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
