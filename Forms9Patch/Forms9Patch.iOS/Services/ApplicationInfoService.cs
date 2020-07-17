// /*******************************************************************
//  *
//  * ApplicationInfoService.cs copyright 2017 Ben Askren, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.iOS.ApplicationInfoService))]
namespace Forms9Patch.iOS
{
    /// <summary>
    /// Application info service.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ApplicationInfoService : IApplicationInfoService
    {
        /// <summary>
        /// Gets the build.
        /// </summary>
        /// <value>The build.</value>
        public int Build
        {
            get
            {
                var resultAsString = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
                if (int.TryParse(resultAsString, out int result))
                    return result;
                return -1;
            }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier
        {
            get
            {
                return NSBundle.MainBundle.BundleIdentifier;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return NSBundle.MainBundle.InfoDictionary["CFBundleDisplayName"]?.ToString() ?? NSBundle.MainBundle.InfoDictionary["CFBundleName"]?.ToString();
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version
        {
            get
            {
                return NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
            }
        }

        /// <summary>
        /// Gets the fingerprint.
        /// </summary>
        /// <value>The fingerprint.</value>
        public string Fingerprint
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /*
        /// <summary>
        /// Gets the network connectivity.
        /// </summary>
        /// <value>The network connectivity.</value>
        public NetworkConnectivity NetworkConnectivity => Reachability.InternetConnectionStatus().ToNetworkConnectivity();
        */
    }
}
