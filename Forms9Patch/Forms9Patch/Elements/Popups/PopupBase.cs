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
using System.Threading;
using System.Runtime.CompilerServices;

namespace Forms9Patch
{

    /// <summary>
    /// Forms9Patch Popup base.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentProperty(nameof(ContentView))]
    public abstract class PopupBase : Rg.Plugins.Popup.Pages.PopupPage, IPopup, IDisposable

    {
        /// <summary>
        /// backing store for IsAnimationEnabled property
        /// </summary>
        public static new readonly BindableProperty IsAnimationEnabledProperty = BindableProperty.Create(nameof(IsAnimationEnabled), typeof(bool), typeof(PopupBase), default(bool));
        /// <summary>
        /// Determines if popup Pop and Push events are animated
        /// </summary>
        public new bool IsAnimationEnabled
        {
            get => (bool)GetValue(PopupBase.IsAnimationEnabledProperty);
            set => SetValue(PopupBase.IsAnimationEnabledProperty, value);
        }

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

        #region PopAfter property
        /// <summary>
        /// Backing store for PopAfter property
        /// </summary>
        public static readonly BindableProperty PopAfterProperty = BindableProperty.Create("Forms9Patch.PopupBase.PopAfter", typeof(TimeSpan), typeof(PopupBase), default(TimeSpan));
        /// <summary>
        /// Will cause the popup to cancel (disappear) after Popafter TimeSpan
        /// </summary>
        public TimeSpan PopAfter
        {
            get => (TimeSpan)GetValue(PopAfterProperty);
            set => SetValue(PopAfterProperty, value);
        }
        #endregion PopAfter property

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
        /// <summary>
        /// The boarder color property.
        /// </summary>
        public static readonly BindableProperty BorderColorProperty = ShapeBase.OutlineColorProperty;
        /// <summary>
        /// Gets or sets the color of the boarder.
        /// </summary>
        /// <value>The color of the boarder.</value>
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        #endregion OutlineColor property

        #region OutlineRadius property
        /// <summary>
        /// Backing store for the outline radius property.
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
        /// <summary>
        /// The boarder radius property.
        /// </summary>
        public static readonly BindableProperty BorderRadiusProperty = ShapeBase.OutlineRadiusProperty;
        /// <summary>
        /// Gets or sets the boarder radius.
        /// </summary>
        /// <value>The boarder radius.</value>
        public float BorderRadius
        {
            get => (float)GetValue(BorderRadiusProperty);
            set => SetValue(BorderRadiusProperty, value);
        }
        #endregion OutlineRadius property

        #region OutlineWidth property
        /// <summary>
        /// Backing store for the outline width property.
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
        /// <summary>
        /// The boarder width property.
        /// </summary>
        public static readonly BindableProperty BorderWidthProperty = ShapeBase.OutlineWidthProperty;
        /// <summary>
        /// Gets or sets the width of the boarder.
        /// </summary>
        /// <value>The width of the boarder.</value>
        public float BorderWidth
        {
            get => (float)GetValue(OutlineWidthProperty);
            set => SetValue(OutlineWidthProperty, value);
        }
        #endregion OutlineWidth property

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
        public event EventHandler<PopupPoppedEventArgs> Cancelled;

        /// <summary>
        /// Occurs when popup has popped;
        /// </summary>
        public event EventHandler<PopupPoppedEventArgs> Popped;
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
        PopupPoppedEventArgs PopupPoppedEventArgs;
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
        /// <param name="popAfter">Pop after TimeSpan.</param>
        internal PopupBase(VisualElement target = null, bool retain = false, TimeSpan popAfter = default)
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

            IsAnimationEnabled = false;

            PopAfter = popAfter;


        }

        #endregion


        #region Cancelation

