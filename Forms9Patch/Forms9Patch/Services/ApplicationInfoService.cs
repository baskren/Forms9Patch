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
        static ApplicationInfoService()
            => Settings.ConfirmInitialization();


        static IApplicationInfoService _service;
        static IApplicationInfoService Service
        {
            get
            {
                _service = _service ?? DependencyService.Get<IApplicationInfoService>();
                if (_service == null)
                {
                    //System.Diagnostics.Debug.WriteLine("ApplicationInfoService is not available");
                    throw new ServiceNotAvailableException("ApplicationInfoService not available");
                }
                return _service;
            }
        }

        /// <summary>
        /// Gets the version string.
        /// </summary>
        /// <value>The version.</value>
        public static string Version => Service?.Version;

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
        public static string Identifier => Service?.Identifier;

        /// <summary>
        /// Gets the applications display name.
        /// </summary>
        /// <value>The name.</value>
        public static string Name => Service?.Name;

        /// <summary>
        /// Gets the fingerprint (valid on Android only).
        /// </summary>
        /// <value>The fingerprint.</value>
        public static string Fingerprint => Service?.Fingerprint;

        /// <summary>
        /// Gets the application's assembly.
        /// </summary>
        /// <value>The assembly.</value>
        public static Assembly Assembly => Application.Current?.GetType().GetTypeInfo().Assembly;

        /// <summary>
        /// Gets the currently displayed page.
        /// </summary>
        /// <value>The current page.</value>
        public static Page CurrentPage => Application.Current?.MainPage == null ? null : NavigationPage?.CurrentPage ?? Application.Current.MainPage;

        /*
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
        */

        /// <summary>
        /// Gets the application's navigation page.
        /// </summary>
        /// <value>The navigation page.</value>
        public static NavigationPage NavigationPage
        {
            get
            {
                if (Application.Current?.MainPage is NavigationPage navPage)
                    return navPage;
                if (Application.Current?.MainPage is IPageController rootController)
                    foreach (var child in rootController.InternalChildren)
                        if (child is NavigationPage result)
                            return result;
                return null;
            }
        }


        static P42.Utils.AsyncAwaitForSet<bool> _asyncAwaitForMainPageSet;
        /// <summary>
        /// Waits for Xamarin.Forms.Application.MainPAge to be set.
        /// </summary>
        /// <returns>The for main page.</returns>
        public static async Task WaitForMainPage()
        {
            if (Application.Current?.MainPage != null)
                return;

            while (Application.Current == null)
            {
                await Task.Delay(50);
            }
            _asyncAwaitForMainPageSet = new P42.Utils.AsyncAwaitForSet<bool>(null, null);
            Application.Current.PropertyChanged += OnApplicationPropertyChanged;
            await _asyncAwaitForMainPageSet.Result();
            Application.Current.PropertyChanged -= OnApplicationPropertyChanged;
            _asyncAwaitForMainPageSet = null;
        }


        static void OnApplicationPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Xamarin.Forms.Application.MainPage) && Application.Current?.MainPage != null)
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
