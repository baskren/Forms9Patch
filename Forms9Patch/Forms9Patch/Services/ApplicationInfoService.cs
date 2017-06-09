// /*******************************************************************
//  *
//  * PackageInfoService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;

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

        public static string Fingerprint { get { return Service?.Fingerprint; } }
    }
}
