using Xamarin.Forms;
using System;
using FormsGestures;
using System.Collections.Generic;
using Forms9Patch;
using System.ComponentModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;
using System.Linq;

namespace Forms9Patch
{

    /// <summary>
    /// Forms9Patch Popup base.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentProperty("ContentView")]
    public abstract class PopupBase : Rg.Plugins.Popup.Pages.PopupPage,  /* Xamarin.Forms.Layout<View>,*/ IDisposable, IPopup //Xamarin.Forms.Layout<View>, IShape

    {
        #region IPopup

        #region Padding  // IMPORTANT: Need to override Xamarin.Forms.Layout.Padding property in order to correctly compute & store shadow padding
        /// <summary>
        /// override Xamarin.Forms.Layout.Padding property backing store in order to correctly compute and store shadow padding
        /// </summary>
        public static new BindableProperty PaddingProperty = BindableProperty.Create("Forms9Patch.PopupBase.Padding", typeof(Thickness), typeof(PopupBase), default(Thickness));
        /// <summary>
        /// Gets or sets the inner padding of the Layout.
        /// </summary>
        /// <value>The Thickness values for the layout. The default value is a Thickness with all values set to 0.</value>
        public new Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
        #endregion Padding

        #region IsVisible
        /// <summary>
        /// The is visible property.
        /// </summary>
        public static new readonly BindableProperty IsVisibleProperty = BindableProperty.Create("Forms9Patch.PopupBase.IsVisible", typeof(bool), typeof(PopupBase), default(bool));
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.PopupBase"/> is visible.
        /// </summary>
        /// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
        public new bool IsVisible
        {
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }
        #endregion IsVisible

        #region Margin
        /// <summary>
        /// The margin property.
        /// </summary>
        public static readonly BindableProperty MarginProperty = BindableProperty.Create("Forms9Patch.PopupBase.Margin", typeof(Thickness), typeof(PopupBase), new Thickness(30));
        /// <summary>
        /// Gets or sets the margin.
        /// </summary>
        /// <value>The margin.</value>
        public Thickness Margin
        {
            get => (Thickness)GetValue(MarginProperty);
            set => SetValue(MarginProperty, value);
        }
        #endregion Margin

        #region HorizontalOptions
        /// <summary>
        /// The horizontal options property backing store.
        /// </summary>
        public static readonly BindableProperty HorizontalOptionsProperty = BindableProperty.Create("Forms9Patch.PopupBase.HorizontalOptions", typeof(LayoutOptions), typeof(PopupBase), default(LayoutOptions));
        /// <summary>
        /// Gets or sets the horizontal options.
        /// </summary>
        /// <value>The horizontal options.</value>
        public LayoutOptions HorizontalOptions
        {
            get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
            set => SetValue(HorizontalOptionsProperty, value);
        }

        #endregion HorizontalOptions

        #region VerticalOptions
        /// <summary>
        /// The vertical options property.
        /// </summary>
        public static readonly BindableProperty VerticalOptionsProperty = BindableProperty.Create("Forms9Patch.PopupBase.VerticalOptions", typeof(LayoutOptions), typeof(PopupBase), default(LayoutOptions));
        /// <summary>
        /// Gets or sets the vertical options.
        /// </summary>
        /// <value>The vertical options.</value>
        public LayoutOptions VerticalOptions
        {
            get => (LayoutOptions)GetValue(VerticalOptionsProperty);
            set => SetValue(VerticalOptionsProperty, value);
        }
        #endregion VerticalOptions

        #region Target
        /// <summary>
        /// The target property.
        /// </summary>
        public static readonly BindableProperty TargetProperty = BindableProperty.Create("Forms9Patch.PopupBase.Target", typeof(VisualElement), typeof(PopupBase), default(Element));
        /// <summary>
        /// Gets or sets the popup target (could be a Page or a VisualElement on a Page).
        /// </summary>
        /// <value>The target.</value>
        public VisualElement Target
        {
            get => (VisualElement)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }
        #endregion Target

        #region PageOverlayColor
        /// <summary>
        /// Identifies the PageOverlayColor bindable property.
        /// </summary>
        /// <remarks>To be added.</remarks>
        public static readonly BindableProperty PageOverlayColorProperty = BindableProperty.Create("Forms9Patch.PopupBase.PageOverlayColor", typeof(Color), typeof(PopupBase), Color.FromRgba(128, 128, 128, 128));
        /// <summary>
        /// Gets or sets the color of the page overlay.
        /// </summary>
        /// <value>The color of the page overlay.</value>
        public Color PageOverlayColor
        {
            get => (Color)GetValue(PageOverlayColorProperty);
            set => SetValue(PageOverlayColorProperty, value);
        }
        #endregion PageOverlayColor

