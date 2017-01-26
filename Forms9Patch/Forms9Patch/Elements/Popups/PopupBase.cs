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
		public new IList<View> Children
		{
			get
			{
				throw new InvalidOperationException("Children property is not valid for Forms9Patch popups. Use Content property instead.");
			}
		}
		#endregion


		#region Overridden VisualElementProperties
		/// <summary>
		/// The margin property.
		/// </summary>
		public static readonly new BindableProperty MarginProperty = BindableProperty.Create("Margin", typeof(Thickness), typeof(PopupBase), default(Thickness));
		/// <summary>
		/// Gets or sets the margin.
		/// </summary>
		/// <value>The margin.</value>
		public new Thickness Margin
		{
			get { return (Thickness)GetValue(MarginProperty); }
			set { SetValue(MarginProperty, value); }
		}

		/// <summary>
		/// The horizontal options property backing store.
		/// </summary>
		public static readonly new BindableProperty HorizontalOptionsProperty = BindableProperty.Create("HorizontalOptions", typeof(LayoutOptions), typeof(PopupBase), default(LayoutOptions));
		/// <summary>
		/// Gets or sets the horizontal options.
		/// </summary>
		/// <value>The horizontal options.</value>
		public new LayoutOptions HorizontalOptions
		{
			get { return (LayoutOptions)GetValue(HorizontalOptionsProperty); }
			set { SetValue(HorizontalOptionsProperty, value); }
		}

		/// <summary>
		/// The vertical options property.
		/// </summary>
		public static readonly new BindableProperty VerticalOptionsProperty = BindableProperty.Create("VerticalOptions", typeof(LayoutOptions), typeof(PopupBase), default(LayoutOptions));
		/// <summary>
		/// Gets or sets the vertical options.
		/// </summary>
		/// <value>The vertical options.</value>
		public new LayoutOptions VerticalOptions
		{
			get { return (LayoutOptions)GetValue(VerticalOptionsProperty); }
			set { SetValue(VerticalOptionsProperty, value); }
		}

		#endregion


		#region Popup Properties

		/// <summary>
		/// The target property.
		/// </summary>
		public static readonly BindableProperty TargetProperty = BindableProperty.Create("Target", typeof(VisualElement), typeof(PopupBase), default(Element));
		/// <summary>
		/// Gets or sets the popup target (could be a Page or a VisualElement on a Page).
		/// </summary>
		/// <value>The target.</value>
		public VisualElement Target
		{
			get { return (VisualElement)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}


		internal Listener Listener
		{
			get { return _listener; }
		}


		/// <summary>
		/// Identifies the PageOverlayColor bindable property.
		/// </summary>
		/// <remarks>To be added.</remarks>
		public static readonly BindableProperty PageOverlayColorProperty = BindableProperty.Create("PageOverlayColor", typeof(Color), typeof(PopupBase), Color.FromRgba(128, 128, 128, 128));
		/// <summary>
		/// Gets or sets the color of the page overlay.
		/// </summary>
		/// <value>The color of the page overlay.</value>
		public Color PageOverlayColor
		{
			get { return (Color)GetValue(PageOverlayColorProperty); }
			set { SetValue(PageOverlayColorProperty, value); }
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
		public static readonly BindableProperty HasShadowProperty = BindableProperty.Create("HasShadow", typeof(bool), typeof(PopupBase), true);
		/// <summary>
		/// Gets or sets a flag indicating if the AbsoluteLayout has a shadow displayed. This is a bindable property.
		/// </summary>
		/// <value><c>true</c> if this instance has shadow; otherwise, <c>false</c>.</value>
		public bool HasShadow
		{
			get { return (bool)GetValue(HasShadowProperty); }
			set { SetValue(HasShadowProperty, value); }
		}

		/// <summary>
		/// The shadow inverted property backing store.
		/// </summary>
		public static readonly BindableProperty ShadowInvertedProperty = BindableProperty.Create("ShadowInverted", typeof(bool), typeof(PopupBase), (bool)RoundedBoxBase.ShadowInvertedProperty.DefaultValue);
		/// <summary>
		/// Gets or sets a flag indicating if the Frame's shadow is inverted. This is a bindable property.
		/// </summary>
		/// <value><c>true</c> if this instance's shadow is inverted; otherwise, <c>false</c>.</value>
		public bool ShadowInverted
		{
			get { return (bool)GetValue(ShadowInvertedProperty); }
			set { SetValue(ShadowInvertedProperty, value); }
		}

		/// <summary>
		/// The outline color property.
		/// </summary>
		public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create("OutlineColor", typeof(Color), typeof(PopupBase), (Color)RoundedBoxBase.OutlineColorProperty.DefaultValue);
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
		/// The outline radius property.
		/// </summary>
		public static readonly BindableProperty OutlineRadiusProperty = BindableProperty.Create("OutlineRadius", typeof(float), typeof(PopupBase), (float)RoundedBoxBase.OutlineRadiusProperty.DefaultValue);
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
		/// The outline width property.
		/// </summary>
		public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("OutlineWidth", typeof(float), typeof(PopupBase), (float)RoundedBoxBase.OutlineWidthProperty.DefaultValue);
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
		public static new readonly BindableProperty PaddingProperty = BindableProperty.Create("Padding", typeof(Thickness), typeof(PopupBase), (Thickness)RoundedBoxBase.PaddingProperty.DefaultValue);
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
		/// Identifies the BackgroundColor bindable property.
		/// </summary>
		/// <remarks>To be added.</remarks>
		public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof(Color), typeof(PopupBase), Color.White);
		/// <summary>
		/// Gets or sets the color which will fill the background of a VisualElement. This is a bindable property.
		/// </summary>
		/// <value>The color of the background.</value>
		public new Color BackgroundColor
		{
			get { return (Color)GetValue(BackgroundColorProperty); }
			set
			{
				SetValue(BackgroundColorProperty, value);
			}
		}

		/// <summary>
		/// The is elliptical property.
		/// </summary>
		public static readonly BindableProperty IsEllipticalProperty = BindableProperty.Create("IsElliptical", typeof(bool), typeof(PopupBase), default(bool));
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.PopupBase"/> is elliptical rathen than rectangular.
		/// </summary>
		/// <value><c>true</c> if is elliptical; otherwise, <c>false</c>.</value>
		public bool IsElliptical
		{
			get { return (bool)GetValue(IsEllipticalProperty); }
			set { SetValue(IsEllipticalProperty, value); }
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
		internal DateTime PresentedAt;
		#endregion

		RootPage RootPage { get { return Application.Current.MainPage as RootPage; } }

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="BubblePopup"/> class.
		/// </summary>
		/// <param name="target">Page or Element on Page in which Popup will be presented.</param>
		internal PopupBase(VisualElement target=null)
		{
			
			IsVisible = false;
			_pageOverlay = new BoxView
			{
				BackgroundColor = PageOverlayColor,
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Fill
			};
			_listener = new Listener(_pageOverlay);
			_listener.Tapped += OnTapped;
			_listener.Panning += OnPanning;
			//HostPage = host ?? Application.Current.MainPage;
			Target = target;
			base.Children.Add(_pageOverlay);
		}
		#endregion


		#region Gesture event responders

		void OnTapped(object sender, TapEventArgs e)
		{
			if (CancelOnPageOverlayTouch)
				Cancel();
		}

		void OnPanning(object sender, PanEventArgs e)
		{
			if (CancelOnPageOverlayTouch)
				Cancel();
		}

		#endregion


		#region IDisposable Support
		bool disposedValue; // To detect redundant calls

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
			}
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:Forms9Patch.PopupBase"/> object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}
		#endregion


		/// <summary>
		/// Cancel the display of this Popup (will fire Cancelled event);
		/// </summary>
		public void Cancel()
		{
			IsVisible = false;
			Cancelled?.Invoke(this, EventArgs.Empty);
		}

		internal BoxView PageOverlay
		{
			get { return _pageOverlay; }
		}

		internal View ContentView
		{
			get { return (View)_roundedBox; }
			set
			{
				_roundedBox = (IRoundedBox)value;
				if (base.Children.Count < 2)
					base.Children.Add((View)_roundedBox);
				else
					base.Children[1] = (View)_roundedBox;

			}
		}


		/// <param name="propertyName">The name of the property that changed.</param>
		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == PageOverlayColorProperty.PropertyName)
				_pageOverlay.BackgroundColor = PageOverlayColor;
			else if (propertyName == IsVisibleProperty.PropertyName)
			{
				if (IsVisible)
				{
					ContentView.TranslationX = 0;
					ContentView.TranslationY = 0;
					if (RootPage == null)
						throw new NotSupportedException("Forms9Patch popup elements require the Application's MainPage property to be set to a Forms9Patch.RootPage instance");
					RootPage.AddPopup(this);
				}
				else
					RootPage?.RemovePopup(this);
			}
			else if (propertyName == PaddingProperty.PropertyName)
				_roundedBox.Padding = Padding;
			else if (propertyName == HasShadowProperty.PropertyName)
				_roundedBox.HasShadow = HasShadow;
			else if (propertyName == OutlineColorProperty.PropertyName)
				_roundedBox.OutlineColor = OutlineColor;
			else if (propertyName == OutlineWidthProperty.PropertyName)
				_roundedBox.OutlineWidth = OutlineWidth;
			else if (propertyName == OutlineRadiusProperty.PropertyName)
				_roundedBox.OutlineRadius = OutlineRadius;
			else if (propertyName == BackgroundColorProperty.PropertyName)
				_roundedBox.BackgroundColor = BackgroundColor;
			else if (propertyName == ShadowInvertedProperty.PropertyName)
				_roundedBox.ShadowInverted = ShadowInverted;
		}


		/*
		internal Action ForceNativeLayout { get; set; }

		public void Relayout()
		{
			ForceNativeLayout?.Invoke();
		}
		*/

		/// <summary>
		/// Layouts the children.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			if (width > 0 && height > 0)
			{
				LayoutChildIntoBoundingRegion(PageOverlay, new Rectangle(x,y,width,height));
			}
			else
				ContentView.IsVisible = false;
		}
	}
}

