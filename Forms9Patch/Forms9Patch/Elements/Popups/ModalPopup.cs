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
		/// <param name="host">Host.</param>
		public ModalPopup (Page host=null) : base (host: host) 
		{
			Margin = 0;
			Padding = 10;
			_frame = new Frame {
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center
			};
			SetRoundedBoxBindings (_frame);
			_frame.SetBinding (Frame.HasShadowProperty, "HasShadow");
			_frame.SetBinding (Frame.BackgroundImageProperty, "BackgroundImage");
			_frame.SetBinding (Frame.PaddingProperty, "Padding");
			_frame.SetBinding(View.MarginProperty, MarginProperty.PropertyName);
			_frame.SetBinding(View.HorizontalOptionsProperty, HorizontalOptionsProperty.PropertyName);
			_frame.SetBinding(View.VerticalOptionsProperty, VerticalOptionsProperty.PropertyName);

			_frame.BindingContext = this;
			ContentView = _frame;
		}


		/// <summary>
		/// Responds to a change in a PopupBase property.
		/// </summary>
		/// <param name="propertyName">The name of the property that changed.</param>
		protected override void OnPropertyChanged (string propertyName = null) {
			//System.Diagnostics.Debug.WriteLine ($"{this.GetType().FullName}.OnPropertyChanged property={propertyName}");
			//if (propertyName == IsPresentedProperty.PropertyName) {
			if (propertyName == TranslationXProperty.PropertyName) {
				_frame.TranslationX = TranslationX;
				return;
			}
			if (propertyName == TranslationYProperty.PropertyName) {
				_frame.TranslationY = TranslationY;
				return;
			}
			base.OnPropertyChanged (propertyName);
			if (_frame == null)
				return;
			if (propertyName == IsVisibleProperty.PropertyName) {
				if (Host == null)
					Host = Application.Current.MainPage;			
				if (Host != null) {
					if (IsVisible) {
						TranslationX = 0;
						TranslationY = 0;
						Host.SizeChanged += OnHostSizeChanged;
						Parent = Host;
						Host.SetValue (PopupProperty, this);
						LayoutChildIntoBoundingRegion (this, new Rectangle (0, 0, Host.Bounds.Width, Host.Bounds.Height));
						// So, Bounds is correct but the Android draw cycle seemed to happen too soon - so only the background is rendered, not the contents.
						ForceNativeLayout?.Invoke ();
					} else {
						Host.SizeChanged -= OnHostSizeChanged;
						Host.SetValue (PopupProperty, null);
						LayoutChildIntoBoundingRegion (this, new Rectangle (0, 0, -1, -1));
					}
				}
			}	
		}

		void OnHostSizeChanged(object sender, EventArgs e) {
			//Host = Host ?? Application.Current.MainPage;			
			if (Host != null) {
				LayoutChildIntoBoundingRegion (this, new Rectangle (0, 0, Host.Bounds.Width, Host.Bounds.Height));
				// So, Bounds is correct but the Android draw cycle seemed to happen too soon - so only the background is rendered, not the contents.
				ForceNativeLayout?.Invoke ();
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
				_frame.IsVisible = true;

				//LayoutChildIntoBoundingRegion (PageOverlay, new Rectangle (x, y, width, height));
				LayoutChildIntoBoundingRegion(PageOverlay, Host.Bounds);

				RoundedBoxBase.UpdateBasePadding (_frame, true);
				var shadow = BubbleLayout.ShadowPadding (_frame);

				// new approach
				var request = _frame.Measure(Host.Bounds.Width, Host.Bounds.Height, MeasureFlags.IncludeMargins);
				var rboxSize = new Size(request.Request.Width, request.Request.Height);

				// old approach
				//var request = _frame.Content.Measure(Host.Bounds.Width - Margin.HorizontalThickness - _frame.Padding.HorizontalThickness - shadow.HorizontalThickness, Host.Bounds.Height - Margin.VerticalThickness - _frame.Padding.VerticalThickness - shadow.VerticalThickness, MeasureFlags.None);  //
				//var rBoxWidth = (HorizontalOptions.Alignment == LayoutAlignment.Fill ? width : request.Request.Width + Margin.HorizontalThickness + _frame.Padding.HorizontalThickness + shadow.HorizontalThickness);
				//var rBoxHeight = (VerticalOptions.Alignment == LayoutAlignment.Fill ? height : request.Request.Height + Margin.VerticalThickness + _frame.Padding.VerticalThickness + shadow.VerticalThickness);
				//var rboxSize = new Size(rBoxWidth, rBoxHeight);

				var contentX = double.IsNegativeInfinity(Location.X) || HorizontalOptions.Alignment == LayoutAlignment.Fill ? width  / 2.0 - rboxSize.Width  / 2.0 : Location.X;
				var contentY = double.IsNegativeInfinity(Location.Y) || VerticalOptions.Alignment == LayoutAlignment.Fill ? height / 2.0 - rboxSize.Height / 2.0 : Location.Y;
				LayoutChildIntoBoundingRegion (ContentView, new Rectangle (contentX, contentY, rboxSize.Width, rboxSize.Height));
				//_frame.ForceLayout ();
			} else
				_frame.IsVisible = false;
		}


	}
}

