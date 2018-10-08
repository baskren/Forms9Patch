// /*******************************************************************
//  *
//  * PopupPage.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
using System.Collections.Generic;
//using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
//using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Forms9Patch
{
    [Obsolete("Forms9Patch.RootPage does not work with iOS Modal Pages.  User Forms9Patch.PopupPage instead.", false)]
    public class RootPage : PopupPage
    {
        #region Properties
        //protected override IPageController PageController => (_modals.Count > 0 ? _modals[_modals.Count - 1] : this) as IPageController;
        #endregion


        #region Fields
        //static internal RootPage _instance;
        #endregion


        #region Constructor
        [Obsolete("Forms9Patch.RootPage does not work with iOS Modal Pages.  User Forms9Patch.PopupPage instead.", false)]
        public RootPage(Page page) : base(page) { }

        /// <summary>
        /// Create the specified page.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="page">Page.</param>
        [Obsolete("Forms9Patch.RootPage does not work with iOS Modal Pages.  User Forms9Patch.PopupPage instead.", false)]
        public static new RootPage Create(Page page)
        {
            //_instance = _instance ?? new RootPage(page);
            var _instance = new RootPage(page);
            return _instance;
        }
        #endregion



    }

    /// <summary>
    /// Page that supports popups
    /// </summary>
    public class PopupPage : Xamarin.Forms.Page
    {

        #region Events
        /// <summary>
        /// Occurs when modal popped.
        /// </summary>
        public static event EventHandler<Page> ModalPopped;
        /// <summary>
        /// Occurs when modal popping.
        /// </summary>
        public static event EventHandler<Page> ModalPopping;
        /// <summary>
        /// Occurs when modal pushed.
        /// </summary>
        public static event EventHandler<Page> ModalPushed;
        /// <summary>
        /// Occurs when modal pushing.
        /// </summary>
        public static event EventHandler<Page> ModalPushing;
        /// <summary>
        /// Occurs when navigation popped.
        /// </summary>
        public static event EventHandler<Page> NavigationPopped;
        /// <summary>
        /// Occurs when navigation pushed.
        /// </summary>
        public static event EventHandler<Page> NavigationPushed;


        public static event EventHandler StatusBarPaddingChanged;
        #endregion

        #region Properties
        protected virtual IPageController PageController => this as IPageController;
        //static readonly List<Page> _modals = new List<Page>();
        #endregion


        #region Fields
        static internal NavigationPage _navPage;
        #endregion


        #region Constructor
        static PopupPage()
        {
            Settings.ConfirmInitialization();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.RootPage"/> class.
        /// </summary>
        /// <param name="page">Page.</param>
        public PopupPage(Page page = null)
        {

            // needed to comment out the following because Android was repeating the MainAcitivty.OnCreate during a resume.
            //if (_instance != null)
            //    throw new Exception("A second instance of RootPage is not allowed.  Try using RootPage.Create(Page page) instead");
            //_instance = this;
            Page = page;
            Application.Current.ModalPopping += OnModalPopping;
            Application.Current.ModalPushing += OnModalPushing;
            Application.Current.ModalPushed += OnModalPushed;
            Application.Current.ModalPopped += OnModalPopped;
        }

        /// <summary>
        /// Create the specified page.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="page">Page.</param>
        public static PopupPage Create(Page page)
        {
            //_instance = _instance ?? new RootPage(page);
            var _instance = new PopupPage(page);
            return _instance;
        }

        #endregion


        #region Static Methods


        #endregion


        #region Navigation Event Handlers
        void OnNavigationPagePushed(object sender, NavigationEventArgs e)
        {
            RemovePopups(false);
            NavigationPushed?.Invoke(sender, e.Page);
        }

        void OnNavigationPagePopped(object sender, NavigationEventArgs e)
        {
            RemovePopups(true);
            NavigationPopped?.Invoke(sender, e.Page);
        }

        void OnModalPushing(object sender, ModalPushingEventArgs e)
        {
            ModalPushing?.Invoke(sender, e.Modal);
            RemovePopups(false);
        }

        void OnModalPopping(object sender, ModalPoppingEventArgs e)
        {
            RemovePopups(true);
            ModalPopping?.Invoke(sender, e.Modal);
        }

        void OnModalPushed(object sender, ModalPushedEventArgs e)
        {
            ModalPushed?.Invoke(sender, e.Modal);
            //_modals.Add(e.Modal);
        }


        void OnModalPopped(object sender, ModalPoppedEventArgs e)
        {
            ModalPopped?.Invoke(sender, e.Modal);
            //_modals.Remove(e.Modal);
            //var current = _modals.Count > 0 ? _modals[_modals.Count - 1] : null;
            //current = current ?? (_navPage.Navigation.NavigationStack.Count > 0 ? _navPage.Navigation.NavigationStack[_navPage.Navigation.NavigationStack.Count - 1] : null);
            var current = PageExtensions.FindCurrentPage(Application.Current.MainPage);
            if (current is HardwareKeyPage hkPage)
                hkPage.OnReappearing();
        }
        #endregion


        #region Page Property
        /// <summary>
        /// Gets or sets the apps content page.
        /// </summary>
        /// <value>The page.</value>
        public Page Page
        {
            get
            {
                var result = PageController.InternalChildren[0] as Page;
                return result;
            }
            set
            {
                //_instance = _instance ?? new RootPage();

                //NavigationPage navPage;
                if (PageController.InternalChildren.Count > 0)
                {
                    var oldPage = PageController.InternalChildren[0] as Page;
                    _navPage = PageController.InternalChildren[0] as NavigationPage;
                    if (_navPage != null)
                    {
                        _navPage.Pushed -= OnNavigationPagePushed;
                        _navPage.Popped -= OnNavigationPagePopped;
                        _navPage.PoppedToRoot -= OnNavigationPagePopped;
                    }
                    if (oldPage != null)
                        PageController.InternalChildren.Remove(oldPage);
                }

                if (value != null)
                {
                    PageController.InternalChildren.Insert(0, value);
                    _navPage = PageController.InternalChildren[0] as NavigationPage;
                    if (_navPage != null)
                    {
                        _navPage.Pushed += OnNavigationPagePushed;
                        _navPage.Popped += OnNavigationPagePopped;
                        _navPage.PoppedToRoot += OnNavigationPagePopped;
                    }
                    var masterDetailPage = PageController.InternalChildren[0] as MasterDetailPage;
                    if (masterDetailPage != null)
                    {
                        masterDetailPage.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
                        {
                            if (e.PropertyName == "Renderer")
                                System.Diagnostics.Debug.WriteLine(masterDetailPage);
                        };
                    }
                }
            }
        }

        #endregion



        #region Popup stack
        internal void AddPopup(PopupBase popup)
        {
            //Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(popup, Bounds);
            popup.ManualLayout(Bounds);
            if (!PageController.InternalChildren.Contains(popup))
            {
                PageController.InternalChildren.Add(popup);
                popup.PresentedAt = DateTime.Now;
                popup.MeasureInvalidated += OnChildMeasureInvalidated;
            }
        }

        internal void RemovePopup(PopupBase popup)
        {
            if (popup.Retain)
                return;
            /* seems to be causing some problems (race condition?) with presenting a
			while (PageController.InternalChildren.Contains(popup))
			{
				PageController.InternalChildren.Remove(PageController.InternalChildren.Last());
				//Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(popup, new Rectangle(0, 0, -1, -1));
			}
			*/
            popup.MeasureInvalidated -= OnChildMeasureInvalidated;

            if (PageController.InternalChildren.Contains(popup))
                PageController.InternalChildren.Remove(popup);
        }

        internal void RemovePopups(bool popping)
        {
            for (int i = PageController.InternalChildren.Count - 1; i > 0; i--)
            {
                var popup = PageController.InternalChildren[i] as PopupBase;
                if (popup != null && (popup.PresentedAt.AddSeconds(2) < DateTime.Now || popping))
                {
                    popup.MeasureInvalidated += OnChildMeasureInvalidated;
                    PageController.InternalChildren.Remove(popup);
                }
            }
        }
        #endregion


        #region Android back button
        /// <summary>
        /// Ons the back button pressed.
        /// </summary>
        /// <returns><c>true</c>, if back button pressed was oned, <c>false</c> otherwise.</returns>
        protected override bool OnBackButtonPressed()
        {
            if (PageController.InternalChildren.Count > 1)
            {
                var lastChild = PageController.InternalChildren[PageController.InternalChildren.Count - 1];
                if (lastChild is PopupBase popup)
                    popup.Cancel();
                else
                    PageController.InternalChildren.Remove(lastChild);
                return true;
            }
            var masterDetailPage = (PageController.InternalChildren[0] as MasterDetailPage) ?? (PageController.InternalChildren[0] as NavigationPage)?.CurrentPage as MasterDetailPage;
            if (Device.RuntimePlatform != Device.UWP && masterDetailPage != null && masterDetailPage.IsPresented)
            {
                masterDetailPage.IsPresented = false;
                return true;
            }
            // check if primary child is NavigationPage.  If so, pass the back button press to it.
            if (PageController.InternalChildren[0] is Page page)
            {
                var handled = page.SendBackButtonPressed();
                if (handled)
                    return true;
            }
            return base.OnBackButtonPressed();
        }
        #endregion


        #region Layout
        bool _ignoreChildren;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.StackLayout"/> will not invalidate itself when a child changes.
        /// </summary>
        /// <value><c>true</c> if ignore children; otherwise, <c>false</c>.</value>
        public bool IgnoreChildren
        {
            get
            {
                return _ignoreChildren;
            }
            set
            {
                if (_ignoreChildren != value)
                {
                    _ignoreChildren = value;
                    if (_ignoreChildren)
                        foreach (var child in PageController.InternalChildren)
                        {
                            var view = child as View;
                            if (view != null)
                                view.MeasureInvalidated -= OnChildMeasureInvalidated;
                        }
                    else
                        foreach (var child in PageController.InternalChildren)
                        {
                            var view = child as View;
                            if (view != null)
                                view.MeasureInvalidated += OnChildMeasureInvalidated;
                        }
                }
            }
        }

        static double _startingHeight = -1;
        static Thickness _oldStatusBarPadding = new Thickness();

        /// <summary>
        /// Layouts the children.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (_startingHeight < 0)
                _startingHeight = StatusBarService.Height;
            //var newStatusBarPadding = StatusBarPadding;
            var platformConfiguration = On<Xamarin.Forms.PlatformConfiguration.iOS>();//.SafeAreaInsets();
            var safeAreaInsets = Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SafeAreaInsets(platformConfiguration);
            if (safeAreaInsets != _oldStatusBarPadding)
            {
                _oldStatusBarPadding = safeAreaInsets;
                StatusBarPaddingChanged?.Invoke(this, EventArgs.Empty);
            }
            base.LayoutChildren(x, y, width, height);
        }
        #endregion

        /*
        #region iPhone X
        /// <summary>
        /// Returns the amount of padding you'll need to keep your views from overlapping the status bar.
        /// </summary>
        /// <value>The status bar padding.</value>
        public static double StatusBarPadding
        {
            get
            {
                if (_instance == null)
                    return 0;
                var pageController = _instance.PageController.InternalChildren[0] as IPageController;
                var ignoresContainerArea = pageController == null || pageController.IgnoresContainerArea;
                double verticalY = 0;
                //if (Device.OS == TargetPlatform.iOS && !ignoresContainerArea)
                if (Device.RuntimePlatform == Device.iOS && !ignoresContainerArea)
                {
                    verticalY = 20;
                    if (Device.Idiom == TargetIdiom.Phone)
                        verticalY = 20 - Math.Max(20, _startingHeight) + Math.Min(20, StatusBarService.Height);
                }
                return verticalY;
            }
        }

        /// <summary>
        /// Occurs when status bar padding changed.
        /// </summary>
        #endregion
        */

    }
}
