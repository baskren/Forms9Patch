// /*******************************************************************
//  *
//  * PackageInfoService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
using System.Reflection;
using System.Threading.Tasks;

namespace Forms9Patch
{
    /// <summary>
    /// Application infomation service.
    /// </summary>
    public static class ApplicationInfoService
    {
        static IApplicationInfoService _service;
        static IApplicationInfoService Service
        {
            get
            {
                Settings.ConfirmInitialization();
                _service = _service ?? DependencyService.Get<IApplicationInfoService>();
                if (_service == null)
                {
                    System.Diagnostics.Debug.WriteLine("ApplicationInfoService is not available");
                    throw new ServiceNotAvailableException("ApplicationInfoService not available");
                }
                return _service;
            }
        }

        /// <summary>
        /// Gets the version string.
        /// </summary>
        /// <value>The version.</value>
        public static string Version { get { return Service?.Version; } }

        /// <summary>
        /// Gets the application's build number.
        /// </summary>
        /// <value>The build.</value>
        public static int Build
        {
            get
            {
                if (Service != null)
                    return Service.Build;
                return -1;
            }
        }

        /// <summary>
        /// Gets the bundle or package identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public static string Identifier { get { return Service?.Identifier; } }

        /// <summary>
        /// Gets the applications display name.
        /// </summary>
        /// <value>The name.</value>
        public static string Name { get { return Service?.Name; } }

        /// <summary>
        /// Gets the fingerprint (valid on Android only).
        /// </summary>
        /// <value>The fingerprint.</value>
        public static string Fingerprint { get { return Service?.Fingerprint; } }

        /// <summary>
        /// Gets the application's assembly.
        /// </summary>
        /// <value>The assembly.</value>
        public static Assembly Assembly
        {
            get
            {
                return Application.Current.GetType().GetTypeInfo().Assembly;
            }
        }

        /// <summary>
        /// Gets the currently displayed page.
        /// </summary>
        /// <value>The current page.</value>
        public static Page CurrentPage
        {
            get
            {
                if (Application.Current.MainPage == null)
                    return null;
                return NavigationPage?.CurrentPage ?? Application.Current.MainPage;
            }
        }

        /// <summary>
        /// Gets the current state of network connectivity.
        /// </summary>
        /// <value>The network connectivity.</value>
        public static NetworkConnectivity NetworkConnectivity
        {
            get
            {
                if (Service == null)
                    return NetworkConnectivity.None;
                return Service.NetworkConnectivity;
            }
        }

        /// <summary>
        /// Gets the application's navigation page.
        /// </summary>
        /// <value>The navigation page.</value>
        public static NavigationPage NavigationPage
        {
            get
            {
                if (Application.Current.MainPage == null)
                    return null;
                NavigationPage navPage = Application.Current.MainPage as NavigationPage;
                if (navPage == null)
                {
                    IPageController rootController = Application.Current.MainPage;
                    foreach (var child in rootController.InternalChildren)
                    {
                        navPage = child as NavigationPage;
                        if (navPage != null)
                            break;
                    }
                }
                return navPage;
            }
        }

        static P42.Utils.AsyncAwaitForSet<bool> _asyncAwaitForMainPageSet;
        /// <summary>
        /// Waits for Xamarin.Forms.Application.MainPAge to be set.
        /// </summary>
        /// <returns>The for main page.</returns>
        public static async Task WaitForMainPage()
        {
            if (Application.Current.MainPage != null)
                return;
            _asyncAwaitForMainPageSet = new P42.Utils.AsyncAwaitForSet<bool>(null, null);
            Application.Current.PropertyChanged += OnApplicationPropertyChanged;
            await _asyncAwaitForMainPageSet.Result();
            Application.Current.PropertyChanged -= OnApplicationPropertyChanged;
            _asyncAwaitForMainPageSet = null;
        }

        static void OnApplicationPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MainPage" && Application.Current.MainPage != null)
                _asyncAwaitForMainPageSet?.Set(true);
        }

        /// <summary>
        /// Is the page in the current navigation stacks.
        /// </summary>
        /// <returns><c>true</c>, if page in navigation stacks was ised, <c>false</c> otherwise.</returns>
        /// <param name="page">Page.</param>
        public static bool IsPageInNavigationStacks(Page page)
        {
            if (page == CurrentPage)
                return true;
            //var navPage = NavigationPage as IPageController;
            foreach (var p in CurrentPage.Navigation.NavigationStack)
                if (p == page)
                    return true;
            foreach (var p in CurrentPage.Navigation.ModalStack)
                if (p == page)
                    return true;
            return false;
        }

    }
}