        #region CancelOnPageOverlayTouch
        /// <summary>
        /// Cancel the Popup when the PageOverlay is touched
        /// </summary>
        public static readonly BindableProperty CancelOnPageOverlayTouchProperty = BindableProperty.Create("Forms9Patch.PopupBase.CancelOnPageOverlayTouch", typeof(bool), typeof(PopupBase), true);
        /// <summary>
        /// Gets or sets a value indicating whether Popup <see cref="T:Forms9Patch.PopupBase"/> will cancel on page overlay touch.
        /// </summary>
        /// <value><c>true</c> if cancel on page overlay touch; otherwise, <c>false</c>.</value>
        public bool CancelOnPageOverlayTouch
        {
            get => (bool)GetValue(CancelOnPageOverlayTouchProperty);
            set => SetValue(CancelOnPageOverlayTouchProperty, value);
        }
        #endregion CancelOnPageOverlayTouch

        #region CancelOnBackButtonClick
        /// <summary>
        /// Cancel the Popup when the back button is touched
        /// </summary>
        public static readonly BindableProperty CancelOnBackButtonClickProperty = BindableProperty.Create("Forms9Patch.PopupBase.CancelOnBackButtonClick", typeof(bool), typeof(PopupBase), true);
        /// <summary>
        /// Gets or sets a value indicating whether Popup <see cref="T:Forms9Patch.PopupBase"/> will cancel on the back button touch.
        /// </summary>
        /// <value><c>true</c> if cancel on back button touch; otherwise, <c>false</c>.</value>
        public bool CancelOnBackButtonClick
        {
            get => (bool)GetValue(CancelOnBackButtonClickProperty);
            set => SetValue(CancelOnBackButtonClickProperty, value);
        }
        #endregion CancelOnBackButtonClick

        #region Retain
        /// <summary>
        /// The retain property.
        /// </summary>
        public static readonly BindableProperty RetainProperty = BindableProperty.Create("Forms9Patch.PopupBase.Retain", typeof(bool), typeof(PopupBase), default(bool));
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.PopupBase"/> is retained after it is hidden.
        /// </summary>
        /// <value><c>true</c> if retain; otherwise, <c>false</c>.</value>
        public bool Retain
        {
            get => (bool)GetValue(RetainProperty);
            set => SetValue(RetainProperty, value);
        }
        #endregion Retail

        #region IBackground

        #region BackgroundImage
        /// <summary>
        /// Backing store for the BackgroundImage bindable property.
        /// </summary>
        public static new BindableProperty BackgroundImageProperty = ShapeBase.BackgroundImageProperty;
        /// <summary>
        /// Gets or sets the background image.
        /// </summary>
        /// <value>The background image.</value>
        public new Image BackgroundImage
        {
            get => (Forms9Patch.Image)GetValue(BackgroundImageProperty);
            set => SetValue(BackgroundImageProperty, value);
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
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        #endregion BackgroundColor property

        #region HasShadow property
        /// <summary>
        /// HasShadow property backing store
        /// </summary>
        public static BindableProperty HasShadowProperty = ShapeBase.HasShadowProperty;
        /// <summary>
        /// Gets/Sets the HasShadow property
        /// </summary>
        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
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
            get => (bool)GetValue(ShadowInvertedProperty);
            set => SetValue(ShadowInvertedProperty, value);
        }
        #endregion ShadowInverted

        #region OutlineColor property

        /// <summary>
        /// backing store for OutlineColor property
        /// </summary>
        public static readonly BindableProperty OutlineColorProperty = ShapeBase.OutlineColorProperty;
        /// <summary>
        /// Gets/Sets the OutlineColor property
        /// </summary>
        public Color OutlineColor
        {
            get => (Color)GetValue(OutlineColorProperty);
            set => SetValue(OutlineColorProperty, value);
        }

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
            get => (float)GetValue(OutlineRadiusProperty);
            set => SetValue(OutlineRadiusProperty, value);
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
            get => (float)GetValue(OutlineWidthProperty);
            set => SetValue(OutlineWidthProperty, value);
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
            get => (ElementShape)GetValue(ElementShapeProperty);
            set => SetValue(ElementShapeProperty, value);
        }
        #endregion ElementShape property

        /*
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
            get => (ExtendedElementShape)GetValue(ExtendedElementShapeProperty);
            set => SetValue(ExtendedElementShapeProperty, value);
        }
        #endregion ExtendedElementShape property
        */

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


