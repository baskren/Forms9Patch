using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Forms9Patch.Elements.Popups.Core.Animations;
using Xamarin.Forms;

#pragma warning disable CS0618 // Type or member is obsolete
namespace Forms9Patch.Elements.Popups.Core
{
    /// <summary>
    /// Foundation for Popups
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class PopupPage : Xamarin.Forms.ContentPage
    {
        #region Private
        /*
        private const string IsAnimatingObsoleteText =
            nameof(IsAnimating) +
            " is obsolute as of v1.1.5. Please use "
            + nameof(IsAnimationEnabled) +
            " instead. See more info: "
            + Config.MigrationV1_0_xToV1_1_xUrl;
            */
        #endregion

        #region Internal Properties

        internal Task AppearingTransactionTask { get; set; }

        internal Task DisappearingTransactionTask { get; set; }

        #endregion

        #region Events
        /// <summary>
        /// Triggered when popup's background is clicked
        /// </summary>
        public event EventHandler BackgroundClicked;

        #endregion

        #region Bindable Properties
        /// <summary>
        /// BindableProperty for IsAnmiationEnabled property
        /// </summary>
        public static readonly BindableProperty IsAnimationEnabledProperty = BindableProperty.Create(nameof(IsAnimationEnabled), typeof(bool), typeof(PopupPage), true);

        /// <summary>
        /// Enables animation
        /// </summary>
        public bool IsAnimationEnabled
        {
            get { return (bool)GetValue(IsAnimationEnabledProperty); }
            set { SetValue(IsAnimationEnabledProperty, value); }
        }

        /// <summary>
        /// BindableProperty for HasSystemPadding property
        /// </summary>
        public static readonly BindableProperty HasSystemPaddingProperty = BindableProperty.Create(nameof(HasSystemPadding), typeof(bool), typeof(PopupPage), true);

        /// <summary>
        /// Enables use of system padding
        /// </summary>
        public bool HasSystemPadding
        {
            get { return (bool)GetValue(HasSystemPaddingProperty); }
            set { SetValue(HasSystemPaddingProperty, value); }
        }

        /// <summary>
        /// BindableProperty for Animation property
        /// </summary>
        public static readonly BindableProperty AnimationProperty = BindableProperty.Create(nameof(Animation), typeof(IPopupAnimation), typeof(PopupPage));

        /// <summary>
        /// Sets the animation
        /// </summary>
        public IPopupAnimation Animation
        {
            get { return (IPopupAnimation)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        /// <summary>
        /// BindableProperty for SystemPadding property
        /// </summary>
        public static readonly BindableProperty SystemPaddingProperty = BindableProperty.Create(nameof(SystemPadding), typeof(Thickness), typeof(PopupPage), default(Thickness), BindingMode.OneWayToSource);

        /// <summary>
        /// Sets the system padding
        /// </summary>
        public Thickness SystemPadding
        {
            get { return (Thickness)GetValue(SystemPaddingProperty); }
            private set { SetValue(SystemPaddingProperty, value); }
        }

        /// <summary>
        /// BindableProperty for SystemPaddingSides property
        /// </summary>
        public static readonly BindableProperty SystemPaddingSidesProperty = BindableProperty.Create(nameof(SystemPaddingSides), typeof(PaddingSide), typeof(PopupPage), PaddingSide.All);

        /// <summary>
        /// Sets the sides for the system padding
        /// </summary>
        public PaddingSide SystemPaddingSides
        {
            get { return (PaddingSide)GetValue(SystemPaddingSidesProperty); }
            set { SetValue(SystemPaddingSidesProperty, value); }
        }

        /// <summary>
        /// BindableProperty for CloseWhen BackgroundIsClicked property
        /// </summary>
        public static readonly BindableProperty CloseWhenBackgroundIsClickedProperty = BindableProperty.Create(nameof(CloseWhenBackgroundIsClicked), typeof(bool), typeof(PopupPage), true);

        /// <summary>
        /// Closes popup when background is clicked
        /// </summary>
        public bool CloseWhenBackgroundIsClicked
        {
            get { return (bool)GetValue(CloseWhenBackgroundIsClickedProperty); }
            set { SetValue(CloseWhenBackgroundIsClickedProperty, value); }
        }

        /// <summary>
        /// BindableProperty for BackgroundInputTransparent property
        /// </summary>
        public static readonly BindableProperty BackgroundInputTransparentProperty = BindableProperty.Create(nameof(BackgroundInputTransparent), typeof(bool), typeof(PopupPage), false);

        /// <summary>
        /// Passes gestures to page below background
        /// </summary>
        public bool BackgroundInputTransparent
        {
            get { return (bool)GetValue(BackgroundInputTransparentProperty); }
            set { SetValue(BackgroundInputTransparentProperty, value); }
        }

        /// <summary>
        /// BindableProperty for HasKeyboardOffset property
        /// </summary>
        public static readonly BindableProperty HasKeyboardOffsetProperty = BindableProperty.Create(nameof(HasKeyboardOffset), typeof(bool), typeof(PopupPage), true);

        /// <summary>
        /// enables automated keyboard offset
        /// </summary>
        public bool HasKeyboardOffset
        {
            get { return (bool)GetValue(HasKeyboardOffsetProperty); }
            set { SetValue(HasKeyboardOffsetProperty, value); }
        }

        /// <summary>
        /// BindableProperty for KeyboardOffset property
        /// </summary>
        public static readonly BindableProperty KeyboardOffsetProperty = BindableProperty.Create(nameof(KeyboardOffset), typeof(double), typeof(PopupPage), 0d, BindingMode.OneWayToSource);
        /// <summary>
        /// Gets the current keyboard offset
        /// </summary>
        public double KeyboardOffset
        {
            get { return (double)GetValue(KeyboardOffsetProperty); }
            private set { SetValue(KeyboardOffsetProperty, value); }
        }

        #endregion

        #region Main Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public PopupPage()
        {
            BackgroundColor = Color.FromHex("#80000000");
            Animation = new ScaleAnimation();
        }

        /// <summary>
        /// Invoked when property has changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(HasSystemPadding):
                case nameof(HasKeyboardOffset):
                case nameof(SystemPaddingSides):
                    ForceLayout();
                    break;
                    /*
                case nameof(IsAnimating):
                    IsAnimationEnabled = IsAnimating;
                    break;
                case nameof(IsAnimationEnabled):
                    IsAnimating = IsAnimationEnabled;
                    break;
                    */
            }
        }

        /// <summary>
        /// Invoked when back button has been pressed
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        #endregion

        #region Size Methods
        /// <summary>
        /// Invoked upon layout
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (HasSystemPadding)
            {
                var systemPadding = SystemPadding;
                var systemPaddingSide = SystemPaddingSides;
                var left = 0d;
                var top = 0d;
                var right = 0d;
                var bottom = 0d;

                if (systemPaddingSide.HasFlag(PaddingSide.Left))
                    left = systemPadding.Left;
                if (systemPaddingSide.HasFlag(PaddingSide.Top))
                    top = systemPadding.Top;
                if (systemPaddingSide.HasFlag(PaddingSide.Right))
                    right = systemPadding.Right;
                if (systemPaddingSide.HasFlag(PaddingSide.Bottom))
                    bottom = systemPadding.Bottom;

                x += left;
                y += top;
                width -= left + right;

                if (HasKeyboardOffset)
                    height -= top + Math.Max(bottom, KeyboardOffset);
                else
                    height -= top + bottom;
            }
            else if (HasKeyboardOffset)
            {
                height -= KeyboardOffset;
            }

            base.LayoutChildren(x, y, width, height);
        }