        /// <summary>
        /// Called when back button is pressed
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            //wSystem.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName());
            if (CancelOnBackButtonClick)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                CancelAsync(PopupPoppedCause.HardwareBackButtonPressed);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            return CancelOnBackButtonClick;

        }

        /// <summary>
        /// Called when background is clicked;
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackgroundClicked()
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName());
            var isClose = base.OnBackgroundClicked();
            if (isClose)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                CancelAsync(PopupPoppedCause.BackgroundTouch);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            return isClose;
        }

        /// <summary>
        /// Cancels the async.
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="trigger">Optional, object or PopupEventCause that triggered cancelation.</param>
        public async Task CancelAsync(object trigger = null)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName());
            if (P42.Utils.Environment.IsOnMainThread)
            {
                if (trigger == null)
                    await PopAsync(PopupPoppedCause.MethodCalled, P42.Utils.ReflectionExtensions.CallerMemberName());
                else
                    await PopAsync(trigger);
                while (_isPushed)
                    await Task.Delay(50);
                Cancelled?.Invoke(this, PopupPoppedEventArgs);
            }
            else
                Device.BeginInvokeOnMainThread(async () => await CancelAsync(trigger));
        }

        /// <summary>
        /// Cancels the popup
        /// </summary>
        /// <param name="trigger"></param>
        [Obsolete("Use CancelAsync instead")]
        public void Cancel(object trigger = null) => Task.Run(async () => await CancelAsync(trigger ?? P42.Utils.ReflectionExtensions.CallerMemberName()));

        #endregion


        #region IDisposable Support
        bool _disposed; // To detect redundant calls

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                //  Don't want to do this.  Will cause popups to not be reusable by default.
                //try
                //{
                //    if (Content is IDisposable disposable)
                //        disposable.Dispose();
                //}
                //catch (Exception) { }
                if (_decorativeContainerView is VisualElement oldLayout)
                    oldLayout.PropertyChanged -= OnContentViewPropertyChanged;
                KeyboardService.HeightChanged -= OnKeyboardHeightChanged;


                _lock.Dispose();
                Retain = false;
            }
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:Forms9Patch.PopupBase"/> object.
        /// </summary>
        public void Dispose()
        {
            if (IsVisible || _isPushing || _isPushed || _isPopping)
                return;
            Dispose(true);
            GC.SuppressFinalize(this);
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

            layout.BackgroundColor = (BackgroundColor == Color.Default || BackgroundColor == default ? Color.White : BackgroundColor);
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
                Device.BeginInvokeOnMainThread(() => OnContentViewPropertyChanged(sender, e));
            else if (IsVisible && (e.PropertyName == Xamarin.Forms.Layout.PaddingProperty.PropertyName || e.PropertyName == KeyboardServiceHeight))
                LayoutChildren(X, Y, Width, Height);
        }

        /// <summary>
        /// Is true when the popup is being pushed
        /// </summary>
        internal protected bool _isPushing;
        /// <summary>
        /// Is true when the popup has been pushed
        /// </summary>
        internal protected bool _isPushed;
        /// <summary>
        /// Is true when the popup is being popped
        /// </summary>
        internal protected bool _isPopping;

        /// <summary>
        /// Called when the popup is starting to appear
        /// </summary>
        protected override void OnAppearingAnimationBegin()
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                //var mainPage = 
            }
            _isPushing = true;
            base.OnAppearingAnimationBegin();
        }

        /// <summary>
        /// Called when the popup has appeared
        /// </summary>
        protected override async void OnAppearingAnimationEnd()
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName());
            base.OnAppearingAnimationEnd();
            _isPushed = true;
            _isPushing = false;

            if (!IsVisible && !_isPopping)
                await PopAsync(PopupPoppedCause.IsVisiblePropertySet);
            else if (PopAfter > default(TimeSpan))
                Device.StartTimer(PopAfter, () =>
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    PopAsync(PopupPoppedCause.Timeout);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    return false;
                });
        }

        /// <summary>
        /// Called when the popup is starting to disappear
        /// </summary>
        protected override void OnDisappearingAnimationBegin()
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName());
            _isPopping = true;
            _isPushed = false;
            base.OnDisappearingAnimationBegin();
        }

        /// <summary>
        /// Called when the popup has disappeared
        /// </summary>
        protected override void OnDisappearingAnimationEnd()
        {

            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName());
            base.OnDisappearingAnimationEnd();
            //IsVisible = false;
            _isPopping = false;

            if (IsVisible && !_isPushing)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                PushAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            //else if (!Retain)
            //    Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            //    {
            //        if (!Retain)
            //            Dispose();
            //        return false;
            //    });
        }

        readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Obsolete.  Use PushAsync instead
        /// </summary>
        /// <returns>The push.</returns>
        [Obsolete("Use PushAsync instead")]
        public async Task Push()
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName());
            await PushAsync();
        }

        //bool _isPushing;
        /// <summary>
        /// Push the popup asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task PushAsync()
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName());
            // do not use the following ... it will prevent popups from appearing when quickly showing and hiding
            //if (!Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Contains(this))
            {
                //System.Diagnostics.Debug.WriteLine("PUSH");
                IsVisible = true;
                //if (_isPushing || _isPushed)
                //    return;
                if (P42.Utils.Environment.IsOnMainThread)
                {
                    //_isPushing = true;
                    //while (_isPoping) await Task.Delay(100);
                    //if (IsVisible)
                    await _lock.WaitAsync();

                    if (!Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Contains(this))
                    {
                        PopupPoppedEventArgs = null;
                        _isPushing = true;
                        base.IsAnimationEnabled = IsAnimationEnabled;
                        await Navigation.PushPopupAsync(this);
                        PopupLayerEffect.ApplyTo(this);
                    }
                    //_isPushing = false;
                    _lock.Release();
                }
                else
                    Device.BeginInvokeOnMainThread(async () => await PushAsync());
            }
        }

        /// <summary>
        /// Pops the popup
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        [Obsolete("Use PopAsync instead")]
        public async Task Pop(object trigger = null)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName());
            if (trigger == null)
                await PopAsync(PopupPoppedCause.MethodCalled, P42.Utils.ReflectionExtensions.CallerMemberName());
            else
                await PopAsync(trigger);
        }


        /// <summary>
        /// Called to Pop a popup
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public async Task PopAsync(object trigger = null, [CallerMemberName] string callerName = "")
        {
            // do not use the following ... it will prevent popups from appearing when quickly showing and hiding
            //if (Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Contains(this))
            {
                //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName());
                if (P42.Utils.Environment.IsOnMainThread)
                {
                    _isPopping = true;
                    IsVisible = false;
                    await _lock.WaitAsync();
                    if (Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Contains(this) && PopupPoppedEventArgs == null)
                    {
                        _isPopping = true;
                        SetPopupPoppedEventArgs(trigger, callerName);
                        PopupLayerEffect.RemoveFrom(this);
                        base.IsAnimationEnabled = IsAnimationEnabled;
                        await Navigation.RemovePopupPageAsync(this);
                        Popped?.Invoke(this, PopupPoppedEventArgs);
                    }
                    _lock.Release();
                }
                else
                    Device.BeginInvokeOnMainThread(async () => await PopAsync(trigger, callerName));
            }
        }


        void SetPopupPoppedEventArgs(object trigger, string callerName)
        {
            if (trigger is PopupPoppedCause cause)
            {
                if (cause == PopupPoppedCause.MethodCalled)
                    PopupPoppedEventArgs = new PopupPoppedEventArgs(PopupPoppedCause.MethodCalled, callerName);
                else
                    PopupPoppedEventArgs = new PopupPoppedEventArgs(cause, null);
            }
            else if (trigger is Button)
                PopupPoppedEventArgs = new PopupPoppedEventArgs(PopupPoppedCause.ButtonTapped, trigger);
            else if (trigger is Segment)
                PopupPoppedEventArgs = new PopupPoppedEventArgs(PopupPoppedCause.SegmentTapped, trigger);
            else
                PopupPoppedEventArgs = new PopupPoppedEventArgs(PopupPoppedCause.Custom, trigger);
        }

        /// <summary>
        /// Delay until the popup is popped.
        /// </summary>
        /// <returns>Why the popup was popped and, if appropriate, what triggered it.</returns>
        public virtual async Task<PopupPoppedEventArgs> DelayUntilPoppedAsync()
        {
            while (PopupPoppedEventArgs == null)
                await Task.Delay(50);
            var args = PopupPoppedEventArgs;
            PopupPoppedEventArgs = null;
            return args;
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
            else if (propertyName == IsAnimationEnabledProperty.PropertyName)
                base.IsAnimationEnabled = IsAnimationEnabled;
            else if (propertyName == IsVisibleProperty.PropertyName)
            {
                if (IsVisible && !_isPushed)// && PopupPage != null)
                {
                    //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + " IsVisible");
                    DecorativeContainerView.TranslationX = 0;
                    DecorativeContainerView.TranslationY = 0;
                    if (Application.Current.MainPage == null)
                    {
                        IsVisible = false;
                        return;
                    }
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    PushAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
                else if (!IsVisible && _isPushed && !_isPopping)
                {
                    //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + " !IsVisible");
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    PopAsync(PopupPoppedCause.IsVisiblePropertySet);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
                //else
                //    System.Diagnostics.Debug.WriteLine("IsVisible=[" + IsVisible + "] _isPushed=[" + _isPushed + "]");
            }
            //else if (propertyName == RetainProperty.PropertyName && !Retain)
            //    Dispose();
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
                    _decorativeContainerView.BackgroundColor = (BackgroundColor == Color.Default || BackgroundColor == default ? Color.White : BackgroundColor);
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

            /*
            if (IsVisible)
                HardForceLayout();


            if (propertyName == IsVisibleProperty.PropertyName && IsVisible)
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(refreshPeriod), () =>
                {
                    Update();
                    return IsVisible;// && !_disposed;
                });
            }
            */
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