        #region ContentView property
        /// <summary>
        /// What is the decorative container view for the popup (BubbleLayout, Frame)?
        /// </summary>
        internal protected View DecorativeContainerView
        {
            get => (View)_decorativeContainerView;
            set
            {
                if (_decorativeContainerView is VisualElement oldLayout)
                    oldLayout.PropertyChanged -= OnContentViewPropertyChanged;
                _decorativeContainerView = (ILayout)value;
                if (_decorativeContainerView is VisualElement newLayout)
                    newLayout.PropertyChanged += OnContentViewPropertyChanged;
                base.Content = _decorativeContainerView as View;
                _decorativeContainerView.IgnoreChildren = false;

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
        internal ILayout _decorativeContainerView;
        //internal BoxView _pageOverlay;
        //readonly Listener _listener;
        internal DateTime PresentedAt;
        static int _instances;
        readonly int _id;
        /// <summary>
        /// Say, when was the last time I ...
        /// </summary>
        protected DateTime _lastLayout = DateTime.MinValue;

        #endregion


        #region Constructor
        static PopupBase()
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.PopupBase"/> class.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="retain">If set to <c>true</c> retain.</param>
        internal PopupBase(VisualElement target = null, bool retain = false)
        {
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;

            CloseWhenBackgroundIsClicked = CancelOnPageOverlayTouch;
            BackgroundColor = Color.White;

            Padding = 10;
            HasShadow = true;
            OutlineRadius = 5;

            _id = _instances++;
            Retain = retain;
            IsVisible = false;
            Target = target;

            KeyboardService.HeightChanged += OnKeyboardHeightChanged;
        }

        #endregion


        #region Cancelation
        /// <summary>
        /// Called when back button is pressed
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            if (CancelOnBackButtonClick)
                Cancel();
            return CancelOnBackButtonClick;
        }

        /// <summary>
        /// Called when background is clicked;
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackgroundClicked()
        {
            var isClose = base.OnBackgroundClicked();
            if (isClose)
                Cancel();
            return isClose;
        }


        public async Task CancelAsync()
        {
            if (P42.Utils.Environment.IsOnMainThread)
            {
                await Pop();
                //await Navigation.RemovePopupPageAsync(this);
                //IsVisible = false;
                while (_isPushed)
                    await Task.Delay(50);
                Cancelled?.Invoke(this, EventArgs.Empty);
            }
            else
                Device.BeginInvokeOnMainThread(async () => await CancelAsync());
        }

        /// <summary>
        /// Cancel the display of this Popup (will fire Cancelled event);
        /// </summary>
        [Obsolete("Use CancelAsync instead")]
        public void Cancel() => Task.Run(async () => await CancelAsync());

        #endregion


        #region IDisposable Support
        bool _disposed; // To detect redundant calls

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    /*
                    if (IsVisible)

                        _listener.Tapped -= OnTapped;
                    _listener.Panning -= OnPanning;
                    _listener.Dispose();
                    */
                    Retain = false;
                    //PopupPage?.RemovePopup(this);
                    _disposed = true;
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


        #region PropertyChange managment
        const string KeyboardServiceHeight = "KeyboardService.Height";

        void OnKeyboardHeightChanged(object sender, double e) => OnContentViewPropertyChanged(sender, new PropertyChangedEventArgs(KeyboardServiceHeight));

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
            //layout.ExtendedElementShape = ((ILayout)this).ExtendedElementShape;

            #endregion IShape

            #endregion IBackground

            #region ILayout

            layout.Padding = Padding;
            layout.IgnoreChildren = false;

            #endregion ILayout

        }

