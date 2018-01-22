using Xamarin.Forms;
using System;
using FormsGestures;
using System.Collections.Generic;
using Forms9Patch;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Popup base.
    /// </summary>
    [ContentProperty("Content")]
    public abstract class PopupBase : Layout<View>, IDisposable, IPopup //Xamarin.Forms.Layout<View>, IShape

    {
        #region Invalid Parent Properties
        /// <summary>
        /// Invalid Property, do not use
        /// </summary>
        /// <value>Invalid</value>
        /// <remarks>Do not use</remarks>
        [Obsolete("Use Content")]
        public new IList<View> Children
        {
            get
            {
                throw new InvalidOperationException("Children property is not valid for Forms9Patch popups. Use Content property instead.");
            }
        }
        #endregion


        #region IPopup

        #region Padding  // IMPORTANT: Need to override Xamarin.Forms.Layout.Padding property in order to correctly compute & store shadow padding
        /// <summary>
        /// override Xamarin.Forms.Layout.Padding property backing store in order to correctly compute and store shadow padding
        /// </summary>
        public static new BindableProperty PaddingProperty = BindableProperty.Create("Padding", typeof(Thickness), typeof(PopupBase), default(Thickness));
        /// <summary>
        /// Gets or sets the inner padding of the Layout.
        /// </summary>
        /// <value>The Thickness values for the layout. The default value is a Thickness with all values set to 0.</value>
        public new Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        #endregion Padding

        #region IsVisible
        /// <summary>
        /// The is visible property.
        /// </summary>
        public static new readonly BindableProperty IsVisibleProperty = BindableProperty.Create("PobIsVisible", typeof(bool), typeof(PopupBase), default(bool));
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.PopupBase"/> is visible.
        /// </summary>
        /// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
        public new bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }
        #endregion IsVisible

        #region Margin
        /// <summary>
        /// The margin property.
        /// </summary>
        public static readonly new BindableProperty MarginProperty = BindableProperty.Create("PobMargin", typeof(Thickness), typeof(PopupBase), default(Thickness));
        /// <summary>
        /// Gets or sets the margin.
        /// </summary>
        /// <value>The margin.</value>
        public new Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }
        #endregion Margin

        #region HorizontalOptions
        /// <summary>
        /// The horizontal options property backing store.
        /// </summary>
        public static readonly new BindableProperty HorizontalOptionsProperty = BindableProperty.Create("PobHorizontalOptions", typeof(LayoutOptions), typeof(PopupBase), default(LayoutOptions));
        /// <summary>
        /// Gets or sets the horizontal options.
        /// </summary>
        /// <value>The horizontal options.</value>
        public new LayoutOptions HorizontalOptions
        {
            get { return (LayoutOptions)GetValue(HorizontalOptionsProperty); }
            set { SetValue(HorizontalOptionsProperty, value); }
        }

        #endregion HorizontalOptions

        #region VerticalOptions
        /// <summary>
        /// The vertical options property.
        /// </summary>
        public static readonly new BindableProperty VerticalOptionsProperty = BindableProperty.Create("PobVerticalOptions", typeof(LayoutOptions), typeof(PopupBase), default(LayoutOptions));
        /// <summary>
        /// Gets or sets the vertical options.
        /// </summary>
        /// <value>The vertical options.</value>
        public new LayoutOptions VerticalOptions
        {
            get { return (LayoutOptions)GetValue(VerticalOptionsProperty); }
            set { SetValue(VerticalOptionsProperty, value); }
        }
        #endregion VerticalOptions

        #region Target
        /// <summary>
        /// The target property.
        /// </summary>
        public static readonly BindableProperty TargetProperty = BindableProperty.Create("PobTarget", typeof(VisualElement), typeof(PopupBase), default(Element));
        /// <summary>
        /// Gets or sets the popup target (could be a Page or a VisualElement on a Page).
        /// </summary>
        /// <value>The target.</value>
        public VisualElement Target
        {
            get { return (VisualElement)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
        #endregion Target

        #region PageOverlayColor
        /// <summary>
        /// Identifies the PageOverlayColor bindable property.
        /// </summary>
        /// <remarks>To be added.</remarks>
        public static readonly BindableProperty PageOverlayColorProperty = BindableProperty.Create("PobPageOverlayColor", typeof(Color), typeof(PopupBase), Color.FromRgba(128, 128, 128, 128));
        /// <summary>
        /// Gets or sets the color of the page overlay.
        /// </summary>
        /// <value>The color of the page overlay.</value>
        public Color PageOverlayColor
        {
            get { return (Color)GetValue(PageOverlayColorProperty); }
            set { SetValue(PageOverlayColorProperty, value); }
        }
        #endregion PageOverlayColor

        #region CancelOnPageOverlayTouch
        /// <summary>
        /// Cancel the Popup when the PageOverlay is touched
        /// </summary>
        public static readonly BindableProperty CancelOnPageOverlayTouchProperty = BindableProperty.Create("PobCancelOnPageOverlayTouch", typeof(bool), typeof(PopupBase), true);
        /// <summary>
        /// Gets or sets a value indicating whether Popup <see cref="T:Forms9Patch.PopupBase"/> will cancel on page overlay touch.
        /// </summary>
        /// <value><c>true</c> if cancel on page overlay touch; otherwise, <c>false</c>.</value>
        public bool CancelOnPageOverlayTouch
        {
            get { return (bool)GetValue(CancelOnPageOverlayTouchProperty); }
            set { SetValue(CancelOnPageOverlayTouchProperty, value); }
        }
        #endregion CancelOnPageOverlayTouch

        #region Retain
        /// <summary>
        /// The retain property.
        /// </summary>
        public static readonly BindableProperty RetainProperty = BindableProperty.Create("Retain", typeof(bool), typeof(PopupBase), default(bool));
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.PopupBase"/> is retained after it is hidden.
        /// </summary>
        /// <value><c>true</c> if retain; otherwise, <c>false</c>.</value>
        public bool Retain
        {
            get { return (bool)GetValue(RetainProperty); }
            set { SetValue(RetainProperty, value); }
        }
        #endregion Retail

        #region IBackground

        #region BackgroundImage
        /// <summary>
        /// Backing store for the BackgroundImage bindable property.
        /// </summary>
        public static BindableProperty BackgroundImageProperty = ShapeBase.BackgroundImageProperty;
        /// <summary>
        /// Gets or sets the background image.
        /// </summary>
        /// <value>The background image.</value>
        public Image BackgroundImage
        {
            get { return (Image)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }
        #endregion BackgroundImage

        #region IShape

        #region BackgroundColor property
        /// <summary>
        /// backing store for BackgroundColor property
        /// </summary>
        public static new readonly BindableProperty BackgroundColorProperty = ShapeBase.BackgroundColorProperty;
        /// <summary>
        /// Gets/Sets the BackgroundColor property
        /// </summary>
        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
        #endregion BackgroundColor property

        #region HasShadow property
#if _Forms9Patch_Frame_
        /// <summary>
        /// override Xamarin.Forms.Frame.HasShadow property backing store in order to correctly compute & store shadow padding
        /// </summary>
        public static new BindableProperty HasShadowProperty = ShapeBase.HasShadowProperty;
        /// <summary>
        /// Gets/Sets the HasShadow property
        /// </summary>
        public new bool HasShadow
#else
        /// <summary>
        /// HasShadow property backing store
        /// </summary>
        public static BindableProperty HasShadowProperty = ShapeBase.HasShadowProperty;
        /// <summary>
        /// Gets/Sets the HasShadow property
        /// </summary>
        public bool HasShadow
#endif
        {
            get { return (bool)GetValue(HasShadowProperty); }
            set { SetValue(HasShadowProperty, value); }
        }
        #endregion HasShadow property

        #region ShadowInverted
        /// <summary>
        /// Backing store for the ShadowInverted bindable property.
        /// </summary>
        /// <remarks></remarks>
        public static readonly BindableProperty ShadowInvertedProperty = ShapeBase.ShadowInvertedProperty;
        /// <summary>
        /// Gets or sets a flag indicating if the layout's shadow is inverted. This is a bindable property.
        /// </summary>
        /// <value><c>true</c> if this instance's shadow is inverted; otherwise, <c>false</c>.</value>
        public bool ShadowInverted
        {
            get { return (bool)GetValue(ShadowInvertedProperty); }
            set { SetValue(ShadowInvertedProperty, value); }
        }
        #endregion ShadowInverted

        #region OutlineColor property

#if _Forms9Patch_Frame_
        // OutlineColor inherited from Xamarin.Forms.AbsoluteLayout
#else
        /// <summary>
        /// backing store for OutlineColor property
        /// </summary>
        public static readonly BindableProperty OutlineColorProperty = ShapeBase.OutlineColorProperty;
        /// <summary>
        /// Gets/Sets the OutlineColor property
        /// </summary>
        public Color OutlineColor
        {
            get { return (Color)GetValue(OutlineColorProperty); }
            set { SetValue(OutlineColorProperty, value); }
        }
#endif

        #endregion OutlineColor property

        #region OutlineRadius
        /// <summary>
        /// Backing store for the OutlineRadius bindable property.
        /// </summary>
        public static readonly BindableProperty OutlineRadiusProperty = ShapeBase.OutlineRadiusProperty;
        /// <summary>
        /// Gets or sets the outline radius.
        /// </summary>
        /// <value>The outline radius.</value>
        public float OutlineRadius
        {
            get { return (float)GetValue(OutlineRadiusProperty); }
            set { SetValue(OutlineRadiusProperty, value); }
        }
        #endregion OutlineRadius

        #region OutlineWidth
        /// <summary>
        /// Backing store for the OutlineWidth bindable property.
        /// </summary>
        public static readonly BindableProperty OutlineWidthProperty = ShapeBase.OutlineWidthProperty;
        /// <summary>
        /// Gets or sets the width of the outline.
        /// </summary>
        /// <value>The width of the outline.</value>
        public float OutlineWidth
        {
            get { return (float)GetValue(OutlineWidthProperty); }
            set { SetValue(OutlineWidthProperty, value); }
        }
        #endregion OutlineWidth

        #region ElementShape property
        /// <summary>
        /// Backing store for the ElementShape property
        /// </summary>
        public static readonly BindableProperty ElementShapeProperty = ShapeBase.ElementShapeProperty;
        /// <summary>
        /// Gets/sets the geometry of the element
        /// </summary>
        public ElementShape ElementShape
        {
            get { return (ElementShape)GetValue(ElementShapeProperty); }
            set { SetValue(ElementShapeProperty, value); }
        }
        #endregion ElementShape property

        #region ExtendedElementShape property
        /// <summary>
        /// backing store for ExtendedElementShape property
        /// </summary>
        internal static readonly BindableProperty ExtendedElementShapeProperty = ShapeBase.ExtendedElementShapeProperty;
        /// <summary>
        /// Gets/Sets the ExtendedElementShape property
        /// </summary>
        ExtendedElementShape IShape.ExtendedElementShape
        {
            get { return (ExtendedElementShape)GetValue(ExtendedElementShapeProperty); }
            set { SetValue(ExtendedElementShapeProperty, value); }
        }
        #endregion ExtendedElementShape property

        #region IElement

        #region InstanceId
        /// <summary>
        /// The Instance Id (for debugging purposes)
        /// </summary>
        public int InstanceId => _id;
        #endregion InstanceId

        #endregion IElement


        #endregion IShape

        #endregion IBackground

        #endregion IPopup


        #region Internal Properties

        internal Listener Listener { get { return _listener; } }

        internal RootPage RootPage { get { return Application.Current.MainPage as RootPage; } }

        internal BoxView PageOverlay { get { return _pageOverlay; } }

        #region ContentView property
        internal View ContentView
        {
            get { return (View)_modalLayout; }
            set
            {
                if (_modalLayout is VisualElement oldLayout)
                    oldLayout.PropertyChanged -= OnContentViewPropertyChanged;
                _modalLayout = (ILayout)value;
                if (_modalLayout is VisualElement newLayout)
                    newLayout.PropertyChanged += OnContentViewPropertyChanged;
                if (base.Children.Count < 2)
                    base.Children.Add((View)_modalLayout);
                else
                    base.Children[1] = (View)_modalLayout;
                _modalLayout.IgnoreChildren = false;

            }
        }
        #endregion ContentView property

        #endregion Internal Properties


        #region Events
        /// <summary>
        /// Occurs when popup has been cancelled.
        /// </summary>
        public event EventHandler Cancelled;
        #endregion


        #region Fields
        internal ILayout _modalLayout;
        internal BoxView _pageOverlay;
        readonly Listener _listener;
        internal DateTime PresentedAt;
        static int _instances;
        int _id;
        #endregion


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.PopupBase"/> class.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="retain">If set to <c>true</c> retain.</param>
        internal PopupBase(VisualElement target = null, bool retain = false)
        {
            Settings.ConfirmInitialization();

            BackgroundColor = Color.White;
            HasShadow = true;

            _id = _instances++;
            Retain = retain;
            IsVisible = false;
            _pageOverlay = new BoxView
            {
                BackgroundColor = PageOverlayColor,
            };
            _listener = Listener.For(_pageOverlay);
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
                    Retain = false;
                    RootPage.RemovePopup(this);
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


        #region Public Methods
        /// <summary>
        /// Cancel the display of this Popup (will fire Cancelled event);
        /// </summary>
        public void Cancel()
        {
            IsVisible = false;
            Cancelled?.Invoke(this, EventArgs.Empty);
        }
        #endregion


        #region PropertyChange managment

        void InitializeILayoutProperties(ILayout layout)
        {
            #region IBackground

            layout.BackgroundImage = BackgroundImage;

            #region IShape

            layout.BackgroundColor = (BackgroundColor == Color.Default || BackgroundColor == default(Color) ? Color.White : BackgroundColor);
            layout.HasShadow = HasShadow;
            layout.ShadowInverted = ShadowInverted;
            layout.OutlineColor = OutlineColor;
            layout.OutlineRadius = OutlineRadius;
            layout.OutlineWidth = OutlineWidth;
            layout.ElementShape = ElementShape;
            layout.ExtendedElementShape = ((ILayout)this).ExtendedElementShape;

            #endregion IShape

            #endregion IBackground

            #region ILayout

            layout.Padding = Padding;
            layout.IgnoreChildren = false;

            #endregion ILayout

        }

        private void OnContentViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (RootPage != null && e.PropertyName == Xamarin.Forms.Layout.PaddingProperty.PropertyName)
            {
                LayoutChildren(RootPage.X, RootPage.Y, RootPage.Bounds.Size.Width, RootPage.Bounds.Height);
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
                    if (Application.Current.MainPage == null)
                    {
                        IsVisible = false;
                        return;
                    }
                    if (RootPage == null)
                        throw new NotSupportedException("Forms9Patch popup elements require the Application's MainPage property to be set to a Forms9Patch.RootPage instance");
                    RootPage?.AddPopup(this);
                    base.IsVisible = true;
                }
                else
                {
                    base.IsVisible = false;
                    RootPage?.RemovePopup(this);
                }
            }
            else if (propertyName == RetainProperty.PropertyName && !Retain)
                Dispose();


            if (_modalLayout != null)
            {
                #region ILayout
                if (propertyName == PaddingProperty.PropertyName)
                    _modalLayout.Padding = Padding;
                #endregion

                #region IBackground
                else if (propertyName == BackgroundImageProperty.PropertyName)
                    _modalLayout.BackgroundImage = BackgroundImage;

                #region IShape
                else if (propertyName == BackgroundColorProperty.PropertyName)
                    _modalLayout.BackgroundColor = (BackgroundColor == Color.Default || BackgroundColor == default(Color) ? Color.White : BackgroundColor);
                else if (propertyName == HasShadowProperty.PropertyName)
                    _modalLayout.HasShadow = HasShadow;
                else if (propertyName == ShadowInvertedProperty.PropertyName)
                    _modalLayout.ShadowInverted = ShadowInverted;
                else if (propertyName == OutlineColorProperty.PropertyName)
                    _modalLayout.OutlineColor = OutlineColor;
                else if (propertyName == OutlineWidthProperty.PropertyName)
                    _modalLayout.OutlineWidth = OutlineWidth;
                else if (propertyName == OutlineRadiusProperty.PropertyName)
                    _modalLayout.OutlineRadius = OutlineRadius;
                else if (propertyName == ElementShapeProperty.PropertyName)
                    _modalLayout.ElementShape = ElementShape;
                else if (propertyName == ExtendedElementShapeProperty.PropertyName)
                    _modalLayout.ExtendedElementShape = ((IShape)this).ExtendedElementShape;
                #endregion IShape

                #endregion IBackground
            }


        }
        #endregion


        #region Layout
        Thickness IShape.ShadowPadding() => ShapeBase.ShadowPadding(this, HasShadow);

        /// <summary>
        /// processes measurement requests
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            // this is going to be the size of the root page.
            var result = new SizeRequest(RootPage.Bounds.Size, RootPage.Bounds.Size);
            return result;
        }

        internal void ManualLayout(Rectangle bounds)
        {
            LayoutChildren(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }

        /// <summary>
        /// Layouts the children.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            //System.Diagnostics.Debug.WriteLine("{0}[{1}] x,y,w,h=[" + x + "," + y + "," + width + "," + height + "]", P42.Utils.ReflectionExtensions.CallerString(), GetType());

            var targetPage = Application.Current.MainPage;
            var hostingPage = this.HostingPage();
            foreach (var page in Application.Current.MainPage.Navigation.ModalStack)
            {
                if (page == hostingPage)
                {
                    targetPage = hostingPage;
                    break;
                }
            }



            if (width > 0 && height > 0)
                LayoutChildIntoBoundingRegion(PageOverlay, new Rectangle(-targetPage.Padding.Left, -targetPage.Padding.Top, targetPage.Width, targetPage.Height));
            else
                ContentView.IsVisible = false;
        }
        #endregion
    }
}

