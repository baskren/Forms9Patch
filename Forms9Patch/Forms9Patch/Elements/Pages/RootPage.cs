// /*******************************************************************
//  *
//  * PopupPage.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Forms9Patch
{
    /// <summary>
    /// Page that supports popups
    /// </summary>
    public class RootPage : Page
    {
        static RootPage _instance;

        public static event EventHandler<Page> ModalPopped;
        public static event EventHandler<Page> ModalPopping;
        public static event EventHandler<Page> ModalPushed;
        public static event EventHandler<Page> ModalPushing;
        public static event EventHandler<Page> NavigationPopped;
        public static event EventHandler<Page> NavigationPushed;

        static List<Page> _modals = new List<Page>();
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.RootPage"/> class.
        /// </summary>
        /// <param name="page">Page.</param>
        public RootPage(Page page = null)
        {
            if (_instance != null)
                throw new Exception("A second instance of RootPage is not allowed.  Try using RootPage.Create(Page page) instead");
            _instance = this;
            Page = page;
            Application.Current.ModalPopping += (object sender, ModalPoppingEventArgs e) =>
            {
                ModalPopping?.Invoke(sender, e.Modal);
                System.Diagnostics.Debug.WriteLine("ModalPopping");
                RemovePopups(true);
            };
            Application.Current.ModalPushing += (object sender, ModalPushingEventArgs e) =>
            {
                ModalPushing?.Invoke(sender, e.Modal);
                System.Diagnostics.Debug.WriteLine("ModalPusing");
            };
            Application.Current.ModalPushed += (object sender, ModalPushedEventArgs e) =>
            {
                ModalPushed?.Invoke(sender, e.Modal);
                System.Diagnostics.Debug.WriteLine("Modal Pushed");
                _modals.Add(e.Modal);
            };
            Application.Current.ModalPopped += (object sender, ModalPoppedEventArgs e) =>
            {
                ModalPopped?.Invoke(sender, e.Modal);
                System.Diagnostics.Debug.WriteLine("ModalPopped");
                _modals.Remove(e.Modal);
            };

        }

        /// <summary>
        /// Create the specified page.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="page">Page.</param>
        public static RootPage Create(Page page)
        {
            _instance = _instance ?? new RootPage(page);
            return _instance;
        }

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
                NavigationPage navPage;
                if (_instance.PageController.InternalChildren.Count() > 0)
                {
                    var page = _instance.PageController.InternalChildren[0] as Page;
                    navPage = _instance.PageController.InternalChildren[0] as NavigationPage;
                    if (navPage != null)
                    {
                        navPage.Popped -= OnNavigationPagePopped;
                        navPage.Pushed -= OnNavigationPagePushed;
                        navPage.PoppedToRoot -= OnNavigationPagePopped;
                    }
                    if (page != null)
                        _instance.PageController.InternalChildren.Remove(page);
                }
                _instance.PageController.InternalChildren.Insert(0, value);
                navPage = value as NavigationPage;
                if (navPage != null)
                {
                    navPage.Popped += OnNavigationPagePopped;
                    navPage.Pushed += OnNavigationPagePushed;
                    navPage.PoppedToRoot += OnNavigationPagePopped;
                }
            }
        }

        static void OnNavigationPagePopped(object sender, NavigationEventArgs e)
        {
            _instance?.RemovePopups(true);
            NavigationPopped?.Invoke(sender, e.Page);
        }

        static void OnNavigationPagePushed(object sender, NavigationEventArgs e)
        {
            _instance?.RemovePopups(false);
            NavigationPushed?.Invoke(sender, e.Page);
        }

        IPageController PageController => (_modals.Count > 0 ? _modals.Last() : this) as IPageController;

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
            for (int i = PageController.InternalChildren.Count() - 1; i > 0; i--)
            {
                var popup = PageController.InternalChildren[i] as PopupBase;
                if (popup != null && (popup.PresentedAt.AddSeconds(2) < DateTime.Now || popping))
                    PageController.InternalChildren.RemoveAt(i);
            }
        }

        /// <summary>
        /// Ons the back button pressed.
        /// </summary>
        /// <returns><c>true</c>, if back button pressed was oned, <c>false</c> otherwise.</returns>
        protected override bool OnBackButtonPressed()
        {
            if (PageController.InternalChildren.Count() > 1)
            {
                PageController.InternalChildren.Remove(PageController.InternalChildren.Last());
                return true;
            }
            var masterDetailPage = (PageController.InternalChildren[0] as MasterDetailPage) ?? (PageController.InternalChildren[0] as NavigationPage)?.CurrentPage as MasterDetailPage;
            if (masterDetailPage != null && masterDetailPage.IsPresented)
            {
                masterDetailPage.IsPresented = false;
                return true;
            }
            // check if primary child is NavigationPage.  If so, pass the back button press to it.
            var page = PageController.InternalChildren[0] as Page;
            if (page != null)
            {
                var handled = page.SendBackButtonPressed();
                if (handled)
                    return true;
            }
            return base.OnBackButtonPressed();
        }

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
                var ignoresContainerArea = pageController != null ? pageController.IgnoresContainerArea : true;
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
            //System.Diagnostics.Debug.WriteLine("_startingHeight=["+_startingHeight+"]  StatusBar.Visible=["+StatusBarService.IsVisible+"] StatusBar.Height=["+StatusBarService.Height+"]");
            base.LayoutChildren(x, y, width, height);
        }
    }
}
