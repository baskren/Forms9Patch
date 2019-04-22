using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Modal popup.
    /// </summary>
    public class ModalPopup : PopupBase
    {
        #region Properties

        #region Content
        /// <summary>
        /// Gets or sets the content of the FormsPopup.Modal.
        /// </summary>
        /// <value>The content.</value>
        public new View Content
        {
            get => _frame.Content;
            set => _frame.Content = value;
        }
        #endregion

        #region IBackgroundImage

        // BackgroundImage inherited from PopupBase

        /// <summary>
        /// The location property backing store.
        /// </summary>
        public static readonly BindableProperty LocationProperty = BindableProperty.Create("Location", typeof(Point), typeof(ModalPopup), new Point(double.NegativeInfinity, double.NegativeInfinity));
        /// <summary>
        /// Gets or sets the Modal Popup's location.
        /// </summary>
        /// <value>The location (default centers it in Host Page).</value>
        public Point Location
        {
            get => (Point)GetValue(LocationProperty);
            set => SetValue(LocationProperty, value);
        }

        /*
		public double TranslationX {
			get { return (double)GetValue (TranslationXProperty); }
			set { SetValue (TranslationXProperty, value); }
		}
		public double TranslationY {
			get { return (double)GetValue (TranslationYProperty); }
			set { SetValue (TranslationYProperty, value); }
		}
		*/
        #endregion

        #endregion


        #region Fields
        Frame _frame;
        #endregion


        #region Constructors
        void Init()
        {
            _frame = new Forms9Patch.Frame
            {
                Padding = Padding,
                HasShadow = HasShadow,
                OutlineColor = OutlineColor,
                OutlineWidth = OutlineWidth,
                OutlineRadius = OutlineRadius,
                BackgroundColor = BackgroundColor
            };
            DecorativeContainerView = _frame;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ModalPopup"/> class.
        /// </summary>
        /// <param name="retain">If set to <c>true</c> retain.</param>
        /// <param name="popAfter">Pop after TimeSpan.</param>
        public ModalPopup(bool retain = false, TimeSpan popAfter = default(TimeSpan)) : base(retain: retain, popAfter: popAfter) => Init();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ModalPopup"/> class.
        /// </summary>
        /// <param name="popAfter">Pop after TimeSpan.</param>
        public ModalPopup(TimeSpan popAfter) : base(popAfter: popAfter) => Init();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModalPopup"/> class.
        /// </summary>
        /// <param name="target">Element or Page pointed to by Popup.</param>
        [Obsolete]
        public ModalPopup(VisualElement target) : this() { }
        #endregion


        #region Property Change management
        /// <summary>
        /// Responds to a change in a PopupBase property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            //System.Diagnostics.Debug.WriteLine ($"{this.GetType().FullName}.OnPropertyChanged property={propertyName}");
            //if (propertyName == IsPresentedProperty.PropertyName) {
            if (propertyName == TranslationXProperty.PropertyName)
            {
                Content.TranslationX = TranslationX;
                return;
            }
            if (propertyName == TranslationYProperty.PropertyName)
            {
                Content.TranslationY = TranslationY;
                return;
            }
            if (propertyName == RotationProperty.PropertyName)
            {
                Content.Rotation = Rotation;
                return;
            }
            if (propertyName == RotationXProperty.PropertyName)
            {
                Content.RotationX = RotationX;
                return;
            }
            if (propertyName == RotationYProperty.PropertyName)
            {
                Content.RotationY = RotationY;
                return;
            }

            base.OnPropertyChanged(propertyName);
        }
        #endregion


        #region Layout
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
            if (_frame?.Content == null)
                return;

            // layout the page overlay
            //base.LayoutChildren(x, y, width, height);

            height -= KeyboardService.Height;

            // layout the content
            if (width > 0 && height > 0)
            {
                _frame.IsVisible = true;
                _frame.Content.IsVisible = true;
                //ShapeBase.UpdateBasePadding(_frame, true);
                //var shadow = ShadowPadding();

                var availWidth = width - (Margin.HorizontalThickness + _frame.Padding.HorizontalThickness); // + shadow.HorizontalThickness);
                var availHeight = height - (Margin.VerticalThickness + _frame.Padding.VerticalThickness); // + shadow.VerticalThickness);
                if (_frame.Content.WidthRequest > 0)
                    availWidth = _frame.Content.WidthRequest;
                if (_frame.Content.HeightRequest > 0)
                    availHeight = _frame.Content.HeightRequest;
                var request = _frame.Content.Measure(availWidth, availHeight, MeasureFlags.None);  //

                var shadowPadding = ShapeBase.ShadowPadding(_frame);

                var rBoxWidth = HorizontalOptions.Alignment == LayoutAlignment.Fill ? availWidth : Math.Min(request.Request.Width, availWidth);// + _frame.Padding.HorizontalThickness);// + shadow.HorizontalThickness);
                rBoxWidth += _frame.Padding.HorizontalThickness;
                rBoxWidth += shadowPadding.HorizontalThickness;
                var rBoxHeight = VerticalOptions.Alignment == LayoutAlignment.Fill ? availHeight : Math.Min(request.Request.Height, availHeight);// + _frame.Padding.VerticalThickness);// + shadow.VerticalThickness);
                rBoxHeight += _frame.Padding.VerticalThickness;
                rBoxHeight += shadowPadding.VerticalThickness;
                var rboxSize = new Size(rBoxWidth, rBoxHeight);

                double contentX = Location.X;
                if (double.IsInfinity(contentX) || double.IsNaN(contentX))
                {
                    switch (HorizontalOptions.Alignment)
                    {
                        case LayoutAlignment.Fill:
                        case LayoutAlignment.Center: contentX = width / 2.0 - rboxSize.Width / 2.0; break;
                        case LayoutAlignment.Start: contentX = Margin.Left + shadowPadding.Left; break;
                        case LayoutAlignment.End: contentX = width - Margin.Right - shadowPadding.HorizontalThickness - rboxSize.Width; break;
                            //case LayoutAlignment.Fill: contentX = Margin.Left + shadowPadding.Left; break;
                    }
                }
                //var contentX = double.IsNegativeInfinity(Location.X) || HorizontalOptions.Alignment == LayoutAlignment.Fill ? width / 2.0 - rboxSize.Width / 2.0 : Location.X;
                double contentY = Location.Y;
                if (double.IsInfinity(contentY) || double.IsNaN(contentY))
                {
                    switch (VerticalOptions.Alignment)
                    {
                        case LayoutAlignment.Fill:
                        case LayoutAlignment.Center: contentY = height / 2.0 - rboxSize.Height / 2.0; break;
                        case LayoutAlignment.Start: contentY = Margin.Top + shadowPadding.Top; break;
                        case LayoutAlignment.End: contentY = height - Margin.Bottom - shadowPadding.VerticalThickness - rboxSize.Height; break;
                            //case LayoutAlignment.Fill: contentY = height / 2.0 - rboxSize.Height / 2.0; break;
                    }
                }
                //var contentY = double.IsNegativeInfinity(Location.Y) || VerticalOptions.Alignment == LayoutAlignment.Fill ? height / 2.0 - rboxSize.Height / 2.0 : Location.Y;

                var bounds = new Rectangle(contentX, contentY, rboxSize.Width, rboxSize.Height);
                //System.Diagnostics.Debug.WriteLine("LayoutChildIntoBoundingRegion("+contentX+","+contentY+","+rboxSize.Width+","+rboxSize.Height+")");

                Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_frame, bounds);

                _lastLayout = DateTime.Now;
            }
        }
        #endregion
    }
}

