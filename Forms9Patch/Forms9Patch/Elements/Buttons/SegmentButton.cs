using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Forms9Patch
{
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    class SegmentButton : Forms9Patch.Button, IExtendedShape
    {
        #region ExtendedElementShapeOrientation property
        /// <summary>
        /// Backing store for the extended element shape orientation property.
        /// </summary>
        public static readonly BindableProperty ExtendedElementShapeOrientationProperty = ShapeBase.ExtendedElementShapeOrientationProperty;
        /// <summary>
        /// Gets or sets the orientation of the shape if it's an extended element shape
        /// </summary>
        /// <value>The forms9 patch. IS hape. extended element shape orientation.</value>
        public Xamarin.Forms.StackOrientation ExtendedElementShapeOrientation
        {
            get => (Xamarin.Forms.StackOrientation)GetValue(ExtendedElementShapeOrientationProperty);
            set => SetValue(ExtendedElementShapeOrientationProperty, value);
        }
        #endregion

        public event EventHandler<EventArgs> SegmentSelectionChanged;

        #region ExtendedElementShape property
        /// <summary>
        /// backing store for ExtendedElementShape property
        /// </summary>
        public static readonly BindableProperty ExtendedElementShapeProperty = ShapeBase.ExtendedElementShapeProperty;// = BindableProperty.Create("ExtendedElementShape", typeof(ExtendedElementShape), typeof(ShapeAndImageView), default(ExtendedElementShape));
        /// <summary>
        /// Gets/Sets the ExtendedElementShape property
        /// </summary>
        public ExtendedElementShape ExtendedElementShape
        {
            get => (ExtendedElementShape)GetValue(ExtendedElementShapeProperty);
            set => SetValue(ExtendedElementShapeProperty, value);
        }
        #endregion ExtendedElementShape property

        #region ExtendedElementSeparatorWidth
        public static readonly BindableProperty ExtendedElementSeparatorWidthProperty = ShapeBase.ExtendedElementSeparatorWidthProperty;
        public float ExtendedElementSeparatorWidth
        {
            get => (float)GetValue(ExtendedElementSeparatorWidthProperty);
            set => SetValue(ExtendedElementSeparatorWidthProperty, value);
        }
        #endregion ExtendedElementSeparatorWidth


        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                base.OnPropertyChanged(propertyName);

                if (propertyName == ExtendedElementShapeProperty.PropertyName)
                {
                    UpdateElements();
                    ((IExtendedShape)CurrentBackgroundImage).ExtendedElementShape = ((IExtendedShape)_fallbackBackgroundImage).ExtendedElementShape = ExtendedElementShape;
                }
                else if (propertyName == ExtendedElementSeparatorWidthProperty.PropertyName)
                    ((IExtendedShape)CurrentBackgroundImage).ExtendedElementSeparatorWidth = ((IExtendedShape)_fallbackBackgroundImage).ExtendedElementSeparatorWidth = ExtendedElementSeparatorWidth;
                else if (propertyName == ExtendedElementShapeOrientationProperty.PropertyName)
                    ((IExtendedShape)CurrentBackgroundImage).ExtendedElementShapeOrientation = ((IExtendedShape)_fallbackBackgroundImage).ExtendedElementShapeOrientation = ExtendedElementShapeOrientation;
                else if (propertyName == IsSelectedProperty.PropertyName)
                    SegmentSelectionChanged?.Invoke(this, new EventArgs());
            });
        }

        protected override void UpdateElements(bool isSegment = true) => base.UpdateElements(isSegment);
    }
}