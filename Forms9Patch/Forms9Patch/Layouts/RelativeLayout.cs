using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch RelativeLayout.
	/// </summary>
	public class RelativeLayout : Xamarin.Forms.RelativeLayout, IRoundedBox, IBackgroundImage
	{

		#region debug support
		static int _count = 0;
		int _id;

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.RelativeLayout"/> class.
		/// </summary>
		public RelativeLayout ()
		{
			_id = _count++;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Forms9Patch.RelativeLayout"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Forms9Patch.RelativeLayout"/>.</returns>
		public string Description () { return string.Format ("[{0}.{1}]",GetType(),_id); }
		#endregion

		#region Properties
		/// <summary>
		/// Backing store for the BackgroundImage bindable property.
		/// </summary>
		public static BindableProperty BackgroundImageProperty = BindableProperty.Create ("BackgroundImage", typeof(Image), typeof(RelativeLayout), null);
		/// <summary>
		/// Gets or sets the background image.
		/// </summary>
		/// <value>The background image.</value>
		public Image BackgroundImage {
			get { return (Image)GetValue (BackgroundImageProperty); }
			set { SetValue (BackgroundImageProperty, value); }
		}

		/// <summary>
		/// Backing store for the HasShadow bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static readonly BindableProperty HasShadowProperty = RoundedBoxBase.HasShadowProperty;
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
		/// Gets or sets a flag indicating if the RelativeLayout's shadow is inverted. This is a bindable property.
		/// </summary>
		/// <value><c>true</c> if this instance's shadow is inverted; otherwise, <c>false</c>.</value>
		public bool ShadowInverted {
			get { return (bool)GetValue (ShadowInvertedProperty); }
			set { SetValue (ShadowInvertedProperty, value); }
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
			get { return (Color)GetValue (OutlineColorProperty); }
			set { SetValue (OutlineColorProperty, value); }
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
			get { return (float) GetValue (OutlineRadiusProperty); }
			set { SetValue (OutlineRadiusProperty, value); }
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
			get { return (float) GetValue (OutlineWidthProperty); }
			set { SetValue (OutlineWidthProperty, value); }
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
		public new Thickness Padding {
			get { return (Thickness)GetValue (PaddingProperty); }
			set { SetValue (PaddingProperty, value); }
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
	
		/// <param name="propertyName">The name of the property that changed.</param>
		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		protected override void OnPropertyChanged (string propertyName = null)
		{
			base.OnPropertyChanged (propertyName);
			if (propertyName == PaddingProperty.PropertyName ||
				propertyName == HasShadowProperty.PropertyName)
				InvalidateLayout ();
		}


	}
}

