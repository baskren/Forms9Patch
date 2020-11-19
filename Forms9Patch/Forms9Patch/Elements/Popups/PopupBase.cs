using Xamarin.Forms;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.Runtime.CompilerServices;
using P42.Utils;
using Forms9Patch.Elements.Popups.Core;
using System.Collections.Generic;

namespace Forms9Patch
{

    /// <summary>
    /// Forms9Patch Popup base.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentProperty(nameof(ContentView))]
    public abstract class PopupBase : PopupPage, IPopup, IDisposable

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
            get => (bool)GetValue(IsAnimationEnabledProperty);
            set => SetValue(IsAnimationEnabledProperty, value);
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
        public static readonly BindableProperty TargetProperty = BindableProperty.Create(nameof(Target), typeof(VisualElement), typeof(PopupBase), default(Element));
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
        public static readonly BindableProperty PageOverlayColorProperty = BindableProperty.Create(nameof(PageOverlayColor), typeof(Color), typeof(PopupBase), Color.FromRgba(128, 128, 128, 128));
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
        public static readonly BindableProperty CancelOnPageOverlayTouchProperty = BindableProperty.Create(nameof(CancelOnPageOverlayTouch), typeof(bool), typeof(PopupBase), true);
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
        public static readonly BindableProperty CancelOnBackButtonClickProperty = BindableProperty.Create(nameof(CancelOnBackButtonClick), typeof(bool), typeof(PopupBase), true);
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


        #region PopAfter property
        /// <summary>
        /// BindableProperty key for PopAfter property
        /// </summary>
        public static readonly BindableProperty PopAfterProperty = BindableProperty.Create(nameof(PopAfter), typeof(TimeSpan), typeof(PopupBase), default(TimeSpan));
        /// <summary>
        /// Duration of popup appearance before it is automatically cancelled
        /// </summary>
        public TimeSpan PopAfter
        {
            get => (TimeSpan)GetValue(PopAfterProperty);
            set => SetValue(PopAfterProperty, value);
        }
        #endregion PopAfter property


