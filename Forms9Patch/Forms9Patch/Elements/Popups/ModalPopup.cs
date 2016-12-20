using System;
using Xamarin.Forms;

namespace Forms9Patch {
	/// <summary>
	/// Forms9Patch Modal popup.
	/// </summary>
	public class ModalPopup : PopupBase, IBackgroundImage {


		#region Content
		/// <summary>
		/// Gets or sets the content of the FormsPopup.Modal.
		/// </summary>
		/// <value>The content.</value>
		public View Content {
			get { return _frame.Content; }
			set { _frame.Content = value; }
		}
		#endregion


		#region IBackgroundImage
		/// <summary>
		/// The background image property backing store.
		/// </summary>
		public static readonly BindableProperty BackgroundImageProperty = BindableProperty.Create ("BackgroundImage", typeof(Image), typeof(ModalPopup), null);
		/// <summary>
		/// Gets or sets the background image.
		/// </summary>
		/// <value>The background image.</value>
		public Image BackgroundImage {
			get { return (Image)GetValue (BackgroundImageProperty); }
			set { SetValue (BackgroundImageProperty, value); }
		}
			
		/// <summary>
		/// The location property backing store.
		/// </summary>
		public static readonly BindableProperty LocationProperty = BindableProperty.Create ("Location", typeof(Point), typeof(ModalPopup), new Point(double.NegativeInfinity,double.NegativeInfinity));
		/// <summary>
		/// Gets or sets the Modal Popup's location.
		/// </summary>
		/// <value>The location (default centers it in Host Page).</value>
		public Point Location {
			get { return (Point)GetValue (LocationProperty); }
			set { SetValue (LocationProperty, value); }
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
		/// Initializes a new instance of the <see cref="ModalPopup"/> class.
		/// </summary>
		/// <param name="target">Element or Page pointed to by Popup.</param>
		public ModalPopup (VisualElement target) : base (target) 
		{

			_frame = new Frame
			{
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
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
		protected override void OnPropertyChanged (string propertyName = null) {
			//System.Diagnostics.Debug.WriteLine ($"{this.GetType().FullName}.OnPropertyChanged property={propertyName}");
			//if (propertyName == IsPresentedProperty.PropertyName) {
			if (propertyName == TranslationXProperty.PropertyName) {
				Content.TranslationX = TranslationX;
				return;
			}
			if (propertyName == TranslationYProperty.PropertyName) {
				Content.TranslationY = TranslationY;
				return;
			}
			base.OnPropertyChanged (propertyName);
			if (_frame == null)
				return;
			if (propertyName == IsVisibleProperty.PropertyName) {
				if (HostPage == null)
					HostPage = Application.Current.MainPage;			
				if (HostPage != null) {
					if (IsVisible) {
						Content.TranslationX = 0;
						Content.TranslationY = 0;
						HostPage.SizeChanged += OnHostSizeChanged;
						Parent = HostPage;
						HostPage.SetValue (PopupProperty, this);
						LayoutChildIntoBoundingRegion (this, new Rectangle (0, 0, HostPage.Bounds.Width, HostPage.Bounds.Height));
						// So, Bounds is correct but the Android draw cycle seemed to happen too soon - so only the background is rendered, not the contents.
						ForceNativeLayout?.Invoke ();
					} else {
						HostPage.SizeChanged -= OnHostSizeChanged;
						HostPage.SetValue (PopupProperty, null);
						LayoutChildIntoBoundingRegion (this, new Rectangle (0, 0, -1, -1));
					}
				}
			}	
		}

		void OnHostSizeChanged(object sender, EventArgs e) {
			//Host = Host ?? Application.Current.MainPage;			
			if (HostPage != null) {
				//LayoutChildIntoBoundingRegion (this, new Rectangle (0, 0, HostPage.Bounds.Width, HostPage.Bounds.Height));
				LayoutChildIntoBoundingRegion(this, HostPage.Bounds);
				// So, Bounds is correct but the Android draw cycle seemed to happen too soon - so only the background is rendered, not the contents.
				ForceNativeLayout?.Invoke ();
				Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
				{
					//LayoutChildIntoBoundingRegion(this, new Rectangle(0, 0, HostPage.Bounds.Width, HostPage.Bounds.Height));
					LayoutChildIntoBoundingRegion(this, HostPage.Bounds);
					ForceNativeLayout?.Invoke();
					return false;
				});
			}
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
		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			if (width > 0 && height > 0) {
				//_frame.IsVisible = true;

				//LayoutChildIntoBoundingRegion (PageOverlay, new Rectangle (x, y, width, height));
				LayoutChildIntoBoundingRegion(PageOverlay, HostPage.Bounds);

				_frame.IsVisible = true;
				_frame.Content.IsVisible = true;
				RoundedBoxBase.UpdateBasePadding (_frame, true);
				var frameShadow = BubbleLayout.ShadowPadding (_frame);

				// new approach
				//var request = _frame.Measure(Host.Bounds.Width, Host.Bounds.Height, MeasureFlags.IncludeMargins);
				//var rboxSize = new Size(request.Request.Width, request.Request.Height);

				// old approach
				var availContentWidth = HostPage.Bounds.Width - Margin.HorizontalThickness - _frame.Padding.HorizontalThickness - frameShadow.HorizontalThickness;
				var availContentHeight = HostPage.Bounds.Height - Margin.VerticalThickness - _frame.Padding.VerticalThickness - frameShadow.VerticalThickness;
				var request = _frame.Content.Measure(availContentWidth, availContentHeight, MeasureFlags.None);  //
				var rBoxWidth = (HorizontalOptions.Alignment == LayoutAlignment.Fill ? availContentWidth : Math.Min(request.Request.Width,availContentWidth) + _frame.Padding.HorizontalThickness + frameShadow.HorizontalThickness);
				var rBoxHeight = (VerticalOptions.Alignment == LayoutAlignment.Fill ? availContentHeight : Math.Min(request.Request.Height,availContentHeight) +  _frame.Padding.VerticalThickness + frameShadow.VerticalThickness);
				var rboxSize = new Size(rBoxWidth, rBoxHeight);

				var contentX = double.IsNegativeInfinity(Location.X) || HorizontalOptions.Alignment == LayoutAlignment.Fill ? width  / 2.0 - rboxSize.Width  / 2.0 : Location.X;
				var contentY = double.IsNegativeInfinity(Location.Y) || VerticalOptions.Alignment == LayoutAlignment.Fill ? height / 2.0 - rboxSize.Height / 2.0 : Location.Y;

				var bounds = new Rectangle(contentX, contentY, rboxSize.Width, rboxSize.Height);
				//System.Diagnostics.Debug.WriteLine("LayoutChildIntoBoundingRegion("+contentX+","+contentY+","+rboxSize.Width+","+rboxSize.Height+")");

				LayoutChildIntoBoundingRegion (_frame, bounds);
				//_frame.ForceLayout ();
			} else
				_frame.IsVisible = false;
		}


	}
}

