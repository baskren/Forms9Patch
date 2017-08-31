using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch AbsoluteLayout.
	/// </summary>
	public class VisualElement<TElement> : TElement, IRoundedBox, IBackgroundImage where TElement : VisualElement
	{

		#region debug support
		static int _count = 0;
		int _id;

		/*
		public VisualElement ()
		{
			_id = _count++;
		}
		*/

		internal VisualElement() : this (1) {
		}

		VisualElement(int i)  : this() {
			_id = _count++;
		}


		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Forms9Patch.Frame"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Forms9Patch.Frame"/>.</returns>
		public string Description () { return string.Format ("[{0}.{1}]",GetType(),_id); }
		#endregion

		#region Properties
		/// <summary>
		/// Backing store for the BackgroundImage bindable property.
		/// </summary>
		public static BindableProperty BackgroundImageProperty = BindableProperty.Create ("BackgroundImage", typeof(Image), typeof(VisualElement<TElement>), null);
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
		public static readonly BindableProperty HasShadowProperty = BindableProperty.Create ("HasShadow", typeof(bool), typeof(VisualElement<TElement>), true);
		/// <summary>
		/// Gets or sets a flag indicating if the Frame has a shadow displayed. This is a bindable property.
		/// </summary>
		/// <value><c>true</c> if this instance has shadow; otherwise, <c>false</c>.</value>
		public bool HasShadow {
			get { return (bool)GetValue (Frame.HasShadowProperty); }
			set { SetValue (Frame.HasShadowProperty, value); }
		}


		/// <summary>
		/// Backing store for the OutlineColor bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create ("OutlineColor", typeof(Color), typeof(VisualElement<TElement>), Color.Default);
		/// <summary>
		/// Gets or sets the color of the border of the Frame. This is a bindable property.
		/// </summary>
		/// <value>The color of the outline.</value>
		public Color OutlineColor {
			get { return (Color)GetValue (Frame.OutlineColorProperty); }
			set { SetValue (Frame.OutlineColorProperty, value); }
		}

		/// <summary>
		/// Backing store for the OutlineRadius bindable property.
		/// </summary>
		public static readonly BindableProperty OutlineRadiusProperty = BindableProperty.Create("OutlineRadius", typeof (double), typeof (VisualElement<TElement>), (object) -1.0);
		/// <summary>
		/// Gets or sets the outline radius.
		/// </summary>
		/// <value>The outline radius.</value>
		public double OutlineRadius {
			get { return (double) GetValue (OutlineRadiusProperty); }
			set { SetValue (OutlineRadiusProperty, value); }
		}

		/// <summary>
		/// Backing store for the OutlineWidth bindable property.
		/// </summary>
		public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("OutlineWidth", typeof (double), typeof (VisualElement<TElement>), (object) -1.0);
		/// <summary>
		/// Gets or sets the width of the outline.
		/// </summary>
		/// <value>The width of the outline.</value>
		public double OutlineWidth {
			get { return (double) GetValue (OutlineWidthProperty); }
			set { SetValue (OutlineWidthProperty, value); }
		}

		#endregion
	
	}
}

