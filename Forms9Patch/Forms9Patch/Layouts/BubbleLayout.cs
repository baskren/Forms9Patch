using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Bubble layout.
	/// </summary>
	internal class BubbleLayout: Xamarin.Forms.ContentView, IRoundedBox
	{
		#region Pointer Properties
		/// <summary>
		/// Backing store for pointer length property.
		/// </summary>
		public static readonly BindableProperty PointerLengthProperty = BindableProperty.Create("PointerLength",typeof(float), typeof(BubbleLayout), 4.0f,  propertyChanged: UpdateBasePadding);
		/// <summary>
		/// Gets or sets the length of the bubble layout's pointer.
		/// </summary>
		/// <value>The length of the pointer.</value>
		public float PointerLength {
			get { return (float)GetValue (PointerLengthProperty); }
			set { SetValue (PointerLengthProperty, value); }
		}

		/// <summary>
		/// Backing store for pointer tip radius property.
		/// </summary>
		public static readonly BindableProperty PointerTipRadiusProperty = BindableProperty.Create("PointerTipRadius", typeof(float), typeof(BubbleLayout), 2.0f, propertyChanged: UpdateBasePadding);
		/// <summary>
		/// Gets or sets the radius of the bubble's pointer tip.
		/// </summary>
		/// <value>The pointer tip radius.</value>
		public float PointerTipRadius {
			get { return (float)GetValue (PointerTipRadiusProperty); }
			set { SetValue (PointerTipRadiusProperty, value); }
		}

		/// <summary>
		/// Backing store for pointer axial position property.
		/// </summary>
		public static readonly BindableProperty PointerAxialPositionProperty = BindableProperty.Create("PointerAxialPosition", typeof(float), typeof(BubbleLayout), 0.5f);
		/// <summary>
		/// Gets or sets the position of the bubble's pointer along the face it's on.
		/// </summary>
		/// <value>The pointer axial position (left/top is zero).</value>
		public float PointerAxialPosition {
			get { return (float)GetValue (PointerAxialPositionProperty); }
			set { SetValue (PointerAxialPositionProperty, value); }
		}

		/// <summary>
		/// Backing store for pointer direction property.
		/// </summary>
		public static readonly BindableProperty PointerDirectionProperty = BindableProperty.Create("PointerDirection", typeof(PointerDirection), typeof(BubbleLayout), PointerDirection.Any, propertyChanged: UpdateBasePadding);
		/// <summary>
		/// Gets or sets the direction in which the pointer pointing.
		/// </summary>
		/// <value>The pointer direction.</value>
		public PointerDirection PointerDirection {
			get { 
				return (PointerDirection)GetValue (PointerDirectionProperty); 
			}
			set { 
				SetValue (PointerDirectionProperty, value); 
			}
		}

		/// <summary>
		/// The pointer corner radius property.
		/// </summary>
		public static readonly BindableProperty PointerCornerRadiusProperty = BindableProperty.Create( "PointerCornerRadius", typeof(float), typeof(BubbleLayout), 0f);
		/// <summary>
		/// Gets or sets the pointer corner radius.
		/// </summary>
		/// <value>The pointer corner radius.</value>
		public float PointerCornerRadius {
			get {return (float)GetValue(PointerCornerRadiusProperty); }
			set { 
				SetValue (PointerCornerRadiusProperty, value); 
			}
		}


		#endregion


		#region IRoundedBox Properties
		/// <summary>
		/// Backing store for the HasShadow bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static readonly BindableProperty HasShadowProperty = BindableProperty.Create ("HasShadow", typeof(bool), typeof(BubbleLayout), false, propertyChanged: UpdateBasePadding);
		/// <summary>
		/// Gets or sets a flag indicating if the AbsoluteLayout has a shadow displayed. This is a bindable property.
		/// </summary>
		/// <value><c>true</c> if this instance has shadow; otherwise, <c>false</c>.</value>
		public bool HasShadow {
			get { return (bool)GetValue (HasShadowProperty); }
			set { SetValue (HasShadowProperty, value); }
		}


		/// <summary>
		/// Backing store for the ShadowInverted bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static readonly BindableProperty ShadowInvertedProperty = RoundedBoxBase.ShadowInvertedProperty;
		/// <summary>
		/// Gets or sets a flag indicating if the Frame's shadow is inverted. This is a bindable property.
		/// </summary>
		/// <value><c>true</c> if this instance's shadow is inverted; otherwise, <c>false</c>.</value>
		public bool ShadowInverted {
			get { return (bool)GetValue (RoundedBoxBase.ShadowInvertedProperty); }
			set { SetValue (RoundedBoxBase.ShadowInvertedProperty, value); }
		}


		/// <summary>
		/// Backing store for the OutlineColor bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static readonly BindableProperty OutlineColorProperty = RoundedBoxBase.OutlineColorProperty;
		/// <summary>
		/// Gets or sets the color of the border of the AbsoluteLayout. This is a bindable property.
		/// </summary>
		/// <value>The color of the outline.</value>
		public Color OutlineColor {
			get { return (Color)GetValue (RoundedBoxBase.OutlineColorProperty); }
			set { SetValue (RoundedBoxBase.OutlineColorProperty, value); }
		}

		/// <summary>
		/// Backing store for the OutlineRadius bindable property.
		/// </summary>
		public static readonly BindableProperty OutlineRadiusProperty = RoundedBoxBase.OutlineRadiusProperty;
		/// <summary>
		/// Gets or sets the outline radius.
		/// </summary>
		/// <value>The outline radius.</value>
		public float OutlineRadius {
			get { return (float) GetValue (RoundedBoxBase.OutlineRadiusProperty); }
			set { SetValue (RoundedBoxBase.OutlineRadiusProperty, value); }
		}

		/// <summary>
		/// Backing store for the OutlineWidth bindable property.
		/// </summary>
		public static readonly BindableProperty OutlineWidthProperty = RoundedBoxBase.OutlineWidthProperty;
		/// <summary>
		/// Gets or sets the width of the outline.
		/// </summary>
		/// <value>The width of the outline.</value>
		public float OutlineWidth {
			get { return (float) GetValue (RoundedBoxBase.OutlineWidthProperty); }
			set { SetValue (RoundedBoxBase.OutlineWidthProperty, value); }
		}

		/// <summary>
		/// Identifies the Padding bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static new readonly BindableProperty PaddingProperty = BindableProperty.Create ("Padding", typeof(Thickness), typeof(BubbleLayout), new Thickness (20), BindingMode.OneWay, propertyChanged: UpdateBasePadding);
		/// <summary>
		/// Gets or sets the inner padding of the Layout.
		/// </summary>
		/// <value>The Thickness values for the layout. The default value is a Thickness with all values set to 0.</value>
		public new Thickness Padding {
			get { return (Thickness)GetValue (PaddingProperty); }
			set { SetValue (PaddingProperty, value); }
		}


		/// <summary>
		/// Identifies the BackgroundColor bindable property.
		/// </summary>
		/// <remarks>To be added.</remarks>
		public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create ("BackgroundColor", typeof(Color), typeof(BubbleLayout), Color.White);
		/// <summary>
		/// Gets or sets the color which will fill the background of a VisualElement. This is a bindable property.
		/// </summary>
		/// <value>The color of the background.</value>
		public new Color BackgroundColor {
			get { return (Color)GetValue (BackgroundColorProperty); }
			set { 
				SetValue (BackgroundColorProperty, value); 
			}
		}

		/// <summary>
		/// The is elliptical property backing store.
		/// </summary>
		public static readonly BindableProperty IsEllipticalProperty = RoundedBoxBase.IsEllipticalProperty;
		/// <summary>
		/// Gets or sets a value indicating whether this Element is elliptical (rather than rectangular).
		/// </summary>
		/// <value><c>true</c> if is elliptical; otherwise, <c>false</c>.</value>
		public bool IsElliptical
		{
			get { return (bool)GetValue(IsEllipticalProperty); }
			set { SetValue(IsEllipticalProperty, value); }
		}
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.BubbleLayout"/> class.
		/// </summary>
		public BubbleLayout() {
			Padding = 10;
			//BackgroundColor = Color.White;
		}

		/// <param name="propertyName">The name of the property that changed.</param>
		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		protected override void OnPropertyChanged (string propertyName = null)
		{
			base.OnPropertyChanged (propertyName);
			if (propertyName == PaddingProperty.PropertyName ||
			    propertyName == HasShadowProperty.PropertyName ||
			    propertyName == PointerLengthProperty.PropertyName)
				InvalidateLayout ();
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
			//System.Diagnostics.Debug.WriteLine ("\t\tBubbleLayout.LayoutChildren({0},{1},{2},{3})", x, y, width, height);
			//System.Diagnostics.Debug.WriteLine ("\t\tBubbleLayout.Padding=[{0}, {1}, {2}, {3}]",Padding.Left, Padding.Top, Padding.Right, Padding.Bottom);
			//System.Diagnostics.Debug.WriteLine ($"\t\tBubbleLayout.base.Padding=[{base.Padding.Left}, {base.Padding.Top}, {base.Padding.Right}, {base.Padding.Bottom}]");
			//System.Diagnostics.Debug.WriteLine ("\t\tBubbleLayout.Content.Padding=[{0}, {1}, {2}, {3}]",((StackLayout)Content).Padding.Left, ((StackLayout)Content).Padding.Top, ((StackLayout)Content).Padding.Right, ((StackLayout)Content).Padding.Bottom);
			Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion (base.Content, new Rectangle (x, y, width, height));
		}

		#region Base Padding Calculation
		internal static void UpdateBasePadding(BindableObject bindable, object oldValue, object newValue) {
			var layout = bindable as BubbleLayout;

			Thickness layoutPadding;
			if (newValue is Thickness)
				layoutPadding = (Thickness)newValue;
			else
				layoutPadding = layout.Padding;

			double xLeft = layoutPadding.Left + (layout.PointerDirection == PointerDirection.Left ? layout.PointerLength : 0);
			double xTop = layoutPadding.Top + (layout.PointerDirection==PointerDirection.Up ? layout.PointerLength : 0);
			double xRight = layoutPadding.Right + (layout.PointerDirection==PointerDirection.Right ? layout.PointerLength : 0);
			double xBottom = layoutPadding.Bottom + (layout.PointerDirection==PointerDirection.Down ? layout.PointerLength : 0);

			if (layout.HasShadow) {
				var shadowPadding = ShadowPadding (layout);
				xLeft += shadowPadding.Left;
				xTop +=  shadowPadding.Top;
				xRight += shadowPadding.Right;
				xBottom += shadowPadding.Bottom;
			} 
			var newPadding = new Thickness(xLeft, xTop, xRight, xBottom);
			layout.SetValue(Xamarin.Forms.Layout.PaddingProperty, newPadding);
			//System.Diagnostics.Debug.WriteLine ("newPadding: " + newPadding.Description ());
			return;
		}

		internal static Thickness ShadowPadding(IRoundedBox layout) {
			if (layout == null)
				return new Thickness (0);
			//var materialButton = layout as MaterialButton;
			//var hasShadow = (bool)layout.GetValue (HasShadowProperty);
			//var makeRoomForShadow = materialButton == null ? hasShadow : (bool)layout.GetValue (MaterialButton.HasShadowProperty);

			if (layout.HasShadow) {

				var shadowX = Settings.ShadowOffset.X;
				var shadowY = Settings.ShadowOffset.Y;
				var shadowR = Settings.ShadowRadius;

				// additional padding alocated for the button's shadow
				var padL = Math.Max (0, shadowR - shadowX);
				var padR = Math.Max (0, shadowR + shadowX);
				var padT = Math.Max (0, shadowR - shadowY);
				var padB = Math.Max (0, shadowR + shadowY);

				return new Thickness (padL, padT, padR, padB);
			} else
				return new Thickness (0);
		}
		#endregion

	}
}

