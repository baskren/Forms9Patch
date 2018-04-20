// /*******************************************************************
//  *
//  * PopupPage.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Forms9Patch
{
    /// <summary>
    /// Page that supports popups
    /// </summary>
    public class RootPage : Page
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
        #endregion


        #region Fields
        static internal RootPage _instance;
        static List<Page> _modals = new List<Page>();
        static internal NavigationPage _navPage;
        #endregion


        #region Constructor
        static RootPage()
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.RootPage"/> class.
        /// </summary>
        /// <param name="page">Page.</param>
        public RootPage(Page page = null)
        {

            // needed to comment out the following because Android was repeating the MainAcitivty.OnCreate during a resume.
            //if (_instance != null)
            //    throw new Exception("A second instance of RootPage is not allowed.  Try using RootPage.Create(Page page) instead");
            _instance = this;
            Page = page;
            Application.Current.ModalPopping += OnModalPopping;
            Application.Current.ModalPushing += OnModalPushing;
            Application.Current.ModalPushed += OnModalPushed;
            Application.Current.ModalPopped += OnModalPopped;

            /*
            Application.Current.ChildRemoved += (s, e) =>
                {
                    System.Diagnostics.Debug.WriteLine("ChildRemoved");
                };

            Application.Current.DescendantRemoved += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("ChildAdded");
            };
            */

            //HardwareKeyHandlerEffect.ApplyTo(this);
        }
        #endregion


        #region Factory
        /// <summary>
        /// Create the specified page.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="page">Page.</param>
        public static RootPage Create(Page page)
        {
            //_instance = _instance ?? new RootPage(page);
            _instance = new RootPage(page);
            return _instance;
        }
        #endregion


        #region Page Property
        /// <summary>
        /// Gets or sets the apps content page.
        /// </summary>
        /// <value>The page.</value>
        public static Page Page
        {
            get
            {
                var result = _instance.PageController.InternalChildren[0] as Page;
                return result;
            }
            set
            {
                _instance = _instance ?? new RootPage();

                //NavigationPage navPage;
                if (_instance.PageController.InternalChildren.Count > 0)
                {
                    var oldPage = _instance.PageController.InternalChildren[0] as Page;
                    _navPage = _instance.PageController.InternalChildren[0] as NavigationPage;
                    if (_navPage != null)
                    {
                        _navPage.Pushed -= OnNavigationPagePushed;
                        _navPage.Popped -= OnNavigationPagePopped;
                        _navPage.PoppedToRoot -= OnNavigationPagePopped;
                    }
                    if (oldPage != null)
                        _instance.PageController.InternalChildren.Remove(oldPage);
                }

                if (value != null)
                {
                    _instance.PageController.InternalChildren.Insert(0, value);
                    _navPage = _instance.PageController.InternalChildren[0] as NavigationPage;
                    if (_navPage != null)
                    {
                        _navPage.Pushed += OnNavigationPagePushed;
                        _navPage.Popped += OnNavigationPagePopped;
                        _navPage.PoppedToRoot += OnNavigationPagePopped;
                    }
                    var masterDetailPage = _instance.PageController.InternalChildren[0] as MasterDetailPage;
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


        #region Navigation Event Handlers



        static void OnNavigationPagePushed(object sender, NavigationEventArgs e)
        {
            _instance?.RemovePopups(false);
            NavigationPushed?.Invoke(sender, e.Page);
        }

        static void OnNavigationPagePopped(object sender, NavigationEventArgs e)
        {
            _instance?.RemovePopups(true);
            NavigationPopped?.Invoke(sender, e.Page);
        }

        void OnModalPushing(object sender, ModalPushingEventArgs e)
        {
            ModalPushing?.Invoke(sender, e.Modal);
            //System.Diagnostics.Debug.WriteLine("ModalPushing");
        }

        void OnModalPopping(object sender, ModalPoppingEventArgs e)
        {
            ModalPopping?.Invoke(sender, e.Modal);
            //System.Diagnostics.Debug.WriteLine("ModalPopping");
            RemovePopups(true);
        }

        void OnModalPushed(object sender, ModalPushedEventArgs e)
        {
            ModalPushed?.Invoke(sender, e.Modal);
            _modals.Add(e.Modal);
            System.Diagnostics.Debug.WriteLine("Modal Pushed");
        }

        void OnModalPopped(object sender, ModalPoppedEventArgs e)
        {
            ModalPopped?.Invoke(sender, e.Modal);
            _modals.Remove(e.Modal);
            System.Diagnostics.Debug.WriteLine("ModalPopped");
            var current = _modals.Count > 0 ? _modals[_modals.Count - 1] : null;
            current = current ?? (_navPage.Navigation.NavigationStack.Count > 0 ? _navPage.Navigation.NavigationStack[_navPage.Navigation.NavigationStack.Count - 1] : null);
            if (current is HardwareKeyPage hkPage)
                hkPage.OnReappearing();
        }
        #endregion


        #region Popup stack
        IPageController PageController => (_modals.Count > 0 ? _modals[_modals.Count - 1] : this) as IPageController;

        internal void AddPopup(PopupBase popup)
        {
            //Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(popup, Bounds);
            popup.ManualLayout(Bounds);
            if (!PageController.InternalChildren.Contains(popup))
                PageController.InternalChildren.Add(popup);
            popup.PresentedAt = DateTime.Now;
            popup.MeasureInvalidated -= OnChildMeasureInvalidated;
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
            if (PageController.InternalChildren.Contains(popup))
                PageController.InternalChildren.Remove(popup);
        }

        internal void RemovePopups(bool popping)
        {
            for (int i = PageController.InternalChildren.Count - 1; i > 0; i--)
            {
                var popup = PageController.InternalChildren[i] as PopupBase;
                if (popup != null && (popup.PresentedAt.AddSeconds(2) < DateTime.Now || popping))
                    PageController.InternalChildren.RemoveAt(i);
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
        static bool _ignoreChildren;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.StackLayout"/> will not invalidate itself when a child changes.
        /// </summary>
        /// <value><c>true</c> if ignore children; otherwise, <c>false</c>.</value>
        public static bool IgnoreChildren
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
                        foreach (var child in _instance.PageController.InternalChildren)
                        {
                            var view = child as View;
                            if (view != null)
                                view.MeasureInvalidated -= _instance.OnChildMeasureInvalidated;
                        }
                    else
                        foreach (var child in _instance.PageController.InternalChildren)
                        {
                            var view = child as View;
                            if (view != null)
                                view.MeasureInvalidated += _instance.OnChildMeasureInvalidated;
                        }
                }
            }
        }

        static double _startingHeight = -1;
        static double _oldStatusBarPadding = -1;

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
            var newStatusBarPadding = StatusBarPadding;
            if (Math.Abs(newStatusBarPadding - _oldStatusBarPadding) > 0.1)
            {
                _oldStatusBarPadding = newStatusBarPadding;
                StatusBarPaddingChanged?.Invoke(this, EventArgs.Empty);
            }
            base.LayoutChildren(x, y, width, height);
        }
        #endregion


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
        public static event EventHandler StatusBarPaddingChanged;
        #endregion

    }
}
