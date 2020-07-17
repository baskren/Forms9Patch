using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.UWP.ApplicationInfoService))]
namespace Forms9Patch.UWP
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ApplicationInfoService : IApplicationInfoService
    {
        public string Version => Package.Current.Id.Version.Major + "." + Package.Current.Id.Version.Minor; // + "." + Package.Current.Id.Version.Revision;

        public int Build => Package.Current.Id.Version.Build;

        public string Identifier => Package.Current.Id.ResourceId;

        public string Name => Package.Current.DisplayName;

        public string Fingerprint => throw new NotImplementedException();

/*
        public NetworkConnectivity NetworkConnectivity
        {
            get
            {
                var temp = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();
                if (temp == null)
                    return NetworkConnectivity.None;
                if (temp.IsWwanConnectionProfile)
                    return NetworkConnectivity.Mobile;
                return NetworkConnectivity.LAN;
            }
        }
*/

    }
}
