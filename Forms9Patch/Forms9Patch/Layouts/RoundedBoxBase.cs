using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Rounded box base.
	/// </summary>
	public static class RoundedBoxBase 
	{
		/// <summary>
		/// Backing store for the HasShadow bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static readonly BindableProperty HasShadowProperty = BindableProperty.Create ("HasShadow", typeof(bool), typeof(RoundedBoxBase), false, BindingMode.OneWay, UpdateBasePadding);

		/// <summary>
		/// Inverts shadow to create an embossed effect
		/// </summary>
		public static readonly BindableProperty ShadowInvertedProperty = BindableProperty.Create("ShadowInverted", typeof(bool), typeof(RoundedBoxBase), false);

		/// <summary>
		/// Backing store for the OutlineColor bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create ("OutlineColor", typeof(Color), typeof(RoundedBoxBase), Color.Default);

		/// <summary>
		/// Backing store for the OutlineRadius bindable property.
		/// </summary>
		public static readonly BindableProperty OutlineRadiusProperty = BindableProperty.Create("OutlineRadius", typeof (float), typeof (RoundedBoxBase), (object) -1.0f);

		/// <summary>
		/// Backing store for the OutlineWidth bindable property.
		/// </summary>
		public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("OutlineWidth", typeof (float), typeof (RoundedBoxBase), (object) -1.0f);

		/// <summary>
		/// Identifies the Padding bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static readonly BindableProperty PaddingProperty = BindableProperty.Create ("Padding", typeof(Thickness), typeof(RoundedBoxBase), new Thickness (0), BindingMode.OneWay, UpdateBasePadding);

		/// <summary>
		/// The elliptical property backing store.
		/// </summary>
		public static readonly BindableProperty IsEllipticalProperty = BindableProperty.Create("IsElliptical", typeof(bool), typeof(RoundedBoxBase), false);

        /// <summary>
        /// The elliptical property backing store.
        /// </summary>
        public static readonly BindableProperty BackgroundImageProperty = BindableProperty.Create("BackgroundImage", typeof(Forms9Patch.Image), typeof(RoundedBoxBase), default(Forms9Patch.Image));

        internal static bool UpdateBasePadding(BindableObject bindable, object newValue) {
			var layout = bindable as Layout;
			Thickness layoutPadding;
			if (newValue is bool)
				layoutPadding = (Thickness)layout.GetValue (RoundedBoxBase.PaddingProperty);
			else
				layoutPadding = (Thickness)newValue;
			var materialButton = layout as MaterialButton;
			var hasShadow = (bool)layout.GetValue (RoundedBoxBase.HasShadowProperty);
			var makeRoomForShadow = materialButton == null ? hasShadow : (bool)layout.GetValue (MaterialButton.HasShadowProperty);

			if (makeRoomForShadow) {
				
				var shadowPadding = ShadowPadding (layout);

				Thickness newPadding;
				double xLeft = layoutPadding.Left + shadowPadding.Left;
				double xTop = layoutPadding.Top + shadowPadding.Top;
				double xRight = layoutPadding.Right + shadowPadding.Right;
				double xBottom = layoutPadding.Bottom + shadowPadding.Bottom;
				newPadding = new Thickness (xLeft, xTop, xRight, xBottom);

				layout.SetValue (Layout.PaddingProperty, newPadding);
				//System.Diagnostics.Debug.WriteLine ("\tRoundedBoxBase.UpdateBasePadding newPadding: " + newPadding.Description ());
			} else {
				layout.SetValue (Layout.PaddingProperty, layoutPadding);
				//System.Diagnostics.Debug.WriteLine ("\tRoundedBoxBase.UpdateBasePadding layoutPadding: " + layoutPadding.Description ());
			}
			//var contentView = bindable as ContentView;
			return true;
		}


		internal static Thickness ShadowPadding(Layout layout) {
			if (layout == null)
				return new Thickness (0);
			var materialButton = layout as MaterialButton;
			var hasShadow = (bool)layout.GetValue (RoundedBoxBase.HasShadowProperty);
			var makeRoomForShadow = materialButton == null ? hasShadow : (bool)layout.GetValue (MaterialButton.HasShadowProperty);

			if (makeRoomForShadow) {
				ButtonShape type = materialButton == null ? ButtonShape.Rectangle : materialButton.SegmentType;
				StackOrientation orientation = materialButton == null ? StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

				var shadowX = Settings.ShadowOffset.X;
				var shadowY = Settings.ShadowOffset.Y;
				var shadowR = Settings.ShadowRadius;

				// additional padding alocated for the button's shadow
				var padL = orientation == StackOrientation.Horizontal && (type == ButtonShape.SegmentMid || type == ButtonShape.SegmentEnd) ? 0 : Math.Max (0, shadowR - shadowX);
				var padR = orientation == StackOrientation.Horizontal && (type == ButtonShape.SegmentStart || type == ButtonShape.SegmentMid) ? 0 : Math.Max (0, shadowR + shadowX);
				var padT = orientation == StackOrientation.Vertical && (type == ButtonShape.SegmentMid || type == ButtonShape.SegmentEnd) ? 0 : Math.Max (0, shadowR - shadowY);
				var padB = orientation == StackOrientation.Vertical && (type == ButtonShape.SegmentStart || type == ButtonShape.SegmentMid) ? 0 : Math.Max (0, shadowR + shadowY);

				return new Thickness (padL, padT, padR, padB);
			} else
				return new Thickness (0);
		}

	}
}

