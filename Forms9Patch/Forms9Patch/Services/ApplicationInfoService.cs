// /*******************************************************************
//  *
//  * PackageInfoService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;

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
                _service = _service ?? DependencyService.Get<IApplicationInfoService>();
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

        static PCL.Utils.AsyncAwaitForSet<bool> _asyncAwaitForMainPageSet;
        public static async Task WaitForMainPage()
        {
            if (Application.Current.MainPage != null)
                return;
            _asyncAwaitForMainPageSet = new PCL.Utils.AsyncAwaitForSet<bool>(null, null);
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
