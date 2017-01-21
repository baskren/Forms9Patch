using System;
using Xamarin.Forms;
namespace Forms9Patch
{
	/// <summary>
	/// Manual layout.
	/// </summary>
	public class ManualLayout : Xamarin.Forms.Layout<View>, IRoundedBox, IBackgroundImage
	{

		#region Properties
		/// <summary>
		/// Backing store for the BackgroundImage bindable property.
		/// </summary>
		public static BindableProperty BackgroundImageProperty = BindableProperty.Create("BackgroundImage", typeof(Image), typeof(ManualLayout), null);
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
		/// Backing store for the HasShadow bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static readonly BindableProperty HasShadowProperty = RoundedBoxBase.HasShadowProperty;
		/// <summary>
		/// Gets or sets a flag indicating if the ManualLayout has a shadow displayed. This is a bindable property.
		/// </summary>
		/// <value><c>true</c> if this instance has shadow; otherwise, <c>false</c>.</value>
		public bool HasShadow
		{
			get { return (bool)GetValue(HasShadowProperty); }
			set { SetValue(HasShadowProperty, value); }
		}

		/// <summary>
		/// Backing store for the ShadowInverted bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static readonly BindableProperty ShadowInvertedProperty = RoundedBoxBase.ShadowInvertedProperty;
		/// <summary>
		/// Gets or sets a flag indicating if the ManualLayout has a shadow inverted. This is a bindable property.
		/// </summary>
		/// <value><c>true</c> if this instance's shadow is inverted; otherwise, <c>false</c>.</value>
		public bool ShadowInverted
		{
			get { return (bool)GetValue(ShadowInvertedProperty); }
			set { SetValue(ShadowInvertedProperty, value); }
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
		public Color OutlineColor
		{
			get { return (Color)GetValue(OutlineColorProperty); }
			set { SetValue(OutlineColorProperty, value); }
		}

		/// <summary>
		/// Backing store for the OutlineRadius bindable property.
		/// </summary>
		public static readonly BindableProperty OutlineRadiusProperty = RoundedBoxBase.OutlineRadiusProperty;
		/// <summary>
		/// Gets or sets the outline radius.
		/// </summary>
		/// <value>The outline radius.</value>
		public float OutlineRadius
		{
			get { return (float)GetValue(OutlineRadiusProperty); }
			set { SetValue(OutlineRadiusProperty, value); }
		}

		/// <summary>
		/// Backing store for the OutlineWidth bindable property.
		/// </summary>
		public static readonly BindableProperty OutlineWidthProperty = RoundedBoxBase.OutlineWidthProperty;
		/// <summary>
		/// Gets or sets the width of the outline.
		/// </summary>
		/// <value>The width of the outline.</value>
		public float OutlineWidth
		{
			get { return (float)GetValue(OutlineWidthProperty); }
			set { SetValue(OutlineWidthProperty, value); }
		}

		/// <summary>
		/// Identifies the Padding bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static new readonly BindableProperty PaddingProperty = RoundedBoxBase.PaddingProperty;
		/// <summary>
		/// Gets or sets the inner padding of the Layout.
		/// </summary>
		/// <value>The Thickness values for the layout. The default value is a Thickness with all values set to 0.</value>
		public new Thickness Padding
		{
			get { return (Thickness)GetValue(PaddingProperty); }
			set { SetValue(PaddingProperty, value); }
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
		/// Occurs when layout children event is triggered.
		/// </summary>
		public event EventHandler<ManualLayoutEventArgs> LayoutChildrenEvent;

		/// <summary>
		/// Layouts the children.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			//System.Diagnostics.Debug.WriteLine("ManualLayout.LayoutChildren("+x+", "+y+", "+width+", "+height+")");
			LayoutChildrenEvent?.Invoke(this, new ManualLayoutEventArgs(x, y, width, height));
		}
	}

	/// <summary>
	/// Manual layout event arguments.
	/// </summary>
	public class ManualLayoutEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the x.
		/// </summary>
		/// <value>The x.</value>
		public double X { get; }

		/// <summary>
		/// Gets the y.
		/// </summary>
		/// <value>The y.</value>
		public double Y { get; }

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		public double Width { get; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public double Height { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.ManualLayoutEventArgs"/> class.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public ManualLayoutEventArgs(double x, double y, double width, double height) 
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}


	}
}