        void OnContentViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnContentViewPropertyChanged(sender, e));
                return;
            }
            if (IsVisible && (e.PropertyName == Xamarin.Forms.Layout.PaddingProperty.PropertyName || e.PropertyName == KeyboardServiceHeight))
                LayoutChildren(X, Y, Width, Height);
        }


        internal protected bool _isPushed;
        protected override void OnAppearingAnimationBegin()
        {
            _isPushed = true;
            base.OnAppearingAnimationBegin();
        }

        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
            _isPushed = false;
        }

        bool _isPushing;
        /// <summary>
        /// Push the popup asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task Push()
        {
            // do not use the following ... it will prevent popups from appearing when quickly showing and hiding
            //if (!Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Contains(this))
            {
                if (_isPushing)
                    return;
                if (P42.Utils.Environment.IsOnMainThread)
                {
                    _isPushing = true;
                    while (_isPoping) await Task.Delay(100);
                    if (IsVisible)
                        await Navigation.PushPopupAsync(this);
                    _isPushing = false;
                }
                else
                    Device.BeginInvokeOnMainThread(async () => await Push());
            }
        }

        bool _isPoping;
        /// <summary>
        /// Pop the popup asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task Pop()
        {
            // do not use the following ... it will prevent popups from appearing when quickly showing and hiding
            //if (Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Contains(this))
            {
                if (_isPoping)
                    return;
                if (P42.Utils.Environment.IsOnMainThread)
                {
                    _isPoping = true;
                    while (_isPushing) await Task.Delay(100);
                    if (!IsVisible)
                        await Navigation.RemovePopupPageAsync(this);
                    _isPoping = false;
                }
                else
                    Device.BeginInvokeOnMainThread(async () => await Pop());
            }
        }



        /// <param name="propertyName">The name of the property that changed.</param>
        /// <summary>
        /// Call this method from a child class to notify that a change happened on a property.
        /// </summary>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            base.OnPropertyChanged(propertyName);

            if (propertyName == PageOverlayColorProperty.PropertyName)
                base.BackgroundColor = PageOverlayColor;
            else if (propertyName == IsVisibleProperty.PropertyName)
            {
                if (IsVisible)// && PopupPage != null)
                {
                    DecorativeContainerView.TranslationX = 0;
                    DecorativeContainerView.TranslationY = 0;
                    if (Application.Current.MainPage == null)
                    {
                        IsVisible = false;
                        return;
                    }
                    Push();
                }
                else
                {
                    Pop();
                }
            }
            else if (propertyName == RetainProperty.PropertyName && !Retain)
                Dispose();
            else if (propertyName == CancelOnPageOverlayTouchProperty.PropertyName)
                CloseWhenBackgroundIsClicked = CancelOnPageOverlayTouch;


            if (_decorativeContainerView != null)
            {
                #region ILayout
                if (propertyName == PaddingProperty.PropertyName)
                    _decorativeContainerView.Padding = Padding;
                #endregion

                #region IBackground
                else if (propertyName == BackgroundImageProperty.PropertyName)
                    _decorativeContainerView.BackgroundImage = BackgroundImage;

                #region IShape
                else if (propertyName == BackgroundColorProperty.PropertyName)
                    _decorativeContainerView.BackgroundColor = (BackgroundColor == Color.Default || BackgroundColor == default(Color) ? Color.White : BackgroundColor);
                else if (propertyName == HasShadowProperty.PropertyName)
                    _decorativeContainerView.HasShadow = HasShadow;
                else if (propertyName == ShadowInvertedProperty.PropertyName)
                    _decorativeContainerView.ShadowInverted = ShadowInverted;
                else if (propertyName == OutlineColorProperty.PropertyName)
                    _decorativeContainerView.OutlineColor = OutlineColor;
                else if (propertyName == OutlineWidthProperty.PropertyName)
                    _decorativeContainerView.OutlineWidth = OutlineWidth;
                else if (propertyName == OutlineRadiusProperty.PropertyName)
                    _decorativeContainerView.OutlineRadius = OutlineRadius;
                else if (propertyName == ElementShapeProperty.PropertyName)
                    _decorativeContainerView.ElementShape = ElementShape;
                #endregion IShape

                #endregion IBackground
            }

            if (IsVisible)
                HardForceLayout();


            if (propertyName == IsVisibleProperty.PropertyName && IsVisible)
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(refreshPeriod), () =>
                {
                    Update();
                    return IsVisible && !_disposed;
                });
            }

        }

        const int refreshPeriod = 50;


        Rectangle _lastTargetBounds = new Rectangle();

        /// <summary>
        /// For internal use only!
        /// </summary>
        protected void Update()
        {
            if (IsVisible && Target != null)
            {
                var targetBounds = Target is PopupBase popup
                    ? DependencyService.Get<IDescendentBounds>().PageDescendentBounds(this, popup.DecorativeContainerView)
                    : DependencyService.Get<IDescendentBounds>().PageDescendentBounds(this, Target);

                if (targetBounds.Width < 0 && targetBounds.Height < 0 && targetBounds.X < 0 && targetBounds.Y < 0)
                    return;

                //System.Diagnostics.Debug.WriteLine("Target.Bounds=[" + targetBounds + "]");

                if (_lastTargetBounds != targetBounds) //&& DateTime.Now - _lastLayout > TimeSpan.FromMilliseconds(refreshPeriod))
                {
                    _lastTargetBounds = targetBounds;
                    //System.Diagnostics.Debug.WriteLine("B");
                    HardForceLayout();
                }
            }
        }

        /// <summary>
        /// I really mean it this time!
        /// </summary>
        protected void HardForceLayout()
        {
            //System.Diagnostics.Debug.WriteLine("C");
            LayoutChildren(0, 0, Width, Height);// - KeyboardService.Height);
        }


        #endregion


    }
}

