using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Modal popup.
    /// </summary>
    public class ModalPopup : PopupBase, IBackgroundImage
    {


        #region Content
        /// <summary>
        /// Gets or sets the content of the FormsPopup.Modal.
        /// </summary>
        /// <value>The content.</value>
        public View Content
        {
            get { return _frame.Content; }
            set { _frame.Content = value; }
        }
        #endregion


        #region IBackgroundImage
        /// <summary>
        /// The background image property backing store.
        /// </summary>
        public static readonly BindableProperty BackgroundImageProperty = BindableProperty.Create("BackgroundImage", typeof(Image), typeof(ModalPopup), null);
        /// <summary>
        /// Gets or sets the background image.
        /// </summary>
        /// <value>The background image.</value>
        public Image BackgroundImage
        {
            get { return (Image)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

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
            get { return (Point)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
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


        #region Fields
        readonly Frame _frame;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ModalPopup"/> class.
        /// </summary>
        /// <param name="retain">If set to <c>true</c> retain.</param>
        public ModalPopup(bool retain = false) : base(retain: retain)
        {
            _frame = new Frame
            {
                Padding = Padding,
                HasShadow = HasShadow,
                OutlineColor = OutlineColor,
                OutlineWidth = OutlineWidth,
                OutlineRadius = OutlineRadius,
                BackgroundColor = BackgroundColor
            };
            ContentView = _frame;

            Margin = 0;
            Padding = 10;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModalPopup"/> class.
        /// </summary>
        /// <param name="target">Element or Page pointed to by Popup.</param>
        [Obsolete]
        public ModalPopup(VisualElement target) : base()
        {
            _frame = new Frame
            {
                Padding = Padding,
                HasShadow = HasShadow,
                OutlineColor = OutlineColor,
                OutlineWidth = OutlineWidth,
                OutlineRadius = OutlineRadius,
                BackgroundColor = BackgroundColor
            };
            ContentView = _frame;

            Margin = 0;
            Padding = 10;

        }


        /// <summary>
        /// Responds to a change in a PopupBase property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
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
            base.OnPropertyChanged(propertyName);
            if (_frame == null)
                return;
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
            if (_frame?.Content == null)
                return;
            base.LayoutChildren(x, y, width, height);

            if (width > 0 && height > 0)
            {
                _frame.IsVisible = true;
                _frame.Content.IsVisible = true;
                RoundedBoxBase.UpdateBasePadding(_frame, true);
                var shadow = BubbleLayout.ShadowPadding(_frame);

                var availWidth = width - (Margin.HorizontalThickness + _frame.Padding.HorizontalThickness + shadow.HorizontalThickness);
                var availHeight = height - (Margin.VerticalThickness + _frame.Padding.VerticalThickness + shadow.VerticalThickness);
                if (_frame.Content.WidthRequest > 0)
                    availWidth = _frame.Content.WidthRequest;
                if (_frame.Content.HeightRequest > 0)
                    availHeight = _frame.Content.HeightRequest;
                var request = _frame.Content.Measure(availWidth, availHeight, MeasureFlags.None);  //
                var rBoxWidth = (HorizontalOptions.Alignment == LayoutAlignment.Fill ? availWidth : Math.Min(request.Request.Width, availWidth) + _frame.Padding.HorizontalThickness + shadow.HorizontalThickness);
                var rBoxHeight = (VerticalOptions.Alignment == LayoutAlignment.Fill ? availHeight : Math.Min(request.Request.Height, availHeight) + _frame.Padding.VerticalThickness + shadow.VerticalThickness);
                var rboxSize = new Size(rBoxWidth, rBoxHeight);

                var contentX = double.IsNegativeInfinity(Location.X) || HorizontalOptions.Alignment == LayoutAlignment.Fill ? width / 2.0 - rboxSize.Width / 2.0 : Location.X;
                var contentY = double.IsNegativeInfinity(Location.Y) || VerticalOptions.Alignment == LayoutAlignment.Fill ? height / 2.0 - rboxSize.Height / 2.0 : Location.Y;

                var bounds = new Rectangle(contentX, contentY, rboxSize.Width, rboxSize.Height);
                //System.Diagnostics.Debug.WriteLine("LayoutChildIntoBoundingRegion("+contentX+","+contentY+","+rboxSize.Width+","+rboxSize.Height+")");

                LayoutChildIntoBoundingRegion(_frame, bounds);
            }
        }


    }
}