        #endregion

        #region Animation Methods

        internal void PreparingAnimation()
        {
            if (IsAnimationEnabled)
                Animation?.Preparing(Content, this);
        }

        internal void DisposingAnimation()
        {
            if (IsAnimationEnabled)
                Animation?.Disposing(Content, this);
        }

        internal async Task AppearingAnimation()
        {
            OnAppearingAnimationBegin();
            await OnAppearingAnimationBeginAsync();

            if (IsAnimationEnabled && Animation != null)
                await Animation.Appearing(Content, this);

            OnAppearingAnimationEnd();
            await OnAppearingAnimationEndAsync();
        }

        internal async Task DisappearingAnimation()
        {
            OnDisappearingAnimationBegin();
            await OnDisappearingAnimationBeginAsync();

            if (IsAnimationEnabled && Animation != null)
                await Animation.Disappearing(Content, this);

            OnDisappearingAnimationEnd();
            await OnDisappearingAnimationEndAsync();
        }

        #endregion

        #region Override Animation Methods
        /// <summary>
        /// Invoked at beginning of appearing animation
        /// </summary>
        protected virtual void OnAppearingAnimationBegin()
        {
        }

        /// <summary>
        /// Invoked at end of appearing animation
        /// </summary>
        protected virtual void OnAppearingAnimationEnd()
        {
        }

        /// <summary>
        /// Invoked at start of disappearing animation
        /// </summary>
        protected virtual void OnDisappearingAnimationBegin()
        {
        }

        /// <summary>
        /// Invoked at end of disappearing animation
        /// </summary>
        protected virtual void OnDisappearingAnimationEnd()
        {
        }

        /// <summary>
        /// Invoked at start on appearing animation
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnAppearingAnimationBeginAsync()
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// Invoked at end of appearing animation
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnAppearingAnimationEndAsync()
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// Invoked at start of disappearing animation
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnDisappearingAnimationBeginAsync()
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// Invoked at end of disappearing animation
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnDisappearingAnimationEndAsync()
        {
            return Task.FromResult(0);
        }

        #endregion

        #region Background Click
        /// <summary>
        /// Invoked when background is clicked
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnBackgroundClicked()
        {
            return CloseWhenBackgroundIsClicked;
        }

        #endregion

        #region Internal Methods

        internal async void SendBackgroundClick()
        {
            BackgroundClicked?.Invoke(this, EventArgs.Empty);

            var isClose = OnBackgroundClicked();
            if (isClose)
            {
                await PopupNavigation.Instance.RemovePageAsync(this);
            }
        }

        #endregion
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
