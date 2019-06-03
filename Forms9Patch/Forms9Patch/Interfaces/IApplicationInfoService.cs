// /*******************************************************************
//  *
//  * IPackageInfoService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

namespace Forms9Patch
{
    /// <summary>
    /// Application info service.
    /// </summary>
    public interface IApplicationInfoService
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        string Version { get; }

        /// <summary>
        /// Gets the build.
        /// </summary>
        /// <value>The build.</value>
        int Build { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string Identifier { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the fingerprint.
        /// </summary>
        /// <value>The fingerprint.</value>
        string Fingerprint { get; }

        /*
        /// <summary>
        /// Gets the current network connectivity.
        /// </summary>
        /// <value>The network availability.</value>
        NetworkConnectivity NetworkConnectivity { get; }
        */

    }
}
