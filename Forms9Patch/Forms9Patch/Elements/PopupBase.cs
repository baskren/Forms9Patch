using Xamarin.Forms;
using System;
using FormsGestures;
using System.Collections.Generic;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Popup base.
	/// </summary>
	[ContentProperty("Content")]
	public abstract class PopupBase : Xamarin.Forms.AbsoluteLayout, IRoundedBox, IDisposable //Xamarin.Forms.Layout<View>, IRoundedBox

	{
		#region Invalid Parent Properties
		/// <summary>
		/// Invalid Property, do not use
		/// </summary>
		/// <value>Invalid</value>
		/// <remarks>Do not use</remarks>
		public new IList<View> Children {
			get {
				throw new InvalidOperationException ("Children property is not valid for Forms9Patch popups. Use Content property instead.");
			}
		}
		#endregion


		#region Popup Properties
		internal static readonly BindableProperty PopupProperty = BindableProperty.Create("Popup", typeof(PopupBase), typeof(PopupBase), null);

		/// <summary>
		/// The host page property.
		/// </summary>
		public static readonly BindableProperty HostProperty = BindableProperty.Create ("Host", typeof(Page), typeof(PopupBase), default(Page));
		/// <summary>
		/// Gets or sets the popup's host page.
		/// </summary>
		/// <value>The host page (null defaults to MainPage).</value>
		public Page Host {
			get { return (Page)GetValue (HostProperty); }
			set { 
				var effect = Effect.Resolve ("Forms9Patch.PopupEffect");
				Host?.Effects.Remove (effect);
				SetValue (HostProperty, value); 
				Host?.Effects.Add (effect);
				if (!effect.IsAttached) {
					System.Diagnostics.Debug.WriteLine("Not Attached");	
				}
			}
		}

		internal Listener Listener {
			get { return _listener; }
		}


		/// <summary>
		/// Identifies the PageOverlayColor bindable property.
		/// </summary>
		/// <remarks>To be added.</remarks>
		public static readonly BindableProperty PageOverlayColorProperty = BindableProperty.Create("PageOverlayColor", typeof(Color), typeof(PopupBase), Color.FromRgba(128,128,128,128));
		/// <summary>
		/// Gets or sets the color of the page overlay.
		/// </summary>
		/// <value>The color of the page overlay.</value>
		public Color PageOverlayColor {
			get { return (Color) GetValue (PageOverlayColorProperty);}
			set { SetValue (PageOverlayColorProperty, value); }
		}

		/// <summary>
		/// Cancel the Popup when the PageOverlay is touched
		/// </summary>
		public static readonly BindableProperty CancelOnPageOverlayTouchProperty = BindableProperty.Create("CancelOnPageOverlayTouch", typeof(bool), typeof(PopupBase), true);
		/// <summary>
		/// Gets or sets a value indicating whether Popup <see cref="T:Forms9Patch.PopupBase"/> will cancel on page overlay touch.
		/// </summary>
		/// <value><c>true</c> if cancel on page overlay touch; otherwise, <c>false</c>.</value>
		public bool CancelOnPageOverlayTouch
		{
			get { return (bool)GetValue(CancelOnPageOverlayTouchProperty); }
			set { SetValue(CancelOnPageOverlayTouchProperty, value); }
		}

		#endregion


		#region IRoundedBox Properties
		/// <summary>
		/// Backing store for the HasShadow bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static readonly BindableProperty HasShadowProperty =  BindableProperty.Create ("HasShadow", typeof(bool), typeof(PopupBase), true); 
		/// <summary>
		/// Gets or sets a flag indicating if the AbsoluteLayout has a shadow displayed. This is a bindable property.
		/// </summary>
		/// <value><c>true</c> if this instance has shadow; otherwise, <c>false</c>.</value>
		public bool HasShadow {
			get { return (bool)GetValue (HasShadowProperty); }
			set { SetValue (HasShadowProperty, value); }
		}

		/// <summary>
		/// The shadow inverted property backing store.
		/// </summary>
		public static readonly BindableProperty ShadowInvertedProperty = BindableProperty.Create("ShadowInverted", typeof(bool), typeof(PopupBase), (bool)RoundedBoxBase.ShadowInvertedProperty.DefaultValue);
		/// <summary>
		/// Gets or sets a flag indicating if the Frame's shadow is inverted. This is a bindable property.
		/// </summary>
		/// <value><c>true</c> if this instance's shadow is inverted; otherwise, <c>false</c>.</value>
		public bool ShadowInverted {
			get { return (bool)GetValue (ShadowInvertedProperty); }
			set { SetValue (ShadowInvertedProperty, value);}
		}

		/// <summary>
		/// The outline color property.
		/// </summary>
		public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create("OutlineColor", typeof(Color), typeof(PopupBase), (Color)RoundedBoxBase.OutlineColorProperty.DefaultValue);
		/// <summary>
		/// Gets or sets the color of the border of the AbsoluteLayout. This is a bindable property.
		/// </summary>
		/// <value>The color of the outline.</value>
		public Color OutlineColor {
			get { return (Color) GetValue(OutlineColorProperty); }
			set {  SetValue (OutlineColorProperty, value); }
		}

		/// <summary>
		/// The outline radius property.
		/// </summary>
		public static readonly BindableProperty OutlineRadiusProperty = BindableProperty.Create("OutlineRadius", typeof(float), typeof(PopupBase), (float)RoundedBoxBase.OutlineRadiusProperty.DefaultValue);
		/// <summary>
		/// Gets or sets the outline radius.
		/// </summary>
		/// <value>The outline radius.</value>
		public float OutlineRadius {
			get { return (float)GetValue (OutlineRadiusProperty); }
			set { SetValue (OutlineRadiusProperty, value); }
		}

		/// <summary>
		/// The outline width property.
		/// </summary>
		public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("OutlineWidth", typeof(float), typeof(PopupBase), (float)RoundedBoxBase.OutlineWidthProperty.DefaultValue);
		/// <summary>
		/// Gets or sets the width of the outline.
		/// </summary>
		/// <value>The width of the outline.</value>
		public float OutlineWidth {
			get { return (float)GetValue (OutlineWidthProperty); }
			set { SetValue (OutlineWidthProperty, value); }
		}

		/// <summary>
		/// Identifies the Padding bindable property.
		/// </summary>
		/// <remarks></remarks>
		public static new readonly BindableProperty PaddingProperty = BindableProperty.Create("Padding", typeof(Thickness), typeof(PopupBase), (Thickness)RoundedBoxBase.PaddingProperty.DefaultValue);
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
		public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof(Color), typeof(PopupBase), Color.White);
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

		#endregion



		#region Events
		/// <summary>
		/// Occurs when popup has been cancelled.
		/// </summary>
		public event EventHandler Cancelled;
		#endregion


		#region Fields
		//readonly Xamarin.Forms.AbsoluteLayout _layout = new Xamarin.Forms.AbsoluteLayout ();
		internal IRoundedBox _roundedBox;
		internal BoxView _pageOverlay;
		readonly Listener _listener;
		#endregion


		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.BubblePopup"/> class.
		/// </summary>
		/// <param name="host">Host.</param>
		internal PopupBase (Page host=null) {
			IsVisible = false;
			_pageOverlay = new BoxView {
				BackgroundColor = PageOverlayColor,
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Fill,
			};
			_listener = new Listener(_pageOverlay);
			_listener.Tapped += OnTapped;
			_listener.Panning += OnPanning;
			Host = host ?? Application.Current.MainPage;
			base.Children.Add(_pageOverlay);
		}
		#endregion


		#region Gesture event responders

		void OnTapped(object sender, FormsGestures.TapEventArgs e)
		{
			if (CancelOnPageOverlayTouch)
				Cancel();
		}

		void OnPanning(object sender, FormsGestures.PanEventArgs e)
		{
			if (CancelOnPageOverlayTouch)
				Cancel();
		}

		#endregion


		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">Disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_listener.Tapped -= OnTapped;
					_listener.Panning -= OnPanning;
					_listener.Dispose();
					disposedValue = true;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~PopupBase() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		/// <summary>
		/// Releases all resource used by the <see cref="T:Forms9Patch.PopupBase"/> object.
		/// </summary>
		/// <remarks>Call <see cref="O:Forms9Patch.PopupBase.Dispose"/> when you are finished using the <see cref="T:Forms9Patch.PopupBase"/>. The
		/// <see cref="O:Forms9Patch.PopupBase.Dispose"/> method leaves the <see cref="T:Forms9Patch.PopupBase"/> in an unusable state. After calling
		/// <see cref="O:Forms9Patch.PopupBase.Dispose"/>, you must release all references to the <see cref="T:Forms9Patch.PopupBase"/> so the garbage
		/// collector can reclaim the memory that the <see cref="T:Forms9Patch.PopupBase"/> was occupying.</remarks>
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion


		/// <summary>
		/// Cancel the display of this Popup (will fire Cancelled event);
		/// </summary>
		public void Cancel() {
			IsVisible = false;
			Cancelled?.Invoke (this, EventArgs.Empty);
		}

		internal void SetRoundedBoxBindings(BindableObject bindable) {
			//HasShadow drives UpdateBasePadding - which is different in each inherited implementation.  So binding needs to happen in inherited class
			bindable.SetBinding (RoundedBoxBase.ShadowInvertedProperty, "ShadowInverted");
			bindable.SetBinding (RoundedBoxBase.OutlineColorProperty, "OutlineColor");
			bindable.SetBinding (RoundedBoxBase.OutlineRadiusProperty, "OutlineRadius");
			bindable.SetBinding (RoundedBoxBase.OutlineWidthProperty, "OutlineWidth");
			//Padding drives UpdateBasePadding - which is different in each inherited implementation.  So binding needs to happen in inherited class
			bindable.SetBinding (VisualElement.BackgroundColorProperty, "BackgroundColor");
		}

		internal BoxView PageOverlay {
			get { return _pageOverlay; }
		}

		internal View ContentView {
			get { return (View)_roundedBox; }
			set {
				_roundedBox = (IRoundedBox)value;
				if (base.Children.Count < 2)
					base.Children.Add ((View)_roundedBox);
				else
					base.Children [1] = (View)_roundedBox;
			}
		}


		/// <param name="propertyName">The name of the property that changed.</param>
		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		protected override void OnPropertyChanged (string propertyName = null) {
			base.OnPropertyChanged (propertyName);
			if (propertyName == PageOverlayColorProperty.PropertyName)
				//base.BackgroundColor = PageOverlayColor;
				_pageOverlay.BackgroundColor = PageOverlayColor;
		}

		internal Action ForceNativeLayout { get; set; }



	}
}