        #region Parameter property
        /// <summary>
        /// BindableProperty key for Parameter property
        /// </summary>
        public static readonly BindableProperty ParameterProperty = BindableProperty.Create(nameof(Parameter), typeof(object), typeof(PopupBase), default);
        /// <summary>
        /// Object that can be set prior to appearance of Popup for the purpose of application to processing after the popup is disappeared;
        /// </summary>
        public object Parameter
        {
            get => GetValue(ParameterProperty);
            set => SetValue(ParameterProperty, value);
        }
        #endregion Parameter property


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
            get => (Image)GetValue(BackgroundImageProperty);
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
        public static readonly BindableProperty BorderColorProperty = ShapeBase.BorderColorProperty;
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
        public static readonly BindableProperty BorderRadiusProperty = ShapeBase.BorderRadiusProperty;
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
        public static readonly BindableProperty BorderWidthProperty = ShapeBase.BorderWidthProperty;
        /// <summary>
        /// Gets or sets the width of the boarder.
        /// </summary>
        /// <value>The width of the boarder.</value>
        public float BorderWidth
        {
            get => (float)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
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
                {
                    _decorativeContainerView.Padding = Padding;
                    _decorativeContainerView.BackgroundColor = (BackgroundColor == Color.Default || BackgroundColor == default ? Color.White : BackgroundColor);
                    _decorativeContainerView.HasShadow = HasShadow;
                    _decorativeContainerView.ShadowInverted = ShadowInverted;
                    _decorativeContainerView.OutlineColor = OutlineColor;
                    _decorativeContainerView.OutlineWidth = OutlineWidth;
                    _decorativeContainerView.OutlineRadius = OutlineRadius;
                    _decorativeContainerView.ElementShape = ElementShape;
                    newLayout.PropertyChanged += OnContentViewPropertyChanged;
                }
                Content = _decorativeContainerView as View;
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

        /// <summary>
        /// Occurs when popup appearing animation has started
        /// </summary>
        public event EventHandler AppearingAnimationBegin;

        /// <summary>
        /// Occurs when popup appearing animation has ended
        /// </summary>
        public event EventHandler AppearingAnimationEnd;

        /// <summary>
        /// occurs when popup disappearing animation has started
        /// </summary>
        public event EventHandler DisappearingAnimationBegin;

        /// <summary>
        /// Occurs when popup disappearing animation has ended
        /// </summary>
        public event EventHandler DisappearingAnimationEnd;

        #endregion


        #region Fields
        internal ILayout _decorativeContainerView;
        internal DateTime PresentedAt;
        static int _instances;
        /// <summary>
        /// Incremental identifier;
        /// </summary>
        protected readonly int _id;
        /// <summary>
        /// Say, when was the last time I ...
        /// </summary>
        protected DateTime _lastLayout = DateTime.MinValue;
        PopupPoppedEventArgs PopupPoppedEventArgs;
        #endregion


        #region Construction / Disposal
        static PopupBase()
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Initializes new instance of the PopupBase class.
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="popAfter"></param>
        internal PopupBase(Segment segment, TimeSpan popAfter = default) : this(segment._button, popAfter) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.PopupBase"/> class.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="popAfter">Pop after TimeSpan.</param>
        internal PopupBase(VisualElement target = null, TimeSpan popAfter = default)
        {

            P42.Utils.DebugExtensions.AddToCensus(this);

            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;

            CloseWhenBackgroundIsClicked = CancelOnPageOverlayTouch;
            BackgroundColor = Color.White;

            Padding = 10;
            HasShadow = true;
            OutlineRadius = 5;

            _id = _instances++;
            IsVisible = false;
            Target = target;

            KeyboardService.HeightChanged += OnKeyboardHeightChanged;

            IsAnimationEnabled = false;

            PopAfter = popAfter;
        }


        bool _disposed; // To detect redundant calls

        /// <summary>
        /// Clean up unmanaged objects
        /// </summary>
        /// <param name="disposing">Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;

                Cancelled = null;
                Popped = null;
                AppearingAnimationBegin = null;
                AppearingAnimationEnd = null;
                DisappearingAnimationBegin = null;
                DisappearingAnimationEnd = null;

                if (_decorativeContainerView is VisualElement oldLayout)
                    oldLayout.PropertyChanged -= OnContentViewPropertyChanged;

                KeyboardService.HeightChanged -= OnKeyboardHeightChanged;
                Parameter = null;

                if (_decorativeContainerView is IDisposable disposable)
                    disposable.Dispose();

                try
                {
                    _semaphore.Dispose();
                }
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
                catch (Exception) { }
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body

                P42.Utils.DebugExtensions.RemoveFromCensus(this);
            }
        }


        /// <summary>
        /// Releases all resource used by the <see cref="T:Forms9Patch.PopupBase"/> object.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Disposal handled in Device.BeginInvokeOnMainThread block")]
        public void Dispose()
        {
            if (_isPopped || (!_isPushing && !_isPushed))
            {
                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(()=> Dispose(true));

                GC.SuppressFinalize(this);
            }
            else
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
                Device.BeginInvokeOnMainThread((Action)(async () =>
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
                {
                    if (GetType() == typeof(ActivityIndicatorPopup))
                        await CancelAsync(PopupPoppedCause.Disposed);
                    await WaitForPoppedAsync();
                    await Task.Delay(50);
                    Dispose();
                }));
        }
        #endregion


        #region Cancelation
        /// <summary>
        /// Delay until the popup is popped.
        /// </summary>
        /// <returns>Why the popup was popped and, if appropriate, what triggered it.</returns>
        public virtual async Task<PopupPoppedEventArgs> WaitForPoppedAsync()
        {
            while (!_isPopped)
                await Task.Delay(50);
            var args = PopupPoppedEventArgs;
            PopupPoppedEventArgs = null;
            return args;
        }


        /// <summary>
        /// Called when back button is pressed
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            //wSystem.Diagnostics.Debug.WriteLine(GetType() + "." + ReflectionExtensions.CallerMemberName());
            if (CancelOnBackButtonClick)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                CancelAsync(PopupPoppedCause.HardwareBackButtonPressed);
            if (Device.RuntimePlatform == Device.UWP)
                Navigation.PopAllPopupAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            return CancelOnBackButtonClick;

        }

        /// <summary>
        /// Called when background is clicked;
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackgroundClicked()
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + ReflectionExtensions.CallerMemberName());
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
        public virtual async Task CancelAsync(object trigger = null)
            => await PopAsync(trigger ?? PopupPoppedCause.MethodCalled, lastAction: () => Cancelled?.Invoke(this, PopupPoppedEventArgs));

        /// <summary>
        /// Cancels the popup
        /// </summary>
        /// <param name="trigger"></param>
        [Obsolete("Use CancelAsync instead")]
        public void Cancel(object trigger = null) => Task.Run(async () => await CancelAsync(trigger ?? ReflectionExtensions.CallerMemberName()));

        #endregion


        #region PropertyChange managment
        const string KeyboardServiceHeight = "KeyboardService.Height";

        void OnKeyboardHeightChanged(object sender, double e) => OnContentViewPropertyChanged(sender, new PropertyChangedEventArgs(KeyboardServiceHeight));

        /*
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
        */

        void OnContentViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                if (IsVisible && (e.PropertyName == Xamarin.Forms.Layout.PaddingProperty.PropertyName || e.PropertyName == KeyboardServiceHeight))
                    LayoutChildren(X, Y, Width, Height);
            });
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
        /// true if Popup is popped;
        /// </summary>
        internal protected bool _isPopped;

        /// <summary>
        /// true if pop animation is complete
        /// </summary>
        internal protected bool _popAnimationComplete;


        /// <summary>
        /// Called when the popup is starting to appear
        /// </summary>
        protected override void OnAppearingAnimationBegin()
        {
            Recursion.Enter(GetType().ToString(), _id.ToString());
            _isPopped = false;
            _popAnimationComplete = false;
            AppearingAnimationBegin?.Invoke(this, EventArgs.Empty);
            _isPushing = true;
            base.OnAppearingAnimationBegin();
            Recursion.Exit(GetType().ToString(), _id.ToString());
        }

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        /// <summary>
        /// Called when the popup has appeared
        /// </summary>
        protected override async void OnAppearingAnimationEnd()
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + ReflectionExtensions.CallerMemberName());
            Recursion.Enter(GetType().ToString(), _id.ToString());
            _isPopping = false;
            _isPopped = false;
            _popAnimationComplete = false;
            base.OnAppearingAnimationEnd();
            _isPushing = false;
            _isPushed = true;

            await Task.Delay(TimeSpan.FromSeconds(1));

            if (!IsVisible && !_isPopping)
                await CancelAsync(PopupPoppedCause.IsVisiblePropertySet);
            else if (PopAfter > default(TimeSpan))
                Device.StartTimer(PopAfter, () =>
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    CancelAsync(PopupPoppedCause.Timeout);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    return false;
                });

            AppearingAnimationEnd?.Invoke(this, EventArgs.Empty);
            Recursion.Exit(GetType().ToString(), _id.ToString());
        }

        /// <summary>
        /// Called when the popup is starting to disappear
        /// </summary>
        protected override void OnDisappearingAnimationBegin()
        {
            Recursion.Enter(GetType().ToString(), _id.ToString());
            _isPopped = false;
            _popAnimationComplete = false;
            DisappearingAnimationBegin?.Invoke(this, EventArgs.Empty);
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + ReflectionExtensions.CallerMemberName());
            _isPushing = false;
            _isPushed = false;
            _isPopping = true;
            base.OnDisappearingAnimationBegin();
            Recursion.Exit(GetType().ToString(), _id.ToString());
        }

        /// <summary>
        /// Called when the popup has disappeared
        /// </summary>
        protected override void OnDisappearingAnimationEnd()
        {
            Recursion.Enter(GetType().ToString(), _id.ToString());
            _popAnimationComplete = true;
            base.OnDisappearingAnimationEnd();
            _isPushing = false;
            _isPushed = false;
            _isPopping = false;

            if (IsVisible && !_isPushing)
                PushAsync().ConfigureAwait(false);
            else
            {
                _isPopped = true;
                DisappearingAnimationEnd?.Invoke(this, EventArgs.Empty);
            }
            Recursion.Exit(GetType().ToString(), _id.ToString());
        }

        readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Obsolete.  Use PushAsync instead
        /// </summary>
        /// <returns>The push.</returns>
        [Obsolete("Use PushAsync instead")]
        public async Task Push()
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + ReflectionExtensions.CallerMemberName());
            await PushAsync();
        }


        //bool _isPushing;
        /// <summary>
        /// Push the popup asynchronously
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0022:A catch clause that catches System.Exception and has an empty body", Justification = "<Pending>")]
        public async Task PushAsync()
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + ReflectionExtensions.CallerMemberName());
            // do not use the following ... it will prevent popups from appearing when quickly showing and hiding
            //System.Diagnostics.Debug.WriteLine("PUSH");
            IsVisible = true;
            //if (_isPushing || _isPushed)
            //    return;
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(async() =>
            {
                Recursion.Enter(GetType().ToString(), _id.ToString());
                //_isPushing = true;
                //while (_isPoping) await Task.Delay(100);
                //if (IsVisible)
                await _semaphore.WaitAsync();

                if (!PopupNavigation.Instance.PopupStack.Contains(this))
                {
                    PopupPoppedEventArgs = null;
                    _isPushing = true;
                    base.IsAnimationEnabled = IsAnimationEnabled;
                    await Navigation.PushPopupAsync(this);
                    PopupLayerEffect.ApplyTo(this);
                }
                try
                {
                    if (!_disposed)
                        _semaphore.Release();
                }
                catch (Exception) { }
                Recursion.Exit(GetType().ToString(), _id.ToString());
            });

        }

        /// <summary>
        /// Pops the popup
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        [Obsolete("Use PopAsync instead")]
        public async Task Pop(object trigger = null)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + ReflectionExtensions.CallerMemberName());
            if (trigger == null)
                await PopAsync(PopupPoppedCause.MethodCalled, ReflectionExtensions.CallerMemberName());
            else
                await PopAsync(trigger);
        }



        /// <summary>
        /// Called to Pop a popup
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="callerName"></param>
        /// <param name="lastAction"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0022:A catch clause that catches System.Exception and has an empty body", Justification = "<Pending>")]
        public async Task PopAsync(object trigger = null, [CallerMemberName] string callerName = "", Action lastAction = null)
        {
            // do not use the following ... it will prevent popups from appearing when quickly showing and hiding
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + ReflectionExtensions.CallerMemberName());
            if (P42.Utils.Environment.IsOnMainThread)
            {
                Recursion.Enter(GetType(), _id);

                IsVisible = false;
                try
                {
                    if (PopupPoppedEventArgs == null
                        && _semaphore is SemaphoreSlim semaphore
                        && PopupNavigation.Instance?.PopupStack is IReadOnlyList<PopupPage> popupStack
                        && popupStack.Contains(this)
                        && Navigation is INavigation navigation)
                    {
                        await semaphore.WaitAsync();
                        _isPopping = true;
                        SetPopupPoppedEventArgs(trigger, callerName);
                        PopupLayerEffect.RemoveFrom(this);
                        base.IsAnimationEnabled = IsAnimationEnabled;
                        await navigation.RemovePopupPageAsync(this);
                        try
                        {
                            if (!_disposed)
                                semaphore.Release();
                        }
                        catch (Exception) { }
                        Popped?.Invoke(this, PopupPoppedEventArgs);
                    }
                }
                catch (ObjectDisposedException)
                {
                    Recursion.Exit(GetType(), _id);
                    return;
                }

                lastAction?.Invoke();

                if (_isPopping)
                    do
                    {
                        await Task.Delay(100);
                    }
                    while (/*PopupNavigation.Instance.PopupStack.Contains(this) && */!_popAnimationComplete);

                Recursion.Exit(GetType(), _id);
            }
            else
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
                Device.BeginInvokeOnMainThread(async () => await PopAsync(trigger, callerName));
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void

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


        /// <param name="propertyName">The name of the property that changed.</param>
        /// <summary>
        /// Call this method from a child class to notify that a change happened on a property.
        /// </summary>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {

                try
                {
                    base.OnPropertyChanged(propertyName);
                }
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
                catch (Exception) { }
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body

                if (propertyName == PageOverlayColorProperty.PropertyName)
                    base.BackgroundColor = PageOverlayColor;
                else if (propertyName == IsAnimationEnabledProperty.PropertyName)
                    base.IsAnimationEnabled = IsAnimationEnabled;
                else if (propertyName == IsVisibleProperty.PropertyName)
                {
                    if (IsVisible && !_isPushed)// && PopupPage != null)
                    {
                        //System.Diagnostics.Debug.WriteLine(GetType() + "." + ReflectionExtensions.CallerMemberName() + " IsVisible");
                        DecorativeContainerView.TranslationX = 0;
                        DecorativeContainerView.TranslationY = 0;
                        if (Application.Current?.MainPage == null)
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
                        //System.Diagnostics.Debug.WriteLine(GetType() + "." + ReflectionExtensions.CallerMemberName() + " !IsVisible");
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        //PopAsync(PopupPoppedCause.IsVisiblePropertySet);
                        CancelAsync(PopupPoppedCause.IsVisiblePropertySet);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    }
                    //else
                    //    System.Diagnostics.Debug.WriteLine("IsVisible=[" + IsVisible + "] _isPushed=[" + _isPushed + "]");
                }
                else if (propertyName == CancelOnPageOverlayTouchProperty.PropertyName)
                    CloseWhenBackgroundIsClicked = CancelOnPageOverlayTouch;
                else if (propertyName == TargetProperty.PropertyName)
                    Update();


                if (_decorativeContainerView != null && !_disposed)
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
            });
        }



        Rectangle _lastTargetBounds = new Rectangle();

        /// <summary>
        /// For internal use only!
        /// </summary>
        protected void Update()
        {
            if (IsVisible && Target != null)
            {
                Recursion.Enter(GetType().ToString(), _id.ToString());
                var targetBounds = Target is PopupBase popup
                    ? DependencyService.Get<IDescendentBounds>().PageDescendentBounds(this, popup.DecorativeContainerView)
                    : DependencyService.Get<IDescendentBounds>().PageDescendentBounds(this, Target);

                if (!(targetBounds.Width < 0 && targetBounds.Height < 0 && targetBounds.X < 0 && targetBounds.Y < 0))
                {
                    if (_lastTargetBounds != targetBounds) //&& DateTime.Now - _lastLayout > TimeSpan.FromMilliseconds(refreshPeriod))
                    {
                        _lastTargetBounds = targetBounds;
                        HardForceLayout();
                    }
                }
                Recursion.Exit(GetType().ToString(), _id.ToString());
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

